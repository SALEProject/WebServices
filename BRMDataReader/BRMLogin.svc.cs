using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Activation;
using System.Text;
using System.Data;
using System.Reflection;
using Business;
using Business.Common;
using Business.DataModule;
using Business.Exceptions;
using Business.JSONObjects;
using System.Web.Script.Serialization;

namespace BRMDataReader
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BRMLogin" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BRMLogin.svc or BRMLogin.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class BRMLogin : IBRMLogin
    {
        public Stream GET_DefaultEndPoint() { return POST_DefaultEndPoint(); }

        public Stream POST_DefaultEndPoint()
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return new JSONResult(JSONErrorCode.WrongMethodCall, "").GetJSONResponseAsStream();
        }

        public Stream GET_ErrorCodes() { return POST_ErrorCodes(); }

        public Stream POST_ErrorCodes()
        {
            var s = Enum.GetNames(typeof(JSONErrorCode));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return new JSONResult(s).GetJSONResponseAsStream();
        }

        public Stream GET_Capabilities() { return POST_Capabilities(); }

        public Stream POST_Capabilities()
        {
            MethodInfo[] methodInfos = Type.GetType("BRMDataReader.BRMLogin").GetMethods(BindingFlags.Public | BindingFlags.Instance);
            if (methodInfos.Length == 0) return new JSONResult(new JSONMethod[0]).GetJSONResponseAsStream();

            int l = 0;
            foreach (MethodInfo info in methodInfos)
                if (info.Name != "ToString" && info.Name != "Equals" && info.Name != "GetHashCode" && info.Name != "GetType")
                    if (!info.Name.StartsWith("GET_")) l++;

            JSONMethod[] jsonmethods = new JSONMethod[l];
            int k = 0;
            foreach (MethodInfo info in methodInfos)
                if (info.Name != "ToString" && info.Name != "Equals" && info.Name != "GetHashCode" && info.Name != "GetType")
                {
                    string str_MethodName = info.Name.ToLower();
                    if (str_MethodName.StartsWith("get_")) continue; //  this will skip the GET call and report the follow-up POST call
                    if (str_MethodName.StartsWith("post_")) str_MethodName = str_MethodName.Remove(0, 5);

                    string str_CallFormat = "/" + str_MethodName;
                    ParameterInfo[] mparams = info.GetParameters();

                    string[] str_MethodArguments = new string[mparams.Length];
                    for (int i = 0; i < mparams.Length; i++)
                    {
                        str_MethodArguments[i] = mparams[i].Name.ToLower();
                        str_CallFormat += "/{" + str_MethodArguments[i] + "}";
                    }

                    jsonmethods[k] = new JSONMethod() { MethodName = str_MethodName, MethodArguments = str_MethodArguments, CallFormat = str_CallFormat };
                    k++;
                }

            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return new JSONResult(jsonmethods).GetJSONResponseAsStream();
        }

        public string GET_Echo() { return POST_Echo(); }

        public string POST_Echo()
        {
            // Get the raw json POST content.  .Net has this in XML string..
            string JSONstring = OperationContext.Current.RequestContext.RequestMessage.ToString();

            return JSONstring;
        }

        public Stream GET_ServerTime() { return POST_ServerTime(); }

        public Stream POST_ServerTime() { return new JSONResult(DateTime.Now).GetJSONResponseAsStream(); }

        public Stream GET_Login() { return POST_Login(); }

        public Stream POST_Login()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode(); 
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();        
                //if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                
                session.Init(json_request, app);
                
                //--------------------------------------------------------------------------------------------
                //  process json request
                JSONLogin login = json_request.GetLogin();
                if (login == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

                //  check for SQL injection
                if (login.LoginPassword.Contains('=') || login.LoginPassword.Contains('(') || login.LoginPassword.Contains(')'))
                    return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();

                if (login.EntryPoint.Trim() == "" && session.EntryPoint.Trim() == "") return new JSONResult(new JSONLoginResult(false, "entry point was not specified", null)).GetJSONResponseAsStream();
                if (login.EntryPoint.Trim() == "") login.EntryPoint = session.EntryPoint;

                //  find the requested database and make the appropriate connection
                TVariantList vl_params = null;
                DataSet ds = null;
                if (login.EntryPoint != session.EntryPoint)
                {
                    if (app.DB != null)
                    {
                        app.DB.Disconnect();
                        app.DestroyDBs(true);
                    }

                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Code").AsString = login.EntryPoint;
                    ds = app.SessionDB.Select("select_BursarybyCode", "Bursaries", vl_params);
                    if (!app.SessionDB.ValidDSRows(ds)) return new JSONResult(new JSONLoginResult(false, "invalid entry point", null)).GetJSONResponseAsStream();
                    app.Database = ds.Tables[0].Rows[0]["Database"].ToString();

                    if (!app.InitDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                }
                
                vl_params = new TVariantList();
                vl_params.Add("@prm_LoginName").AsString = login.LoginName;
                vl_params.Add("@prm_LoginPassword").AsString = login.LoginPassword;
                vl_params.Add("@prm_EntryPoint").AsString = login.EntryPoint;
                ds = app.DB.Select("CheckLogin", "Users", vl_params);
                if (ds == null) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (ds.Tables.Count == 0) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

                if (ds.Tables[0].Rows.Count == 0)
                {
                    return new JSONResult(new JSONLoginResult(false, "login failed", null)).GetJSONResponseAsStream();
                }
                else
                {
                    session.ID_User = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);

                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_User").AsInt32 = session.ID_User;
                    ds = app.DB.Select("select_CurrentUser", "Users", vl_params);
                    DataRow row = null;
                    if (app.DB.ValidDSRows(ds)) row = ds.Tables[0].Rows[0];

                    if (login.EntryPoint.Trim() != "") session.EntryPoint = login.EntryPoint;                    

                    //  set ID_UserRole in session
                    session.ID_UserRole = 0;
                    if (login.ID_UserRole != 0)
                    {
                        vl_params = new TVariantList();
                        vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                        vl_params.Add("@prm_ID_User").AsInt32 = session.ID_User;
                        vl_params.Add("@prm_ID_UserRole").AsInt32 = -1;
                        DataSet ds_roles = app.DB.Select("select_UserRoles", "UserRoles", vl_params);
                        if (app.DB.ValidDSRows(ds_roles))
                        {
                            session.ID_UserRole = login.ID_UserRole;
                        }
                    }

                    if (login.Language != "") session.Language = login.Language;

                    if (session.EntryPoint == "SALE") 
                    {
                        string str_Message = "Utilizatorul " + row["FirstName"].ToString() + " " + row["LastName"].ToString() + " a fost autentificat";
                        session.SetAlert(4, 0, 0, str_Message, str_Message, str_Message);
                    }

                    session.AddToJournal("user login", 0, 0, 0, 0, 0, 0);
                    return new JSONResult(new JSONLoginResult(true, "", row)).GetJSONResponseAsStream();
                }
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
            //--------------------------------------------------------------------------------------------
        }

        public Stream GET_CheckAccess() { return POST_CheckAccess(); }

        public Stream POST_CheckAccess()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                //--------------------------------------------------------------------------------------------
                //  process json request
                string CheckState = json_request.GetCheckState();

                try
                {
                }
                catch (Exception exc)
                {
                    if (exc is InternalException) ;
                }

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_User").AsInt32 = session.ID_User;
                if (json_request.CurrentState != "") vl_params.Add("@prm_CurrentState").AsString = json_request.CurrentState;
                else vl_params.Add("@prm_CurrentState").AsString = session.CurrentState;
                vl_params.Add("@prm_CheckState").AsString = CheckState;
                vl_params.Add("@prm_ID_UserRole").AsInt32 = session.ID_UserRole;
                DataSet ds = app.DB.Select("select_CheckState", "States", vl_params);
                if (ds == null) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (ds.Tables.Count == 0) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

                if (CheckState == "" || CheckState == "*")
                {
                    string[] str_states = new string[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        str_states[i] = ds.Tables[0].Rows[i]["NextState"].ToString();

                    return new JSONResult(str_states).GetJSONResponseAsStream();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count == 0) return new JSONResult(false).GetJSONResponseAsStream();
                    return new JSONResult(true).GetJSONResponseAsStream();
                }
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
            //--------------------------------------------------------------------------------------------
        }

        public Stream GET_CheckStateOperation() { return POST_CheckStateOperation(); }

        public Stream POST_CheckStateOperation()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                //--------------------------------------------------------------------------------------------
                //  process json request
                string CheckStateOperation = json_request.GetCheckStateOperation();

                try
                {
                }
                catch (Exception exc)
                {
                    if (exc is InternalException) ;
                }

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_User").AsInt32 = session.ID_User;
                if (json_request.CurrentState != "") vl_params.Add("@prm_CurrentState").AsString = json_request.CurrentState;
                else vl_params.Add("@prm_CurrentState").AsString = session.CurrentState;
                vl_params.Add("@prm_CheckStateOperation").AsString = CheckStateOperation;
                DataSet ds = app.DB.Select("select_CheckStateOperation", "StateOperations", vl_params);
                if (ds == null) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (ds.Tables.Count == 0) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

                if (CheckStateOperation == "")
                {
                    string[] str_states = new string[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        str_states[i] = ds.Tables[0].Rows[i]["Name"].ToString();

                    return new JSONResult(str_states).GetJSONResponseAsStream();
                }
                else
                {
                    if (ds.Tables[0].Rows.Count == 0) return new JSONResult(false).GetJSONResponseAsStream();
                    return new JSONResult(true).GetJSONResponseAsStream();
                }
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
            //--------------------------------------------------------------------------------------------
        }

        public Stream GET_CurrentUser() { return POST_CurrentUser(); }

        public Stream POST_CurrentUser()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                //--------------------------------------------------------------------------------------------
                //  process json request
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_User").AsInt32 = session.ID_User;
                DataSet ds = app.DB.Select("select_CurrentUser", "Users", vl_params);
                DataRow row = null;
                if (ds != null)
                    if (ds.Tables.Count > 0)
                        if (ds.Tables[0].Rows.Count > 0)
                            row = ds.Tables[0].Rows[0];

                return new JSONResult(row).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
            //--------------------------------------------------------------------------------------------
        }

        public Stream GET_GetUsers() { return POST_GetUsers(); }

        public Stream POST_GetUsers()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                //--------------------------------------------------------------------------------------------
                //  process json request
                //  check if current user is administrator
                UserValidation valUser = new UserValidation(app);
                if (!valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole) && valUser.LastError != JSONErrorCode.Success) return new JSONResult(valUser.LastError).GetJSONResponseAsStream();

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                vl_params.Add("@prm_ID_User").AsInt32 = -1;

                TVariantList vl_arguments = json_request.GetArguments();
                vl_params.Add("#prm_SortField").AsString = "ID";
                vl_params.Add("#prm_SortOrder").AsString = "ASC";
                vl_params.Add("@prm_QueryOffset").AsInt32 = -1;
                vl_params.Add("@prm_QueryLimit").AsInt32 = -1;
                vl_params.Add("@prm_Language").AsString = session.Language;
                vl_params.Add("@prm_QueryKeyword").AsString = "";

                if (vl_arguments != null)
                {
                    if (vl_arguments["SortField"] != null)
                        if (vl_arguments["SortField"].ValueType == TVariantType.vtString)
                        {
                            string s_sortfield = vl_arguments["SortField"].AsString;
                            if (!s_sortfield.Contains('=') && !s_sortfield.Contains('(') && !s_sortfield.Contains(')'))
                                vl_params["#prm_SortField"].AsString = s_sortfield;
                        }

                    if (vl_arguments["SortOrder"] != null)
                        if (vl_arguments["SortOrder"].ValueType == TVariantType.vtString)
                        {
                            string s_sortorder = vl_arguments["SortOrder"].AsString;
                            if (!s_sortorder.Contains('=') && !s_sortorder.Contains('(') && !s_sortorder.Contains(')'))
                                vl_params["#prm_SortOrder"].AsString = s_sortorder;
                        }

                    if (vl_arguments["QueryOffset"] != null)
                        if (vl_arguments["QueryOffset"].ValueType == TVariantType.vtInt16 || vl_arguments["QueryOffset"].ValueType == TVariantType.vtInt32 || vl_arguments["QueryOffset"].ValueType == TVariantType.vtInt64)
                            vl_params["@prm_QueryOffset"].AsInt32 = vl_arguments["QueryOffset"].AsInt32;

                    if (vl_arguments["QueryLimit"] != null)
                        if (vl_arguments["QueryLimit"].ValueType == TVariantType.vtInt16 || vl_arguments["QueryLimit"].ValueType == TVariantType.vtInt32 || vl_arguments["QueryLimit"].ValueType == TVariantType.vtInt64)
                            vl_params["@prm_QueryLimit"].AsInt32 = vl_arguments["QueryLimit"].AsInt32;

                    if (vl_arguments["QueryKeyword"] != null)
                        if (vl_arguments["QueryKeyword"].ValueType == TVariantType.vtString)
                        {
                            string s_keyword = vl_arguments["QueryKeyword"].AsString;
                            if (!s_keyword.Contains('=') && !s_keyword.Contains('(') && !s_keyword.Contains(')'))
                                vl_params["@prm_QueryKeyword"].AsString = s_keyword;
                        }
                }

                DataSet ds = app.DB.Select("select_UsersWithPaging", "Users", vl_params);
                //DataSet ds = app.DB.Select("select_UsersDetailed", "Users", vl_params);
                if (ds == null) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (ds.Tables.Count == 0) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

                return new JSONResult(ds).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        public Stream GET_AddUser() { return POST_AddUser(); }

        public Stream POST_AddUser()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                //--------------------------------------------------------------------------------------------
                //  process json request
                //  check if current user is administrator
                UserValidation valUser = new UserValidation(app);
                if (!valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole) && valUser.LastError != JSONErrorCode.Success) return new JSONResult(valUser.LastError).GetJSONResponseAsStream();

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                if (vl_arguments["LoginName"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["LoginPassword"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["Email"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["FirstName"] == null && vl_arguments["ID_Broker"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["LastName"] == null && vl_arguments["ID_Broker"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["ID_Agency"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["ID_UserRole"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();

                string FirstName = "";
                if (vl_arguments["FirstName"].ValueType == TVariantType.vtString) FirstName = vl_arguments["FirstName"].AsString;
                else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();

                string LastName = "";
                if (vl_arguments["LastName"].ValueType == TVariantType.vtString) LastName = vl_arguments["LastName"].AsString;
                else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();

                string SocialCode = "";
                if (vl_arguments["SocialCode"] != null)
                {
                    if (vl_arguments["SocialCode"].ValueType == TVariantType.vtString) SocialCode = vl_arguments["SocialCode"].AsString;

                    if (SocialCode != "")
                    {
                        TVariantList vl = new TVariantList();
                        vl.Add("@prm_SocialCode").AsString = SocialCode;
                        vl.Add("@prm_ID_Broker").AsInt32 = 0;
                        DataSet ds = app.DB.Select("select_IdentitiesbySocialCode", "Identities", vl);
                        if (app.DB.ValidDSRows(ds)) return new JSONResult(JSONErrorCode.InvalidProcedureArgument, "Codul CNP trebuie sa fie unic").GetJSONResponseAsStream();
                    }
                }

                string IdentityCard = "";
                if (vl_arguments["IdentityCard"] != null)
                {
                    if (vl_arguments["IdentityCard"].ValueType == TVariantType.vtString) IdentityCard = vl_arguments["IdentityCard"].AsString;

                    if (IdentityCard != "")
                    {
                        TVariantList vl = new TVariantList();
                        vl.Add("@prm_IdentityCard").AsString = IdentityCard;
                        vl.Add("@prm_ID_Broker").AsInt32 = 0;
                        DataSet ds = app.DB.Select("select_IdentitiesbyIdentityCard", "Identities", vl);
                        if (app.DB.ValidDSRows(ds)) return new JSONResult(JSONErrorCode.InvalidProcedureArgument, "Seria si numarul CI trebuie sa fie unice").GetJSONResponseAsStream();
                    }
                }

                string Email = "";
                if (vl_arguments["Email"].ValueType == TVariantType.vtString) Email = vl_arguments["Email"].AsString;
                else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();

                string Phone = "";
                if (vl_arguments["Phone"] != null)
                {
                    if (vl_arguments["Phone"].ValueType == TVariantType.vtString) Phone = vl_arguments["Phone"].AsString;
                    else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();
                }

                string Fax = "";
                if (vl_arguments["Fax"] != null)
                {
                    if (vl_arguments["Fax"].ValueType == TVariantType.vtString) Fax = vl_arguments["Fax"].AsString;
                    else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();
                }

                string Mobile = "";
                if (vl_arguments["Mobile"] != null)
                {
                    if (vl_arguments["Mobile"].ValueType == TVariantType.vtString) Mobile = vl_arguments["Mobile"].AsString;
                    else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();
                }

                //  check for SQL injection
                if (vl_arguments["LoginPassword"].AsString.Contains('=') || vl_arguments["LoginPassword"].AsString.Contains('(') || vl_arguments["LoginPassword"].AsString.Contains(')'))
                    return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();

                //  check for unique loginname
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                vl_params.Add("@prm_ID_User").AsInt32 = 0;
                vl_params.Add("@prm_LoginName").AsString = vl_arguments["LoginName"].AsString;
                DataSet ds_checkuser = app.DB.Select("select_ExistingLogin", "Users", vl_params);
                if (app.DB.ValidDSRows(ds_checkuser)) return new JSONResult(JSONErrorCode.InvalidProcedureContext, "The specified login already exists").GetJSONResponseAsStream();

                //  check for at leat 1 user role to be checked
                if (vl_arguments["ID_UserRole"].ValueType != TVariantType.vtInt16 &&
                    vl_arguments["ID_UserRole"].ValueType != TVariantType.vtInt32 &&
                    vl_arguments["ID_UserRole"].ValueType != TVariantType.vtInt64)
                {
                    if (vl_arguments["ID_UserRole"].ValueType != TVariantType.vtObject) return new JSONResult(JSONErrorCode.InvalidProcedureContext, "At least one user role must be selected").GetJSONResponseAsStream();

                    TVariantList vl = (TVariantList)vl_arguments["ID_UserRole"].AsObject;
                    if (vl.Count == 0) return new JSONResult(JSONErrorCode.InvalidProcedureContext, "At least one user role must be selected").GetJSONResponseAsStream();
                }

                vl_params.Add("@prm_LoginPassword").AsString = vl_arguments["LoginPassword"].AsString;
                vl_params.Add("@prm_Email").AsString = vl_arguments["Email"].AsString;
                vl_params.Add("@prm_isAdministrator").AsBoolean = false;
                vl_params.Add("@prm_isNPC").AsBoolean = false;

                int res = app.DB.Exec("insert_User", "Users", vl_params);
                if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                int ID_User = app.DB.GetIdentity();

                //  create broker
                int ID_Broker = 0;
                if (vl_arguments["ID_Broker"] != null)
                {
                    if (vl_arguments["ID_Broker"].ValueType == TVariantType.vtInt16 ||
                        vl_arguments["ID_Broker"].ValueType == TVariantType.vtInt32 ||
                        vl_arguments["ID_Broker"].ValueType == TVariantType.vtInt64) ID_Broker = vl_arguments["ID_Broker"].AsInt32;

                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID").AsInt32 = ID_Broker;
                    vl_params.Add("@prm_ID_User").AsInt32 = ID_User;
                    res = app.DB.Exec("update_isBroker", "Brokers", vl_params);
                }
                else
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_Agency").AsInt32 = vl_arguments["ID_Agency"].AsInt32;
                    vl_params.Add("@prm_ID_User").AsInt32 = ID_User;
                    vl_params.Add("@prm_isSupervisor").AsBoolean = false;
                    vl_params.Add("@prm_isBroker").AsBoolean = true;

                    res = app.DB.Exec("insert_Broker", "Brokers", vl_params);
                    if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                    ID_Broker = app.DB.GetIdentity();

                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_Client").AsInt32 = 0;
                    vl_params.Add("@prm_ID_Agency").AsInt32 = 0;
                    vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                    vl_params.Add("@prm_IsCompany").AsBoolean = false;
                    vl_params.Add("@prm_FiscalCode").AsString = "";
                    vl_params.Add("@prm_RegisterCode").AsString = "";
                    vl_params.Add("@prm_CompanyName").AsString = "";
                    vl_params.Add("@prm_SocialCode").AsString = SocialCode;
                    vl_params.Add("@prm_IdentityCard").AsString = IdentityCard;
                    vl_params.Add("@prm_Title").AsString = "";
                    vl_params.Add("@prm_FirstName").AsString = FirstName;
                    vl_params.Add("@prm_LastName").AsString = LastName;
                    vl_params.Add("@prm_ID_Address").AsString = "";
                    vl_params.Add("@prm_Bank").AsString = "";
                    vl_params.Add("@prm_BankAccount").AsString = "";
                    vl_params.Add("@prm_Phone").AsString = Phone;
                    vl_params.Add("@prm_Mobile").AsString = Mobile;
                    vl_params.Add("@prm_Fax").AsString = Fax;
                    vl_params.Add("@prm_Email").AsString = Email;
                    vl_params.Add("@prm_Website").AsString = "";

                    res = app.DB.Exec("insert_Identity", "Identities", vl_params);
                    if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                }

                //  assign the user role or user roles
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_User").AsInt32 = ID_User;
                vl_params.Add("@prm_ID_UserRole").AsInt32 = 0;
                app.DB.Exec("delete_UserXUserRolebyID_User", "UsersXUserRoles", vl_params);

                if (vl_arguments["ID_UserRole"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_UserRole"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_UserRole"].ValueType == TVariantType.vtInt64)
                {
                    int ID_UserRole = vl_arguments["ID_UserRole"].AsInt32;
                    vl_params["@prm_ID_UserRole"].AsInt32 = ID_UserRole;
                    res = app.DB.Exec("insert_UserXUserRole", "UsersXUserRoles", vl_params);
                    if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                }
                else if (vl_arguments["ID_UserRole"].ValueType == TVariantType.vtObject)
                {
                    TVariantList vl_roles = (TVariantList)vl_arguments["ID_UserRole"].AsObject;

                    for (int i = 0; i < vl_roles.Count; i++)
                    {
                        int ID_UserRole = vl_roles[i].AsInt32;
                        vl_params["@prm_ID_UserRole"].AsInt32 = ID_UserRole;
                        res = app.DB.Exec("insert_UserXUserRole", "UsersXUserRoles", vl_params);
                        if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                    }
                }

                return new JSONResult(JSONErrorCode.Success).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  Edit an existing user
        public Stream GET_EditUser() { return POST_EditUser(); }

        public Stream POST_EditUser()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                //--------------------------------------------------------------------------------------------
                //  process json request
                //  check if current user is administrator
                UserValidation valUser = new UserValidation(app);
                if (!valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole) && valUser.LastError != JSONErrorCode.Success) return new JSONResult(valUser.LastError).GetJSONResponseAsStream();

                /*TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_User").AsInt32 = session.ID_User;
                DataSet ds = app.DB.Select("select_CurrentUser", "Users", vl_params);
                if (ds == null) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (ds.Tables.Count == 0) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (ds.Tables[0].Rows.Count == 0) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();
                if (!Convert.ToBoolean(ds.Tables[0].Rows[0]["isAdministrator"])) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();*/

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                if (vl_arguments["ID_User"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["LoginName"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["LoginPassword"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["Email"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["FirstName"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["LastName"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["ID_Agency"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["ID_UserRole"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();

                string FirstName = "";
                if (vl_arguments["FirstName"].ValueType == TVariantType.vtString) FirstName = vl_arguments["FirstName"].AsString;
                else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();

                string LastName = "";
                if (vl_arguments["LastName"].ValueType == TVariantType.vtString) LastName = vl_arguments["LastName"].AsString;
                else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();

                string SocialCode = "";
                if (vl_arguments["SocialCode"] != null && vl_arguments["SocialCode"].ValueType == TVariantType.vtString)
                    SocialCode = vl_arguments["SocialCode"].AsString;

                string IdentityCard = "";
                if (vl_arguments["IdentityCard"] != null && vl_arguments["IdentityCard"].ValueType == TVariantType.vtString)
                    IdentityCard = vl_arguments["IdentityCard"].AsString;

                string Email = "";
                if (vl_arguments["Email"].ValueType == TVariantType.vtString) Email = vl_arguments["Email"].AsString;
                else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();

                string Phone = "";
                if (vl_arguments["Phone"] != null)
                {
                    if (vl_arguments["Phone"].ValueType == TVariantType.vtString) Phone = vl_arguments["Phone"].AsString;
                    else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();
                }

                string Fax = "";
                if (vl_arguments["Fax"] != null)
                {
                    if (vl_arguments["Fax"].ValueType == TVariantType.vtString) Fax = vl_arguments["Fax"].AsString;
                    else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();
                }

                string Mobile = "";
                if (vl_arguments["Mobile"] != null)
                {
                    if (vl_arguments["Mobile"].ValueType == TVariantType.vtString) Mobile = vl_arguments["Mobile"].AsString;
                    else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();
                }

                //  check for SQL injection
                if (vl_arguments["LoginPassword"].AsString.Contains('=') || vl_arguments["LoginPassword"].AsString.Contains('(') || vl_arguments["LoginPassword"].AsString.Contains(')'))
                    return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();

                //  check for at leat 1 user role to be checked
                if (vl_arguments["ID_UserRole"].ValueType != TVariantType.vtInt16 &&
                    vl_arguments["ID_UserRole"].ValueType != TVariantType.vtInt32 &&
                    vl_arguments["ID_UserRole"].ValueType != TVariantType.vtInt64)
                {
                    if (vl_arguments["ID_UserRole"].ValueType != TVariantType.vtObject) return new JSONResult(JSONErrorCode.InvalidProcedureContext, "At least one user role must be selected").GetJSONResponseAsStream();

                    TVariantList vl = (TVariantList)vl_arguments["ID_UserRole"].AsObject;
                    if (vl.Count == 0) return new JSONResult(JSONErrorCode.InvalidProcedureContext, "At least one user role must be selected").GetJSONResponseAsStream();
                }

                int ID_User = vl_arguments["ID_User"].AsInt32;

                //  check for unique loginname
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                vl_params.Add("@prm_ID_User").AsInt32 = ID_User;
                vl_params.Add("@prm_LoginName").AsString = vl_arguments["LoginName"].AsString;
                DataSet ds_checkuser = app.DB.Select("select_ExistingLogin", "Users", vl_params);
                if (app.DB.ValidDSRows(ds_checkuser)) return new JSONResult(JSONErrorCode.InvalidProcedureContext, "The specified login already exists").GetJSONResponseAsStream();


                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                vl_params.Add("@prm_ID").AsInt32 = ID_User;
                DataSet ds_user = app.DB.Select("select_Users", "Users", vl_params);
                if (ds_user == null) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (ds_user.Tables.Count == 0) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (ds_user.Tables[0].Rows.Count == 0) return new JSONResult(JSONErrorCode.InvalidProcedureArgument).GetJSONResponseAsStream();

                if (vl_arguments["LoginPassword"].AsString == "")
                    vl_arguments["LoginPassword"].AsString = ds_user.Tables[0].Rows[0]["LoginPassword"].ToString();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_User").AsInt32 = ID_User;
                DataSet ds_broker = app.DB.Select("select_BrokersbyID_User", "Brokers", vl_params);
                if (ds_broker == null) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (ds_broker.Tables.Count == 0) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (ds_broker.Tables[0].Rows.Count == 0) return new JSONResult(JSONErrorCode.InvalidProcedureArgument).GetJSONResponseAsStream();
                int ID_Broker = Convert.ToInt32(ds_broker.Tables[0].Rows[0]["ID"]);

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Agency").AsInt32 = 0;
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                vl_params.Add("@prm_ID_Client").AsInt32 = 0;
                DataSet ds_identity = app.DB.Select("select_IdentitiesbyIDs", "Identities", vl_params);
                if (ds_identity == null) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (ds_identity.Tables.Count == 0) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (ds_identity.Tables[0].Rows.Count == 0) return new JSONResult(JSONErrorCode.InvalidProcedureArgument).GetJSONResponseAsStream();
                int ID_Identity = Convert.ToInt32(ds_identity.Tables[0].Rows[0]["ID"]);

                if (vl_arguments["SocialCode"] != null && SocialCode != "")
                {
                    TVariantList vl = new TVariantList();
                    vl.Add("@prm_SocialCode").AsString = SocialCode;
                    vl.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                    DataSet ds = app.DB.Select("select_IdentitiesbySocialCode", "Identities", vl);
                    if (app.DB.ValidDSRows(ds)) return new JSONResult(JSONErrorCode.InvalidProcedureArgument, "Codul CNP trebuie sa fie unic").GetJSONResponseAsStream();
                }

                if (vl_arguments["IdentityCard"] != null && IdentityCard != "")
                {
                    TVariantList vl = new TVariantList();
                    vl.Add("@prm_IdentityCard").AsString = IdentityCard;
                    vl.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                    DataSet ds = app.DB.Select("select_IdentitiesbyIdentityCard", "Identities", vl);
                    if (app.DB.ValidDSRows(ds)) return new JSONResult(JSONErrorCode.InvalidProcedureArgument, "Seria si numarul CI trebuie sa fie unice").GetJSONResponseAsStream();
                }

                bool isActive = true;
                if (vl_arguments["isActive"] != null) isActive = vl_arguments["isActive"].AsBoolean;

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                vl_params.Add("@prm_LoginName").AsString = vl_arguments["LoginName"].AsString;
                vl_params.Add("@prm_LoginPassword").AsString = vl_arguments["LoginPassword"].AsString;
                vl_params.Add("@prm_Email").AsString = vl_arguments["Email"].AsString;
                vl_params.Add("@prm_isAdministrator").AsBoolean = false;
                vl_params.Add("@prm_isNPC").AsBoolean = false;
                vl_params.Add("@prm_isActive").AsBoolean = isActive;
                vl_params.Add("@prm_ID").AsInt32 = vl_arguments["ID_User"].AsInt32;

                int res = app.DB.Exec("update_User", "Users", vl_params);
                if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                //int ID_User = app.DB.GetIdentity();

                //  edit broker
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Agency").AsInt32 = vl_arguments["ID_Agency"].AsInt32;
                vl_params.Add("@prm_ID_User").AsInt32 = vl_arguments["ID_User"].AsInt32;
                vl_params.Add("@prm_isSupervisor").AsBoolean = false;
                vl_params.Add("@prm_ID").AsInt32 = ID_Broker;

                res = app.DB.Exec("update_Broker", "Brokers", vl_params);                
                if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                //int ID_Broker = app.DB.GetIdentity();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Client").AsInt32 = 0;
                vl_params.Add("@prm_ID_Agency").AsInt32 = 0;
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                vl_params.Add("@prm_IsCompany").AsBoolean = false;
                vl_params.Add("@prm_FiscalCode").AsString = "";
                vl_params.Add("@prm_RegisterCode").AsString = "";
                vl_params.Add("@prm_CompanyName").AsString = "";
                vl_params.Add("@prm_SocialCode").AsString = SocialCode;
                vl_params.Add("@prm_IdentityCard").AsString = IdentityCard;
                vl_params.Add("@prm_Title").AsString = "";
                vl_params.Add("@prm_FirstName").AsString = FirstName;
                vl_params.Add("@prm_LastName").AsString = LastName;
                vl_params.Add("@prm_Bank").AsString = "";
                vl_params.Add("@prm_BankAccount").AsString = "";
                vl_params.Add("@prm_ID_Address").AsInt32 = 0;
                vl_params.Add("@prm_Phone").AsString = Phone;
                vl_params.Add("@prm_Mobile").AsString = Mobile;
                vl_params.Add("@prm_Fax").AsString = Fax;
                vl_params.Add("@prm_Email").AsString = Email;
                vl_params.Add("@prm_Website").AsString = "";
                vl_params.Add("@prm_ID").AsInt32 = ID_Identity;

                res = app.DB.Exec("update_Identity", "Identities", vl_params);
                if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

                //  assign user roles
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_User").AsInt32 = vl_arguments["ID_User"].AsInt32;
                vl_params.Add("@prm_ID_UserRole").AsInt32 = 0;
                app.DB.Exec("delete_UserXUserRolebyID_User", "UsersXUserRoles", vl_params);

                if (vl_arguments["ID_UserRole"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_UserRole"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_UserRole"].ValueType == TVariantType.vtInt64)
                {
                    int ID_UserRole = vl_arguments["ID_UserRole"].AsInt32;
                    vl_params["@prm_ID_UserRole"].AsInt32 = ID_UserRole;
                    res = app.DB.Exec("insert_UserXUserRole", "UsersXUserRoles", vl_params);
                    if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                }
                else if (vl_arguments["ID_UserRole"].ValueType == TVariantType.vtObject)
                {
                    TVariantList vl_roles = (TVariantList)vl_arguments["ID_UserRole"].AsObject;

                    for (int i = 0; i < vl_roles.Count; i++)
                    {
                        int ID_UserRole = vl_roles[i].AsInt32;
                        vl_params["@prm_ID_UserRole"].AsInt32 = ID_UserRole;
                        res = app.DB.Exec("insert_UserXUserRole", "UsersXUserRoles", vl_params);
                        if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                    }
                }

                return new JSONResult(JSONErrorCode.Success).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  Delete user
        public Stream GET_DeleteUser() { return POST_DeleteUser(); }

        public Stream POST_DeleteUser()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                UserValidation valUser = new UserValidation(app);
                if (!valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole) && valUser.LastError != JSONErrorCode.Success) return new JSONResult(valUser.LastError).GetJSONResponseAsStream();

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                if (vl_arguments["ID_User"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing, "ID_User is not specified").GetJSONResponseAsStream();
                int ID_User = 0;
                if (vl_arguments["ID_User"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_User"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_User"].ValueType == TVariantType.vtInt64) ID_User = vl_arguments["ID_User"].AsInt32;
                else return new JSONResult(JSONErrorCode.InvalidProcedureArgument, "Value not recognized").GetJSONResponseAsStream();

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_User;
                int int_users = app.DB.Exec("delete_User", "Users", vl_params);
                if (int_users <= 0) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

                return new JSONResult(JSONErrorCode.Success).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  Register user
        public Stream GET_RegisterUser() { return POST_RegisterUser(); }
        public Stream POST_RegisterUser()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();

                //--------------------------------------------------------------------------------------------
                //  process json request
                //  check if current user is administrator
                UserValidation valUser = new UserValidation(app);
                if (!valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole) && valUser.LastError != JSONErrorCode.Success) return new JSONResult(valUser.LastError).GetJSONResponseAsStream();

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                /*
                if (vl_arguments["LoginName"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["LoginPassword"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["Email"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["FirstName"] == null && vl_arguments["ID_Broker"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["LastName"] == null && vl_arguments["ID_Broker"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["ID_Agency"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();
                if (vl_arguments["ID_UserRole"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing).GetJSONResponseAsStream();

                string FirstName = "";
                if (vl_arguments["FirstName"].ValueType == TVariantType.vtString) FirstName = vl_arguments["FirstName"].AsString;
                else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();

                string LastName = "";
                if (vl_arguments["LastName"].ValueType == TVariantType.vtString) LastName = vl_arguments["LastName"].AsString;
                else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();

                string SocialCode = "";
                if (vl_arguments["SocialCode"] != null)
                {
                    if (vl_arguments["SocialCode"].ValueType == TVariantType.vtString) SocialCode = vl_arguments["SocialCode"].AsString;

                    if (SocialCode != "")
                    {
                        TVariantList vl = new TVariantList();
                        vl.Add("@prm_SocialCode").AsString = SocialCode;
                        vl.Add("@prm_ID_Broker").AsInt32 = 0;
                        DataSet ds = app.DB.Select("select_IdentitiesbySocialCode", "Identities", vl);
                        if (app.DB.ValidDSRows(ds)) return new JSONResult(JSONErrorCode.InvalidProcedureArgument, "Codul CNP trebuie sa fie unic").GetJSONResponseAsStream();
                    }
                }

                string IdentityCard = "";
                if (vl_arguments["IdentityCard"] != null)
                {
                    if (vl_arguments["IdentityCard"].ValueType == TVariantType.vtString) IdentityCard = vl_arguments["IdentityCard"].AsString;

                    if (IdentityCard != "")
                    {
                        TVariantList vl = new TVariantList();
                        vl.Add("@prm_IdentityCard").AsString = IdentityCard;
                        vl.Add("@prm_ID_Broker").AsInt32 = 0;
                        DataSet ds = app.DB.Select("select_IdentitiesbyIdentityCard", "Identities", vl);
                        if (app.DB.ValidDSRows(ds)) return new JSONResult(JSONErrorCode.InvalidProcedureArgument, "Seria si numarul CI trebuie sa fie unice").GetJSONResponseAsStream();
                    }
                }

                string Email = "";
                if (vl_arguments["Email"].ValueType == TVariantType.vtString) Email = vl_arguments["Email"].AsString;
                else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();

                string Phone = "";
                if (vl_arguments["Phone"] != null)
                {
                    if (vl_arguments["Phone"].ValueType == TVariantType.vtString) Phone = vl_arguments["Phone"].AsString;
                    else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();
                }

                string Fax = "";
                if (vl_arguments["Fax"] != null)
                {
                    if (vl_arguments["Fax"].ValueType == TVariantType.vtString) Fax = vl_arguments["Fax"].AsString;
                    else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();
                }

                string Mobile = "";
                if (vl_arguments["Mobile"] != null)
                {
                    if (vl_arguments["Mobile"].ValueType == TVariantType.vtString) Mobile = vl_arguments["Mobile"].AsString;
                    else return new JSONResult(JSONErrorCode.SecurityAuditFailed, "Value not recognized").GetJSONResponseAsStream();
                }

                //  check for SQL injection
                if (vl_arguments["LoginPassword"].AsString.Contains('=') || vl_arguments["LoginPassword"].AsString.Contains('(') || vl_arguments["LoginPassword"].AsString.Contains(')'))
                    return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();

                //  check for unique loginname
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                vl_params.Add("@prm_ID_User").AsInt32 = 0;
                vl_params.Add("@prm_LoginName").AsString = vl_arguments["LoginName"].AsString;
                DataSet ds_checkuser = app.DB.Select("select_ExistingLogin", "Users", vl_params);
                if (app.DB.ValidDSRows(ds_checkuser)) return new JSONResult(JSONErrorCode.InvalidProcedureContext, "The specified login already exists").GetJSONResponseAsStream();

                //  check for at leat 1 user role to be checked
                if (vl_arguments["ID_UserRole"].ValueType != TVariantType.vtInt16 &&
                    vl_arguments["ID_UserRole"].ValueType != TVariantType.vtInt32 &&
                    vl_arguments["ID_UserRole"].ValueType != TVariantType.vtInt64)
                {
                    if (vl_arguments["ID_UserRole"].ValueType != TVariantType.vtObject) return new JSONResult(JSONErrorCode.InvalidProcedureContext, "At least one user role must be selected").GetJSONResponseAsStream();

                    TVariantList vl = (TVariantList)vl_arguments["ID_UserRole"].AsObject;
                    if (vl.Count == 0) return new JSONResult(JSONErrorCode.InvalidProcedureContext, "At least one user role must be selected").GetJSONResponseAsStream();
                }

                vl_params.Add("@prm_LoginPassword").AsString = vl_arguments["LoginPassword"].AsString;
                vl_params.Add("@prm_Email").AsString = vl_arguments["Email"].AsString;
                vl_params.Add("@prm_isAdministrator").AsBoolean = false;
                vl_params.Add("@prm_isNPC").AsBoolean = false;

                int res = app.DB.Exec("insert_User", "Users", vl_params);
                if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                int ID_User = app.DB.GetIdentity();

                //  create broker
                int ID_Broker = 0;
                if (vl_arguments["ID_Broker"] != null)
                {
                    if (vl_arguments["ID_Broker"].ValueType == TVariantType.vtInt16 ||
                        vl_arguments["ID_Broker"].ValueType == TVariantType.vtInt32 ||
                        vl_arguments["ID_Broker"].ValueType == TVariantType.vtInt64) ID_Broker = vl_arguments["ID_Broker"].AsInt32;

                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID").AsInt32 = ID_Broker;
                    vl_params.Add("@prm_ID_User").AsInt32 = ID_User;
                    res = app.DB.Exec("update_isBroker", "Brokers", vl_params);
                }
                else
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_Agency").AsInt32 = vl_arguments["ID_Agency"].AsInt32;
                    vl_params.Add("@prm_ID_User").AsInt32 = ID_User;
                    vl_params.Add("@prm_isSupervisor").AsBoolean = false;
                    vl_params.Add("@prm_isBroker").AsBoolean = true;

                    res = app.DB.Exec("insert_Broker", "Brokers", vl_params);
                    if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                    ID_Broker = app.DB.GetIdentity();

                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_Client").AsInt32 = 0;
                    vl_params.Add("@prm_ID_Agency").AsInt32 = 0;
                    vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                    vl_params.Add("@prm_IsCompany").AsBoolean = false;
                    vl_params.Add("@prm_FiscalCode").AsString = "";
                    vl_params.Add("@prm_RegisterCode").AsString = "";
                    vl_params.Add("@prm_CompanyName").AsString = "";
                    vl_params.Add("@prm_SocialCode").AsString = SocialCode;
                    vl_params.Add("@prm_IdentityCard").AsString = IdentityCard;
                    vl_params.Add("@prm_Title").AsString = "";
                    vl_params.Add("@prm_FirstName").AsString = FirstName;
                    vl_params.Add("@prm_LastName").AsString = LastName;
                    vl_params.Add("@prm_ID_Address").AsString = "";
                    vl_params.Add("@prm_Bank").AsString = "";
                    vl_params.Add("@prm_BankAccount").AsString = "";
                    vl_params.Add("@prm_Phone").AsString = Phone;
                    vl_params.Add("@prm_Mobile").AsString = Mobile;
                    vl_params.Add("@prm_Fax").AsString = Fax;
                    vl_params.Add("@prm_Email").AsString = Email;
                    vl_params.Add("@prm_Website").AsString = "";

                    res = app.DB.Exec("insert_Identity", "Identities", vl_params);
                    if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                }

                //  assign the user role or user roles
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_User").AsInt32 = ID_User;
                vl_params.Add("@prm_ID_UserRole").AsInt32 = 0;
                app.DB.Exec("delete_UserXUserRolebyID_User", "UsersXUserRoles", vl_params);

                if (vl_arguments["ID_UserRole"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_UserRole"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_UserRole"].ValueType == TVariantType.vtInt64)
                {
                    int ID_UserRole = vl_arguments["ID_UserRole"].AsInt32;
                    vl_params["@prm_ID_UserRole"].AsInt32 = ID_UserRole;
                    res = app.DB.Exec("insert_UserXUserRole", "UsersXUserRoles", vl_params);
                    if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                }
                else if (vl_arguments["ID_UserRole"].ValueType == TVariantType.vtObject)
                {
                    TVariantList vl_roles = (TVariantList)vl_arguments["ID_UserRole"].AsObject;

                    for (int i = 0; i < vl_roles.Count; i++)
                    {
                        int ID_UserRole = vl_roles[i].AsInt32;
                        vl_params["@prm_ID_UserRole"].AsInt32 = ID_UserRole;
                        res = app.DB.Exec("insert_UserXUserRole", "UsersXUserRoles", vl_params);
                        if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                    }
                }
                */

                return new JSONResult(JSONErrorCode.Success).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  Enable/Disable user
        public Stream GET_EnableUser() { return POST_EnableUser(); }

        public Stream POST_EnableUser()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                UserValidation valUser = new UserValidation(app);
                if (!valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole) && valUser.LastError != JSONErrorCode.Success) return new JSONResult(valUser.LastError).GetJSONResponseAsStream();

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                if (vl_arguments["ID_User"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing, "ID_User is not specified").GetJSONResponseAsStream();
                int ID_User = 0;
                if (vl_arguments["ID_User"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_User"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_User"].ValueType == TVariantType.vtInt64) ID_User = vl_arguments["ID_User"].AsInt32;
                else return new JSONResult(JSONErrorCode.InvalidProcedureArgument, "Value not recognized").GetJSONResponseAsStream();

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_User;
                int int_users = app.DB.Exec("enable_User", "Users", vl_params);
                if (int_users <= 0) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

                return new JSONResult(JSONErrorCode.Success).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        public Stream GET_DisableUser() { return POST_DisableUser(); }

        public Stream POST_DisableUser()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                UserValidation valUser = new UserValidation(app);
                if (!valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole) && valUser.LastError != JSONErrorCode.Success) return new JSONResult(valUser.LastError).GetJSONResponseAsStream();

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                if (vl_arguments["ID_User"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing, "ID_User is not specified").GetJSONResponseAsStream();
                int ID_User = 0;
                if (vl_arguments["ID_User"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_User"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_User"].ValueType == TVariantType.vtInt64) ID_User = vl_arguments["ID_User"].AsInt32;
                else return new JSONResult(JSONErrorCode.InvalidProcedureArgument, "Value not recognized").GetJSONResponseAsStream();

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_User;
                int int_users = app.DB.Exec("disable_User", "Users", vl_params);
                if (int_users <= 0) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

                return new JSONResult(JSONErrorCode.Success).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  return the list of user roles
        public Stream GET_GetUserRoles() { return POST_GetUserRoles(); }

        public Stream POST_GetUserRoles()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                //--------------------------------------------------------------------------------------------
                //  process json request
                //  check if current user is administrator
                UserValidation valUser = new UserValidation(app);
                if (/*!valUser.isAdministrator(session.ID_User) && */valUser.LastError != JSONErrorCode.Success) return new JSONResult(valUser.LastError).GetJSONResponseAsStream();

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                vl_params.Add("@prm_ID_User").AsInt32 = -1;
                if (!valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole)) vl_params["@prm_ID_User"].AsInt32 = session.ID_User;
                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments != null)
                {
                    if (vl_arguments["ID_User"] != null && (valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole) || session.ID_User == vl_arguments["ID_User"].AsInt32))
                        vl_params["@prm_ID_User"].AsInt32 = vl_arguments["ID_User"].AsInt32;
                }
                vl_params.Add("@prm_ID_UserRole").AsInt32 = -1;

                DataSet ds = app.DB.Select("select_UserRoles", "UserRoles", vl_params);
                if (ds == null) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (ds.Tables.Count == 0) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

                return new JSONResult(ds).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  Add user role
        public Stream GET_AddUserRole() { return POST_AddUserRole(); }

        public Stream POST_AddUserRole()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  Edit user role
        public Stream GET_EditUserRole() { return POST_EditUserRole(); }

        public Stream POST_EditUserRole()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  Duplicate user role
        public Stream GET_DuplicateUserRole() { return POST_DuplicateUserRole(); }

        public Stream POST_DuplicateUserRole()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  Delete user role
        public Stream GET_DeleteUserRole() { return POST_DeleteUserRole(); }

        public Stream POST_DeleteUserRole()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        public Stream GET_GetStates() { return POST_GetStates(); }

        public Stream POST_GetStates()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                //  check if current user is administrator
                UserValidation valUser = new UserValidation(app);
                if (!valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole) && valUser.LastError != JSONErrorCode.Success) return new JSONResult(valUser.LastError).GetJSONResponseAsStream();

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = -1;
                DataSet ds = app.DB.Select("select_States", "States", vl_params);
                if (ds == null) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (ds.Tables.Count == 0) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

                return new JSONResult(ds).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  Add state
        public Stream GET_AddState() { return POST_AddState(); }

        public Stream POST_AddState()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                //  check if current user is administrator
                UserValidation valUser = new UserValidation(app);
                if (!valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole) && valUser.LastError != JSONErrorCode.Success) return new JSONResult(valUser.LastError).GetJSONResponseAsStream();

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                if (vl_arguments["Name"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing, "Name").GetJSONResponseAsStream();
                if (vl_arguments["Name"].ValueType != TVariantType.vtString) return new JSONResult(JSONErrorCode.InvalidProcedureArgument, "type unrecognized").GetJSONResponseAsStream();
                string str_Name = vl_arguments["Name"].AsString.Trim();
                if (str_Name == "") return new JSONResult(JSONErrorCode.InvalidProcedureArgument, "Name argument empty").GetJSONResponseAsStream();

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                int res = app.DB.Exec("insert_State", "States", vl_params);
                if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

                return new JSONResult(res).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  Edit state
        public Stream GET_EditState() { return POST_EditState(); }

        public Stream POST_EditState()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                //  check if current user is administrator
                UserValidation valUser = new UserValidation(app);
                if (!valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole) && valUser.LastError != JSONErrorCode.Success) return new JSONResult(valUser.LastError).GetJSONResponseAsStream();

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                if (vl_arguments["ID_State"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing, "ID_State").GetJSONResponseAsStream();
                if (vl_arguments["Name"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing, "Name").GetJSONResponseAsStream();

                if (vl_arguments["ID_State"].ValueType != TVariantType.vtByte ||
                    vl_arguments["ID_State"].ValueType != TVariantType.vtInt16 ||
                    vl_arguments["ID_State"].ValueType != TVariantType.vtInt32 ||
                    vl_arguments["ID_State"].ValueType != TVariantType.vtInt64) return new JSONResult(JSONErrorCode.InvalidProcedureArgument, "type unrecognized").GetJSONResponseAsStream();
                int ID_State = vl_arguments["ID_State"].AsInt32;

                if (vl_arguments["Name"].ValueType != TVariantType.vtString) return new JSONResult(JSONErrorCode.InvalidProcedureArgument, "type unrecognized").GetJSONResponseAsStream();
                string str_Name = vl_arguments["Name"].AsString.Trim();
                if (str_Name == "") return new JSONResult(JSONErrorCode.InvalidProcedureArgument, "Name argument empty").GetJSONResponseAsStream();

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_ID").AsInt32 = ID_State;
                int res = app.DB.Exec("update_State", "States", vl_params);
                if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

                return new JSONResult(res).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  Delete state
        public Stream GET_DeleteState() { return POST_DeleteState(); }

        public Stream POST_DeleteState()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                //  check if current user is administrator
                UserValidation valUser = new UserValidation(app);
                if (!valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole) && valUser.LastError != JSONErrorCode.Success) return new JSONResult(valUser.LastError).GetJSONResponseAsStream();

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                if (vl_arguments["ID_State"] == null) return new JSONResult(JSONErrorCode.ProcedureArgumentMissing, "ID_State").GetJSONResponseAsStream();

                if (vl_arguments["ID_State"].ValueType != TVariantType.vtByte ||
                    vl_arguments["ID_State"].ValueType != TVariantType.vtInt16 ||
                    vl_arguments["ID_State"].ValueType != TVariantType.vtInt32 ||
                    vl_arguments["ID_State"].ValueType != TVariantType.vtInt64) return new JSONResult(JSONErrorCode.InvalidProcedureArgument, "type unrecognized").GetJSONResponseAsStream();
                int ID_State = vl_arguments["ID_State"].AsInt32;

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_isDeleted").AsBoolean = true;
                vl_params.Add("@prm_ID").AsInt32 = ID_State;
                int res = app.DB.Exec("update_Deleted", "States", vl_params);
                if (res == -1) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

                return new JSONResult(res).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  Add state operation
        public Stream GET_AddStateOperation() { return POST_AddStateOperation(); }

        public Stream POST_AddStateOperation()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  Edit state operation
        public Stream GET_EditStateOperation() { return POST_EditStateOperation(); }

        public Stream POST_EditStateOperation()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  Delete state operation
        public Stream GET_DeleteStateOperation() { return POST_DeleteStateOperation(); }

        public Stream POST_DeleteStateOperation()
        {
            TBusiness app = new TBusiness();
            Session session = new Session();

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                if (!session.Init(json_request, app)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();                

                TVariantList vl_arguments = json_request.GetArguments();
                if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }

        //  Set variable in Session
        public Stream GET_SetVar() { return POST_SetVar(); }

        public Stream POST_SetVar()
        {
            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //if (!TBusiness.InitDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
            //if (!Session.Init(json_request)) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            return null;
        }

        //  Get variable from Session
        public Stream GET_GetVar() { return POST_GetVar(); }

        public Stream POST_GetVar()
        {
            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();

            //if (!TBusiness.InitDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
            //if (!Session.Init(json_request)) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

            return null;
        }


    }
}
