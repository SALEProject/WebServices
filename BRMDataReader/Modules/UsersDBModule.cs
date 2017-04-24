using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Business;
using Business.Common;
using Business.DataModule;
using Business.JSONObjects;

namespace BRMDataReader
{
    public class UsersDBModule : AbstractDBModule
    {
        private TBusiness app;
        private Session session;

        public UsersDBModule(TBusiness app, Session session):base()
        {
            this.app = app;
            this.session = session;

            if (app == null) return;

            string serverPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
            if (!serverPath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                serverPath += System.IO.Path.DirectorySeparatorChar;
            app.DB.MergeXML(serverPath + "Modules" + System.IO.Path.DirectorySeparatorChar + "UsersDBModule.xml");

            app.DB.AddDBFunction("getChatHistory", "Messages", new TDBSelectFunction(getChatHistory));
            app.DB.AddDBFunction("getChatLastTimestamp", "Messages", new TDBSelectFunction(getChatLastTimestamp));
            app.DB.AddDBFunction("getAlerts", "Alerts", new TDBSelectFunction(getAlerts));
            app.DB.AddDBFunction("getAlertsLastTimestamp", "Alerts", new TDBSelectFunction(getAlertsLastTimestamp));
            app.DB.AddDBFunction("getNotifications", "Alerts", new TDBSelectFunction(getNotifications));
            app.DB.AddDBFunction("getNotificationsLastTimestamp", "Alerts", new TDBSelectFunction(getNotificationsLastTimestamp));
            app.DB.AddDBFunction("getAgencies", "Agencies", new TDBSelectFunction(getAgencies));
            app.DB.AddDBFunction("getAgenciesShort", "Agencies", new TDBSelectFunction(getAgenciesShort));
            app.DB.AddDBFunction("getContacts", "Agencies", new TDBSelectFunction(getContacts));
            app.DB.AddDBFunction("getMarketData", "Markets", new TDBSelectFunction(getMarketData));
            app.DB.AddDBFunction("getMarketParameters", "Markets", new TDBSelectFunction(getMarketParameters));
            app.DB.AddDBFunction("getClients", "Clients", new TDBSelectFunction(getClients));
            app.DB.AddDBFunction("getClientsShort", "Clients", new TDBSelectFunction(getClientsShort));
            app.DB.AddDBFunction("getBrokerClients", "Clients", new TDBSelectFunction(getBrokerClients));

            app.DB.AddDBFunction("getMarkets", "Markets", new TDBSelectFunction(getMarkets));
            app.DB.AddDBFunction("getRings", "Rings", new TDBSelectFunction(getRings));
            app.DB.AddDBFunction("getAssets", "Rings", new TDBSelectFunction(getAssets));
            app.DB.AddDBFunction("getAssetsDetailed", "Rings", new TDBSelectFunction(getAssetsDetailed));
            app.DB.AddDBFunction("getTradeInstruments", "Rings", new TDBSelectFunction(getTradeInstruments));
            app.DB.AddDBFunction("getAssetTradeParameters", "Rings", new TDBSelectFunction(getAssetTradeParameters));
            app.DB.AddDBFunction("getAssetsCalendar", "Rings", new TDBSelectFunction(getAssetsCalendar));

            app.DB.AddDBFunction("getTradingSessionStats", "RingSessions", new TDBSelectFunction(getTradingSessionStats));
            app.DB.AddDBFunction("getDeltaTimings", "RingSessions", new TDBSelectFunction(getDeltaTimings));
            app.DB.AddDBFunction("getLastTradingSessionStats", "RingSessions", new TDBSelectFunction(getLastTradingSessionStats));
            app.DB.AddDBFunction("getOrders", "Orders", new TDBSelectFunction(getOrders));
            app.DB.AddDBFunction("getGNTypes", "Orders", new TDBSelectFunction(getGNTypes));
            app.DB.AddDBFunction("getOrderTransactions", "Orders", new TDBSelectFunction(getOrderTransactions));
            app.DB.AddDBFunction("getOrderDetails", "Orders", new TDBSelectFunction(getOrderDetails));
            app.DB.AddDBFunction("getOrderMatches", "Orders", new TDBSelectFunction(getOrderMatches));
            app.DB.AddDBFunction("getOrderPrecheck", "Orders", new TDBSelectFunction(getOrderPrecheck));
            app.DB.AddDBFunction("getMarketChartData", "Transactions", new TDBSelectFunction(getMarketChartData));
            app.DB.AddDBFunction("getTransactions", "Transactions", new TDBSelectFunction(getTransactions));
            app.DB.AddDBFunction("getWhitelist", "Whitelist", new TDBSelectFunction(getWhitelist));
            app.DB.AddDBFunction("getWhitelistReasons", "Whitelist", new TDBSelectFunction(getWhitelistReasons));
            app.DB.AddDBFunction("getWhitelistRequests", "Whitelist", new TDBSelectFunction(getWhitelistRequests));
            app.DB.AddDBFunction("getEvents", "Events", new TDBSelectFunction(getEvents));
            app.DB.AddDBFunction("getDBTime", "Events", new TDBSelectFunction(getDBTime));
            app.DB.AddDBFunction("getJournal", "Events", new TDBSelectFunction(getJournal));
            app.DB.AddDBFunction("getEntryPoints", "EntryPoints", new TDBSelectFunction(getEntryPoints));
            app.DB.AddDBFunction("getUserEntryPoints", "EntryPoints", new TDBSelectFunction(getUserEntryPoints));
            app.DB.AddDBFunction("getOrderEntryPoints", "EntryPoints", new TDBSelectFunction(getOrderEntryPoints));
            app.DB.AddDBFunction("getCounties", "Counties", new TDBSelectFunction(getCounties));
            app.DB.AddDBFunction("getReportList", "GridReports", new TDBSelectFunction(getReportList));
            app.DB.AddDBFunction("getReportDataSet", "GridReports", new TDBSelectFunction(getReportDataSet));
            app.DB.AddDBFunction("getDISPOrdersReport", "GridReports", new TDBSelectFunction(getDISPOrdersReport));

            app.DB.AddDBFunction("getAssetTypes", "AssetTypes", new TDBSelectFunction(getAssetTypes));
            app.DB.AddDBFunction("getInitialOrders", "Orders", new TDBSelectFunction(getInitialOrders));
            app.DB.AddDBFunction("getAssetsSchedules", "Rings", new TDBSelectFunction(getAssetsSchedules));

            app.DB.AddDBFunction("getCurrencies", "Nomenclators", new TDBSelectFunction(getCurrencies));
            app.DB.AddDBFunction("getMeasuringUnits", "Nomenclators", new TDBSelectFunction(getMeasuringUnits));
            app.DB.AddDBFunction("getCPVS", "Nomenclators", new TDBSelectFunction(getCPVS));
            app.DB.AddDBFunction("getCAENS", "Nomenclators", new TDBSelectFunction(getCAENS));
            app.DB.AddDBFunction("getTerminals", "Nomenclators", new TDBSelectFunction(getTerminals));
            app.DB.AddDBFunction("getTranslations", "Nomenclators", new TDBSelectFunction(getTranslations)); 

            app.DB.AddDBFunction("getAgencyRings", "Agencies", new TDBSelectFunction(getAgencyRings));
            app.DB.AddDBFunction("getAgencyClients", "Agencies", new TDBSelectFunction(getAgencyClients));
            app.DB.AddDBFunction("getAgencyAssets", "Agencies", new TDBSelectFunction(getAgencyAssets)); 
            app.DB.AddDBFunction("getRingAgencies", "Rings", new TDBSelectFunction(getRingAgencies));
            app.DB.AddDBFunction("getRingClients", "Rings", new TDBSelectFunction(getRingClients));
            app.DB.AddDBFunction("getRingAdministrators", "Rings", new TDBSelectFunction(getRingAdministrators));
            app.DB.AddDBFunction("getAssetClients", "Rings", new TDBSelectFunction(getAssetClients));
            app.DB.AddDBFunction("getDocuments", "Documents", new TDBSelectFunction(getDocuments));
            app.DB.AddDBFunction("getDocumentTypes", "Documents", new TDBSelectFunction(getDocumentTypes));
            app.DB.AddDBFunction("getWarrantyTypes", "Warranties", new TDBSelectFunction(getWarrantyTypes));
            app.DB.AddDBFunction("getRingWarrantyTypes", "Warranties", new TDBSelectFunction(getRingWarrantyTypes));
            app.DB.AddDBFunction("getAssetWarrantyTypes", "Warranties", new TDBSelectFunction(getAssetWarrantyTypes));
            app.DB.AddDBFunction("getWarranties", "Warranties", new TDBSelectFunction(getWarranties));
            app.DB.AddDBFunction("getAssetWarranties", "Warranties", new TDBSelectFunction(getAssetWarranties)); 
            app.DB.AddDBFunction("getClientWarranties", "Warranties", new TDBSelectFunction(getClientWarranties));
            app.DB.AddDBFunction("getClientWarrantySummary", "Warranties", new TDBSelectFunction(getClientWarrantySummary));

            app.DB.AddDBFunction("getRings", "WebContent", new TDBSelectFunction(getWebRings));
            app.DB.AddDBFunction("getRingDetails", "WebContent", new TDBSelectFunction(getWebRingDetails));
            app.DB.AddDBFunction("getAssetTypes", "WebContent", new TDBSelectFunction(getWebAssetTypes));
            app.DB.AddDBFunction("getAssets", "WebContent", new TDBSelectFunction(getWebAssets));
            app.DB.AddDBFunction("getAssetDetails", "WebContent", new TDBSelectFunction(getWebAssetDetails));
            app.DB.AddDBFunction("getAssetDocuments", "WebContent", new TDBSelectFunction(getWebAssetDocuments));
            app.DB.AddDBFunction("getAssetSessionHistory", "WebContent", new TDBSelectFunction(getWebAssetSessionHistory));
            app.DB.AddDBFunction("getAssetQuotations", "WebContent", new TDBSelectFunction(getWebAssetQuotations));
            app.DB.AddDBFunction("getTodayQuotations", "WebContent", new TDBSelectFunction(getWebTodayQuotations));
            app.DB.AddDBFunction("getTerminals", "WebContent", new TDBSelectFunction(getWebTerminals));
            app.DB.AddDBFunction("getAgencies", "WebContent", new TDBSelectFunction(getWebAgencies));
            app.DB.AddDBFunction("getNews", "WebContent", new TDBSelectFunction(getWebNews));

            app.DB.AddDBFunction("getStatistics", "Transactions", new TDBSelectFunction(getStatistics));
            app.DB.AddDBFunction("validate", "Rings", new TDBSelectFunction(validate));
            app.DB.AddDBFunction("getOperationsBuffer", "Warranties", new TDBSelectFunction(getOperationsBuffer));

            //  nomenclators for procedures
            app.DB.AddDBFunction("getContractTypes", "Nomenclators", new TDBSelectFunction(getContractTypes));
            app.DB.AddDBFunction("getProcedureTypes", "Nomenclators", new TDBSelectFunction(getProcedureTypes));
            app.DB.AddDBFunction("getProcedureCriteria", "Nomenclators", new TDBSelectFunction(getProcedureCriteria));
            app.DB.AddDBFunction("getProcedureLegislations", "Nomenclators", new TDBSelectFunction(getProcedureLegislations));

            app.DB.AddDBFunction("getProcedures", "Procedures", new TDBSelectFunction(getProcedures));
            app.DB.AddDBFunction("getExportTemplates", "Procedures", new TDBSelectFunction(getExportTemplates));
            app.DB.AddDBFunction("getExportTemplate", "Procedures", new TDBSelectFunction(getExportTemplate));
            app.DB.AddDBFunction("getFavouriteProcedures", "Procedures", new TDBSelectFunction(getFavouriteProcedures));
            app.DB.AddDBFunction("getProcedureDocuments", "Procedures", new TDBSelectFunction(getProcedureDocuments));
            app.DB.AddDBFunction("getProcedureVariables", "Procedures", new TDBSelectFunction(getProcedureVariables));
            app.DB.AddDBFunction("getProcedureParticipants", "Procedures", new TDBSelectFunction(getProcedureParticipants));
            app.DB.AddDBFunction("getParticipantProcedures", "Procedures", new TDBSelectFunction(getParticipantProcedures));
            app.DB.AddDBFunction("getForms", "Forms", new TDBSelectFunction(getForms));

            //  remit
            app.DB.AddDBFunction("getDataSources", "REMIT", new TDBSelectFunction(getREMITDataSources));
            app.DB.AddDBFunction("getContractNames", "REMIT", new TDBSelectFunction(getREMITContractNames));
            app.DB.AddDBFunction("getContractTypes", "REMIT", new TDBSelectFunction(getREMITContractTypes));
            app.DB.AddDBFunction("getLoadTypes", "REMIT", new TDBSelectFunction(getREMITLoadTypes));
            app.DB.AddDBFunction("getDataSourceHistoryXLS", "REMIT", new TDBSelectFunction(getREMITDataSourceHistoryXLS));
            app.DB.AddDBFunction("getDataSourceHistoryTable1Reports", "REMIT", new TDBSelectFunction(getREMITDataSourceHistoryTable1Reports));
            app.DB.AddDBFunction("getDataSourceHistoryTable2Reports", "REMIT", new TDBSelectFunction(getREMITDataSourceHistoryTable2Reports));
            app.DB.AddDBFunction("getDataSourceHistoryStorageReports", "REMIT", new TDBSelectFunction(getREMITDataSourceHistoryStorageReports));
            app.DB.AddDBFunction("getDataSourceProcessLog", "REMIT", new TDBSelectFunction(getREMITDataSourceProcessLog));
            app.DB.AddDBFunction("getDataSourceOutputData", "REMIT", new TDBSelectFunction(getREMITDataSourceOutputData));
            app.DB.AddDBFunction("getDataSourceReceiptData", "REMIT", new TDBSelectFunction(getREMITDataSourceReceiptData));
            app.DB.AddDBFunction("getParticipants", "REMIT", new TDBSelectFunction(getREMITParticipants));
            app.DB.AddDBFunction("getOrders", "REMIT", new TDBSelectFunction(getREMITOrders));
            app.DB.AddDBFunction("getOrderDetails", "REMIT", new TDBSelectFunction(getREMITOrderDetails));
            app.DB.AddDBFunction("getTable1ReportDetails", "REMIT", new TDBSelectFunction(getREMITTable1ReportDetails));
            app.DB.AddDBFunction("getTable2ReportDetails", "REMIT", new TDBSelectFunction(getREMITTable2ReportDetails));
            app.DB.AddDBFunction("getTable2VolumeOptionalities", "REMIT", new TDBSelectFunction(getREMITTable2VolumeOptionalities));
            app.DB.AddDBFunction("getTable2FixingIndexDetails", "REMIT", new TDBSelectFunction(getREMITTable2FixingIndexDetails));
            app.DB.AddDBFunction("getExistingParticipantIDs", "REMIT", new TDBSelectFunction(getREMITExistingParticipantIDs));
            app.DB.AddDBFunction("getExistingCounterpartIDs", "REMIT", new TDBSelectFunction(getREMITExistingCounterpartIDs));
            app.DB.AddDBFunction("getNGStorageReportDetails", "REMIT", new TDBSelectFunction(getREMITNGStorageReportDetails));
            app.DB.AddDBFunction("getStorageFacilityReports", "REMIT", new TDBSelectFunction(getREMITStorageFacilityReports));
            app.DB.AddDBFunction("getStorageParticipantActivityReports", "REMIT", new TDBSelectFunction(getREMITStorageParticipantActivityReports));
            app.DB.AddDBFunction("getStorageUnavailabilityReports", "REMIT", new TDBSelectFunction(getREMITStorageUnavailabilityReports));
            app.DB.AddDBFunction("getARISActivityLog", "REMIT", new TDBSelectFunction(getREMITARISActivityLog));
        }

        public DataSet getChatHistory(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            if (vl_arguments["Since"] != null) vl_params.Add("@prm_Date").AsDateTime = vl_arguments["Since"].AsDateTime;
            else vl_params.Add("@prm_Date").AsDateTime = session.LastMessageTimestamp;
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;

            DataSet ds = app.DB.Select("select_ChatHistory", "Messages", vl_params);
            session.LastMessageTimestamp = DateTime.Now;
            return ds;
        }

        public DataSet getChatLastTimestamp(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = session.ID;
            DataSet ds = app.DB.Select("select_LastMessageTimestamp", "Sessions", vl_params);
            return ds;
        }

        //getAlerts (SessionId,CurrentState,Filters)
        public DataSet getAlerts(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            if (vl_arguments["Since"] != null) vl_params.Add("@prm_Date").AsDateTime = vl_arguments["Since"].AsDateTime;
            else vl_params.Add("@prm_Date").AsDateTime = session.LastAlertTimestamp;

            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;

            DataSet ds = app.DB.Select("select_AlertsHistory", "Alerts", vl_params);
            session.LastAlertTimestamp = DateTime.Now;
            return ds;
        }

        public DataSet getAlertsLastTimestamp(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = session.ID;
            DataSet ds = app.DB.Select("select_LastAlertTimestamp", "Sessions", vl_params);
            return ds;
        }

        public DataSet getNotifications(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            if (vl_arguments["Since"] != null) vl_params.Add("@prm_Date").AsDateTime = vl_arguments["Since"].AsDateTime;
            else vl_params.Add("@prm_Date").AsDateTime = session.LastNotificationTimestamp;

            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;

            DataSet ds = app.DB.Select("select_NotificationsHistory", "Notifications", vl_params);
            session.LastNotificationTimestamp = DateTime.Now;
            return ds;
        }

        public DataSet getNotificationsLastTimestamp(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = session.ID;
            DataSet ds = app.DB.Select("select_LastNotificationTimestamp", "Sessions", vl_params);
            return ds;
        }

        public DataSet getAgencies(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_Agency").AsInt32 = -1;

            vl_params.Add("#prm_SortField").AsString = "ID";
            vl_params.Add("#prm_SortOrder").AsString = "ASC";
            vl_params.Add("@prm_QueryOffset").AsInt32 = -1;
            vl_params.Add("@prm_QueryLimit").AsInt32 = -1;
            vl_params.Add("@prm_Language").AsString = session.Language;
            vl_params.Add("@prm_QueryKeyword").AsString = "";

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

            DataSet ds = app.DB.Select("select_AgenciesDetailed", "Agencies", vl_params);
            return ds;
        }

        public DataSet getAgenciesShort(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_Agency").AsInt32 = -1;
            vl_params.Add("@prm_Status").AsString = "";

            if (vl_arguments["Status"] != null && vl_arguments["Status"].ValueType == TVariantType.vtString)
            {
                vl_params["@prm_Status"].AsString = vl_arguments["Status"].AsString;
            }

            DataSet ds = app.DB.Select("select_AgenciesShort", "Agencies", vl_params);
            return ds;
        }

        public DataSet getContacts(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Agency"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Agency").AsInt32 = vl_arguments["ID_Agency"].AsInt32;
            vl_params.Add("@prm_isBroker").AsInt32 = -1;
            if (vl_arguments["isBroker"] != null) vl_params["@prm_isBroker"].AsBoolean = vl_arguments["isBroker"].AsBoolean;

            DataSet ds = app.DB.Select("select_Contacts", "Brokers", vl_params);
            return ds;
        }

        public DataSet getClients(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_Client").AsInt32 = -1;
            vl_params.Add("@prm_Status").AsString = "any";
            if (vl_arguments["Status"] != null) vl_params["@prm_Status"].AsString = vl_arguments["Status"].AsString;

            vl_params.Add("#prm_SortField").AsString = "ID";
            vl_params.Add("#prm_SortOrder").AsString = "ASC";
            vl_params.Add("@prm_QueryOffset").AsInt32 = -1;
            vl_params.Add("@prm_QueryLimit").AsInt32 = -1;
            vl_params.Add("@prm_Language").AsString = session.Language;
            vl_params.Add("@prm_QueryKeyword").AsString = "";

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

            DataSet ds = app.DB.Select("select_ClientsDetailed", "Clients", vl_params);
            return ds;
        }

        public DataSet getClientsShort(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_Client").AsInt32 = -1;
            vl_params.Add("@prm_Status").AsString = "any";
            if (vl_arguments["Status"] != null) vl_params["@prm_Status"].AsString = vl_arguments["Status"].AsString;

            DataSet ds = app.DB.Select("select_ClientsShort", "Clients", vl_params);
            return ds;
        }

        public DataSet getBrokerClients(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_Asset").AsInt32 = 0;
            vl_params.Add("@prm_ID_Broker").AsInt32 = session.ID_Broker;
            vl_params.Add("@prm_ID_Order").AsInt32 = 0;

            if (vl_arguments["ID_Asset"] != null) vl_params["@prm_ID_Asset"].AsInt32 = vl_arguments["ID_Asset"].AsInt32;
            if (vl_arguments["ID_Broker"] != null) vl_params["@prm_ID_Broker"].AsInt32 = vl_arguments["ID_Broker"].AsInt32;
            if (vl_arguments["ID_Order"] != null) vl_params["@prm_ID_Order"].AsInt32 = vl_arguments["ID_Order"].AsInt32;

            DataSet ds = app.DB.Select("select_BrokerClients", "Clients", vl_params);
            return ds;
        }

        //getMarketData (SessionId, CurrentState,Market) 
        public DataSet getMarketData(TVariantList vl_arguments)
        {
            return null;
        }

        public DataSet getRingSessionStats(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Ring"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Ring").AsInt32 = vl_arguments["ID_Ring"].AsInt32;
            //DataSet ds = app.DB.Select("select_SessionStats", "RingSessions", vl_params);
            DataSet ds = app.DB.Select("select_SessionStats", "RingSessions", vl_params);
            return ds;
        }

        public DataSet getAssetSessionStats(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Asset"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = vl_arguments["ID_Asset"].AsInt32;
            //DataSet ds = app.DB.Select("select_SessionStats", "RingSessions", vl_params);
            DataSet ds = app.DB.Select("select_SessionStats", "AssetSessions", vl_params);
            return ds;
        }

        //getTradingSessionStats (SessionId,CuurentState,Market) 
        public DataSet getTradingSessionStats(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = 1;
            vl_params.Add("@prm_ID_Broker").AsInt32 = -1;

            DataSet ds = null;

            switch (session.EntryPoint)
            {
                case "BTGN":
                    //DataSet ds = app.DB.Select("select_SessionStats", "RingSessions", vl_params);
                    ds = app.DB.Select("select_SessionStats_RGN", "AssetSessions", vl_params);
                    return ds;
                case "DISPONIBIL":
                    if (vl_arguments["ID_Asset"] != null)
                        vl_params["@prm_ID_Asset"].AsInt32 = vl_arguments["ID_Asset"].AsInt32;
                    else vl_params["@prm_ID_Asset"].AsInt32 = 0;

                    if (vl_arguments["ID_Broker"] != null)
                        vl_params["@prm_ID_Broker"].AsInt32 = vl_arguments["ID_Broker"].AsInt32;
                    else
                        vl_params["@prm_ID_Broker"].AsInt32 = session.ID_Broker;

                    ds = app.DB.Select("select_SessionStats_DISPONIBIL", "AssetSessions", vl_params);
                    return ds;                    
            }

            return null;
        }

        public DataSet getDeltaTimings(TVariantList vl_arguments)
        {
            if (session.EntryPoint != "DISPONIBIL") return null;
            if (vl_arguments["ID_Asset"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = vl_arguments["ID_Asset"].AsInt32;
            return app.DB.Select("select_DeltaTimings", "AssetSessions", vl_params);
        }

        public DataSet getLastTradingSessionStats(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = 1;
            //DataSet ds = app.DB.Select("select_SessionStats", "RingSessions", vl_params);
            DataSet ds = app.DB.Select("select_lastSessionStats", "AssetSessions", vl_params);
            return ds;
        }

        public DataSet getRingSession(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Ring").AsInt32 = 1;
            DataSet ds = app.DB.Select("select_CurrentSession", "RingSessions", vl_params);
            return ds;
        }

        public DataSet getAssetSession(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = 1;
            DataSet ds = app.DB.Select("select_CurrentSession", "AssetSessions", vl_params);
            return ds;
        }

        public DataSet getMarketParameters(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Ring").AsInt32 = 1;
            DataSet ds = app.DB.Select("select_MarketParameters", "Rings", vl_params);
            return ds;
        }

        public DataSet getMarkets(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            DataSet ds = app.DB.Select("select_MarketsbyID_Bursary", "Markets", vl_params);
            return ds;
        }

        public DataSet getRings(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Market"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_Market").AsInt32 = vl_arguments["ID_Market"].AsInt32;
            vl_params.Add("@prm_ID_Ring").AsInt32 = -1;
            vl_params.Add("@prm_AnyStatus").AsBoolean = false;
            vl_params.Add("@prm_All").AsBoolean = false;

            if (vl_arguments["ID_Ring"] != null)
                if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64)
                    vl_params["@prm_ID_Ring"].AsInt32 = vl_arguments["ID_Ring"].AsInt32;

            if (vl_arguments["anystatus"] != null)
                if (vl_arguments["anystatus"].AsBoolean)
                    vl_params["@prm_AnyStatus"].AsBoolean = true;

            if (vl_arguments["all"] != null)
                if (vl_arguments["all"].AsBoolean)
                    vl_params["@prm_All"].AsBoolean = true;

            DataSet ds = app.DB.Select("select_RingsbyID_Market", "Rings", vl_params);
            return ds;
        }

        public DataSet getAssets(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Market"] == null) return null;
            
            //if (vl_arguments["ID_Ring"] == null) return null;

            int ID_Ring = -1;
            if (vl_arguments["ID_Ring"] != null)
            {
                if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
                else return null;
            }

            int ID_Asset = -1;
            if (vl_arguments["ID_Asset"] != null)
            {
                if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
                else return null;
            }

            int ID_AssetType = -1;
            if (vl_arguments["ID_AssetType"] != null)
            {
                if (vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt64) ID_AssetType = vl_arguments["ID_AssetType"].AsInt32;
                else return null;
            }

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_Market").AsInt32 = vl_arguments["ID_Market"].AsInt32;
            //vl_params.Add("@prm_ID_Ring").AsInt32 = vl_arguments["ID_Ring"].AsInt32;
            vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
            vl_params.Add("@prm_ID").AsInt32 = ID_Asset;
            vl_params.Add("@prm_ID_AssetType").AsInt32 = ID_AssetType;
            vl_params.Add("@prm_AnyStatus").AsBoolean = false;
            vl_params.Add("@prm_All").AsBoolean = false;

            if (vl_arguments["anystatus"] != null)
                if (vl_arguments["anystatus"].ValueType == TVariantType.vtBoolean)
                    vl_params["@prm_AnyStatus"].AsBoolean = vl_arguments["anystatus"].AsBoolean;

            if (vl_arguments["all"] != null)
                if (vl_arguments["all"].ValueType == TVariantType.vtBoolean)
                    vl_params["@prm_All"].AsBoolean = vl_arguments["all"].AsBoolean;

            vl_params.Add("#prm_SortField").AsString = "ID";
            vl_params.Add("#prm_SortOrder").AsString = "ASC";
            vl_params.Add("@prm_QueryOffset").AsInt32 = -1;
            vl_params.Add("@prm_QueryLimit").AsInt32 = -1;
            vl_params.Add("@prm_Language").AsString = session.Language;
            vl_params.Add("@prm_QueryKeyword").AsString = "";

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

            DataSet ds = app.DB.Select("select_AssetsbyID_MarketandID_Ring", "Assets", vl_params);
            return ds;
        }

        public DataSet getAssetsDetailed(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Market"] == null) return null;

            //if (vl_arguments["ID_Ring"] == null) return null;

            int ID_Ring = -1;
            if (vl_arguments["ID_Ring"] != null)
            {
                if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
                else return null;
            }

            int ID_Asset = -1;
            if (vl_arguments["ID_Asset"] != null)
            {
                if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
                else return null;
            }

            int ID_AssetType = -1;
            if (vl_arguments["ID_AssetType"] != null)
            {
                if (vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt64) ID_AssetType = vl_arguments["ID_AssetType"].AsInt32;
                else return null;
            }

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_Market").AsInt32 = vl_arguments["ID_Market"].AsInt32;
            //vl_params.Add("@prm_ID_Ring").AsInt32 = vl_arguments["ID_Ring"].AsInt32;
            vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
            vl_params.Add("@prm_ID").AsInt32 = ID_Asset;
            vl_params.Add("@prm_ID_AssetType").AsInt32 = ID_AssetType;
            vl_params.Add("@prm_AnyStatus").AsBoolean = false;
            vl_params.Add("@prm_All").AsBoolean = false;

            if (vl_arguments["anystatus"] != null)
                if (vl_arguments["anystatus"].ValueType == TVariantType.vtBoolean)
                    vl_params["@prm_AnyStatus"].AsBoolean = vl_arguments["anystatus"].AsBoolean;

            if (vl_arguments["all"] != null)
                if (vl_arguments["all"].ValueType == TVariantType.vtBoolean)
                    vl_params["@prm_All"].AsBoolean = vl_arguments["all"].AsBoolean;

            vl_params.Add("#prm_SortField").AsString = "ID";
            vl_params.Add("#prm_SortOrder").AsString = "ASC";
            vl_params.Add("@prm_QueryOffset").AsInt32 = -1;
            vl_params.Add("@prm_QueryLimit").AsInt32 = -1;
            vl_params.Add("@prm_Language").AsString = session.Language;
            vl_params.Add("@prm_QueryKeyword").AsString = "";

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

            DataSet ds = app.DB.Select("select_AssetsDetailedbyID_MarketandID_Ring", "Assets", vl_params);
            return ds;
        }

        public DataSet getTradeInstruments(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_Market").AsInt32 = -1;
            vl_params.Add("@prm_ID_Ring").AsInt32 = -1;
            vl_params.Add("@prm_ID_AssetType").AsInt32 = -1;
            vl_params.Add("@prm_Language").AsString = session.Language;

            if (vl_arguments["ID_Market"] != null) vl_params["@prm_ID_Market"].AsInt32 = vl_arguments["ID_Market"].AsInt32;
            if (vl_arguments["ID_Ring"] != null) vl_params["@prm_ID_Ring"].AsInt32 = vl_arguments["ID_Ring"].AsInt32;
            if (vl_arguments["ID_AssetType"] != null) vl_params["@prm_ID_AssetType"].AsInt32 = vl_arguments["ID_AssetType"].AsInt32;

            DataSet ds = app.DB.Select("select_TradeInstruments", "Assets", vl_params);
            return ds;
        }

        public DataSet getAssetTradeParameters(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Asset"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = vl_arguments["ID_Asset"].AsInt32;
                
            DataSet ds = app.DB.Select("select_AssetTradeParameters", "Assets", vl_params);
            return ds;
        }


        public DataSet getAssetsCalendar(TVariantList vl_arguments)
        {
            DataSet dsAssets = getAssets(vl_arguments);
            vl_arguments = new TVariantList();
            DataSet dsAssetsSchedules = getAssetsSchedules(vl_arguments);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds.Tables.Add(dt);
            DataRow row = dt.NewRow();

            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ID";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Code";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.DateTime");
            column.ColumnName = "DateTime";
            dt.Columns.Add(column);


            for (int i = 0; i < dsAssets.Tables[0].Rows.Count; i++)
            {
                int idAsset = Convert.ToInt32(dsAssets.Tables[0].Rows[i]["ID"]);
                bool isDeleted = Convert.ToBoolean(dsAssets.Tables[0].Rows[i]["isDeleted"]);
                if (isDeleted) continue;

                for (int j = 0; j < dsAssetsSchedules.Tables[0].Rows.Count; j++)
                {
                    int id_Asset = Convert.ToInt32(dsAssetsSchedules.Tables[0].Rows[j]["ID_Asset"]);
                    if (idAsset == id_Asset)
                    {
                        string code = dsAssets.Tables[0].Rows[i]["Code"].ToString();
                        DateTime StartDate = Convert.ToDateTime(dsAssetsSchedules.Tables[0].Rows[j]["StartDate"]);
                        DateTime EndDate = Convert.ToDateTime(dsAssetsSchedules.Tables[0].Rows[j]["EndDate"]);
                        bool dayMonday = Convert.ToBoolean(dsAssetsSchedules.Tables[0].Rows[j]["dayMonday"]);
                        bool dayTuesday = Convert.ToBoolean(dsAssetsSchedules.Tables[0].Rows[j]["dayTuesday"]);
                        bool dayWednesday = Convert.ToBoolean(dsAssetsSchedules.Tables[0].Rows[j]["dayWednesday"]);
                        bool dayThursday = Convert.ToBoolean(dsAssetsSchedules.Tables[0].Rows[j]["dayThursday"]);
                        bool dayFriday = Convert.ToBoolean(dsAssetsSchedules.Tables[0].Rows[j]["dayFriday"]);
                        bool daySaturday = Convert.ToBoolean(dsAssetsSchedules.Tables[0].Rows[j]["daySaturday"]);
                        bool daySunday = Convert.ToBoolean(dsAssetsSchedules.Tables[0].Rows[j]["daySunday"]);

                        if (StartDate.Date != EndDate.Date) //multiple assets trading sessions
                        {
                            DateTime date = StartDate;
                            while (date.Date <= EndDate.Date)
                            {
                                bool isTradingSessionToday = false;
                                switch (date.DayOfWeek)
                                {
                                    case DayOfWeek.Monday:
                                        if (dayMonday) isTradingSessionToday = true;
                                        break;
                                    case DayOfWeek.Tuesday:
                                        if (dayTuesday) isTradingSessionToday = true;
                                        break;
                                    case DayOfWeek.Wednesday:
                                        if (dayWednesday) isTradingSessionToday = true;
                                        break;
                                    case DayOfWeek.Thursday:
                                        if (dayThursday) isTradingSessionToday = true;
                                        break;
                                    case DayOfWeek.Friday:
                                        if (dayFriday) isTradingSessionToday = true;
                                        break;
                                    case DayOfWeek.Sunday:
                                        if (daySunday) isTradingSessionToday = true;
                                        break;
                                    case DayOfWeek.Saturday:
                                        if (daySaturday) isTradingSessionToday = true;
                                        break;
                                }
                                if (isTradingSessionToday)
                                {
                                    row = dt.NewRow();
                                    row["ID"] = idAsset;
                                    row["Code"] = code;
                                    row["DateTime"] = DateTime.Parse(date.Date.ToString("d") + " " + dsAssetsSchedules.Tables[0].Rows[j]["PreOpeningTime"].ToString());
                                    dt.Rows.Add(row);
                                }

                                date = date.AddDays(1);
                            }
                        }
                        else  //just one trading session and that's today
                        {
                            row = dt.NewRow();
                            row["ID"] = idAsset;
                            row["Code"] = code;
                            row["DateTime"] = DateTime.Parse(StartDate.Date.ToString("d") + " " + dsAssetsSchedules.Tables[0].Rows[j]["PreOpeningTime"].ToString());
                            dt.Rows.Add(row);
                        }

                        break;  //jump to next asset
                    }
                }
            }
            return ds;
        }

        public DataSet getOrders(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_RingSession").AsInt32 = -1;
            vl_params.Add("@prm_ID_AssetSession").AsInt32 = -1;
            vl_params.Add("@prm_ID_Broker").AsInt32 = session.ID_Broker;
            vl_params.Add("@prm_ID_Asset").AsInt32 = -1;
            vl_params.Add("@prm_AnyStatus").AsBoolean = false;
            vl_params.Add("@prm_ID").AsInt32 = -1;

            DataSet ds_session = getRingSession(null);
            if (ds_session != null)
                if (ds_session.Tables.Count == 0)
                    if (ds_session.Tables[0].Rows.Count == 0)
                        vl_params.Add("@prm_ID_RingSession").AsInt32 = Convert.ToInt32(ds_session.Tables[0].Rows[0]["ID"]);

            if (vl_arguments["ID_Asset"] != null)
                vl_params["@prm_ID_Asset"].AsInt32 = vl_arguments["ID_Asset"].AsInt32;

            if (vl_arguments["all"] != null)
                if (vl_arguments["all"].AsBoolean)
                    vl_params["@prm_ID_Broker"].AsInt32 = -1;

            if (vl_arguments["anystatus"] != null)
                if (vl_arguments["anystatus"].AsBoolean)
                    vl_params["@prm_AnyStatus"].AsBoolean = true;

            if (vl_arguments["ID_Order"] != null)
                if (vl_arguments["ID_Order"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Order"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Order"].ValueType == TVariantType.vtInt64) vl_params["@prm_ID"].AsInt32 = vl_arguments["ID_Order"].AsInt32;

            DataSet ds = null;
            switch (session.EntryPoint)
            {
                case "BTGN":
                    ds = app.DB.Select("select_OrdersRGN", "Orders", vl_params);
                    break;
                case "DISPONIBIL":
                    ds = app.DB.Select("select_OrdersDISPONIBIL", "Orders", vl_params);
                    break;
            }

            return ds;
        }

        public DataSet getGNTypes(TVariantList vl_arguments)
        {
            DataSet ds = app.DB.Select("select_GNTypes", "GNTypes", null);
            return ds;
        }

        public DataSet getOrderTransactions(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Order"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Order").AsInt32 = vl_arguments["ID_Order"].AsInt32;
            DataSet ds = app.DB.Select("select_OrderTransactions", "Orders", vl_params);
            return ds;
        }

        public DataSet getOrderDetails(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Order"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_RingSession").AsInt32 = -1;
            vl_params.Add("@prm_ID_AssetSession").AsInt32 = -1;
            vl_params.Add("@prm_ID_Broker").AsInt32 = session.ID_Broker;
            vl_params.Add("@prm_ID_Asset").AsInt32 = -1;
            vl_params.Add("@prm_AnyStatus").AsBoolean = false;
            vl_params.Add("@prm_ID").AsInt32 = vl_arguments["ID_Order"].AsInt32;

            DataSet ds_session = getRingSession(null);
            if (ds_session != null)
                if (ds_session.Tables.Count == 0)
                    if (ds_session.Tables[0].Rows.Count == 0)
                        vl_params.Add("@prm_ID_RingSession").AsInt32 = Convert.ToInt32(ds_session.Tables[0].Rows[0]["ID"]);

            if (vl_arguments["ID_Asset"] != null)
                vl_params["@prm_ID_Asset"].AsInt32 = vl_arguments["ID_Asset"].AsInt32;

            if (vl_arguments["all"] != null)
                if (vl_arguments["all"].AsBoolean)
                    vl_params["@prm_ID_Broker"].AsInt32 = -1;

            if (vl_arguments["anystatus"] != null)
                if (vl_arguments["anystatus"].AsBoolean)
                    vl_params["@prm_AnyStatus"].AsBoolean = true;

            DataSet ds = null;
            switch (session.EntryPoint)
            {
                case "BTGN":
                    ds = app.DB.Select("select_OrdersRGN", "Orders", vl_params);
                    break;
                case "DISPONIBIL":
                    ds = app.DB.Select("select_OrdersDISPONIBIL", "Orders", vl_params);
                    break;
            }
            return ds;
        }

        public DataSet getOrderMatches(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Order"] == null && vl_arguments["ID_Asset"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Order").AsInt32 = -1;
            vl_params.Add("@prm_ID_Asset").AsInt32 = -1;

            if (vl_arguments["ID_Order"] != null) vl_params["@prm_ID_Order"].AsInt32 = vl_arguments["ID_Order"].AsInt32;
            if (vl_arguments["ID_Asset"] != null) vl_params["@prm_ID_Asset"].AsInt32 = vl_arguments["ID_Asset"].AsInt32;

            DataSet ds = null;
            switch (session.EntryPoint)
            {
                case "BTGN":
                    ds = app.DB.Select("select_OrderMatches_RGN", "OrderMatches", vl_params);
                    break;
                case "DISPONIBIL":
                    ds = app.DB.Select("select_OrderMatches_DISPONIBIL", "OrderMatches", vl_params);
                    break;
            }
            return ds;
        }

        public DataSet getOrderPrecheck(TVariantList vl_arguments)
        {
            bool Direction = false;
            double Quantity = 0;
            double Price = 0;
            DateTime StartDeliveryDate = DateTime.Today;
            DateTime EndDeliveryDate = DateTime.Today;

            if (vl_arguments["Direction"] != null)
                switch (vl_arguments["Direction"].AsString)
                {
                    case "S":
                        Direction = true;
                        break;
                    default:
                        Direction = false;
                        break;
                }

            if (vl_arguments["Quantity"] != null &&
                (vl_arguments["Quantity"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtSingle ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtDouble)) Quantity = vl_arguments["Quantity"].AsDouble;

            if (vl_arguments["Price"] != null &&
                (vl_arguments["Price"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Price"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Price"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["Price"].ValueType == TVariantType.vtSingle ||
                vl_arguments["Price"].ValueType == TVariantType.vtDouble)) Price = vl_arguments["Price"].AsDouble;

            if (vl_arguments["StartDeliveryDate"].ValueType == TVariantType.vtDateTime) StartDeliveryDate = vl_arguments["StartDeliveryDate"].AsDateTime;
            if (vl_arguments["EndDeliveryDate"].ValueType == TVariantType.vtDateTime) EndDeliveryDate = vl_arguments["EndDeliveryDate"].AsDateTime;

            //  get ID_Client
            int ID_Broker = session.ID_Broker;
            int ID_Client = 0;
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
            DataSet ds_client = app.DB.Select("select_HousebyID_Broker", "Clients", vl_params);
            if (ds_client == null) return null;
            if (ds_client.Tables.Count == 0) return null;
            if (ds_client.Tables[0].Rows.Count > 0)
            {
                ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
            }
            
            vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
            vl_params.Add("@prm_Direction").AsBoolean = Direction;
            vl_params.Add("@prm_Quantity").AsDouble = Quantity;
            vl_params.Add("@prm_Price").AsDouble = Price;
            vl_params.Add("@prm_StartDeliveryDate").AsDateTime = StartDeliveryDate;
            vl_params.Add("@prm_EndDeliveryDate").AsDateTime = EndDeliveryDate;
            DataSet ds = app.DB.Select("select_Precheck", "Orders", vl_params);

            return ds;
        }

        public DataSet getMarketChartData(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = 1;

            if (vl_arguments["Since"] != null) vl_params.Add("@prm_Date").AsDateTime = vl_arguments["Since"].AsDateTime;
            else vl_params.Add("@prm_Date").AsString = "";//session.LastMessageTimestamp;
            
            DataSet ds = app.DB.Select("getMarketChartDatabyID_Asset", "MarketData", vl_params);
            return ds;
        }

        //getTransactions (SessionId,CurrentState,Filters) 
        public DataSet getTransactions(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_Broker").AsInt32 = session.ID_Broker;
            vl_params.Add("@prm_StartDate").AsDateTime = DateTime.Today;
            vl_params.Add("@prm_EndDate").AsDateTime = DateTime.Today.AddDays(1);
            vl_params.Add("@prm_ID_Asset").AsInt32 = -1;

            if (vl_arguments["all"] != null)
                if (vl_arguments["all"].AsBoolean)
                    vl_params["@prm_ID_Broker"].AsInt32 = -1;

            if (vl_arguments["ID_Broker"] != null)
            {
                if (vl_arguments["ID_Broker"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Broker"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Broker"].ValueType == TVariantType.vtInt64) vl_params["@prm_ID_Broker"].AsInt32 = vl_arguments["ID_Broker"].AsInt32;
            }

            if (vl_arguments["StartDate"] != null)
                if (vl_arguments["StartDate"].ValueType == TVariantType.vtDateTime)
                    vl_params["@prm_StartDate"].AsDateTime = vl_arguments["StartDate"].AsDateTime;

            if (vl_arguments["Since"] != null)
                if (vl_arguments["Since"].ValueType == TVariantType.vtDateTime)
                    vl_params["@prm_StartDate"].AsDateTime = vl_arguments["Since"].AsDateTime;

            if (vl_arguments["EndDate"] != null)
                if (vl_arguments["EndDate"].ValueType == TVariantType.vtDateTime)
                    vl_params["@prm_EndDate"].AsDateTime = vl_arguments["EndDate"].AsDateTime;

            if (vl_arguments["ID_Asset"] != null)
            {
                if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) vl_params["@prm_ID_Asset"].AsInt32 = vl_arguments["ID_Asset"].AsInt32;
            }
            
            DataSet ds = null;
            switch (session.EntryPoint)
            {
                case "BTGN":
                    ds = app.DB.Select("select_TransactionsRGN", "Transactions", vl_params);
                    break;
                case "DISPONIBIL":
                    ds = app.DB.Select("select_TransactionsDISPONIBIL", "Transactions", vl_params);
                    break;
            }
            return ds;
        }

        public DataSet getWhitelist(TVariantList vl_arguments)
        {
            int ID_Broker = session.ID_Broker;
            int ID_Client = 0;
            int ID_Bursary = session.ID_Bursary;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
            DataSet ds_client = app.DB.Select("select_HousebyID_Broker", "Clients", vl_params);
            if (ds_client == null) return null;
            if (ds_client.Tables.Count == 0) return null;

            vl_params.Add("@prm_ID_Client").AsInt32 = 0;
            if (ds_client.Tables[0].Rows.Count > 0)
            {
                ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
                vl_params["@prm_ID_Client"].AsInt32 = ID_Client;
            }

            vl_params.Add("@prm_ID_Bursary").AsInt32 = ID_Bursary;

            DataSet ds = app.DB.Select("select_AgreedClients", "AgreedClients", vl_params);
            return ds;
        }

        public DataSet getWhitelistReasons(TVariantList vl_arguments)
        {
            DataSet ds = app.DB.Select("select_DisagreeReasons", "DisagreeReasons", null);
            return ds;
        }

        public DataSet getWhitelistRequests(TVariantList vl_arguments)
        {
            DataSet ds = app.DB.Select("select_WhitelistRequests", "AgreedClients", null);
            return ds;
        }

        public DataSet getEvents(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            if (vl_arguments["Since"] != null) vl_params.Add("@prm_Date").AsDateTime = vl_arguments["Since"].AsDateTime;
            else vl_params.Add("@prm_Date").AsDateTime = session.LastEventTimestamp;

            DataSet ds = app.DB.Select("select_EventHistory", "Events", vl_params);
            session.LastEventTimestamp = DateTime.Now;
            return ds;
        }

        public DataSet getDBTime(TVariantList vl_arguments)
        {
            DataSet ds = app.DB.Select("select_DBTime", "Events", null);
            return ds;
        }

        public DataSet getJournal(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_User").AsInt32 = -1;
            vl_params.Add("@prm_ID_Broker").AsInt32 = -1;
            vl_params.Add("@prm_ID_Agency").AsInt32 = -1;
            vl_params.Add("@prm_ID_Client").AsInt32 = -1;
            vl_params.Add("@prm_ID_Ring").AsInt32 = -1;
            vl_params.Add("@prm_ID_Asset").AsInt32 = -1;            
            vl_params.Add("@prm_ID").AsInt32 = -1;

            if (vl_arguments["ID_User"] != null) vl_params["@prm_ID_User"].AsInt32 = vl_arguments["ID_User"].AsInt32;
            if (vl_arguments["ID_Broker"] != null) vl_params["@prm_ID_Broker"].AsInt32 = vl_arguments["ID_Broker"].AsInt32;
            if (vl_arguments["ID_Agency"] != null) vl_params["@prm_ID_Agency"].AsInt32 = vl_arguments["ID_Agency"].AsInt32;
            if (vl_arguments["ID_Client"] != null) vl_params["@prm_ID_Client"].AsInt32 = vl_arguments["ID_Client"].AsInt32;
            if (vl_arguments["ID_Ring"] != null) vl_params["@prm_ID_Ring"].AsInt32 = vl_arguments["ID_Ring"].AsInt32;
            if (vl_arguments["ID_Asset"] != null) vl_params["@prm_ID_Asset"].AsInt32 = vl_arguments["ID_Asset"].AsInt32;
            if (vl_arguments["ID_Journal"] != null) vl_params["@prm_ID"].AsInt32 = vl_arguments["ID_Journal"].AsInt32;

            if (vl_arguments["Since"] != null) vl_params.Add("@prm_Date").AsDateTime = vl_arguments["Since"].AsDateTime;
            else vl_params.Add("@prm_Date").AsDateTime = DateTime.Today;

            DataSet ds = app.DB.Select("select_Journal", "Journal", vl_params);
            session.LastEventTimestamp = DateTime.Now;
            return ds;
        }

        public DataSet getEntryPoints(TVariantList vl_arguments)
        {
            int idEntryPoint = -1;
            TVariantList vl_params = new TVariantList();
            if (vl_arguments["ID_EntryPoint"] != null) vl_params.Add("@prm_ID").AsInt32 = vl_arguments["ID_EntryPoint"].AsInt32;
            else vl_params.Add("@prm_ID").AsInt32 = idEntryPoint;

            DataSet ds = app.DB.Select("select_EntryPoints_RGN", "EntryPoints_RGN", vl_params);
            return ds;
        }

        public DataSet getUserEntryPoints(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_User").AsInt32 = session.ID_User;

            DataSet ds = app.DB.Select("select_UsersXEntryPointsbyID_User", "UsersXEntryPoints_RGN", vl_params);
            return ds;
        }

        public DataSet getCounties(TVariantList vl_arguments)
        {
            int idCounty = -1;
            TVariantList vl_params = new TVariantList();
            if (vl_arguments["ID_County"] != null) vl_params.Add("@prm_ID").AsInt32 = vl_arguments["ID_County"].AsInt32;
            else vl_params.Add("@prm_ID").AsInt32 = idCounty;

            DataSet ds = app.DB.Select("select_Counties", "Counties", vl_params);
            return ds;
        }

        public DataSet getOrderEntryPoints(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Order"] == null) return null;

            int orderID = -1;
            TVariantList vl_params = new TVariantList();
            if (vl_arguments["ID_Order"] != null) vl_params.Add("@prm_ID").AsInt32 = vl_arguments["ID_Order"].AsInt32;
            else vl_params.Add("@prm_ID").AsInt32 = orderID;

            DataSet ds = app.DB.Select("select_OrderEntryPoints", "EntryPoints_RGN", vl_params);
            return ds;
        }

        public DataSet getReportList(TVariantList vl_arguments)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add();
            ds.Tables[0].Columns.Add("ReportName", Type.GetType("System.String"));

            DataRow row = null;

            switch (session.EntryPoint)
            {
                case "BTGN":
                    row = ds.Tables[0].NewRow();
                    row["ReportName"] = "Istoric Ordine";
                    ds.Tables[0].Rows.Add(row);

                    row = ds.Tables[0].NewRow();
                    row["ReportName"] = "Istoric Tranzactii";
                    ds.Tables[0].Rows.Add(row);
                    break;
                case "DISPONIBIL":
                    row = ds.Tables[0].NewRow();
                    row["ReportName"] = "Raport Sedinta";
                    ds.Tables[0].Rows.Add(row);
                    break;
            }

            return ds;
        }

        public DataSet getReportDataSet(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return null;
            if (vl_arguments["ReportName"] == null) return null;
            /*if (vl_arguments["StartDate"] == null) return null;
            if (vl_arguments["EndDate"] == null) return null;*/

            switch (vl_arguments["ReportName"].AsString)
            {
                case "Istoric Ordine":
                    return getOrdersHistory(vl_arguments);
                case "Istoric Tranzactii":
                    return getTransactionsHistory(vl_arguments);
                case "Raport Sedinta":
                    return getAssetJournal(vl_arguments);
                default:
                    DataSet ds = new DataSet();
                    ds.Tables.Add();
                    for (int i = 0; i < vl_arguments.Count; i++)
                        ds.Tables[0].Columns.Add(vl_arguments[i].Name, Type.GetType("System.String"));

                    DataRow row = ds.Tables[0].NewRow();
                    for (int i = 0; i < vl_arguments.Count; i++)
                        row[vl_arguments[i].Name] = vl_arguments[i].ToString();

                    return ds;
            }
        }

        public DataSet getDISPOrdersReport(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Asset"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = vl_arguments["ID_Asset"].AsInt32;

            return app.DB.Select("select_DISPOrdersReport", "Orders", vl_params);
        }

        public DataSet getOrdersHistory(TVariantList vl_arguments)
        {
            if (vl_arguments["StartDate"] == null) return null;
            if (vl_arguments["EndDate"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Broker").AsInt32 = session.ID_Broker;
            vl_params.Add("@prm_ID_Agency").AsInt32 = session.ID_Agency;
            vl_params.Add("@prm_StartDate").AsDateTime = vl_arguments["StartDate"].AsDateTime;
            vl_params.Add("@prm_EndDate").AsDateTime = vl_arguments["EndDate"].AsDateTime;

            UserValidation uval = new UserValidation(app);
            if (uval.isSupervisor(session.ID_Bursary, session.ID_User, session.ID_UserRole))
                vl_params["@prm_ID_Broker"].AsInt32 = -1;

            switch (session.EntryPoint)
            {
                case "BTGN":
                    return app.DB.Select("select_OrdersHistory", "Orders", vl_params);
                case "DISPONIBIL":
                    return app.DB.Select("select_OrdersHistoryDISPONIBIL", "Orders", vl_params);
            }

            return null;
        }

        public DataSet getTransactionsHistory(TVariantList vl_arguments)
        {
            if (vl_arguments["StartDate"] == null) return null;
            if (vl_arguments["EndDate"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Broker").AsInt32 = session.ID_Broker;
            vl_params.Add("@prm_ID_Agency").AsInt32 = session.ID_Agency;
            vl_params.Add("@prm_StartDate").AsDateTime = vl_arguments["StartDate"].AsDateTime;
            vl_params.Add("@prm_EndDate").AsDateTime = vl_arguments["EndDate"].AsDateTime;

            UserValidation uval = new UserValidation(app);
            if (uval.isSupervisor(session.ID_Bursary, session.ID_User, session.ID_UserRole))
                vl_params["@prm_ID_Broker"].AsInt32 = -1;

            switch (session.EntryPoint)
            {
                case "BTGN":
                    return app.DB.Select("select_TransactionsHistory", "Transactions", vl_params);
                case "DISPONIBIL":
                    return app.DB.Select("select_TransactionsHistoryDISPONIBIL", "Transactions", vl_params);
            }

            return null;

        }

        public DataSet getAssetJournal(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_User").AsInt32 = -1;
            vl_params.Add("@prm_ID_Broker").AsInt32 = -1;
            vl_params.Add("@prm_ID_Agency").AsInt32 = -1;
            vl_params.Add("@prm_ID_Client").AsInt32 = -1;
            vl_params.Add("@prm_ID_Ring").AsInt32 = -1;
            vl_params.Add("@prm_ID_Asset").AsInt32 = -1;
            vl_params.Add("@prm_ID").AsInt32 = -1;

            if (vl_arguments["ID_User"] != null) vl_params["@prm_ID_User"].AsInt32 = vl_arguments["ID_User"].AsInt32;
            if (vl_arguments["ID_Broker"] != null) vl_params["@prm_ID_Broker"].AsInt32 = vl_arguments["ID_Broker"].AsInt32;
            if (vl_arguments["ID_Agency"] != null) vl_params["@prm_ID_Agency"].AsInt32 = vl_arguments["ID_Agency"].AsInt32;
            if (vl_arguments["ID_Client"] != null) vl_params["@prm_ID_Client"].AsInt32 = vl_arguments["ID_Client"].AsInt32;
            if (vl_arguments["ID_Ring"] != null) vl_params["@prm_ID_Ring"].AsInt32 = vl_arguments["ID_Ring"].AsInt32;
            if (vl_arguments["ID_Asset"] != null) vl_params["@prm_ID_Asset"].AsInt32 = vl_arguments["ID_Asset"].AsInt32;
            if (vl_arguments["ID_Journal"] != null) vl_params["@prm_ID"].AsInt32 = vl_arguments["ID_Journal"].AsInt32;

            if (vl_arguments["Since"] != null) vl_params.Add("@prm_Date").AsDateTime = vl_arguments["Since"].AsDateTime;
            else vl_params.Add("@prm_Date").AsDateTime = DateTime.Today.AddMonths(-1);

            DataSet ds = app.DB.Select("select_AssetJournal", "GridReports", vl_params);
            session.LastEventTimestamp = DateTime.Now;
            return ds;
        }

        public DataSet getAssetTypes(TVariantList vl_arguments)
        {
            int idAssetType = -1;
            TVariantList vl_params = new TVariantList();
            if (vl_arguments["ID_AssetType"] != null) vl_params.Add("@prm_ID").AsInt32 = vl_arguments["ID_AssetType"].AsInt32;
            else vl_params.Add("@prm_ID").AsInt32 = idAssetType;

            int idRing = -1;
            if (vl_arguments["ID_Ring"] != null) vl_params.Add("@prm_ID_Ring").AsInt32 = vl_arguments["ID_Ring"].AsInt32;
            else vl_params.Add("@prm_ID_Ring").AsInt32 = idRing;

            DataSet ds = app.DB.Select("select_AssetTypes", "AssetTypes", vl_params);
            return ds;
        }

        public DataSet getInitialOrders(TVariantList vl_arguments)
        {
            int ID_Asset = -1;
            if (vl_arguments["ID_Asset"] != null) ID_Asset = vl_arguments["ID_Asset"].AsInt32;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            DataSet ds = app.DB.Select("select_InitialOrders", "Orders", vl_params);
            return ds;
        }

        public DataSet getAssetsSchedules(TVariantList vl_arguments)
        {
            int ID_Asset = -1;
            if (vl_arguments["ID_Asset"] != null)
            {
                if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
                else return null;
            }

            int ID_Ring = -1;
            if (vl_arguments["ID_Ring"] != null)
            {
                if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
                else return null;
            }

            int ID_AssetSchedule = -1;
            if (vl_arguments["ID_AssetSchedule"] != null)
            {
                if (vl_arguments["ID_AssetSchedule"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_AssetSchedule"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_AssetSchedule"].ValueType == TVariantType.vtInt64) ID_AssetSchedule = vl_arguments["ID_AssetSchedule"].AsInt32;
                else return null;
            }

            TVariantList vl_params = new TVariantList();           
            vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            vl_params.Add("@prm_ID_AssetSchedule").AsInt32 = ID_AssetSchedule;
            DataSet ds = app.DB.Select("select_AssetSchedules", "AssetSchedules", vl_params);
            return ds;
        }

        public DataSet getCurrencies(TVariantList vl_arguments)
        {
            int ID_Currency = -1;
            if (vl_arguments["ID_Currency"] != null)
            {
                if (vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt64) ID_Currency = vl_arguments["ID_Currency"].AsInt32;
                else return null;
            }
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Currency").AsInt32 = ID_Currency;
            vl_params.Add("@prm_Language").AsString = session.Language;

            DataSet ds = app.DB.Select("select_Currencies", "Currencies", vl_params);
            return ds;
            
            //return app.DB.Select("select_Currencies", "Nomenclators", null);
        }

        public DataSet getMeasuringUnits(TVariantList vl_arguments)
        {
            int ID_MeasuringUnit = -1;
            if (vl_arguments["ID_MeasuringUnit"] != null)
            {
                if (vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt64) ID_MeasuringUnit = vl_arguments["ID_MeasuringUnit"].AsInt32;
                else return null;
            }
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_MeasuringUnit").AsInt32 = ID_MeasuringUnit;
            vl_params.Add("@prm_Language").AsString = session.Language;

            DataSet ds = app.DB.Select("select_MeasuringUnits", "MeasuringUnits", vl_params);
            return ds;

            //return app.DB.Select("select_MeasuringUnits", "Nomenclators", null);
        }

        public DataSet getCPVS(TVariantList vl_arguments)
        {
            int ID_CPV = -1;
            if (vl_arguments["ID_CPV"] != null)
            {
                if (vl_arguments["ID_CPV"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_CPV"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_CPV"].ValueType == TVariantType.vtInt64) ID_CPV = vl_arguments["ID_CPV"].AsInt32;
                else return null;
            }

            string Key = "";
            if (vl_arguments["Key"] != null) Key = vl_arguments["Key"].AsString;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_CPV").AsInt32 = ID_CPV;
            vl_params.Add("@prm_Key").AsString = Key;
            DataSet ds = app.DB.Select("select_CPVS", "CPV", vl_params);
            return ds;
        }

        public DataSet getCAENS(TVariantList vl_arguments)
        {
            int ID_CAEN = -1;
            if (vl_arguments["ID_CAEN"] != null)
            {
                if (vl_arguments["ID_CAEN"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_CAEN"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_CAEN"].ValueType == TVariantType.vtInt64) ID_CAEN = vl_arguments["ID_CAEN"].AsInt32;
                else return null;
            }
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_CAEN").AsInt32 = ID_CAEN;
            DataSet ds = app.DB.Select("select_CAENS", "CAEN", vl_params);
            return ds;
        }

        public DataSet getTerminals(TVariantList vl_arguments)
        {
            int ID_Terminal = -1;
            if (vl_arguments["ID_Terminal"] != null)
            {
                if (vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt64) ID_Terminal = vl_arguments["ID_Terminal"].AsInt32;
                else return null;
            }
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Terminal").AsInt32 = ID_Terminal;
            DataSet ds = app.DB.Select("select_Terminals", "Terminals", vl_params);
            return ds;
        }

        public DataSet getAgencyRings(TVariantList vl_arguments)
        {
            int ID_Agency = -1;
            if (vl_arguments["ID_Agency"] != null)
            {
                if (vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt64) ID_Agency = vl_arguments["ID_Agency"].AsInt32;
                else return null;
            }
            TVariantList vl_params = new TVariantList();  
            vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
            DataSet ds = app.DB.Select("select_AgencyRings", "Agencies", vl_params);
            return ds;
        }

        public DataSet getAgencyClients(TVariantList vl_arguments)
        {
            int ID_Agency = -1;
            if (vl_arguments["ID_Agency"] != null)
            {
                if (vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt64) ID_Agency = vl_arguments["ID_Agency"].AsInt32;
                else return null;
            }
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
            DataSet ds = app.DB.Select("select_AgencyClients", "Agencies", vl_params);
            return ds;
        }

        public DataSet getAgencyAssets(TVariantList vl_arguments)
        {
            int ID_Agency = -1;
            if (vl_arguments["ID_Agency"] != null)
            {
                if (vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt64) ID_Agency = vl_arguments["ID_Agency"].AsInt32;
                else return null;
            }

            int ID_Asset = -1;
            if (vl_arguments["ID_Asset"] != null)
            {
                if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
                else return null;
            }

            /*StartDate = DateTime.Today;
            EndDate = DateTime.Today;

            if (vl_arguments["StartDeliveryDate"].ValueType == TVariantType.vtDateTime) StartDeliveryDate = vl_arguments["StartDeliveryDate"].AsDateTime;

            string date = vl_arguments["StartDate"].AsString;*/


            DateTime StartDate = DateTime.Now.AddYears(-50);
            DateTime EndDate = DateTime.Now.AddYears(50);

            if (vl_arguments["StartDate"] != null) StartDate = vl_arguments["StartDate"].AsDateTime;
            if (vl_arguments["EndDate"] != null) EndDate = vl_arguments["EndDate"].AsDateTime;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            vl_params.Add("@prm_StartDate").AsDateTime = StartDate;
            vl_params.Add("@prm_EndDate").AsDateTime = EndDate;

            DataSet ds = app.DB.Select("select_AgencyAssets", "Agencies", vl_params);
            return ds;
        }

        public DataSet getRingAgencies(TVariantList vl_arguments)
        {
            int ID_Ring = -1;
            if (vl_arguments["ID_Ring"] != null)
            {
                if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
                else return null;
            }
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
            DataSet ds = app.DB.Select("select_RingAgencies", "Rings", vl_params);
            return ds;
        }

        public DataSet getRingClients(TVariantList vl_arguments)
        {
            int ID_Ring = -1;
            if (vl_arguments["ID_Ring"] != null)
            {
                if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
                else return null;
            }
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
            DataSet ds = app.DB.Select("select_RingClients", "Rings", vl_params);
            return ds;
        }

        public DataSet getRingAdministrators(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Ring"] == null) return null;

            int ID_Ring = 0;
            ID_Ring = vl_arguments["ID_Ring"].AsInt32;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
            DataSet ds = app.DB.Select("select_RingAdministrators", "Rings", vl_params);
            return ds;
        }

        public DataSet getAssetClients(TVariantList vl_arguments)
        {
            int ID_Asset = -1;
            if (vl_arguments["ID_Asset"] != null)
            {
                if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
                else return null;
            }
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            DataSet ds = app.DB.Select("select_AssetClients", "Rings", vl_params);
            return ds;
        }

        public DataSet getDocumentTypes(TVariantList vl_arguments)
        {
            int ID_DocumentType = -1;
            if (vl_arguments["ID_DocumentType"] != null)
            {
                if (vl_arguments["ID_DocumentType"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_DocumentType"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_DocumentType"].ValueType == TVariantType.vtInt64) ID_DocumentType = vl_arguments["ID_DocumentType"].AsInt32;
                else return null;
            }
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_DocumentType").AsInt32 = ID_DocumentType;
            DataSet ds = app.DB.Select("select_DocumentTypes", "Documents", vl_params);
            return ds;
        }

        public DataSet getDocuments(TVariantList vl_arguments)
        {
            int ID_Document = -1;
            if (vl_arguments["ID_Document"] != null)
            {
                if (vl_arguments["ID_Document"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Document"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Document"].ValueType == TVariantType.vtInt64) ID_Document = vl_arguments["ID_Document"].AsInt32;
                else return null;
            }

            int ID_Asset = -1;
            if (vl_arguments["ID_Asset"] != null)
            {
                if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
                else return null;
            }

            int isPublic = 1;
            if (vl_arguments["all"] != null && vl_arguments["all"].ValueType == TVariantType.vtBoolean)
                if (vl_arguments["all"].AsBoolean) isPublic = -1;


            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Document").AsInt32 = ID_Document;
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            vl_params.Add("@prm_isPublic").AsInt32 = isPublic;
            DataSet ds = app.DB.Select("select_Documents", "Documents", vl_params);
            return ds;
        }

        public DataSet getWarrantyTypes(TVariantList vl_arguments)
        {
            int ID_WarrantyType = -1;
            if (vl_arguments["ID_WarrantyType"] != null)
            {
                if (vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt64) ID_WarrantyType = vl_arguments["ID_WarrantyType"].AsInt32;
                else return null;
            }
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_WarrantyType").AsInt32 = ID_WarrantyType;
            DataSet ds = app.DB.Select("select_WarrantyTypes", "WarrantyTypes", vl_params);
            return ds;
        }

        public DataSet getRingWarrantyTypes(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Ring"] == null) return null;

            int ID_Ring = 0;
            ID_Ring = vl_arguments["ID_Ring"].AsInt32;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
            DataSet ds = app.DB.Select("select_RingWarrantyType", "Rings", vl_params);
            return ds;
        }

        public DataSet getAssetWarrantyTypes(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Asset"] == null) return null;

            int ID_Asset = 0;
            ID_Asset = vl_arguments["ID_Asset"].AsInt32;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            DataSet ds = app.DB.Select("select_AssetWarrantyType", "Rings", vl_params);
            int count = ds.Tables[0].Rows.Count;
            return ds;
        }
        
        //------se modifica actuala getWarranties cu ce e mai jos
        public DataSet getWarranties(TVariantList vl_arguments)
        {
            int ID_BOperation = -1;
            if (vl_arguments["ID_BOperation"] != null)
            {
                if (vl_arguments["ID_BOperation"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_BOperation"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_BOperation"].ValueType == TVariantType.vtInt64) ID_BOperation = vl_arguments["ID_BOperation"].AsInt32;
                else return null;
            }

            int ID_Client = -1;
            if (vl_arguments["ID_Client"] != null)
            {
                if (vl_arguments["ID_Client"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Client"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Client"].ValueType == TVariantType.vtInt64) ID_Client = vl_arguments["ID_Client"].AsInt32;
                else return null;
            }

            int ID_Agency = -1;
            if (vl_arguments["ID_Agency"] != null)
            {
                if (vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt64) ID_Agency = vl_arguments["ID_Agency"].AsInt32;
                else return null;
            }

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_BOperation").AsInt32 = ID_BOperation;
            vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
            vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
            DataSet ds = app.DB.Select("select_BO_Operations", "BO_Operations", vl_params);
            return ds;
        }

        /*public DataSet getWarranties(TVariantList vl_arguments)
        {
            int ID_BOperation = -1;
            if (vl_arguments["ID_BOperation"] != null)
            {
                if (vl_arguments["ID_BOperation"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_BOperation"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_BOperation"].ValueType == TVariantType.vtInt64) ID_BOperation = vl_arguments["ID_BOperation"].AsInt32;
                else return null;
            }
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_BOperation").AsInt32 = ID_BOperation;
            vl_params.Add("@prm_ID_Agency").AsInt32 = -1;
            vl_params.Add("@prm_ID_Client").AsInt32 = -1;
            DataSet ds = app.DB.Select("select_BO_Operations", "BO_Operations", vl_params);
            return ds;
        }*/

        public DataSet getAssetWarranties(TVariantList vl_arguments)
        {
            int ID_Asset = -1;
            if (vl_arguments["ID_Asset"] != null)
            {
                if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
                else return null;
            }

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            DataSet ds = app.DB.Select("select_AssetWarranties", "BO_Operations", vl_params);
            return ds;
        }

        public DataSet getClientWarranties(TVariantList vl_arguments)
        {
            int ID_Client = -1;
            if (vl_arguments["ID_Client"] != null)
            {
                if (vl_arguments["ID_Client"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Client"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Client"].ValueType == TVariantType.vtInt64) ID_Client = vl_arguments["ID_Client"].AsInt32;
                else return null;
            }
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
            vl_params.Add("@prm_ID_BOperation").AsInt32 = -1;
            vl_params.Add("@prm_ID_Agency").AsInt32 = -1;
            DataSet ds = app.DB.Select("select_BO_Operations", "BO_Operations", vl_params);
            return ds;
        }

        public DataSet getClientWarrantySummary(TVariantList vl_arguments)
        {
            int ID_Client = -1;
            if (vl_arguments["ID_Client"] != null)
            {
                if (vl_arguments["ID_Client"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Client"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Client"].ValueType == TVariantType.vtInt64) ID_Client = vl_arguments["ID_Client"].AsInt32;
                else return null;
            }
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
            vl_params.Add("@prm_ID_BOperation").AsInt32 = -1;
            vl_params.Add("@prm_ID_Agency").AsInt32 = -1;
            DataSet ds = app.DB.Select("select_BO_OperationSummary", "BO_Operations", vl_params);
            return ds;
        }

        public DataSet getTranslations(TVariantList vl_arguments)
        {
            int ID_Translation = -1;
            if (vl_arguments["ID_Translation"] != null)
            {
                if (vl_arguments["ID_Translation"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Translation"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Translation"].ValueType == TVariantType.vtInt64) ID_Translation = vl_arguments["ID_Translation"].AsInt32;
                else return null;
            }

            string Label = "";
            if (vl_arguments["Label"] != null)
                if (vl_arguments["Label"].ValueType == TVariantType.vtString) 
                    Label = vl_arguments["Label"].AsString;

            int SysLabels = -1;
            if (vl_arguments["SysLabels"] != null)
                if (vl_arguments["SysLabels"].ValueType == TVariantType.vtBoolean)
                    if (vl_arguments["SysLabels"].AsBoolean == true) SysLabels = 1;
                    else SysLabels = 0;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Translation").AsInt32 = ID_Translation;
            vl_params.Add("@prm_Label").AsString = Label;
            vl_params.Add("@prm_SysLabels").AsInt32 = SysLabels;

            DataSet ds = app.DB.Select("select_Translations", "Translations", vl_params);
            return ds;
        }


        public DataSet getWebRings(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_Market").AsInt32 = -1;
            vl_params.Add("@prm_ID_Ring").AsInt32 = -1;

            if (vl_arguments["ID_Ring"] != null)
                if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64)
                    vl_params["@prm_ID_Ring"].AsInt32 = vl_arguments["ID_Ring"].AsInt32;

            DataSet ds = null;
            if (vl_arguments["Language"] != null)
                switch (vl_arguments["Language"].AsString)
                {
                    case "EN":
                        ds = app.DB.Select("select_WebRings_EN", "Rings", vl_params);
                        break;
                    case "GB":
                        ds = app.DB.Select("select_WebRings_EN", "Rings", vl_params);
                        break;
                    case "RO":
                        ds = app.DB.Select("select_WebRings_RO", "Rings", vl_params);
                        break;
                }
            else ds = app.DB.Select("select_WebRings_EN", "Rings", vl_params);
            return ds;
        }

        public DataSet getWebRingDetails(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Ring"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = vl_arguments["ID_Ring"].AsInt32;

            DataSet ds = null;
            if (vl_arguments["Language"] != null)
                switch (vl_arguments["Language"].AsString)
                {
                    case "EN":
                        ds = app.DB.Select("select_WebRingDetails_EN", "Rings", vl_params);
                        break;
                    case "GB":
                        ds = app.DB.Select("select_WebRingDetails_EN", "Rings", vl_params);
                        break;
                    case "RO":
                        ds = app.DB.Select("select_WebRingDetails_RO", "Rings", vl_params);
                        break;
                }
            else ds = app.DB.Select("select_WebRingDetails_EN", "Rings", vl_params);
            return ds;
        }

        public DataSet getWebAssetTypes(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Ring"] == null) return null;


            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Ring").AsInt32 = vl_arguments["ID_Ring"].AsInt32;

            DataSet ds = null;
            if (vl_arguments["Language"] != null)
                switch (vl_arguments["Language"].AsString)
                {
                    case "EN":
                        ds = app.DB.Select("select_WebAssetTypes_EN", "AssetTypes", vl_params);
                        break;
                    case "GB":
                        ds = app.DB.Select("select_WebAssetTypes_EN", "AssetTypes", vl_params);
                        break;
                    case "RO":
                        ds = app.DB.Select("select_WebAssetTypes_RO", "AssetTypes", vl_params);
                        break;
                }
            else ds = app.DB.Select("select_WebAssetTypes_EN", "AssetTypes", vl_params);
            return ds;
        }

        public DataSet getWebAssets(TVariantList vl_arguments)
        {
            int ID_Ring = -1;
            if (vl_arguments["ID_Ring"] != null)
            {
                if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
                else return null;
            }

            int ID_Asset = -1;
            if (vl_arguments["ID_Asset"] != null)
            {
                if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
                else return null;
            }

            int ID_AssetType = -1;
            if (vl_arguments["ID_AssetType"] != null)
            {
                if (vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt64) ID_AssetType = vl_arguments["ID_AssetType"].AsInt32;
                else return null;
            }

            string Date_Start = "";
            string Date_End = "";
            if (vl_arguments["Date_Start"] != null && vl_arguments["Date_End"] != null)
            {
                Date_Start = vl_arguments["Date_Start"].AsString;
                Date_End = vl_arguments["Date_End"].AsString;
            }

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_Market").AsInt32 = -1;
            vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
            vl_params.Add("@prm_ID").AsInt32 = ID_Asset;
            vl_params.Add("@prm_ID_AssetType").AsInt32 = ID_AssetType;
            vl_params.Add("@prm_Date_Start").AsString = Date_Start;
            vl_params.Add("@prm_Date_End").AsString = Date_End;

            DataSet ds = null;
            if (vl_arguments["Language"] != null)
                switch (vl_arguments["Language"].AsString)
                {
                    case "EN":
                        ds = app.DB.Select("select_WebAssets_EN", "Assets", vl_params);
                        break;
                    case "GB":
                        ds = app.DB.Select("select_WebAssets_EN", "Assets", vl_params);
                        break;
                    case "RO":
                        ds = app.DB.Select("select_WebAssets_RO", "Assets", vl_params);
                        break;
                }
            else ds = app.DB.Select("select_WebAssets_EN", "Assets", vl_params);
            return ds;
        }

        public DataSet getWebAssetDetails(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Asset"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = vl_arguments["ID_Asset"].AsInt32;

            DataSet ds = null;
            if (vl_arguments["Language"] != null)
                switch (vl_arguments["Language"].AsString)
                {
                    case "EN":
                        ds = app.DB.Select("select_WebAssetDetails_EN", "Assets", vl_params);
                        break;
                    case "GB":
                        ds = app.DB.Select("select_WebAssetDetails_EN", "Assets", vl_params);
                        break;
                    case "RO":
                        ds = app.DB.Select("select_WebAssetDetails_RO", "Assets", vl_params);
                        break;
                }
            else ds = app.DB.Select("select_WebAssetDetails_EN", "Assets", vl_params);

            return ds;
        }

        public DataSet getWebAssetDocuments(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Asset"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = vl_arguments["ID_Asset"].AsInt32;
            vl_params.Add("#prm_BaseURL").AsString = "";

            if (app.Database.Contains("Apollo")) vl_params["#prm_BaseURL"].AsString = "http://disponibil.brmconsulting.ro/documents/download/";
            else vl_params["#prm_BaseURL"].AsString = "http://disponibiltest.brmconsulting.ro/documents/download/";

            DataSet ds = null;
            if (vl_arguments["Language"] != null)
                switch (vl_arguments["Language"].AsString)
                {
                    case "EN":
                        ds = app.DB.Select("select_WebAssetDocuments_EN", "Assets", vl_params);
                        break;
                    case "GB":
                        ds = app.DB.Select("select_WebAssetDocuments_EN", "Assets", vl_params);
                        break;
                    case "RO":
                        ds = app.DB.Select("select_WebAssetDocuments_RO", "Assets", vl_params);
                        break;
                }
            else ds = app.DB.Select("select_WebAssetDocuments_EN", "Assets", vl_params);
            return ds;
        }

        public DataSet getWebAssetSessionHistory(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Asset"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = vl_arguments["ID_Asset"].AsInt32;

            DataSet ds = null;
            if (vl_arguments["Language"] != null)
                switch (vl_arguments["Language"].AsString)
                {
                    case "EN":
                        ds = app.DB.Select("select_WebAssetSessionHistory_EN", "Assets", vl_params);
                        break;
                    case "GB":
                        ds = app.DB.Select("select_WebAssetSessionHistory_EN", "Assets", vl_params);
                        break;
                    case "RO":
                        ds = app.DB.Select("select_WebAssetSessionHistory_RO", "Assets", vl_params);
                        break;
                }
            else ds = app.DB.Select("select_WebAssetSessionHistory_EN", "Assets", vl_params);
            return ds;
        }

        public DataSet getWebAssetQuotations(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Asset"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = vl_arguments["ID_Asset"].AsInt32;

            DataSet ds = null;
            if (vl_arguments["Language"] != null)
                switch (vl_arguments["Language"].AsString)
                {
                    case "EN":
                        ds = app.DB.Select("select_WebAssetQuotations_EN", "Assets", vl_params);
                        break;
                    case "GB":
                        ds = app.DB.Select("select_WebAssetQuotations_EN", "Assets", vl_params);
                        break;
                    case "RO":
                        ds = app.DB.Select("select_WebAssetQuotations_RO", "Assets", vl_params);
                        break;
                }
            else ds = app.DB.Select("select_WebAssetQuotations_EN", "Assets", vl_params);

            return ds;
        }

        public DataSet getWebTodayQuotations(TVariantList vl_arguments)
        {
            DataSet ds = null;
            if (vl_arguments["Language"] != null)
                switch (vl_arguments["Language"].AsString)
                {
                    case "EN":
                        ds = app.DB.Select("select_WebTodayQuotations_EN", "Assets", null);
                        break;
                    case "GB":
                        ds = app.DB.Select("select_WebTodayQuotations_EN", "Assets", null);
                        break;
                    case "RO":
                        ds = app.DB.Select("select_WebTodayQuotations_RO", "Assets", null);
                        break;
                }
            else ds = app.DB.Select("select_WebTodayQuotations_EN", "Assets", null);

            return ds;
        }

        public DataSet getWebTerminals(TVariantList vl_arguments)
        {
            int ID_Terminal = -1;
            if (vl_arguments["ID_Terminal"] != null)
            {
                if (vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt64) ID_Terminal = vl_arguments["ID_Terminal"].AsInt32;
                else return null;
            }
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Terminal").AsInt32 = ID_Terminal;

            DataSet ds = null;
            if (vl_arguments["Language"] != null)
                switch (vl_arguments["Language"].AsString)
                {
                    case "GB":
                        ds = app.DB.Select("select_WebTerminals_EN", "Terminals", vl_params);
                        break;
                    case "EN":
                        ds = app.DB.Select("select_WebTerminals_EN", "Terminals", vl_params);
                        break;
                    case "RO":
                        ds = app.DB.Select("select_WebTerminals_RO", "Terminals", vl_params);
                        break;
                }
            else ds = app.DB.Select("select_WebTerminals_EN", "Terminals", vl_params);

            return ds;
        }

        public DataSet getWebAgencies(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_Agency").AsInt32 = -1;

            DataSet ds = null;
            if (vl_arguments["Language"] != null)
                switch (vl_arguments["Language"].AsString)
                {
                    case "GB":
                        ds = app.DB.Select("select_WebAgencies_EN", "Agencies", vl_params);
                        break;
                    case "EN":
                        ds = app.DB.Select("select_WebAgencies_EN", "Agencies", vl_params);
                        break;
                    case "RO":
                        ds = app.DB.Select("select_WebAgencies_RO", "Agencies", vl_params);
                        break;
                }
            else ds = app.DB.Select("select_WebAgencies_EN", "Agencies", vl_params);

            return ds;
        }

        public DataSet getWebNews(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;

            DataSet ds = null;
            ds = app.DB.Select("select_WebJournal", "Journal", vl_params);
            return ds;
        }

        public DataSet getStatistics(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Market").AsInt32 = -1;
            if (vl_arguments["ID_Market"] != null) vl_params["@prm_ID_Market"].AsInt32 = vl_arguments["ID_Market"].AsInt32;

            DataSet ds = null;
            ds = app.DB.Select("select_Statistics", "Transactions", vl_params);
            return ds;
        }

        public DataSet validate(TVariantList vl_arguments)
        {
            //-----------------------------------------------------------------------
            //  check if arguments are provided
            //  generic
            if (vl_arguments["ID_Asset"] == null) return null;
            if (vl_arguments["ID_Market"] == null) return null;

            int ID_Asset = -1;
            if (vl_arguments["ID_Asset"] != null)
            {
                if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
                else return null;
            }

            int ID_Market = -1;
            if (vl_arguments["ID_Market"] != null)
            {
                if (vl_arguments["ID_Market"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Market"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Market"].ValueType == TVariantType.vtInt64) ID_Market = vl_arguments["ID_Market"].AsInt32;
                else return null;
            }

            //-----------------------------------------------------------------------
            //  extract values and validate
            DataSet errors = new DataSet();
            DataTable dtErrors = new DataTable();
            errors.Tables.Add(dtErrors);
            DataColumn column = new DataColumn();
            DataRow row = dtErrors.NewRow();

            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Pas";
            dtErrors.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Mesaj";
            dtErrors.Columns.Add(column);

            //step 1. Verify asset info
            TVariantList vl_params = new TVariantList();
            vl_params.Add("ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("ID_Market").AsInt32 = ID_Market;
            vl_params.Add("ID_Ring").AsInt32 = -1;
            vl_params.Add("ID_Asset").AsInt32 = ID_Asset;
            vl_params.Add("ID_AssetType").AsInt32 = -1;
            vl_params.Add("anystatus").AsBoolean = true;
            vl_params.Add("all").AsBoolean = true;

            DataSet ds_assets = getAssetsDetailed(vl_params);

            if ((!app.DB.ValidDS(ds_assets)) || (!app.DB.ValidDSRows(ds_assets)))
            {
                row = dtErrors.NewRow();
                row["Pas"] = "Activ";
                row["Mesaj"] = "Nu a fost identificat activul!";
                dtErrors.Rows.Add(row);
            }

            //if (ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "Opened" && ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "PreOpened") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            //ID_Asset = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID"]);
            ID_Market = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID_Market"]);
            int ID_Ring = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID_Ring"]);
            int ID_AssetType = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID_AssetType"]);
            string Code = Convert.ToString(ds_assets.Tables[0].Rows[0]["Code"]);
            string Name = Convert.ToString(ds_assets.Tables[0].Rows[0]["Name"]);
            string Description = Convert.ToString(ds_assets.Tables[0].Rows[0]["Description"]);
            int ID_Currency = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID_Currency"]);


            int ID_PaymentCurrency = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID_PaymentCurrency"]);
            int ID_MeasuringUnit = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID_MeasuringUnit"]);
            bool isSpotContract = Convert.ToBoolean(ds_assets.Tables[0].Rows[0]["isSpotContract"]);
            double SpotQuotation = Convert.ToDouble(ds_assets.Tables[0].Rows[0]["SpotQuotation"]);
            int ID_InitialOrder = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID_InitialOrder"]);
            bool inheritRingSchedule = Convert.ToBoolean(ds_assets.Tables[0].Rows[0]["inheritRingSchedule"]);
            string Visibility = Convert.ToString(ds_assets.Tables[0].Rows[0]["Visibility"]);
            bool isActive = Convert.ToBoolean(ds_assets.Tables[0].Rows[0]["isActive"]);
            bool isDeleted = Convert.ToBoolean(ds_assets.Tables[0].Rows[0]["isDeleted"]);
            bool isDefault = Convert.ToBoolean(ds_assets.Tables[0].Rows[0]["isDefault"]);
            bool PartialFlagChangeAllowed = Convert.ToBoolean(ds_assets.Tables[0].Rows[0]["PartialFlagChangeAllowed"]);
            bool InitialPriceMandatory = Convert.ToBoolean(ds_assets.Tables[0].Rows[0]["InitialPriceMandatory"]);
            bool InitialPriceMaintenance = Convert.ToBoolean(ds_assets.Tables[0].Rows[0]["InitialPriceMaintenance"]);
            bool DiminishedQuantityAllowed = Convert.ToBoolean(ds_assets.Tables[0].Rows[0]["DiminishedQuantityAllowed"]);
            bool DiminishedPriceAllowed = Convert.ToBoolean(ds_assets.Tables[0].Rows[0]["DiminishedPriceAllowed"]);
            bool DifferentialPriceAllowed = Convert.ToBoolean(ds_assets.Tables[0].Rows[0]["DifferentialPriceAllowed"]);
            bool OppositeDirectionAllowed = Convert.ToBoolean(ds_assets.Tables[0].Rows[0]["OppositeDirectionAllowed"]);
            int DeltaT = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["DeltaT"]);
            int DeltaT1 = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["DeltaT1"]);


            if (ID_Ring == 0)
            {
                row = dtErrors.NewRow();
                row["Pas"] = "Activ";
                row["Mesaj"] = "Nu a fost selectat ring-ul!";
                dtErrors.Rows.Add(row);
            }

            if (ID_AssetType == 0)
            {
                row = dtErrors.NewRow();
                row["Pas"] = "Activ";
                row["Mesaj"] = "Nu a fost selectat tipul de activ!";
                dtErrors.Rows.Add(row);
            }

            if (Name == "")
            {
                row = dtErrors.NewRow();
                row["Pas"] = "Activ";
                row["Mesaj"] = "Nu a fost introdusa denumirea activului!";
                dtErrors.Rows.Add(row);
            }

            if (Code == "")
            {
                row = dtErrors.NewRow();
                row["Pas"] = "Activ";
                row["Mesaj"] = "Nu a fost introdus codul activului!";
                dtErrors.Rows.Add(row);
            }

            if (ID_MeasuringUnit == 0)
            {
                row = dtErrors.NewRow();
                row["Pas"] = "Activ";
                row["Mesaj"] = "Nu a fost selectata unitatea de masura!";
                dtErrors.Rows.Add(row);
            }

            if (ID_Currency == 0)
            {
                row = dtErrors.NewRow();
                row["Pas"] = "Activ";
                row["Mesaj"] = "Nu a fost selectata moneda!";
                dtErrors.Rows.Add(row);
            }

            if (ID_PaymentCurrency == 0)
            {
                row = dtErrors.NewRow();
                row["Pas"] = "Activ";
                row["Mesaj"] = "Nu a fost selectata moneda de plata!";
                dtErrors.Rows.Add(row);
            }

            if (Visibility == "")
            {
                row = dtErrors.NewRow();
                row["Pas"] = "Activ";
                row["Mesaj"] = "Nu a fost selectat statusul activului!";
                dtErrors.Rows.Add(row);
            }

            if (Visibility != "public")
            {
                row = dtErrors.NewRow();
                row["Pas"] = "Activ";
                row["Mesaj"] = "Activul nu a fost facut public, nu se va genera automat sedinta pentru acesta!";
                dtErrors.Rows.Add(row);
            }

            //step 2. Verify initial order info
            int ID_RingSession = 0;
            int ID_AssetSession = 0;
            int ID_Agency = 0;
            int ID_Broker = 0;
            int ID_Client = 0;
            bool Direction = false;
            double Quantity = 0;
            double Price = 0;
            bool PartialFlag = false;

            vl_params = new TVariantList();
            vl_params.Add("ID_Asset").AsInt32 = ID_Asset;
            DataSet ds_initialOrders = getInitialOrders(vl_params);
            if (app.DB.ValidDSRows(ds_initialOrders))
            {
                DateTime Date = Convert.ToDateTime(ds_initialOrders.Tables[0].Rows[0]["Date"]);
                ID_RingSession = Convert.ToInt32(ds_initialOrders.Tables[0].Rows[0]["ID_RingSession"]);
                ID_AssetSession = Convert.ToInt32(ds_initialOrders.Tables[0].Rows[0]["ID_AssetSession"]);
                ID_Agency = Convert.ToInt32(ds_initialOrders.Tables[0].Rows[0]["ID_Agency"]);
                ID_Broker = Convert.ToInt32(ds_initialOrders.Tables[0].Rows[0]["ID_Broker"]);
                ID_Client = Convert.ToInt32(ds_initialOrders.Tables[0].Rows[0]["ID_Client"]);
                Direction = Convert.ToBoolean(ds_initialOrders.Tables[0].Rows[0]["Direction"]);
                Quantity = Convert.ToDouble(ds_initialOrders.Tables[0].Rows[0]["Quantity"]);
                Price = double.NaN;
                if (ds_initialOrders.Tables[0].Rows[0]["Price"] != DBNull.Value)
                    Price = Convert.ToDouble(ds_initialOrders.Tables[0].Rows[0]["Price"]);
                PartialFlag = Convert.ToBoolean(ds_initialOrders.Tables[0].Rows[0]["PartialFlag"]);
                DateTime ExpirationDate = Convert.ToDateTime(ds_initialOrders.Tables[0].Rows[0]["ExpirationDate"]);
                bool isTransacted = Convert.ToBoolean(ds_initialOrders.Tables[0].Rows[0]["isTransacted"]);
                bool isSuspended = Convert.ToBoolean(ds_initialOrders.Tables[0].Rows[0]["isSuspended"]);
                isActive = Convert.ToBoolean(ds_initialOrders.Tables[0].Rows[0]["isActive"]);
                bool isCanceled = Convert.ToBoolean(ds_initialOrders.Tables[0].Rows[0]["isSuspended"]);
                bool isApproved = Convert.ToBoolean(ds_initialOrders.Tables[0].Rows[0]["isSuspended"]);

                if (Quantity <= 0)
                {
                    row = dtErrors.NewRow();
                    row["Pas"] = "Ordin Initiator";
                    row["Mesaj"] = "Cantitatea ordinului trebuie sa fie pozitiva!";
                    dtErrors.Rows.Add(row);
                }

                if (ExpirationDate < DateTime.Now)
                {
                    row = dtErrors.NewRow();
                    row["Pas"] = "Ordin Initiator";
                    row["Mesaj"] = "Ordinul initiator este expirat!";
                    dtErrors.Rows.Add(row);
                }

                if (ID_Agency == 0)
                {
                    row = dtErrors.NewRow();
                    row["Pas"] = "Ordin Initiator";
                    row["Mesaj"] = "Nu a fost selectata agentia pentru ordinul initiator!";
                    dtErrors.Rows.Add(row);
                }

                if (ID_Client == 0)
                {
                    row = dtErrors.NewRow();
                    row["Pas"] = "Ordin Initiator";
                    row["Mesaj"] = "Nu a fost selectat clientul pentru ordinul initiator!";
                    dtErrors.Rows.Add(row);
                }

            }

            //step 3. Verify asset session
            vl_params = new TVariantList();
            vl_params.Add("ID_Asset").AsInt32 = ID_Asset;
            DataSet ds_assetschedules = getAssetsSchedules(vl_params);

            if ((app.DB.ValidDS(ds_assetschedules)))
            {
                if (app.DB.ValidDSRows(ds_assetschedules))
                {
                    DateTime StartDate = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["StartDate"]);
                    DateTime EndDate = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["EndDate"]);
                    byte DaysOfWeek = Convert.ToByte(ds_assetschedules.Tables[0].Rows[0]["DaysOfWeek"]);
                    /*DateTime PreOpeningTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["PreOpeningTime"].ToString());
                    DateTime OpeningTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["OpeningTime"].ToString());
                    DateTime PreClosingTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["PreClosingTime"].ToString());
                    DateTime ClosingTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["ClosingTime"].ToString());*/
                    TimeSpan PreOpeningTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["PreOpeningTime"].ToString()).TimeOfDay;
                    TimeSpan OpeningTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["OpeningTime"].ToString()).TimeOfDay;
                    TimeSpan PreClosingTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["PreClosingTime"].ToString()).TimeOfDay;
                    TimeSpan ClosingTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["ClosingTime"].ToString()).TimeOfDay;
                    bool isElectronicSession = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["isElectronicSession"]);
                    bool launchAutomatically = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["launchAutomatically"]);
                    double QuantityStepping = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["QuantityStepping"]);
                    double MinQuantity = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["MinQuantity"]);
                    double PriceStepping = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["PriceStepping"]);
                    double MaxPriceVariation = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["MaxPriceVariation"]);
                    double MinPrice = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["MinPrice"]);
                    double MaxPrice = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["MaxPrice"]);
                    bool daySunday = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["daySunday"]);
                    bool dayMonday = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["dayMonday"]);
                    bool dayTuesday = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["dayTuesday"]);
                    bool dayWednesday = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["dayWednesday"]);
                    bool dayThursday = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["dayThursday"]);
                    bool dayFriday = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["dayFriday"]);
                    bool daySaturday = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["daySaturday"]);
                    DeltaT = Convert.ToInt32(ds_assetschedules.Tables[0].Rows[0]["DeltaT"]);
                    DeltaT1 = Convert.ToInt32(ds_assetschedules.Tables[0].Rows[0]["DeltaT1"]);
                    DateTime EndDateTime = DateTime.Parse(Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["EndDate"].ToString()).Date.ToString("d") + " " + ds_assetschedules.Tables[0].Rows[0]["ClosingTime"].ToString());

                    TimeSpan defaultTime = new TimeSpan(0, 0, 0, 0, 0);
                    if ((TimeSpan.Compare(PreOpeningTime, defaultTime) == 0) && (TimeSpan.Compare(OpeningTime, defaultTime) == 0) && (TimeSpan.Compare(PreClosingTime, defaultTime) == 0) && (TimeSpan.Compare(ClosingTime, defaultTime) == 0))
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Sedinta Activ";
                        row["Mesaj"] = "Va rugam sa completati timpii de desfasurare ai sedintei. Conditia este urmatoarea: T0 <= T1 < T2 <= T3";
                        dtErrors.Rows.Add(row);
                    }
                    else if (!(PreOpeningTime <= OpeningTime) || !(OpeningTime < PreClosingTime) || !(PreClosingTime <= ClosingTime))
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Sedinta Activ";
                        row["Mesaj"] = "Va rugam verificati timpii de desfasurare ai sedintei. Conditia este urmatoarea: T0 <= T1 < T2 <= T3";
                        dtErrors.Rows.Add(row);
                    }

                    if (EndDateTime < DateTime.Now)
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Sedinta Activ";
                        row["Mesaj"] = "Sedinta activului este expirata!";
                        dtErrors.Rows.Add(row);
                    }

                    if (DeltaT < 0)
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Sedinta Activ";
                        row["Mesaj"] = "DeltaT este negativ!";
                        dtErrors.Rows.Add(row);
                    }

                    if (DeltaT1 < 0)
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Sedinta Activ";
                        row["Mesaj"] = "DeltaT este negativ!";
                        dtErrors.Rows.Add(row);
                    }

                    if (QuantityStepping < 0)
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Sedinta Activ";
                        row["Mesaj"] = "Pasul de incrementare cantitate este negativ!";
                        dtErrors.Rows.Add(row);
                    }

                    if (MinQuantity < 0)
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Sedinta Activ";
                        row["Mesaj"] = "Cantitatea minima este negativa!";
                        dtErrors.Rows.Add(row);
                    }

                    if (PriceStepping < 0)
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Sedinta Activ";
                        row["Mesaj"] = "Incrementarea de pret este negativa!";
                        dtErrors.Rows.Add(row);
                    }

                    if (MaxPriceVariation < 0)
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Sedinta Activ";
                        row["Mesaj"] = "Variatia maxima de pret este negativa!";
                        dtErrors.Rows.Add(row);
                    }

                    if (MinPrice < 0)
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Sedinta Activ";
                        row["Mesaj"] = "Pretul minim este negativ!";
                        dtErrors.Rows.Add(row);
                    }

                    if (MaxPrice < 0)
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Sedinta Activ";
                        row["Mesaj"] = "Pretul maxim este negativ!";
                        dtErrors.Rows.Add(row);
                    }

                    if ((daySunday == dayMonday) && (dayMonday == dayTuesday) && (dayTuesday == dayWednesday) && (dayWednesday == dayThursday) && (dayThursday == dayFriday) && (dayFriday == daySaturday) && (daySaturday == false))
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Sedinta Activ";
                        row["Mesaj"] = "Sedinta nu este programata in nicio zi a saptamanii!";
                        dtErrors.Rows.Add(row);
                    }


                    vl_params = new TVariantList();
                    vl_params.Add("ID_Market").AsInt32 = ID_Market;
                    DataSet ds_rings = getRings(vl_params);
                    if ((app.DB.ValidDS(ds_rings)))
                    {
                        if (app.DB.ValidDSRows(ds_rings))
                        {
                            TimeSpan RingPreOpeningTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["PreOpeningTime"].ToString()).TimeOfDay;
                            TimeSpan RingOpeningTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["OpeningTime"].ToString()).TimeOfDay;
                            TimeSpan RingPreClosingTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["PreClosingTime"].ToString()).TimeOfDay;
                            TimeSpan RingClosingTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["ClosingTime"].ToString()).TimeOfDay;
                            if (PreOpeningTime < RingPreOpeningTime)
                            {
                                row = dtErrors.NewRow();
                                row["Pas"] = "Sedinta Activ";
                                row["Mesaj"] = "Sedinta pe activ nu se poate predeschide inainte de predeschiderea ring-ului!";
                                dtErrors.Rows.Add(row);
                            }

                            if (RingOpeningTime < OpeningTime)
                            {
                                row = dtErrors.NewRow();
                                row["Pas"] = "Sedinta Activ";
                                row["Mesaj"] = "Sedinta pe activ nu se poate deschide inainte de deschiderea ring-ului!";
                                dtErrors.Rows.Add(row);
                            }

                            if (RingPreClosingTime > PreClosingTime)
                            {
                                row = dtErrors.NewRow();
                                row["Pas"] = "Sedinta Activ";
                                row["Mesaj"] = "Sedinta pe activ nu se poate preinchide dupa preinchiderea ring-ului!";
                                dtErrors.Rows.Add(row);
                            }

                            if (RingClosingTime > ClosingTime)
                            {
                                row = dtErrors.NewRow();
                                row["Pas"] = "Sedinta Activ";
                                row["Mesaj"] = "Sedinta pe activ nu se poate inchide dupa inchiderea ring-ului!";
                                dtErrors.Rows.Add(row);
                            }
                        }
                    }
                }
                else
                {
                    row = dtErrors.NewRow();
                    row["Pas"] = "Sedinta Activ";
                    row["Mesaj"] = "Nu a fost definita sesiune pentru activ!";
                    dtErrors.Rows.Add(row);
                }
            }
            else
            {
                row = dtErrors.NewRow();
                row["Pas"] = "Sedinta Activ";
                row["Mesaj"] = "Nu a fost definita sesiune pentru activ!";
                dtErrors.Rows.Add(row);
            }


            //step 4. Verify trade params
            vl_params = new TVariantList();
            vl_params.Add("ID_Asset").AsInt32 = ID_Asset;
            DataSet ds_tradeparameters = getAssetTradeParameters(vl_params);

            if (app.DB.ValidDS(ds_tradeparameters))
            {
                if (app.DB.ValidDSRows(ds_tradeparameters))
                {
                    Double SellWarrantyPercent = Convert.ToDouble(ds_tradeparameters.Tables[0].Rows[0]["SellWarrantyPercent"]);
                    Double SellWarrantyMU = Convert.ToDouble(ds_tradeparameters.Tables[0].Rows[0]["SellWarrantyMU"]);
                    Double SellWarrantyFixed = Convert.ToDouble(ds_tradeparameters.Tables[0].Rows[0]["SellWarrantyFixed"]);
                    Double BuyWarrantyPercent = Convert.ToDouble(ds_tradeparameters.Tables[0].Rows[0]["BuyWarrantyPercent"]);
                    Double BuyWarrantyMU = Convert.ToDouble(ds_tradeparameters.Tables[0].Rows[0]["BuyWarrantyMU"]);
                    Double BuyWarrantyFixed = Convert.ToDouble(ds_tradeparameters.Tables[0].Rows[0]["BuyWarrantyFixed"]);

                    if (SellWarrantyPercent < 0)
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Parametri Tranzactionare";
                        row["Mesaj"] = "Procent garantie la vanzare este negativ!";
                        dtErrors.Rows.Add(row);
                    }

                    if (SellWarrantyPercent < 0)
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Parametri Tranzactionare";
                        row["Mesaj"] = "Valoare garantie pe UM la vanzare este negativ!";
                        dtErrors.Rows.Add(row);
                    }

                    if (SellWarrantyPercent < 0)
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Parametri Tranzactionare";
                        row["Mesaj"] = "Parte fixa garantie la vanzare este negativ!";
                        dtErrors.Rows.Add(row);
                    }

                    if (SellWarrantyPercent < 0)
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Parametri Tranzactionare";
                        row["Mesaj"] = "Procent garantie la cumparare este negativ!";
                        dtErrors.Rows.Add(row);
                    }

                    if (SellWarrantyPercent < 0)
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Parametri Tranzactionare";
                        row["Mesaj"] = "Valoare garantie pe UM la cumparare este negativ!";
                        dtErrors.Rows.Add(row);
                    }

                    if (SellWarrantyPercent < 0)
                    {
                        row = dtErrors.NewRow();
                        row["Pas"] = "Parametri Tranzactionare";
                        row["Mesaj"] = "Parte fixa garantie la cumparare este negativ!";
                        dtErrors.Rows.Add(row);
                    }
                }
            }

            //step 5. Verify clients
            vl_params = new TVariantList();
            vl_params.Add("ID_Asset").AsInt32 = ID_Asset;
            DataSet ds_clients = getAssetClients(vl_params);
            List<int> buyClients = new List<int>();
            List<int> sellClients = new List<int>();

            if (app.DB.ValidDS(ds_clients))
            {
                if (app.DB.ValidDSRows(ds_clients))
                {
                    for (int i = 0; i < ds_clients.Tables[0].Rows.Count; i++)
                    {
                        bool canBuy = Convert.ToBoolean(ds_clients.Tables[0].Rows[i]["canBuy"]);
                        bool canSell = Convert.ToBoolean(ds_clients.Tables[0].Rows[i]["canSell"]);

                        if (canBuy)
                            buyClients.Add(Convert.ToInt32(ds_clients.Tables[0].Rows[i]["ID_Client"]));

                        if (canSell)
                            sellClients.Add(Convert.ToInt32(ds_clients.Tables[0].Rows[i]["ID_Client"]));
                    }

                    if (ID_InitialOrder == 0) //double trouble
                    {
                        if (buyClients.Count <= 0)
                        {
                            row = dtErrors.NewRow();
                            row["Pas"] = "Clienti";
                            row["Mesaj"] = "Nu au fost setati clienti cu drept de cumparare pentru sedinta de tip dublu competitiva!";
                            dtErrors.Rows.Add(row);
                        }

                        if (sellClients.Count <= 0)
                        {
                            row = dtErrors.NewRow();
                            row["Pas"] = "Clienti";
                            row["Mesaj"] = "Nu au fost setati clienti cu drept de vanzare pentru sedinta de tip dublu competitiva!";
                            dtErrors.Rows.Add(row);
                        }
                    }
                    else //simply
                    {
                        switch (Direction)
                        {
                            case false: //"B"
                                Direction = false;
                                if (buyClients.Count > 1)
                                {
                                    row = dtErrors.NewRow();
                                    row["Pas"] = "Clienti";
                                    row["Mesaj"] = "Ati selectat mai mult de un participant pe sensul de cumparare!";
                                    dtErrors.Rows.Add(row);
                                }
                                if (sellClients.Count < 1)
                                {
                                    row = dtErrors.NewRow();
                                    row["Pas"] = "Clienti";
                                    row["Mesaj"] = "Nu ati selectat participanti pe sensul de vanzare!";
                                    dtErrors.Rows.Add(row);
                                }
                                break;
                            case true:  //"S"
                                if (sellClients.Count > 1)
                                {
                                    row = dtErrors.NewRow();
                                    row["Pas"] = "Clienti";
                                    row["Mesaj"] = "Ati selectat mai mult de un participant pe sensul de vanzare!";
                                    dtErrors.Rows.Add(row);
                                }
                                if (buyClients.Count < 1)
                                {
                                    row = dtErrors.NewRow();
                                    row["Pas"] = "Clienti";
                                    row["Mesaj"] = "Nu ati selectat participanti pe sensul de cumparare!";
                                    dtErrors.Rows.Add(row);
                                }

                                break;
                        }
                    }

                }
                else
                {
                    row = dtErrors.NewRow();
                    row["Pas"] = "Clienti";
                    row["Mesaj"] = "Nu ati selectat participanti pentru acest activ!";
                    dtErrors.Rows.Add(row);
                }
            }
            else
            {
                row = dtErrors.NewRow();
                row["Pas"] = "Clienti";
                row["Mesaj"] = "Nu ati selectat participanti pentru acest activ!";
                dtErrors.Rows.Add(row);
            }

            //step 6. Documents
            vl_params = new TVariantList();
            vl_params.Add("ID_Asset").AsInt32 = ID_Asset;
            DataSet ds_documents = getDocuments(vl_params);
            if (!(app.DB.ValidDS(ds_documents)) || (!app.DB.ValidDSRows(ds_documents)))
            {
                row = dtErrors.NewRow();
                row["Pas"] = "Documente";
                row["Mesaj"] = "Nu au fost atasate documente la sedinta";
                dtErrors.Rows.Add(row);
            }
            //-----------------------------

            //step 7. Verify warranty
            if (ID_InitialOrder > 0)
            {
                //string s = validateWarranty(ID_InitialOrder, ID_Asset, ID_AssetSession, ID_Broker, ID_Client, Direction, Quantity, Price, PartialFlag, DateTime.Now);
                string s = "";
                if (s != "")
                {
                    row = dtErrors.NewRow();
                    row["Pas"] = "Warranty";
                    row["Mesaj"] = "Nu au fost atasate documente la sedinta";
                    dtErrors.Rows.Add(row);
                }
            }

            return errors;
        }
        public DataSet getOperationsBuffer(TVariantList vl_arguments)
        {
            int ID_BOperationBuffer = -1;
            if (vl_arguments["ID_BOperationBuffer"] != null)
            {
                if (vl_arguments["ID_BOperationBuffer"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_BOperationBuffer"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_BOperationBuffer"].ValueType == TVariantType.vtInt64) ID_BOperationBuffer = vl_arguments["ID_BOperationBuffer"].AsInt32;
                else return null;
            }

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_BOperationBuffer").AsInt32 = ID_BOperationBuffer;
            DataSet ds = app.DB.Select("select_BO_OperationsBuffer", "BO_OperationsBuffer", vl_params);
            return ds;
        }

        public DataSet getContractTypes(TVariantList vl_arguments)
        {
            return app.DB.Select("select_ContractTypes", "Nomenclators", null);
        }

        public DataSet getProcedureTypes(TVariantList vl_arguments)
        {
            return app.DB.Select("select_ProcedureTypes", "Nomenclators", null);
        }

        public DataSet getProcedureCriteria(TVariantList vl_arguments)
        {
            return app.DB.Select("select_ProcedureCriteria", "Nomenclators", null);
        }

        public DataSet getProcedureLegislations(TVariantList vl_arguments)
        {
            return app.DB.Select("select_ProcedureLegislations", "Nomenclators", null);
        }

        public DataSet getProcedures(TVariantList vl_arguments)
        {
            int ID_Procedure = -1;
            int ID_ContractType = -1;
            int ID_ProcedureType = -1;
            int ID_ProcedureCriterion = -1;
            string Status = "";

            if (vl_arguments["ID_Procedure"] != null) ID_Procedure = vl_arguments["ID_Procedure"].AsInt32;
            if (vl_arguments["ID_ContractType"] != null) ID_ContractType = vl_arguments["ID_ContractType"].AsInt32;
            if (vl_arguments["ID_ProcedureType"] != null) ID_ProcedureType = vl_arguments["ID_ProcedureType"].AsInt32;
            if (vl_arguments["ID_ProcedureCriterion"] != null) ID_ProcedureCriterion = vl_arguments["ID_ProcedureCriterion"].AsInt32;
            if (vl_arguments["Status"] != null) Status = vl_arguments["Status"].AsString;

            string SortField = "Date";
            if (vl_arguments["SortField"] != null)
                if (vl_arguments["SortField"].ValueType == TVariantType.vtString)
                {
                    SortField = vl_arguments["SortField"].AsString;
                    if (SortField.Contains('=') || SortField.Contains('(') || SortField.Contains(')'))
                        SortField = "Date";
                }

            string SortOrder = "DESC";
            if (vl_arguments["SortOrder"] != null)
                if (vl_arguments["SortOrder"].ValueType == TVariantType.vtString)
                {
                    SortOrder = vl_arguments["SortOrder"].AsString;
                    if (SortOrder.Contains('=') || SortOrder.Contains('(') || SortOrder.Contains(')'))
                        SortOrder = "DESC";
                }

            int QueryOffset = 0;
            if (vl_arguments["QueryOffset"] != null)
                if (vl_arguments["QueryOffset"].ValueType == TVariantType.vtInt16 || vl_arguments["QueryOffset"].ValueType == TVariantType.vtInt32 || vl_arguments["QueryOffset"].ValueType == TVariantType.vtInt64)
                    QueryOffset = vl_arguments["QueryOffset"].AsInt32;

            int QueryLimit = 200;
            if (vl_arguments["QueryLimit"] != null)
                if (vl_arguments["QueryLimit"].ValueType == TVariantType.vtInt16 || vl_arguments["QueryLimit"].ValueType == TVariantType.vtInt32 || vl_arguments["QueryLimit"].ValueType == TVariantType.vtInt64)
                    QueryLimit = vl_arguments["QueryLimit"].AsInt32;

            string QueryKeyword = "";
            if (vl_arguments["QueryKeyword"] != null)
                if (vl_arguments["QueryKeyword"].ValueType == TVariantType.vtString)
                {
                    QueryKeyword = vl_arguments["QueryKeyword"].AsString;
                    if (QueryKeyword.Contains('=') || QueryKeyword.Contains('(') || QueryKeyword.Contains(')'))
                        QueryKeyword = "";
                }


            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Procedure").AsInt32 = ID_Procedure;
            vl_params.Add("@prm_ID_ContractType").AsInt32 = ID_ContractType;
            vl_params.Add("@prm_ID_ProcedureType").AsInt32 = ID_ProcedureType;
            vl_params.Add("@prm_ID_ProcedureCriterion").AsInt32 = ID_ProcedureCriterion;
            vl_params.Add("@prm_Status").AsString = Status;
            //  sorting, searching and limiting arguments
            vl_params.Add("#prm_SortField").AsString = SortField;
            vl_params.Add("#prm_SortOrder").AsString = SortOrder;
            vl_params.Add("@prm_QueryOffset").AsInt32 = QueryOffset;
            vl_params.Add("#prm_QueryLimit").AsString = QueryLimit.ToString();
            vl_params.Add("@prm_Language").AsString = session.Language;
            vl_params.Add("@prm_QueryKeyword").AsString = QueryKeyword;

            return app.DB.Select("select_Procedures", "Procedures", vl_params);
        }

        public DataSet getExportTemplates(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Procedure"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Procedure").AsInt32 = vl_arguments["ID_Procedure"].AsInt32;
            return app.DB.Select("select_ProcedureExportTemplates", "ProcedureExportTemplates", vl_params);
        }

        public DataSet getExportTemplate(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Procedure"] == null && vl_arguments["ID_ExportTemplate"] == null && vl_arguments["Step"] == null) return null;

            int ID_Procedure = -1;
            if (vl_arguments["ID_Procedure"] != null) ID_Procedure = vl_arguments["ID_Procedure"].AsInt32;
            
            int Step = -1;
            if (vl_arguments["Step"] != null) Step = vl_arguments["Step"].AsInt32;

            int ID_ExportTemplate = -1;
            if (vl_arguments["ID_ExportTemplate"] != null) ID_ExportTemplate = vl_arguments["ID_ExportTemplate"].AsInt32;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Procedure").AsInt32 = ID_Procedure;
            vl_params.Add("@prm_Step").AsInt32 = Step;
            vl_params.Add("@prm_ID_ExportTemplate").AsInt32 = ID_ExportTemplate;
            return app.DB.Select("select_ExportTemplate", "ProcedureExportTemplates", vl_params);
        }

        public DataSet getProcedureDocuments(TVariantList vl_arguments)
        {
            int ID_Document = -1;
            if (vl_arguments["ID_Document"] != null)
            {
                if (vl_arguments["ID_Document"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Document"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Document"].ValueType == TVariantType.vtInt64) ID_Document = vl_arguments["ID_Document"].AsInt32;
                else return null;
            }

            int ID_Procedure = -1;
            if (vl_arguments["ID_Procedure"] != null)
            {
                if (vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt64) ID_Procedure = vl_arguments["ID_Procedure"].AsInt32;
                else return null;
            }

            int isPublic = 1;
            if (vl_arguments["all"] != null && vl_arguments["all"].ValueType == TVariantType.vtBoolean)
                if (vl_arguments["all"].AsBoolean) isPublic = -1;


            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Document").AsInt32 = ID_Document;
            vl_params.Add("@prm_ID_Procedure").AsInt32 = ID_Procedure;
            vl_params.Add("@prm_isPublic").AsInt32 = isPublic;
            DataSet ds = app.DB.Select("select_ProcedureDocuments", "ProcedureDocuments", vl_params);
            return ds;
        }

        public DataSet getProcedureVariables(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Procedure"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Procedure").AsInt32 = vl_arguments["ID_Procedure"].AsInt32;
            return app.DB.Select("select_ProcedureVariables", "Procedures", vl_params);
        }

        public DataSet getFavouriteProcedures(TVariantList vl_arguments)
        {
            int ID_Broker = session.ID_Broker;
            int ID_Client = 0;
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
            DataSet ds_client = app.DB.Select("select_HousebyID_Broker", "Clients", vl_params);
            if (app.DB.ValidDSRows(ds_client))
            {
                ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
            }

            vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
            return app.DB.Select("select_FavouriteProcedures", "FavouriteProcedures", vl_params);
        }

        public DataSet getProcedureParticipants(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Procedure"] == null) return null;

            int ID_Procedure = vl_arguments["ID_Procedure"].AsInt32;
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Procedure").AsInt32 = ID_Procedure;
            DataSet ds = app.DB.Select("select_ProcedureParticipants", "Procedures", vl_params);
            return ds;
        }

        public DataSet getParticipantProcedures(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Agency").AsInt32 = session.ID_Agency;
            DataSet ds = app.DB.Select("select_ParticipantProcedures", "Procedures", vl_params);
            return ds;
        }

        public DataSet getForms(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Step").AsInt32 = -1;
            vl_params.Add("@prm_isActive").AsInt32 = -1;

            if (vl_arguments["Step"] != null) vl_params["@prm_Step"].AsInt32 = vl_arguments["Step"].AsInt32;
            if (vl_arguments["isActive"] != null)
            {
                bool isActive = vl_params["isActive"].AsBoolean;
                if (isActive) vl_params["@prm_isActive"].AsInt32 = 1;
                else vl_params["@prm_isActive"].AsInt32 = 0;
            }

            return app.DB.Select("select_Forms", "Forms", vl_params);
        }

        public DataSet getREMITDataSources(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_DataSourceType").AsString = "";
            if (vl_arguments["DataSourceType"] != null) 
                vl_params["@prm_DataSourceType"].AsString = vl_arguments["DataSourceType"].AsString;

            return app.DB.Select("select_REMIT_DataSources", "REMIT_DataSources", vl_params);
        }

        public DataSet getREMITContractNames(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = -1;

            if (vl_arguments["ID_ContractName"] != null) vl_params["@prm_ID"].AsInt32 = vl_arguments["ID_ContractName"].AsInt32;

            return app.DB.Select("select_REMIT_ContractNames", "REMIT_ContractNames", vl_params);
        }

        public DataSet getREMITContractTypes(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = -1;
            vl_params.Add("@prm_enableTable1").AsInt32 = -1;
            vl_params.Add("@prm_enableTable2").AsInt32 = -1;

            if (vl_arguments["ID_ContractType"] != null) vl_params["@prm_ID"].AsInt32 = vl_arguments["ID_ContractType"].AsInt32;
            if (vl_arguments["enableTable1"] != null) vl_params["@prm_enableTable1"].AsBoolean = vl_arguments["enableTable1"].AsBoolean;
            if (vl_arguments["enableTable2"] != null) vl_params["@prm_enableTable2"].AsBoolean = vl_arguments["enableTable2"].AsBoolean;

            return app.DB.Select("select_REMIT_ContractTypes", "REMIT_ContractTypes", vl_params);
        }

        public DataSet getREMITLoadTypes(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = -1;

            if (vl_arguments["ID_LoadType"] != null) vl_params["@prm_ID"].AsInt32 = vl_arguments["ID_LoadType"].AsInt32;

            return app.DB.Select("select_REMIT_LoadTypes", "REMIT_LoadTypes", vl_params);
        }

        public DataSet getREMITDataSourceHistoryXLS(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_QueryKeyword").AsString = "";

            if (vl_arguments["QueryKeyword"] != null)
                if (vl_arguments["QueryKeyword"].ValueType == TVariantType.vtString)
                {
                    string s_keyword = vl_arguments["QueryKeyword"].AsString;
                    if (!s_keyword.Contains('=') && !s_keyword.Contains('(') && !s_keyword.Contains(')'))
                        vl_params["@prm_QueryKeyword"].AsString = s_keyword;
                }

            return app.DB.Select("select_REMIT_DataSourceHistory_XLS", "REMIT_DataSourceHistory", vl_params);
        }

        public DataSet getREMITDataSourceHistoryTable1Reports(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Agency").AsInt32 = 0;
            vl_params.Add("@prm_QueryKeyword").AsString = "";
            if (vl_arguments["ID_Agency"] != null) vl_params["@prm_ID_Agency"].AsInt32 = vl_arguments["ID_Agency"].AsInt32;

            if (vl_arguments["QueryKeyword"] != null)
                if (vl_arguments["QueryKeyword"].ValueType == TVariantType.vtString)
                {
                    string s_keyword = vl_arguments["QueryKeyword"].AsString;
                    if (!s_keyword.Contains('=') && !s_keyword.Contains('(') && !s_keyword.Contains(')'))
                        vl_params["@prm_QueryKeyword"].AsString = s_keyword;
                }

            return app.DB.Select("select_REMIT_DataSourceHistory_Table1Reports", "REMIT_DataSourceHistory", vl_params);
        }

        public DataSet getREMITDataSourceHistoryTable2Reports(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Agency").AsInt32 = 0;
            vl_params.Add("@prm_QueryKeyword").AsString = "";
            if (vl_arguments["ID_Agency"] != null) vl_params["@prm_ID_Agency"].AsInt32 = vl_arguments["ID_Agency"].AsInt32;

            if (vl_arguments["QueryKeyword"] != null)
                if (vl_arguments["QueryKeyword"].ValueType == TVariantType.vtString)
                {
                    string s_keyword = vl_arguments["QueryKeyword"].AsString;
                    if (!s_keyword.Contains('=') && !s_keyword.Contains('(') && !s_keyword.Contains(')'))
                        vl_params["@prm_QueryKeyword"].AsString = s_keyword;
                }

            return app.DB.Select("select_REMIT_DataSourceHistory_Table2Reports", "REMIT_DataSourceHistory", vl_params);
        }

        public DataSet getREMITDataSourceHistoryStorageReports(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Agency").AsInt32 = 0;
            vl_params.Add("@prm_QueryKeyword").AsString = "";
            if (vl_arguments["ID_Agency"] != null) vl_params["@prm_ID_Agency"].AsInt32 = vl_arguments["ID_Agency"].AsInt32;

            if (vl_arguments["QueryKeyword"] != null)
                if (vl_arguments["QueryKeyword"].ValueType == TVariantType.vtString)
                {
                    string s_keyword = vl_arguments["QueryKeyword"].AsString;
                    if (!s_keyword.Contains('=') && !s_keyword.Contains('(') && !s_keyword.Contains(')'))
                        vl_params["@prm_QueryKeyword"].AsString = s_keyword;
                }

            return app.DB.Select("select_REMIT_DataSourceHistory_StorageReports", "REMIT_DataSourceHistory", vl_params);
        }

        public DataSet getREMITDataSourceProcessLog(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_DataSourceHistory"] == null) return null;

            int ID_DataSourceHistory = vl_arguments["ID_DataSourceHistory"].AsInt32;
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_DataSourceHistory;

            return app.DB.Select("select_REMIT_DataSourceHistoryProcessLog", "REMIT_DataSourceHistory", vl_params);
        }

        public DataSet getREMITDataSourceOutputData(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_DataSourceHistory"] == null) return null;

            int ID_DataSourceHistory = vl_arguments["ID_DataSourceHistory"].AsInt32;
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_DataSourceHistory;

            return app.DB.Select("select_REMIT_DataSourceHistoryOutputData", "REMIT_DataSourceHistory", vl_params);
        }

        public DataSet getREMITDataSourceReceiptData(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_DataSourceHistory"] == null) return null;

            int ID_DataSourceHistory = vl_arguments["ID_DataSourceHistory"].AsInt32;
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_DataSourceHistory;

            return app.DB.Select("select_REMIT_DataSourceHistoryReceiptData", "REMIT_DataSourceHistory", vl_params);
        }

        public DataSet getREMITParticipants(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Agency").AsInt32 = -1;
            vl_params.Add("@prm_QueryKeyword").AsString = "";
            if (vl_arguments["ID_Agency"] != null) vl_params["@prm_ID_Agency"].AsInt32 = vl_arguments["ID_Agency"].AsInt32;

            if (vl_arguments["QueryKeyword"] != null)
                if (vl_arguments["QueryKeyword"].ValueType == TVariantType.vtString)
                {
                    string s_keyword = vl_arguments["QueryKeyword"].AsString;
                    if (!s_keyword.Contains('=') && !s_keyword.Contains('(') && !s_keyword.Contains(')'))
                        vl_params["@prm_QueryKeyword"].AsString = s_keyword;
                }


            return app.DB.Select("select_REMIT_Participants", "REMIT_Participants", vl_params);
        }

        public DataSet getREMITOrders(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Agency").AsInt32 = 0;
            vl_params.Add("@prm_QueryKeyword").AsString = "";
            if (vl_arguments["ID_Agency"] != null) vl_params["@prm_ID_Agency"].AsInt32 = vl_arguments["ID_Agency"].AsInt32;

            if (vl_arguments["QueryKeyword"] != null)
                if (vl_arguments["QueryKeyword"].ValueType == TVariantType.vtString)
                {
                    string s_keyword = vl_arguments["QueryKeyword"].AsString;
                    if (!s_keyword.Contains('=') && !s_keyword.Contains('(') && !s_keyword.Contains(')'))
                        vl_params["@prm_QueryKeyword"].AsString = s_keyword;
                }

            return app.DB.Select("select_REMIT_Orders", "REMIT_Orders", vl_params);
        }

        public DataSet getREMITOrderDetails(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = 0;
            if (vl_arguments["ID_Order"] != null) vl_params["@prm_ID"].AsInt32 = vl_arguments["ID_Order"].AsInt32;

            return app.DB.Select("select_REMIT_OrderDetails", "REMIT_Orders", vl_params);
        }

        public DataSet getREMITTable1ReportDetails(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Table1Report"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = -1;
            vl_params["@prm_ID"].AsInt32 = vl_arguments["ID_Table1Report"].AsInt32;

            return app.DB.Select("select_REMIT_Table1ReportDetails", "REMIT_Table1Reports", vl_params);
        }

        public DataSet getREMITTable2ReportDetails(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Table2Report"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = vl_arguments["ID_Table2Report"].AsInt32;

            return app.DB.Select("select_REMIT_Table2ReportDetails", "REMIT_Table2Reports", vl_params);
        }

        public DataSet getREMITTable2VolumeOptionalities(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Table2Report"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Table2Report").AsInt32 = vl_arguments["ID_Table2Report"].AsInt32;

            return app.DB.Select("select_REMIT_Table2VolumeOptionalities", "REMIT_Table2Reports", vl_params);
        }

        public DataSet getREMITTable2FixingIndexDetails(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Table2Report"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Table2Report").AsInt32 = vl_arguments["ID_Table2Report"].AsInt32;

            return app.DB.Select("select_REMIT_Table2FixingIndexDetails", "REMIT_Table2Reports", vl_params);
        }

        public DataSet getREMITExistingParticipantIDs(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();

            return app.DB.Select("select_REMIT_ExistingParticipantIDs", "REMIT_NonStandardContracts", vl_params);
        }

        public DataSet getREMITExistingCounterpartIDs(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();

            return app.DB.Select("select_REMIT_ExistingCounterpartIDs", "REMIT_NonStandardContracts", vl_params);
        }

        public DataSet getREMITNGStorageReportDetails(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_StorageReport"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = vl_arguments["ID_StorageReport"].AsInt32;

            return app.DB.Select("select_REMIT_StorageReportDetails", "REMIT_StorageReports", vl_params);
        }

        public DataSet getREMITStorageFacilityReports(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_StorageReport"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_StorageReport").AsInt32 = vl_arguments["ID_StorageReport"].AsInt32;
            vl_params.Add("@prm_ID").AsInt32 = -1;
            if (vl_arguments["ID_StorageFacilityReport"] != null) vl_params["@prm_ID"].AsInt32 = vl_arguments["ID_StorageFacilityReport"].AsInt32;

            return app.DB.Select("select_REMIT_StorageFacilityReports", "REMIT_StorageFacilityReports", vl_params);
        }

        public DataSet getREMITStorageParticipantActivityReports(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_StorageReport"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_StorageReport").AsInt32 = vl_arguments["ID_StorageReport"].AsInt32;
            vl_params.Add("@prm_ID").AsInt32 = -1;
            if (vl_arguments["ID_StorageParticipantActivityReport"] != null) vl_params["@prm_ID"].AsInt32 = vl_arguments["ID_StorageParticipantActivityReport"].AsInt32;

            return app.DB.Select("select_REMIT_StorageParticipantActivityReports", "REMIT_StorageParticipantActivityReports", vl_params);
        }

        public DataSet getREMITStorageUnavailabilityReports(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_StorageReport"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_StorageReport").AsInt32 = vl_arguments["ID_StorageReport"].AsInt32;
            vl_params.Add("@prm_ID").AsInt32 = -1;
            if (vl_arguments["ID_StorageUnavailabilityReport"] != null) vl_params["@prm_ID"].AsInt32 = vl_arguments["ID_StorageUnavailabilityReport"].AsInt32;

            return app.DB.Select("select_REMIT_StorageUnavailabilityReports", "REMIT_StorageUnavailabilityReports", vl_params);
        }

        public DataSet getREMITARISActivityLog(TVariantList vl_arguments)
        {
            TVariantList vl_params = new TVariantList();
            return app.DB.Select("select_REMIT_ARISActivity", "REMIT_ARIS", vl_params);
        }

    }
}