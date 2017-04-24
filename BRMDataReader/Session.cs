using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Data;
using Business;
using Business.Common;
using Business.JSONObjects;

namespace Business
{
    public /*static*/ class Session
    {
        private TBusiness app = null;
        private /*static*/ JSONRequest FRequest;
        private bool FLogRequest;
        private /*static*/ int FID = 0;
        private /*static*/ int FID_User = 0;
        private /*static*/ int FID_Broker = 0;
        private /*static*/ int FID_Agency = 0;
        private int FID_Client = 0;
        private /*static*/ string FSessionId = "";
        private /*static*/ string FCurrentState = "";
        private int FID_UserRole = 0;
        private string FLanguage = "EN";

        private string FEntryPoint = "";
        private int FID_Bursary = 0;

        //  should there be default values for market/ring/asset selection?
        private int FID_DefaultMarket = 0;
        private int FID_DefaultRing = 0;
        private int FID_DefaultAsset = 0;

        private /*static*/ DateTime FLastMessageTimestamp = DateTime.Now;
        private /*static*/ DateTime FLastAlertTimestamp = DateTime.Now;
        private DateTime FLastNotificationTimestamp = DateTime.Now;
        private DateTime FLastEventTimestamp = DateTime.Now;
        private /*static*/ bool FdebugSessionStreams = true;
        private bool FSessionChanged = false;

        public double GetVar(string VarName, string X, string Y, double Default)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_VarName").AsString = VarName;
            vl_params.Add("@prm_X").AsString = X;
            vl_params.Add("@prm_Y").AsString = Y;
            vl_params.Add("@prm_Default").AsDouble = Default;
            DataSet ds = app.DB.Select("GetVar", "sys", vl_params);
            if (ds == null) return Default;
            if (ds.Tables.Count == 0) return Default;
            if (ds.Tables[0].Rows.Count > 0) return Convert.ToDouble(ds.Tables[0].Rows[0]["Value"]);
            return Default;
        }

        public string GetVar(string VarName, string X, string Y, string Default)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_VarName").AsString = VarName;
            vl_params.Add("@prm_X").AsString = X;
            vl_params.Add("@prm_Y").AsString = Y;
            vl_params.Add("@prm_Default").AsString = Default;
            DataSet ds = app.DB.Select("GetVarSTR", "sys", vl_params);
            if (ds == null) return Default;
            if (ds.Tables.Count == 0) return Default;
            if (ds.Tables[0].Rows.Count > 0) return ds.Tables[0].Rows[0]["Value"].ToString();
            return Default;
        }

        public /*static*/ bool Init(string SessionId, string CurrentState, TBusiness app)
        {
            this.app = app;
            if (app.SessionDB == null) return false;  //if (app.DB == null) return false;
            if (SessionId.Trim() == "") return false;

            //  set global params
            //FdebugSessionStreams = Convert.ToBoolean(GetVar("debugSessionStreams", "", "", Convert.ToDouble(false)));

            //  set sessionid
            FSessionId = SessionId;
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_SessionId").AsString = SessionId;
            DataSet ds = app.SessionDB.Select("select_SessionbySessionId", "Sessions", vl_params); //DataSet ds = app.DB.Select("select_SessionbySessionId", "Sessions", vl_params);
            if (ds == null) return false;
            if (ds.Tables.Count == 0) return false;

            if (ds.Tables[0].Rows.Count == 0)
            {
                FID = CreateSession();
                return false;
            }
            else
            {
                FSessionChanged = false;

                FID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                FID_User = Convert.ToInt32(ds.Tables[0].Rows[0]["ID_User"]);
                FID_UserRole = Convert.ToInt32(ds.Tables[0].Rows[0]["ID_UserRole"]);
                FCurrentState = ds.Tables[0].Rows[0]["CurrentState"].ToString();
                if (ds.Tables[0].Rows[0]["EntryPoint"] != DBNull.Value)
                    FEntryPoint = ds.Tables[0].Rows[0]["EntryPoint"].ToString();
                FID_Bursary = Convert.ToInt32(ds.Tables[0].Rows[0]["ID_Bursary"]);
                FID_Broker = Convert.ToInt32(ds.Tables[0].Rows[0]["ID_Broker"]);
                FID_Agency = Convert.ToInt32(ds.Tables[0].Rows[0]["ID_Agency"]);
                FID_Client = Convert.ToInt32(ds.Tables[0].Rows[0]["ID_Client"]);

                if (ds.Tables[0].Rows[0]["LastMessageTimestamp"] != DBNull.Value)
                    FLastMessageTimestamp = Convert.ToDateTime(ds.Tables[0].Rows[0]["LastMessageTimestamp"]);
                else FLastMessageTimestamp = DateTime.Now.Date;

                if (ds.Tables[0].Rows[0]["LastAlertTimestamp"] != DBNull.Value)
                    FLastAlertTimestamp = Convert.ToDateTime(ds.Tables[0].Rows[0]["LastAlertTimestamp"]);
                else FLastAlertTimestamp = DateTime.Now.Date;

                if (ds.Tables[0].Rows[0]["LastNotificationTimestamp"] != DBNull.Value)
                    FLastNotificationTimestamp = Convert.ToDateTime(ds.Tables[0].Rows[0]["LastNotificationTimestamp"]);
                else FLastNotificationTimestamp = DateTime.Now.Date;

                if (ds.Tables[0].Rows[0]["LastEventTimestamp"] != DBNull.Value)
                    FLastEventTimestamp = Convert.ToDateTime(ds.Tables[0].Rows[0]["LastEventTimestamp"]);
                else FLastEventTimestamp = DateTime.Now.Date;

                if (ds.Tables[0].Rows[0]["Language"] != DBNull.Value)
                    FLanguage = ds.Tables[0].Rows[0]["Language"].ToString();
                else FLanguage = "EN";

                app.Database = ds.Tables[0].Rows[0]["Database"].ToString();

                if (app.Database == "") return false;
                else
                    if (!app.InitDB()) return false;
                    else
                    {
                        if (FCurrentState != CurrentState)
                        {
                            //  check if the state is accessible from the previous registered state

                            /*
                            vl_params = new TVariantList();
                            vl_params.Add("@prm_ID_User").AsInt32 = FID_User;
                            vl_params.Add("@prm_CurrentState").AsString = FCurrentState;
                            vl_params.Add("@prm_CheckState").AsString = CurrentState;
                            vl_params.Add("@prm_ID_UserRole").AsInt32 = FID_UserRole;
                            DataSet ds_checkAccess = app.DB.Select("select_CheckState", "States", vl_params);
                            if (app.DB.ValidDSRows(ds_checkAccess)) FCurrentState = CurrentState;
                            */

                            FCurrentState = CurrentState;
                        }
                    }
            }

            if (FdebugSessionStreams && FLogRequest)
            {
                Log(FRequest.RequestBody);
            }

            return true;
        }

        public /*static*/ bool Init(JSONRequest json_request, TBusiness app, bool b_LogRequest = true)
        //public Session(JSONReques json_request, TBusiness app)
        {
            if (json_request == null) return false;
            FRequest = json_request;
            FLogRequest = b_LogRequest;
            return Init(json_request.SessionId, json_request.CurrentState, app);
        }

        private int CreateSession()
        {
            if (app == null) return 0;
            if (app.SessionDB == null) return 0; //if (app.DB == null) return 0;
            //if (FID == 0) return 0;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_SessionId").AsString = FSessionId;
            vl_params.Add("@prm_ID_User").AsInt32 = FID_User;
            vl_params.Add("@prm_ID_UserRole").AsInt32 = FID_UserRole;
            vl_params.Add("@prm_CurrentState").AsString = FCurrentState;
            vl_params.Add("@prm_EntryPoint").AsString = FEntryPoint;
            vl_params.Add("@prm_ID_Bursary").AsInt32 = FID_Bursary;
            vl_params.Add("@prm_LastMessageTimestamp").AsDateTime = FLastMessageTimestamp;
            vl_params.Add("@prm_LastAlertTimestamp").AsDateTime = FLastAlertTimestamp;
            vl_params.Add("@prm_LastNotificationTimestamp").AsDateTime = FLastNotificationTimestamp;
            vl_params.Add("@prm_LastEventTimestamp").AsDateTime = FLastEventTimestamp;
            vl_params.Add("@prm_Language").AsString = FLanguage;
            int res = app.SessionDB.Exec("insert_Session", "Sessions", vl_params);
            if (res > 0) return app.SessionDB.GetIdentity();
            else return 0;
        }

        public bool SaveSession()
        {
            if (!FSessionChanged) return true;

            if (app == null) return false;
            if (app.SessionDB == null) return false; //if (app.DB == null) return false;
            if (FID == 0) return false;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_SessionId").AsString = FSessionId;
            vl_params.Add("@prm_ID_User").AsInt32 = FID_User;
            vl_params.Add("@prm_ID_UserRole").AsInt32 = FID_UserRole;
            vl_params.Add("@prm_CurrentState").AsString = FCurrentState;
            vl_params.Add("@prm_EntryPoint").AsString = FEntryPoint;
            vl_params.Add("@prm_ID_Bursary").AsInt32 = FID_Bursary;
            vl_params.Add("@prm_ID_Broker").AsInt32 = FID_Broker;
            vl_params.Add("@prm_ID_Agency").AsInt32 = FID_Agency;
            vl_params.Add("@prm_ID_Client").AsInt32 = FID_Client;
            vl_params.Add("@prm_LastMessageTimestamp").AsDateTime = FLastMessageTimestamp;
            vl_params.Add("@prm_LastAlertTimestamp").AsDateTime = FLastAlertTimestamp;
            vl_params.Add("@prm_LastNotificationTimestamp").AsDateTime = FLastNotificationTimestamp;
            vl_params.Add("@prm_LastEventTimestamp").AsDateTime = FLastEventTimestamp;
            vl_params.Add("@prm_Language").AsString = FLanguage;
            vl_params.Add("@prm_ID").AsInt32 = FID;
            int res = app.SessionDB.Exec("update_Session", "Sessions", vl_params);
            if (res > 0) return true;
            else return false;
        }

        private bool SetDefaultBursaryParams()
        {
            if (app == null) return false;
            if (app.DB == null) return false;
            if (FID_Bursary == 0) return false;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = FID_Bursary;
            DataSet ds_market = app.DB.Select("select_DefaultMarket", "Markets", vl_params);
            if (ds_market == null) return false;
            if (ds_market.Tables.Count == 0) return false;
            if (ds_market.Tables[0].Rows.Count == 0) return false;

            FID_DefaultMarket = Convert.ToInt32(ds_market.Tables[0].Rows[0]["ID"]);

            vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Market").AsInt32 = FID_DefaultMarket;
            DataSet ds_ring = app.DB.Select("select_DefaultRing", "Rings", vl_params);
            if (ds_ring == null) return false;
            if (ds_ring.Tables.Count == 0) return false;
            if (ds_ring.Tables[0].Rows.Count == 0) return false;

            FID_DefaultRing = Convert.ToInt32(ds_ring.Tables[0].Rows[0]["ID"]);

            vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Ring").AsInt32 = FID_DefaultRing;
            DataSet ds_asset = app.DB.Select("select_DefaultAsset", "Assets", vl_params);
            if (ds_asset == null) return false;
            if (ds_asset.Tables.Count == 0) return false;
            if (ds_asset.Tables[0].Rows.Count == 0) return false;

            FID_DefaultAsset = Convert.ToInt32(ds_asset.Tables[0].Rows[0]["ID"]);

            return true;
        }

        public /*static*/ int ID
        {
            get { return FID; }
            set { }
        }

        public /*static*/ int ID_User
        {
            get { return FID_User; }
            set
            {
                if (value != FID_User) FSessionChanged = true;

                if (FSessionChanged)
                {
                    FID_User = value;

                    //  change agency and broker currently attached to the user
                    FID_UserRole = 0;

                    TVariantList vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_User").AsInt32 = FID_User;
                    DataSet ds_broker = app.DB.Select("select_CurrentUser", "Users", vl_params);
                    if (ds_broker != null)
                        if (ds_broker.Tables.Count > 0)
                            if (ds_broker.Tables[0].Rows.Count > 0)
                            {
                                FID_Broker = Convert.ToInt32(ds_broker.Tables[0].Rows[0]["ID_Broker"]);
                                FID_Agency = Convert.ToInt32(ds_broker.Tables[0].Rows[0]["ID_Agency"]);
                                FID_Client = Convert.ToInt32(ds_broker.Tables[0].Rows[0]["ID_Client"]);
                            }
                }

                SaveSession();
            }
        }

        public /*static*/ int ID_Broker
        {
            get { return FID_Broker; }
            set { }
        }

        public /*static*/ int ID_Agency
        {
            get { return FID_Agency; }
            set { }
        }

        public int ID_Client
        {
            get { return FID_Client; }
            set { }
        }

        public /*static*/ string CurrentState
        {
            get { return FCurrentState; }
            set { }
        }

        public int ID_UserRole
        {
            get { return FID_UserRole; }
            set
            {
                if (value != FID_UserRole) FSessionChanged = true;

                FID_UserRole = value;
                SaveSession();
            }
        }

        public string EntryPoint
        {
            get { return FEntryPoint; }
            set 
            {
                if (value != FEntryPoint) FSessionChanged = true;

                if (FSessionChanged)
                {
                    TVariantList vl_params = new TVariantList();
                    vl_params.Add("@prm_Code").AsString = value;
                    DataSet ds = app.DB.Select("select_BursarybyCode", "Bursaries", vl_params);
                    if (ds == null) return;
                    if (ds.Tables.Count == 0) return;
                    if (ds.Tables[0].Rows.Count == 0) return;

                    FEntryPoint = value;
                    FID_Bursary = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                }

                SaveSession();
                SetDefaultBursaryParams();
            }
        }

        public int ID_Bursary
        {
            get { return FID_Bursary; }
            set
            {
                if (value != FID_Bursary) FSessionChanged = true;

                if (FSessionChanged)
                {
                    TVariantList vl_params = new TVariantList();
                    vl_params.Add("@prm_ID").AsInt32 = value;
                    DataSet ds = app.DB.Select("select_Bursaries", "Bursaries", vl_params);
                    if (ds == null) return;
                    if (ds.Tables.Count == 0) return;
                    if (ds.Tables[0].Rows.Count == 0) return;

                    FID_Bursary = value;
                    if (ds.Tables[0].Rows[0]["Code"] != DBNull.Value)
                        FEntryPoint = ds.Tables[0].Rows[0]["Code"].ToString();
                }

                SaveSession();
                SetDefaultBursaryParams();
            }
        }

        public /*static*/ DateTime LastMessageTimestamp
        {
            get { return FLastMessageTimestamp; }
            set
            {
                if (value != FLastMessageTimestamp) FSessionChanged = true;
             
                FLastMessageTimestamp = value;
                SaveSession();
            }
        }

        public /*static*/ DateTime LastAlertTimestamp
        {
            get { return FLastAlertTimestamp; }
            set
            {
                if (value != FLastAlertTimestamp) FSessionChanged = true;

                FLastAlertTimestamp = value;
                SaveSession();
            }
        }

        public DateTime LastNotificationTimestamp
        {
            get { return FLastNotificationTimestamp; }
            set
            {
                if (value != FLastNotificationTimestamp) FSessionChanged = true;

                FLastNotificationTimestamp = value;
                SaveSession();
            }
        }

        public DateTime LastEventTimestamp
        {
            get { return FLastEventTimestamp; }
            set
            {
                if (value != FLastEventTimestamp) FSessionChanged = true;

                FLastEventTimestamp = value;
                SaveSession();
            }
        }

        public string Language
        {
            get { return FLanguage; }
            set
            {
                if (value != FLanguage) FSessionChanged = true;

                FLanguage = value;
                SaveSession();
            }
        }

        public /*static*/ object GetVar(string Name)
        {
            return null;
        }

        public /*static*/ bool SetVar(string Name, object Value)
        {
            return false;
        }

        public /*static*/ string[] GetAccessibleStates()
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_User").AsInt32 = FID_User;
            vl_params.Add("@prm_CurrentState").AsString = FCurrentState;
            vl_params.Add("@prm_CheckState").AsString = "";
            DataSet ds = app.DB.Select("select_CheckState", "States", vl_params);

            return null;
        }

        public /*static*/ bool CheckAccess(string NewState)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_User").AsInt32 = FID_User;
            vl_params.Add("@prm_CurrentState").AsString = FCurrentState;
            vl_params.Add("@prm_CheckState").AsString = NewState;
            DataSet ds = app.DB.Select("select_CheckState", "States", vl_params);
            //if (ds == null) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
            //if (ds.Tables.Count == 0) return new JSONResult(JSONErrorCode.DatabaseError).GetJSONResponseAsStream();
            return false;
        }

        public /*static*/ bool debugSessionStreams
        {
            get { return FdebugSessionStreams; }
            set { }
        }

        public bool SessionChanged
        {
            get { return FSessionChanged; }
            set { }
        }

        public /*static*/ bool Log(string str_Log)
        {
            if (app.SessionDB == null) return false;
            if (FID == 0) return false;

            string s;
            if (str_Log.Length > 1024) s = str_Log.Substring(0, 512);
            else s = str_Log;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Session").AsInt32 = FID;
            vl_params.Add("@prm_Log").AsString = s;
            int res = app.SessionDB.Exec("insert_SessionLog", "SessionLogs", vl_params);

            return (res == 1);
        }

        public void SetAlert(int ID_Market, int ID_Ring, int ID_Asset, string Message, string Message_RO, string Message_EN)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Market").AsInt32 = ID_Market;
            vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            vl_params.Add("@prm_Message").AsString = Message;
            vl_params.Add("@prm_Message_RO").AsString = Message_RO;
            vl_params.Add("@prm_Message_EN").AsString = Message_EN;
            app.DB.Exec("insert_Alert", "Alerts", vl_params);
        }

        public void SetEvent(int Priority, string Resource, string EventType, int ID_Resource, int ID_LinkedResource)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Priority").AsInt32 = Priority;
            vl_params.Add("@prm_Resource").AsString = Resource;
            vl_params.Add("@prm_EventType").AsString = EventType;
            vl_params.Add("@prm_ID_Resource").AsInt32 = ID_Resource;
            vl_params.Add("@prm_ID_LinkedResource").AsInt32 = ID_LinkedResource;

            app.DB.Exec("insert_Event", "Events", vl_params);
        }

        public void AddToJournal(string Operation, int ID_Client, int ID_Ring, int ID_Asset, int ID_Order, double Quantity, double Price, string Message = "")
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Operation").AsString = Operation;
            vl_params.Add("@prm_ID_User").AsInt32 = FID_User;
            vl_params.Add("@prm_ID_Broker").AsInt32 = FID_Broker;
            vl_params.Add("@prm_ID_Agency").AsInt32 = FID_Agency;
            vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
            vl_params.Add("@prm_ID_Bursary").AsInt32 = ID_Bursary;
            vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            vl_params.Add("@prm_ID_Order").AsInt32 = ID_Order;
            vl_params.Add("@prm_Quantity").AsDouble = Quantity;
            vl_params.Add("@prm_Price").AsDouble = Price;
            vl_params.Add("@prm_Message").AsString = Message;

            app.DB.Exec("insert_Journal", "Journal", vl_params);
        }
    }
}