using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business.JSONObjects
{
    public enum JSONErrorCode
    {
        Success,
        ProcedureNotFound,
        InvalidProcedureContext,
        ProcedureArgumentMissing,  
        InvalidProcedureArgument,
        InternalError,
        DatabaseError,
        WrongMethodCall,
        WrongRequestFormat,
        SecurityAuditFailed
    }
    
    public class JSONError
    {
        public static string GetErrorString(JSONErrorCode ErrorCode)
        {
            switch (ErrorCode)
            {
                case JSONErrorCode.Success:
                    return "";
                case JSONErrorCode.ProcedureNotFound:
                    return "Procedure not defined.";
                case JSONErrorCode.InvalidProcedureContext:
                    return "Invalid Procedure Context.";
                case JSONErrorCode.ProcedureArgumentMissing:
                    return "Procedure Argument Missing.";
                case JSONErrorCode.InvalidProcedureArgument:
                    return "Procedure Argument is Invalid.";
                case JSONErrorCode.InternalError:
                    return "Internal Error Encountered";
                case JSONErrorCode.DatabaseError:
                    return "Error at database level.";
                case JSONErrorCode.WrongMethodCall:
                    return "Wrong Method Call. Try /capabilities method";
                case JSONErrorCode.WrongRequestFormat:
                    return "Wrong Request Format.";
                case JSONErrorCode.SecurityAuditFailed:
                    return "Security Audit Failed";
                default:
                    return "Undefined Error Code String";
            }
        }
    }
}