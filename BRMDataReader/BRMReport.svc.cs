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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BRMReport" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BRMReport.svc or BRMReport.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class BRMReport : IBRMReport
    {

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
            MethodInfo[] methodInfos = Type.GetType("BRMDataReader.BRMReport").GetMethods(BindingFlags.Public | BindingFlags.Instance);
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
            try
            {
                JasperReports js = new JasperReports();

                string[] str_collections = js.EnumerateCollections();
                JSONProcedureCollection[] json_collections = new JSONProcedureCollection[str_collections.Length];
                for (int i = 0; i < str_collections.Length; i++)
                {
                    json_collections[i] = new JSONProcedureCollection();
                    json_collections[i].Collection = str_collections[i];
                    json_collections[i].Procedures = js.EnumerateReports(str_collections[i]);
                }

                return new JSONResult(json_collections).GetJSONResponseAsStream();
            }
            catch (Exception exc)
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
            try
            {
                JasperReports js = new JasperReports();

                string[] repcollections = js.EnumerateCollections();
                return new JSONResult(repcollections).GetJSONResponseAsStream();
            }
            catch (Exception exc)
            {
                return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();
            }
        }

        public Stream GET_EnumerateProcedures(string ReportPath)
        {
            return POST_EnumerateProcedures(ReportPath);
        }

        public Stream POST_EnumerateProcedures(string ReportPath)
        {
            try
            {
                JasperReports js = new JasperReports();

                string[] reports = js.EnumerateReports(ReportPath);
                return new JSONResult(reports).GetJSONResponseAsStream();
            }
            catch (Exception exc)
            {
                return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();
            }
        }

        public Stream GET_GenerateReport(string ReportPath, string ReportID)
        {            
            return POST_GenerateReport(ReportPath, ReportID);
        }

        public Stream POST_GenerateReport(string ReportPath, string ReportID)
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
                new UsersDBModule(app, session);

                //--------------------------------------------------------------------------------------------
                //  process json request

                try
                {                    
                    TVariantList vl_arguments = json_request.GetArguments();
                    if (vl_arguments == null) return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();

                    JasperReports js = new JasperReports();
                    TReportExecResult res = js.getReport(ReportPath, ReportID, vl_arguments);

                    switch (res.ErrorCode)
                    {
                        case TReportExecError.Success:
                            JSONFile json_file = new JSONFile() { FileName = res.FileName, Base64Data = res.Base64Data };
                            return new JSONResult(json_file).GetJSONResponseAsStream();
                            break;

                        case TReportExecError.ArgumentMissing:
                            return new JSONResult(JSONErrorCode.ProcedureArgumentMissing, res.Message).GetJSONResponseAsStream();

                        case TReportExecError.ValidationUnsuccesful:
                            if (res.ParamValidations != null)
                            {
                                JSONKeyValuePair[] validations = new JSONKeyValuePair[res.ParamValidations.Count];
                                for (int i = 0; i < res.ParamValidations.Count; i++)
                                    validations[i] = new JSONKeyValuePair() { Key = res.ParamValidations[i].Name, Value = res.ParamValidations[i].AsString };

                                return new JSONResult(JSONErrorCode.InvalidProcedureArgument, validations, true).GetJSONResponseAsStream();
                            }
                            else
                            {
                                if (res.Message != "") return new JSONResult(JSONErrorCode.InvalidProcedureArgument, res.Message).GetJSONResponseAsStream();
                                else return new JSONResult(JSONErrorCode.InvalidProcedureArgument).GetJSONResponseAsStream();
                            }

                        case TReportExecError.UnspecifiedError:
                            return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();

                        default:
                            return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();
                    }                  
                }
                catch (Exception exc)
                {
                    return new JSONResult(JSONErrorCode.InternalError).GetJSONResponseAsStream();
                }
            }
            finally
            {
                app.ReleaseContext();
            }
        }
    }
}
