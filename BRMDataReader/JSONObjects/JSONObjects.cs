using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Business;
using Business.Common;

namespace Business.JSONObjects
{    

    public class JSONMethod
    {
        public string MethodName;
        public string[] MethodArguments;
        public string CallFormat;
    }

    public class JSONProcedureCollection
    {
        public string Collection;
        public string[] Procedures;
    }

    public class JSONKeyValuePair
    {
        public string Key;
        public string Value;
    }

    public class JSONLogin
    {
        public string LoginName;
        public string LoginPassword;
        public string EntryPoint;
        public string CertificateFingerprint;
        public int ID_UserRole;
        public string Language;

        public JSONLogin(Dictionary<string, object> data)
        {
            this.LoginName = "";
            this.LoginPassword = "";
            this.EntryPoint = "";
            this.CertificateFingerprint = "";
            this.ID_UserRole = 0;
            this.Language = "";

            if (data.ContainsKey("LoginName")) this.LoginName = data["LoginName"].ToString();
            if (data.ContainsKey("LoginPassword")) this.LoginPassword = data["LoginPassword"].ToString();
            if (data.ContainsKey("EntryPoint")) this.EntryPoint = data["EntryPoint"].ToString();
            if (data.ContainsKey("CertificateFingerprint")) this.CertificateFingerprint = data["CertificateFingerprint"].ToString();
            if (data.ContainsKey("ID_UserRole")) this.ID_UserRole = (int)data["ID_UserRole"];
            if (data.ContainsKey("Language")) this.Language = data["Language"].ToString();
        }

        public JSONLogin(Newtonsoft.Json.Linq.JObject data)
        {
            this.LoginName = "";
            this.LoginPassword = "";
            this.EntryPoint = "";
            this.CertificateFingerprint = "";
            this.ID_UserRole = 0;
            this.Language = "";

            //if (data.ContainsKey("LoginName")) this.LoginName = data["LoginName"].ToString();
            //if (data.ContainsKey("LoginPassword")) this.LoginPassword = data["LoginPassword"].ToString();
            //if (data.ContainsKey("CertificateFingerprint")) this.CertificateFingerprint = data["CertificateFingerprint"].ToString();

            if (data["LoginName"] != null) this.LoginName = data["LoginName"].ToString();
            if (data["LoginPassword"] != null) this.LoginPassword = data["LoginPassword"].ToString();
            if (data["EntryPoint"] != null) this.EntryPoint = data["EntryPoint"].ToString();
            if (data["CertificateFingerprint"] != null) this.CertificateFingerprint = data["CertificateFingerprint"].ToString();
            if (data["ID_UserRole"] != null)
                if (data["ID_UserRole"].Type != Newtonsoft.Json.Linq.JTokenType.Null)
                    this.ID_UserRole = (int)data["ID_UserRole"].ToObject(typeof(Int32));
            if (data["Language"] != null) this.Language = data["Language"].ToString();
        }
    }

    public class JSONLoginResult
    {
        public bool Success;
        public string Error;
        public DataRow User;

        public JSONLoginResult(bool Success, string Error, DataRow User)
        {
            this.Success = Success;
            this.Error = Error;
            this.User = User;
        }

        internal class JSONDataRowConverter : JavaScriptConverter
        {
            public override IEnumerable<Type> SupportedTypes
            {
                get { return new Type[] { typeof(DataRow) }; }
            }

            public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
            {
                DataRow dataRow = obj as DataRow;
                Dictionary<string, object> propValues = new Dictionary<string, object>();

                if (dataRow != null)
                {
                    foreach (DataColumn dc in dataRow.Table.Columns)
                    {
                        propValues.Add(dc.ColumnName, dataRow[dc]);
                    }
                }

                return propValues;
            }
        }

        public string Serialize()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<JavaScriptConverter> converters = new List<JavaScriptConverter>();
            converters.Add(new JSONDataRowConverter());
            serializer.RegisterConverters(converters);
            return serializer.Serialize(this);
        }
    }

    public class JSONFile
    {
        public string FileName;
        public string Base64Data;
    }

    public class JSONRequest
    {
        public string RequestBody;
        public string SessionId;
        public string CurrentState;
        public Object[] objects;

        public JSONRequest()
        {
        }

        public static JSONRequest Decode()
        {
            System.Net.ServicePointManager.Expect100Continue = false;

            //  get the request string
            string str_request = "";
            try
            {
                using (var reader = OperationContext.Current.RequestContext.RequestMessage.GetReaderAtBodyContents())
                {
                    reader.ReadStartElement("Binary");
                    byte[] b_array = reader.ReadContentAsBase64();
                    str_request = Encoding.UTF8.GetString(b_array);

                    /*//  write to log
                    using (StreamWriter w = File.AppendText(path))
                    {
                        w.WriteLine("Decoded request:");
                        w.WriteLine(str_request);
                        w.WriteLine("");
                    }*/
                }
            }
            catch (Exception exc)
            {
                return null;
            }

            if (str_request == "") return null;

            //  deserialize str_request
            JSONRequest json = null;
            try
            {
                //Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                json = (JSONRequest)JSONInterface.Deserialize(str_request, typeof(JSONRequest));
                json.RequestBody = str_request;
                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                //json = serializer.Deserialize<JSONRequest>(str_request);
                //var data = serializer.DeserializeObject(str_request);
            }
            catch (Exception exc)
            {
                return null;
            }

            return json;
        }

        public object GetObject(string Name)
        {
            if (this.objects == null) return null;

            bool b = false;
            int i = -1;
            //Dictionary<string, object> obj = null;
            Newtonsoft.Json.Linq.JObject obj = null;
            while (!b && i < this.objects.Length - 1)
            {
                i++;

                //obj = (Dictionary<string, object>)this.objects[i];
                obj = (Newtonsoft.Json.Linq.JObject)this.objects[i];
                //if (obj.ContainsKey(Name)) b = true;
                if (obj[Name] != null) b = true;
            }

            if (b) return obj[Name];
            else return null;
        }

        public JSONLogin GetLogin()
        {
            //Dictionary<string, object> obj = (Dictionary<string, object>)GetObject("Login");
            Newtonsoft.Json.Linq.JObject obj = (Newtonsoft.Json.Linq.JObject)GetObject("Login");
            if (obj == null) return null;
            return new JSONLogin(obj);
        }

        public string GetCheckState()
        {
            Newtonsoft.Json.Linq.JToken obj = (Newtonsoft.Json.Linq.JToken)GetObject("CheckState");
            if (obj == null) return "";

            if (obj.Type == Newtonsoft.Json.Linq.JTokenType.String)
            {
                return (string)obj.ToObject(typeof(String));
            }
            else return "";
        }

        public string GetCheckStateOperation()
        {
            Newtonsoft.Json.Linq.JToken obj = (Newtonsoft.Json.Linq.JToken)GetObject("CheckStateOperation");
            if (obj == null) return "";

            if (obj.Type == Newtonsoft.Json.Linq.JTokenType.String)
            {
                return (string)obj.ToObject(typeof(String));
            }
            else return "";
        }

        public TVariantList GetArguments()
        {
            TVariantList vl_arguments = new TVariantList();
            //Dictionary<string, object> obj = (Dictionary<string, object>)GetObject("Arguments");
            Newtonsoft.Json.Linq.JObject obj = (Newtonsoft.Json.Linq.JObject)GetObject("Arguments");
            if (obj == null) return vl_arguments;

            foreach (KeyValuePair<string, Newtonsoft.Json.Linq.JToken> entry in obj)
            {
                string str_name = entry.Key;
                Newtonsoft.Json.Linq.JToken obj_value = entry.Value;

                switch (obj_value.Type)
                {
                    case Newtonsoft.Json.Linq.JTokenType.Boolean:
                        bool b = (bool)obj_value.ToObject(typeof(Boolean));
                        vl_arguments.Add(str_name).AsBoolean = b;
                        break;
                    case Newtonsoft.Json.Linq.JTokenType.Integer:
                        int i = (int)obj_value.ToObject(typeof(Int32));
                        vl_arguments.Add(str_name).AsInt32 = i;
                        break;
                    case Newtonsoft.Json.Linq.JTokenType.Float:
                        double d = (double)obj_value.ToObject(typeof(Double));
                        vl_arguments.Add(str_name).AsDouble = d;
                        break;
                    case Newtonsoft.Json.Linq.JTokenType.Date:
                        DateTime dt = (DateTime)obj_value.ToObject(typeof(DateTime));
                        vl_arguments.Add(str_name).AsDateTime = dt;
                        break;
                    case Newtonsoft.Json.Linq.JTokenType.String:
                        string s = (string)obj_value.ToObject(typeof(String));
                        vl_arguments.Add(str_name).AsString = s;
                        break;
                    case Newtonsoft.Json.Linq.JTokenType.Object:
                        TVariantList vl_struct = new TVariantList();
                        foreach (Newtonsoft.Json.Linq.JToken item in obj_value.Children())
                        {
                            switch (item.Type)
                            {
                                case Newtonsoft.Json.Linq.JTokenType.Property:
                                    Newtonsoft.Json.Linq.JProperty property = item as Newtonsoft.Json.Linq.JProperty;
                                    string item_name = property.Name;
                                    switch (property.Value.Type)
                                    {
                                        case Newtonsoft.Json.Linq.JTokenType.Boolean:
                                            vl_struct.Add(item_name).AsBoolean = (bool)property.Value.ToObject(typeof(Boolean));
                                            break;
                                        case Newtonsoft.Json.Linq.JTokenType.Integer:
                                            vl_struct.Add(item_name).AsInt32 = (int)property.Value.ToObject(typeof(Int32));
                                            break;
                                        case Newtonsoft.Json.Linq.JTokenType.Float:
                                            vl_struct.Add(item_name).AsDouble = (double)property.Value.ToObject(typeof(Double));
                                            break;
                                        case Newtonsoft.Json.Linq.JTokenType.Date:
                                            vl_struct.Add(item_name).AsDateTime = (DateTime)property.Value.ToObject(typeof(DateTime));
                                            break;
                                        case Newtonsoft.Json.Linq.JTokenType.String:
                                            vl_struct.Add(item_name).AsString = (string)property.Value.ToObject(typeof(String));
                                            break;
                                    }
                                    break;
                            }
                        }
                        vl_arguments.Add(str_name).AsObject = vl_struct;
                        break;
                    case Newtonsoft.Json.Linq.JTokenType.Array:
                        TVariantList vl_params = new TVariantList();
                        int idx = 0;
                        foreach (Newtonsoft.Json.Linq.JToken item in obj_value.Children())
                        {
                            string item_Key = idx.ToString();
                            switch (item.Type)
                            {
                                case Newtonsoft.Json.Linq.JTokenType.Boolean:
                                    vl_params.Add(item_Key).AsBoolean = (bool)item.ToObject(typeof(Boolean));
                                    break;
                                case Newtonsoft.Json.Linq.JTokenType.Integer:
                                    vl_params.Add(item_Key).AsInt32 = (int)item.ToObject(typeof(Int32));
                                    break;
                                case Newtonsoft.Json.Linq.JTokenType.Float:
                                    vl_params.Add(item_Key).AsDouble = (double)item.ToObject(typeof(Double));
                                    break;
                                case Newtonsoft.Json.Linq.JTokenType.Date:
                                    vl_params.Add(item_Key).AsDateTime = (DateTime)item.ToObject(typeof(DateTime));
                                    break;
                                case Newtonsoft.Json.Linq.JTokenType.String:
                                    vl_params.Add(item_Key).AsString = (string)item.ToObject(typeof(String));
                                    break;
                                case Newtonsoft.Json.Linq.JTokenType.Object:
                                    vl_params.Add(item_Key).AsString = item.ToString();
                                    break;
                            }
                            idx++;
                        }
                        vl_arguments.Add(str_name).AsObject = vl_params;
                        break;
                }
            }
            /*foreach (KeyValuePair<string, object> entry in obj)
            {
                string str_name = entry.Key;
                object obj_value = entry.Value;

                switch (obj_value.GetType().Name)
                {
                    case "Boolean":
                        vl_arguments.Add(str_name).AsBoolean = (bool)obj_value;
                        break;
                    case "Int32":
                        vl_arguments.Add(str_name).AsInt32 = (int)obj_value;
                        break;
                    case "DateTime":
                        vl_arguments.Add(str_name).AsDateTime = (DateTime)obj_value;
                        break;
                    case "String":
                        vl_arguments.Add(str_name).AsString = (string)obj_value;
                        break;
                }
            }*/

            return vl_arguments;
        }
    }

    public class JSONResult
    {
        public bool Success { get; set; }
        public JSONErrorCode ErrorCode { get; set; }
        public string ResultType { get; set; }
        public object Result { get; set; }

        public JSONResult(JSONErrorCode ErrorCode, string Message)
        {
            this.Success = false;
            if (ErrorCode == JSONErrorCode.Success) this.Success = true;
            this.ErrorCode = ErrorCode;
            this.ResultType = "String";
            this.Result = Message;
        }

        public JSONResult(JSONErrorCode ErrorCode)
        {
            this.Success = false;
            if (ErrorCode == JSONErrorCode.Success) this.Success = true;
            this.ErrorCode = ErrorCode;
            this.ResultType = "String";
            this.Result = "";
        }

        public JSONResult(bool Value)
        {
            this.Success = true;
            this.ErrorCode = JSONErrorCode.Success;
            this.ResultType = "Bool";
            this.Result = Value;
        }

        public JSONResult(int Value)
        {
            this.Success = true;
            this.ErrorCode = JSONErrorCode.Success;
            this.ResultType = "Int32";
            this.Result = Value;
        }

        public JSONResult(string Value)
        {
            this.Success = true;
            this.ErrorCode = JSONErrorCode.Success;
            this.ResultType = "String";
            this.Result = Value;
        }

        public JSONResult(string[] Value)
        {
            this.Success = true;
            this.ErrorCode = JSONErrorCode.Success;
            this.ResultType = "String[]";
            this.Result = Value;
        }

        public JSONResult(DateTime Value)
        {
            this.Success = true;
            this.ErrorCode = JSONErrorCode.Success;
            this.ResultType = "DateTime";
            
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            using (StringWriter wr = new StringWriter())
            {
                serializer.Serialize(wr, Value);
                this.Result = wr.ToString();
            }
        }

        public JSONResult(JSONMethod[] methods)
        {
            this.Success = true;
            this.ErrorCode = JSONErrorCode.Success;
            this.ResultType = "JSONMethod[]";

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            this.Result = serializer.Serialize(methods);
        }

        public JSONResult(JSONProcedureCollection[] collections)
        {
            this.Success = true;
            this.ErrorCode = JSONErrorCode.Success;
            this.ResultType = "JSONProcedureCollection[]";

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            this.Result = serializer.Serialize(collections);
        }

        public JSONResult(JSONFile filerec)
        {
            this.Success = true;
            this.ErrorCode = JSONErrorCode.Success;
            this.ResultType = "JSONFile";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            this.Result = serializer.Serialize(filerec);
        }

        public JSONResult(JSONErrorCode ErrorCode, JSONKeyValuePair[] v, bool asStruct = false)
        {
            if (ErrorCode == JSONErrorCode.Success) this.Success = true;
            else this.Success = false;

            this.ErrorCode = ErrorCode;

            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            switch (asStruct)
            {
                case false:
                    this.ResultType = "JSONKeyValuePair[]";

                    using (StringWriter wr = new StringWriter())
                    {
                        serializer.Serialize(wr, v);
                        this.Result = wr.ToString();
                    }

                    break;
                case true:
                    this.ResultType = "JSONKeyValuePairStruct";

                    this.Result = "{";
                    for (int i = 0; i < v.Length; i++)
                    {
                        using (StringWriter wr = new StringWriter())
                        {
                            serializer.Serialize(wr, v[i].Value);
                            string s = wr.ToString();
                            if (i == 0) this.Result += '"' + v[i].Key + "\":" + s;
                            else this.Result += ", \"" + v[i].Key + "\":" + s;
                        }
                    }
                    this.Result += "}";

                    break;
            }

        }

        public JSONResult(DataSet ds)
        {
            this.Success = true;
            this.ErrorCode = JSONErrorCode.Success;
            this.ResultType = "DataSet";

            JSONDataSet jsonds = new JSONDataSet(ds);
            this.Result = jsonds.Serialize();
        }

        public JSONResult(DataRow row)
        {
            this.Success = true;
            this.ErrorCode = JSONErrorCode.Success;
            this.ResultType = "DataRow";

            JSONDataRow jsrow = new JSONDataRow(row);
            this.Result = jsrow.Serialize();
        }

        public JSONResult(JSONLoginResult LoginResult)
        {
            this.Success = true;
            this.ErrorCode = JSONErrorCode.Success;
            this.ResultType = "LoginResult";

            this.Result = LoginResult.Serialize();
        }

        public JSONResult(JSONForm form)
        {
            this.Success = true;
            this.ErrorCode = JSONErrorCode.Success;
            this.ResultType = "JSONForm";

            this.Result = form.Serialize();
        }

        public string GetJSONResponseAsString()
        {
            string result = "{\"Success\": " + this.Success.ToString().ToLower() + ", \"ErrorCode\": " + ((int)this.ErrorCode).ToString() + ", \"ResultType\": \"" + this.ResultType.ToString() + "\", ";
            switch (this.ResultType)
            {
                case "Bool":
                    result += "\"Result\": " + ((bool)this.Result).ToString().ToLower() + "}";
                    break;
                case "Int32":
                    result += "\"Result\": " + ((int)this.Result).ToString() + "}";
                    break;
                case "String":
                    result += "\"Result\": \"" + JSONError.GetErrorString(this.ErrorCode) + this.Result + "\"}";
                    break;
                case "String[]":
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string s = serializer.Serialize(this.Result);
                    result += "\"Result\": " + s + "}";
                    break;
                case "DateTime":
                    result += "\"Result\": " + this.Result.ToString() + "}";
                    break;
                case "JSONKeyValuePair[]":
                case "JSONKeyValuePairStruct":
                    result += "\"Result\": " + this.Result + "}";
                    break;
                case "DataRow":
                case "DataSet":
                case "JSONMethod[]":
                case "JSONProcedureCollection[]":      
                case "JSONFile":
                case "LoginResult":
                case "JSONForm":
                    result += "\"Result\": " + this.Result + "}";
                    break;
                default:
                    result += "\"Result\": \"" + JSONError.GetErrorString(this.ErrorCode) + this.Result + "\"}";
                    break;
            }
            return result;
        }

        public MemoryStream GetJSONResponseAsStream()
        {
            string s = this.GetJSONResponseAsString();

            //if (Session.debugSessionStreams) Session.Log(s);

            MemoryStream ms = new MemoryStream();
            StreamWriter writer = new StreamWriter(ms);
            writer.Write(s);
            writer.Flush();
            ms.Position = 0;

            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return ms;
        }
    }

}