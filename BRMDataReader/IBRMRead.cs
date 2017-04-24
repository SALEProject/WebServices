using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;

namespace BRMDataReader
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBRMRead" in both code and config file together.
    [ServiceContract]
    public interface IBRMRead
    {
        //  Default End Point of service
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "*")]
        Stream GET_DefaultEndPoint();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "*")]
        Stream POST_DefaultEndPoint();

        //  Enumerator for error codes
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "errorcodes")]
        Stream GET_ErrorCodes();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "errorcodes")]
        Stream POST_ErrorCodes();

        //  Enumerator for methods supported by the service
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "capabilities")]
        Stream GET_Capabilities();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "capabilities")]
        Stream POST_Capabilities();

        //  Echo the message back to the sender
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "echo")]
        string GET_Echo();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "echo")]
        string POST_Echo();

        //  Get server time
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "servertime")]
        Stream GET_ServerTime();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "servertime")]
        Stream POST_ServerTime();

        //  Enumerate collections and procedures executed by the service
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "enumerate")]
        Stream GET_Enumerate();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "enumerate")]
        Stream POST_Enumerate();

        //  Enumerator for procedure collections
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "enumeratecollections")]
        Stream GET_EnumerateCollections();

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "enumeratecollections")]
        Stream POST_EnumerateCollections();

        //  Enumerator for procedures in a specific collection
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "enumerateprocedures/{collection}")]
        Stream GET_EnumerateProcedures(string collection);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "enumerateprocedures/{collection}")]
        Stream POST_EnumerateProcedures(string collection);

        //  Make a select operation (execute a procedure)
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "select/{collection}/{procedure}")]
        Stream GET_Select(string collection, string procedure);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "select/{collection}/{procedure}")]
        Stream POST_Select(string collection, string procedure);

    }
}
