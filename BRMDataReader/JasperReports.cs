using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net;
using System.Xml;
using System.IO;
using System.Collections;

using Business;
using Business.Common;
using Business.DataModule;
using Business.JSONObjects;



namespace BRMDataReader
{
    public class JasperReports
    {
        //string reportName = "Report1";
        //private string reportName = "";
        private string serverProtocol = "";
        private string serverIP = "";
        private string serverPort = "";
        private string jasperServer = "";
        private string jasperUsername = "";
        private string jasperPassword = "";

        private string ReportServicePath = "";
        private string ReportsFolder = "";
        private string ResourcesServicePath = "";
        //private string salePath = "";

        private string urlReportService = "";
        private string urlResourcesService = "";

        /*private string urlSALE = "";
        private string urlResourceSALE = "";*/

        XmlDocument doc = new XmlDocument();

        public JasperReports()
        {
            init();
        }

        private void init()
        {
            doc = getJasperConfig();

            XmlNode node = doc.DocumentElement.SelectSingleNode("SERVER_PROTOCOL");
            serverProtocol = node.InnerText;

            node = doc.DocumentElement.SelectSingleNode("SERVER_IP");
            serverIP = node.InnerText;

            node = doc.DocumentElement.SelectSingleNode("SERVER_PORT");
            serverPort = node.InnerText;

            node = doc.DocumentElement.SelectSingleNode("JASPER_SERVER");
            jasperServer = node.InnerText;

            node = doc.DocumentElement.SelectSingleNode("JASPER_USERNAME");
            jasperUsername = node.InnerText;

            node = doc.DocumentElement.SelectSingleNode("JASPER_PASSWORD");
            jasperPassword = node.InnerText;

            node = doc.DocumentElement.SelectSingleNode("ReportServicePath");
            ReportServicePath = node.InnerText;

            node = doc.DocumentElement.SelectSingleNode("ReportsFolder");
            ReportsFolder = node.InnerText;

            node = doc.DocumentElement.SelectSingleNode("ResourcesServicePath");
            ResourcesServicePath = node.InnerText;

            /*node = doc.DocumentElement.SelectSingleNode("REPORTS_BASIC_PATH");
            reportsBasicPath = node.InnerText;

            node = doc.DocumentElement.SelectSingleNode("SALE_PATH");
            salePath = node.InnerText;

            node = doc.DocumentElement.SelectSingleNode("RESOURCE_PATH");
            resourcePath = node.InnerText;*/

            urlReportService = serverProtocol + "://" + serverIP + ":" + serverPort + "/" + jasperServer + ReportServicePath;
            urlResourcesService = serverProtocol + "://" + serverIP + ":" + serverPort + "/" + jasperServer + ResourcesServicePath;

            /*urlSALE = urlRestReport + salePath;
            urlResourceSALE = serverProtocol + "://" + serverIP + ":" + serverPort + "/" + jasperServer + resourcePath + salePath;*/
        }

        public TReportExecResult getReport(string ReportPath, string ReportID, TVariantList vl_Params)
        {
            TReportExecResult res = verifyParams(ReportPath, ReportID, vl_Params);
            switch (res.ErrorCode)
            {
                case TReportExecError.Success:                    
                    break;                
                default:
                    return res;
            }

            //1. create webclient
            WebClient webClient = new WebClient();
            webClient.Credentials = new NetworkCredential(jasperUsername, jasperPassword);
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            //2. create request arguments
            string uriString = /*"/Reports/"*/ReportsFolder + "/" + ReportPath + "/" + ReportID;
            string requestXml = "";
            requestXml = "<resourceDescriptor name='" + ReportID + "' wsType='reportUnit' uriString='" + uriString + "' isNew='false'>";           
            requestXml += getReportParams(vl_Params);
            requestXml += "</resourceDescriptor>";

            //3. run report
            string requestAllResult = "";
            try
            {
                requestAllResult = webClient.UploadString(urlReportService + uriString, "PUT", requestXml);
            }
            catch (Exception ex)
            {
                return new TReportExecResult(TReportExecError.ValidationUnsuccesful, "The server could not be reached!");
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(requestAllResult);
            XmlNode node = doc.DocumentElement.SelectSingleNode("uuid");
            string uuid = node.InnerText;
            string session = webClient.ResponseHeaders.Get("Set-Cookie");
            webClient.Headers.Add("Cookie", session);

            //4. generate pdf
            string reportUrl = urlReportService;
            reportUrl += "/" + uuid;
            reportUrl += "?file=report";
            byte[] reportData;
            try
            {
                reportData = webClient.DownloadData(reportUrl);
            }
            catch (Exception ex)
            {
                return new TReportExecResult(TReportExecError.ValidationUnsuccesful, "The report could not be downloaded!");
            }
            //webClient.DownloadFile(reportUrl, @"D:\file.pdf");

            //5. encode and return
            string Base64Data = Convert.ToBase64String(reportData); //Base64Encode(reportData);
            res = new TReportExecResult(TReportExecError.Success, "");
            res.FileName = ReportID + ".pdf";
            res.Base64Data = Base64Data;
            return res;
        }

        private TReportExecResult verifyParams(string ReportPath, string ReportID, TVariantList vlParams)
        {
            //1. verify if vlParams contains the report name
            //if (vlParams["ReportName"] == null) return new TReportExecResult(TReportExecError.ArgumentMissing, "ReportName");            

            //2. verify if the report name exist in xml repository
            //reportName = vlParams["ReportName"].Value.ToString().ToUpper();
            XmlNode node = doc.DocumentElement.SelectSingleNode("REPORTS");
            XmlNode nodeReport = node.SelectSingleNode(ReportID.ToUpper());
            if (nodeReport == null) return new TReportExecResult(TReportExecError.ValidationUnsuccesful, "ReportID not configured in local configuration file");

            //3. verify parameters
            TReportExecResult res = new TReportExecResult(TReportExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vlParams);

            bool found = false;

            XmlNodeList prmList = nodeReport.SelectNodes("PARAM");
            for (int i = 0; i < prmList.Count; i++)
            {
                found = false;
                string paramName = prmList[i].Attributes["name"].Value.ToString();
                for (int j = 0; j < vlParams.Count; j++)
                {
                    string prmName = vlParams[j].Name;
                    string prmValue = vlParams[j].Value.ToString();
                    if (prmName.ToUpper().Equals(paramName.ToUpper()))
                    {
                        found = true;
                        string type = prmList[i].Attributes["type"].Value.ToString();
                        string value = vlParams[j].Value.ToString();
                        TVariantType types = vlParams[j].ValueType;
                        //just because i don't fucking understand right now why all value types are string
                        continue;
                        string argumentType = "";
                        switch (type)
                        {
                            case "int":
                                if (vlParams[j].ValueType == TVariantType.vtInt16 ||
                                    vlParams[j].ValueType == TVariantType.vtInt32 ||
                                    vlParams[j].ValueType == TVariantType.vtInt64)
                                    argumentType = "";
                                else
                                    res.ParamValidations[prmName].AsString = "Unrecognized Value";
                                break;

                            case "date":
                                if (vlParams[j].ValueType == TVariantType.vtDateTime)
                                    argumentType = "";
                                else
                                    res.ParamValidations[prmName].AsString = "Unrecognized Value";
                                break;

                            case "double":
                                if (vlParams[j].ValueType == TVariantType.vtInt16 ||
                                    vlParams[j].ValueType == TVariantType.vtInt32 ||
                                    vlParams[j].ValueType == TVariantType.vtInt64 ||
                                    vlParams[j].ValueType == TVariantType.vtDouble)
                                    argumentType = "";
                                else
                                    res.ParamValidations[prmName].AsString = "Unrecognized Value";
                                break;
                        }
                        break;
                    }
                }            
                if(!found)
                    return new TReportExecResult(TReportExecError.ArgumentMissing, paramName);
            }

            if (res.bInvalid()) return res;

            res = new TReportExecResult(TReportExecError.Success, "");
            return res;
        }

        private string getReportParams(TVariantList vlParams)
        {
            string requestXml = "";
            for (int i = 0; i < vlParams.Count; i++)
            {
                string name = vlParams[i].Name;
                string value = vlParams[i].Value.ToString();
                if (!name.Equals("ReportName"))
                {
                    requestXml+= "<parameter name='" + name + "'>" + value + "</parameter>";
                }
            }

            return requestXml;
        }

        public string[] EnumerateCollections()
        {
            WebClient webClient = new WebClient();
            webClient.Credentials = new NetworkCredential(jasperUsername, jasperPassword);
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            string str_response = "";
            try
            {
                str_response = webClient.DownloadString(urlResourcesService + "/Reports");
            }
            catch { return null; }

            XmlDocument xmlCollections = new XmlDocument();
            List<string> lst = new List<string>();
            try
            {
                xmlCollections.LoadXml(str_response);
                XmlNodeList elemList = xmlCollections.GetElementsByTagName("resourceDescriptor");
                for (int i = 0; i < elemList.Count; i++)
                {
                    string resName = elemList[i].Attributes["name"].Value;
                    string resType = elemList[i].Attributes["wsType"].Value;

                    if (resType == "folder") lst.Add(resName);
                }

                return lst.ToArray(); 
            }
            catch { return null; }
        }

        public string[] EnumerateReports(string ReportPath)
        {
            WebClient webClient = new WebClient();
            webClient.Credentials = new NetworkCredential(jasperUsername, jasperPassword);
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            string str_response = "";
            try
            {
                str_response = webClient.DownloadString(urlResourcesService + "/Reports/" + ReportPath);
            }
            catch { return null; }

            XmlDocument xmlCollections = new XmlDocument();
            List<string> lst = new List<string>();
            try
            {
                xmlCollections.LoadXml(str_response);
                XmlNodeList elemList = xmlCollections.GetElementsByTagName("resourceDescriptor");
                for (int i = 0; i < elemList.Count; i++)
                {
                    string resName = elemList[i].Attributes["name"].Value;
                    string resType = elemList[i].Attributes["wsType"].Value;

                    if (resType == "reportUnit") lst.Add(resName);
                }

                return lst.ToArray();
            }
            catch { return null; }
        }

        public JSONProcedureCollection[] EnumerateParameters()
        {          
            Dictionary<string, List<string>> reports = new Dictionary<string, List<string>>();
            //1. create webclient
            WebClient webClient = new WebClient();
            webClient.Credentials = new NetworkCredential(jasperUsername, jasperPassword);
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            string requestAllResult = webClient.DownloadString(urlResourcesService + "/Reports");
            XmlDocument jasperConfig = getJasperConfig();
            XmlDocument repositoryReports = new XmlDocument();
            repositoryReports.LoadXml(requestAllResult);
            XmlNodeList elemList = repositoryReports.GetElementsByTagName("resourceDescriptor");
            for (int i = 0; i < elemList.Count; i++)
            {
                string rName = elemList[i].Attributes["name"].Value;

                XmlNode nodeReports = jasperConfig.DocumentElement.SelectSingleNode("REPORTS");
                XmlNode searchedReport = nodeReports.SelectSingleNode(rName);
                XmlNodeList paramsList = searchedReport.SelectNodes("PARAM");

                List<string> list = new List<string>();
                for (int j = 0; j < paramsList.Count; j++)
                { 
                    list.Add(paramsList[j].Attributes["name"].Value.ToString());
                }
                reports.Add(rName, list);                
            }

            string[] collections = new string[reports.Count];
            List<ArrayList> procedures = new List<ArrayList>();
            int nr = 0;
            foreach (KeyValuePair<string, List<string>> entry in reports)
            {
                collections[nr] = entry.Key;
                List<string> list = entry.Value;
                ArrayList arr = new ArrayList();
                for (int i = 0; i < list.Count; i++)
                {
                    arr.Add(list[i]);
                }                
                nr++;
                procedures.Add(arr);
            }

            JSONProcedureCollection[] jsoncollections = new JSONProcedureCollection[collections.Length];
            for (int i = 0; i < jsoncollections.Length; i++)
                jsoncollections[i] = new JSONProcedureCollection() { Collection = collections[i], Procedures = (string[])procedures[i].ToArray(typeof(string)) };

            return jsoncollections;
        }

        private XmlDocument getJasperConfig()
        {
            string FServerPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
            if (!FServerPath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                FServerPath += System.IO.Path.DirectorySeparatorChar;

            string jasperConfigPath = FServerPath + "JasperConfig.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(jasperConfigPath);

            return doc;
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }

    //  result structure for report execution
    public enum TReportExecError
    {
        Success,
        ArgumentMissing,
        ValidationUnsuccesful,
        UnspecifiedError
    }

    public struct TReportExecResult
    {
        public TReportExecError ErrorCode;
        public string Message;
        public TVariantList ParamValidations;
        public string FileName;
        public string Base64Data;

        public TReportExecResult(TReportExecError ErrorCode, string Message)
        {
            this.ErrorCode = ErrorCode;
            this.Message = Message;
            this.ParamValidations = null;
            this.FileName = "";
            this.Base64Data = "";
        }

        public TReportExecResult(TReportExecError ErrorCode, string Message, string Base64Data)
        {
            this.ErrorCode = ErrorCode;
            this.Message = Message;
            this.ParamValidations = null;
            this.FileName = "";
            this.Base64Data = Base64Data;
        }

        public void setParamValidations(TVariantList vl_params)
        {
            ParamValidations = new TVariantList();
            for (int i = 0; i < vl_params.Count; i++) ParamValidations.Add(vl_params[i].Name).AsString = "";
        }

        public bool bInvalid()
        {
            if (ParamValidations == null) return false;

            bool b = false;
            int i = -1;
            while (!b && i < ParamValidations.Count - 1)
            {
                i++;
                if (ParamValidations[i].AsString != "") b = true;
            }

            return b;
        }
    }

}