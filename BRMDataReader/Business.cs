using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business;
using Business.Common;
using Business.DataModule;
using Business.JSONObjects;

namespace Business
{
    public class TBusiness
    {
        private TDBApplicationLayer FSessionDB = null;
        private /*static*/ TDBApplicationLayer FDB = null;
        private /*static*/ string FServerPath = "";
        private string FDatabase = "";

        public bool InitSessionDB()
        {
            if (FSessionDB != null) return true;

            FServerPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
            if (!FServerPath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                FServerPath += System.IO.Path.DirectorySeparatorChar;

            try
            {
                TExceptionManager.ExcManager_File = FServerPath + "WebServices.log";
                TExceptionManager.GetExcManager();
            }
            catch (Exception exc)
            {
                return false;
            }

            try
            {
                FSessionDB = new Business.DataModule.TDBApplicationLayer(Business.DataModule.TDBConnectionType.con_MSSQL, FServerPath + "DB_Session.xml");
                FSessionDB.MergeXML(FServerPath + "BRMSys.xml");

                return true;
            }
            catch (Exception exc)
            {
                TExceptionManager.GetExcManager().ProcessException(exc);
                return false;
            }
        }

        public /*static*/ bool InitDB(bool doConnect = true)
        {
            if (FDB != null) return true;

            FServerPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
            if (!FServerPath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                FServerPath += System.IO.Path.DirectorySeparatorChar;

            try
            {
                TExceptionManager.ExcManager_File = FServerPath + "WebServices.log";
                TExceptionManager.GetExcManager();
            }
            catch (Exception exc)
            {
                return false;
            }

            try
            {
                TVariantList vl_params = null;
                if (FDatabase != "")
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Database").AsString = FDatabase;
                }

                FDB = new Business.DataModule.TDBApplicationLayer(Business.DataModule.TDBConnectionType.con_MSSQL, FServerPath + "DB.xml", vl_params);
                FDB.MergeXML(FServerPath + "BRMSys.xml");

                if (doConnect) return FDB.Connect();
                else return true;
            }
            catch (Exception exc)
            {
                TExceptionManager.GetExcManager().ProcessException(exc);
                return false;
            }
        }

        public /*static*/ void DestroyDBs(bool onlyDB = false)
        {
            AbstractDBModule.UnloadModules();
            if (FDB != null) FDB = null;

            if (onlyDB) return;
            if (FSessionDB != null) FSessionDB = null;
            
            //if (FDB == null) return;
            //FDB.Disconnect();
            //FDB = null;
        }

        public /*static*/ bool InitContext()
        {
            return FSessionDB.Connect();
            //return FDB.Connect();
        }

        public /*static*/ void ReleaseContext()
        {
            if (FDB != null) FDB.Disconnect();
            if (FSessionDB != null) FSessionDB.Disconnect();
        }

        public TDBApplicationLayer SessionDB
        {
            get { return FSessionDB; }
            set { }
        }

        public /*static*/ TDBApplicationLayer DB
        {
            get { return FDB; }
            set { }
        }

        public /*static*/ string ServerPath
        {
            get { return FServerPath; }
        }

        public string Database
        {
            get { return FDatabase; }
            set { FDatabase = value; }
        }


    }
}