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
using Business.JSONObjects;

namespace BRMDataReader
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BRMRead" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BRMRead.svc or BRMRead.svc.cs at the Solution Explorer and start debugging.    
    
    /*[ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]*/
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class BRMRead : IBRMRead
    {
        public BRMRead()
        {
            /*if (TBusiness.DB == null) TBusiness.InitDB();

            if (!AbstractDBModule.isLoaded(typeof(UsersDBModule))) new UsersDBModule();*/
        }

        ~BRMRead()
        {
            /*if (TBusiness.DB != null)
            {
                TBusiness.CloseDB();
            }*/
        }

        public Stream GET_DefaultEndPoint()
        {
            return POST_DefaultEndPoint(); 
        }

        public Stream POST_DefaultEndPoint()
        {
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return new JSONResult(JSONErrorCode.WrongMethodCall, "").GetJSONResponseAsStream();
        }

        public Stream GET_ErrorCodes()
        {
            return POST_ErrorCodes();
        }

        public Stream POST_ErrorCodes()
        {
            var s = Enum.GetNames(typeof(JSONErrorCode));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return new JSONResult(s).GetJSONResponseAsStream();
        }

        public Stream GET_Capabilities()
        {
            return POST_Capabilities();
        }

        public Stream POST_Capabilities()
        {
            MethodInfo[] methodInfos = Type.GetType("BRMDataReader.BRMRead").GetMethods(BindingFlags.Public | BindingFlags.Instance);
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

        public string GET_Echo()
        {
            return POST_Echo();
        }

        public string POST_Echo()
        {
            // Get the raw json POST content.  .Net has this in XML string..
            string JSONstring = OperationContext.Current.RequestContext.RequestMessage.ToString();

            return JSONstring;
        }

        public Stream GET_ServerTime()
        {
            return POST_ServerTime();
        }

        public Stream POST_ServerTime()
        {
            return new JSONResult(DateTime.Now).GetJSONResponseAsStream();
        }

        public Stream GET_Enumerate()
        {
            return POST_Enumerate();
        }

        public Stream POST_Enumerate()
        {
            TBusiness app = new TBusiness();
            if (!app.InitDB(false)) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
            new UsersDBModule(app, null);

            try
            {
                string[] appcollections = app.DB.ApplicationCollections;
                JSONProcedureCollection[] jsoncollections = new JSONProcedureCollection[appcollections.Length];
                for (int i = 0; i < jsoncollections.Length; i++)
                    jsoncollections[i] = new JSONProcedureCollection() { Collection = appcollections[i], Procedures = app.DB.ApplicationSQLs(appcollections[i], TDBFunctionType.DBFunctionSelect) };

                /*string[] collections = TBusiness.DB.SQLCollections;
                JSONProcedureCollection[] jsoncollections = new JSONProcedureCollection[collections.Length];
                for (int i = 0; i < jsoncollections.Length; i++)
                {
                    jsoncollections[i] = new JSONProcedureCollection() { Collection = collections[i], Procedures = app.SQLs(collections[i]) };
                }*/

                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                return new JSONResult(jsoncollections).GetJSONResponseAsStream();
            }
            catch(Exception exc)
            {
                return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();
            }
        }

        public Stream GET_EnumerateCollections()
        {
            return POST_EnumerateCollections();
        }

        public Stream POST_EnumerateCollections()
        {
            TBusiness app = new TBusiness();
            if (!app.InitDB(false)) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
            new UsersDBModule(app, null);
            new FormsDBModule(app, null);

            try
            {
                string[] appcollections = app.DB.ApplicationCollections;

                return new JSONResult(appcollections).GetJSONResponseAsStream();
            }
            catch(Exception exc)
            {
                return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();
            }
        }

        public Stream GET_EnumerateProcedures(string collection)
        {
            return POST_EnumerateProcedures(collection);
        }

        public Stream POST_EnumerateProcedures(string collection)
        {
            TBusiness app = new TBusiness();
            if (!app.InitDB(false)) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
            new UsersDBModule(app, null);
            new FormsDBModule(app, null);

            try
            {
                string[] appprocedures = app.DB.ApplicationSQLs(collection, TDBFunctionType.DBFunctionSelect);

                return new JSONResult(appprocedures).GetJSONResponseAsStream();
            }
            catch (Exception exc)
            {
                return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();
            }
        }

        public Stream GET_Select(string collection, string procedure)
        {
            return POST_Select(collection, procedure);
        }

        public Stream POST_Select(string collection, string procedure)
        {
            DateTime t_start = DateTime.Now;
            DateTime t_create = DateTime.Now;
            DateTime t_decode = DateTime.Now;
            DateTime t_dbsession = DateTime.Now;
            DateTime t_initcontext = DateTime.Now;
            DateTime t_loadmodules = DateTime.Now;
            DateTime t_select = DateTime.Now;
            DateTime t_encode = DateTime.Now;
            
            TBusiness app = new TBusiness();
            Session session = new Session();
            t_create = DateTime.Now;

            JSONRequest json_request = JSONRequest.Decode();
            if (json_request == null) return new JSONResult(JSONErrorCode.WrongRequestFormat).GetJSONResponseAsStream();
            t_decode = DateTime.Now;

            //  session db can be the same database as the rest of the application or a separate thing
            //  if it is separated from the main db then the main db can be customized to load depending on the entrypoint
            if (!app.InitSessionDB()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
            t_dbsession = DateTime.Now;

            try
            {
                if (!app.InitContext()) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();        
                if (!session.Init(json_request, app, false)) return new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();
                t_initcontext = DateTime.Now;

                new UsersDBModule(app, session);
                FormsDBModule mod = new FormsDBModule(app, session);
                t_loadmodules = DateTime.Now;

                //--------------------------------------------------------------------------------------------
                //  process json request
                try
                {
                    int idx_function = app.DB.FindFunction(procedure, collection);
                    int idx_sql = app.DB.GetSQLIndex(procedure, collection);
                    if (idx_function == -1 && idx_sql == -1)
                        return new JSONResult(JSONErrorCode.ProcedureNotFound).GetJSONResponseAsStream();

                    TVariantList vl_arguments = json_request.GetArguments();
                    if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                    DataSet ds = app.DB.Select(procedure, collection, vl_arguments);
                    t_select = DateTime.Now;
                    if (ds == null)
                    {
                        session.Log("Error encountered at request " + collection + "." + procedure);
                        session.Log("ID Session: " + session.ID.ToString());
                        session.Log("ID User: " + session.ID_User.ToString());
                        session.Log("ID Broker: " + session.ID_Broker.ToString());
                        session.Log("ID Bursary: " + session.ID_Bursary.ToString());

                        return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                    }
                    if (ds.Tables.Count == 0)
                    {
                        if (ds.DataSetName == "structured")
                        {
                            return new JSONResult(mod.getFormData(vl_arguments)).GetJSONResponseAsStream();
                        }
                        else
                        {
                            session.Log("Error encountered at request " + collection + "." + procedure);
                            session.Log("ID Session: " + session.ID.ToString());
                            session.Log("ID User: " + session.ID_User.ToString());
                            session.Log("ID Broker: " + session.ID_Broker.ToString());
                            session.Log("ID Bursary: " + session.ID_Bursary.ToString());

                            return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
                        }
                    }

                    JSONResult json = new JSONResult(ds);

                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
                    t_encode = DateTime.Now;
                    return json.GetJSONResponseAsStream();
                }
                catch (Exception exc)
                {
                    return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();
                }
            }
            finally
            {
                app.ReleaseContext();
                app.DestroyDBs();
            }
        }


    }


}
