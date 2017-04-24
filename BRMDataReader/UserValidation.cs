using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Business.Common;
using Business.JSONObjects;

namespace Business
{
    public class UserValidation
    {
        private TBusiness app = null;

        public UserValidation(TBusiness app)
        {
            this.app = app;
        }

        private JSONErrorCode FLastError = JSONErrorCode.Success;
        public JSONErrorCode LastError
        {
            get { return FLastError; }
            set {}
        }

        protected object SetReturn(JSONErrorCode ErrorCode, object Value)
        {
            FLastError = ErrorCode;
            return Value;
        }

        public bool isAdministrator(int ID_Bursary, int ID_User, int ID_UserRole)
        {
            if (app == null) return (bool)SetReturn(JSONErrorCode.InternalError, false);

            //  check if current user is administrator
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_User").AsInt32 = ID_User;
            DataSet ds = app.DB.Select("select_CurrentUser", "Users", vl_params);
            if (ds == null) return (bool)SetReturn(JSONErrorCode.DatabaseError, false); //new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
            if (ds.Tables.Count == 0) return (bool)SetReturn(JSONErrorCode.DatabaseError, false); // new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
            if (ds.Tables[0].Rows.Count == 0) return (bool)SetReturn(JSONErrorCode.SecurityAuditFailed, false); //  new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();

            if (!Convert.ToBoolean(ds.Tables[0].Rows[0]["isAdministrator"]))
            {
                vl_params.Add("@prm_ID_Bursary").AsInt32 = ID_Bursary;
                vl_params.Add("@prm_ID_UserRole").AsInt32 = ID_UserRole;
                ds = app.DB.Select("select_UserRoles", "UserRoles", vl_params);
                if (!app.DB.ValidDSRows(ds)) return (bool)SetReturn(JSONErrorCode.SecurityAuditFailed, false);
                if (!Convert.ToBoolean(ds.Tables[0].Rows[0]["isAdministrator"])) return (bool)SetReturn(JSONErrorCode.SecurityAuditFailed, false); 
            }

            return true;
        }

        public bool isSupervisor(int ID_Bursary, int ID_User, int ID_UserRole)
        {
            if (app == null) return (bool)SetReturn(JSONErrorCode.InternalError, false);

            //  check if current user is administrator
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_User").AsInt32 = ID_User;
            DataSet ds = app.DB.Select("select_CurrentUser", "Users", vl_params);
            if (ds == null) return (bool)SetReturn(JSONErrorCode.DatabaseError, false); //new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
            if (ds.Tables.Count == 0) return (bool)SetReturn(JSONErrorCode.DatabaseError, false); // new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
            if (ds.Tables[0].Rows.Count == 0) return (bool)SetReturn(JSONErrorCode.SecurityAuditFailed, false); //  new JSONResult(JSONErrorCode.SecurityAuditFailed).GetJSONResponseAsStream();

            if (!Convert.ToBoolean(ds.Tables[0].Rows[0]["isAdministrator"]))
            {
                vl_params.Add("@prm_ID_Bursary").AsInt32 = ID_Bursary;
                vl_params.Add("@prm_ID_UserRole").AsInt32 = ID_UserRole;
                vl_params.Add("@prm_StateName").AsString = "dashboard_journal";
                ds = app.DB.Select("select_StateAccess", "States", vl_params);
                if (!app.DB.ValidDSRows(ds)) return (bool)SetReturn(JSONErrorCode.SecurityAuditFailed, false);
                //if (!Convert.ToBoolean(ds.Tables[0].Rows[0]["isAdministrator"])) return (bool)SetReturn(JSONErrorCode.SecurityAuditFailed, false);
            }

            return true;
        }
    }
}