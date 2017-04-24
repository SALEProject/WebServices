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
    public class OrdersDBModule:AbstractDBModule
    {
        private TBusiness app;
        private Session session;

        public OrdersDBModule(TBusiness app, Session session) : base()
        {
            this.app = app;
            this.session = session;
            if (app == null) return;

            string serverPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
            if (!serverPath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                serverPath += System.IO.Path.DirectorySeparatorChar;
            app.DB.MergeXML(serverPath + "Modules" + System.IO.Path.DirectorySeparatorChar + "OrdersDBModule.xml");

            app.DB.AddDBFunction("addChatMessage", "Messages", new TDBExecFunction(addChatMessage));
            app.DB.AddDBFunction("setNotificationRead", "Alerts", new TDBExecFunctionExtended(setNotificationRead));
            app.DB.AddDBFunction("setMarketParameters", "Markets", new TDBExecFunctionExtended(setMarketParameters));
            app.DB.AddDBFunction("addMarket", "Markets", new TDBExecFunctionExtended(addMarket));
            app.DB.AddDBFunction("addAgency", "Agencies", new TDBExecFunctionExtended(addAgency));
            app.DB.AddDBFunction("editAgency", "Agencies", new TDBExecFunctionExtended(editAgency));
            app.DB.AddDBFunction("deleteAgency", "Agencies", new TDBExecFunctionExtended(deleteAgency));
            app.DB.AddDBFunction("addContact", "Agencies", new TDBExecFunctionExtended(addContact));
            app.DB.AddDBFunction("editContact", "Agencies", new TDBExecFunctionExtended(editContact));
            app.DB.AddDBFunction("deleteContact", "Agencies", new TDBExecFunctionExtended(deleteContact));
            app.DB.AddDBFunction("setAgency2Ring", "Agencies", new TDBExecFunctionExtended(setAgency2Ring));
            app.DB.AddDBFunction("setClient2Agency", "Agencies", new TDBExecFunctionExtended(setClient2Agency));
            app.DB.AddDBFunction("addClient", "Clients", new TDBExecFunctionExtended(addClient));
            app.DB.AddDBFunction("editClient", "Clients", new TDBExecFunction(editClient));
            app.DB.AddDBFunction("deleteClient", "Clients", new TDBExecFunctionExtended(deleteClient));
            app.DB.AddDBFunction("addOrder", "Orders", new TDBExecFunctionExtended(addOrder));
            app.DB.AddDBFunction("validateOrder", "Orders", new TDBExecFunctionExtended(validateOrder));
            app.DB.AddDBFunction("modifyOrder", "Orders", new TDBExecFunctionExtended(modifyOrder));
            app.DB.AddDBFunction("cancelOrder", "Orders", new TDBExecFunctionExtended(cancelOrder));
            app.DB.AddDBFunction("suspendOrder", "Orders", new TDBExecFunctionExtended(suspendOrder));
            app.DB.AddDBFunction("reactivateOrder", "Orders", new TDBExecFunctionExtended(reactivateOrder));
            app.DB.AddDBFunction("acceptOrderMatch", "Orders", new TDBExecFunction(acceptOrderMatch));
            app.DB.AddDBFunction("setWhitelist", "Whitelist", new TDBExecFunctionExtended(setWhitelist));
            app.DB.AddDBFunction("addWhitelistReason", "Whitelist", new TDBExecFunctionExtended(addWhitelistReason));
            app.DB.AddDBFunction("editWhitelistReason", "Whitelist", new TDBExecFunctionExtended(editWhitelistReason));
            app.DB.AddDBFunction("deleteWhitelistReason", "Whitelist", new TDBExecFunctionExtended(deleteWhitelistReason));
            app.DB.AddDBFunction("approveWhitelistRequest", "Whitelist", new TDBExecFunctionExtended(approveWhitelistRequest));
            app.DB.AddDBFunction("addEntryPoint", "EntryPoints", new TDBExecFunctionExtended(addEntryPoint_RGN));
            app.DB.AddDBFunction("editEntryPoint", "EntryPoints", new TDBExecFunctionExtended(editEntryPoint_RGN));
            app.DB.AddDBFunction("deleteEntryPoint", "EntryPoints", new TDBExecFunctionExtended(deleteEntryPoint_RGN));
            app.DB.AddDBFunction("setUserEntryPoint", "EntryPoints", new TDBExecFunctionExtended(setUserEntryPoint));
            //app.DB.AddDBFunction("deleteUser", "Users", new TDBExecFunctionExtended(deleteUser));

            app.DB.AddDBFunction("addAssetType", "AssetTypes", new TDBExecFunctionExtended(addAssetType));
            app.DB.AddDBFunction("editAssetType", "AssetTypes", new TDBExecFunctionExtended(editAssetType));
            app.DB.AddDBFunction("deleteAssetType", "AssetTypes", new TDBExecFunctionExtended(deleteAssetType));
            app.DB.AddDBFunction("addAsset", "Assets", new TDBExecFunctionExtended(addAsset));
            app.DB.AddDBFunction("editAsset", "Assets", new TDBExecFunctionExtended(editAsset));
            app.DB.AddDBFunction("deleteAsset", "Assets", new TDBExecFunctionExtended(deleteAsset));
            app.DB.AddDBFunction("duplicateAsset", "Assets", new TDBExecFunctionExtended(duplicateAsset));
            app.DB.AddDBFunction("setAssetTradeParameters", "Assets", new TDBExecFunctionExtended(setAssetTradeParameters));
            app.DB.AddDBFunction("addRing", "Rings", new TDBExecFunctionExtended(addRing));
            app.DB.AddDBFunction("editRing", "Rings", new TDBExecFunctionExtended(editRing));
            app.DB.AddDBFunction("deleteRing", "Rings", new TDBExecFunctionExtended(deleteRing));
            app.DB.AddDBFunction("setClient2Ring", "Rings", new TDBExecFunctionExtended(setClient2Ring));
            app.DB.AddDBFunction("setClient2Asset", "Rings", new TDBExecFunctionExtended(setClient2Asset));
            app.DB.AddDBFunction("setRingAdministrator", "Rings", new TDBExecFunctionExtended(setRingAdministrator));
            app.DB.AddDBFunction("resetRingAdministrator", "Rings", new TDBExecFunctionExtended(resetRingAdministrator));
            app.DB.AddDBFunction("approveInitialOrder", "Orders", new TDBExecFunctionExtended(approveInitialOrder));
            app.DB.AddDBFunction("rejectInitialOrder", "Orders", new TDBExecFunctionExtended(rejectInitialOrder));
            app.DB.AddDBFunction("addAssetSchedule", "Assets", new TDBExecFunctionExtended(addAssetSchedule));
            app.DB.AddDBFunction("editAssetSchedule", "Assets", new TDBExecFunctionExtended(editAssetSchedule));
            app.DB.AddDBFunction("deleteAssetSchedule", "Assets", new TDBExecFunctionExtended(deleteAssetSchedule));
            app.DB.AddDBFunction("addGasType2EntryPoint", "Assets", new TDBExecFunctionExtended(addAssetSchedule));

            app.DB.AddDBFunction("addCurrency", "Nomenclators", new TDBExecFunctionExtended(addCurrency));
            app.DB.AddDBFunction("editCurrency", "Nomenclators", new TDBExecFunctionExtended(editCurrency));
            app.DB.AddDBFunction("deleteCurrency", "Nomenclators", new TDBExecFunctionExtended(deleteCurrency));
            app.DB.AddDBFunction("addMeasuringUnit", "Nomenclators", new TDBExecFunctionExtended(addMeasuringUnit));
            app.DB.AddDBFunction("editMeasuringUnit", "Nomenclators", new TDBExecFunctionExtended(editMeasuringUnit));
            app.DB.AddDBFunction("deleteMeasuringUnit", "Nomenclators", new TDBExecFunctionExtended(deleteMeasuringUnit));
            app.DB.AddDBFunction("addCPV", "Nomenclators", new TDBExecFunctionExtended(addCPV));
            app.DB.AddDBFunction("editCPV", "Nomenclators", new TDBExecFunctionExtended(editCPV));
            app.DB.AddDBFunction("deleteCPV", "Nomenclators", new TDBExecFunctionExtended(deleteCPV));
            app.DB.AddDBFunction("addCAEN", "Nomenclators", new TDBExecFunctionExtended(addCAEN));
            app.DB.AddDBFunction("editCAEN", "Nomenclators", new TDBExecFunctionExtended(editCAEN));
            app.DB.AddDBFunction("deleteCAEN", "Nomenclators", new TDBExecFunctionExtended(deleteCAEN));
            app.DB.AddDBFunction("addTerminal", "Nomenclators", new TDBExecFunctionExtended(addTerminal));
            app.DB.AddDBFunction("editTerminal", "Nomenclators", new TDBExecFunctionExtended(editTerminal));
            app.DB.AddDBFunction("deleteTerminal", "Nomenclators", new TDBExecFunctionExtended(deleteTerminal));
            app.DB.AddDBFunction("addTranslation", "Nomenclators", new TDBExecFunctionExtended(addTranslation));
            app.DB.AddDBFunction("editTranslation", "Nomenclators", new TDBExecFunctionExtended(editTranslation));
            app.DB.AddDBFunction("deleteTranslation", "Nomenclators", new TDBExecFunctionExtended(deleteTranslation));

            app.DB.AddDBFunction("addDocumentType", "Documents", new TDBExecFunctionExtended(addDocumentType));
            app.DB.AddDBFunction("editDocumentType", "Documents", new TDBExecFunctionExtended(editDocumentType));
            app.DB.AddDBFunction("deleteDocumentType", "Documents", new TDBExecFunctionExtended(deleteDocumentType));
            app.DB.AddDBFunction("addDocument", "Documents", new TDBExecFunctionExtended(addDocument));
            app.DB.AddDBFunction("editDocument", "Documents", new TDBExecFunctionExtended(editDocument));
            app.DB.AddDBFunction("deleteDocument", "Documents", new TDBExecFunctionExtended(deleteDocument));

            app.DB.AddDBFunction("addWarrantyType", "Warranties", new TDBExecFunctionExtended(addWarrantyType));
            app.DB.AddDBFunction("editWarrantyType", "Warranties", new TDBExecFunctionExtended(editWarrantyType));
            app.DB.AddDBFunction("deleteWarrantyType", "Warranties", new TDBExecFunctionExtended(deleteWarrantyType));
            app.DB.AddDBFunction("setWarrantyType2Ring", "Rings", new TDBExecFunctionExtended(setWarrantyType2Ring));
            //app.DB.AddDBFunction("setWarrantyType2Asset", "Rings", new TDBExecFunctionExtended(setWarrantyType2Asset));
            app.DB.AddDBFunction("setWarrantyType2Asset", "Assets", new TDBExecFunctionExtended(setWarrantyType2Asset));
            app.DB.AddDBFunction("setWarrantyTypePriority", "Rings", new TDBExecFunctionExtended(setWarrantyTypePriority));
            app.DB.AddDBFunction("setWarrantyTypePriority", "Assets", new TDBExecFunctionExtended(setWarrantyTypePriority));
            app.DB.AddDBFunction("addWarranty", "Warranties", new TDBExecFunctionExtended(addWarranty));
            app.DB.AddDBFunction("editWarranty", "Warranties", new TDBExecFunctionExtended(editWarranty));
            app.DB.AddDBFunction("deleteWarranty", "Warranties", new TDBExecFunctionExtended(deleteWarranty));
            app.DB.AddDBFunction("unblockWarranty", "Warranties", new TDBExecFunctionExtended(unblockWarranty));

            app.DB.AddDBFunction("addCounty", "Nomenclators", new TDBExecFunctionExtended(addCounty));
            app.DB.AddDBFunction("editCounty", "Nomenclators", new TDBExecFunctionExtended(editCounty));
            app.DB.AddDBFunction("deleteCounty", "Nomenclators", new TDBExecFunctionExtended(deleteCounty));

            app.DB.AddDBFunction("changePassword", "Users", new TDBExecFunctionExtended(changePassword));

            //  manual transactions
            app.DB.AddDBFunction("addTransaction", "Transactions", new TDBExecFunctionExtended(addTransaction));
            //app.DB.AddDBFunction("editTransaction", "Transactions", new TDBExecFunction(editTransaction));
            //app.DB.AddDBFunction("deleteTransaction", "Transactions", new TDBExecFunctionExtended(deleteTransaction));
            app.DB.AddDBFunction("editOperationsBuffer", "Warranties", new TDBExecFunctionExtended(editOperationsBuffer));
            app.DB.AddDBFunction("confirmTransaction", "Transactions", new TDBExecFunctionExtended(confirmTransaction));


            app.DB.AddDBFunction("addProcedure", "Procedures", new TDBExecFunctionExtended(addProcedure));
            app.DB.AddDBFunction("editProcedure", "Procedures", new TDBExecFunctionExtended(editProcedure));
            app.DB.AddDBFunction("deleteProcedure", "Procedures", new TDBExecFunctionExtended(deleteProcedure));
            app.DB.AddDBFunction("addProcedureDocument", "Procedures", new TDBExecFunctionExtended(addProcedureDocument));
            app.DB.AddDBFunction("editProcedureDocument", "Procedures", new TDBExecFunctionExtended(editProcedureDocument));
            app.DB.AddDBFunction("deleteProcedureDocument", "Procedures", new TDBExecFunctionExtended(deleteProcedureDocument));
            app.DB.AddDBFunction("setFavouriteProcedure", "Procedures", new TDBExecFunctionExtended(setFavouriteProcedure));
            app.DB.AddDBFunction("resetFavouriteProcedure", "Procedures", new TDBExecFunctionExtended(resetFavouriteProcedure));
            app.DB.AddDBFunction("setProcedureStatus", "Procedures", new TDBExecFunctionExtended(setProcedureStatus));

            app.DB.AddDBFunction("addForm", "Forms", new TDBExecFunctionExtended(addForm));
            app.DB.AddDBFunction("editForm", "Forms", new TDBExecFunctionExtended(editForm));
            app.DB.AddDBFunction("deleteForm", "Forms", new TDBExecFunctionExtended(deleteForm));
            app.DB.AddDBFunction("setFormData", "Forms", new TDBExecFunctionExtended(setFormData));

            //20150814
            app.DB.AddDBFunction("addProcedureType", "Nomenclators", new TDBExecFunctionExtended(addProcedureType));
            app.DB.AddDBFunction("editProcedureType", "Nomenclators", new TDBExecFunctionExtended(editProcedureType));
            app.DB.AddDBFunction("deleteProcedureType", "Nomenclators", new TDBExecFunctionExtended(deleteProcedureType));
            app.DB.AddDBFunction("addContractType", "Nomenclators", new TDBExecFunctionExtended(addContractType));
            app.DB.AddDBFunction("editContractType", "Nomenclators", new TDBExecFunctionExtended(editContractType));
            app.DB.AddDBFunction("deleteContractType", "Nomenclators", new TDBExecFunctionExtended(deleteContractType));
            app.DB.AddDBFunction("addProcedureLegislation", "Nomenclators", new TDBExecFunctionExtended(addProcedureLegislation));
            app.DB.AddDBFunction("editProcedureLegislation", "Nomenclators", new TDBExecFunctionExtended(editProcedureLegislation));
            app.DB.AddDBFunction("deleteProcedureLegislation", "Nomenclators", new TDBExecFunctionExtended(deleteProcedureLegislation));
            app.DB.AddDBFunction("addProcedureCriteria", "Nomenclators", new TDBExecFunctionExtended(addProcedureCriteria));
            app.DB.AddDBFunction("editProcedureCriteria", "Nomenclators", new TDBExecFunctionExtended(editProcedureCriteria));
            app.DB.AddDBFunction("deleteProcedureCriteria", "Nomenclators", new TDBExecFunctionExtended(deleteProcedureCriteria));

            //  REMIT
            app.DB.AddDBFunction("addDataSource", "REMIT", new TDBExecFunction(addREMITDataSource));
            app.DB.AddDBFunction("editDataSource", "REMIT", new TDBExecFunction(editREMITDataSource));
            app.DB.AddDBFunction("deleteDataSource", "REMIT", new TDBExecFunction(deleteREMITDataSource));

            app.DB.AddDBFunction("addContractName", "REMIT", new TDBExecFunction(addREMITContractName));
            app.DB.AddDBFunction("editContractName", "REMIT", new TDBExecFunction(editREMITContractName));
            app.DB.AddDBFunction("deleteContractName", "REMIT", new TDBExecFunction(deleteREMITContractName));

            app.DB.AddDBFunction("addContractType", "REMIT", new TDBExecFunction(addREMITContractType));
            app.DB.AddDBFunction("editContractType", "REMIT", new TDBExecFunction(editREMITContractType));
            app.DB.AddDBFunction("deleteContractType", "REMIT", new TDBExecFunction(deleteREMITContractType));

            app.DB.AddDBFunction("addLoadType", "REMIT", new TDBExecFunction(addREMITLoadType));
            app.DB.AddDBFunction("editLoadType", "REMIT", new TDBExecFunction(editREMITLoadType));
            app.DB.AddDBFunction("deleteLoadType", "REMIT", new TDBExecFunction(deleteREMITLoadType));

            app.DB.AddDBFunction("uploadFile", "REMIT", new TDBExecFunction(uploadREMITFile));
            app.DB.AddDBFunction("uploadReceipt", "REMIT", new TDBExecFunction(uploadREMITReceipt));

            app.DB.AddDBFunction("addTable1Report", "REMIT", new TDBExecFunctionExtended(addREMITTable1Report));
            app.DB.AddDBFunction("editTable1Report", "REMIT", new TDBExecFunctionExtended(editREMITTable1Report));
            app.DB.AddDBFunction("submitTable1Report", "REMIT", new TDBExecFunctionExtended(submitREMITTable1Report));
            //app.DB.AddDBFunction("modifyNonStandardContract", "REMIT", new TDBExecFunction(modifyREMITNonStandardContract));
            //app.DB.AddDBFunction("errorNonStandardContract", "REMIT", new TDBExecFunction(errorREMITNonStandardContract));
            //app.DB.AddDBFunction("cancelNonStandardContract", "REMIT", new TDBExecFunction(cancelREMITNonStandardContract));

            app.DB.AddDBFunction("addTable2Report", "REMIT", new TDBExecFunctionExtended(addREMITTable2Report));
            app.DB.AddDBFunction("editTable2Report", "REMIT", new TDBExecFunctionExtended(editREMITTable2Report));
            app.DB.AddDBFunction("submitTable2Report", "REMIT", new TDBExecFunctionExtended(submitREMITTable2Report));

            app.DB.AddDBFunction("createStorageReport", "REMIT", new TDBExecFunctionExtended(createREMITStorageReport));
            app.DB.AddDBFunction("submitStorageReport", "REMIT", new TDBExecFunctionExtended(submitREMITStorageReport));
            app.DB.AddDBFunction("checkEmptyStorageReport", "REMIT", new TDBExecFunctionExtended(checkREMITEmptyStorageReport));

            app.DB.AddDBFunction("addStorageFacilityReport", "REMIT", new TDBExecFunctionExtended(addREMITStorageFacilityReport));
            app.DB.AddDBFunction("editStorageFacilityReport", "REMIT", new TDBExecFunctionExtended(editREMITStorageFacilityReport));

            app.DB.AddDBFunction("addStorageParticipantActivityReport", "REMIT", new TDBExecFunctionExtended(addREMITStorageParticipantActivityReport));
            app.DB.AddDBFunction("editStorageParticipantActivityReport", "REMIT", new TDBExecFunctionExtended(editREMITStorageParticipantActivityReport));

            app.DB.AddDBFunction("addStorageUnavailabilityReport", "REMIT", new TDBExecFunctionExtended(addREMITStorageUnavailabilityReport));
            app.DB.AddDBFunction("editStorageUnavailabilityReport", "REMIT", new TDBExecFunctionExtended(editREMITStorageUnavailabilityReport));
        }

        public TDBExecResult ReturnDBError(TVariantList vl_arguments)
        {
            return new TDBExecResult(TDBExecError.SQLExecutionError, "");
        }

        public int addChatMessage(TVariantList vl_arguments)
        {
            if (vl_arguments["Message"] == null) return -1;

            //  validate if there are no obsece words

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_User").AsInt32 = session.ID_User;
            vl_params.Add("@prm_Message").AsString = vl_arguments["Message"].AsString;
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;

            int res = app.DB.Exec("insert_Message", "Messages", vl_params);

            if (res > 0)
                return app.DB.GetIdentity();
            else return res;
        }

        public TDBExecResult setNotificationRead(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Notification"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Notification");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Notification = 0;
            if (vl_arguments["ID_Notification"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Notification"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Notification"].ValueType == TVariantType.vtInt64) ID_Notification = vl_arguments["ID_Notification"].AsInt32;
            else res.ParamValidations["ID_Notification"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Notification;
            int int_count_Notifications = app.DB.Exec("update_isRead", "Notifications", vl_params);
            if (int_count_Notifications <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult setMarketParameters(TVariantList vl_arguments)
        {
            if (vl_arguments["PreOpeningTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PreOpeningTime");
            if (vl_arguments["OpeningTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OpeningTime");
            if (vl_arguments["PreClosingTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PreClosingTime");
            if (vl_arguments["ClosingTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ClosingTime");
            if (vl_arguments["MinQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MinQuantity");
            if (vl_arguments["MaxPriceVariation"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MaxPriceVariation");
            if (vl_arguments["QuantityStepping"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "QuantityStepping");
            if (vl_arguments["PriceStepping"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PriceStepping");
            if (vl_arguments["StartDeliveryDateOffset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StartDeliveryDateOffset");
            if (vl_arguments["EndDeliveryDateOffset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "EndDeliveryDateOffset");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Ring = 1;

            DateTime dt_PreOpeningTime = DateTime.Now;
            try { dt_PreOpeningTime = vl_arguments["PreOpeningTime"].AsDateTime; }
            catch { res.ParamValidations["PreOpeningTime"].AsString = "Value not recognized"; }

            DateTime dt_OpeningTime = DateTime.Now;
            try { dt_OpeningTime = vl_arguments["OpeningTime"].AsDateTime; }
            catch { res.ParamValidations["OpeningTime"].AsString = "Value not recognized"; }

            DateTime dt_PreClosingTime = DateTime.Now;
            try { dt_PreClosingTime = vl_arguments["PreClosingTime"].AsDateTime; }
            catch { res.ParamValidations["PreClosingTime"].AsString = "Value not recognized"; }

            DateTime dt_ClosingTime = DateTime.Now;
            try { dt_ClosingTime = vl_arguments["ClosingTime"].AsDateTime; }
            catch { res.ParamValidations["ClosingTime"].AsString = "Value not recognized"; }

            double flt_MinQuantity = 0;
            try { flt_MinQuantity = vl_arguments["MinQuantity"].AsDouble; }
            catch { res.ParamValidations["MinQuantity"].AsString = "Value not recognized"; }

            double flt_MaxPriceVariation = 0;
            try { flt_MaxPriceVariation = vl_arguments["MaxPriceVariation"].AsDouble; }
            catch { res.ParamValidations["MaxPriceVariation"].AsString = "Value not recognized"; }

            double flt_QuantityStepping = 0;
            try { flt_QuantityStepping = vl_arguments["QuantityStepping"].AsDouble; }
            catch { res.ParamValidations["QuantityStepping"].AsString = "Value not recognized"; }

            double flt_PriceStepping = 0;
            try { flt_PriceStepping = vl_arguments["PriceStepping"].AsDouble; }
            catch { res.ParamValidations["PriceStepping"].AsString = "Value not recognized"; }

            int int_StartDeliveryDateOffset = 0;
            try { int_StartDeliveryDateOffset = vl_arguments["StartDeliveryDateOffset"].AsInt32; }
            catch { res.ParamValidations["StartDeliveryDateStepping"].AsString = "Value not recognized"; }

            int int_EndDeliveryDateOffset = 0;
            try { int_EndDeliveryDateOffset = vl_arguments["EndDeliveryDateOffset"].AsInt32; }
            catch { res.ParamValidations["EndDeliveryDateOffset"].AsString = "Value not recognized"; }

            bool hasSchedule = false;
            byte DaysOfWeek = 0;
            if (vl_arguments["DaysOfWeek"] != null)
            {
                if (vl_arguments["DaysOfWeek"].ValueType == TVariantType.vtObject && vl_arguments["DaysOfWeek"].AsObject is TVariantList)
                {
                    hasSchedule = true;
                    DaysOfWeek = 0;
                    TVariantList vl_DaysOfWeek = vl_arguments["DaysOfWeek"].AsObject as TVariantList;
                    for (int i = 0; i < vl_DaysOfWeek.Count; i++)
                    {
                        switch (vl_DaysOfWeek[i].Name.Trim().ToUpper())
                        {
                            case "DAYSUNDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 1;
                                break;
                            case "DAYMONDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 2;
                                break;
                            case "DAYTUESDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 4;
                                break;
                            case "DAYWEDNESDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 8;
                                break;
                            case "DAYTHURSDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 16;
                                break;
                            case "DAYFRIDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 32;
                                break;
                            case "DAYSATURDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 64;
                                break;
                        }
                    }
                }
                else res.ParamValidations["DaysOfWeek"].AsString = "Value not recognized";
            }


            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_Ring;
                vl_params.Add("@prm_PreOpeningTime").AsDateTime = dt_PreOpeningTime;
                vl_params.Add("@prm_OpeningTime").AsDateTime = dt_OpeningTime;
                vl_params.Add("@prm_PreClosingTime").AsDateTime = dt_PreClosingTime;
                vl_params.Add("@prm_ClosingTime").AsDateTime = dt_ClosingTime;
                vl_params.Add("@prm_MinQuantity").AsDouble = flt_MinQuantity;
                vl_params.Add("@prm_MaxPriceVariation").AsDouble = flt_MaxPriceVariation;
                vl_params.Add("@prm_QuantityStepping").AsDouble = flt_QuantityStepping;
                vl_params.Add("@prm_PriceStepping").AsDouble = flt_PriceStepping;
                vl_params.Add("@prm_hasSchedule").AsBoolean = hasSchedule;
                vl_params.Add("@prm_DaysOfWeek").AsInt16 = DaysOfWeek;

                int int_count_Rings = app.DB.Exec("update_RingParameters", "Rings", vl_params);
                if (int_count_Rings == -1) throw new Exception("Ring update failed");
                if (int_count_Rings == 0) throw new Exception("Ring does not exist");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("@prm_StartDeliveryDateOffset").AsInt32 = int_StartDeliveryDateOffset;
                vl_params.Add("@prm_EndDeliveryDateOffset").AsInt32 = int_EndDeliveryDateOffset;
                int int_count_RingParameters = app.DB.Exec("update_RingParameters_RGN", "RingParameters_RGN", vl_params);
                if (int_count_RingParameters == -1) throw new Exception("Ring Parameters update failed");
                if (int_count_RingParameters == 0)
                {
                    int_count_RingParameters = app.DB.Exec("insert_RingParamters_RGN", "RingParameters_RGN", vl_params);
                    if (int_count_RingParameters == -1) throw new Exception("Ring Parameters update failed");
                    if (int_count_RingParameters == 0) throw new Exception("Query error");
                }

                app.DB.CommitTransaction();

                res.RowsModified = int_count_Rings + int_count_RingParameters;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult addMarket(TVariantList vl_arguments)
        {
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["isDefault"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isDefault");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);
            
            string Name = "";
            if (vl_arguments["Name"].ValueType == TVariantType.vtString) Name = vl_arguments["Name"].AsString;
            else res.ParamValidations["Name"].AsString = "Value type not recognized";

            bool isDefault = false;
            if (vl_arguments["isDefault"].ValueType == TVariantType.vtBoolean) isDefault = vl_arguments["isDefault"].AsBoolean;
            else res.ParamValidations["isDefault"].AsString = "Value type not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                vl_params.Add("@prm_Name").AsString = Name;
                vl_params.Add("@prm_isDefault").AsBoolean = isDefault;

                int int_count_Markets = app.DB.Exec("insert_Market", "Markets", vl_params);
                if (int_count_Markets == -1) throw new Exception("Market insert failed");

                app.DB.CommitTransaction();

                res.RowsModified = int_count_Markets;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }


        public TDBExecResult addAgency(TVariantList vl_arguments)
        {
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["FiscalCode"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "FiscalCode");
            if (vl_arguments["RegisterCode"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "RegisterCode");
            //if (vl_arguments["CompanyName"] == null) return -1;
            /*if (vl_arguments["Bank"] == null) return -1;
            if (vl_arguments["BankAccount"] == null) return -1;
            if (vl_arguments["StreetAddress"] == null) return -1;
            if (vl_arguments["City"] == null) return -1;            
            if (vl_arguments["PostalCode"] == null) return -1;
            if (vl_arguments["ID_County"] == null) return -1;                        */

            string Bank = (vl_arguments["Bank"] == null) ? "" : vl_arguments["Bank"].AsString;
            string BankAccount = (vl_arguments["BankAccount"] == null) ? "" : vl_arguments["BankAccount"].AsString;



            //  validation
            string Code = vl_arguments["Code"].AsString;
            string Name = vl_arguments["Name"].AsString;
            string FiscalCode = vl_arguments["FiscalCode"].AsString;
            string RegisterCode = vl_arguments["RegisterCode"].AsString;
            string CompanyName = vl_arguments["Name"].AsString;
            /*string Bank = vl_arguments["Bank"].AsString;
            string BankAccount = vl_arguments["BankAccount"].AsString;
            string StreetAddress = vl_arguments["StreetAddress"].AsString;
            string City = vl_arguments["City"].AsString;           
            string PostalCode = vl_arguments["PostalCode"].AsString;
            int ID_County = 0;
            if (vl_arguments["ID_County"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt64) ID_County = vl_arguments["ID_County"].AsInt32;
            else return -1;
            */

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            DateTime ContractStartDate = DateTime.Today;
            if (vl_arguments["ContractStart"] != null)
            {
                if (vl_arguments["ContractStart"].ValueType == TVariantType.vtDateTime) ContractStartDate = vl_arguments["ContractStart"].AsDateTime;
                else res.ParamValidations["ContractStart"].AsString = "Value type not recognized";
            }

            DateTime ContractEndDate = DateTime.Today;
            if (vl_arguments["ContractEnd"] != null)
            {
                if (vl_arguments["ContractEnd"].ValueType == TVariantType.vtDateTime) ContractEndDate = vl_arguments["ContractEnd"].AsDateTime;
                else res.ParamValidations["ContractEnd"].AsString = "Value type not recognized";
            }

            string ContractNumber = "";
            if (vl_arguments["ContractNumber"] != null)
            {
                if (vl_arguments["ContractNumber"].ValueType == TVariantType.vtString) ContractNumber = vl_arguments["ContractNumber"].AsString;
                else res.ParamValidations["ContractNumber"].AsString = "Value type not recognized";
            }

            string Status = "active";
            if (vl_arguments["Status"] != null)
            {
                if (vl_arguments["Status"].ValueType == TVariantType.vtString) Status = vl_arguments["Status"].AsString;
                else res.ParamValidations["Status"].AsString = "Value type not recognized";
            }

            string StreetAddress = (vl_arguments["StreetAddress"] == null) ? "" : vl_arguments["StreetAddress"].AsString;
            string City = (vl_arguments["City"] == null) ? "" : vl_arguments["City"].AsString;
            string PostalCode = (vl_arguments["PostalCode"] == null) ? "" : vl_arguments["PostalCode"].AsString;
            int ID_County = ((vl_arguments["ID_County"] != null) && (vl_arguments["ID_County"].ValueType == TVariantType.vtByte ||
                                                                    vl_arguments["ID_County"].ValueType == TVariantType.vtInt16 ||
                                                                    vl_arguments["ID_County"].ValueType == TVariantType.vtInt32 ||
                                                                    vl_arguments["ID_County"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_County"].AsInt32 : 0;

            string Phone = (vl_arguments["Phone"] == null) ? "" : vl_arguments["Phone"].AsString;
            string Mobile = (vl_arguments["Mobile"] == null) ? "" : vl_arguments["Mobile"].AsString;
            string Fax = (vl_arguments["Fax"] == null) ? "" : vl_arguments["Fax"].AsString;
            string Email = (vl_arguments["Email"] == null) ? "" : vl_arguments["Email"].AsString;
            string Website = (vl_arguments["Website"] == null) ? "" : vl_arguments["Website"].AsString;

            //  Validate unicity of Code
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Code").AsString = Code;
            vl_params.Add("@prm_ID").AsInt32 = 0;
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            DataSet ds_AgencyCode = app.DB.Select("select_AgenciesbyCode", "Agencies", vl_params);
            if (app.DB.ValidDSRows(ds_AgencyCode)) res.ParamValidations["Code"].AsString = "Codul de Agentie este deja utilizat";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");
            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs = new TVariantList();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                vl_params.Add("@prm_Code").AsString = Code;
                vl_params.Add("@prm_Name").AsString = Name;
                vl_params.Add("@prm_ContractStartDate").AsDateTime = ContractStartDate;
                vl_params.Add("@prm_ContractEndDate").AsDateTime = ContractEndDate;
                vl_params.Add("@prm_ContractNumber").AsString = ContractNumber;
                vl_params.Add("@prm_Status").AsString = Status;

                int int_count_Agencies = app.DB.Exec("insert_Agency", "Agencies", vl_params);
                if (int_count_Agencies <= 0) throw new Exception("SQL execution error");
                int ID_Agency = app.DB.GetIdentity();

                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_isHouse").AsBoolean = true;

                int int_count_Clients = app.DB.Exec("insert_Client", "Clients", vl_params);
                if (int_count_Clients <= 0) throw new Exception("SQL execution error");
                int ID_Client = app.DB.GetIdentity();


                int ID_Address = 0;
                int int_count_Address = 0;
                if (StreetAddress != "")
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_StreetAddress").AsString = StreetAddress;
                    vl_params.Add("@prm_City").AsString = City;
                    vl_params.Add("@prm_ID_County").AsInt32 = ID_County;
                    vl_params.Add("@prm_PostalCode").AsString = PostalCode;
                    int_count_Address = app.DB.Exec("insert_Address", "Addresses", vl_params);
                    if (int_count_Address <= 0) throw new Exception("SQL execution error");
                    ID_Address = app.DB.GetIdentity();
                }

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_ID_Broker").AsInt32 = 0;
                vl_params.Add("@prm_IsCompany").AsBoolean = true;
                vl_params.Add("@prm_FiscalCode").AsString = FiscalCode;
                vl_params.Add("@prm_RegisterCode").AsString = RegisterCode;
                vl_params.Add("@prm_CompanyName").AsString = CompanyName;
                vl_params.Add("@prm_SocialCode").AsString = "";
                vl_params.Add("@prm_IdentityCard").AsString = "";
                vl_params.Add("@prm_Title").AsString = "";
                vl_params.Add("@prm_FirstName").AsString = "";
                vl_params.Add("@prm_LastName").AsString = "";
                vl_params.Add("@prm_ID_Address").AsInt32 = ID_Address;
                vl_params.Add("@prm_Bank").AsString = Bank;
                vl_params.Add("@prm_BankAccount").AsString = BankAccount;
                vl_params.Add("@prm_Phone").AsString = Phone;
                vl_params.Add("@prm_Mobile").AsString = Mobile;
                vl_params.Add("@prm_Fax").AsString = Fax;
                vl_params.Add("@prm_Email").AsString = Email;
                vl_params.Add("@prm_Website").AsString = Website;

                int int_count_Identities = app.DB.Exec("insert_Identity", "Identities", vl_params);
                if (int_count_Identities <= 0) throw new Exception("SQL execution error");

                app.DB.CommitTransaction();

                res.RowsModified = int_count_Agencies + int_count_Clients + int_count_Identities + int_count_Address;
                res.IDs.Add("ID_Agency").AsInt32 = ID_Agency;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult editAgency(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Agency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Agency");

            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["FiscalCode"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "FiscalCode");
            if (vl_arguments["RegisterCode"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "RegisterCode");
            //if (vl_arguments["CompanyName"] == null) return -1;
            /*if (vl_arguments["Bank"] == null) return -1;
            if (vl_arguments["BankAccount"] == null) return -1;
            if (vl_arguments["StreetAddress"] == null) return -1;
            if (vl_arguments["City"] == null) return -1;            
            if (vl_arguments["PostalCode"] == null) return -1;
            if (vl_arguments["ID_County"] == null) return -1;                        */

            string Bank = (vl_arguments["Bank"] == null) ? "" : vl_arguments["Bank"].AsString;
            string BankAccount = (vl_arguments["BankAccount"] == null) ? "" : vl_arguments["BankAccount"].AsString;
            string StreetAddress = (vl_arguments["StreetAddress"] == null) ? "" : vl_arguments["StreetAddress"].AsString;
            string City = (vl_arguments["City"] == null) ? "" : vl_arguments["City"].AsString;
            string PostalCode = (vl_arguments["PostalCode"] == null) ? "" : vl_arguments["PostalCode"].AsString;
            int ID_County = ((vl_arguments["ID_County"] != null) && (vl_arguments["ID_County"].ValueType == TVariantType.vtByte ||
                                                                    vl_arguments["ID_County"].ValueType == TVariantType.vtInt16 ||
                                                                    vl_arguments["ID_County"].ValueType == TVariantType.vtInt32 ||
                                                                    vl_arguments["ID_County"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_County"].AsInt32 : 0;

            //  validation
            int ID_Agency = vl_arguments["ID_Agency"].AsInt32;
            //  validation
            string Code = vl_arguments["Code"].AsString;
            string Name = vl_arguments["Name"].AsString;
            string FiscalCode = vl_arguments["FiscalCode"].AsString;
            string RegisterCode = vl_arguments["RegisterCode"].AsString;
            string CompanyName = vl_arguments["Name"].AsString;
            /*string Bank = vl_arguments["Bank"].AsString;
            string BankAccount = vl_arguments["BankAccount"].AsString;
            string StreetAddress = vl_arguments["StreetAddress"].AsString;
            string City = vl_arguments["City"].AsString;           
            string PostalCode = vl_arguments["PostalCode"].AsString;
            int ID_County = 0;
            if (vl_arguments["ID_County"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt64) ID_County = vl_arguments["ID_County"].AsInt32;
            else return -1;
            */
            /*int ID_Address = 0;
            if ((vl_arguments["ID_Address"] != null) && (vl_arguments["ID_Address"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Address"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Address"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Address"].ValueType == TVariantType.vtInt64)) ID_Address = vl_arguments["ID_Address"].AsInt32;*/
            /*else return -1;*/

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            DateTime ContractStartDate = DateTime.Today;
            if (vl_arguments["ContractStart"] != null)
            {
                if (vl_arguments["ContractStart"].ValueType == TVariantType.vtDateTime) ContractStartDate = vl_arguments["ContractStart"].AsDateTime;
                else res.ParamValidations["ContractStart"].AsString = "Value type not recognized"; ;
            }

            DateTime ContractEndDate = DateTime.Today;
            if (vl_arguments["ContractEnd"] != null)
            {
                if (vl_arguments["ContractEnd"].ValueType == TVariantType.vtDateTime) ContractEndDate = vl_arguments["ContractEnd"].AsDateTime;
                else res.ParamValidations["ContractEnd"].AsString = "Value type not recognized";
            }

            string ContractNumber = "";
            if (vl_arguments["ContractNumber"] != null)
            {
                if (vl_arguments["ContractNumber"].ValueType == TVariantType.vtString) ContractNumber = vl_arguments["ContractNumber"].AsString;
                else res.ParamValidations["ContractNumber"].AsString = "Value type not recognized"; ;
            }

            string Status = "active";
            if (vl_arguments["Status"] != null)
            {
                if (vl_arguments["Status"].ValueType == TVariantType.vtString) Status = vl_arguments["Status"].AsString;
                else res.ParamValidations["Status"].AsString = "Value type not recognized";
            }

            string Phone = (vl_arguments["Phone"] == null) ? "" : vl_arguments["Phone"].AsString;
            string Mobile = (vl_arguments["Mobile"] == null) ? "" : vl_arguments["Mobile"].AsString;
            string Fax = (vl_arguments["Fax"] == null) ? "" : vl_arguments["Fax"].AsString;
            string Email = (vl_arguments["Email"] == null) ? "" : vl_arguments["Email"].AsString;
            string Website = (vl_arguments["Website"] == null) ? "" : vl_arguments["Website"].AsString;

            if (res.bInvalid()) return res;

            //  Validate unicity of Code
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Code").AsString = Code;
            vl_params.Add("@prm_ID").AsInt32 = ID_Agency;
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            DataSet ds_AgencyCode = app.DB.Select("select_AgenciesbyCode", "Agencies", vl_params);
            if (app.DB.ValidDSRows(ds_AgencyCode)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Codul de Agentie este deja utilizat");

            vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
            DataSet ds_client = app.DB.Select("select_ClientsbyID_Agency", "Clients", vl_params);
            if (!app.DB.ValidDSRows(ds_client)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Agentia nu are client setat");

            int ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
            vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
            DataSet ds_identity = app.DB.Select("select_IdentitiesbyID_ClientAndID_Agency", "Identities", vl_params);
            if (!app.DB.ValidDSRows(ds_identity)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Agentia nu are identitatea setata");
            int ID_Identity = Convert.ToInt32(ds_identity.Tables[0].Rows[0]["ID"]);
            int ID_Address = Convert.ToInt32(ds_identity.Tables[0].Rows[0]["ID_Address"]);

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");
            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                vl_params.Add("@prm_Code").AsString = Code;
                vl_params.Add("@prm_Name").AsString = Name;
                vl_params.Add("@prm_ContractStartDate").AsDateTime = ContractStartDate;
                vl_params.Add("@prm_ContractEndDate").AsDateTime = ContractEndDate;
                vl_params.Add("@prm_ContractNumber").AsString = ContractNumber;
                vl_params.Add("@prm_Status").AsString = Status;
                vl_params.Add("@prm_ID").AsInt32 = ID_Agency;

                int int_count_Agencies = app.DB.Exec("update_Agency", "Agencies", vl_params);
                if (int_count_Agencies <= 0) throw new Exception("SQL execution error");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_Code").AsString = Code;
                vl_params.Add("@prm_Name").AsString = Name;
                vl_params.Add("@prm_isHouse").AsBoolean = true;
                vl_params.Add("@prm_ID").AsInt32 = ID_Client;

                int int_count_Clients = app.DB.Exec("update_Client", "Clients", vl_params);
                if (int_count_Clients <= 0) throw new Exception("SQL execution error");

                int int_count_Address = 0;
                if (StreetAddress != "")
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_StreetAddress").AsString = StreetAddress;
                    vl_params.Add("@prm_City").AsString = City;
                    vl_params.Add("@prm_ID_County").AsInt32 = ID_County;
                    vl_params.Add("@prm_PostalCode").AsString = PostalCode;

                    if (ID_Address > 0)
                    {
                        vl_params.Add("@prm_ID_Address").AsInt32 = ID_Address;
                        int_count_Address = app.DB.Exec("update_Address", "Addresses", vl_params);
                        if (int_count_Address < 0) throw new Exception("SQL execution error");
                    }
                    else
                    {
                        int_count_Address = app.DB.Exec("insert_Address", "Addresses", vl_params);
                        if (int_count_Address < 0) throw new Exception("SQL execution error");
                        ID_Address = app.DB.GetIdentity();
                    }
                }

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_ID_Broker").AsInt32 = 0;
                vl_params.Add("@prm_IsCompany").AsBoolean = true;
                vl_params.Add("@prm_FiscalCode").AsString = FiscalCode;
                vl_params.Add("@prm_RegisterCode").AsString = RegisterCode;
                vl_params.Add("@prm_CompanyName").AsString = CompanyName;
                vl_params.Add("@prm_SocialCode").AsString = "";
                vl_params.Add("@prm_IdentityCard").AsString = "";
                vl_params.Add("@prm_Title").AsString = "";
                vl_params.Add("@prm_FirstName").AsString = "";
                vl_params.Add("@prm_LastName").AsString = "";
                vl_params.Add("@prm_Bank").AsString = Bank;
                vl_params.Add("@prm_BankAccount").AsString = BankAccount;
                vl_params.Add("@prm_ID_Address").AsInt32 = ID_Address;
                vl_params.Add("@prm_Phone").AsString = Phone;
                vl_params.Add("@prm_Mobile").AsString = Mobile;
                vl_params.Add("@prm_Fax").AsString = Fax;
                vl_params.Add("@prm_Email").AsString = Email;
                vl_params.Add("@prm_Website").AsString = Website;
                vl_params.Add("@prm_ID").AsInt32 = ID_Identity;

                int int_count_Identities = app.DB.Exec("update_Identity", "Identities", vl_params);
                if (int_count_Identities <= 0) throw new Exception("SQL execution error");

                app.DB.CommitTransaction();
                res.RowsModified = int_count_Agencies + int_count_Clients + int_count_Identities + int_count_Address;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }
        
        /*public int addAgency(TVariantList vl_arguments)
        {
            if (vl_arguments["Code"] == null) return -1;
            if (vl_arguments["Name"] == null) return -1;
            if (vl_arguments["FiscalCode"] == null) return -1;
            if (vl_arguments["RegisterCode"] == null) return -1;
            if (vl_arguments["CompanyName"] == null) return -1;

            //  validation
            string Code = vl_arguments["Code"].AsString;
            string Name = vl_arguments["Name"].AsString;
            string FiscalCode = vl_arguments["FiscalCode"].AsString;
            string RegisterCode = vl_arguments["RegisterCode"].AsString;
            string CompanyName = vl_arguments["CompanyName"].AsString;

            if (!app.DB.BeginTransaction()) return -1;
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_Code").AsString = Code;
                vl_params.Add("@prm_Name").AsString = Name;

                int int_count_Agencies = app.DB.Exec("insert_Agency", "Agencies", vl_params);
                if (int_count_Agencies <= 0) throw new Exception("SQL execution error");
                int ID_Agency = app.DB.GetIdentity();

                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_isHouse").AsBoolean = true;

                int int_count_Clients = app.DB.Exec("insert_Client", "Clients", vl_params);
                if (int_count_Clients <= 0) throw new Exception("SQL execution error");
                int ID_Client = app.DB.GetIdentity();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_ID_Broker").AsInt32 = 0;
                vl_params.Add("@prm_IsCompany").AsBoolean = true;
                vl_params.Add("@prm_FiscalCode").AsString = FiscalCode;
                vl_params.Add("@prm_RegisterCode").AsString = RegisterCode;
                vl_params.Add("@prm_CompanyName").AsString = CompanyName;
                vl_params.Add("@prm_SocialCode").AsString = "";
                vl_params.Add("@prm_IdentityCard").AsString = "";
                vl_params.Add("@prm_Title").AsString = "";
                vl_params.Add("@prm_FirstName").AsString = "";
                vl_params.Add("@prm_LastName").AsString = "";

                int int_count_Identities = app.DB.Exec("insert_Identity", "Identities", vl_params);
                if (int_count_Identities <= 0) throw new Exception("SQL execution error");

                app.DB.CommitTransaction();
                return int_count_Agencies + int_count_Clients + int_count_Identities;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return -1;
            }

            return 0;
        }

        public int editAgency(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Agency"] == null) return -1;

            if (vl_arguments["Code"] == null) return -1;
            if (vl_arguments["Name"] == null) return -1;
            if (vl_arguments["FiscalCode"] == null) return -1;
            if (vl_arguments["RegisterCode"] == null) return -1;
            if (vl_arguments["CompanyName"] == null) return -1;

            //  validation
            int ID_Agency = vl_arguments["ID_Agency"].AsInt32;
            string Code = vl_arguments["Code"].AsString;
            string Name = vl_arguments["Name"].AsString;
            string FiscalCode = vl_arguments["FiscalCode"].AsString;
            string RegisterCode = vl_arguments["RegisterCode"].AsString;
            string CompanyName = vl_arguments["CompanyName"].AsString;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
            DataSet ds_client = app.DB.Select("select_ClientsbyID_Agency", "Clients", vl_params);
            if (ds_client == null) return -1;
            if (ds_client.Tables.Count == 0) return -1;
            if (ds_client.Tables[0].Rows.Count == 0) return -1;

            int ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
            vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
            DataSet ds_identity = app.DB.Select("select_IdentitiesbyID_ClientAndID_Agency", "Identities", vl_params);
            if (ds_identity == null) return -1;
            if (ds_identity.Tables.Count == 0) return -1;
            if (ds_identity.Tables[0].Rows.Count == 0) return -1;
            int ID_Identity = Convert.ToInt32(ds_identity.Tables[0].Rows[0]["ID"]);

            if (!app.DB.BeginTransaction()) return -1;
            try
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_Code").AsString = Code;
                vl_params.Add("@prm_Name").AsString = Name;
                vl_params.Add("@prm_ID").AsInt32 = ID_Agency;

                int int_count_Agencies = app.DB.Exec("update_Agency", "Agencies", vl_params);
                if (int_count_Agencies <= 0) throw new Exception("SQL execution error");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_Code").AsString = Code;
                vl_params.Add("@prm_Name").AsString = Name;
                vl_params.Add("@prm_isHouse").AsBoolean = true;
                vl_params.Add("@prm_ID").AsInt32 = ID_Client;

                int int_count_Clients = app.DB.Exec("update_Client", "Clients", vl_params);
                if (int_count_Clients <= 0) throw new Exception("SQL execution error");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_ID_Broker").AsInt32 = 0;
                vl_params.Add("@prm_IsCompany").AsBoolean = true;
                vl_params.Add("@prm_FiscalCode").AsString = FiscalCode;
                vl_params.Add("@prm_RegisterCode").AsString = RegisterCode;
                vl_params.Add("@prm_CompanyName").AsString = CompanyName;
                vl_params.Add("@prm_SocialCode").AsString = "";
                vl_params.Add("@prm_IdentityCard").AsString = "";
                vl_params.Add("@prm_Title").AsString = "";
                vl_params.Add("@prm_FirstName").AsString = "";
                vl_params.Add("@prm_LastName").AsString = "";
                vl_params.Add("@prm_ID").AsInt32 = ID_Identity;

                int int_count_Identities = app.DB.Exec("update_Identity", "Identities", vl_params);
                if (int_count_Identities <= 0) throw new Exception("SQL execution error");

                app.DB.CommitTransaction();
                return int_count_Agencies + int_count_Clients + int_count_Identities;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return -1;
            }

            return 0;
        }*/

        public TDBExecResult deleteAgency(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Agency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Agency");

            //  validation
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int idAgency = 0;
            try { idAgency = vl_arguments["ID_Agency"].AsInt32; }
            catch { res.ParamValidations["ID_Agency"].AsString = "Value not recognized"; }

            if (res.bInvalid()) return res;


            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Agency").AsInt32 = idAgency;

                int countDeleted = app.DB.Exec("delete_Agency", "Agencies", vl_params);
                if (countDeleted <= 0) throw new Exception("Agency deletion failed");

                res.RowsModified = countDeleted;
                return res;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult addContact(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Agency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Agency");
            if (vl_arguments["SocialCode"] == null && vl_arguments["IdentityCard"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "SocialCode or IdentityCard required");
            if (vl_arguments["FirstName"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "FirstName");
            if (vl_arguments["LastName"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "LastName");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Agency = 0;
            if (vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt64) ID_Agency = vl_arguments["ID_Agency"].AsInt32;
            else res.ParamValidations["ID_Agency"].AsString = "Value not recognized";

            string str_SocialCode = "";
            if (vl_arguments["SocialCode"] != null)
            {
                if (vl_arguments["SocialCode"].ValueType == TVariantType.vtString) str_SocialCode = vl_arguments["SocialCode"].AsString;
                else res.ParamValidations["SocialCode"].AsString = "Value not recognized";

                if (str_SocialCode != "")
                {
                    TVariantList vl = new TVariantList();
                    vl.Add("@prm_SocialCode").AsString = str_SocialCode;
                    vl.Add("@prm_ID_Broker").AsInt32 = 0;
                    DataSet ds = app.DB.Select("select_IdentitiesbySocialCode", "Identities", vl);
                    if (app.DB.ValidDSRows(ds) && res.ParamValidations["SocialCode"].AsString == "") res.ParamValidations["SocialCode"].AsString = "Codul CNP trebuie sa fie unic";
                }
            }

            string str_IdentityCard = "";
            if (vl_arguments["IdentityCard"] != null)
            {
                if (vl_arguments["IdentityCard"].ValueType == TVariantType.vtString) str_IdentityCard = vl_arguments["IdentityCard"].AsString;
                else res.ParamValidations["IdentityCard"].AsString = "Value not recognized";

                if (str_IdentityCard != "")
                {
                    TVariantList vl = new TVariantList();
                    vl.Add("@prm_IdentityCard").AsString = str_IdentityCard;
                    vl.Add("@prm_ID_Broker").AsInt32 = 0;
                    DataSet ds = app.DB.Select("select_IdentitiesbyIdentityCard", "Identities", vl);
                    if (app.DB.ValidDSRows(ds) && res.ParamValidations["IdentityCard"].AsString == "") res.ParamValidations["IdentityCard"].AsString = "Seria si numarul CI trebuie sa fie unice";
                }
            }

            string str_FirstName = "";
            if (vl_arguments["FirstName"].ValueType == TVariantType.vtString) str_FirstName = vl_arguments["FirstName"].AsString;
            else res.ParamValidations["FirstName"].AsString = "Value not recognized";

            string str_LastName = "";
            if (vl_arguments["LastName"].ValueType == TVariantType.vtString) str_LastName = vl_arguments["LastName"].AsString;
            else res.ParamValidations["LastName"].AsString = "Value not recognized";

            string str_Phone = "";
            if (vl_arguments["Phone"].ValueType == TVariantType.vtString) str_Phone = vl_arguments["Phone"].AsString;
            else res.ParamValidations["Phone"].AsString = "Value not recognized";
            
            string str_Mobile = "";
            if (vl_arguments["Mobile"].ValueType == TVariantType.vtString) str_Mobile = vl_arguments["Mobile"].AsString;
            else res.ParamValidations["Mobile"].AsString = "Value not recognized";
            
            string str_Fax = "";
            if (vl_arguments["Fax"].ValueType == TVariantType.vtString) str_Fax = vl_arguments["Fax"].AsString;
            else res.ParamValidations["Fax"].AsString = "Value not recognized";
            
            string str_Email = "";
            if (vl_arguments["Email"].ValueType == TVariantType.vtString) str_Email = vl_arguments["Email"].AsString;
            else res.ParamValidations["Email"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Database transaction failed");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_ID_User").AsInt32 = 0;
                vl_params.Add("@prm_isSupervisor").AsBoolean = false;
                vl_params.Add("@prm_isBroker").AsBoolean = false;

                int num_rows = app.DB.Exec("insert_Broker", "Brokers", vl_params);
                if (num_rows == -1) throw new Exception("Contact creation failed");
                int ID_Broker = app.DB.GetIdentity();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Client").AsInt32 = 0;
                vl_params.Add("@prm_ID_Agency").AsInt32 = 0;
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                vl_params.Add("@prm_IsCompany").AsBoolean = false;
                vl_params.Add("@prm_FiscalCode").AsString = "";
                vl_params.Add("@prm_RegisterCode").AsString = "";
                vl_params.Add("@prm_CompanyName").AsString = "";
                vl_params.Add("@prm_SocialCode").AsString = str_SocialCode;
                vl_params.Add("@prm_IdentityCard").AsString = str_IdentityCard;
                vl_params.Add("@prm_Title").AsString = "";
                vl_params.Add("@prm_FirstName").AsString = str_FirstName;
                vl_params.Add("@prm_LastName").AsString = str_LastName;
                vl_params.Add("@prm_ID_Address").AsString = "";
                vl_params.Add("@prm_Bank").AsString = "";
                vl_params.Add("@prm_BankAccount").AsString = "";
                vl_params.Add("@prm_Phone").AsString = str_Phone;
                vl_params.Add("@prm_Mobile").AsString = str_Mobile;
                vl_params.Add("@prm_Fax").AsString = str_Fax;
                vl_params.Add("@prm_Email").AsString = str_Email;
                vl_params.Add("@prm_Website").AsString = "";

                num_rows = app.DB.Exec("insert_Identity", "Identities", vl_params);
                if (num_rows <= 0) throw new Exception("Identity creation failed");

                app.DB.CommitTransaction();

                return new TDBExecResult(TDBExecError.Success, "");
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Database operation failed");
            }
        }

        public TDBExecResult editContact(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Broker"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Broker");
            if (vl_arguments["ID_Agency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Agency");
            if (vl_arguments["SocialCode"] == null && vl_arguments["IdentityCard"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "SocialCode or IdentityCard required");
            if (vl_arguments["FirstName"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "FirstName");
            if (vl_arguments["LastName"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "LastName");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Broker = 0;
            if (vl_arguments["ID_Broker"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Broker"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Broker"].ValueType == TVariantType.vtInt64) ID_Broker = vl_arguments["ID_Broker"].AsInt32;
            else res.ParamValidations["ID_Broker"].AsString = "Value not recognized";

            int ID_Agency = 0;
            if (vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt64) ID_Agency = vl_arguments["ID_Agency"].AsInt32;
            else res.ParamValidations["ID_Agency"].AsString = "Value not recognized";

            string str_SocialCode = "";
            if (vl_arguments["SocialCode"] != null)
            {
                if (vl_arguments["SocialCode"].ValueType == TVariantType.vtString) str_SocialCode = vl_arguments["SocialCode"].AsString;
                else res.ParamValidations["SocialCode"].AsString = "Value not recognized";

                if (str_SocialCode != "")
                {
                    TVariantList vl = new TVariantList();
                    vl.Add("@prm_SocialCode").AsString = str_SocialCode;
                    vl.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                    DataSet ds = app.DB.Select("select_IdentitiesbySocialCode", "Identities", vl);
                    if (app.DB.ValidDSRows(ds) && res.ParamValidations["SocialCode"].AsString == "") res.ParamValidations["SocialCode"].AsString = "Codul CNP trebuie sa fie unic";
                }
            }

            string str_IdentityCard = "";
            if (vl_arguments["IdentityCard"] != null)
            {
                if (vl_arguments["IdentityCard"].ValueType == TVariantType.vtString) str_IdentityCard = vl_arguments["IdentityCard"].AsString;
                else res.ParamValidations["IdentityCard"].AsString = "Value not recognized";

                if (str_IdentityCard != "")
                {
                    TVariantList vl = new TVariantList();
                    vl.Add("@prm_IdentityCard").AsString = str_IdentityCard;
                    vl.Add("@prm_ID_Broker").AsInt32 = 0;
                    DataSet ds = app.DB.Select("select_IdentitiesbyIdentityCard", "Identities", vl);
                    if (app.DB.ValidDSRows(ds) && res.ParamValidations["IdentityCard"].AsString == "") res.ParamValidations["IdentityCard"].AsString = "Seria si numarul CI trebuie sa fie unice";
                }
            }

            string str_FirstName = "";
            if (vl_arguments["FirstName"].ValueType == TVariantType.vtString) str_FirstName = vl_arguments["FirstName"].AsString;
            else res.ParamValidations["FirstName"].AsString = "Value not recognized";

            string str_LastName = "";
            if (vl_arguments["LastName"].ValueType == TVariantType.vtString) str_LastName = vl_arguments["LastName"].AsString;
            else res.ParamValidations["LastName"].AsString = "Value not recognized";

            string str_Phone = "";
            if (vl_arguments["Phone"].ValueType == TVariantType.vtString) str_Phone = vl_arguments["Phone"].AsString;
            else res.ParamValidations["Phone"].AsString = "Value not recognized";

            string str_Mobile = "";
            if (vl_arguments["Mobile"].ValueType == TVariantType.vtString) str_Mobile = vl_arguments["Mobile"].AsString;
            else res.ParamValidations["Mobile"].AsString = "Value not recognized";

            string str_Fax = "";
            if (vl_arguments["Fax"].ValueType == TVariantType.vtString) str_Fax = vl_arguments["Fax"].AsString;
            else res.ParamValidations["Fax"].AsString = "Value not recognized";

            string str_Email = "";
            if (vl_arguments["Email"].ValueType == TVariantType.vtString) str_Email = vl_arguments["Email"].AsString;
            else res.ParamValidations["Email"].AsString = "Value not recognized";

            bool isBroker = true;
            if (vl_arguments["isBroker"] != null && vl_arguments["isBroker"].ValueType == TVariantType.vtBoolean) isBroker = vl_arguments["isBroker"].AsBoolean;

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Agency").AsInt32 = 0;
            vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
            vl_params.Add("@prm_ID_Client").AsInt32 = 0;
            DataSet ds_identity = app.DB.Select("select_IdentitiesbyIDs", "Identities", vl_params);
            if (ds_identity == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "Database transaction failed");
            if (ds_identity.Tables.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "Database transaction failed");
            if (ds_identity.Tables[0].Rows.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "Database transaction failed");
            int ID_Identity = Convert.ToInt32(ds_identity.Tables[0].Rows[0]["ID"]);
            
            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Database transaction failed");
            try
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Client").AsInt32 = 0;
                vl_params.Add("@prm_ID_Agency").AsInt32 = 0;
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                vl_params.Add("@prm_IsCompany").AsBoolean = false;
                vl_params.Add("@prm_FiscalCode").AsString = "";
                vl_params.Add("@prm_RegisterCode").AsString = "";
                vl_params.Add("@prm_CompanyName").AsString = "";
                vl_params.Add("@prm_SocialCode").AsString = str_SocialCode;
                vl_params.Add("@prm_IdentityCard").AsString = str_IdentityCard;
                vl_params.Add("@prm_Title").AsString = "";
                vl_params.Add("@prm_FirstName").AsString = str_FirstName;
                vl_params.Add("@prm_LastName").AsString = str_LastName;
                vl_params.Add("@prm_Bank").AsString = "";
                vl_params.Add("@prm_BankAccount").AsString = "";
                vl_params.Add("@prm_ID_Address").AsInt32 = 0;
                vl_params.Add("@prm_Phone").AsString = str_Phone;
                vl_params.Add("@prm_Mobile").AsString = str_Mobile;
                vl_params.Add("@prm_Fax").AsString = str_Fax;
                vl_params.Add("@prm_Email").AsString = str_Email;
                vl_params.Add("@prm_Website").AsString = "";
                vl_params.Add("@prm_ID").AsInt32 = ID_Identity;

                int num_rows = app.DB.Exec("update_Identity", "Identities", vl_params);
                if (num_rows <= 0) throw new Exception("Identity creation failed");

                if (!isBroker)
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID").AsInt32 = ID_Broker;
                    app.DB.Exec("detach_User", "Brokers", vl_params);
                }

                app.DB.CommitTransaction();

                return new TDBExecResult(TDBExecError.Success, "");
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Database operation failed");
            }
        }

        public TDBExecResult deleteContact(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Broker"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Broker");

            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = vl_arguments["ID_Broker"].AsInt32;
                int num_rows = app.DB.Exec("delete_Broker", "Brokers", vl_params);
                if (num_rows <= 0) throw new Exception("Contact deletion failed");

                return new TDBExecResult(TDBExecError.Success, "");
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Database operation failed");
            }
        }

        public TDBExecResult setAgency2Ring(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Agency"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["isDeleted"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            int ID_Agency = ((vl_arguments["ID_Agency"] != null) && (vl_arguments["ID_Agency"].ValueType == TVariantType.vtByte ||
                                                                    vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt16 ||
                                                                    vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt32 ||
                                                                    vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_Agency"].AsInt32 : 0;

            int ID_Ring = ((vl_arguments["ID_Ring"] != null) && (vl_arguments["ID_Ring"].ValueType == TVariantType.vtByte ||
                                                        vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                                                        vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                                                        vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_Ring"].AsInt32 : 0;

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            bool isDeleted = false;
            if (vl_arguments["isDeleted"].ValueType == TVariantType.vtBoolean) isDeleted = (vl_arguments["isDeleted"].AsBoolean)?false:true;
            else res.ParamValidations["isDeleted"].AsString = "Value type not recognized";

            if (ID_Agency == 0)
                res.ParamValidations["ID_Agency"].AsString = "Value type not recognized";

            if (ID_Ring == 0)
                res.ParamValidations["ID_Ring"].AsString = "Value type not recognized";            

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
            vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;                                
            vl_params.Add("@prm_isDeleted").AsBoolean = isDeleted;

            int int_count_AgencyXRing = app.DB.Exec("update_AgencyXRing", "Agencies", vl_params);
            if (int_count_AgencyXRing == -1) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (int_count_AgencyXRing == 0)
            {
                int_count_AgencyXRing = app.DB.Exec("insert_AgencyXRing", "Agencies", vl_params);
                if (int_count_AgencyXRing == -1) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

                return new TDBExecResult(TDBExecError.Success, "", int_count_AgencyXRing);
            }

            return new TDBExecResult(TDBExecError.Success, "", int_count_AgencyXRing);
        }

        public TDBExecResult setClient2Agency(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Agency"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["ID_Client"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["isDeleted"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            int ID_Agency = ((vl_arguments["ID_Agency"] != null) && (vl_arguments["ID_Agency"].ValueType == TVariantType.vtByte ||
                                                                    vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt16 ||
                                                                    vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt32 ||
                                                                    vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_Agency"].AsInt32 : 0;

            int ID_Client = ((vl_arguments["ID_Client"] != null) && (vl_arguments["ID_Client"].ValueType == TVariantType.vtByte ||
                                                        vl_arguments["ID_Client"].ValueType == TVariantType.vtInt16 ||
                                                        vl_arguments["ID_Client"].ValueType == TVariantType.vtInt32 ||
                                                        vl_arguments["ID_Client"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_Client"].AsInt32 : 0;

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            bool isDeleted = false;
            if (vl_arguments["isDeleted"].ValueType == TVariantType.vtBoolean) isDeleted = (vl_arguments["isDeleted"].AsBoolean) ? false : true;
            else res.ParamValidations["isDeleted"].AsString = "Value type not recognized";

            if (ID_Agency == 0)
                res.ParamValidations["ID_Agency"].AsString = "Value type not recognized";

            if (ID_Client == 0)
                res.ParamValidations["ID_Client"].AsString = "Value type not recognized";

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
            vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
            vl_params.Add("@prm_isDeleted").AsBoolean = isDeleted;

            int int_count_ClientXAgency = app.DB.Exec("update_ClientXAgency", "Agencies", vl_params);
            if (int_count_ClientXAgency == -1) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (int_count_ClientXAgency == 0)
            {
                int_count_ClientXAgency = app.DB.Exec("insert_ClientXAgency", "Agencies", vl_params);
                if (int_count_ClientXAgency == -1) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

                return new TDBExecResult(TDBExecError.Success, "", int_count_ClientXAgency);
            }

            return new TDBExecResult(TDBExecError.Success, "", int_count_ClientXAgency);
        }


        public TDBExecResult addClient(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Agency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Agency");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["FiscalCode"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "FiscalCode");
            if (vl_arguments["RegisterCode"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "RegisterCode");
            //if (vl_arguments["CompanyName"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "CompanyName");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            string Bank = (vl_arguments["Bank"] == null) ? "" : vl_arguments["Bank"].AsString;
            string BankAccount = (vl_arguments["BankAccount"] == null) ? "" : vl_arguments["BankAccount"].AsString;
            string StreetAddress = (vl_arguments["StreetAddress"] == null) ? "" : vl_arguments["StreetAddress"].AsString;
            string City = (vl_arguments["City"] == null) ? "" : vl_arguments["City"].AsString;
            string PostalCode = (vl_arguments["PostalCode"] == null) ? "" : vl_arguments["PostalCode"].AsString;
            int ID_County = ((vl_arguments["ID_County"] != null) && (vl_arguments["ID_County"].ValueType == TVariantType.vtByte ||
                                                                    vl_arguments["ID_County"].ValueType == TVariantType.vtInt16 ||
                                                                    vl_arguments["ID_County"].ValueType == TVariantType.vtInt32 ||
                                                                    vl_arguments["ID_County"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_County"].AsInt32 : 0;




            //  validation
            int ID_Agency = vl_arguments["ID_Agency"].AsInt32;
            string Code = vl_arguments["Code"].AsString;
            string Name = vl_arguments["Name"].AsString;
            string FiscalCode = vl_arguments["FiscalCode"].AsString;
            string RegisterCode = vl_arguments["RegisterCode"].AsString;
            string CompanyName = vl_arguments["Name"].AsString;
            /*string Bank = vl_arguments["Bank"].AsString;
            string BankAccount = vl_arguments["BankAccount"].AsString;
            string StreetAddress = vl_arguments["StreetAddress"].AsString;
            string City = vl_arguments["City"].AsString;           
            string PostalCode = vl_arguments["PostalCode"].AsString;
            int ID_County = 0;
            if (vl_arguments["ID_County"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt64) ID_County = vl_arguments["ID_County"].AsInt32;
            else return -1;
            */

            string FirstName = "";
            if (vl_arguments["FirstName"] != null)
                if (vl_arguments["FirstName"].ValueType == TVariantType.vtString) FirstName = vl_arguments["FirstName"].AsString;

            string LastName = "";
            if (vl_arguments["LastName"] != null)
                if (vl_arguments["LastName"].ValueType == TVariantType.vtString) LastName = vl_arguments["LastName"].AsString;

            string Phone = (vl_arguments["Phone"] == null) ? "" : vl_arguments["Phone"].AsString;
            string Mobile = (vl_arguments["Mobile"] == null) ? "" : vl_arguments["Mobile"].AsString;
            string Fax = (vl_arguments["Fax"] == null) ? "" : vl_arguments["Fax"].AsString;
            string Email = (vl_arguments["Email"] == null) ? "" : vl_arguments["Email"].AsString;
            string Website = (vl_arguments["Website"] == null) ? "" : vl_arguments["Website"].AsString;

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");
            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs = new TVariantList();

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                vl_params.Add("@prm_isHouse").AsBoolean = false;
                vl_params.Add("@prm_Code").AsString = Code;
                vl_params.Add("@prm_Name").AsString = Name;
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;


                int int_count_Clients = app.DB.Exec("insert_Client", "Clients", vl_params);
                if (int_count_Clients <= 0) throw new Exception("SQL execution error");
                int ID_Client = app.DB.GetIdentity();

                int ID_Address = 0;
                int int_count_Address = 0;
                if (StreetAddress != "")
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_StreetAddress").AsString = StreetAddress;
                    vl_params.Add("@prm_City").AsString = City;
                    vl_params.Add("@prm_ID_County").AsInt32 = ID_County;
                    vl_params.Add("@prm_PostalCode").AsString = PostalCode;
                    int_count_Address = app.DB.Exec("insert_Address", "Addresses", vl_params);
                    if (int_count_Address <= 0) throw new Exception("SQL execution error");
                    ID_Address = app.DB.GetIdentity();
                }

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_ID_Broker").AsInt32 = 0;
                vl_params.Add("@prm_IsCompany").AsBoolean = true;
                vl_params.Add("@prm_FiscalCode").AsString = FiscalCode;
                vl_params.Add("@prm_RegisterCode").AsString = RegisterCode;
                vl_params.Add("@prm_CompanyName").AsString = CompanyName;
                vl_params.Add("@prm_SocialCode").AsString = "";
                vl_params.Add("@prm_IdentityCard").AsString = "";
                vl_params.Add("@prm_Title").AsString = "";
                vl_params.Add("@prm_FirstName").AsString = FirstName;
                vl_params.Add("@prm_LastName").AsString = LastName;
                vl_params.Add("@prm_ID_Address").AsInt32 = ID_Address;
                vl_params.Add("@prm_Bank").AsString = Bank;
                vl_params.Add("@prm_BankAccount").AsString = BankAccount;
                vl_params.Add("@prm_Phone").AsString = Phone;
                vl_params.Add("@prm_Mobile").AsString = Mobile;
                vl_params.Add("@prm_Fax").AsString = Fax;
                vl_params.Add("@prm_Email").AsString = Email;
                vl_params.Add("@prm_Website").AsString = Website;

                int int_count_Identities = app.DB.Exec("insert_Identity", "Identities", vl_params);
                if (int_count_Identities <= 0) throw new Exception("SQL execution error");

                app.DB.CommitTransaction();
                res.RowsModified = int_count_Clients + int_count_Identities + int_count_Address;
                res.IDs.Add("ID_Client").AsInt32 = ID_Client;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public int editClient(TVariantList vl_arguments)
        {

            if (vl_arguments["ID_Client"] == null) return -1;
            if (vl_arguments["ID_Agency"] == null) return -1;
            if (vl_arguments["Code"] == null) return -1;
            if (vl_arguments["Name"] == null) return -1;
            if (vl_arguments["FiscalCode"] == null) return -1;
            if (vl_arguments["RegisterCode"] == null) return -1;
            //if (vl_arguments["CompanyName"] == null) return -1;
            if (vl_arguments["isHouse"] == null) return -1;
            /*if (vl_arguments["Bank"] == null) return -1;
            if (vl_arguments["BankAccount"] == null) return -1;
            if (vl_arguments["StreetAddress"] == null) return -1;
            if (vl_arguments["City"] == null) return -1;            
            if (vl_arguments["PostalCode"] == null) return -1;
            if (vl_arguments["ID_County"] == null) return -1;                        */

            string Bank = (vl_arguments["Bank"] == null) ? "" : vl_arguments["Bank"].AsString;
            string BankAccount = (vl_arguments["BankAccount"] == null) ? "" : vl_arguments["BankAccount"].AsString;
            string StreetAddress = (vl_arguments["StreetAddress"] == null) ? "" : vl_arguments["StreetAddress"].AsString;
            string City = (vl_arguments["City"] == null) ? "" : vl_arguments["City"].AsString;
            string PostalCode = (vl_arguments["PostalCode"] == null) ? "" : vl_arguments["PostalCode"].AsString;
            int ID_County = ((vl_arguments["ID_County"] != null) && (vl_arguments["ID_County"].ValueType == TVariantType.vtByte ||
                                                                    vl_arguments["ID_County"].ValueType == TVariantType.vtInt16 ||
                                                                    vl_arguments["ID_County"].ValueType == TVariantType.vtInt32 ||
                                                                    vl_arguments["ID_County"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_County"].AsInt32 : 0;

            //  validation
            //int ID_Agency = vl_arguments["ID_Agency"].AsInt32;
            int ID_Agency = vl_arguments["ID_Agency"].AsInt32;
            int ID_Client = vl_arguments["ID_Client"].AsInt32;
            //  validation
            string Code = vl_arguments["Code"].AsString;
            string Name = vl_arguments["Name"].AsString;
            string FiscalCode = vl_arguments["FiscalCode"].AsString;
            string RegisterCode = vl_arguments["RegisterCode"].AsString;
            string CompanyName = vl_arguments["Name"].AsString;            
            bool isHouse = false;
            if (vl_arguments["isHouse"].ValueType == TVariantType.vtBoolean) isHouse = vl_arguments["isHouse"].AsBoolean;

            /*string Bank = vl_arguments["Bank"].AsString;
            string BankAccount = vl_arguments["BankAccount"].AsString;
            string StreetAddress = vl_arguments["StreetAddress"].AsString;
            string City = vl_arguments["City"].AsString;           
            string PostalCode = vl_arguments["PostalCode"].AsString;
            int ID_County = 0;
            if (vl_arguments["ID_County"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt64) ID_County = vl_arguments["ID_County"].AsInt32;
            else return -1;
            */

            string FirstName = "";
            if (vl_arguments["FirstName"] != null)
                if (vl_arguments["FirstName"].ValueType == TVariantType.vtString) FirstName = vl_arguments["FirstName"].AsString;

            string LastName = "";
            if (vl_arguments["LastName"] != null)
                if (vl_arguments["LastName"].ValueType == TVariantType.vtString) LastName = vl_arguments["LastName"].AsString;

            string Phone = (vl_arguments["Phone"] == null) ? "" : vl_arguments["Phone"].AsString;
            string Mobile = (vl_arguments["Mobile"] == null) ? "" : vl_arguments["Mobile"].AsString;
            string Fax = (vl_arguments["Fax"] == null) ? "" : vl_arguments["Fax"].AsString;
            string Email = (vl_arguments["Email"] == null) ? "" : vl_arguments["Email"].AsString;
            string Website = (vl_arguments["Website"] == null) ? "" : vl_arguments["Website"].AsString;

            int ID_Address = 0;
            if ((vl_arguments["ID_Address"] != null) && (vl_arguments["ID_Address"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Address"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Address"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Address"].ValueType == TVariantType.vtInt64)) ID_Address = vl_arguments["ID_Address"].AsInt32;
            /*else return -1;*/

            TVariantList vl_params = new TVariantList();
            //vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;            
            vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
            DataSet ds_identity = app.DB.Select("select_IdentitiesbyID_Client", "Identities", vl_params);
            if (ds_identity == null) return -1;
            if (ds_identity.Tables.Count == 0) return -1;
            if (ds_identity.Tables[0].Rows.Count == 0) return -1;
            int ID_Identity = Convert.ToInt32(ds_identity.Tables[0].Rows[0]["ID"]);
            ID_Address = Convert.ToInt32(ds_identity.Tables[0].Rows[0]["ID_Address"]);

            if (!app.DB.BeginTransaction()) return -1;
            try
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_Code").AsString = Code;
                vl_params.Add("@prm_Name").AsString = Name;
                vl_params.Add("@prm_isHouse").AsBoolean = isHouse;
                vl_params.Add("@prm_ID").AsInt32 = ID_Client;

                int int_count_Clients = app.DB.Exec("update_Client", "Clients", vl_params);
                if (int_count_Clients <= 0) throw new Exception("SQL execution error");

                int int_count_Address = 0;
                if (StreetAddress != "")
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_StreetAddress").AsString = StreetAddress;
                    vl_params.Add("@prm_City").AsString = City;
                    vl_params.Add("@prm_ID_County").AsInt32 = ID_County;
                    vl_params.Add("@prm_PostalCode").AsString = PostalCode;

                    if (ID_Address > 0)
                    {
                        vl_params.Add("@prm_ID_Address").AsInt32 = ID_Address;
                        int_count_Address = app.DB.Exec("update_Address", "Addresses", vl_params);
                        if (int_count_Address < 0) throw new Exception("SQL execution error");
                    }
                    else
                    {
                        int_count_Address = app.DB.Exec("insert_Address", "Addresses", vl_params);
                        if (int_count_Address < 0) throw new Exception("SQL execution error");
                        ID_Address = app.DB.GetIdentity();
                    }
                }

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_ID_Broker").AsInt32 = 0;
                vl_params.Add("@prm_IsCompany").AsBoolean = true;
                vl_params.Add("@prm_FiscalCode").AsString = FiscalCode;
                vl_params.Add("@prm_RegisterCode").AsString = RegisterCode;
                vl_params.Add("@prm_CompanyName").AsString = CompanyName;
                vl_params.Add("@prm_SocialCode").AsString = "";
                vl_params.Add("@prm_IdentityCard").AsString = "";
                vl_params.Add("@prm_Title").AsString = "";
                vl_params.Add("@prm_FirstName").AsString = FirstName;
                vl_params.Add("@prm_LastName").AsString = LastName;
                vl_params.Add("@prm_Bank").AsString = Bank;
                vl_params.Add("@prm_BankAccount").AsString = BankAccount;
                vl_params.Add("@prm_ID_Address").AsInt32 = ID_Address;
                vl_params.Add("@prm_Phone").AsString = Phone;
                vl_params.Add("@prm_Mobile").AsString = Mobile;
                vl_params.Add("@prm_Fax").AsString = Fax;
                vl_params.Add("@prm_Email").AsString = Email;
                vl_params.Add("@prm_Website").AsString = Website;
                vl_params.Add("@prm_ID").AsInt32 = ID_Identity;

                int int_count_Identities = app.DB.Exec("update_Identity", "Identities", vl_params);
                if (int_count_Identities <= 0) throw new Exception("SQL execution error");

                app.DB.CommitTransaction();
                return int_count_Clients + int_count_Identities + int_count_Address;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return -1;
            }

            return 0;
        }

        public TDBExecResult deleteClient(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Client"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Client");

            //  validation
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int idClient = 0;
            try { idClient = vl_arguments["ID_Client"].AsInt32; }
            catch { res.ParamValidations["ID_Client"].AsString = "Value not recognized"; }

            if (res.bInvalid()) return res;

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Client").AsInt32 = idClient;

                int countDeleted = app.DB.Exec("delete_Client", "Clients", vl_params);
                if (countDeleted <= 0) throw new Exception("Client deletion failed");

                res.RowsModified = countDeleted;
                return res;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult validateOrder_RGN(TVariantList vl_arguments)
        {
            //-----------------------------------------------------------------------
            //  check if arguments are provided
            //  generic
            if (vl_arguments["Direction"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Direction");
            if (vl_arguments["Quantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Quantity");
            if (vl_arguments["Price"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Price");

            //  specific arguments
            if (vl_arguments["ID_GNType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_GNType");
            if (vl_arguments["StartDeliveryDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StartDeliveryDate");
            if (vl_arguments["EndDeliveryDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "EndDeliveryDate");
            if (vl_arguments["CombinationsAccepted"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "CombinationsAccepted");
            //-----------------------------------------------------------------------

            //-----------------------------------------------------------------------
            //  extract values and validate
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            int ID_Ring = 1;  //  regularizare gaze
            int ID_Asset = 1; //  gaz natural
            int ID_Broker = session.ID_Broker;
            int ID_Client = 0;
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
            DataSet ds_client = app.DB.Select("select_HousebyID_Broker", "Clients", vl_params);
            if (ds_client != null)
                if (ds_client.Tables.Count > 0)
                    if (ds_client.Tables[0].Rows.Count > 0)
                    {
                        ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
                    }

            if (ID_Broker == 0 || ID_Client == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Current user is not attached to a broker");

            bool Direction = false;
            switch (vl_arguments["Direction"].AsString.Trim().ToUpper())
            {
                case "B":
                    Direction = false;
                    break;
                case "S":
                    Direction = true;
                    break;
                default:
                    res.ParamValidations["Direction"].AsString = "Expected value 'B' or 'S'. Actual value: " + vl_arguments["Direction"].AsString;
                    break;
                //return new TDBExecResult() { ErrorCode = TDBExecError.ValidationUnsuccesful, Message = "Argument value differs from expected.", ParamValidations = new TVariantList().Add("Direction").AsString = "Expected values B or S" };
            }

            double Quantity = 0;
            if (vl_arguments["Quantity"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtInt32 ||                
                vl_arguments["Quantity"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtSingle ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtDouble) Quantity = vl_arguments["Quantity"].AsDouble; 
            else res.ParamValidations["Quantity"].AsString = "Unrecognized Value";
            if (Quantity <= 0 && res.ParamValidations["Quantity"].AsString == "") res.ParamValidations["Quantity"].AsString = "Value cannot be negative";

            double Price = 0;
            if (vl_arguments["Price"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Price"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Price"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["Price"].ValueType == TVariantType.vtSingle ||
                vl_arguments["Price"].ValueType == TVariantType.vtDouble) Price = vl_arguments["Price"].AsDouble;
            else res.ParamValidations["Price"].AsString = "Unrecognized Value";
            if (Price <= 0 && res.ParamValidations["Price"].AsString == "") res.ParamValidations["Price"].AsString = "Value cannot be negative";

            //  specific arguments
            int ID_GNType = 0;
            if (vl_arguments["ID_GNType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_GNType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_GNType"].ValueType == TVariantType.vtInt64) ID_GNType = vl_arguments["ID_GNType"].AsInt32;

            DateTime StartDeliveryDate = DateTime.Now;
            if (vl_arguments["StartDeliveryDate"].ValueType == TVariantType.vtDateTime) StartDeliveryDate = vl_arguments["StartDeliveryDate"].AsDateTime;
            else res.ParamValidations["StartDeliveryDate"].AsString = "Unrecognized Value";

            DateTime EndDeliveryDate = DateTime.Now;
            if (vl_arguments["EndDeliveryDate"].ValueType == TVariantType.vtDateTime) EndDeliveryDate = vl_arguments["EndDeliveryDate"].AsDateTime;
            else res.ParamValidations["EndDeliveryDate"].AsString = "Unrecognized Value";

            bool CombinationsAccepted = false;
            if (vl_arguments["CombinationsAccepted"].ValueType == TVariantType.vtBoolean) CombinationsAccepted = vl_arguments["CombinationsAccepted"].AsBoolean;
            else res.ParamValidations["CombinationsAccepted"].AsString = "Unrecognized Value";

            if (res.ParamValidations["StartDeliveryDate"].AsString == "" && res.ParamValidations["EndDeliveryDate"].AsString == "")
            {
                //added by Alex P
                int startDeliveryDateOffset = 3;
                int endDeliveryDateOffset = 30;
                TVariantList vl = new TVariantList();
                DataSet ds = app.DB.Select("select_RingParameters_RGN", "RingParameters_RGN", vl);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    startDeliveryDateOffset = Convert.ToInt32(ds.Tables[0].Rows[0]["StartDeliveryDateOffset"].ToString());
                    endDeliveryDateOffset = Convert.ToInt32(ds.Tables[0].Rows[0]["EndDeliveryDateOffset"].ToString());
                }
                //---------------

                if (StartDeliveryDate < DateTime.Today.AddDays(startDeliveryDateOffset)) res.ParamValidations["StartDeliveryDate"].AsString = "Start Delivery Date must be at least " + DateTime.Today.AddDays(3).ToString();
                if (StartDeliveryDate > DateTime.Today.AddDays(endDeliveryDateOffset)) res.ParamValidations["StartDeliveryDate"].AsString = "Start Delivery Date must be at most " + DateTime.Today.AddDays(30).ToString();
                if (EndDeliveryDate < StartDeliveryDate) res.ParamValidations["EndDeliveryDate"].AsString = "End Delivery Date cannot be less than Start Delivery Date";
                if (EndDeliveryDate > DateTime.Today.AddDays(endDeliveryDateOffset)) res.ParamValidations["EndDeliveryDate"].AsString = "End Delivery Date must be at most " + DateTime.Today.AddDays(30).ToString();
            }

            if (res.bInvalid()) return res;

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult validateOrder_DISPONIBIL(TVariantList vl_arguments)
        {
            //-----------------------------------------------------------------------
            //  check if arguments are provided
            //  generic
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Ring");
            if (vl_arguments["Direction"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Direction");
            if (vl_arguments["Quantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Quantity");
            if (vl_arguments["Price"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Price");

            //-----------------------------------------------------------------------
            //  extract values and validate
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            int ID_Order = 0;
            if (vl_arguments["ID_Order"] != null)
            {
                if (vl_arguments["ID_Order"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Order"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Order"].ValueType == TVariantType.vtInt64) ID_Order = vl_arguments["ID_Order"].AsInt32;
                else res.ParamValidations["ID_Order"].AsString = "Unrecognized value";
            }

            bool isInitial = false;
            if (vl_arguments["isInitial"] != null)
            {
                if (vl_arguments["isInitial"].ValueType == TVariantType.vtBoolean) isInitial = vl_arguments["isInitial"].AsBoolean;
                else res.ParamValidations["isInitial"].AsString = "Unrecognized Value";
            }

            int ID_Ring = 0;
            if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
            else res.ParamValidations["ID_Ring"].AsString = "Unrecognized Value";
            if (ID_Ring <= 0 && res.ParamValidations["ID_Ring"].AsString == "") res.ParamValidations["ID_Ring"].AsString = "Invalid Ring";

            int ID_Asset = 0;
            if (vl_arguments["ID_Asset"] != null)
            {
                if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
                else res.ParamValidations["ID_Asset"].AsString = "Unrecognized Value";
            }

            if (!isInitial)
            {
                if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");
                if (ID_Asset <= 0 && res.ParamValidations["ID_Asset"].AsString == "") res.ParamValidations["ID_Asset"].AsString = "Invalid Asset";
            }

            int ID_Agency = 0;
            int ID_Broker = 0;
            int ID_Client = 0;

            UserValidation uv = new UserValidation(app);
            if (uv.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole))
            {
                if (vl_arguments["ID_Agency"] != null) ID_Agency = vl_arguments["ID_Agency"].AsInt32;
                if (vl_arguments["ID_Broker"] != null) ID_Broker = vl_arguments["ID_Broker"].AsInt32;
            }
            else ID_Broker = session.ID_Broker;

            if (vl_arguments["ID_Client"] != null) ID_Client = vl_arguments["ID_Client"].AsInt32;

            TVariantList vl_params;
            if (ID_Broker != 0)
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                DataSet ds_client = app.DB.Select("select_HousebyID_Broker", "Clients", vl_params);
                if (app.DB.ValidDSRows(ds_client))
                {
                    ID_Agency = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID_Agency"]);
                    if (ID_Client == 0 && ds_client.Tables[0].Rows[0]["ID"] != DBNull.Value) ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
                }
            }

            if (ID_Agency == 0 && (ID_Broker == 0 || ID_Client == 0)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Current user is not attached to a broker");

            //  get current session for asset
            int ID_RingSession = 0;
            int ID_AssetSession = 0;

            if (!isInitial)
            {
                DataSet ds_ringsession = getRingSession(ID_Ring);
                DataSet ds_assetsession = getAssetSession(ID_Asset);

                if (!(app.DB.ValidDS(ds_ringsession) && app.DB.ValidDS(ds_assetsession))) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

                if (!app.DB.ValidDSRows(ds_ringsession)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
                if (Convert.ToDateTime(ds_ringsession.Tables[0].Rows[0]["Date"]).Date != DateTime.Today) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
                //if (ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "Opened" && ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "PreOpened") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
                ID_RingSession = Convert.ToInt32(ds_ringsession.Tables[0].Rows[0]["ID"]);

                if (!app.DB.ValidDSRows(ds_assetsession)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
                if (Convert.ToDateTime(ds_assetsession.Tables[0].Rows[0]["Date"]).Date != DateTime.Today) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
                if (ds_assetsession.Tables[0].Rows[0]["Status"].ToString() != "Opened" &&
                    ds_assetsession.Tables[0].Rows[0]["Status"].ToString() != "PreOpened" &&
                    ds_assetsession.Tables[0].Rows[0]["Status"].ToString() != "PreClosed") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Sedinta nu se afla intr-o faza de admitere noi ordine");
                ID_AssetSession = Convert.ToInt32(ds_assetsession.Tables[0].Rows[0]["ID"]);
            }

            bool Direction = false;
            switch (vl_arguments["Direction"].AsString.Trim().ToUpper())
            {
                case "B":
                    Direction = false;
                    break;
                case "S":
                    Direction = true;
                    break;
                default:
                    res.ParamValidations["Direction"].AsString = "Expected value 'B' or 'S'. Actual value: " + vl_arguments["Direction"].AsString;
                    break;
            }

            double Quantity = 0;
            if (vl_arguments["Quantity"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtSingle ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtDouble) Quantity = vl_arguments["Quantity"].AsDouble;
            else res.ParamValidations["Quantity"].AsString = "Unrecognized Value";
            if (Quantity <= 0 && res.ParamValidations["Quantity"].AsString == "") res.ParamValidations["Quantity"].AsString = "Cantitatea trebuie sa fie mai mare ca 0";

            double Price = 0;
            if (isInitial && vl_arguments["Price"].ValueType == TVariantType.vtString && vl_arguments["Price"].AsString == "none") ;
            else
            {
                if (vl_arguments["Price"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["Price"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["Price"].ValueType == TVariantType.vtInt64 ||
                    vl_arguments["Price"].ValueType == TVariantType.vtSingle ||
                    vl_arguments["Price"].ValueType == TVariantType.vtDouble) Price = vl_arguments["Price"].AsDouble;
                else res.ParamValidations["Price"].AsString = "Unrecognized Value";
            }
            //if (!isInitial && Price <= 0 && res.ParamValidations["Price"].AsString == "") res.ParamValidations["Price"].AsString = " Value cannot be negative";

            bool PartialFlag = false;
            if (vl_arguments["PartialFlag"] != null)
            {
                if (vl_arguments["PartialFlag"].ValueType == TVariantType.vtBoolean) PartialFlag = vl_arguments["PartialFlag"].AsBoolean;
            }

            if (vl_arguments["isPartial"] != null)
            {
                if (vl_arguments["isPartial"].ValueType == TVariantType.vtBoolean) PartialFlag = vl_arguments["isPartial"].AsBoolean;
            }

            DateTime ExpirationDate = DateTime.Now;
            if (vl_arguments["ExpirationDate"] != null)
                if (vl_arguments["ExpirationDate"].ValueType == TVariantType.vtDateTime)
                    ExpirationDate = vl_arguments["ExpirationDate"].AsDateTime;

            DateTime SubmitTime = DateTime.Now;
            if (vl_arguments["SubmitTime"] != null)
                if (vl_arguments["SubmitTime"].ValueType == TVariantType.vtDateTime)
                    SubmitTime = vl_arguments["SubmitTime"].AsDateTime;

            //-----------------------------------------------------------------------
            //  check for existing order
            if (ID_Order != 0)
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_Order;
                DataSet dsOrder = app.DB.Select("select_Orders", "Orders", vl_params);
                if (dsOrder == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve original order");
                if (dsOrder.Tables.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve original order");
                if (dsOrder.Tables[0].Rows.Count == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Could not retrieve original order");

                double qty_Transacted = Convert.ToDouble(dsOrder.Tables[0].Rows[0]["TransactedQuantity"]);
                Quantity += qty_Transacted;
            }

            bool isDoubleTrouble = false;
            bool isSameAsInitial = false;
            if (ID_AssetSession != 0 && !uv.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole))
            {
                string s = validateTradeParameters(ID_Order, ID_Asset, ID_AssetSession, ID_Broker, ID_Client, Direction, Quantity, Price, PartialFlag, SubmitTime, ref isDoubleTrouble, ref isSameAsInitial);
                if (s != "")
                {
                    string action = (ID_Order == 0) ? "Adaugare Ordin: " : "Modificare Ordin: ";
                    session.AddToJournal("validateOrder", ID_Client, ID_Ring, ID_Asset, ID_Order, Quantity, Price, action + s);
                    return new TDBExecResult(TDBExecError.ValidationUnsuccesful, s);
                }

                s = validateWarranty(ID_Order, ID_Asset, ID_AssetSession, ID_Broker, ID_Client, Direction, Quantity, Price, PartialFlag, SubmitTime);
                if (s != "")
                {
                    string action = (ID_Order == 0) ? "Adaugare Ordin: " : "Modificare Ordin: ";
                    session.AddToJournal("validateOrder", ID_Client, ID_Ring, ID_Asset, ID_Order, Quantity, Price, s);
                    return new TDBExecResult(TDBExecError.ValidationUnsuccesful, s);
                }
            }

            //if order IsInitial, we expect the params for asset creation
            //int ID_Market = 0;
            int ID_AssetType = 0;
            string str_AssetCode = "";
            string str_AssetName = "";
            string str_AssetMeasuringUnit = "";
            bool bit_isDefault = false;

            if (isInitial && ID_Asset <= 0)
            {
                if (vl_arguments["ID_AssetType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AssetType");
                if (vl_arguments["AssetCode"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
                if (vl_arguments["AssetName"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
                if (vl_arguments["AssetMeasuringUnit"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MeasuringUnit");

                ID_AssetType = 0;
                if (vl_arguments["ID_AssetType"].ValueType == TVariantType.vtByte ||
                    vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt64) ID_AssetType = vl_arguments["ID_AssetType"].AsInt32;
                else res.ParamValidations["ID_AssetType"].AsString = "Value type not recognized";

                str_AssetCode = "";
                if (vl_arguments["AssetCode"].ValueType == TVariantType.vtString) str_AssetCode = vl_arguments["AssetCode"].AsString;
                else res.ParamValidations["AssetCode"].AsString = "Value type not recognized";

                str_AssetName = "";
                if (vl_arguments["AssetName"].ValueType == TVariantType.vtString) str_AssetName = vl_arguments["AssetName"].AsString;
                else res.ParamValidations["AssetName"].AsString = "Value type not recognized";

                str_AssetMeasuringUnit = "";
                if (vl_arguments["AssetMeasuringUnit"].ValueType == TVariantType.vtString) str_AssetMeasuringUnit = vl_arguments["AssetMeasuringUnit"].AsString;
                else res.ParamValidations["AssetMeasuringUnit"].AsString = "Value type not recognized";
            }
            //--------------

            //  specific arguments

            //-----------------------------
            if (res.bInvalid()) return res;

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult validateOrder(TVariantList vl_arguments)
        {
            switch (session.EntryPoint)
            {
                case "BTGN":
                    return validateOrder_RGN(vl_arguments);
                case "DISPONIBIL":
                    return validateOrder_DISPONIBIL(vl_arguments);
                default:
                    return new TDBExecResult(TDBExecError.ProcedureNotFound, "");
            }
        }

        public DataSet getRingSession(/*TVariantList vl_arguments*/int ID_Ring)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;

            DataSet ds = app.DB.Select("select_CurrentSession", "RingSessions", vl_params);
            return ds;
        }

        public DataSet getAssetSession(/*TVariantList vl_arguments*/int ID_Asset)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            DataSet ds = app.DB.Select("select_CurrentSession", "AssetSessions", vl_params);
            return ds;
        }


        public TDBExecResult addOrder_RGN(TVariantList vl_arguments)
        {
            //-----------------------------------------------------------------------
            //  check if arguments are provided
            //  generic
            if (vl_arguments["Direction"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Direction");
            if (vl_arguments["Quantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Quantity");
            if (vl_arguments["Price"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Price");

            //  specific arguments
            if (vl_arguments["ID_GNType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_GNType");
            if (vl_arguments["StartDeliveryDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StartDeliveryDate");
            if (vl_arguments["EndDeliveryDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "EndDeliveryDate");
            if (vl_arguments["CombinationsAccepted"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "CombinationsAccepted");
            if (vl_arguments["EntryPoints"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "EntryPoints");
            //-----------------------------------------------------------------------

            //-----------------------------------------------------------------------
            //  extract values and validate
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            int ID_Ring = 1;  //  regularizare gaze
            int ID_Asset = 1; //  gaz natural

            int ID_Agency = 0;
            int ID_Broker = session.ID_Broker;
            int ID_Client = 0;
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
            DataSet ds_client = app.DB.Select("select_HousebyID_Broker", "Clients", vl_params);
            if (!app.DB.ValidDSRows(ds_client)) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            else
            {
                ID_Agency = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID_Agency"]);
                ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
            }

            if (ID_Broker == 0 || ID_Client == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Current user is not attached to a broker");

            //  get current session for asset
            DataSet ds_ringsession = getRingSession(1);
            DataSet ds_assetsession = getAssetSession(1);
            if (ds_ringsession == null || ds_assetsession == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (ds_ringsession.Tables.Count == 0 || ds_assetsession.Tables.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (ds_ringsession.Tables[0].Rows.Count == 0 || ds_assetsession.Tables[0].Rows.Count == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            if (Convert.ToDateTime(ds_ringsession.Tables[0].Rows[0]["Date"]).Date != DateTime.Today) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            if (Convert.ToDateTime(ds_assetsession.Tables[0].Rows[0]["Date"]).Date != DateTime.Today) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            if (ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "Opened" && ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "PreOpened") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            if (ds_assetsession.Tables[0].Rows[0]["Status"].ToString() != "Opened" && ds_assetsession.Tables[0].Rows[0]["Status"].ToString() != "PreOpened") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            int ID_RingSession = Convert.ToInt32(ds_ringsession.Tables[0].Rows[0]["ID"]);
            int ID_AssetSession = Convert.ToInt32(ds_assetsession.Tables[0].Rows[0]["ID"]);

            bool Direction = false;
            switch (vl_arguments["Direction"].AsString.Trim().ToUpper())
            {
                case "B":
                    Direction = false;
                    break;
                case "S":
                    Direction = true;
                    break;
                default:
                    res.ParamValidations["Direction"].AsString = "Expected value 'B' or 'S'. Actual value: " + vl_arguments["Direction"].AsString;
                    break;
                    //return new TDBExecResult() { ErrorCode = TDBExecError.ValidationUnsuccesful, Message = "Argument value differs from expected.", ParamValidations = new TVariantList().Add("Direction").AsString = "Expected values B or S" };
            }

            double Quantity = 0;
            if (vl_arguments["Quantity"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtSingle ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtDouble) Quantity = vl_arguments["Quantity"].AsDouble;
            else res.ParamValidations["Quantity"].AsString = "Unrecognized Value";
            if (Quantity <= 0 && res.ParamValidations["Quantity"].AsString == "") res.ParamValidations["Quantity"].AsString = "Value cannot be negative";

            double Price = 0;
            if (vl_arguments["Price"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Price"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Price"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["Price"].ValueType == TVariantType.vtSingle ||
                vl_arguments["Price"].ValueType == TVariantType.vtDouble) Price = vl_arguments["Price"].AsDouble;
            else res.ParamValidations["Price"].AsString = "Unrecognized Value";
            if (Price <= 0 && res.ParamValidations["Price"].AsString == "") res.ParamValidations["Price"].AsString = "Value cannot be negative";

            //  specific arguments
            int ID_GNType = 0;
            if (vl_arguments["ID_GNType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_GNType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_GNType"].ValueType == TVariantType.vtInt64) ID_GNType = vl_arguments["ID_GNType"].AsInt32;

            DateTime StartDeliveryDate = DateTime.Now;
            if (vl_arguments["StartDeliveryDate"].ValueType == TVariantType.vtDateTime) StartDeliveryDate = vl_arguments["StartDeliveryDate"].AsDateTime;
            else res.ParamValidations["StartDeliveryDate"].AsString = "Unrecognized Value";

            DateTime EndDeliveryDate = DateTime.Now;
            if (vl_arguments["EndDeliveryDate"].ValueType == TVariantType.vtDateTime) EndDeliveryDate = vl_arguments["EndDeliveryDate"].AsDateTime;
            else res.ParamValidations["EndDeliveryDate"].AsString = "Unrecognized Value";

            bool CombinationsAccepted = false;
            if (vl_arguments["CombinationsAccepted"].ValueType == TVariantType.vtBoolean) CombinationsAccepted = vl_arguments["CombinationsAccepted"].AsBoolean;
            else res.ParamValidations["CombinationsAccepted"].AsString = "Unrecognized Value";

            if (res.ParamValidations["StartDeliveryDate"].AsString == "" && res.ParamValidations["EndDeliveryDate"].AsString == "")
            {
                //added by Alex P
                int startDeliveryDateOffset = 3;
                int endDeliveryDateOffset = 30;
                TVariantList vl = new TVariantList();
                DataSet ds = app.DB.Select("select_RingParameters_RGN", "RingParameters_RGN", vl);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    startDeliveryDateOffset = Convert.ToInt32(ds.Tables[0].Rows[0]["StartDeliveryDateOffset"].ToString());
                    endDeliveryDateOffset = Convert.ToInt32(ds.Tables[0].Rows[0]["EndDeliveryDateOffset"].ToString());
                }
                //---------------

                //modified by Alex P. on 20140529
                if (StartDeliveryDate < DateTime.Today.AddDays(startDeliveryDateOffset)) res.ParamValidations["StartDeliveryDate"].AsString = "Start Delivery Date must be at least " + DateTime.Today.AddDays(startDeliveryDateOffset).ToString();
                if (StartDeliveryDate > DateTime.Today.AddDays(endDeliveryDateOffset)) res.ParamValidations["StartDeliveryDate"].AsString = "Start Delivery Date must be at most " + DateTime.Today.AddDays(endDeliveryDateOffset).ToString();
                if (EndDeliveryDate <= StartDeliveryDate) res.ParamValidations["EndDeliveryDate"].AsString = "End Delivery Date cannot be less or equal than Start Delivery Date";
                if (EndDeliveryDate > DateTime.Today.AddDays(endDeliveryDateOffset)) res.ParamValidations["EndDeliveryDate"].AsString = "End Delivery Date must be at most " + DateTime.Today.AddDays(endDeliveryDateOffset).ToString();
                //----------------
            }

            //added by Alex P. on 20140529
            TVariantList entryPoints = (TVariantList)vl_arguments["EntryPoints"].AsObject;
            for (int i = 0; i < entryPoints.Count; i++)
            {
                int entryPointID = Convert.ToInt32(entryPoints[i].Value);
                TVariantList vl = new TVariantList();
                vl.Add("@prm_ID").AsInt32 = entryPointID;               
                DataSet ds = app.DB.Select("select_EntryPoint_RGN", "EntryPoints_RGN", vl);

                if (ds == null)
                {
                    res.ParamValidations["EntryPoints"].AsString = "Innexistent entry point " + entryPointID.ToString();
                    return res;
                }

                if (ds.Tables.Count == 0)
                {
                    res.ParamValidations["EntryPoints"].AsString = "Innexistent entry point " + entryPointID.ToString();
                    return res;
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    res.ParamValidations["EntryPoints"].AsString = "Innexistent entry point " + entryPointID.ToString();
                    return res;
                }
            }
            
            //-----------------------------
            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");
            
            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs = new TVariantList();

                //  insert order
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_ID_RingSession").AsInt32 = ID_RingSession;
                vl_params.Add("@prm_ID_AssetSession").AsInt32 = ID_AssetSession;
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                vl_params.Add("@prm_Direction").AsBoolean = Direction;
                vl_params.Add("@prm_Quantity").AsDouble = Quantity;
                vl_params.Add("@prm_Price").AsDouble = Price;
                vl_params.Add("@prm_isActive").AsBoolean = true;

                /*int int_count_Orders = app.DB.Exec("insert_Order", "Orders", vl_params);
                if (int_count_Orders == -1) throw new Exception();
                int ID_Order = app.DB.GetIdentity();*/

                //vl_params = new TVariantList();
                //vl_params.Add("@prm_ID_Order").AsInt32 = ID_Order;
                vl_params.Add("@prm_ID_GNType").AsInt32 = ID_GNType;
                vl_params.Add("@prm_StartDeliveryDate").AsDateTime = StartDeliveryDate;
                vl_params.Add("@prm_EndDeliveryDate").AsDateTime = EndDeliveryDate;
                vl_params.Add("@prm_CombinationsAccepted").AsBoolean = CombinationsAccepted;

                /*int int_count_OrderDetails_RGN = app.DB.Exec("insert_OrderDetails_RGN", "Orders", vl_params);
                if (int_count_OrderDetails_RGN == -1) throw new Exception();
                int ID_OrderDetail_RGN = app.DB.GetIdentity();*/

                int int_count_Orders = app.DB.Exec("insert_Order_RGN", "Orders", vl_params);
                if (int_count_Orders == -1) throw new Exception();

                int ID_OrderDetail_RGN = app.DB.GetIdentity();               

                app.DB.CommitTransaction();

                //----------tbr---------------
                //this piece of code should be standing before app.DB.CommitTransaction();
                //added by Alex P on 20140529
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_OrderDetail_RGN;
                DataSet ds = app.DB.Select("select_OrderDetails", "Orders", vl_params);
                if ((ds == null) || (ds.Tables.Count == 0) || (ds.Tables[0].Rows.Count == 0))
                    return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "Order not found");

                int orderID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID_Order"].ToString());

                for (int i = 0; i < entryPoints.Count; i++)
                {
                    TVariantList vl = new TVariantList();
                    vl.Add("@prm_ID_Order").AsInt32 = orderID;
                    vl.Add("@prm_ID_EntryPoint").AsInt32 = Convert.ToInt32(entryPoints[i].Value);
                    int int_count_entrypoints = app.DB.Exec("insert_OrderEntryPoint_RGN", "EntryPoints_RGN", vl);
                    if (int_count_entrypoints == -1) throw new Exception();
                }
                //----------------------------

                //res.IDs.Add("ID_Order").AsInt32 = ID_Order;
                res.IDs.Add("ID_Order_RGN").AsInt32 = ID_OrderDetail_RGN;
                res.RowsModified = int_count_Orders;// +int_count_OrderDetails_RGN;
                session.AddToJournal("order added", ID_Client, ID_Ring, ID_Asset, orderID, Quantity, Price);
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }

        }

        public struct TTradeParameters
        {
            public bool PartialFlagChangeAllowed;
            public bool InitialPriceMandatory;
            public bool InitialPriceMaintenance;
            public bool DiminishedQuantityAllowed;
            public bool DiminishedPriceAllowed;
            public bool DifferentialPriceAllowed;
            public string DifferentialPriceText;
            public bool OppositeDirectionAllowed;
            public int DeltaT;
            public int DeltaT1;
            public double SellWarrantyPercent;
            public double SellWarrantyMU;
            public double SellWarrantyFixed;
            public double BuyWarrantyPercent;
            public double BuyWarrantyMU;
            public double BuyWarrantyFixed;

            public double QuantityStepping;
            public double MinQuantity;
            public double PriceStepping;
            public double MaxPriceVariation;
            public double MinPrice;
            public double MaxPrice;

            public DateTime PreOpeningTime;
            public DateTime OpeningTime;

            public double SpotQuotation;
            public double Quotation;
        }

        public TTradeParameters getTradeParameters(int ID_AssetSession)
        {
            TTradeParameters res = new TTradeParameters();

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_AssetSession").AsInt32 = ID_AssetSession;
            DataSet ds = app.DB.Select("select_ConsolidatedTradeParameters", "TradeParameters", vl_params);
            if (ds == null) return res;
            if (ds.Tables.Count == 0) return res;
            if (ds.Tables[0].Rows.Count == 0) return res;

            res.PartialFlagChangeAllowed = Convert.ToBoolean(ds.Tables[0].Rows[0]["PartialFlagChangeAllowed"]);
            res.InitialPriceMandatory = Convert.ToBoolean(ds.Tables[0].Rows[0]["InitialPriceMandatory"]);
            res.InitialPriceMaintenance = Convert.ToBoolean(ds.Tables[0].Rows[0]["InitialPriceMaintenance"]);
            res.DiminishedQuantityAllowed = Convert.ToBoolean(ds.Tables[0].Rows[0]["DiminishedQuantityAllowed"]);
            res.DiminishedPriceAllowed = Convert.ToBoolean(ds.Tables[0].Rows[0]["DiminishedPriceAllowed"]);
            res.DifferentialPriceAllowed = Convert.ToBoolean(ds.Tables[0].Rows[0]["DifferentialPriceAllowed"]);
            res.DifferentialPriceText = ds.Tables[0].Rows[0]["DifferentialPriceText"].ToString();
            res.OppositeDirectionAllowed = Convert.ToBoolean(ds.Tables[0].Rows[0]["OppositeDirectionAllowed"]);
            res.DeltaT = Convert.ToInt32(ds.Tables[0].Rows[0]["DeltaT"]);
            res.DeltaT1 = Convert.ToInt32(ds.Tables[0].Rows[0]["DeltaT1"]);

            res.SellWarrantyPercent = Convert.ToDouble(ds.Tables[0].Rows[0]["SellWarrantyPercent"]);
            res.SellWarrantyMU = Convert.ToDouble(ds.Tables[0].Rows[0]["SellWarrantyMU"]);
            res.SellWarrantyFixed = Convert.ToDouble(ds.Tables[0].Rows[0]["SellWarrantyFixed"]);
            res.BuyWarrantyPercent = Convert.ToDouble(ds.Tables[0].Rows[0]["BuyWarrantyPercent"]);
            res.BuyWarrantyMU = Convert.ToDouble(ds.Tables[0].Rows[0]["BuyWarrantyMU"]);
            res.BuyWarrantyFixed = Convert.ToDouble(ds.Tables[0].Rows[0]["BuyWarrantyFixed"]);

            res.QuantityStepping = Convert.ToDouble(ds.Tables[0].Rows[0]["QuantityStepping"]);
            res.MinQuantity = Convert.ToDouble(ds.Tables[0].Rows[0]["MinQuantity"]);
            res.PriceStepping = Convert.ToDouble(ds.Tables[0].Rows[0]["PriceStepping"]);
            res.MaxPriceVariation = Convert.ToDouble(ds.Tables[0].Rows[0]["MaxPriceVariation"]);
            res.MinPrice = Convert.ToDouble(ds.Tables[0].Rows[0]["MinPrice"]);
            res.MaxPrice = Convert.ToDouble(ds.Tables[0].Rows[0]["MaxPrice"]);

            res.PreOpeningTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["PreOpeningTime"]);
            res.OpeningTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["OpeningTime"]);

            res.SpotQuotation = Convert.ToDouble(ds.Tables[0].Rows[0]["SpotQuotation"]);
            res.Quotation = Convert.ToDouble(ds.Tables[0].Rows[0]["Quotation"]);

            return res;
        }

        public void updateTradeParameters(int ID_AssetType, int ID_Asset, int ID_Order, int ID_AssetSchedule, TVariantList vl_arguments)
        {
            TTradeParameters trd = new TTradeParameters()
            {
                PartialFlagChangeAllowed = false,
                InitialPriceMandatory = false,
                InitialPriceMaintenance = false,
                DiminishedQuantityAllowed = false,
                DiminishedPriceAllowed = false,
                DifferentialPriceAllowed = false,
                DifferentialPriceText = "",
                OppositeDirectionAllowed = false,
                DeltaT = 0,
                DeltaT1 = 0,
                SellWarrantyPercent = 0,
                SellWarrantyMU = 0,
                SellWarrantyFixed = 0,
                BuyWarrantyPercent = 0,
                BuyWarrantyMU = 0,
                BuyWarrantyFixed = 0
            };

            TVariantList vl_params;

            int prev_ID_AssetType = 0;
            int prev_ID_Asset = 0;
            int prev_ID_Order = 0;
            int prev_ID_AssetSchedule = 0;
            if (ID_Asset != 0)
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_Asset;
                DataSet ds_asset = app.DB.Select("select_Assets", "Assets", vl_params);
                if (app.DB.ValidDSRows(ds_asset)) prev_ID_AssetType = Convert.ToInt32(ds_asset.Tables[0].Rows[0]["ID_AssetType"]);
            }
            else if (ID_Order != 0)
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_Order;
                DataSet ds_order = app.DB.Select("select_Orders", "Orders", vl_params);
                if (app.DB.ValidDSRows(ds_order)) prev_ID_Asset = Convert.ToInt32(ds_order.Tables[0].Rows[0]["ID_Asset"]);
            }
            else if (ID_AssetSchedule != 0)
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_AssetSchedule;
                DataSet ds_schedule = app.DB.Select("select_AssetSchedules", "AssetSchedules", vl_params);
                if (app.DB.ValidDSRows(ds_schedule)) prev_ID_Order = Convert.ToInt32(ds_schedule.Tables[0].Rows[0]["ID_InitialOrder"]);
            }

            vl_params = new TVariantList();
            vl_params.Add("@prm_ID_AssetType").AsInt32 = prev_ID_AssetType;
            vl_params.Add("@prm_ID_Asset").AsInt32 = prev_ID_Asset;
            vl_params.Add("@prm_ID_Order").AsInt32 = prev_ID_Order;
            vl_params.Add("@prm_ID_AssetSchedule").AsInt32 = prev_ID_AssetSchedule;
            DataSet ds_prev = app.DB.Select("select_TradeParametersbyIDs", "TradeParameters", vl_params);
            if (!app.DB.ValidDS(ds_prev)) return;

            vl_params = new TVariantList();
            vl_params.Add("@prm_ID_AssetType").AsInt32 = ID_AssetType;
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            vl_params.Add("@prm_ID_Order").AsInt32 = ID_Order;
            vl_params.Add("@prm_ID_AssetSchedule").AsInt32 = ID_AssetSchedule;
            DataSet ds = app.DB.Select("select_TradeParametersbyIDs", "TradeParameters", vl_params);
            if (!app.DB.ValidDS(ds)) return;

            if (app.DB.ValidDSRows(ds_prev) && !app.DB.ValidDSRows(ds))
            {
                trd.PartialFlagChangeAllowed = Convert.ToBoolean(ds_prev.Tables[0].Rows[0]["PartialFlagChangeAllowed"]);
                trd.InitialPriceMandatory = Convert.ToBoolean(ds_prev.Tables[0].Rows[0]["InitialPriceMandatory"]);
                trd.InitialPriceMaintenance = Convert.ToBoolean(ds_prev.Tables[0].Rows[0]["InitialPriceMaintenance"]);
                trd.DiminishedQuantityAllowed = Convert.ToBoolean(ds_prev.Tables[0].Rows[0]["DiminishedQuantityAllowed"]);
                trd.DiminishedPriceAllowed = Convert.ToBoolean(ds_prev.Tables[0].Rows[0]["DiminishedPriceAllowed"]);
                trd.DifferentialPriceText = ds_prev.Tables[0].Rows[0]["DifferentialPriceText"].ToString();
                trd.DifferentialPriceAllowed = Convert.ToBoolean(ds_prev.Tables[0].Rows[0]["DifferentialPriceAllowed"]);
                trd.OppositeDirectionAllowed = Convert.ToBoolean(ds_prev.Tables[0].Rows[0]["OppositeDirectionAllowed"]);
                trd.DeltaT = Convert.ToInt32(ds_prev.Tables[0].Rows[0]["DeltaT"]);
                trd.DeltaT1 = Convert.ToInt32(ds_prev.Tables[0].Rows[0]["DeltaT1"]);
                trd.SellWarrantyPercent = Convert.ToDouble(ds_prev.Tables[0].Rows[0]["SellWarrantyPercent"]);
                trd.SellWarrantyMU = Convert.ToDouble(ds_prev.Tables[0].Rows[0]["SellWarrantyMU"]);
                trd.SellWarrantyFixed = Convert.ToDouble(ds_prev.Tables[0].Rows[0]["SellWarrantyFixed"]);
                trd.BuyWarrantyPercent = Convert.ToDouble(ds_prev.Tables[0].Rows[0]["BuyWarrantyPercent"]);
                trd.BuyWarrantyMU = Convert.ToDouble(ds_prev.Tables[0].Rows[0]["BuyWarrantyMU"]);
                trd.BuyWarrantyFixed = Convert.ToDouble(ds_prev.Tables[0].Rows[0]["BuyWarrantyFixed"]);
            }
            else if (app.DB.ValidDSRows(ds))
            {
                trd.PartialFlagChangeAllowed = Convert.ToBoolean(ds.Tables[0].Rows[0]["PartialFlagChangeAllowed"]);
                trd.InitialPriceMandatory = Convert.ToBoolean(ds.Tables[0].Rows[0]["InitialPriceMandatory"]);
                trd.InitialPriceMaintenance = Convert.ToBoolean(ds.Tables[0].Rows[0]["InitialPriceMaintenance"]);
                trd.DiminishedQuantityAllowed = Convert.ToBoolean(ds.Tables[0].Rows[0]["DiminishedQuantityAllowed"]);
                trd.DiminishedPriceAllowed = Convert.ToBoolean(ds.Tables[0].Rows[0]["DiminishedPriceAllowed"]);
                trd.DifferentialPriceAllowed = Convert.ToBoolean(ds.Tables[0].Rows[0]["DifferentialPriceAllowed"]);
                trd.DifferentialPriceText = ds.Tables[0].Rows[0]["DifferentialPriceText"].ToString();
                trd.OppositeDirectionAllowed = Convert.ToBoolean(ds.Tables[0].Rows[0]["OppositeDirectionAllowed"]);
                trd.DeltaT = Convert.ToInt32(ds.Tables[0].Rows[0]["DeltaT"]);
                trd.DeltaT1 = Convert.ToInt32(ds.Tables[0].Rows[0]["DeltaT1"]);
                trd.SellWarrantyPercent = Convert.ToDouble(ds.Tables[0].Rows[0]["SellWarrantyPercent"]);
                trd.SellWarrantyMU = Convert.ToDouble(ds.Tables[0].Rows[0]["SellWarrantyMU"]);
                trd.SellWarrantyFixed = Convert.ToDouble(ds.Tables[0].Rows[0]["SellWarrantyFixed"]);
                trd.BuyWarrantyPercent = Convert.ToDouble(ds.Tables[0].Rows[0]["BuyWarrantyPercent"]);
                trd.BuyWarrantyMU = Convert.ToDouble(ds.Tables[0].Rows[0]["BuyWarrantyMU"]);
                trd.BuyWarrantyFixed = Convert.ToDouble(ds.Tables[0].Rows[0]["BuyWarrantyFixed"]);
            }

            if (vl_arguments["PartialFlagChangeAllowed"] != null) trd.PartialFlagChangeAllowed = vl_arguments["PartialFlagChangeAllowed"].AsBoolean;
            if (vl_arguments["InitialPriceMandatory"] != null) trd.InitialPriceMandatory = vl_arguments["InitialPriceMandatory"].AsBoolean;
            if (vl_arguments["InitialPriceMaintenance"] != null) trd.InitialPriceMaintenance = vl_arguments["InitialPriceMaintenance"].AsBoolean;
            if (vl_arguments["DiminishedQuantityAllowed"] != null) trd.DiminishedQuantityAllowed = vl_arguments["DiminishedQuantityAllowed"].AsBoolean;
            if (vl_arguments["DiminishedPriceAllowed"] != null) trd.DiminishedPriceAllowed = vl_arguments["DiminishedPriceAllowed"].AsBoolean;
            if (vl_arguments["DifferentialPriceAllowed"] != null) trd.DifferentialPriceAllowed = vl_arguments["DifferentialPriceAllowed"].AsBoolean;
            if (vl_arguments["DifferentialPriceText"] != null) trd.DifferentialPriceText = vl_arguments["DifferentialPriceText"].AsString;
            if (vl_arguments["OppositeDirectionAllowed"] != null) trd.OppositeDirectionAllowed = vl_arguments["OppositeDirectionAllowed"].AsBoolean;
            if (vl_arguments["DeltaT"] != null) trd.DeltaT = vl_arguments["DeltaT"].AsInt32;
            if (vl_arguments["DeltaT1"] != null) trd.DeltaT1 = vl_arguments["DeltaT1"].AsInt32;
            if (vl_arguments["SellWarrantyPercent"] != null) trd.SellWarrantyPercent = vl_arguments["SellWarrantyPercent"].AsDouble;
            if (vl_arguments["SellWarrantyMU"] != null) trd.SellWarrantyMU = vl_arguments["SellWarrantyMU"].AsDouble;
            if (vl_arguments["SellWarrantyFixed"] != null) trd.SellWarrantyFixed = vl_arguments["SellWarrantyFixed"].AsDouble;
            if (vl_arguments["BuyWarrantyPercent"] != null) trd.BuyWarrantyPercent = vl_arguments["BuyWarrantyPercent"].AsDouble;
            if (vl_arguments["BuyWarrantyMU"] != null) trd.BuyWarrantyMU = vl_arguments["BuyWarrantyMU"].AsDouble;
            if (vl_arguments["BuyWarrantyFixed"] != null) trd.BuyWarrantyFixed = vl_arguments["BuyWarrantyFixed"].AsDouble;

            vl_params.Add("@prm_PartialFlagChangeAllowed").AsBoolean = trd.PartialFlagChangeAllowed;
            vl_params.Add("@prm_InitialPriceMandatory").AsBoolean = trd.InitialPriceMandatory;
            vl_params.Add("@prm_InitialPriceMaintenance").AsBoolean = trd.InitialPriceMaintenance;
            vl_params.Add("@prm_DiminishedQuantityAllowed").AsBoolean = trd.DiminishedQuantityAllowed;
            vl_params.Add("@prm_DiminishedPriceAllowed").AsBoolean = trd.DiminishedPriceAllowed;
            vl_params.Add("@prm_DifferentialPriceAllowed").AsBoolean = trd.DifferentialPriceAllowed;
            vl_params.Add("@prm_DifferentialPriceText").AsString = trd.DifferentialPriceText;
            vl_params.Add("@prm_OppositeDirectionAllowed").AsBoolean = trd.OppositeDirectionAllowed;
            vl_params.Add("@prm_DeltaT").AsInt32 = trd.DeltaT;
            vl_params.Add("@prm_DeltaT1").AsInt32 = trd.DeltaT1;
            vl_params.Add("@prm_SellWarrantyPercent").AsDouble = trd.SellWarrantyPercent;
            vl_params.Add("@prm_SellWarrantyMU").AsDouble = trd.SellWarrantyMU;
            vl_params.Add("@prm_SellWarrantyFixed").AsDouble = trd.SellWarrantyFixed;
            vl_params.Add("@prm_BuyWarrantyPercent").AsDouble = trd.BuyWarrantyPercent;
            vl_params.Add("@prm_BuyWarrantyMU").AsDouble = trd.BuyWarrantyMU;
            vl_params.Add("@prm_BuyWarrantyFixed").AsDouble = trd.BuyWarrantyFixed;

            if (ds.Tables[0].Rows.Count == 0)
            {
                app.DB.Exec("insert_TradeParameters", "TradeParameters", vl_params);
            }
            else
            {
                vl_params.Add("@prm_ID").AsInt32 = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                app.DB.Exec("update_TradeParameters", "TradeParameters", vl_params);
            }
        }

        public int DecimalsCount(double v)
        {
            double vv = v - Math.Floor(v);
            if (vv == 0) return 0;
            else return DecimalsCount(vv * 10) + 1;
        }

        public string validateTradeParameters(int ID_Order, int ID_Asset, int ID_AssetSession, int ID_Broker, int ID_Client, bool Direction, double Quantity, double Price, bool PartialFlag, DateTime SubmitTime, ref bool isDoubleTrouble, ref bool isSameAsInitial)
        {
            TTradeParameters trd = getTradeParameters(ID_AssetSession);

            TVariantList vl_params;

            DataSet dsAssetSession = getAssetSession(ID_Asset);
            string SessionStatus = dsAssetSession.Tables[0].Rows[0]["Status"].ToString();

            isDoubleTrouble = false;
            isSameAsInitial = false;
            vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
            DataSet ds_NexOrderType = app.DB.Select("select_NexOrderType", "Orders", vl_params);
            if (app.DB.ValidDSRows(ds_NexOrderType))
            {
                isDoubleTrouble = Convert.ToBoolean(ds_NexOrderType.Tables[0].Rows[0]["isDoubleTrouble"]);
                isSameAsInitial = Convert.ToBoolean(ds_NexOrderType.Tables[0].Rows[0]["isSameAsInitial"]);
            }

            if (ID_Order == 0 && !isDoubleTrouble && !isSameAsInitial) //  add order for simple competitive session
            {
                if (trd.PreOpeningTime == trd.OpeningTime && SessionStatus != "Opened") return "Ordinele noi pot fi introduse doar cand sedinta este deschisa";
                else if (trd.PreOpeningTime != trd.OpeningTime && SessionStatus != "PreOpened") return "Ordinele noi pot fi introduse numai in deschiderea sedintei";
            }

            if (ID_Order == 0 && isDoubleTrouble && !isSameAsInitial)
            {
                if (SessionStatus != "PreOpened" && SessionStatus != "Opened") return "Ordinele noi pot fi introduse doar cand sedinta este deschisa";
            }

            if (ID_Order != 0 && !isSameAsInitial) //  modify order when not initiator
            {
                if (SessionStatus != "PreOpened" && SessionStatus != "Opened") return "Ordinele pot fi modificate doar in faza de deschidere sau tranzactionare";
            }

            bool isInitial = false;
            DataSet dsOrder = null;
            DateTime dt_MaxDateOnBroker = trd.OpeningTime;
            if (ID_Order != 0)
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_Order;
                dsOrder = app.DB.Select("select_Orders", "Orders", vl_params);
                if (!app.DB.ValidDSRows(dsOrder)) return "Database failure";

                DataSet dsMaxDateOnBroker = app.DB.Select("select_MaxDateOnBroker", "Orders", vl_params);
                if (!app.DB.ValidDSRows(dsMaxDateOnBroker)) return "Database failure";
                dt_MaxDateOnBroker = Convert.ToDateTime(dsMaxDateOnBroker.Tables[0].Rows[0]["MaxDateOnBroker"]);

                //  make checks for initial orders
                isInitial = Convert.ToBoolean(dsOrder.Tables[0].Rows[0]["isInitial"]);
                if (isInitial && !trd.InitialPriceMaintenance &&
                    (SessionStatus == "PreOpened" || SessionStatus == "Opened") && Price != Convert.ToDouble(dsOrder.Tables[0].Rows[0]["Price"]))
                    return "Ordonatorul nu are dreptul de intretinere pret in faza 1 si faza 2";
            }

            //  check if the Client has rights on this side
            if (!isInitial)
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                DataSet dsAssetRights = app.DB.Select("select_AssetsXClientsbyID_Client", "AssetsXClients", vl_params);
                if (!app.DB.ValidDSRows(dsAssetRights)) return "Clientul nu are permisiuni pe acest activ";

                switch (Direction)
                {
                    case false:
                        if (!Convert.ToBoolean(dsAssetRights.Tables[0].Rows[0]["canBuy"])) return "Clientul nu are permisiune pe ordine de cumparare";
                        break;
                    case true:
                        if (!Convert.ToBoolean(dsAssetRights.Tables[0].Rows[0]["canSell"])) return "Clientul nu are permisiune pe ordine de vanzare";
                        break;
                }
            }

            //  check for deltaT
            if (ID_Order != 0 && !isDoubleTrouble && !isInitial && !isSameAsInitial)
            {
                DateTime dt_OrderDate = Convert.ToDateTime(dsOrder.Tables[0].Rows[0]["Date"]);
                if (trd.OpeningTime > dt_OrderDate) dt_OrderDate = trd.OpeningTime;
                if (dt_MaxDateOnBroker > dt_OrderDate) dt_OrderDate = dt_MaxDateOnBroker;
                dt_OrderDate = dt_OrderDate.AddMinutes(trd.DeltaT);
                if (!isInitial && SessionStatus == "Opened" && dt_OrderDate < SubmitTime) return "Ordinul nu mai poate fi modificat dupa intervalul Delta T";
            }


            //  check if change of partial flag is allowed
            if (!isInitial && !trd.PartialFlagChangeAllowed && ID_Order != 0)
            {
                bool old_PartialFlag = Convert.ToBoolean(dsOrder.Tables[0].Rows[0]["PartialFlag"]);
                if (old_PartialFlag != PartialFlag) return "Nu este permisa modificarea campului de tranzactionare partiala";
            }

            //  check if diminished quantity is allowed
            if (!isInitial && !trd.DiminishedQuantityAllowed && ID_Order != 0)
            {
                double old_Quantity = Convert.ToDouble(dsOrder.Tables[0].Rows[0]["Quantity"]);
                if (Quantity < old_Quantity) return "Nu este permisa inrautatirea cantitatii in oferta";
            }

            //  check if diminished price is allowed
            if (!isInitial && !trd.DiminishedPriceAllowed && ID_Order != 0 && dsOrder.Tables[0].Rows[0]["Price"] != DBNull.Value)
            {                
                double old_Price = Convert.ToDouble(dsOrder.Tables[0].Rows[0]["Price"]);
                switch (Direction)
                {
                    case false:
                        if (Price < old_Price) return "Nu este permisa inrautatirea ofertei de pret";
                        break;
                    case true:
                        if (Price > old_Price) return "Nu este permisa inrautatirea ofertei de pret";
                        break;
                }
            }

            if (!trd.DifferentialPriceAllowed && Price < 0) return "Nu se permit preturi negative";
            if (!trd.DifferentialPriceAllowed && Price == 0 && !isInitial && !isSameAsInitial) return "Nu se permit preturi nule sau negative";

            //  validate quantity and price values
            double d;
            if (trd.QuantityStepping != 0)
            {
                d = (Quantity * 100000) / (trd.QuantityStepping * 100000);
                d = Math.Abs(d - (int)d);
                if (d > 0) return "Cantitatea nu respecta pasul de incrementare";
            }

            if (trd.MinQuantity > 0 && Quantity < trd.MinQuantity) return "Cantitatea minima nu este satisfacuta";

            int nrOfFloatingPoints = 0;

            if (trd.PriceStepping != 0)
            {
                nrOfFloatingPoints = DecimalsCount(trd.PriceStepping);

                d = (Price * 100000) / (trd.PriceStepping * 100000);
                d = Math.Round(Math.Abs(d - (int)d), 2);
                if (d % 1 != 0) return "Pretul nu respecta pasul de incrementare";
            }

            if (trd.MinPrice != trd.MaxPrice && (trd.MinPrice != 0 || trd.MaxPrice != 0))
            {

                if (Price < trd.MinPrice) return "Pretul oferit este mai mic decat pretul minim acceptat de " + trd.MinPrice.ToString("F" + nrOfFloatingPoints);
                if (Price > trd.MaxPrice) return "Pretul oferit depaseste pretul maxim acceptat de " + trd.MaxPrice.ToString("F" + nrOfFloatingPoints);
            }
            else if (trd.MaxPriceVariation != 0)
            {
                if (trd.Quotation != 0)
                {
                    double var_Price = (Price - trd.SpotQuotation) / trd.Quotation * 100;
                    if (Math.Abs(var_Price) > trd.MaxPriceVariation) return "Pretul iese din variatia maxima acceptata de " + trd.MaxPriceVariation.ToString("F2");
                }
                else if (trd.SpotQuotation != 0)
                {

                    double var_Price = (Price - trd.SpotQuotation) / trd.SpotQuotation * 100;
                    if (Math.Abs(var_Price) > trd.MaxPriceVariation) return "Pretul iese din variatia maxima acceptata de " + trd.MaxPriceVariation.ToString("F2");
                }
            }
            
            return "";
        }

        public int blockWarranty(int ID_Order, int ID_Client, int ID_Asset, double WarrantyValue)
        {
            if (!app.DB.BeginTransaction()) return 0;

            try
            {
                int ID_Operation = 0;

                //  try to update an existing lock
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_ID_Order").AsInt32 = ID_Order;
                vl_params.Add("@prm_Amount").AsDouble = WarrantyValue;
                int int_rows_updated = app.DB.Exec("update_BO_OperationDetails_Asset", "BO_OperationDetails_Assets", vl_params);
                if (int_rows_updated < 0) throw new Exception("SQL execution error");

                if (int_rows_updated == 0)
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_OperationType").AsString = "LOCK_WAR";
                    vl_params.Add("@prm_isClosed").AsBoolean = true;
                    vl_params.Add("@prm_isCanceled").AsBoolean = false;
                    vl_params.Add("@prm_Reference").AsString = "";

                    int int_rows_operation = app.DB.Exec("insert_BO_Operation", "BO_Operations", vl_params);
                    if (int_rows_operation <= 0) throw new Exception("SQL execution error");

                    ID_Operation = app.DB.GetIdentity();

                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                    vl_params.Add("@prm_ID_Order").AsInt32 = ID_Order;
                    vl_params.Add("@prm_Amount").AsDouble = WarrantyValue;
                    /*vl_params.Add("@prm_ID_PaymentType").AsInt32 = 0;
                    vl_params.Add("@prm_PaymentDescription").AsString = "";
                    vl_params.Add("@prm_Sum").AsDouble = WarrantyValue;
                    vl_params.Add("@prm_Percent").AsDouble = 1;
                    vl_params.Add("@prm_isPayed").AsBoolean = true;
                    vl_params.Add("@prm_PaymentDate").AsDateTime = DateTime.Now;
                    vl_params.Add("@prm_ID_Currency").AsInt32 = 1;
                    vl_params.Add("@prm_ValabilityStartDate").AsDateTime = DateTime.Now;
                    vl_params.Add("@prm_ValabilityEndDate").AsDateTime = DateTime.Now;
                    vl_params.Add("@prm_ExecutionDate").AsDateTime = DateTime.Now;
                    vl_params.Add("@prm_ExchangeRate").AsDouble = 1;*/

                    int int_rows_payments = app.DB.Exec("insert_BO_OperationDetails_Asset", "BO_OperationDetails_Assets", vl_params);
                    if (int_rows_payments <= 0) throw new Exception("SQL execution error");

                    int ID_OperationDetail = app.DB.GetIdentity();

                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_Operation").AsInt32 = ID_Operation;
                    vl_params.Add("@prm_ID_Parent").AsInt32 = 0;
                    vl_params.Add("@prm_ID_InterNode").AsInt32 = 0;
                    vl_params.Add("@prm_Date").AsDateTime = DateTime.Now;
                    vl_params.Add("@prm_ObjectType").AsInt32 = 1; //Retailer logic. 1-TDA, 2-TDP
                    vl_params.Add("@prm_Description").AsString = "Asset";
                    vl_params.Add("@prm_ID_OperationDetail").AsInt32 = ID_OperationDetail;
                    vl_params.Add("@prm_ID_ClientSRC").AsInt32 = ID_Client;
                    vl_params.Add("@prm_ID_ClientDEST").AsInt32 = 0;
                    vl_params.Add("@prm_ID_AgencySRC").AsInt32 = 0;
                    vl_params.Add("@prm_ID_AgencyDEST").AsInt32 = 0;
                    vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                    vl_params.Add("@prm_ID_CreatedByUser").AsInt32 = session.ID_User;
                    vl_params.Add("@prm_isCanceled").AsInt32 = 0;

                    //int int_rows = app.DB.Exec("insert_Warranty", "Warranties", vl_params);  

                    int int_rows_details = app.DB.Exec("insert_BO_OperationXDetails", "BO_OperationsXDetails", vl_params);
                    if (int_rows_details <= 0) throw new Exception("SQL execution error");

                    int int_rows = int_rows_operation + int_rows_payments + int_rows_details;
                }

                app.DB.CommitTransaction();

                return ID_Operation;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();

                return 0;
            }
        }

        public TDBExecResult unblockWarranty(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Order"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Order");
            if (vl_arguments["ID_Client"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Client");
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");
            if (vl_arguments["Amount"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Amount");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Order = 0;
            if (vl_arguments["ID_Order"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Order"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Order"].ValueType == TVariantType.vtInt64) ID_Order = vl_arguments["ID_Order"].AsInt32;
            else res.ParamValidations["ID_Order"].AsString = "Value not recognized";

            int ID_Client = 0;
            if (vl_arguments["ID_Client"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Client"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Client"].ValueType == TVariantType.vtInt64) ID_Client = vl_arguments["ID_Client"].AsInt32;
            else res.ParamValidations["ID_Client"].AsString = "Value not recognized";

            int ID_Asset = 0;
            if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
            else res.ParamValidations["ID_Asset"].AsString = "Value not recognized";

            double Amount = 0;
            if (vl_arguments["Amount"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Amount"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Amount"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["Amount"].ValueType == TVariantType.vtSingle ||
                vl_arguments["Amount"].ValueType == TVariantType.vtDouble) Amount = vl_arguments["Amount"].AsDouble;
            else res.ParamValidations["Amount"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "Cannot open transaction");

            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_OperationType").AsString = "UNLOCK_WAR";
                vl_params.Add("@prm_isClosed").AsBoolean = true;
                vl_params.Add("@prm_isCanceled").AsBoolean = false;
                vl_params.Add("@prm_Reference").AsString = "";

                int int_rows_operation = app.DB.Exec("insert_BO_Operation", "BO_Operations", vl_params);
                if (int_rows_operation <= 0) throw new Exception("SQL execution error");

                int ID_Operation = app.DB.GetIdentity();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_ID_Order").AsInt32 = ID_Order;
                vl_params.Add("@prm_Amount").AsDouble = Amount;

                int int_rows_payments = app.DB.Exec("insert_BO_OperationDetails_Asset", "BO_OperationDetails_Assets", vl_params);
                if (int_rows_payments <= 0) throw new Exception("SQL execution error");

                int ID_OperationDetail = app.DB.GetIdentity();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Operation").AsInt32 = ID_Operation;
                vl_params.Add("@prm_ID_Parent").AsInt32 = 0;
                vl_params.Add("@prm_ID_InterNode").AsInt32 = 0;
                vl_params.Add("@prm_Date").AsDateTime = DateTime.Now;
                vl_params.Add("@prm_ObjectType").AsInt32 = 1; //Retailer logic. 1-TDA, 2-TDP
                vl_params.Add("@prm_Description").AsString = "Asset";
                vl_params.Add("@prm_ID_OperationDetail").AsInt32 = ID_OperationDetail;
                vl_params.Add("@prm_ID_ClientSRC").AsInt32 = ID_Client;
                vl_params.Add("@prm_ID_ClientDEST").AsInt32 = 0;
                vl_params.Add("@prm_ID_AgencySRC").AsInt32 = 0;
                vl_params.Add("@prm_ID_AgencyDEST").AsInt32 = 0;
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_ID_CreatedByUser").AsInt32 = session.ID_User;
                vl_params.Add("@prm_isCanceled").AsInt32 = 0;

                int int_rows_details = app.DB.Exec("insert_BO_OperationXDetails", "BO_OperationsXDetails", vl_params);
                if (int_rows_details <= 0) throw new Exception("SQL execution error");

                int int_rows = int_rows_operation + int_rows_payments + int_rows_details;

                app.DB.CommitTransaction();

                res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs.Add("ID_Operation").AsInt32 = ID_Operation;
                res.RowsModified = int_rows;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();

                return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            }
        }

        public string validateWarranty(int ID_Order, int ID_Asset, int ID_AssetSession, int ID_Broker, int ID_Client, bool Direction, double Quantity, double Price, bool PartialFlag, DateTime SubmitTime, bool lockWarranty = false)
        {
            TTradeParameters trd = getTradeParameters(ID_AssetSession);

            double flt_Warranty = 0;
            switch (Direction)
            {
                case false:
                    flt_Warranty += trd.BuyWarrantyFixed;
                    flt_Warranty += Quantity * trd.BuyWarrantyMU;
                    flt_Warranty += trd.BuyWarrantyPercent * (Quantity * Price) / 100;
                    break;
                case true:
                    flt_Warranty += trd.SellWarrantyFixed;
                    flt_Warranty += Quantity * trd.SellWarrantyMU;
                    flt_Warranty += trd.SellWarrantyPercent * (Quantity * Price) / 100;
                    break;
            }

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            vl_params.Add("@prm_ID_Order").AsInt32 = ID_Order;
            DataSet ds = app.DB.Select("select_AvailableWarranty", "BO_Operations", vl_params);
            if (!app.DB.ValidDS(ds)) return "SQL Execution Error";

            if (!app.DB.ValidDSRows(ds) && flt_Warranty > 0) return "Nu exista suficiente garantii depuse pentru a valida ordinul";
            double flt_Credit = 0;
            double flt_Debit = 0;
            if (app.DB.ValidDSRows(ds))
            {
                flt_Credit = Convert.ToDouble(ds.Tables[0].Rows[0]["Credit"]);
                flt_Debit = Convert.ToDouble(ds.Tables[0].Rows[0]["Debit"]);
            }

            if (flt_Credit - flt_Debit < flt_Warranty) return "Nu exista suficiente garantii depuse pentru a valida ordinul";

            if (lockWarranty && ID_Order != 0)
            {
                blockWarranty(ID_Order, ID_Client, ID_Asset, flt_Warranty);
            }

            return "";
        }

        public TDBExecResult addOrder_DISPONIBIL(TVariantList vl_arguments)
        {
            //-----------------------------------------------------------------------
            //  check if arguments are provided
            //  generic
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Ring");
            if (vl_arguments["Direction"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Direction");
            if (vl_arguments["Quantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Quantity");
            if (vl_arguments["Price"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Price");
            if (vl_arguments["isInitial"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isInitial");

            //-----------------------------------------------------------------------

            //-----------------------------------------------------------------------
            //  extract values and validate
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            bool isInitial = false;
            if (vl_arguments["isInitial"].ValueType == TVariantType.vtBoolean) isInitial = vl_arguments["isInitial"].AsBoolean;
            else res.ParamValidations["isInitial"].AsString = "Unrecognized Value";

            int ID_Ring = 0;
            if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
            else res.ParamValidations["ID_Ring"].AsString = "Unrecognized Value";
            if (ID_Ring <= 0 && res.ParamValidations["ID_Ring"].AsString == "") res.ParamValidations["ID_Ring"].AsString = "Invalid Ring";

            int ID_Asset = 0;
            if (vl_arguments["ID_Asset"] != null)
            {
                if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
                else res.ParamValidations["ID_Asset"].AsString = "Unrecognized Value";
            }

            //added on 20140825 by Alex P
            if (!isInitial)
            {
                if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");
                if (ID_Asset <= 0 && res.ParamValidations["ID_Asset"].AsString == "") res.ParamValidations["ID_Asset"].AsString = "Invalid Asset";
            }

            int ID_Agency = 0;
            int ID_Broker = 0;
            int ID_Client = 0;

            UserValidation uv = new UserValidation(app);
            if (uv.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole))
            {
                if (vl_arguments["ID_Agency"] != null) ID_Agency = vl_arguments["ID_Agency"].AsInt32;
                if (vl_arguments["ID_Broker"] != null) ID_Broker = vl_arguments["ID_Broker"].AsInt32;
            }
            else ID_Broker = session.ID_Broker;

            if (vl_arguments["ID_Client"] != null) ID_Client = vl_arguments["ID_Client"].AsInt32;

            TVariantList vl_params;
            if (ID_Broker != 0)
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                DataSet ds_client = app.DB.Select("select_HousebyID_Broker", "Clients", vl_params);
                if (app.DB.ValidDSRows(ds_client))
                {
                    ID_Agency = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID_Agency"]);
                    if (ID_Client == 0) ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
                }
            }

            if (ID_Agency == 0 && (ID_Broker == 0 || ID_Client == 0)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Current user is not attached to a broker");

            //  get current session for asset
            int ID_RingSession = 0;
            int ID_AssetSession = 0;

            if (!isInitial)
            {
                DataSet ds_ringsession = getRingSession(ID_Ring);
                DataSet ds_assetsession = getAssetSession(ID_Asset);

                if (!(app.DB.ValidDS(ds_ringsession) && app.DB.ValidDS(ds_assetsession))) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

                if (!app.DB.ValidDSRows(ds_ringsession)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
                if (Convert.ToDateTime(ds_ringsession.Tables[0].Rows[0]["Date"]).Date != DateTime.Today) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
                //if (ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "Opened" && ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "PreOpened") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
                ID_RingSession = Convert.ToInt32(ds_ringsession.Tables[0].Rows[0]["ID"]);

                if (!app.DB.ValidDSRows(ds_assetsession)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
                if (Convert.ToDateTime(ds_assetsession.Tables[0].Rows[0]["Date"]).Date != DateTime.Today) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
                if (ds_assetsession.Tables[0].Rows[0]["Status"].ToString() != "Opened" &&
                    ds_assetsession.Tables[0].Rows[0]["Status"].ToString() != "PreOpened" &&
                    ds_assetsession.Tables[0].Rows[0]["Status"].ToString() != "PreClosed") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Sedinta nu se afla intr-o faza de admitere noi ordine");
                ID_AssetSession = Convert.ToInt32(ds_assetsession.Tables[0].Rows[0]["ID"]);
            }

            bool Direction = false;
            switch (vl_arguments["Direction"].AsString.Trim().ToUpper())
            {
                case "B":
                    Direction = false;
                    break;
                case "S":
                    Direction = true;
                    break;
                default:
                    res.ParamValidations["Direction"].AsString = "Expected value 'B' or 'S'. Actual value: " + vl_arguments["Direction"].AsString;
                    break;
                //return new TDBExecResult() { ErrorCode = TDBExecError.ValidationUnsuccesful, Message = "Argument value differs from expected.", ParamValidations = new TVariantList().Add("Direction").AsString = "Expected values B or S" };
            }

            double Quantity = 0;
            if (vl_arguments["Quantity"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtSingle ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtDouble) Quantity = vl_arguments["Quantity"].AsDouble;
            else res.ParamValidations["Quantity"].AsString = "Unrecognized Value";
            if (Quantity <= 0 && res.ParamValidations["Quantity"].AsString == "") res.ParamValidations["Quantity"].AsString = "Value cannot be negative";

            double Price = double.NaN;
            if (isInitial && vl_arguments["Price"].ValueType == TVariantType.vtString && vl_arguments["Price"].AsString == "none") ;
            else
            {
                if (vl_arguments["Price"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["Price"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["Price"].ValueType == TVariantType.vtInt64 ||
                    vl_arguments["Price"].ValueType == TVariantType.vtSingle ||
                    vl_arguments["Price"].ValueType == TVariantType.vtDouble) Price = vl_arguments["Price"].AsDouble;
                else res.ParamValidations["Price"].AsString = "Unrecognized Value";
            }
            //if (!isInitial && Price <= 0 && res.ParamValidations["Price"].AsString == "") res.ParamValidations["Price"].AsString = " Value cannot be negative";

            bool PartialFlag = false;
            if (vl_arguments["PartialFlag"] != null)
            {
                if (vl_arguments["PartialFlag"].ValueType == TVariantType.vtBoolean) PartialFlag = vl_arguments["PartialFlag"].AsBoolean;
            }

            if (vl_arguments["isPartial"] != null)
            {
                if (vl_arguments["isPartial"].ValueType == TVariantType.vtBoolean) PartialFlag = vl_arguments["isPartial"].AsBoolean;
            }

            DateTime ExpirationDate = DateTime.Now;
            if (vl_arguments["ExpirationDate"] != null)
                if (vl_arguments["ExpirationDate"].ValueType == TVariantType.vtDateTime)
                    ExpirationDate = vl_arguments["ExpirationDate"].AsDateTime;

            DateTime SubmitTime = DateTime.Now;
            if (vl_arguments["SubmitTime"] != null)
                if (vl_arguments["SubmitTime"].ValueType == TVariantType.vtDateTime)
                    SubmitTime = vl_arguments["SubmitTime"].AsDateTime;

            bool isDoubleTrouble = false;
            bool isSameAsInitial = false;
            if (ID_AssetSession != 0)
            {
                string s = validateTradeParameters(0, ID_Asset, ID_AssetSession, ID_Broker, ID_Client, Direction, Quantity, Price, PartialFlag, SubmitTime, ref isDoubleTrouble, ref isSameAsInitial);
                if (s != "") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, s);

                s = validateWarranty(0, ID_Asset, ID_AssetSession, ID_Broker, ID_Client, Direction, Quantity, Price, PartialFlag, SubmitTime);
                if (s != "") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, s);
            }

            //if order IsInitial, we expect the params for asset creation
            //int ID_Market = 0;
            int ID_AssetType = 0;
            string str_AssetCode = "";
            string str_AssetName = "";
            string str_AssetMeasuringUnit = "";
            bool bit_isDefault = false;

            if (isInitial && ID_Asset <= 0)
            {
                //if (vl_arguments["ID_Market"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Market");
                if (vl_arguments["ID_AssetType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AssetType");
                if (vl_arguments["AssetCode"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
                if (vl_arguments["AssetName"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
                if (vl_arguments["AssetMeasuringUnit"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MeasuringUnit");

                /*ID_Market = 0;
                if (vl_arguments["ID_Market"].ValueType == TVariantType.vtByte ||
                    vl_arguments["ID_Market"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Market"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Market"].ValueType == TVariantType.vtInt64) ID_Market = vl_arguments["ID_Market"].AsInt32;
                else res.ParamValidations["ID_Market"].AsString = "Value type not recognized";*/

                ID_AssetType = 0;
                if (vl_arguments["ID_AssetType"].ValueType == TVariantType.vtByte ||
                    vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt64) ID_AssetType = vl_arguments["ID_AssetType"].AsInt32;
                else res.ParamValidations["ID_AssetType"].AsString = "Value type not recognized";

                str_AssetCode = "";
                if (vl_arguments["AssetCode"].ValueType == TVariantType.vtString) str_AssetCode = vl_arguments["AssetCode"].AsString;
                else res.ParamValidations["AssetCode"].AsString = "Value type not recognized";

                str_AssetName = "";
                if (vl_arguments["AssetName"].ValueType == TVariantType.vtString) str_AssetName = vl_arguments["AssetName"].AsString;
                else res.ParamValidations["AssetName"].AsString = "Value type not recognized";

                str_AssetMeasuringUnit = "";
                if (vl_arguments["AssetMeasuringUnit"].ValueType == TVariantType.vtString) str_AssetMeasuringUnit = vl_arguments["AssetMeasuringUnit"].AsString;
                else res.ParamValidations["AssetMeasuringUnit"].AsString = "Value type not recognized";

                bit_isDefault = false;
                if (vl_arguments["isDefault"] != null)
                {
                    if (vl_arguments["isDefault"].ValueType == TVariantType.vtBoolean) bit_isDefault = vl_arguments["isDefault"].AsBoolean;
                    else res.ParamValidations["isDefault"].AsString = "Value type not recognized";
                }
            }
            //--------------

            //  specific arguments



            //-----------------------------
            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            //////////////////////////
            //1. first step, if IsInitial, create Asset and get the new asset ID
            if (isInitial && ID_Asset <= 0)
            {
                try
                {
                    res = new TDBExecResult(TDBExecError.Success, "");

                    vl_params = new TVariantList();
                    //vl_params.Add("@prm_ID_Market").AsInt32 = ID_Market;
                    vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                    vl_params.Add("@prm_ID_AssetType").AsInt32 = ID_AssetType;
                    vl_params.Add("@prm_Code").AsString = str_AssetCode;
                    vl_params.Add("@prm_Name").AsString = str_AssetName;
                    vl_params.Add("@prm_MeasuringUnit").AsString = str_AssetMeasuringUnit;
                    vl_params.Add("@prm_ID_Terminal").AsInt32 = 0;
                    vl_params.Add("@prm_isDefault").AsBoolean = bit_isDefault;
                    vl_params.Add("@prm_isActive").AsBoolean = false;

                    int int_count_Asset = app.DB.Exec("insert_Asset", "Assets", vl_params);
                    if (int_count_Asset == -1) throw new Exception("Asset insert failed");

                    DataSet dsAsset = app.DB.Select("select_LastAsset", "Assets", null);
                    if (dsAsset != null)
                        if (dsAsset.Tables.Count > 0)
                            if (dsAsset.Tables[0].Rows.Count > 0)
                            {
                                ID_Asset = Convert.ToInt32(dsAsset.Tables[0].Rows[0]["ID"]);
                            }

                    //ID_Asset = app.DB.GetIdentity();

                    /*res.RowsModified = int_count_Asset;*/

                }
                catch (Exception exc)
                {
                    app.DB.RollBackTransaction();
                    return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
                }
            }
            ////////////////////////

            //2. second step, create the order
            int ID_Order = 0;
            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs = new TVariantList();

                //  insert order
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("@prm_ID_RingSession").AsInt32 = ID_RingSession;
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_ID_AssetSession").AsInt32 = ID_AssetSession;
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                vl_params.Add("@prm_Direction").AsBoolean = Direction;
                vl_params.Add("@prm_Quantity").AsDouble = Quantity;
                vl_params.Add("@prm_Price").AsDouble = Price;
                vl_params.Add("@prm_PartialFlag").AsBoolean = PartialFlag;
                vl_params.Add("@prm_ExpirationDate").AsDateTime = ExpirationDate;
                vl_params.Add("@prm_isActive").AsBoolean = true;

                int int_count_Orders = app.DB.Exec("insert_Order_DISPONIBIL", "Orders", vl_params);
                if (int_count_Orders == -1) throw new Exception();

                app.DB.CommitTransaction();

                vl_params = new TVariantList();
                DataSet dsOrder = app.DB.Select("select_LastOrder", "Orders", vl_params);
                if (dsOrder != null)
                    if (dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
                    {
                        ID_Order = Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID"]);
                        updateTradeParameters(0, 0, ID_Order, 0, vl_arguments);

                        // lock warranty
                        if (ID_AssetSession != 0)
                            validateWarranty(ID_Order, ID_Asset, ID_AssetSession, ID_Broker, ID_Client, Direction, Quantity, Price, PartialFlag, SubmitTime, true);
                    }


                res.IDs.Add("ID_Order").AsInt32 = ID_Order;
                res.RowsModified = int_count_Orders;// +int_count_OrderDetails_RGN;

                session.AddToJournal("order added", ID_Client, ID_Ring, ID_Asset, ID_Order, Quantity, Price);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
            //

            //3. third step, if IsInitial, update asset ID_InitialOrder with ID_Order
            if (isInitial || isSameAsInitial)
            {
                try
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_InitialOrder").AsInt32 = ID_Order;
                    vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                    int count = app.DB.Exec("update_Asset_ID_InitialOrder", "Assets", vl_params);
                    if (count == -1) throw new Exception();
                    res.RowsModified += count;

                    if (isInitial && uv.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole))
                    {
                        vl_params = new TVariantList();
                        vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                        vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                        vl_params.Add("@prm_Direction").AsBoolean = Direction;
                        app.DB.Exec("reset_AssetsXClients", "AssetsXClients", vl_params);
                    }

                }
                catch (Exception ex)
                {
                    //app.DB.RollBackTransaction();
                    return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
                }
            }
            //app.DB.CommitTransaction();

            return res;
        }
        public TDBExecResult addOrder(TVariantList vl_arguments)
        {
            switch (session.EntryPoint)
            {
                case "BTGN":
                    return addOrder_RGN(vl_arguments);
                case "DISPONIBIL":
                    return addOrder_DISPONIBIL(vl_arguments);
                default:
                    return new TDBExecResult(TDBExecError.ProcedureNotFound, "");
            }
        }

        public TDBExecResult modifyOrder_RGN(TVariantList vl_arguments)
        {
            //-----------------------------------------------------------------------
            //  check if arguments are provided
            //  generic
            if (vl_arguments["ID_Order"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Order");
            if (vl_arguments["Direction"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Direction");
            if (vl_arguments["Quantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Quantity");
            if (vl_arguments["Price"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Price");

            //  specific arguments
            if (vl_arguments["ID_GNType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_GNType");
            if (vl_arguments["StartDeliveryDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StartDeliveryDate");
            if (vl_arguments["EndDeliveryDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "EndDeliveryDate");
            if (vl_arguments["CombinationsAccepted"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "CombinationsAccepted");
            if (vl_arguments["EntryPoints"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "EntryPoints");
            //-----------------------------------------------------------------------

            //-----------------------------------------------------------------------
            //  extract values and validate
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            int ID_Ring = 1;  //  regularizare gaze
            int ID_Asset = 1; //  gaz natural

            int ID_Agency = 0;
            int ID_Broker = session.ID_Broker;
            int ID_Client = 0;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
            DataSet ds_client = app.DB.Select("select_HousebyID_Broker", "Clients", vl_params);
            if (!app.DB.ValidDSRows(ds_client)) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            else
            {
                ID_Agency = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID_Agency"]);
                ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
            }

            if (ID_Broker == 0 || ID_Client == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Current user is not attached to a broker");

            int ID_Order = 0;
            if (vl_arguments["ID_Order"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Order"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Order"].ValueType == TVariantType.vtInt64) ID_Order = vl_arguments["ID_Order"].AsInt32;
            else res.ParamValidations["ID_Order"].AsString = "Unrecognized Value";

            bool Direction = false;
            switch (vl_arguments["Direction"].AsString.Trim().ToUpper())
            {
                case "B":
                    Direction = false;
                    break;
                case "S":
                    Direction = true;
                    break;
                default:
                    res.ParamValidations["Direction"].AsString = "Expected value 'B' or 'S'. Actual value: " + vl_arguments["Direction"].AsString;
                    break;
                //return new TDBExecResult() { ErrorCode = TDBExecError.ValidationUnsuccesful, Message = "Argument value differs from expected.", ParamValidations = new TVariantList().Add("Direction").AsString = "Expected values B or S" };
            }

            double Quantity = 0;
            if (vl_arguments["Quantity"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtSingle ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtDouble) Quantity = vl_arguments["Quantity"].AsDouble;
            else res.ParamValidations["Quantity"].AsString = "Unrecognized Value";
            if (Quantity <= 0 && res.ParamValidations["Quantity"].AsString == "") res.ParamValidations["Quantity"].AsString = "Value cannot be negative";

            double Price = 0;
            if (vl_arguments["Price"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Price"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Price"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["Price"].ValueType == TVariantType.vtSingle ||
                vl_arguments["Price"].ValueType == TVariantType.vtDouble) Price = vl_arguments["Price"].AsDouble;
            else res.ParamValidations["Price"].AsString = "Unrecognized Value";
            if (Price <= 0 && res.ParamValidations["Price"].AsString == "") res.ParamValidations["Price"].AsString = "Value cannot be negative";

            //  specific arguments
            int ID_GNType = 0;
            if (vl_arguments["ID_GNType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_GNType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_GNType"].ValueType == TVariantType.vtInt64) ID_GNType = vl_arguments["ID_GNType"].AsInt32;

            DateTime StartDeliveryDate = DateTime.Now;
            if (vl_arguments["StartDeliveryDate"].ValueType == TVariantType.vtDateTime) StartDeliveryDate = vl_arguments["StartDeliveryDate"].AsDateTime;
            else res.ParamValidations["StartDeliveryDate"].AsString = "Unrecognized Value";

            DateTime EndDeliveryDate = DateTime.Now;
            if (vl_arguments["EndDeliveryDate"].ValueType == TVariantType.vtDateTime) EndDeliveryDate = vl_arguments["EndDeliveryDate"].AsDateTime;
            else res.ParamValidations["EndDeliveryDate"].AsString = "Unrecognized Value";

            bool CombinationsAccepted = false;
            if (vl_arguments["CombinationsAccepted"].ValueType == TVariantType.vtBoolean) CombinationsAccepted = vl_arguments["CombinationsAccepted"].AsBoolean;
            else res.ParamValidations["CombinationsAccepted"].AsString = "Unrecognized Value";

            if (res.ParamValidations["StartDeliveryDate"].AsString == "" && res.ParamValidations["EndDeliveryDate"].AsString == "")
            {
                //added by Alex P
                int startDeliveryDateOffset = 3;
                int endDeliveryDateOffset = 30;
                TVariantList vl = new TVariantList();
                DataSet ds = app.DB.Select("select_RingParameters_RGN", "RingParameters_RGN", vl);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    startDeliveryDateOffset = Convert.ToInt32(ds.Tables[0].Rows[0]["StartDeliveryDateOffset"].ToString());
                    endDeliveryDateOffset = Convert.ToInt32(ds.Tables[0].Rows[0]["EndDeliveryDateOffset"].ToString());
                }
                //---------------

                //modified by Alex P. on 20140529
                if (StartDeliveryDate < DateTime.Today.AddDays(startDeliveryDateOffset)) res.ParamValidations["StartDeliveryDate"].AsString = "Start Delivery Date must be at least " + DateTime.Today.AddDays(startDeliveryDateOffset).ToString();
                if (StartDeliveryDate > DateTime.Today.AddDays(endDeliveryDateOffset)) res.ParamValidations["StartDeliveryDate"].AsString = "Start Delivery Date must be at most " + DateTime.Today.AddDays(endDeliveryDateOffset).ToString();
                if (EndDeliveryDate <= StartDeliveryDate) res.ParamValidations["EndDeliveryDate"].AsString = "End Delivery Date cannot be less or equal than Start Delivery Date";
                if (EndDeliveryDate > DateTime.Today.AddDays(endDeliveryDateOffset)) res.ParamValidations["EndDeliveryDate"].AsString = "End Delivery Date must be at most " + DateTime.Today.AddDays(endDeliveryDateOffset).ToString();
                //----------------
            }

            //added by Alex P. on 20140529
            TVariantList entryPoints = (TVariantList)vl_arguments["EntryPoints"].AsObject;
            //if (entryPoints.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "There has to be at least 1 Entry Point");
            for (int i = 0; i < entryPoints.Count; i++)
            {
                int entryPointID = Convert.ToInt32(entryPoints[i].Value);
                TVariantList vl = new TVariantList();
                vl.Add("@prm_ID").AsInt32 = entryPointID;
                DataSet ds = app.DB.Select("select_EntryPoint_RGN", "EntryPoints_RGN", vl);

                if (ds == null)
                {
                    res.ParamValidations["EntryPoints"].AsString = "Innexistent entry point " + entryPointID.ToString();
                    return res;
                }

                if (ds.Tables.Count == 0)
                {
                    res.ParamValidations["EntryPoints"].AsString = "Innexistent entry point " + entryPointID.ToString();
                    return res;
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    res.ParamValidations["EntryPoints"].AsString = "Innexistent entry point " + entryPointID.ToString();
                    return res;
                }
            }

            //-----------------------------

            if (res.bInvalid()) return res;

            //-----------------------------------------------------------------------
            //  check for existing order
            vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Order;
            DataSet dsOrder = app.DB.Select("select_Orders", "Orders", vl_params);
            if (dsOrder == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve original order");
            if (dsOrder.Tables.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve original order");
            if (dsOrder.Tables[0].Rows.Count == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Could not retrieve original order"); 

            //  check if we modify an active order
            if (!Convert.ToBoolean(dsOrder.Tables[0].Rows[0]["isActive"])) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Cannot modify an inactive order");

            //  check if the client and asset are the same
            if (Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Client"]) != ID_Client) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Cannot modify an order from a different owner");

            if (Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Asset"]) != ID_Asset) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Cannot modify an order for a different asset");
            //-----------------------------------------------------------------------

            //  get current session for asset
            DataSet ds_ringsession = getRingSession(1);
            DataSet ds_assetsession = getAssetSession(1);
            if (ds_ringsession == null || ds_assetsession == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (ds_ringsession.Tables.Count == 0 || ds_assetsession.Tables.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (ds_ringsession.Tables[0].Rows.Count == 0 || ds_assetsession.Tables[0].Rows.Count == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            if (Convert.ToDateTime(ds_ringsession.Tables[0].Rows[0]["Date"]).Date != DateTime.Today) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            if (Convert.ToDateTime(ds_assetsession.Tables[0].Rows[0]["Date"]).Date != DateTime.Today) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            if (ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "Opened" && ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "PreOpened") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            if (ds_assetsession.Tables[0].Rows[0]["Status"].ToString() != "Opened" && ds_assetsession.Tables[0].Rows[0]["Status"].ToString() != "PreOpened") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs = new TVariantList();

                //  update order
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                vl_params.Add("@prm_Direction").AsBoolean = Direction;
                vl_params.Add("@prm_Quantity").AsDouble = Quantity;
                vl_params.Add("@prm_Price").AsDouble = Price;
                vl_params.Add("@prm_PartialFlag").AsBoolean = CombinationsAccepted;
                vl_params.Add("@prm_ExpirationDate").AsDateTime = EndDeliveryDate;
                vl_params.Add("@prm_isActive").AsBoolean = true;
                vl_params.Add("@prm_ID").AsInt32 = ID_Order;

                int int_count_Orders = app.DB.Exec("update_Order", "Orders", vl_params);
                if (int_count_Orders == -1) throw new Exception();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Order").AsInt32 = ID_Order;
                vl_params.Add("@prm_ID_GNType").AsInt32 = ID_GNType;
                vl_params.Add("@prm_StartDeliveryDate").AsDateTime = StartDeliveryDate;
                vl_params.Add("@prm_EndDeliveryDate").AsDateTime = EndDeliveryDate;
                vl_params.Add("@prm_CombinationsAccepted").AsBoolean = CombinationsAccepted;

                int int_count_OrderDetails_RGN = app.DB.Exec("update_OrderDetails_RGN", "Orders", vl_params);
                if (int_count_OrderDetails_RGN == -1) throw new Exception();
                
                //added by Alex P
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Order").AsInt32 = ID_Order;
                vl_params.Add("@prm_ID_EntryPoint").AsInt32 = -1;
                vl_params.Add("@prm_IsDeleted").AsInt32 = 1;
                int int_count_EntryPoints_RGN = app.DB.Exec("update_OrderEntryPoint_RGN", "EntryPoints_RGN", vl_params);
                if (int_count_EntryPoints_RGN == -1) throw new Exception();
                                
                for (int i = 0; i < entryPoints.Count; i++)
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_Order").AsInt32 = ID_Order;
                    vl_params.Add("@prm_ID_EntryPoint").AsInt32 = Convert.ToInt32(entryPoints[i].Value);
                    vl_params.Add("@prm_IsDeleted").AsInt32 = 0;
                    int int_count_entrypoints = app.DB.Exec("update_OrderEntryPoint_RGN", "EntryPoints_RGN", vl_params);
                    if (int_count_entrypoints <= 0)
                    {
                        int_count_entrypoints = app.DB.Exec("insert_OrderEntryPoint_RGN", "EntryPoints_RGN", vl_params);
                        if (int_count_entrypoints == -1) throw new Exception();
                    }
                    
                }
                //----------------

                session.AddToJournal("order modified", ID_Client, ID_Ring, ID_Asset, ID_Order, Quantity, Price);

                app.DB.CommitTransaction();

                res.IDs.Add("ID_Order").AsInt32 = ID_Order;
                res.RowsModified = int_count_Orders + int_count_OrderDetails_RGN;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }

        }

        public TDBExecResult modifyOrder_DISPONIBIL(TVariantList vl_arguments)
        {
            //-----------------------------------------------------------------------
            //  check if arguments are provided
            //  generic
            if (vl_arguments["ID_Order"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Order");
            if (vl_arguments["Direction"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Direction");
            if (vl_arguments["Quantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Quantity");
            if (vl_arguments["Price"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Price");

            //  specific arguments
            //-----------------------------------------------------------------------
            bool isInitial = false;
            if (vl_arguments["isInitial"] != null)
            {
                if (vl_arguments["isInitial"].ValueType == TVariantType.vtBoolean) isInitial = vl_arguments["isInitial"].AsBoolean;
            }

            //-----------------------------------------------------------------------
            //  extract values and validate
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            int ID_Agency = 0;
            int ID_Broker = 0;
            int ID_Client = 0;

            UserValidation uv = new UserValidation(app);
            if (uv.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole))
            {
                if (vl_arguments["ID_Agency"] != null) ID_Agency = vl_arguments["ID_Agency"].AsInt32;
                if (vl_arguments["ID_Broker"] != null) ID_Broker = vl_arguments["ID_Broker"].AsInt32;
            }
            else ID_Broker = session.ID_Broker;

            if (vl_arguments["ID_Client"] != null) ID_Client = vl_arguments["ID_Client"].AsInt32;

            TVariantList vl_params;
            if (ID_Broker != 0)
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                DataSet ds_client = app.DB.Select("select_HousebyID_Broker", "Clients", vl_params);
                if (app.DB.ValidDSRows(ds_client))
                {
                    ID_Agency = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID_Agency"]);
                    //if (ID_Client == 0) ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
                }
            }

            if (ID_Agency == 0 && (ID_Broker == 0 || ID_Client == 0)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Current user is not attached to a broker");

            int ID_Order = 0;
            if (vl_arguments["ID_Order"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Order"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Order"].ValueType == TVariantType.vtInt64) ID_Order = vl_arguments["ID_Order"].AsInt32;
            else res.ParamValidations["ID_Order"].AsString = "Unrecognized Value";

            bool Direction = false;
            switch (vl_arguments["Direction"].AsString.Trim().ToUpper())
            {
                case "B":
                    Direction = false;
                    break;
                case "S":
                    Direction = true;
                    break;
                default:
                    res.ParamValidations["Direction"].AsString = "Expected value 'B' or 'S'. Actual value: " + vl_arguments["Direction"].AsString;
                    break;
                //return new TDBExecResult() { ErrorCode = TDBExecError.ValidationUnsuccesful, Message = "Argument value differs from expected.", ParamValidations = new TVariantList().Add("Direction").AsString = "Expected values B or S" };
            }

            double Quantity = 0;
            if (vl_arguments["Quantity"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtSingle ||
                vl_arguments["Quantity"].ValueType == TVariantType.vtDouble) Quantity = vl_arguments["Quantity"].AsDouble;
            else res.ParamValidations["Quantity"].AsString = "Unrecognized Value";
            if (Quantity <= 0 && res.ParamValidations["Quantity"].AsString == "") res.ParamValidations["Quantity"].AsString = "Value cannot be negative";

            double Price = double.NaN;
            if (isInitial && vl_arguments["Price"].ValueType == TVariantType.vtString && vl_arguments["Price"].AsString == "none") ;
            else
            {
                if (vl_arguments["Price"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["Price"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["Price"].ValueType == TVariantType.vtInt64 ||
                    vl_arguments["Price"].ValueType == TVariantType.vtSingle ||
                    vl_arguments["Price"].ValueType == TVariantType.vtDouble) Price = vl_arguments["Price"].AsDouble;
                else res.ParamValidations["Price"].AsString = "Unrecognized Value";
            }
            //if (Price <= 0 && res.ParamValidations["Price"].AsString == "") res.ParamValidations["Price"].AsString = "Value cannot be negative";

            //  specific arguments
            bool PartialFlag = false;
            if (vl_arguments["PartialFlag"] != null)
            {
                if (vl_arguments["PartialFlag"].ValueType == TVariantType.vtBoolean) PartialFlag = vl_arguments["PartialFlag"].AsBoolean;
            }

            if (vl_arguments["isPartial"] != null)
            {
                if (vl_arguments["isPartial"].ValueType == TVariantType.vtBoolean) PartialFlag = vl_arguments["isPartial"].AsBoolean;
            }

            DateTime ExpirationDate = DateTime.Now;
            if (vl_arguments["ExpirationDate"] != null)
                if (vl_arguments["ExpirationDate"].ValueType == TVariantType.vtDateTime)
                    ExpirationDate = vl_arguments["ExpirationDate"].AsDateTime;

            DateTime SubmitTime = DateTime.Now;
            if (vl_arguments["SubmitTime"] != null)
                if (vl_arguments["SubmitTime"].ValueType == TVariantType.vtDateTime)
                    SubmitTime = vl_arguments["SubmitTime"].AsDateTime;

            if (res.bInvalid()) return res;

            //-----------------------------------------------------------------------
            //  check for existing order
            vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Order;
            DataSet dsOrder = app.DB.Select("select_Orders", "Orders", vl_params);
            if (dsOrder == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve original order");
            if (dsOrder.Tables.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve original order");
            if (dsOrder.Tables[0].Rows.Count == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Could not retrieve original order");

            int ID_Ring = Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Ring"]);
            int ID_Asset = Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Asset"]);
            isInitial = Convert.ToBoolean(dsOrder.Tables[0].Rows[0]["isInitial"]);
            double qty_Transacted = Convert.ToDouble(dsOrder.Tables[0].Rows[0]["TransactedQuantity"]);
            Quantity += qty_Transacted;
            double old_Price = double.NaN;
            try
            {
                Convert.ToDouble(dsOrder.Tables[0].Rows[0]["Price"]);
            }
            catch {}

            //  check if we modify an active order
            if (!Convert.ToBoolean(dsOrder.Tables[0].Rows[0]["isActive"]) && !uv.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole))
            {
                session.AddToJournal("modifyOrder", ID_Client, ID_Ring, ID_Asset, ID_Order, Quantity, Price, "Cannot modify an inactive order");
                return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Cannot modify an inactive order");
            }

            //  check if the client and asset are the same
            if (Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Agency"]) != ID_Agency && !uv.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole))
            {
                session.AddToJournal("modifyOrder", ID_Client, ID_Ring, ID_Asset, ID_Order, Quantity, Price, "Cannot modify an order from a different agency");
                return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Cannot modify an order from a different agency");
            }

            if (ID_Client != 0)
            {
                if (Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Client"]) != ID_Client && !uv.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole))
                {
                    session.AddToJournal("modifyOrder", ID_Client, ID_Ring, ID_Asset, ID_Order, Quantity, Price, "Cannot modify an order from a different owner");
                    return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Cannot modify an order from a different owner");
                }
            }
            else
            {
                ID_Broker = Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Broker"]);
                ID_Client = Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Client"]);
            }


            if (Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Asset"]) != ID_Asset) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Cannot modify an order for a different asset");
            //-----------------------------------------------------------------------

            bool isDoubleTrouble = false;
            bool isSameAsInitial = false;
            int ID_AssetSession = Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_AssetSession"]);
            if (ID_AssetSession != 0 && !uv.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole))
            {
                string s = validateTradeParameters(ID_Order, ID_Asset, ID_AssetSession, ID_Broker, ID_Client, Direction, Quantity, Price, PartialFlag, SubmitTime, ref isDoubleTrouble, ref isSameAsInitial);
                if (s != "")
                {
                    session.AddToJournal("modifyOrder", ID_Client, ID_Ring, ID_Asset, ID_Order, Quantity, Price, s);
                    return new TDBExecResult(TDBExecError.ValidationUnsuccesful, s);
                }

                s = validateWarranty(ID_Order, ID_Asset, ID_AssetSession, ID_Broker, ID_Client, Direction, Quantity, Price, PartialFlag, SubmitTime);
                if (s != "")
                {
                    session.AddToJournal("modifyOrder", ID_Client, ID_Ring, ID_Asset, ID_Order, Quantity, Price, s);
                    return new TDBExecResult(TDBExecError.ValidationUnsuccesful, s);
                }
            }

            //  get current session for asset
            DataSet ds_ringsession = getRingSession(ID_Ring);
            DataSet ds_assetsession = getAssetSession(ID_Asset);
            if (!isInitial)
            {
                if (ds_ringsession == null || ds_assetsession == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
                if (ds_ringsession.Tables.Count == 0 || ds_assetsession.Tables.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
                if (ds_ringsession.Tables[0].Rows.Count == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
                if (Convert.ToDateTime(ds_ringsession.Tables[0].Rows[0]["Date"]).Date != DateTime.Today) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
                //if (ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "Opened" && ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "PreOpened") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
                int ID_RingSession = Convert.ToInt32(ds_ringsession.Tables[0].Rows[0]["ID"]);

                if (ds_assetsession.Tables[0].Rows.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
                if (!isInitial && Convert.ToDateTime(ds_assetsession.Tables[0].Rows[0]["Date"]).Date != DateTime.Today) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");

                if (ds_assetsession.Tables[0].Rows[0]["Status"].ToString() != "Opened" &&
                    ds_assetsession.Tables[0].Rows[0]["Status"].ToString() != "PreOpened" && !isSameAsInitial) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Ordinele nu mai pot fi modificate in afara fazei 1 si fazei a doua");
                /*if (ds_assetsession.Tables[0].Rows[0]["Status"].ToString() != "Opened" &&
                    ds_assetsession.Tables[0].Rows[0]["Status"].ToString() != "PreOpened") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Ordinele nu mai pot fi modificate in afara fazei 1 si fazei a doua");*/
            }


            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs = new TVariantList();

                //  update order
                vl_params = new TVariantList();
                //vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                //vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                vl_params.Add("@prm_Direction").AsBoolean = Direction;
                vl_params.Add("@prm_Quantity").AsDouble = Quantity;
                /*if (double.IsNaN(Price)) vl_params.Add("@prm_Price").AsDouble = null;
                else*/
                vl_params.Add("@prm_Price").AsDouble = Price;
                vl_params.Add("@prm_PartialFlag").AsBoolean = PartialFlag;
                vl_params.Add("@prm_ExpirationDate").AsDateTime = ExpirationDate;
                vl_params.Add("@prm_isActive").AsBoolean = true;
                vl_params.Add("@prm_ID").AsInt32 = ID_Order;

                int int_count_Orders = app.DB.Exec("update_Order", "Orders", vl_params);
                if (int_count_Orders == -1) throw new Exception();

                vl_params.Add("@prm_isTransacted").AsBoolean = false;
                app.DB.Exec("update_isTransacted", "Orders", vl_params);

                if (vl_arguments["isInitial"] != null) updateTradeParameters(0, 0, ID_Order, 0, vl_arguments);

                session.AddToJournal("order modified", ID_Client, ID_Ring, ID_Asset, ID_Order, Quantity, Price);
                if (!isInitial && ds_assetsession.Tables[0].Rows[0]["Status"].ToString() == "Opened")
                {
                    switch (Direction)
                    {
                        case false:
                            if (Price > old_Price) session.SetEvent(0, "DeltaT", "reset", ID_Asset, ID_Order);
                            break;
                        case true:
                            if (Price < old_Price) session.SetEvent(0, "DeltaT", "reset", ID_Asset, ID_Order);
                            break;
                    }
                }

                if (isInitial && uv.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole))
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                    vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                    vl_params.Add("@prm_Direction").AsBoolean = Direction;
                    app.DB.Exec("reset_AssetsXClients", "AssetsXClients", vl_params);
                }

                app.DB.CommitTransaction();

                if (ID_AssetSession != 0) validateWarranty(ID_Order, ID_Asset, ID_AssetSession, ID_Broker, ID_Client, Direction, Quantity, Price, PartialFlag, SubmitTime, true);

                res.IDs.Add("ID_Order").AsInt32 = ID_Order;
                res.RowsModified = int_count_Orders;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }

        }

        public TDBExecResult modifyOrder(TVariantList vl_arguments)
        {
            switch (session.EntryPoint)
            {
                case "BTGN":
                    return modifyOrder_RGN(vl_arguments);
                case "DISPONIBIL":
                    return modifyOrder_DISPONIBIL(vl_arguments);
                default:
                    return new TDBExecResult(TDBExecError.ProcedureNotFound, "");
            }
        }

        public TDBExecResult cancelOrder(TVariantList vl_arguments)
        {
            //-----------------------------------------------------------------------
            //  check if arguments are provided
            if (vl_arguments["ID_Order"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Order");
            //-----------------------------------------------------------------------

            //-----------------------------------------------------------------------
            //  extract values and validate

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            int ID_Broker = session.ID_Broker;
            if (ID_Broker == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Current user is not attached to a broker");

            int ID_Client = 0;

            int ID_Order = 0;
            if (vl_arguments["ID_Order"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Order"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Order"].ValueType == TVariantType.vtInt64) ID_Order = vl_arguments["ID_Order"].AsInt32;
            else res.ParamValidations["ID_Order"].AsString = "Unrecognized Value";

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Order;
            DataSet dsOrder = app.DB.Select("select_Orders", "Orders", vl_params);
            if (dsOrder == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve original order");
            if (dsOrder.Tables.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve original order");
            if (dsOrder.Tables[0].Rows.Count == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Could not retrieve original order");

            //  check if we modify an active order
            if (!Convert.ToBoolean(dsOrder.Tables[0].Rows[0]["isActive"])) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Cannot cancel an inactive order");

            int ID_Ring = Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Ring"]);
            int ID_Asset = Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Asset"]);
            ID_Client = Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Client"]);

            if (ID_Client == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Current user is not attached to a broker");

            if (ID_Client != session.ID_Client)
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                vl_params.Add("@prm_ID_Order").AsInt32 = ID_Order;

                DataSet ds = app.DB.Select("select_BrokerClients", "Clients", vl_params);
                if (ds == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve broker clients");
                if (ds.Tables.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve broker clients");
                if (ds.Tables[0].Rows.Count == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Could not retrieve broker clients");

                bool found = false;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int ID_BrokerClient = Convert.ToInt32(ds.Tables[0].Rows[i]["ID_Client"].ToString());
                    if (ID_BrokerClient == ID_Client)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Cannot cancel an order from a different owner");
            }
            //-----------------------------

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs = new TVariantList();

                //  update order
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_Order;

                int int_count_Orders = app.DB.Exec("cancel_Order", "Orders", vl_params);
                if (int_count_Orders == -1) throw new Exception();

                app.DB.Exec("cancel_WarrantyLock", "BO_Operations", vl_params);

                //----------------

                session.AddToJournal("order canceled", ID_Client, ID_Ring, ID_Asset, ID_Order, 0, 0);

                app.DB.CommitTransaction();

                res.IDs.Add("ID_Order").AsInt32 = ID_Order;
                res.RowsModified = int_count_Orders /*+ int_count_OrderDetails_RGN*/;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }

        }
        /*public TDBExecResult cancelOrder(TVariantList vl_arguments)
        {
            //-----------------------------------------------------------------------
            //  check if arguments are provided
            if (vl_arguments["ID_Order"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Order");
            //-----------------------------------------------------------------------

            //-----------------------------------------------------------------------
            //  extract values and validate
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            //int ID_Ring = 1;  //  regularizare gaze
            //int ID_Asset = 1; //  gaz natural
            int ID_Broker = session.ID_Broker;
            int ID_Client = 0;
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
            DataSet ds_client = app.DB.Select("select_HousebyID_Broker", "Clients", vl_params);
            if (ds_client != null)
                if (ds_client.Tables.Count > 0)
                    if (ds_client.Tables[0].Rows.Count > 0)
                    {
                        ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
                    }

            if (ID_Broker == 0 || ID_Client == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Current user is not attached to a broker");

            int ID_Order = 0;
            if (vl_arguments["ID_Order"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Order"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Order"].ValueType == TVariantType.vtInt64) ID_Order = vl_arguments["ID_Order"].AsInt32;
            else res.ParamValidations["ID_Order"].AsString = "Unrecognized Value";
            //-----------------------------

            if (res.bInvalid()) return res;

            //-----------------------------------------------------------------------
            //  check for existing order
            vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Order;
            DataSet dsOrder = app.DB.Select("select_Orders", "Orders", vl_params);
            if (dsOrder == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve original order");
            if (dsOrder.Tables.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve original order");
            if (dsOrder.Tables[0].Rows.Count == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Could not retrieve original order");

            //  check if we modify an active order
            if (!Convert.ToBoolean(dsOrder.Tables[0].Rows[0]["isActive"])) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Cannot cancel an inactive order");

            //  check if the client and asset are the same
            if (Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Client"]) != ID_Client) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Cannot cancel an order from a different owner");

            int ID_Ring = Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Ring"]);
            int ID_Asset = Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Asset"]);
            //if (Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ID_Asset"]) != ID_Asset) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Cannot cancel an order for a different asset");
            //-----------------------------------------------------------------------


            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs = new TVariantList();

                //  update order
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_Order;

                int int_count_Orders = app.DB.Exec("cancel_Order", "Orders", vl_params);
                if (int_count_Orders == -1) throw new Exception();

                app.DB.Exec("cancel_WarrantyLock", "BO_Operations", vl_params);

                //----------------

                session.AddToJournal("order canceled", ID_Client, ID_Ring, ID_Asset, ID_Order, 0, 0);

                app.DB.CommitTransaction();

                res.IDs.Add("ID_Order").AsInt32 = ID_Order;
                res.RowsModified = int_count_Orders; //+ int_count_OrderDetails_RGN;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }

        }*/

        public TDBExecResult suspendOrder(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Order"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Order");

            int ID_Order = 0;
            if (vl_arguments["ID_Order"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Order"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Order"].ValueType == TVariantType.vtInt64) ID_Order = vl_arguments["ID_Order"].AsInt32;
            else return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Value not recognized");

            //-----------------------------------------------------------------------
            //  check for existing order
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Order;
            DataSet dsOrder = app.DB.Select("select_Orders", "Orders", vl_params);
            if (dsOrder == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve original order");
            if (dsOrder.Tables.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve original order");
            if (dsOrder.Tables[0].Rows.Count == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Could not retrieve original order");

            //  check if we modify an active order
            if (!Convert.ToBoolean(dsOrder.Tables[0].Rows[0]["isActive"])) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Cannot suspend an inactive order");

            vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Order").AsInt32 = ID_Order;
            app.DB.Exec("suspend_Order", "Orders", vl_params);

            TDBExecResult res = new TDBExecResult(TDBExecError.Success, "");
            return res;
        }

        public TDBExecResult reactivateOrder(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Order"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Order");

            int ID_Order = 0;
            if (vl_arguments["ID_Order"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Order"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Order"].ValueType == TVariantType.vtInt64) ID_Order = vl_arguments["ID_Order"].AsInt32;
            else return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Value not recognized");

            //-----------------------------------------------------------------------
            //  check for existing order
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Order;
            DataSet dsOrder = app.DB.Select("select_Orders", "Orders", vl_params);
            if (dsOrder == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve original order");
            if (dsOrder.Tables.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "Could not retrieve original order");
            if (dsOrder.Tables[0].Rows.Count == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Could not retrieve original order");

            //  check if we modify an active order
            if (!Convert.ToBoolean(dsOrder.Tables[0].Rows[0]["isSuspended"])) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Can only activate suspended orders");
            
            vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Order").AsInt32 = ID_Order;
            app.DB.Exec("reactivate_Order", "Orders", vl_params);

            TDBExecResult res = new TDBExecResult(TDBExecError.Success, "");
            return res;
        }

        public int acceptOrderMatch(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_OrderMatch"] == null) return -1;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_OrderMatch").AsInt32 = vl_arguments["ID_OrderMatch"].AsInt32;
            DataSet ds = app.DB.Select("ExecuteOrderMatch", "Orders", vl_params);

            if (ds == null) return -1;
            if (ds.Tables.Count == 0) return -1;
            if (ds.Tables[0].Rows.Count == 0) return -1;

            return Convert.ToInt32(ds.Tables[0].Rows[0]["Result"]);
        }

        public TDBExecResult setWhitelist(TVariantList vl_arguments)
        {
            int ID_Ring = 1;  //  regularizare gaze
            int ID_Asset = 1; //  gaz natural
            int ID_Broker = session.ID_Broker;
            int ID_Client = 0;
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
            DataSet ds_client = app.DB.Select("select_HousebyID_Broker", "Clients", vl_params);
            if (ds_client == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (ds_client.Tables.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (ds_client.Tables[0].Rows.Count > 0)
            {
                ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
                vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
            }
            if (ID_Broker == 0 || ID_Client == 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Current user is not attached to a broker");

            if (vl_arguments["ID_AgreedClient"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AgreedClient");
            if (vl_arguments["isAgreed"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isAgreed");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            int ID_AgreedClient = 0;
            if (vl_arguments["ID_AgreedClient"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_AgreedClient"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_AgreedClient"].ValueType == TVariantType.vtInt64) ID_AgreedClient = vl_arguments["ID_AgreedClient"].AsInt32;
            else res.ParamValidations["ID_AgreedClient"].AsString = "Unrecognized Value";
            if (ID_AgreedClient <= 0 && res.ParamValidations["ID_AgreedClient"].AsString == "") res.ParamValidations["ID_AgreedClient"].AsString = "Value cannot be negative";

            bool isAgreed = false;
            if (vl_arguments["isAgreed"].ValueType == TVariantType.vtBoolean) isAgreed = vl_arguments["isAgreed"].AsBoolean;
            else res.ParamValidations["isAgreed"].AsString = "Unrecognized Value";

            int int_ID_Reason = 0;
            if (!isAgreed)
            {
                if (vl_arguments["ID_Reason"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Reason");

                if (vl_arguments["ID_Reason"].ValueType == TVariantType.vtByte ||
                    vl_arguments["ID_Reason"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Reason"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Reason"].ValueType == TVariantType.vtInt64) int_ID_Reason = vl_arguments["ID_Reason"].AsInt32;
                else res.ParamValidations["ID_Reason"].AsString = "Value not recognized";
            }

            /*string Reason = "";
            if (!isAgreed)
            {
                if (vl_arguments["Reason"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Reason");

                if (vl_arguments["Reason"].ValueType == TVariantType.vtString)
                {
                    Reason = vl_arguments["Reason"].AsString;
                    Reason = Reason.Trim();
                    if (Reason == "") res.ParamValidations["Reason"].AsString = "Empty value for Reason";
                }
                else res.ParamValidations["Reason"].AsString = "Unrecognized Value";
            }*/
            
            if (res.bInvalid()) return res;

            vl_params.Add("@prm_ID_AgreedClient").AsInt32 = ID_AgreedClient;
            vl_params.Add("@prm_isAgreed").AsBoolean = isAgreed;
            vl_params.Add("@prm_ID_DisagreeReason").AsInt32 = int_ID_Reason;
            vl_params.Add("@prm_Reason").AsString = "";// Reason;
            switch (isAgreed)
            {
                case false:
                    vl_params.Add("@prm_isApproved").AsBoolean = false;
                    break;
                case true:
                    vl_params.Add("@prm_isApproved").AsBoolean = true;
                    break;
            }

            //  override the choice of isAgreed - all requests will be approved
            vl_params["@prm_isApproved"].AsBoolean = true;

            int int_count_AgreedClients = app.DB.Exec("update_AgreedClient", "AgreedClients", vl_params);
            if (int_count_AgreedClients == -1) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (int_count_AgreedClients == 0)
            {
                int_count_AgreedClients = app.DB.Exec("insert_AgreedClient", "AgreedClients", vl_params);
                if (int_count_AgreedClients == -1) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

                return new TDBExecResult(TDBExecError.Success, "", int_count_AgreedClients);
            }

            return new TDBExecResult(TDBExecError.Success, "", int_count_AgreedClients);
        }

        public TDBExecResult addWhitelistReason(TVariantList vl_arguments)
        {
            int ID_User = session.ID_User;
            UserValidation valUser = new UserValidation(this.app);
            if (!valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Only Administrator can perform this action");

            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "No arguments are provided");
            if (vl_arguments["Reason"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Reason");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);
            string str_Reason = "";
            if (vl_arguments["Reason"].ValueType == TVariantType.vtString) str_Reason = vl_arguments["Reason"].AsString;
            else res.ParamValidations["Reason"].AsString = "Value not recognized";
            
            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Reason").AsString = str_Reason;
            int int_count_reasons = app.DB.Exec("insert_DisagreeReason", "DisagreeReasons", vl_params);
            if (int_count_reasons == -1) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            
            return new TDBExecResult(TDBExecError.Success, "", int_count_reasons);
        }

        public TDBExecResult editWhitelistReason(TVariantList vl_arguments)
        {
            int ID_User = session.ID_User;
            UserValidation valUser = new UserValidation(this.app);
            if (!valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Only Administrator can perform this action");

            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "No arguments are provided");
            if (vl_arguments["ID_Reason"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Reason");
            if (vl_arguments["Reason"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Reason");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int int_ID_Reason = 0;
            if (vl_arguments["ID_Reason"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Reason"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Reason"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Reason"].ValueType == TVariantType.vtInt64) int_ID_Reason = vl_arguments["ID_Reason"].AsInt32;
            else res.ParamValidations["ID_Reason"].AsString = "Value not recognized";

            string str_Reason = "";
            if (vl_arguments["Reason"].ValueType == TVariantType.vtString) str_Reason = vl_arguments["Reason"].AsString;
            else res.ParamValidations["Reason"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Reason").AsString = str_Reason;
            vl_params.Add("@prm_ID").AsInt32 = int_ID_Reason;
            int int_count_reasons = app.DB.Exec("update_DisagreeReason", "DisagreeReasons", vl_params);
            if (int_count_reasons == -1) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            
            return new TDBExecResult(TDBExecError.Success, "", int_count_reasons);
        }

        public TDBExecResult deleteWhitelistReason(TVariantList vl_arguments)
        {
            int ID_User = session.ID_User;
            UserValidation valUser = new UserValidation(this.app);
            if (!valUser.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Only Administrator can perform this action");

            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "No arguments are provided");
            if (vl_arguments["ID_Reason"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Reason");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int int_ID_Reason = 0;
            if (vl_arguments["ID_Reason"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Reason"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Reason"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Reason"].ValueType == TVariantType.vtInt64) int_ID_Reason = vl_arguments["ID_Reason"].AsInt32;
            else res.ParamValidations["ID_Reason"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = int_ID_Reason;
            int int_count_reasons = app.DB.Exec("delete_DisagreeReason", "DisagreeReasons", vl_params);
            if (int_count_reasons == -1) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_count_reasons);
        }

        public TDBExecResult approveWhitelistRequest(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Request"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Request");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Request = 0;
            if (vl_arguments["ID_Request"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Request"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Request"].ValueType == TVariantType.vtInt64) ID_Request = vl_arguments["ID_Request"].AsInt32;
            else res.ParamValidations["ID_Request"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Request;
            int count_AgreedClients = app.DB.Exec("approveRequest", "AgreedClients", vl_params);
            if (count_AgreedClients <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult addEntryPoint_RGN(TVariantList vl_arguments)
        {
            if (vl_arguments["PhysicalPointCode"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PhysicalPointCode");
            if (vl_arguments["PhysicalPointName"] == null) new TDBExecResult(TDBExecError.ArgumentMissing, "PhysicalPointName");
            if (vl_arguments["TechnologicalMinimumPressure"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TechnologicalMinimumPressure");
            if (vl_arguments["AnnualAverageGrossCalorificValue"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "AnnualAverageGrossCalorificValue");
            if (vl_arguments["AnnualUM"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "AnnualUM");
            if (vl_arguments["TechnologicalCapacity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TechnologicalCapacity");
            if (vl_arguments["TechnologicalCapacityUM"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TechnologicalCapacityUM");
            if (vl_arguments["ID_County"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_County");
            if (vl_arguments["GNTypes"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "GNTypes");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            //  validation

            int ID_County = 0;
            try { ID_County = vl_arguments["ID_County"].AsInt32; }
            catch { res.ParamValidations["ID_County"].AsString = "Value not recognized"; }

            string PhysicalPointCode = "";
            try { PhysicalPointCode = vl_arguments["PhysicalPointCode"].AsString; }
            catch { res.ParamValidations["PhysicalPointCode"].AsString = "Value not recognized"; }

            string PhysicalPointName = "";
            try { PhysicalPointName = vl_arguments["PhysicalPointName"].AsString; }
            catch { res.ParamValidations["PhysicalPointName"].AsString = "Value not recognized"; }

            string VirtualPointCode = "";
            try { VirtualPointCode = (vl_arguments["VirtualPointCode"] == null) ? "" : vl_arguments["VirtualPointCode"].AsString; }
            catch { res.ParamValidations["VirtualPointCode "].AsString = "Value not recognized"; }

            string VirtualPointName = "";
            try { VirtualPointName = (vl_arguments["VirtualPointName"] == null) ? "" : vl_arguments["VirtualPointName"].AsString; }
            catch { res.ParamValidations["VirtualPointName"].AsString = "Value not recognized"; }

            string VirtualEntryPointCode = "";
            try { VirtualEntryPointCode = (vl_arguments["VirtualEntryPointCode"] == null) ? "" : vl_arguments["VirtualEntryPointCode"].AsString; }
            catch { res.ParamValidations["VirtualEntryPointCode"].AsString = "Value not recognized"; }

            string VirtualEntryPointName = "";
            try { VirtualEntryPointName = (vl_arguments["VirtualEntryPointName"] == null) ? "" : vl_arguments["VirtualEntryPointName"].AsString; }
            catch { res.ParamValidations["VirtualEntryPointName"].AsString = "Value not recognized"; }

            string AdjacentNetworkOperator = "";
            try { AdjacentNetworkOperator = (vl_arguments["AdjacentNetworkOperator"] == null) ? "" : vl_arguments["AdjacentNetworkOperator"].AsString; }
            catch { res.ParamValidations["AdjacentNetworkOperator"].AsString = "Value not recognized"; }

            string AdjacentNetworkOperatorType = "";
            try { AdjacentNetworkOperatorType = (vl_arguments["AdjacentNetworkOperatorType"] == null) ? "" : vl_arguments["AdjacentNetworkOperatorType"].AsString; }
            catch { res.ParamValidations["AdjacentNetworkOperatorType"].AsString = "Value not recognized"; }

            string Locality = "";
            try { Locality = (vl_arguments["Locality"] == null) ? "" : vl_arguments["Locality"].AsString; }
            catch { res.ParamValidations["Locality"].AsString = "Value not recognized"; }

            string LowerSirutaCode = "";
            try { LowerSirutaCode = (vl_arguments["LowerSirutaCode"] == null) ? "" : vl_arguments["LowerSirutaCode"].AsString; }
            catch { res.ParamValidations["LowerSirutaCode"].AsString = "Value not recognized"; }

            string UpperSirutaCode = "";
            try { UpperSirutaCode = (vl_arguments["UpperSirutaCode"] == null) ? "" : vl_arguments["UpperSirutaCode"].AsString; }
            catch { res.ParamValidations["UpperSirutaCode"].AsString = "Value not recognized"; }

            string TerritorialBranch = "";
            try { TerritorialBranch = (vl_arguments["TerritorialBranch"] == null) ? "" : vl_arguments["TerritorialBranch"].AsString; }
            catch { res.ParamValidations["TerritorialBranch"].AsString = "Value not recognized"; }

            string Sector = "";
            try { Sector = (vl_arguments["Sector"] == null) ? "" : vl_arguments["Sector"].AsString; }
            catch { res.ParamValidations["Sector"].AsString = "Value not recognized"; }

            double TechnologicalMinimumPressure = 0;
            try { TechnologicalMinimumPressure = (vl_arguments["TechnologicalMinimumPressure"] == null) ? 0 : vl_arguments["TechnologicalMinimumPressure"].AsDouble; }
            catch { res.ParamValidations["TechnologicalMinimumPressure"].AsString = "Value not recognized"; }

            string TechnologicalMinimumUM = "";
            try { TechnologicalMinimumUM = (vl_arguments["TechnologicalMinimumUM"] == null) ? "" : vl_arguments["TechnologicalMinimumUM"].AsString; }
            catch { res.ParamValidations["TechnologicalMinimumUM"].AsString = "Value not recognized"; }

            double TechnologicalMaximumPressure = 0;
            try { TechnologicalMaximumPressure = (vl_arguments["TechnologicalMaximumPressure"] == null) ? 0 : vl_arguments["TechnologicalMaximumPressure"].AsDouble; }
            catch { res.ParamValidations["TechnologicalMaximumPressure"].AsString = "Value not recognized"; }

            string TechnologicalMaximumUM = "";
            try { TechnologicalMaximumUM = (vl_arguments["TechnologicalMaximumUM"] == null) ? "" : vl_arguments["TechnologicalMaximumUM"].AsString; }
            catch { res.ParamValidations["TechnologicalMaximumUM"].AsString = "Value not recognized"; }

            double AnnualAverageGrossCalorificValue = 0;
            try { AnnualAverageGrossCalorificValue = (vl_arguments["AnnualAverageGrossCalorificValue"] == null) ? 0 : vl_arguments["AnnualAverageGrossCalorificValue"].AsDouble; }
            catch { res.ParamValidations["AnnualAverageGrossCalorificValue"].AsString = "Value not recognized"; }

            string AnnualUM = "";
            try { AnnualUM = (vl_arguments["AnnualUM"] == null) ? "" : vl_arguments["AnnualUM"].AsString; }
            catch { res.ParamValidations["AnnualUM"].AsString = "Value not recognized"; }

            double TechnologicalCapacity = 0;
            try { TechnologicalCapacity = (vl_arguments["TechnologicalCapacity"] == null) ? 0 : vl_arguments["TechnologicalCapacity"].AsDouble; }
            catch { res.ParamValidations["TechnologicalCapacity"].AsString = "Value not recognized"; }

            string TechnologicalCapacityUM = "";
            try { TechnologicalCapacityUM = (vl_arguments["TechnologicalCapacityUM"] == null) ? "" : vl_arguments["TechnologicalCapacityUM"].AsString; }
            catch { res.ParamValidations["TechnologicalCapacityUM"].AsString = "Value not recognized"; }

            //added by Alex P. on 20140908
            TVariantList GNTypes = (TVariantList)vl_arguments["GNTypes"].AsObject;
            for (int i = 0; i < GNTypes.Count; i++)
            {
                int gnTypeID = Convert.ToInt32(GNTypes[i].Value);
                TVariantList vl = new TVariantList();
                vl.Add("@prm_ID").AsInt32 = gnTypeID;               
                DataSet ds = app.DB.Select("select_GNType", "GNTypes", vl);

                if ((ds == null) || (ds.Tables.Count == 0) || (ds.Tables[0].Rows.Count == 0))
                {
                    res.ParamValidations["GNType"].AsString = "Innexistent GNType " + gnTypeID.ToString();
                    return res;
                }                
            }            
            //-----------------------------

            if (res.bInvalid()) return res;


            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_County").AsInt32 = ID_County;
                vl_params.Add("@prm_PhysicalPointCode").AsString = PhysicalPointCode;
                vl_params.Add("@prm_PhysicalPointName").AsString = PhysicalPointName;
                vl_params.Add("@prm_VirtualPointCode").AsString = VirtualPointCode;
                vl_params.Add("@prm_VirtualPointName").AsString = VirtualPointName;
                vl_params.Add("@prm_VirtualEntryPointCode").AsString = VirtualEntryPointCode;
                vl_params.Add("@prm_VirtualEntryPointName").AsString = VirtualEntryPointName;
                vl_params.Add("@prm_AdjacentNetworkOperator").AsString = AdjacentNetworkOperator;
                vl_params.Add("@prm_AdjacentNetworkOperatorType").AsString = AdjacentNetworkOperatorType;
                vl_params.Add("@prm_Locality").AsString = Locality;
                vl_params.Add("@prm_LowerSirutaCode").AsString = LowerSirutaCode;
                vl_params.Add("@prm_UpperSirutaCode").AsString = UpperSirutaCode;
                vl_params.Add("@prm_TerritorialBranch").AsString = TerritorialBranch;
                vl_params.Add("@prm_Sector").AsString = Sector;
                vl_params.Add("@prm_TechnologicalMinimumPressure").AsDouble = TechnologicalMinimumPressure;
                vl_params.Add("@prm_TechnologicalMinimumUM").AsString = TechnologicalMinimumUM;
                vl_params.Add("@prm_TechnologicalMaximumPressure").AsDouble = TechnologicalMaximumPressure;
                vl_params.Add("@prm_TechnologicalMaximumUM").AsString = TechnologicalMaximumUM;
                vl_params.Add("@prm_AnnualAverageGrossCalorificValue").AsDouble = AnnualAverageGrossCalorificValue;
                vl_params.Add("@prm_AnnualUM").AsString = AnnualUM;
                vl_params.Add("@prm_TechnologicalCapacity").AsDouble = TechnologicalCapacity;
                vl_params.Add("@prm_TechnologicalCapacityUM").AsString = TechnologicalCapacityUM;


                int nr = app.DB.Exec("insert_EntryPoint_RGN", "EntryPoints_RGN", vl_params);
                res.RowsModified = nr;

                if(res.RowsModified<= 0)
                    return res;

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = -1;               
                DataSet ds = app.DB.Select("select_LastEntryPoint", "EntryPoints_RGN", vl_params);

                if ((ds == null) || (ds.Tables.Count == 0) || (ds.Tables[0].Rows.Count == 0))
                {
                    res.ParamValidations["EntryPoint"].AsString = "EntryPoint could not be found ";
                    return res;
                }
               
                int idEntryPoint = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"].ToString());

                for(int i = 0; i < GNTypes.Count; i++)
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_EntryPoint").AsInt32 = idEntryPoint;
                    vl_params.Add("@prm_ID_GNType").AsInt32 = Convert.ToInt32(GNTypes[i].Value);
                    nr = app.DB.Exec("insert_EntryPointGNType_RGN", "EntryPointsXGNTypes_RGN", vl_params);
                                        
                    if (nr <= 0) throw new Exception("SQL execution error");
                }

                return res;
            }
            catch { return new TDBExecResult(TDBExecError.SQLExecutionError, "Query on database failed"); }
        }

        public TDBExecResult editEntryPoint_RGN(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_EntryPoint"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_EntryPoint");
            if (vl_arguments["PhysicalPointCode"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PhysicalPointCode");
            if (vl_arguments["PhysicalPointName"] == null) new TDBExecResult(TDBExecError.ArgumentMissing, "PhysicalPointName");
            if (vl_arguments["TechnologicalMinimumPressure"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TechnologicalMinimumPressure");
            if (vl_arguments["AnnualAverageGrossCalorificValue"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "AnnualAverageGrossCalorificValue");
            if (vl_arguments["AnnualUM"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "AnnualUM");
            if (vl_arguments["TechnologicalCapacity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TechnologicalCapacity");
            if (vl_arguments["TechnologicalCapacityUM"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TechnologicalCapacityUM");
            if (vl_arguments["ID_County"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_County");
            if (vl_arguments["GNTypes"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "GNTypes");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            //  validation
            int ID_EntryPoint = 0;
            try { ID_EntryPoint = vl_arguments["ID_EntryPoint"].AsInt32; }
            catch { res.ParamValidations["ID_EntryPoint"].AsString = "Value not recognized"; }

            int ID_County = 0;
            try { ID_County = vl_arguments["ID_County"].AsInt32; }
            catch { res.ParamValidations["ID_County"].AsString = "Value not recognized"; }

            string PhysicalPointCode = "";
            try { PhysicalPointCode = vl_arguments["PhysicalPointCode"].AsString; }
            catch { res.ParamValidations["PhysicalPointCode"].AsString = "Value not recognized"; }

            string PhysicalPointName = "";
            try { PhysicalPointName = vl_arguments["PhysicalPointName"].AsString; }
            catch { res.ParamValidations["PhysicalPointName"].AsString = "Value not recognized"; }

            string VirtualPointCode = "";
            try { VirtualPointCode = (vl_arguments["VirtualPointCode"] == null) ? "" : vl_arguments["VirtualPointCode"].AsString; }
            catch { res.ParamValidations["VirtualPointCode "].AsString = "Value not recognized"; }

            string VirtualPointName = "";
            try { VirtualPointName = (vl_arguments["VirtualPointName"] == null) ? "" : vl_arguments["VirtualPointName"].AsString; }
            catch { res.ParamValidations["VirtualPointName"].AsString = "Value not recognized"; }

            string VirtualEntryPointCode = "";
            try { VirtualEntryPointCode = (vl_arguments["VirtualEntryPointCode"] == null) ? "" : vl_arguments["VirtualEntryPointCode"].AsString; }
            catch { res.ParamValidations["VirtualEntryPointCode"].AsString = "Value not recognized"; }

            string VirtualEntryPointName = "";
            try { VirtualEntryPointName = (vl_arguments["VirtualEntryPointName"] == null) ? "" : vl_arguments["VirtualEntryPointName"].AsString; }
            catch { res.ParamValidations["VirtualEntryPointName"].AsString = "Value not recognized"; }

            string AdjacentNetworkOperator = "";
            try { AdjacentNetworkOperator = (vl_arguments["AdjacentNetworkOperator"] == null) ? "" : vl_arguments["AdjacentNetworkOperator"].AsString; }
            catch { res.ParamValidations["AdjacentNetworkOperator"].AsString = "Value not recognized"; }

            string AdjacentNetworkOperatorType = "";
            try { AdjacentNetworkOperatorType = (vl_arguments["AdjacentNetworkOperatorType"] == null) ? "" : vl_arguments["AdjacentNetworkOperatorType"].AsString; }
            catch { res.ParamValidations["AdjacentNetworkOperatorType"].AsString = "Value not recognized"; }

            string Locality = "";
            try { Locality = (vl_arguments["Locality"] == null) ? "" : vl_arguments["Locality"].AsString; }
            catch { res.ParamValidations["Locality"].AsString = "Value not recognized"; }

            string LowerSirutaCode = "";
            try { LowerSirutaCode = (vl_arguments["LowerSirutaCode"] == null) ? "" : vl_arguments["LowerSirutaCode"].AsString; }
            catch { res.ParamValidations["LowerSirutaCode"].AsString = "Value not recognized"; }

            string UpperSirutaCode = "";
            try { UpperSirutaCode = (vl_arguments["UpperSirutaCode"] == null) ? "" : vl_arguments["UpperSirutaCode"].AsString; }
            catch { res.ParamValidations["UpperSirutaCode"].AsString = "Value not recognized"; }

            string TerritorialBranch = "";
            try { TerritorialBranch = (vl_arguments["TerritorialBranch"] == null) ? "" : vl_arguments["TerritorialBranch"].AsString; }
            catch { res.ParamValidations["TerritorialBranch"].AsString = "Value not recognized"; }

            string Sector = "";
            try { Sector = (vl_arguments["Sector"] == null) ? "" : vl_arguments["Sector"].AsString; }
            catch { res.ParamValidations["Sector"].AsString = "Value not recognized"; }

            double TechnologicalMinimumPressure = 0;
            try { TechnologicalMinimumPressure = (vl_arguments["TechnologicalMinimumPressure"] == null) ? 0 : vl_arguments["TechnologicalMinimumPressure"].AsDouble; }
            catch { res.ParamValidations["TechnologicalMinimumPressure"].AsString = "Value not recognized"; }

            string TechnologicalMinimumUM = "";
            try { TechnologicalMinimumUM = (vl_arguments["TechnologicalMinimumUM"] == null) ? "" : vl_arguments["TechnologicalMinimumUM"].AsString; }
            catch { res.ParamValidations["TechnologicalMinimumUM"].AsString = "Value not recognized"; }

            double TechnologicalMaximumPressure = 0;
            try { TechnologicalMaximumPressure = (vl_arguments["TechnologicalMaximumPressure"] == null) ? 0 : vl_arguments["TechnologicalMaximumPressure"].AsDouble; }
            catch { res.ParamValidations["TechnologicalMaximumPressure"].AsString = "Value not recognized"; }

            string TechnologicalMaximumUM = "";
            try { TechnologicalMaximumUM = (vl_arguments["TechnologicalMaximumUM"] == null) ? "" : vl_arguments["TechnologicalMaximumUM"].AsString; }
            catch { res.ParamValidations["TechnologicalMaximumUM"].AsString = "Value not recognized"; }

            double AnnualAverageGrossCalorificValue = 0;
            try { AnnualAverageGrossCalorificValue = (vl_arguments["AnnualAverageGrossCalorificValue"] == null) ? 0 : vl_arguments["AnnualAverageGrossCalorificValue"].AsDouble; }
            catch { res.ParamValidations["AnnualAverageGrossCalorificValue"].AsString = "Value not recognized"; }

            string AnnualUM = "";
            try { AnnualUM = (vl_arguments["AnnualUM"] == null) ? "" : vl_arguments["AnnualUM"].AsString; }
            catch { res.ParamValidations["AnnualUM"].AsString = "Value not recognized"; }

            double TechnologicalCapacity = 0;
            try { TechnologicalCapacity = (vl_arguments["TechnologicalCapacity"] == null) ? 0 : vl_arguments["TechnologicalCapacity"].AsDouble; }
            catch { res.ParamValidations["TechnologicalCapacity"].AsString = "Value not recognized"; }

            string TechnologicalCapacityUM = "";
            try { TechnologicalCapacityUM = (vl_arguments["TechnologicalCapacityUM"] == null) ? "" : vl_arguments["TechnologicalCapacityUM"].AsString; }
            catch { res.ParamValidations["TechnologicalCapacityUM"].AsString = "Value not recognized"; }

            //added by Alex P. on 20140908
            TVariantList GNTypes = (TVariantList)vl_arguments["GNTypes"].AsObject;
            for (int i = 0; i < GNTypes.Count; i++)
            {
                int gnTypeID = Convert.ToInt32(GNTypes[i].Value);
                TVariantList vl = new TVariantList();
                vl.Add("@prm_ID").AsInt32 = gnTypeID;
                DataSet ds = app.DB.Select("select_GNType", "GNTypes", vl);

                if ((ds == null) || (ds.Tables.Count == 0) || (ds.Tables[0].Rows.Count == 0))
                {
                    res.ParamValidations["GNType"].AsString = "Innexistent GNType " + gnTypeID.ToString();
                    return res;
                }
            }
            //-----------------------------

            
            if (res.bInvalid()) return res;


            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_EntryPoint").AsInt32 = ID_EntryPoint;
                vl_params.Add("@prm_ID_County").AsInt32 = ID_County;
                vl_params.Add("@prm_PhysicalPointCode").AsString = PhysicalPointCode;
                vl_params.Add("@prm_PhysicalPointName").AsString = PhysicalPointName;
                vl_params.Add("@prm_VirtualPointCode").AsString = VirtualPointCode;
                vl_params.Add("@prm_VirtualPointName").AsString = VirtualPointName;
                vl_params.Add("@prm_VirtualEntryPointCode").AsString = VirtualEntryPointCode;
                vl_params.Add("@prm_VirtualEntryPointName").AsString = VirtualEntryPointName;
                vl_params.Add("@prm_AdjacentNetworkOperator").AsString = AdjacentNetworkOperator;
                vl_params.Add("@prm_AdjacentNetworkOperatorType").AsString = AdjacentNetworkOperatorType;
                vl_params.Add("@prm_Locality").AsString = Locality;
                vl_params.Add("@prm_LowerSirutaCode").AsString = LowerSirutaCode;
                vl_params.Add("@prm_UpperSirutaCode").AsString = UpperSirutaCode;
                vl_params.Add("@prm_TerritorialBranch").AsString = TerritorialBranch;
                vl_params.Add("@prm_Sector").AsString = Sector;
                vl_params.Add("@prm_TechnologicalMinimumPressure").AsDouble = TechnologicalMinimumPressure;
                vl_params.Add("@prm_TechnologicalMinimumUM").AsString = TechnologicalMinimumUM;
                vl_params.Add("@prm_TechnologicalMaximumPressure").AsDouble = TechnologicalMaximumPressure;
                vl_params.Add("@prm_TechnologicalMaximumUM").AsString = TechnologicalMaximumUM;
                vl_params.Add("@prm_AnnualAverageGrossCalorificValue").AsDouble = AnnualAverageGrossCalorificValue;
                vl_params.Add("@prm_AnnualUM").AsString = AnnualUM;
                vl_params.Add("@prm_TechnologicalCapacity").AsDouble = TechnologicalCapacity;
                vl_params.Add("@prm_TechnologicalCapacityUM").AsString = TechnologicalCapacityUM;


                int nr = app.DB.Exec("update_EntryPoint_RGN", "EntryPoints_RGN", vl_params);
                if (nr <= 0) throw new Exception("SQL execution error");

                res.RowsModified = nr;

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_EntryPoint").AsInt32 = ID_EntryPoint;
                vl_params.Add("@prm_ID_GNType").AsInt32 = -1;
                nr = app.DB.Exec("delete_EntryPointGNTypes", "EntryPointsXGNTypes_RGN", vl_params);                
                //if (nr <= 0) throw new Exception("SQL execution error!");
                
                for (int i = 0; i < GNTypes.Count; i++)
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_EntryPoint").AsInt32 = ID_EntryPoint;
                    vl_params.Add("@prm_ID_GNType").AsInt32 = Convert.ToInt32(GNTypes[i].Value);
                    vl_params.Add("@prm_IsDeleted").AsInt32 = -1;
                    DataSet ds = app.DB.Select("select_EntryPointGNType_RGN", "EntryPointsXGNTypes_RGN", vl_params);
                    if ((ds == null) || (ds.Tables.Count == 0) || (ds.Tables[0].Rows.Count == 0))
                    {
                        vl_params = new TVariantList();
                        vl_params.Add("@prm_ID_EntryPoint").AsInt32 = ID_EntryPoint;
                        vl_params.Add("@prm_ID_GNType").AsInt32 = Convert.ToInt32(GNTypes[i].Value);
                        nr = app.DB.Exec("insert_EntryPointGNType_RGN", "EntryPointsXGNTypes_RGN", vl_params);

                        if (nr <= 0) throw new Exception("SQL execution error");
                    }
                    else
                    {
                        vl_params = new TVariantList();
                        vl_params.Add("@prm_ID_EntryPoint").AsInt32 = ID_EntryPoint;
                        vl_params.Add("@prm_ID_GNType").AsInt32 = Convert.ToInt32(GNTypes[i].Value);
                        vl_params.Add("@prm_IsDeleted").AsInt32 = 0;
                        nr = app.DB.Exec("undelete_EntryPointGNType", "EntryPointsXGNTypes_RGN", vl_params);

                        if (nr <= 0) throw new Exception("SQL execution error");
                    }

                    //if (nr <= 0) throw new Exception("SQL execution error");
                }

                return res;
            }
            catch { return new TDBExecResult(TDBExecError.SQLExecutionError, "Query on database failed"); }
        }

        public TDBExecResult deleteEntryPoint_RGN(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_EntryPoint"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_EntryPoint");


            //  validation
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int idEntryPoint = 0;
            try { idEntryPoint = vl_arguments["ID_EntryPoint"].AsInt32; }
            catch { res.ParamValidations["ID_EntryPoint"].AsString = "Value not recognized"; }

            if (res.bInvalid()) return res;


            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_EntryPoint").AsInt32 = idEntryPoint;

                int countDeleted = app.DB.Exec("delete_EntryPoint_RGN", "EntryPoints_RGN", vl_params);
                if (countDeleted <= 0) throw new Exception("Entry Point deletion failed");

                res.RowsModified = countDeleted;
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_EntryPoint").AsInt32 = idEntryPoint;
                vl_params.Add("@prm_ID_GNType").AsInt32 = -1;
                vl_params.Add("@prm_IsDeleted").AsInt32 = 1;
                int nr = app.DB.Exec("delete_EntryPointGNType", "EntryPointsXGNTypes_RGN", vl_params);

                if (nr <= 0) throw new Exception("SQL execution error");                    

                return res;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult setUserEntryPoint(TVariantList vl_arguments)
        {
            int ID_User = session.ID_User;
            if (vl_arguments["ID_User"] != null) ID_User = vl_arguments["ID_User"].AsInt32;

            int ID_EntryPoint = 0;
            if (vl_arguments["ID_EntryPoint"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_EntryPoint");

            bool isChecked = false;
            if (vl_arguments["isChecked"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isChecked");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            if (vl_arguments["ID_EntryPoint"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_EntryPoint"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_EntryPoint"].ValueType == TVariantType.vtInt64) ID_EntryPoint = vl_arguments["ID_EntryPoint"].AsInt32;
            else res.ParamValidations["ID_EntryPoint"].AsString = "Value not recognized";

            if (vl_arguments["isChecked"].ValueType == TVariantType.vtBoolean) isChecked = vl_arguments["isChecked"].AsBoolean;
            else res.ParamValidations["isChecked"].AsString = "Unrecognized Value";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_User").AsInt32 = ID_User;
            vl_params.Add("@prm_ID_EntryPoint").AsInt32 = ID_EntryPoint;
            vl_params.Add("@prm_isChecked").AsBoolean = isChecked;
            int int_count_UsersXEntryPoints = app.DB.Exec("setUserXEntryPoint", "UsersXEntryPoints_RGN", vl_params);

            if (int_count_UsersXEntryPoints < 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            else return new TDBExecResult(TDBExecError.Success, "");
        }

        /*public TDBExecResult deleteUser(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_User"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_User");

            //  validation
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int idUser = 0;
            try { idUser = vl_arguments["ID_User"].AsInt32; }
            catch { res.ParamValidations["ID_User"].AsString = "Value not recognized"; }

            if (res.bInvalid()) return res;


            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_User").AsInt32 = idUser;

                int countDeleted = app.DB.Exec("delete_User", "Users", vl_params);
                if (countDeleted <= 0) throw new Exception("User deletion failed");

                res.RowsModified = countDeleted;
                return res;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }*/


        public TDBExecResult addAssetType(TVariantList vl_arguments)
        {            
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            //if (vl_arguments["MeasuringUnit"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MeasuringUnit");
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Ring");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            //  validation
            
            int idRing = 0;
            try { idRing = vl_arguments["ID_Ring"].AsInt32; }
            catch { res.ParamValidations["ID_Ring"].AsString = "Value not recognized"; }

            string AssetTypeCode = "";
            try { AssetTypeCode = vl_arguments["Code"].AsString; }
            catch { res.ParamValidations["Code"].AsString = "Value not recognized"; }

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                try { str_Name = vl_arguments["Name"].AsString; }
                catch { res.ParamValidations["Name"].AsString = "Value not recognized"; }
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string str_Description = "";
            if (vl_arguments["Description"] != null)
            {
                if (vl_arguments["Description"].ValueType == TVariantType.vtString) str_Description = vl_arguments["Description"].AsString;
                else res.ParamValidations["Description"].AsString = "Value not recognized";
            }

            string str_Description_EN = "";
            if (vl_arguments["Description_EN"] != null)
            {
                if (vl_arguments["Description_EN"].ValueType == TVariantType.vtString) str_Description_EN = vl_arguments["Description_EN"].AsString;
                else res.ParamValidations["Description_EN"].AsString = "Value type not recognized";
            }

            string str_Description_RO = "";
            if (vl_arguments["Description_RO"] != null)
            {
                if (vl_arguments["Description_RO"].ValueType == TVariantType.vtString) str_Description_RO = vl_arguments["Description_RO"].AsString;
                else res.ParamValidations["Description_RO"].AsString = "Value type not recognized";
            }

            int ID_Currency = 0;
            if (vl_arguments["ID_Currency"] != null)
            {
                if (vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt64) ID_Currency = vl_arguments["ID_Currency"].AsInt32;
                else res.ParamValidations["ID_Currency"].AsString = "Value not recognized";
            }

            int ID_MeasuringUnit = 0;
            if (vl_arguments["ID_MeasuringUnit"] != null)
            {
                if (vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt64) ID_MeasuringUnit = vl_arguments["ID_MeasuringUnit"].AsInt32;
                else res.ParamValidations["ID_MeasuringUnit"].AsString = "Value not recognized";
            }

            string AssetTypeUM = "";
            if (vl_arguments["MeasuringUnit"] != null)
            {
                try { AssetTypeUM = vl_arguments["MeasuringUnit"].AsString; }
                catch { res.ParamValidations["MeasuringUnit"].AsString = "Value not recognized"; }
            }
            
            if (res.bInvalid()) return res;

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Ring").AsInt32 = idRing;
                vl_params.Add("@prm_AssetTypeName").AsString = str_Name;
                vl_params.Add("@prm_AssetTypeCode").AsString = AssetTypeCode;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID_Currency").AsInt32 = ID_Currency;
                vl_params.Add("@prm_ID_MeasuringUnit").AsInt32 = ID_MeasuringUnit;
                vl_params.Add("@prm_AssetTypeUM").AsString = AssetTypeUM;

                int nr = app.DB.Exec("insert_AssetType", "AssetTypes", vl_params);
                res.RowsModified = nr;

                if (nr > 0)
                {
                    int ID_AssetType = app.DB.GetIdentity();

                    if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
                    {
                        str_Name = "fld_AssetType_Name_" + ID_AssetType.ToString();
                        vl_params = new TVariantList();
                        vl_params.Add("@prm_Label").AsString = str_Name;
                        vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                        vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                        app.DB.Exec("setTranslation", "Translations", vl_params);
                    }

                    if (str_Description == "" && (str_Description_EN != "" || str_Description_RO != ""))
                    {
                        str_Description = "fld_AssetType_Description_" + ID_AssetType.ToString();
                        vl_params = new TVariantList();
                        vl_params.Add("@prm_Label").AsString = str_Description;
                        vl_params.Add("@prm_Value_EN").AsString = str_Description_EN;
                        vl_params.Add("@prm_Value_RO").AsString = str_Description_RO;
                        app.DB.Exec("setTranslation", "Translations", vl_params);
                    }

                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Name").AsString = str_Name;
                    vl_params.Add("@prm_Description").AsString = str_Description;
                    vl_params.Add("@prm_ID").AsInt32 = ID_AssetType;
                    app.DB.Exec("update_TextFields", "AssetTypes", vl_params);

                    updateTradeParameters(ID_AssetType, 0, 0, 0, vl_arguments);
                }

                return res;
            }
            catch { return new TDBExecResult(TDBExecError.SQLExecutionError, "Query on database failed"); }
        }

        public TDBExecResult editAssetType(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_AssetType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AssetType");
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Ring");
            //if (vl_arguments["MeasuringUnit"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MeasuringUnit");
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code"); 

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            //  validation
            int ID_AssetType = 0;
            try { ID_AssetType = vl_arguments["ID_AssetType"].AsInt32; }
            catch { res.ParamValidations["ID_AssetType"].AsString = "Value not recognized"; }

            int ID_Ring = 0;
            try { ID_Ring = vl_arguments["ID_Ring"].AsInt32; }
            catch { res.ParamValidations["ID_Ring"].AsString = "Value not recognized"; }

            string AssetTypeUM = "";
            if (vl_arguments["MeasuringUnit"] != null)
            {
                try { AssetTypeUM = vl_arguments["MeasuringUnit"].AsString; }
                catch { res.ParamValidations["MeasuringUnit"].AsString = "Value not recognized"; }
            }

            string AssetTypeCode = "";
            try { AssetTypeCode = vl_arguments["Code"].AsString; }
            catch { res.ParamValidations["Code"].AsString = "Value not recognized"; }

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                try { str_Name = vl_arguments["Name"].AsString; }
                catch { res.ParamValidations["Name"].AsString = "Value not recognized"; }
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string str_Description = "";
            if (vl_arguments["Description"] != null)
            {
                if (vl_arguments["Description"].ValueType == TVariantType.vtString) str_Description = vl_arguments["Description"].AsString;
                else res.ParamValidations["Description"].AsString = "Value not recognized";
            }

            string str_Description_EN = "";
            if (vl_arguments["Description_EN"] != null)
            {
                if (vl_arguments["Description_EN"].ValueType == TVariantType.vtString) str_Description_EN = vl_arguments["Description_EN"].AsString;
                else res.ParamValidations["Description_EN"].AsString = "Value type not recognized";
            }

            string str_Description_RO = "";
            if (vl_arguments["Description_RO"] != null)
            {
                if (vl_arguments["Description_RO"].ValueType == TVariantType.vtString) str_Description_RO = vl_arguments["Description_RO"].AsString;
                else res.ParamValidations["Description_RO"].AsString = "Value type not recognized";
            }

            int ID_Currency = 0;
            if (vl_arguments["ID_Currency"] != null)
            {
                if (vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt64) ID_Currency = vl_arguments["ID_Currency"].AsInt32;
                else res.ParamValidations["ID_Currency"].AsString = "Value not recognized";
            }

            int ID_MeasuringUnit = 0;
            if (vl_arguments["ID_MeasuringUnit"] != null)
            {
                if (vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt64) ID_MeasuringUnit = vl_arguments["ID_MeasuringUnit"].AsInt32;
                else res.ParamValidations["ID_MeasuringUnit"].AsString = "Value not recognized";
            }

            if (res.bInvalid()) return res;

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_AssetType").AsInt32 = ID_AssetType;
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("@prm_AssetTypeCode").AsString = AssetTypeCode;
                vl_params.Add("@prm_AssetTypeName").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID_Currency").AsInt32 = ID_Currency;
                vl_params.Add("@prm_ID_MeasuringUnit").AsInt32 = ID_MeasuringUnit;
                vl_params.Add("@prm_AssetTypeUM").AsString = AssetTypeUM;

                int nr = app.DB.Exec("update_AssetType", "AssetTypes", vl_params);
                if (nr <= 0) throw new Exception("SQL execution error");

                res.RowsModified = nr;

                if (nr > 0)
                {
                    if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
                    {
                        str_Name = "fld_AssetType_Name_" + ID_AssetType.ToString();
                        vl_params = new TVariantList();
                        vl_params.Add("@prm_Label").AsString = str_Name;
                        vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                        vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                        app.DB.Exec("setTranslation", "Translations", vl_params);
                    }

                    if (str_Description == "" && (str_Description_EN != "" || str_Description_RO != ""))
                    {
                        str_Description = "fld_AssetType_Description_" + ID_AssetType.ToString();
                        vl_params = new TVariantList();
                        vl_params.Add("@prm_Label").AsString = str_Description;
                        vl_params.Add("@prm_Value_EN").AsString = str_Description_EN;
                        vl_params.Add("@prm_Value_RO").AsString = str_Description_RO;
                        app.DB.Exec("setTranslation", "Translations", vl_params);
                    }

                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Name").AsString = str_Name;
                    vl_params.Add("@prm_Description").AsString = str_Description;
                    vl_params.Add("@prm_ID").AsInt32 = ID_AssetType;
                    app.DB.Exec("update_TextFields", "AssetTypes", vl_params);

                    updateTradeParameters(ID_AssetType, 0, 0, 0, vl_arguments);
                }

                return res;
            }
            catch { return new TDBExecResult(TDBExecError.SQLExecutionError, "Query on database failed"); }           
        }

        public TDBExecResult deleteAssetType(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_AssetType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AssetType");

            //  validation
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int idAssetType = 0;
            try { idAssetType = vl_arguments["ID_AssetType"].AsInt32; }
            catch { res.ParamValidations["ID_AssetType"].AsString = "Value not recognized"; }

            if (res.bInvalid()) return res;


            //  check for Assets defined on this AssetType
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_AssetType").AsInt32 = idAssetType;
            DataSet ds_A = app.DB.Select("select_AssetsbyID_AssetType", "Assets", vl_params);
            if (app.DB.ValidDSRows(ds_A)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Cannot delete Asset Type while there are still Assets defined");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_AssetType").AsInt32 = idAssetType;

                int countDeleted = app.DB.Exec("delete_AssetType", "AssetTypes", vl_params);
                if (countDeleted <= 0) throw new Exception("AssetType deletion failed");

                res.RowsModified = countDeleted;
                return res;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult addAsset(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Market"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Market");
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Ring");
            if (vl_arguments["ID_AssetType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AssetType");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            //if (vl_arguments["AuctionType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "AuctionType");
            //if (vl_arguments["MeasuringUnit"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MeasuringUnit");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Market = 0;
            if (vl_arguments["ID_Market"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt64) ID_Market = vl_arguments["ID_Market"].AsInt32;
            else res.ParamValidations["ID_Market"].AsString = "Value type not recognized";

            int ID_Ring = 0;
            if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
            else res.ParamValidations["ID_Ring"].AsString = "Value type not recognized";

            int ID_AssetType = 0;
            if (vl_arguments["ID_AssetType"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt64) ID_AssetType = vl_arguments["ID_AssetType"].AsInt32;
            else res.ParamValidations["ID_AssetType"].AsString = "Value type not recognized";

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value type not recognized";

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
                else res.ParamValidations["Name"].AsString = "Value type not recognized";
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string str_Description = "";
            if (vl_arguments["Description"] != null)
            {
                if (vl_arguments["Description"].ValueType == TVariantType.vtString) str_Description = vl_arguments["Description"].AsString;
                else res.ParamValidations["Description"].AsString = "Value not recognized";
            }

            string str_Description_EN = "";
            if (vl_arguments["Description_EN"] != null)
            {
                if (vl_arguments["Description_EN"].ValueType == TVariantType.vtString) str_Description_EN = vl_arguments["Description_EN"].AsString;
                else res.ParamValidations["Description_EN"].AsString = "Value type not recognized";
            }

            string str_Description_RO = "";
            if (vl_arguments["Description_RO"] != null)
            {
                if (vl_arguments["Description_RO"].ValueType == TVariantType.vtString) str_Description_RO = vl_arguments["Description_RO"].AsString;
                else res.ParamValidations["Description_RO"].AsString = "Value type not recognized";
            }

            int ID_Currency = 0;
            if (vl_arguments["ID_Currency"] != null)
            {
                if (vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt64) ID_Currency = vl_arguments["ID_Currency"].AsInt32;
                else res.ParamValidations["ID_Currency"].AsString = "Value not recognized";
            }

            int ID_PaymentCurrency = 0;
            if (vl_arguments["ID_PaymentCurrency"] != null)
            {
                if (vl_arguments["ID_PaymentCurrency"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_PaymentCurrency"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_PaymentCurrency"].ValueType == TVariantType.vtInt64) ID_PaymentCurrency = vl_arguments["ID_PaymentCurrency"].AsInt32;
                else res.ParamValidations["ID_PaymentCurrency"].AsString = "Value not recognized";
            }

            int ID_MeasuringUnit = 0;
            if (vl_arguments["ID_MeasuringUnit"] != null)
            {
                if (vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt64) ID_MeasuringUnit = vl_arguments["ID_MeasuringUnit"].AsInt32;
                else res.ParamValidations["ID_MeasuringUnit"].AsString = "Value not recognized";
            }

            string str_MeasuringUnit = "";
            if (vl_arguments["MeasuringUnit"] != null)
            {
                if (vl_arguments["MeasuringUnit"].ValueType == TVariantType.vtString) str_MeasuringUnit = vl_arguments["MeasuringUnit"].AsString;
                else res.ParamValidations["MeasuringUnit"].AsString = "Value not recognized";
            }

            bool isSpotContract = false;
            if (vl_arguments["isSpotContract"] != null)
            {
                if (vl_arguments["isSpotContract"].ValueType == TVariantType.vtBoolean) isSpotContract = vl_arguments["isSpotContract"].AsBoolean;
                else res.ParamValidations["isSpotContract"].AsString = "Value not recognized";
            }

            double SpotQuotation = 0;
            if (vl_arguments["SpotQuotation"] != null)
            {
                if (vl_arguments["SpotQuotation"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtInt64 ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtSingle ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtDouble) SpotQuotation = vl_arguments["SpotQuotation"].AsDouble;
                else res.ParamValidations["SpotQuotation"].AsString = "Value not recognized";
            }

            string Visibility = "";
            if (vl_arguments["Visibility"] != null)
            {
                if (vl_arguments["Visibility"].ValueType == TVariantType.vtString) Visibility = vl_arguments["Visibility"].AsString;
                else res.ParamValidations["Visibility"].AsString = "Value not recognized";
            }

            if (vl_arguments["Status"] != null)
            {
                if (vl_arguments["Status"].ValueType == TVariantType.vtString) Visibility = vl_arguments["Status"].AsString;
                else res.ParamValidations["Visibility"].AsString = "Value not recognized";
            }

            int ID_Terminal = 0;
            if (vl_arguments["ID_Terminal"] != null)
            {
                if (vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt64) ID_Terminal = vl_arguments["ID_Terminal"].AsInt32;
            }

            bool bit_isActive = true;
            if (vl_arguments["isActive"] != null)
            {
                if (vl_arguments["isActive"].ValueType == TVariantType.vtBoolean) bit_isActive = vl_arguments["isActive"].AsBoolean;
                else res.ParamValidations["isActive"].AsString = "Value not recognized";
            }

            bool bit_isDefault = false;
            if (vl_arguments["isDefault"] != null)
            {
                if (vl_arguments["isDefault"].ValueType == TVariantType.vtBoolean) bit_isDefault = vl_arguments["isDefault"].AsBoolean;
                else res.ParamValidations["isDefault"].AsString = "Value type not recognized";
            }

            string Instructions_RO = "";
            if (vl_arguments["Instructions_RO"] != null)
            {
                if (vl_arguments["Instructions_RO"].ValueType == TVariantType.vtString) Instructions_RO = vl_arguments["Instructions_RO"].AsString;
                else res.ParamValidations["Instructions_RO"].AsString = "Value type not recognized";
            }

            string Instructions_EN = "";
            if (vl_arguments["Instructions_EN"] != null)
            {
                if (vl_arguments["Instructions_EN"].ValueType == TVariantType.vtString) Instructions_EN = vl_arguments["Instructions_EN"].AsString;
                else res.ParamValidations["Instructions_EN"].AsString = "Value type not recognized";
            }

            string DeliveryTerm = "";
            if (vl_arguments["DeliveryTerm"] != null)
            {
                if (vl_arguments["DeliveryTerm"].ValueType == TVariantType.vtString) DeliveryTerm = vl_arguments["DeliveryTerm"].AsString;
                else res.ParamValidations["DeliveryTerm"].AsString = "Value type not recognized";
            }

            string DeliveryConditions = "";
            if (vl_arguments["DeliveryConditions"] != null)
            {
                if (vl_arguments["DeliveryConditions"].ValueType == TVariantType.vtString) DeliveryConditions = vl_arguments["DeliveryConditions"].AsString;
                else res.ParamValidations["DeliveryConditions"].AsString = "Value type not recognized";
            }

            string PackingMethod = "";
            if (vl_arguments["PackingMethod"] != null)
            {
                if (vl_arguments["PackingMethod"].ValueType == TVariantType.vtString) PackingMethod = vl_arguments["PackingMethod"].AsString;
                else res.ParamValidations["PackingMethod"].AsString = "Value type not recognized";
            }

            string PaymentMethod = "";
            if (vl_arguments["PaymentMethod"] != null)
            {
                if (vl_arguments["PaymentMethod"].ValueType == TVariantType.vtString) PaymentMethod = vl_arguments["PaymentMethod"].AsString;
                else res.ParamValidations["PaymentMethod"].AsString = "Value type not recognized";
            }

            string ContractTerm = "";
            if (vl_arguments["ContractTerm"] != null)
            {
                if (vl_arguments["ContractTerm"].ValueType == TVariantType.vtString) ContractTerm = vl_arguments["ContractTerm"].AsString;
                else res.ParamValidations["ContractTerm"].AsString = "Value type not recognized";
            }

            string WarrantyMethod = "";
            if (vl_arguments["WarrantyMethod"] != null)
            {
                if (vl_arguments["WarrantyMethod"].ValueType == TVariantType.vtString) WarrantyMethod = vl_arguments["WarrantyMethod"].AsString;
                else res.ParamValidations["WarrantyMethod"].AsString = "Value type not recognized";
            }

            string AuctionType = "";
            if (vl_arguments["AuctionType"] != null)
            {
                if (vl_arguments["AuctionType"].ValueType == TVariantType.vtString) AuctionType = vl_arguments["AuctionType"].AsString;
                else res.ParamValidations["AuctionType"].AsString = "Value type not recognized";
            }

            DateTime AvailabilityStartDate = DateTime.Today;
            if (vl_arguments["AvailabilityStartDate"] != null)
            {
                if (vl_arguments["AvailabilityStartDate"].ValueType == TVariantType.vtDateTime) AvailabilityStartDate = vl_arguments["AvailabilityStartDate"].AsDateTime;
                else res.ParamValidations["AvailabilityStartDate"].AsString = "Value type not recognized";
            }

            DateTime AvailabilityEndDate = DateTime.Today;
            if (vl_arguments["AvailabilityEndDate"] != null)
            {
                if (vl_arguments["AvailabilityEndDate"].ValueType == TVariantType.vtDateTime) AvailabilityEndDate = vl_arguments["AvailabilityEndDate"].AsDateTime;
                else res.ParamValidations["AvailabilityEndDate"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            //  validate for unique asset code
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_ID").AsInt32 = 0;
            DataSet ds_assetcode = app.DB.Select("select_AssetsbyCode", "Assets", vl_params);
            if (app.DB.ValidDSRows(ds_assetcode)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Codul Activului nu este unic");

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs = new TVariantList();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Market").AsInt32 = ID_Market;
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("@prm_ID_AssetType").AsInt32 = ID_AssetType;
                vl_params.Add("@prm_ID_OwnerAgency").AsInt32 = session.ID_Agency;
                vl_params.Add("@prm_Code").AsString = str_Code;
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID_Currency").AsInt32 = ID_Currency;
                vl_params.Add("@prm_ID_PaymentCurrency").AsInt32 = ID_PaymentCurrency;
                vl_params.Add("@prm_ID_MeasuringUnit").AsInt32 = ID_MeasuringUnit;
                vl_params.Add("@prm_MeasuringUnit").AsString = str_MeasuringUnit;
                vl_params.Add("@prm_isSpotContract").AsBoolean = isSpotContract;
                vl_params.Add("@prm_SpotQuotation").AsDouble = SpotQuotation;
                vl_params.Add("@prm_Visibility").AsString = Visibility;
                vl_params.Add("@prm_ID_Terminal").AsInt32 = ID_Terminal;
                vl_params.Add("@prm_AuctionType").AsString = AuctionType;
                vl_params.Add("@prm_AvailabilityStartDate").AsDateTime = AvailabilityStartDate;
                vl_params.Add("@prm_AvailabilityEndDate").AsDateTime = AvailabilityEndDate;
                vl_params.Add("@prm_isActive").AsBoolean = bit_isActive;
                vl_params.Add("@prm_isDefault").AsBoolean = bit_isDefault;

                int int_count_Asset = app.DB.Exec("insert_Asset", "Assets", vl_params);
                if (int_count_Asset == -1) throw new Exception("Asset insert failed");

                int ID_Asset = 0;
                DataSet ds_Asset = app.DB.Select("select_LastAsset", "Assets", null);
                if (app.DB.ValidDSRows(ds_Asset)) ID_Asset = Convert.ToInt32(ds_Asset.Tables[0].Rows[0]["ID"]);

                if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
                {
                    str_Name = "fld_Asset_Name_" + ID_Asset.ToString();
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Label").AsString = str_Name;
                    vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                    vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                    app.DB.Exec("setTranslation", "Translations", vl_params);
                }

                if (str_Description == "" && (str_Description_EN != "" || str_Description_RO != ""))
                {
                    str_Description = "fld_Asset_Description_" + ID_Asset.ToString();
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Label").AsString = str_Description;
                    vl_params.Add("@prm_Value_EN").AsString = str_Description_EN;
                    vl_params.Add("@prm_Value_RO").AsString = str_Description_RO;
                    app.DB.Exec("setTranslation", "Translations", vl_params);
                }

                vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID").AsInt32 = ID_Asset;
                app.DB.Exec("update_TextFields", "Assets", vl_params);

                updateTradeParameters(0, ID_Asset, 0, 0, vl_arguments);

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_Instructions_RO").AsString = Instructions_RO;
                vl_params.Add("@prm_Instructions_EN").AsString = Instructions_EN;
                vl_params.Add("@prm_DeliveryTerm").AsString = DeliveryTerm;
                vl_params.Add("@prm_DeliveryConditions").AsString = DeliveryConditions;
                vl_params.Add("@prm_PackingMethod").AsString = PackingMethod;
                vl_params.Add("@prm_PaymentMethod").AsString = PaymentMethod;
                vl_params.Add("@prm_ContractTerm").AsString = ContractTerm;
                vl_params.Add("@prm_WarrantyMethod").AsString = WarrantyMethod;
                app.DB.Exec("insert_AssetDetails", "AssetDetails", vl_params);

                app.DB.CommitTransaction();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                DataSet dsRingWarrantyTypes = app.DB.Select("select_RingWarrantyType", "Rings", vl_params);
                if (app.DB.ValidDSRows(dsRingWarrantyTypes))
                {
                    for (int i = 0; i < dsRingWarrantyTypes.Tables[0].Rows.Count; i++)
                    {
                        int warrantyTypeID = Convert.ToInt32(dsRingWarrantyTypes.Tables[0].Rows[i]["ID_WarrantyType"]);
                        vl_params = new TVariantList();
                        vl_params.Add("ID_Asset").AsInt32 = ID_Asset;
                        vl_params.Add("ID_WarrantyType").AsInt32 = warrantyTypeID;
                        vl_params.Add("isDeleted").AsBoolean = false;
                        setWarrantyType2Asset(vl_params);
                    }
                }

                res.RowsModified = int_count_Asset;
                res.IDs.Add("ID_Asset").AsInt32 = ID_Asset;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult editAsset(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");
            if (vl_arguments["ID_Market"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Market");
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Ring");
            if (vl_arguments["ID_AssetType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AssetType");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            //if (vl_arguments["AuctionType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "AuctionType");
            //if (vl_arguments["MeasuringUnit"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MeasuringUnit");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Asset = 0;
            if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
            else res.ParamValidations["ID_Asset"].AsString = "Value type not recognized";

            int ID_Market = 0;
            if (vl_arguments["ID_Market"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt64) ID_Market = vl_arguments["ID_Market"].AsInt32;
            else res.ParamValidations["ID_Market"].AsString = "Value type not recognized";

            int ID_Ring = 0;
            if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
            else res.ParamValidations["ID_Ring"].AsString = "Value type not recognized";

            int ID_AssetType = 0;
            if (vl_arguments["ID_AssetType"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt64) ID_AssetType = vl_arguments["ID_AssetType"].AsInt32;
            else res.ParamValidations["ID_AssetType"].AsString = "Value type not recognized";

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value type not recognized";

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
                else res.ParamValidations["Name"].AsString = "Value type not recognized";
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string str_Description = "";
            if (vl_arguments["Description"] != null)
            {
                if (vl_arguments["Description"].ValueType == TVariantType.vtString) str_Description = vl_arguments["Description"].AsString;
                else res.ParamValidations["Description"].AsString = "Value not recognized";
            }

            string str_Description_EN = "";
            if (vl_arguments["Description_EN"] != null)
            {
                if (vl_arguments["Description_EN"].ValueType == TVariantType.vtString) str_Description_EN = vl_arguments["Description_EN"].AsString;
                else res.ParamValidations["Description_EN"].AsString = "Value type not recognized";
            }

            string str_Description_RO = "";
            if (vl_arguments["Description_RO"] != null)
            {
                if (vl_arguments["Description_RO"].ValueType == TVariantType.vtString) str_Description_RO = vl_arguments["Description_RO"].AsString;
                else res.ParamValidations["Description_RO"].AsString = "Value type not recognized";
            }

            int ID_Currency = 0;
            if (vl_arguments["ID_Currency"] != null)
            {
                if (vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt64) ID_Currency = vl_arguments["ID_Currency"].AsInt32;
                else res.ParamValidations["ID_Currency"].AsString = "Value not recognized";
            }

            int ID_PaymentCurrency = 0;
            if (vl_arguments["ID_PaymentCurrency"] != null)
            {
                if (vl_arguments["ID_PaymentCurrency"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_PaymentCurrency"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_PaymentCurrency"].ValueType == TVariantType.vtInt64) ID_PaymentCurrency = vl_arguments["ID_PaymentCurrency"].AsInt32;
                else res.ParamValidations["ID_PaymentCurrency"].AsString = "Value not recognized";
            }

            int ID_MeasuringUnit = 0;
            if (vl_arguments["ID_MeasuringUnit"] != null)
            {
                if (vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt64) ID_MeasuringUnit = vl_arguments["ID_MeasuringUnit"].AsInt32;
                else res.ParamValidations["ID_MeasuringUnit"].AsString = "Value not recognized";
            }

            string str_MeasuringUnit = "";
            if (vl_arguments["MeasuringUnit"] != null)
            {
                if (vl_arguments["MeasuringUnit"].ValueType == TVariantType.vtString) str_MeasuringUnit = vl_arguments["MeasuringUnit"].AsString;
                else res.ParamValidations["MeasuringUnit"].AsString = "Value type not recognized";
            }

            bool isSpotContract = false;
            if (vl_arguments["isSpotContract"] != null)
            {
                if (vl_arguments["isSpotContract"].ValueType == TVariantType.vtBoolean) isSpotContract = vl_arguments["isSpotContract"].AsBoolean;
                else res.ParamValidations["isSpotContract"].AsString = "Value not recognized";
            }

            double SpotQuotation = 0;
            if (vl_arguments["SpotQuotation"] != null)
            {
                if (vl_arguments["SpotQuotation"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtInt64 ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtSingle ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtDouble) SpotQuotation = vl_arguments["SpotQuotation"].AsDouble;
                else res.ParamValidations["SpotQuotation"].AsString = "Value not recognized";
            }

            string Visibility = "";
            if (vl_arguments["Visibility"] != null)
            {
                if (vl_arguments["Visibility"].ValueType == TVariantType.vtString) Visibility = vl_arguments["Visibility"].AsString;
                else res.ParamValidations["Visibility"].AsString = "Value not recognized";
            }

            if (vl_arguments["Status"] != null)
            {
                if (vl_arguments["Status"].ValueType == TVariantType.vtString) Visibility = vl_arguments["Status"].AsString;
                else res.ParamValidations["Status"].AsString = "Value not recognized";
            }

            int ID_Terminal = 0;
            if (vl_arguments["ID_Terminal"] != null)
            {
                if (vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt64) ID_Terminal = vl_arguments["ID_Terminal"].AsInt32;
            }

            bool bit_isActive = false;
            if (vl_arguments["isActive"] != null)
            {
                if (vl_arguments["isActive"].ValueType == TVariantType.vtBoolean) bit_isActive = vl_arguments["isActive"].AsBoolean;
                else res.ParamValidations["isActive"].AsString = "Value not recognized";
            }

            bool bit_isDefault = false;
            if (vl_arguments["isDefault"] != null)
            {
                if (vl_arguments["isDefault"].ValueType == TVariantType.vtBoolean) bit_isDefault = vl_arguments["isDefault"].AsBoolean;
                else res.ParamValidations["isDefault"].AsString = "Value type not recognized";
            }

            string Instructions_RO = "";
            if (vl_arguments["Instructions_RO"] != null)
            {
                if (vl_arguments["Instructions_RO"].ValueType == TVariantType.vtString) Instructions_RO = vl_arguments["Instructions_RO"].AsString;
                else res.ParamValidations["Instructions_RO"].AsString = "Value type not recognized";
            }

            string Instructions_EN = "";
            if (vl_arguments["Instructions_EN"] != null)
            {
                if (vl_arguments["Instructions_EN"].ValueType == TVariantType.vtString) Instructions_EN = vl_arguments["Instructions_EN"].AsString;
                else res.ParamValidations["Instructions_EN"].AsString = "Value type not recognized";
            }

            string DeliveryTerm = "";
            if (vl_arguments["DeliveryTerm"] != null)
            {
                if (vl_arguments["DeliveryTerm"].ValueType == TVariantType.vtString) DeliveryTerm = vl_arguments["DeliveryTerm"].AsString;
                else res.ParamValidations["DeliveryTerm"].AsString = "Value type not recognized";
            }

            string DeliveryConditions = "";
            if (vl_arguments["DeliveryConditions"] != null)
            {
                if (vl_arguments["DeliveryConditions"].ValueType == TVariantType.vtString) DeliveryConditions = vl_arguments["DeliveryConditions"].AsString;
                else res.ParamValidations["DeliveryConditions"].AsString = "Value type not recognized";
            }

            string PackingMethod = "";
            if (vl_arguments["PackingMethod"] != null)
            {
                if (vl_arguments["PackingMethod"].ValueType == TVariantType.vtString) PackingMethod = vl_arguments["PackingMethod"].AsString;
                else res.ParamValidations["PackingMethod"].AsString = "Value type not recognized";
            }

            string PaymentMethod = "";
            if (vl_arguments["PaymentMethod"] != null)
            {
                if (vl_arguments["PaymentMethod"].ValueType == TVariantType.vtString) PaymentMethod = vl_arguments["PaymentMethod"].AsString;
                else res.ParamValidations["PaymentMethod"].AsString = "Value type not recognized";
            }

            string ContractTerm = "";
            if (vl_arguments["ContractTerm"] != null)
            {
                if (vl_arguments["ContractTerm"].ValueType == TVariantType.vtString) ContractTerm = vl_arguments["ContractTerm"].AsString;
                else res.ParamValidations["ContractTerm"].AsString = "Value type not recognized";
            }

            string WarrantyMethod = "";
            if (vl_arguments["WarrantyMethod"] != null)
            {
                if (vl_arguments["WarrantyMethod"].ValueType == TVariantType.vtString) WarrantyMethod = vl_arguments["WarrantyMethod"].AsString;
                else res.ParamValidations["WarrantyMethod"].AsString = "Value type not recognized";
            }

            string AuctionType = "";
            if (vl_arguments["AuctionType"] != null)
            {
                if (vl_arguments["AuctionType"].ValueType == TVariantType.vtString) AuctionType = vl_arguments["AuctionType"].AsString;
                else res.ParamValidations["AuctionType"].AsString = "Value type not recognized";
            }

            DateTime AvailabilityStartDate = DateTime.Today;
            if (vl_arguments["AvailabilityStartDate"] != null)
            {
                if (vl_arguments["AvailabilityStartDate"].ValueType == TVariantType.vtDateTime) AvailabilityStartDate = vl_arguments["AvailabilityStartDate"].AsDateTime;
                else res.ParamValidations["AvailabilityStartDate"].AsString = "Value type not recognized";
            }

            DateTime AvailabilityEndDate = DateTime.Today;
            if (vl_arguments["AvailabilityEndDate"] != null)
            {
                if (vl_arguments["AvailabilityEndDate"].ValueType == TVariantType.vtDateTime) AvailabilityEndDate = vl_arguments["AvailabilityEndDate"].AsDateTime;
                else res.ParamValidations["AvailabilityEndDate"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            //  validate for unique asset code
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_ID").AsInt32 = ID_Asset;
            DataSet ds_assetcode = app.DB.Select("select_AssetsbyCode", "Assets", vl_params);
            if (app.DB.ValidDSRows(ds_assetcode)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Codul Activului nu este unic");

            //  validate against changing the ID_Ring when other orders are already in the market
            vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_ID_Market").AsInt32 = -1;
            vl_params.Add("@prm_ID_Ring").AsInt32 = -1;
            vl_params.Add("@prm_ID").AsInt32 = ID_Asset;
            vl_params.Add("@prm_ID_AssetType").AsInt32 = -1;
            vl_params.Add("@prm_AnyStatus").AsBoolean = true;
            vl_params.Add("@prm_All").AsBoolean = true;
            DataSet ds_Asset = getAssets(vl_params);
            if (app.DB.ValidDSRows(ds_Asset))
            {
                if (Convert.ToInt32(ds_Asset.Tables[0].Rows[0]["ID_Ring"]) != ID_Ring)
                {
                    //  here should be a stop if ID_Ring is changed and already there are more than 1 order
                }
            }

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_ID_Market").AsInt32 = ID_Market;
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("@prm_ID_AssetType").AsInt32 = ID_AssetType;
                vl_params.Add("@prm_Code").AsString = str_Code;
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID_Currency").AsInt32 = ID_Currency;
                vl_params.Add("@prm_ID_PaymentCurrency").AsInt32 = ID_PaymentCurrency;
                vl_params.Add("@prm_ID_MeasuringUnit").AsInt32 = ID_MeasuringUnit;
                vl_params.Add("@prm_MeasuringUnit").AsString = str_MeasuringUnit;
                vl_params.Add("@prm_isSpotContract").AsBoolean = isSpotContract;
                vl_params.Add("@prm_SpotQuotation").AsDouble = SpotQuotation;
                vl_params.Add("@prm_Visibility").AsString = Visibility;
                vl_params.Add("@prm_ID_Terminal").AsInt32 = ID_Terminal;
                vl_params.Add("@prm_AuctionType").AsString = AuctionType;
                vl_params.Add("@prm_AvailabilityStartDate").AsDateTime = AvailabilityStartDate;
                vl_params.Add("@prm_AvailabilityEndDate").AsDateTime = AvailabilityEndDate;
                vl_params.Add("@prm_isActive").AsBoolean = bit_isActive;
                vl_params.Add("@prm_isDefault").AsBoolean = bit_isDefault;

                int int_count_Asset = app.DB.Exec("update_Asset", "Assets", vl_params);
                if (int_count_Asset == -1) throw new Exception("Asset edit failed");

                if (vl_arguments["ID_InitialOrder"] != null)
                {
                    int ID_InitialOrder = 0;
                    if (vl_arguments["ID_InitialOrder"].ValueType == TVariantType.vtInt16 ||
                        vl_arguments["ID_InitialOrder"].ValueType == TVariantType.vtInt32 ||
                        vl_arguments["ID_InitialOrder"].ValueType == TVariantType.vtInt64) ID_InitialOrder = vl_arguments["ID_InitialOrder"].AsInt32;

                    vl_params.Add("@prm_ID_InitialOrder").AsInt32 = ID_InitialOrder;
                    app.DB.Exec("set_InitialOrder", "Assets", vl_params);
                }

                if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
                {
                    str_Name = "fld_Asset_Name_" + ID_Asset.ToString();
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Label").AsString = str_Name;
                    vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                    vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                    app.DB.Exec("setTranslation", "Translations", vl_params);
                }

                if (str_Description == "" && (str_Description_EN != "" || str_Description_RO != ""))
                {
                    str_Description = "fld_Asset_Description_" + ID_Asset.ToString();
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Label").AsString = str_Description;
                    vl_params.Add("@prm_Value_EN").AsString = str_Description_EN;
                    vl_params.Add("@prm_Value_RO").AsString = str_Description_RO;
                    app.DB.Exec("setTranslation", "Translations", vl_params);
                }

                vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID").AsInt32 = ID_Asset;
                app.DB.Exec("update_TextFields", "Assets", vl_params);

                updateTradeParameters(0, ID_Asset, 0, 0, vl_arguments);

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_Instructions_RO").AsString = Instructions_RO;
                vl_params.Add("@prm_Instructions_EN").AsString = Instructions_EN;
                vl_params.Add("@prm_DeliveryTerm").AsString = DeliveryTerm;
                vl_params.Add("@prm_DeliveryConditions").AsString = DeliveryConditions;
                vl_params.Add("@prm_PackingMethod").AsString = PackingMethod;
                vl_params.Add("@prm_PaymentMethod").AsString = PaymentMethod;
                vl_params.Add("@prm_ContractTerm").AsString = ContractTerm;
                vl_params.Add("@prm_WarrantyMethod").AsString = WarrantyMethod;
                DataSet ds_assetdetails = app.DB.Select("select_AssetDetails", "AssetDetails", vl_params);
                if (app.DB.ValidDSRows(ds_assetdetails))
                    app.DB.Exec("update_AssetDetails", "AssetDetails", vl_params);
                else
                    app.DB.Exec("insert_AssetDetails", "AssetDetails", vl_params);

                app.DB.CommitTransaction();

                res.RowsModified = int_count_Asset;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        /*public TDBExecResult addAsset(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Market"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Market");
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Ring");
            if (vl_arguments["ID_AssetType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AssetType");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            //if (vl_arguments["MeasuringUnit"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MeasuringUnit");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Market = 0;
            if (vl_arguments["ID_Market"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt64) ID_Market = vl_arguments["ID_Market"].AsInt32;
            else res.ParamValidations["ID_Market"].AsString = "Value type not recognized";

            int ID_Ring = 0;
            if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
            else res.ParamValidations["ID_Ring"].AsString = "Value type not recognized";

            int ID_AssetType = 0;
            if (vl_arguments["ID_AssetType"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt64) ID_AssetType = vl_arguments["ID_AssetType"].AsInt32;
            else res.ParamValidations["ID_AssetType"].AsString = "Value type not recognized";

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value type not recognized";

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
                else res.ParamValidations["Name"].AsString = "Value type not recognized";
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string str_Description = "";
            if (vl_arguments["Description"] != null)
            {
                if (vl_arguments["Description"].ValueType == TVariantType.vtString) str_Description = vl_arguments["Description"].AsString;
                else res.ParamValidations["Description"].AsString = "Value not recognized";
            }

            string str_Description_EN = "";
            if (vl_arguments["Description_EN"] != null)
            {
                if (vl_arguments["Description_EN"].ValueType == TVariantType.vtString) str_Description_EN = vl_arguments["Description_EN"].AsString;
                else res.ParamValidations["Description_EN"].AsString = "Value type not recognized";
            }

            string str_Description_RO = "";
            if (vl_arguments["Description_RO"] != null)
            {
                if (vl_arguments["Description_RO"].ValueType == TVariantType.vtString) str_Description_RO = vl_arguments["Description_RO"].AsString;
                else res.ParamValidations["Description_RO"].AsString = "Value type not recognized";
            }

            int ID_Currency = 0;
            if (vl_arguments["ID_Currency"] != null)
            {
                if (vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt64) ID_Currency = vl_arguments["ID_Currency"].AsInt32;
                else res.ParamValidations["ID_Currency"].AsString = "Value not recognized";
            }

            int ID_PaymentCurrency = 0;
            if (vl_arguments["ID_PaymentCurrency"] != null)
            {
                if (vl_arguments["ID_PaymentCurrency"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_PaymentCurrency"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_PaymentCurrency"].ValueType == TVariantType.vtInt64) ID_PaymentCurrency = vl_arguments["ID_PaymentCurrency"].AsInt32;
                else res.ParamValidations["ID_PaymentCurrency"].AsString = "Value not recognized";
            }

            int ID_MeasuringUnit = 0;
            if (vl_arguments["ID_MeasuringUnit"] != null)
            {
                if (vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt64) ID_MeasuringUnit = vl_arguments["ID_MeasuringUnit"].AsInt32;
                else res.ParamValidations["ID_MeasuringUnit"].AsString = "Value not recognized";
            }

            string str_MeasuringUnit = "";
            if (vl_arguments["MeasuringUnit"] != null)
            {
                if (vl_arguments["MeasuringUnit"].ValueType == TVariantType.vtString) str_MeasuringUnit = vl_arguments["MeasuringUnit"].AsString;
                else res.ParamValidations["MeasuringUnit"].AsString = "Value not recognized";
            }

            bool isSpotContract = false;
            if (vl_arguments["isSpotContract"] != null)
            {
                if (vl_arguments["isSpotContract"].ValueType == TVariantType.vtBoolean) isSpotContract = vl_arguments["isSpotContract"].AsBoolean;
                else res.ParamValidations["isSpotContract"].AsString = "Value not recognized";
            }

            double SpotQuotation = 0;
            if (vl_arguments["SpotQuotation"] != null)
            {
                if (vl_arguments["SpotQuotation"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtInt64 ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtSingle ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtDouble) SpotQuotation = vl_arguments["SpotQuotation"].AsDouble;
                else res.ParamValidations["SpotQuotation"].AsString = "Value not recognized";
            }

            string Visibility = "";
            if (vl_arguments["Visibility"] != null)
            {
                if (vl_arguments["Visibility"].ValueType == TVariantType.vtString) Visibility = vl_arguments["Visibility"].AsString;
                else res.ParamValidations["Visibility"].AsString = "Value not recognized";
            }

            if (vl_arguments["Status"] != null)
            {
                if (vl_arguments["Status"].ValueType == TVariantType.vtString) Visibility = vl_arguments["Status"].AsString;
                else res.ParamValidations["Visibility"].AsString = "Value not recognized";
            }

            bool bit_isActive = true;
            if (vl_arguments["isActive"] != null)
            {
                if (vl_arguments["isActive"].ValueType == TVariantType.vtBoolean) bit_isActive = vl_arguments["isActive"].AsBoolean;
                else res.ParamValidations["isActive"].AsString = "Value not recognized";
            }

            bool bit_isDefault = false;
            if (vl_arguments["isDefault"] != null)
            {
                if (vl_arguments["isDefault"].ValueType == TVariantType.vtBoolean) bit_isDefault = vl_arguments["isDefault"].AsBoolean;
                else res.ParamValidations["isDefault"].AsString = "Value type not recognized";
            }

            string Instructions_RO = "";
            if (vl_arguments["Instructions_RO"] != null)
            {
                if (vl_arguments["Instructions_RO"].ValueType == TVariantType.vtString) Instructions_RO = vl_arguments["Instructions_RO"].AsString;
                else res.ParamValidations["Instructions_RO"].AsString = "Value type not recognized";
            }

            string Instructions_EN = "";
            if (vl_arguments["Instructions_EN"] != null)
            {
                if (vl_arguments["Instructions_EN"].ValueType == TVariantType.vtString) Instructions_EN = vl_arguments["Instructions_EN"].AsString;
                else res.ParamValidations["Instructions_EN"].AsString = "Value type not recognized";
            }

            string DeliveryTerm = "";
            if (vl_arguments["DeliveryTerm"] != null)
            {
                if (vl_arguments["DeliveryTerm"].ValueType == TVariantType.vtString) DeliveryTerm = vl_arguments["DeliveryTerm"].AsString;
                else res.ParamValidations["DeliveryTerm"].AsString = "Value type not recognized";
            }

            string DeliveryConditions = "";
            if (vl_arguments["DeliveryConditions"] != null)
            {
                if (vl_arguments["DeliveryConditions"].ValueType == TVariantType.vtString) DeliveryConditions = vl_arguments["DeliveryConditions"].AsString;
                else res.ParamValidations["DeliveryConditions"].AsString = "Value type not recognized";
            }

            string PackingMethod = "";
            if (vl_arguments["PackingMethod"] != null)
            {
                if (vl_arguments["PackingMethod"].ValueType == TVariantType.vtString) PackingMethod = vl_arguments["PackingMethod"].AsString;
                else res.ParamValidations["PackingMethod"].AsString = "Value type not recognized";
            }

            string PaymentMethod = "";
            if (vl_arguments["PaymentMethod"] != null)
            {
                if (vl_arguments["PaymentMethod"].ValueType == TVariantType.vtString) PaymentMethod = vl_arguments["PaymentMethod"].AsString;
                else res.ParamValidations["PaymentMethod"].AsString = "Value type not recognized";
            }

            string ContractTerm = "";
            if (vl_arguments["ContractTerm"] != null)
            {
                if (vl_arguments["ContractTerm"].ValueType == TVariantType.vtString) ContractTerm = vl_arguments["ContractTerm"].AsString;
                else res.ParamValidations["ContractTerm"].AsString = "Value type not recognized";
            }

            string WarrantyMethod = "";
            if (vl_arguments["WarrantyMethod"] != null)
            {
                if (vl_arguments["WarrantyMethod"].ValueType == TVariantType.vtString) WarrantyMethod = vl_arguments["WarrantyMethod"].AsString;
                else res.ParamValidations["WarrantyMethod"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            //  validate for unique asset code
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_ID").AsInt32 = 0;
            DataSet ds_assetcode = app.DB.Select("select_AssetsbyCode", "Assets", vl_params);
            if (app.DB.ValidDSRows(ds_assetcode)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Codul Activului nu este unic");

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs = new TVariantList();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Market").AsInt32 = ID_Market;
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("@prm_ID_AssetType").AsInt32 = ID_AssetType;
                vl_params.Add("@prm_Code").AsString = str_Code;
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID_Currency").AsInt32 = ID_Currency;
                vl_params.Add("@prm_ID_PaymentCurrency").AsInt32 = ID_PaymentCurrency;
                vl_params.Add("@prm_ID_MeasuringUnit").AsInt32 = ID_MeasuringUnit;
                vl_params.Add("@prm_MeasuringUnit").AsString = str_MeasuringUnit;
                vl_params.Add("@prm_isSpotContract").AsBoolean = isSpotContract;
                vl_params.Add("@prm_SpotQuotation").AsDouble = SpotQuotation;
                vl_params.Add("@prm_Visibility").AsString = Visibility;
                vl_params.Add("@prm_isActive").AsBoolean = bit_isActive;
                vl_params.Add("@prm_isDefault").AsBoolean = bit_isDefault;

                int int_count_Asset = app.DB.Exec("insert_Asset", "Assets", vl_params);
                if (int_count_Asset == -1) throw new Exception("Asset insert failed");

                int ID_Asset = 0;
                DataSet ds_Asset = app.DB.Select("select_LastAsset", "Assets", null);
                if (app.DB.ValidDSRows(ds_Asset)) ID_Asset = Convert.ToInt32(ds_Asset.Tables[0].Rows[0]["ID"]);

                if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
                {
                    str_Name = "fld_Asset_Name_" + ID_Asset.ToString();
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Label").AsString = str_Name;
                    vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                    vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                    app.DB.Exec("setTranslation", "Translations", vl_params);
                }

                if (str_Description == "" && (str_Description_EN != "" || str_Description_RO != ""))
                {
                    str_Description = "fld_Asset_Description_" + ID_Asset.ToString();
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Label").AsString = str_Description;
                    vl_params.Add("@prm_Value_EN").AsString = str_Description_EN;
                    vl_params.Add("@prm_Value_RO").AsString = str_Description_RO;
                    app.DB.Exec("setTranslation", "Translations", vl_params);
                }

                vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID").AsInt32 = ID_Asset;
                app.DB.Exec("update_TextFields", "Assets", vl_params);

                updateTradeParameters(0, ID_Asset, 0, 0, vl_arguments);

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_Instructions_RO").AsString = Instructions_RO;
                vl_params.Add("@prm_Instructions_EN").AsString = Instructions_EN;
                vl_params.Add("@prm_DeliveryTerm").AsString = DeliveryTerm;
                vl_params.Add("@prm_DeliveryConditions").AsString = DeliveryConditions;
                vl_params.Add("@prm_PackingMethod").AsString = PackingMethod;
                vl_params.Add("@prm_PaymentMethod").AsString = PaymentMethod;
                vl_params.Add("@prm_ContractTerm").AsString = ContractTerm;
                vl_params.Add("@prm_WarrantyMethod").AsString = WarrantyMethod;
                app.DB.Exec("insert_AssetDetails", "AssetDetails", vl_params);

                app.DB.CommitTransaction();

                res.RowsModified = int_count_Asset;
                res.IDs.Add("ID_Asset").AsInt32 = ID_Asset;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult editAsset(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");
            if (vl_arguments["ID_Market"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Market");
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Ring");
            if (vl_arguments["ID_AssetType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AssetType");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            //if (vl_arguments["MeasuringUnit"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MeasuringUnit");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Asset = 0;
            if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
            else res.ParamValidations["ID_Asset"].AsString = "Value type not recognized";

            int ID_Market = 0;
            if (vl_arguments["ID_Market"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt64) ID_Market = vl_arguments["ID_Market"].AsInt32;
            else res.ParamValidations["ID_Market"].AsString = "Value type not recognized";

            int ID_Ring = 0;
            if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
            else res.ParamValidations["ID_Ring"].AsString = "Value type not recognized";

            int ID_AssetType = 0;
            if (vl_arguments["ID_AssetType"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_AssetType"].ValueType == TVariantType.vtInt64) ID_AssetType = vl_arguments["ID_AssetType"].AsInt32;
            else res.ParamValidations["ID_AssetType"].AsString = "Value type not recognized";

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value type not recognized";

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
                else res.ParamValidations["Name"].AsString = "Value type not recognized";
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string str_Description = "";
            if (vl_arguments["Description"] != null)
            {
                if (vl_arguments["Description"].ValueType == TVariantType.vtString) str_Description = vl_arguments["Description"].AsString;
                else res.ParamValidations["Description"].AsString = "Value not recognized";
            }

            string str_Description_EN = "";
            if (vl_arguments["Description_EN"] != null)
            {
                if (vl_arguments["Description_EN"].ValueType == TVariantType.vtString) str_Description_EN = vl_arguments["Description_EN"].AsString;
                else res.ParamValidations["Description_EN"].AsString = "Value type not recognized";
            }

            string str_Description_RO = "";
            if (vl_arguments["Description_RO"] != null)
            {
                if (vl_arguments["Description_RO"].ValueType == TVariantType.vtString) str_Description_RO = vl_arguments["Description_RO"].AsString;
                else res.ParamValidations["Description_RO"].AsString = "Value type not recognized";
            }

            int ID_Currency = 0;
            if (vl_arguments["ID_Currency"] != null)
            {
                if (vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt64) ID_Currency = vl_arguments["ID_Currency"].AsInt32;
                else res.ParamValidations["ID_Currency"].AsString = "Value not recognized";
            }

            int ID_PaymentCurrency = 0;
            if (vl_arguments["ID_PaymentCurrency"] != null)
            {
                if (vl_arguments["ID_PaymentCurrency"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_PaymentCurrency"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_PaymentCurrency"].ValueType == TVariantType.vtInt64) ID_PaymentCurrency = vl_arguments["ID_PaymentCurrency"].AsInt32;
                else res.ParamValidations["ID_PaymentCurrency"].AsString = "Value not recognized";
            }

            int ID_MeasuringUnit = 0;
            if (vl_arguments["ID_MeasuringUnit"] != null)
            {
                if (vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt64) ID_MeasuringUnit = vl_arguments["ID_MeasuringUnit"].AsInt32;
                else res.ParamValidations["ID_MeasuringUnit"].AsString = "Value not recognized";
            }

            string str_MeasuringUnit = "";
            if (vl_arguments["MeasuringUnit"] != null)
            {
                if (vl_arguments["MeasuringUnit"].ValueType == TVariantType.vtString) str_MeasuringUnit = vl_arguments["MeasuringUnit"].AsString;
                else res.ParamValidations["MeasuringUnit"].AsString = "Value type not recognized";
            }

            bool isSpotContract = false;
            if (vl_arguments["isSpotContract"] != null)
            {
                if (vl_arguments["isSpotContract"].ValueType == TVariantType.vtBoolean) isSpotContract = vl_arguments["isSpotContract"].AsBoolean;
                else res.ParamValidations["isSpotContract"].AsString = "Value not recognized";
            }

            double SpotQuotation = 0;
            if (vl_arguments["SpotQuotation"] != null)
            {
                if (vl_arguments["SpotQuotation"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtInt64 ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtSingle ||
                    vl_arguments["SpotQuotation"].ValueType == TVariantType.vtDouble) SpotQuotation = vl_arguments["SpotQuotation"].AsDouble;
                else res.ParamValidations["SpotQuotation"].AsString = "Value not recognized";
            }

            string Visibility = "";
            if (vl_arguments["Visibility"] != null)
            {
                if (vl_arguments["Visibility"].ValueType == TVariantType.vtString) Visibility = vl_arguments["Visibility"].AsString;
                else res.ParamValidations["Visibility"].AsString = "Value not recognized";
            }

            if (vl_arguments["Status"] != null)
            {
                if (vl_arguments["Status"].ValueType == TVariantType.vtString) Visibility = vl_arguments["Status"].AsString;
                else res.ParamValidations["Status"].AsString = "Value not recognized";
            }

            bool bit_isActive = false;
            if (vl_arguments["isActive"] != null)
            {
                if (vl_arguments["isActive"].ValueType == TVariantType.vtBoolean) bit_isActive = vl_arguments["isActive"].AsBoolean;
                else res.ParamValidations["isActive"].AsString = "Value not recognized";
            }

            bool bit_isDefault = false;
            if (vl_arguments["isDefault"] != null)
            {
                if (vl_arguments["isDefault"].ValueType == TVariantType.vtBoolean) bit_isDefault = vl_arguments["isDefault"].AsBoolean;
                else res.ParamValidations["isDefault"].AsString = "Value type not recognized";
            }

            string Instructions_RO = "";
            if (vl_arguments["Instructions_RO"] != null)
            {
                if (vl_arguments["Instructions_RO"].ValueType == TVariantType.vtString) Instructions_RO = vl_arguments["Instructions_RO"].AsString;
                else res.ParamValidations["Instructions_RO"].AsString = "Value type not recognized";
            }

            string Instructions_EN = "";
            if (vl_arguments["Instructions_EN"] != null)
            {
                if (vl_arguments["Instructions_EN"].ValueType == TVariantType.vtString) Instructions_EN = vl_arguments["Instructions_EN"].AsString;
                else res.ParamValidations["Instructions_EN"].AsString = "Value type not recognized";
            }

            string DeliveryTerm = "";
            if (vl_arguments["DeliveryTerm"] != null)
            {
                if (vl_arguments["DeliveryTerm"].ValueType == TVariantType.vtString) DeliveryTerm = vl_arguments["DeliveryTerm"].AsString;
                else res.ParamValidations["DeliveryTerm"].AsString = "Value type not recognized";
            }

            string DeliveryConditions = "";
            if (vl_arguments["DeliveryConditions"] != null)
            {
                if (vl_arguments["DeliveryConditions"].ValueType == TVariantType.vtString) DeliveryConditions = vl_arguments["DeliveryConditions"].AsString;
                else res.ParamValidations["DeliveryConditions"].AsString = "Value type not recognized";
            }

            string PackingMethod = "";
            if (vl_arguments["PackingMethod"] != null)
            {
                if (vl_arguments["PackingMethod"].ValueType == TVariantType.vtString) PackingMethod = vl_arguments["PackingMethod"].AsString;
                else res.ParamValidations["PackingMethod"].AsString = "Value type not recognized";
            }

            string PaymentMethod = "";
            if (vl_arguments["PaymentMethod"] != null)
            {
                if (vl_arguments["PaymentMethod"].ValueType == TVariantType.vtString) PaymentMethod = vl_arguments["PaymentMethod"].AsString;
                else res.ParamValidations["PaymentMethod"].AsString = "Value type not recognized";
            }

            string ContractTerm = "";
            if (vl_arguments["ContractTerm"] != null)
            {
                if (vl_arguments["ContractTerm"].ValueType == TVariantType.vtString) ContractTerm = vl_arguments["ContractTerm"].AsString;
                else res.ParamValidations["ContractTerm"].AsString = "Value type not recognized";
            }

            string WarrantyMethod = "";
            if (vl_arguments["WarrantyMethod"] != null)
            {
                if (vl_arguments["WarrantyMethod"].ValueType == TVariantType.vtString) WarrantyMethod = vl_arguments["WarrantyMethod"].AsString;
                else res.ParamValidations["WarrantyMethod"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            //  validate for unique asset code
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_ID").AsInt32 = ID_Asset;
            DataSet ds_assetcode = app.DB.Select("select_AssetsbyCode", "Assets", vl_params);
            if (app.DB.ValidDSRows(ds_assetcode)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Codul Activului nu este unic");

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_ID_Market").AsInt32 = ID_Market;
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("@prm_ID_AssetType").AsInt32 = ID_AssetType;
                vl_params.Add("@prm_Code").AsString = str_Code;
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID_Currency").AsInt32 = ID_Currency;
                vl_params.Add("@prm_ID_PaymentCurrency").AsInt32 = ID_PaymentCurrency;
                vl_params.Add("@prm_ID_MeasuringUnit").AsInt32 = ID_MeasuringUnit;
                vl_params.Add("@prm_MeasuringUnit").AsString = str_MeasuringUnit;
                vl_params.Add("@prm_isSpotContract").AsBoolean = isSpotContract;
                vl_params.Add("@prm_SpotQuotation").AsDouble = SpotQuotation;
                vl_params.Add("@prm_Visibility").AsString = Visibility;
                vl_params.Add("@prm_isActive").AsBoolean = bit_isActive;
                vl_params.Add("@prm_isDefault").AsBoolean = bit_isDefault;

                int int_count_Asset = app.DB.Exec("update_Asset", "Assets", vl_params);
                if (int_count_Asset == -1) throw new Exception("Asset edit failed");

                if (vl_arguments["ID_InitialOrder"] != null)
                {
                    int ID_InitialOrder = 0;
                    if (vl_arguments["ID_InitialOrder"].ValueType == TVariantType.vtInt16 ||
                        vl_arguments["ID_InitialOrder"].ValueType == TVariantType.vtInt32 ||
                        vl_arguments["ID_InitialOrder"].ValueType == TVariantType.vtInt64) ID_InitialOrder = vl_arguments["ID_InitialOrder"].AsInt32;

                    vl_params.Add("@prm_ID_InitialOrder").AsInt32 = ID_InitialOrder;
                    app.DB.Exec("set_InitialOrder", "Assets", vl_params);
                }

                if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
                {
                    str_Name = "fld_Asset_Name_" + ID_Asset.ToString();
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Label").AsString = str_Name;
                    vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                    vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                    app.DB.Exec("setTranslation", "Translations", vl_params);
                }

                if (str_Description == "" && (str_Description_EN != "" || str_Description_RO != ""))
                {
                    str_Description = "fld_Asset_Description_" + ID_Asset.ToString();
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Label").AsString = str_Description;
                    vl_params.Add("@prm_Value_EN").AsString = str_Description_EN;
                    vl_params.Add("@prm_Value_RO").AsString = str_Description_RO;
                    app.DB.Exec("setTranslation", "Translations", vl_params);
                }

                vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID").AsInt32 = ID_Asset;
                app.DB.Exec("update_TextFields", "Assets", vl_params);

                updateTradeParameters(0, ID_Asset, 0, 0, vl_arguments);

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_Instructions_RO").AsString = Instructions_RO;
                vl_params.Add("@prm_Instructions_EN").AsString = Instructions_EN;
                vl_params.Add("@prm_DeliveryTerm").AsString = DeliveryTerm;
                vl_params.Add("@prm_DeliveryConditions").AsString = DeliveryConditions;
                vl_params.Add("@prm_PackingMethod").AsString = PackingMethod;
                vl_params.Add("@prm_PaymentMethod").AsString = PaymentMethod;
                vl_params.Add("@prm_ContractTerm").AsString = ContractTerm;
                vl_params.Add("@prm_WarrantyMethod").AsString = WarrantyMethod;
                DataSet ds_assetdetails = app.DB.Select("select_AssetDetails", "AssetDetails", vl_params);
                if (app.DB.ValidDSRows(ds_assetdetails))
                    app.DB.Exec("update_AssetDetails", "AssetDetails", vl_params);
                else
                    app.DB.Exec("insert_AssetDetails", "AssetDetails", vl_params);

                app.DB.CommitTransaction();

                res.RowsModified = int_count_Asset;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }
        */        

        public TDBExecResult deleteAsset(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");

            //  validation
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int idAsset = 0;
            try { idAsset = vl_arguments["ID_Asset"].AsInt32; }
            catch { res.ParamValidations["ID_Asset"].AsString = "Value not recognized"; }

            if (res.bInvalid()) return res;


            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Asset").AsInt32 = idAsset;

                int countDeleted = app.DB.Exec("delete_Asset", "Assets", vl_params);
                if (countDeleted <= 0) throw new Exception("Asset deletion failed");

                res.RowsModified = countDeleted;
                return res;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult setAssetTradeParameters(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");
            int ID_Asset = vl_arguments["ID_Asset"].AsInt32;

            if (vl_arguments["DeltaT"] != null)
            {
                if (vl_arguments["DeltaT"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["DeltaT"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["DeltaT"].ValueType == TVariantType.vtInt64)
                    if (vl_arguments["DeltaT"].AsInt32 < 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Delta T trebuie sa fie mai mare sau egal ca 0");
            }

            if (vl_arguments["DeltaT1"] != null)
            {
                if (vl_arguments["DeltaT1"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["DeltaT1"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["DeltaT1"].ValueType == TVariantType.vtInt64)
                    if (vl_arguments["DeltaT1"].AsInt32 < 0) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Delta T1 trebuie sa fie mai mare sau egal ca 0");
            }

            updateTradeParameters(0, ID_Asset, 0, 0, vl_arguments);
            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult addRing(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Market"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Market");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["hasSchedule"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "hasSchedule");

            if (vl_arguments["MinQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MinQuantity");
            if (vl_arguments["MaxPriceVariation"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MaxPriceVariation");
            if (vl_arguments["QuantityStepping"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "QuantityStepping");
            if (vl_arguments["PriceStepping"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PriceStepping");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Market = 0;
            if (vl_arguments["ID_Market"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt64) ID_Market = vl_arguments["ID_Market"].AsInt32;
            else res.ParamValidations["ID_Market"].AsString = "Value type not recognized";

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value type not recognized";

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
                else res.ParamValidations["Name"].AsString = "Value type not recognized";
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string str_Description = "";
            if (vl_arguments["Description"] != null)
            {
                if (vl_arguments["Description"].ValueType == TVariantType.vtString) str_Description = vl_arguments["Description"].AsString;
                else res.ParamValidations["Description"].AsString = "Value type not recognized";
            }

            string str_Description_EN = "";
            if (vl_arguments["Description_EN"] != null)
            {
                if (vl_arguments["Description_EN"].ValueType == TVariantType.vtString) str_Description_EN = vl_arguments["Description_EN"].AsString;
                else res.ParamValidations["Description_EN"].AsString = "Value type not recognized";
            }

            string str_Description_RO = "";
            if (vl_arguments["Description_RO"] != null)
            {
                if (vl_arguments["Description_RO"].ValueType == TVariantType.vtString) str_Description_RO = vl_arguments["Description_RO"].AsString;
                else res.ParamValidations["Description_RO"].AsString = "Value type not recognized";
            }

            bool hasSchedule = false;
            if (vl_arguments["hasSchedule"].ValueType == TVariantType.vtBoolean) hasSchedule = vl_arguments["hasSchedule"].AsBoolean;
            else res.ParamValidations["hasSchedule"].AsString = "Value type not recognized";

            DateTime dt_StartDate = DateTime.Today;
            DateTime dt_EndDate = DateTime.Today;
            byte DaysOfWeek = 0;
            TimeSpan dt_PreOpeningTime = TimeSpan.Zero;
            TimeSpan dt_OpeningTime = TimeSpan.Zero;
            TimeSpan dt_PreClosingTime = new TimeSpan(23, 59, 0);
            TimeSpan dt_ClosingTime = new TimeSpan(23, 59, 0);

            if (hasSchedule)
            {
                if (vl_arguments["StartDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StartDate");
                if (vl_arguments["EndDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "EndDate");
                if (vl_arguments["DaysOfWeek"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "DaysOfWeek");
                if (vl_arguments["PreOpeningTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PreOpeningTime");
                if (vl_arguments["OpeningTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PreOpeningTime");
                if (vl_arguments["PreClosingTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PreClosingTime");
                if (vl_arguments["ClosingTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ClosingTime");

                if (vl_arguments["StartDate"].ValueType == TVariantType.vtDateTime) dt_StartDate = vl_arguments["StartDate"].AsDateTime;
                else res.ParamValidations["StartDate"].AsString = "Value not recognized";

                if (vl_arguments["EndDate"].ValueType == TVariantType.vtDateTime) dt_EndDate = vl_arguments["EndDate"].AsDateTime;
                else res.ParamValidations["EndDate"].AsString = "Value not recognized";

                if (vl_arguments["DaysOfWeek"].ValueType == TVariantType.vtObject && vl_arguments["DaysOfWeek"].AsObject is TVariantList)
                {
                    DaysOfWeek = 0;
                    TVariantList vl_DaysOfWeek = vl_arguments["DaysOfWeek"].AsObject as TVariantList;
                    for (int i = 0; i < vl_DaysOfWeek.Count; i++)
                    {
                        switch (vl_DaysOfWeek[i].Name.Trim().ToUpper())
                        {
                            case "DAYSUNDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 1;
                                break;
                            case "DAYMONDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 2;
                                break;
                            case "DAYTUESDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 4;
                                break;
                            case "DAYWEDNESDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 8;
                                break;
                            case "DAYTHURSDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 16;
                                break;
                            case "DAYFRIDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 32;
                                break;
                            case "DAYSATURDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 64;
                                break;
                        }
                    }
                }
                else res.ParamValidations["DaysOfWeek"].AsString = "Value not recognized";

                if (vl_arguments["PreOpeningTime"].ValueType == TVariantType.vtDateTime) dt_PreOpeningTime = vl_arguments["PreOpeningTime"].AsDateTime.TimeOfDay;
                else
                {
                    try { dt_PreOpeningTime = Convert.ToDateTime(vl_arguments["PreOpeningTime"].AsString).TimeOfDay; }
                    catch { res.ParamValidations["PreOpeningTime"].AsString = "Value not recognized"; }
                }

                if (vl_arguments["OpeningTime"].ValueType == TVariantType.vtDateTime) dt_OpeningTime = vl_arguments["OpeningTime"].AsDateTime.TimeOfDay;
                else
                {
                    try { dt_OpeningTime = Convert.ToDateTime(vl_arguments["OpeningTime"].AsString).TimeOfDay; }
                    catch { res.ParamValidations["OpeningTime"].AsString = "Value not recognized"; }
                }

                if (vl_arguments["PreClosingTime"].ValueType == TVariantType.vtDateTime) dt_PreClosingTime = vl_arguments["PreClosingTime"].AsDateTime.TimeOfDay;
                else
                {
                    try { dt_PreClosingTime = Convert.ToDateTime(vl_arguments["PreClosingTime"].AsString).TimeOfDay; }
                    catch { res.ParamValidations["PreClosingTime"].AsString = "Value not recognized"; }
                }

                if (vl_arguments["ClosingTime"].ValueType == TVariantType.vtDateTime) dt_ClosingTime = vl_arguments["ClosingTime"].AsDateTime.TimeOfDay;
                else
                {
                    try { dt_ClosingTime = Convert.ToDateTime(vl_arguments["ClosingTime"].AsString).TimeOfDay; }
                    catch { res.ParamValidations["ClosingTime"].AsString = "Value not recognized"; }
                }
            }

            double flt_MinQuantity = 0;
            try { flt_MinQuantity = vl_arguments["MinQuantity"].AsDouble; }
            catch { res.ParamValidations["MinQuantity"].AsString = "Value not recognized"; }

            double flt_MaxPriceVariation = 0;
            try { flt_MaxPriceVariation = vl_arguments["MaxPriceVariation"].AsDouble; }
            catch { res.ParamValidations["MaxPriceVariation"].AsString = "Value not recognized"; }

            double flt_QuantityStepping = 0;
            try { flt_QuantityStepping = vl_arguments["QuantityStepping"].AsDouble; }
            catch { res.ParamValidations["QuantityStepping"].AsString = "Value not recognized"; }

            double flt_PriceStepping = 0;
            try { flt_PriceStepping = vl_arguments["PriceStepping"].AsDouble; }
            catch { res.ParamValidations["PriceStepping"].AsString = "Value not recognized"; }

            bool bit_isDefault = false;
            if (vl_arguments["isDefault"] != null)
            {
                if (vl_arguments["isDefault"].ValueType == TVariantType.vtBoolean) bit_isDefault = vl_arguments["isDefault"].AsBoolean;
                else res.ParamValidations["isDefault"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs = new TVariantList();

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Market").AsInt32 = ID_Market;
                vl_params.Add("@prm_Code").AsString = str_Code;
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_hasSchedule").AsBoolean = hasSchedule;
                vl_params.Add("@prm_StartDate").AsDateTime = dt_StartDate;
                vl_params.Add("@prm_EndDate").AsDateTime = dt_EndDate;
                vl_params.Add("@prm_DaysOfWeek").AsByte = DaysOfWeek;
                vl_params.Add("@prm_PreOpeningTime").AsDateTime = DateTime.Today + dt_PreOpeningTime;
                vl_params.Add("@prm_OpeningTime").AsDateTime = DateTime.Today + dt_OpeningTime;
                vl_params.Add("@prm_PreClosingTime").AsDateTime = DateTime.Today + dt_PreClosingTime;
                vl_params.Add("@prm_ClosingTime").AsDateTime = DateTime.Today + dt_ClosingTime;
                vl_params.Add("@prm_MinQuantity").AsDouble = flt_MinQuantity;
                vl_params.Add("@prm_MaxPriceVariation").AsDouble = flt_MaxPriceVariation;
                vl_params.Add("@prm_QuantityStepping").AsDouble = flt_QuantityStepping;
                vl_params.Add("@prm_PriceStepping").AsDouble = flt_PriceStepping;
                vl_params.Add("@prm_isActive").AsBoolean = true;
                vl_params.Add("@prm_isDefault").AsBoolean = bit_isDefault;

                int int_count_Rings = app.DB.Exec("insert_Ring", "Rings", vl_params);
                if (int_count_Rings == -1) throw new Exception("Ring insert failed");

                int ID_Ring = 0;
                DataSet ds_ring = app.DB.Select("select_Latest", "Rings", null);
                if (app.DB.ValidDSRows(ds_ring)) ID_Ring = Convert.ToInt32(ds_ring.Tables[0].Rows[0]["ID"]);

                if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
                {
                    str_Name = "fld_Ring_Name_" + ID_Ring.ToString();
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Label").AsString = str_Name;
                    vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                    vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                    app.DB.Exec("setTranslation", "Translations", vl_params);
                }

                if (str_Description == "" && (str_Description_EN != "" || str_Description_RO != ""))
                {
                    str_Description = "fld_Ring_Description_" + ID_Ring.ToString();
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Label").AsString = str_Description;
                    vl_params.Add("@prm_Value_EN").AsString = str_Description_EN;
                    vl_params.Add("@prm_Value_RO").AsString = str_Description_RO;
                    app.DB.Exec("setTranslation", "Translations", vl_params);
                }

                vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID").AsInt32 = ID_Ring;
                app.DB.Exec("update_TextFields", "Rings", vl_params);

                app.DB.CommitTransaction();

                res.RowsModified = int_count_Rings;
                res.IDs.Add("ID_Ring").AsInt32 = ID_Ring;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult editRing(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Ring");
            if (vl_arguments["ID_Market"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Market");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["hasSchedule"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "hasSchedule");

            if (vl_arguments["MinQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MinQuantity");
            if (vl_arguments["MaxPriceVariation"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MaxPriceVariation");
            if (vl_arguments["QuantityStepping"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "QuantityStepping");
            if (vl_arguments["PriceStepping"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PriceStepping");
            if (vl_arguments["isActive"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isActive");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);


            int ID_Ring = 0;
            if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
            else res.ParamValidations["ID_Ring"].AsString = "Value type not recognized";
            
            int ID_Market = 0;
            if (vl_arguments["ID_Market"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt64) ID_Market = vl_arguments["ID_Market"].AsInt32;
            else res.ParamValidations["ID_Market"].AsString = "Value type not recognized";

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value type not recognized";

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
                else res.ParamValidations["Name"].AsString = "Value type not recognized";
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string str_Description = "";
            if (vl_arguments["Description"] != null)
            {
                if (vl_arguments["Description"].ValueType == TVariantType.vtString) str_Description = vl_arguments["Description"].AsString;
                else res.ParamValidations["Description"].AsString = "Value type not recognized";
            }

            string str_Description_EN = "";
            if (vl_arguments["Description_EN"] != null)
            {
                if (vl_arguments["Description_EN"].ValueType == TVariantType.vtString) str_Description_EN = vl_arguments["Description_EN"].AsString;
                else res.ParamValidations["Description_EN"].AsString = "Value type not recognized";
            }

            string str_Description_RO = "";
            if (vl_arguments["Description_RO"] != null)
            {
                if (vl_arguments["Description_RO"].ValueType == TVariantType.vtString) str_Description_RO = vl_arguments["Description_RO"].AsString;
                else res.ParamValidations["Description_RO"].AsString = "Value type not recognized";
            }

            bool hasSchedule = false;
            if (vl_arguments["hasSchedule"].ValueType == TVariantType.vtBoolean) hasSchedule = vl_arguments["hasSchedule"].AsBoolean;
            else res.ParamValidations["hasSchedule"].AsString = "Value type not recognized";

            DateTime dt_StartDate = DateTime.Today;
            DateTime dt_EndDate = DateTime.Today;
            byte DaysOfWeek = 0;
            TimeSpan dt_PreOpeningTime = TimeSpan.Zero;
            TimeSpan dt_OpeningTime = TimeSpan.Zero;
            TimeSpan dt_PreClosingTime = new TimeSpan(23, 59, 00);
            TimeSpan dt_ClosingTime = new TimeSpan(23, 59, 00);

            if (hasSchedule)
            {
                if (vl_arguments["StartDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StartDate");
                if (vl_arguments["EndDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "EndDate");
                if (vl_arguments["DaysOfWeek"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "DaysOfWeek");
                if (vl_arguments["PreOpeningTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PreOpeningTime");
                if (vl_arguments["OpeningTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PreOpeningTime");
                if (vl_arguments["PreClosingTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PreClosingTime");
                if (vl_arguments["ClosingTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ClosingTime");

                if (vl_arguments["StartDate"].ValueType == TVariantType.vtDateTime) dt_StartDate = vl_arguments["StartDate"].AsDateTime;
                else res.ParamValidations["StartDate"].AsString = "Value not recognized";

                if (vl_arguments["EndDate"].ValueType == TVariantType.vtDateTime) dt_EndDate = vl_arguments["EndDate"].AsDateTime;
                else res.ParamValidations["EndDate"].AsString = "Value not recognized";

                if (vl_arguments["DaysOfWeek"].ValueType == TVariantType.vtObject && vl_arguments["DaysOfWeek"].AsObject is TVariantList)
                {
                    DaysOfWeek = 0;
                    TVariantList vl_DaysOfWeek = vl_arguments["DaysOfWeek"].AsObject as TVariantList;
                    for (int i = 0; i < vl_DaysOfWeek.Count; i++)
                    {
                        switch (vl_DaysOfWeek[i].Name.Trim().ToUpper())
                        {
                            case "DAYSUNDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 1;
                                break;
                            case "DAYMONDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 2;
                                break;
                            case "DAYTUESDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 4;
                                break;
                            case "DAYWEDNESDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 8;
                                break;
                            case "DAYTHURSDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 16;
                                break;
                            case "DAYFRIDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 32;
                                break;
                            case "DAYSATURDAY":
                                if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 64;
                                break;
                        }
                    }
                }
                else res.ParamValidations["DaysOfWeek"].AsString = "Value not recognized";

                if (vl_arguments["PreOpeningTime"].ValueType == TVariantType.vtDateTime) dt_PreOpeningTime = vl_arguments["PreOpeningTime"].AsDateTime.TimeOfDay;
                else
                {
                    try { dt_PreOpeningTime = Convert.ToDateTime(vl_arguments["PreOpeningTime"].AsString).TimeOfDay; }
                    catch { res.ParamValidations["PreOpeningTime"].AsString = "Value not recognized"; }
                }

                if (vl_arguments["OpeningTime"].ValueType == TVariantType.vtDateTime) dt_OpeningTime = vl_arguments["OpeningTime"].AsDateTime.TimeOfDay;
                else
                {
                    try { dt_OpeningTime = Convert.ToDateTime(vl_arguments["OpeningTime"].AsString).TimeOfDay; }
                    catch { res.ParamValidations["OpeningTime"].AsString = "Value not recognized"; }
                }

                if (vl_arguments["PreClosingTime"].ValueType == TVariantType.vtDateTime) dt_PreClosingTime = vl_arguments["PreClosingTime"].AsDateTime.TimeOfDay;
                else
                {
                    try { dt_PreClosingTime = Convert.ToDateTime(vl_arguments["PreClosingTime"].AsString).TimeOfDay; }
                    catch { res.ParamValidations["PreClosingTime"].AsString = "Value not recognized"; }
                }

                if (vl_arguments["ClosingTime"].ValueType == TVariantType.vtDateTime) dt_ClosingTime = vl_arguments["ClosingTime"].AsDateTime.TimeOfDay;
                else
                {
                    try { dt_ClosingTime = Convert.ToDateTime(vl_arguments["ClosingTime"].AsString).TimeOfDay; }
                    catch { res.ParamValidations["ClosingTime"].AsString = "Value not recognized"; }
                }
            }

            double flt_MinQuantity = 0;
            try { flt_MinQuantity = vl_arguments["MinQuantity"].AsDouble; }
            catch { res.ParamValidations["MinQuantity"].AsString = "Value not recognized"; }

            double flt_MaxPriceVariation = 0;
            try { flt_MaxPriceVariation = vl_arguments["MaxPriceVariation"].AsDouble; }
            catch { res.ParamValidations["MaxPriceVariation"].AsString = "Value not recognized"; }

            double flt_QuantityStepping = 0;
            try { flt_QuantityStepping = vl_arguments["QuantityStepping"].AsDouble; }
            catch { res.ParamValidations["QuantityStepping"].AsString = "Value not recognized"; }

            double flt_PriceStepping = 0;
            try { flt_PriceStepping = vl_arguments["PriceStepping"].AsDouble; }
            catch { res.ParamValidations["PriceStepping"].AsString = "Value not recognized"; }

            bool bit_IsActive = false;
            if (vl_arguments["isActive"].ValueType == TVariantType.vtBoolean) bit_IsActive = vl_arguments["isActive"].AsBoolean;
            else res.ParamValidations["isActive"].AsString = "Value type not recognized";

            bool bit_IsDefault = false;
            if (vl_arguments["isDefault"] != null)
            {
                if (vl_arguments["isDefault"].ValueType == TVariantType.vtBoolean) bit_IsDefault = vl_arguments["isDefault"].AsBoolean;
                else res.ParamValidations["isDefault"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("@prm_ID_Market").AsInt32 = ID_Market;
                vl_params.Add("@prm_Code").AsString = str_Code;
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_hasSchedule").AsBoolean = hasSchedule;
                vl_params.Add("@prm_StartDate").AsDateTime = dt_StartDate;
                vl_params.Add("@prm_EndDate").AsDateTime = dt_EndDate;
                vl_params.Add("@prm_DaysOfWeek").AsByte = DaysOfWeek;
                vl_params.Add("@prm_PreOpeningTime").AsDateTime = DateTime.Today + dt_PreOpeningTime;
                vl_params.Add("@prm_OpeningTime").AsDateTime = DateTime.Today + dt_OpeningTime;
                vl_params.Add("@prm_PreClosingTime").AsDateTime = DateTime.Today + dt_PreClosingTime;
                vl_params.Add("@prm_ClosingTime").AsDateTime = DateTime.Today + dt_ClosingTime;
                vl_params.Add("@prm_MinQuantity").AsDouble = flt_MinQuantity;
                vl_params.Add("@prm_MaxPriceVariation").AsDouble = flt_MaxPriceVariation;
                vl_params.Add("@prm_QuantityStepping").AsDouble = flt_QuantityStepping;
                vl_params.Add("@prm_PriceStepping").AsDouble = flt_PriceStepping;
                vl_params.Add("@prm_isActive").AsBoolean = bit_IsActive;
                vl_params.Add("@prm_isDefault").AsBoolean = bit_IsDefault;

                int int_count_Rings = app.DB.Exec("update_Ring", "Rings", vl_params);
                if (int_count_Rings == -1) throw new Exception("Ring update failed");

                if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
                {
                    str_Name = "fld_Ring_Name_" + ID_Ring.ToString();
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Label").AsString = str_Name;
                    vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                    vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                    app.DB.Exec("setTranslation", "Translations", vl_params);
                }

                if (str_Description == "" && (str_Description_EN != "" || str_Description_RO != ""))
                {
                    str_Description = "fld_Ring_Description_" + ID_Ring.ToString();
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_Label").AsString = str_Description;
                    vl_params.Add("@prm_Value_EN").AsString = str_Description_EN;
                    vl_params.Add("@prm_Value_RO").AsString = str_Description_RO;
                    app.DB.Exec("setTranslation", "Translations", vl_params);
                }

                vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID").AsInt32 = ID_Ring;
                app.DB.Exec("update_TextFields", "Rings", vl_params);

                app.DB.CommitTransaction();

                res.RowsModified = int_count_Rings;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult deleteRing(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Ring");

            //  validation
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int idRing = 0;
            try { idRing = vl_arguments["ID_Ring"].AsInt32; }
            catch { res.ParamValidations["ID_Ring"].AsString = "Value not recognized"; }

            if (res.bInvalid()) return res;

            //  check for the existence of asset types defined on this ring
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = -1;
            vl_params.Add("@prm_ID_Ring").AsInt32 = idRing;
            DataSet ds_AT = app.DB.Select("select_AssetTypes", "AssetTypes", vl_params);
            if (app.DB.ValidDSRows(ds_AT)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Cannot delete a Ring while Asset Types are defined");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Ring").AsInt32 = idRing;

                int countDeleted = app.DB.Exec("delete_Ring", "Rings", vl_params);
                if (countDeleted <= 0) throw new Exception("Ring deletion failed");

                res.RowsModified = countDeleted;
                return res;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult setClient2Ring(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Client"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["canBuy"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["canSell"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            int ID_Client = ((vl_arguments["ID_Client"] != null) && (vl_arguments["ID_Client"].ValueType == TVariantType.vtByte ||
                                                                    vl_arguments["ID_Client"].ValueType == TVariantType.vtInt16 ||
                                                                    vl_arguments["ID_Client"].ValueType == TVariantType.vtInt32 ||
                                                                    vl_arguments["ID_Client"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_Client"].AsInt32 : 0;

            int ID_Ring = ((vl_arguments["ID_Ring"] != null) && (vl_arguments["ID_Ring"].ValueType == TVariantType.vtByte ||
                                                        vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                                                        vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                                                        vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_Ring"].AsInt32 : 0;

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            if (ID_Client == 0)
                res.ParamValidations["ID_Client"].AsString = "Value type not recognized";

            if (ID_Ring == 0)
                res.ParamValidations["ID_Ring"].AsString = "Value type not recognized";

            bool canBuy = false;
            if (vl_arguments["canBuy"].ValueType == TVariantType.vtBoolean) canBuy = vl_arguments["canBuy"].AsBoolean;
            else res.ParamValidations["canBuy"].AsString = "Value type not recognized";

            bool canSell = false;
            if (vl_arguments["canSell"].ValueType == TVariantType.vtBoolean) canSell = vl_arguments["canSell"].AsBoolean;
            else res.ParamValidations["canSell"].AsString = "Value type not recognized";

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
            vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
            vl_params.Add("@prm_canBuy").AsBoolean = canBuy;
            vl_params.Add("@prm_canSell").AsBoolean = canSell;

            int int_count_RingXClient = app.DB.Exec("update_RingXClient", "Rings", vl_params);
            if (int_count_RingXClient == -1) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (int_count_RingXClient == 0)
            {
                int_count_RingXClient = app.DB.Exec("insert_RingXClient", "Rings", vl_params);
                if (int_count_RingXClient == -1) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

                return new TDBExecResult(TDBExecError.Success, "", int_count_RingXClient);
            }

            return new TDBExecResult(TDBExecError.Success, "", int_count_RingXClient);
        }

        public TDBExecResult setClient2Asset(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Client"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["canBuy"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["canSell"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            int ID_Client = ((vl_arguments["ID_Client"] != null) && (vl_arguments["ID_Client"].ValueType == TVariantType.vtByte ||
                                                                    vl_arguments["ID_Client"].ValueType == TVariantType.vtInt16 ||
                                                                    vl_arguments["ID_Client"].ValueType == TVariantType.vtInt32 ||
                                                                    vl_arguments["ID_Client"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_Client"].AsInt32 : 0;

            int ID_Asset = ((vl_arguments["ID_Asset"] != null) && (vl_arguments["ID_Asset"].ValueType == TVariantType.vtByte ||
                                                        vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                                                        vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                                                        vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_Asset"].AsInt32 : 0;

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            if (ID_Client == 0)
                res.ParamValidations["ID_Client"].AsString = "Value type not recognized";

            if (ID_Asset == 0)
                res.ParamValidations["ID_Ring"].AsString = "Value type not recognized";

            bool canBuy = false;
            if (vl_arguments["canBuy"].ValueType == TVariantType.vtBoolean) canBuy = vl_arguments["canBuy"].AsBoolean;
            else res.ParamValidations["canBuy"].AsString = "Value type not recognized";

            bool canSell = false;
            if (vl_arguments["canSell"].ValueType == TVariantType.vtBoolean) canSell = vl_arguments["canSell"].AsBoolean;
            else res.ParamValidations["canSell"].AsString = "Value type not recognized";

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            vl_params.Add("@prm_canBuy").AsBoolean = canBuy;
            vl_params.Add("@prm_canSell").AsBoolean = canSell;

            int int_count_AssetXClient = app.DB.Exec("update_AssetXClient", "Rings", vl_params);
            if (int_count_AssetXClient == -1) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (int_count_AssetXClient == 0)
            {
                int_count_AssetXClient = app.DB.Exec("insert_AssetXClient", "Rings", vl_params);
                if (int_count_AssetXClient == -1) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

                return new TDBExecResult(TDBExecError.Success, "", int_count_AssetXClient);
            }

            return new TDBExecResult(TDBExecError.Success, "", int_count_AssetXClient);
        }

        public TDBExecResult setRingAdministrator(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Ring");
            if (vl_arguments["ID_User"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_User");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Ring = 0;
            if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
            else res.ParamValidations["ID_Ring"].AsString = "Value not recognized";

            int ID_User = 0;
            if (vl_arguments["ID_User"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_User"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_User"].ValueType == TVariantType.vtInt64) ID_User = vl_arguments["ID_User"].AsInt32;
            else res.ParamValidations["ID_User"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "Database transaction error");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("@prm_ID_User").AsInt32 = ID_User;
                int int_count = app.DB.Exec("set_RingXUser", "Rings", vl_params);
                if (int_count <= 0) throw new Exception("Ring Administrator set failed");

                app.DB.CommitTransaction();
            }
            catch
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Ring Administrator set failed");
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult resetRingAdministrator(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Ring");
            if (vl_arguments["ID_User"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_User");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Ring = 0;
            if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
            else res.ParamValidations["ID_Ring"].AsString = "Value not recognized";

            int ID_User = 0;
            if (vl_arguments["ID_User"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_User"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_User"].ValueType == TVariantType.vtInt64) ID_User = vl_arguments["ID_User"].AsInt32;
            else res.ParamValidations["ID_User"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "Database transaction error");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("@prm_ID_User").AsInt32 = ID_User;
                int int_count = app.DB.Exec("reset_RingXUser", "Rings", vl_params);
                if (int_count <= 0) throw new Exception("Ring Administrator reset failed");

                app.DB.CommitTransaction();
            }
            catch
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Ring Administrator reset failed");
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult approveInitialOrder(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Order"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Order");

            //  validation
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int idOrder = 0;
            try { idOrder = vl_arguments["ID_Order"].AsInt32; }
            catch { res.ParamValidations["ID_Order"].AsString = "Value not recognized"; }

            if (res.bInvalid()) return res;
            
            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = idOrder;

                int count_approve_initialOrder = app.DB.Exec("approve_InitialOrder", "Orders", vl_params);
                if (count_approve_initialOrder <= 0) throw new Exception("Order approval  failed");

                int count_approve_asset = app.DB.Exec("approve_AssetByInitialOrderID", "Assets", vl_params);
                if (count_approve_asset <= 0) throw new Exception("Asset approval  failed");

                res.RowsModified = count_approve_initialOrder + count_approve_asset;
                //session.AddToJournal("order approved", ID_Client, ID_Ring, ID_Asset, ID_Order, 0, 0);
                return res;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult rejectInitialOrder(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Order"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Order");

            //  validation
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int idOrder = 0;
            try { idOrder = vl_arguments["ID_Order"].AsInt32; }
            catch { res.ParamValidations["ID_Order"].AsString = "Value not recognized"; }

            if (res.bInvalid()) return res;

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = idOrder;

                int count_reject_initialOrder = app.DB.Exec("cancel_Order", "Orders", vl_params);
                if (count_reject_initialOrder <= 0) throw new Exception("Order rejection  failed");

                int count_reject_Asset = app.DB.Exec("delete_AssetByInitialOrderID", "Assets", vl_params);
                if (count_reject_Asset <= 0) throw new Exception("Asset rejection  failed");

                res.RowsModified = count_reject_initialOrder + count_reject_Asset;
                return res;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }       

        public TDBExecResult addAssetSchedule(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");
            if (vl_arguments["StartDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StartDate");
            if (vl_arguments["EndDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "EndDate");
            if (vl_arguments["DaysOfWeek"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "DaysOfWeek");
            if (vl_arguments["PreOpeningTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PreOpeningTime");
            if (vl_arguments["OpeningTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OpeningTime");
            if (vl_arguments["PreClosingTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PreClosingTime");
            if (vl_arguments["ClosingTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ClosingTime");
            if (vl_arguments["MinQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MinQuantity");
            if (vl_arguments["MaxPriceVariation"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MaxPriceVariation");
            if (vl_arguments["QuantityStepping"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "QuantityStepping");
            if (vl_arguments["PriceStepping"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PriceStepping");
            //if (vl_arguments["isDefault"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isDefault");            

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Asset = 0;
            try { ID_Asset = vl_arguments["ID_Asset"].AsInt32; }
            catch { res.ParamValidations["ID_Asset"].AsString = "Value not recognized"; }

            DateTime dt_StartDate = DateTime.Today;
            if (vl_arguments["StartDate"].ValueType == TVariantType.vtDateTime) dt_StartDate = vl_arguments["StartDate"].AsDateTime;
            else res.ParamValidations["StartDate"].AsString = "Value not recognized";

            DateTime dt_EndDate = DateTime.Today;
            if (vl_arguments["EndDate"].ValueType == TVariantType.vtDateTime) dt_EndDate = vl_arguments["EndDate"].AsDateTime;
            else res.ParamValidations["EndDate"].AsString = "Value not recognized";

            byte DaysOfWeek = 127;
            if (vl_arguments["DaysOfWeek"].ValueType == TVariantType.vtObject && vl_arguments["DaysOfWeek"].AsObject is TVariantList)
            {
                DaysOfWeek = 0;
                TVariantList vl_DaysOfWeek = vl_arguments["DaysOfWeek"].AsObject as TVariantList;
                for (int i = 0; i < vl_DaysOfWeek.Count; i++)
                {
                    switch (vl_DaysOfWeek[i].Name.Trim().ToUpper())
                    {
                        case "DAYSUNDAY":
                            if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 1;
                            break;
                        case "DAYMONDAY":
                            if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 2;
                            break;
                        case "DAYTUESDAY":
                            if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 4;
                            break;
                        case "DAYWEDNESDAY":
                            if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 8;
                            break;
                        case "DAYTHURSDAY":
                            if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 16;
                            break;
                        case "DAYFRIDAY":
                            if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 32;
                            break;
                        case "DAYSATURDAY":
                            if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 64;
                            break;
                    }
                }
            }
            else res.ParamValidations["DaysOfWeek"].AsString = "Value not recognized";

            DateTime dt_PreOpeningTime = DateTime.Now;
            try { dt_PreOpeningTime = vl_arguments["PreOpeningTime"].AsDateTime; }
            catch { res.ParamValidations["PreOpeningTime"].AsString = "Value not recognized"; }

            DateTime dt_OpeningTime = DateTime.Now;
            try { dt_OpeningTime = vl_arguments["OpeningTime"].AsDateTime; }
            catch { res.ParamValidations["OpeningTime"].AsString = "Value not recognized"; }

            DateTime dt_PreClosingTime = DateTime.Now;
            try { dt_PreClosingTime = vl_arguments["PreClosingTime"].AsDateTime; }
            catch { res.ParamValidations["PreClosingTime"].AsString = "Value not recognized"; }

            DateTime dt_ClosingTime = DateTime.Now;
            try { dt_ClosingTime = vl_arguments["ClosingTime"].AsDateTime; }
            catch { res.ParamValidations["ClosingTime"].AsString = "Value not recognized"; }

            if (dt_PreOpeningTime > dt_OpeningTime && res.ParamValidations["PreOpeningTime"].AsString == "")
                res.ParamValidations["PreOpeningTime"].AsString = "T0 nu poate fi mai mare decat T1";

            if (dt_OpeningTime > dt_PreClosingTime && res.ParamValidations["OpeningTime"].AsString == "")
                res.ParamValidations["OpeningTime"].AsString = "T1 nu poate fi mai mare decat T2";

            if (dt_PreClosingTime > dt_ClosingTime && res.ParamValidations["PreClosingTime"].AsString == "")
                res.ParamValidations["PreClosingTime"].AsString = "T2 nu poate fi mai mare decat T3";

            bool isElectronicSession = true;
            if (vl_arguments["isElectronicSession"] != null)
            {
                if (vl_arguments["isElectronicSession"].ValueType == TVariantType.vtBoolean) isElectronicSession = vl_arguments["isElectronicSession"].AsBoolean;
                else res.ParamValidations["isElectronicSession"].AsString = "Value not recognized";
            }

            bool launchAutomatically = true;
            if (vl_arguments["launchAutomatically"] != null)
            {
                if (vl_arguments["launchAutomatically"].ValueType == TVariantType.vtBoolean) launchAutomatically = vl_arguments["launchAutomatically"].AsBoolean;
                else res.ParamValidations["launchAutomatically"].AsString = "Value not recognized";
            }
            
            double flt_MinQuantity = 0;
            if (vl_arguments["MinQuantity"].ValueType == TVariantType.vtString && vl_arguments["MinQuantity"].AsString == "none") ;
            else
            {
                try { flt_MinQuantity = vl_arguments["MinQuantity"].AsDouble; }
                catch { res.ParamValidations["MinQuantity"].AsString = "Value not recognized"; }
            }

            double flt_QuantityStepping = 0.000001;
            if (vl_arguments["QuantityStepping"].ValueType == TVariantType.vtString && vl_arguments["QuantityStepping"].AsString == "none") ;
            else
            {
                try { flt_QuantityStepping = vl_arguments["QuantityStepping"].AsDouble; }
                catch { res.ParamValidations["QuantityStepping"].AsString = "Value not recognized"; }
            }

            double flt_PriceStepping = 0.000001;
            if (vl_arguments["PriceStepping"].ValueType == TVariantType.vtString && vl_arguments["PriceStepping"].AsString == "none") ;
            else
            {
                try { flt_PriceStepping = vl_arguments["PriceStepping"].AsDouble; }
                catch { res.ParamValidations["PriceStepping"].AsString = "Value not recognized"; }
            }

            double flt_MaxPriceVariation = 0;
            if (vl_arguments["MaxPriceVariation"].ValueType == TVariantType.vtString && vl_arguments["MaxPriceVariation"].AsString == "none") ;
            else
            {
                try { flt_MaxPriceVariation = vl_arguments["MaxPriceVariation"].AsDouble; }
                catch { res.ParamValidations["MaxPriceVariation"].AsString = "Value not recognized"; }
            }

            double flt_MinPrice = 0;
            if (vl_arguments["MinPrice"] != null)
            {
                if (vl_arguments["MinPrice"].ValueType == TVariantType.vtString && vl_arguments["MinPrice"].AsString == "none") ;
                else
                {
                    if (vl_arguments["MinPrice"].ValueType == TVariantType.vtInt16 ||
                        vl_arguments["MinPrice"].ValueType == TVariantType.vtInt32 ||
                        vl_arguments["MinPrice"].ValueType == TVariantType.vtInt64 ||
                        vl_arguments["MinPrice"].ValueType == TVariantType.vtSingle ||
                        vl_arguments["MinPrice"].ValueType == TVariantType.vtDouble) flt_MinPrice = vl_arguments["MinPrice"].AsDouble;
                    else res.ParamValidations["MinPrice"].AsString = "Value not recognized";
                }
            }

            double flt_MaxPrice = 0;
            if (vl_arguments["MaxPrice"] != null)
            {
                if (vl_arguments["MaxPrice"].ValueType == TVariantType.vtString && vl_arguments["MaxPrice"].AsString == "none") ;
                else
                {
                    if (vl_arguments["MaxPrice"].ValueType == TVariantType.vtInt16 ||
                        vl_arguments["MaxPrice"].ValueType == TVariantType.vtInt32 ||
                        vl_arguments["MaxPrice"].ValueType == TVariantType.vtInt64 ||
                        vl_arguments["MaxPrice"].ValueType == TVariantType.vtSingle ||
                        vl_arguments["MaxPrice"].ValueType == TVariantType.vtDouble) flt_MaxPrice = vl_arguments["MaxPrice"].AsDouble;
                    else res.ParamValidations["MaxPrice"].AsString = "Value not recognized";
                }
            }

            bool IsDefault = false;
            if (vl_arguments["isDefault"] != null)
            {
                try { IsDefault = vl_arguments["isDefault"].AsBoolean; }
                catch { res.ParamValidations["isDefault"].AsString = "Value not recognized"; }
            }

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_StartDate").AsDateTime = dt_StartDate;
                vl_params.Add("@prm_EndDate").AsDateTime = dt_EndDate;
                vl_params.Add("@prm_DaysOfWeek").AsByte = DaysOfWeek;
                vl_params.Add("@prm_PreOpeningTime").AsDateTime = dt_PreOpeningTime;
                vl_params.Add("@prm_OpeningTime").AsDateTime = dt_OpeningTime;
                vl_params.Add("@prm_PreClosingTime").AsDateTime = dt_PreClosingTime;
                vl_params.Add("@prm_ClosingTime").AsDateTime = dt_ClosingTime;
                vl_params.Add("@prm_isElectronicSession").AsBoolean = isElectronicSession;
                vl_params.Add("@prm_launchAutomatically").AsBoolean = launchAutomatically;
                vl_params.Add("@prm_MinQuantity").AsDouble = flt_MinQuantity;
                vl_params.Add("@prm_MaxPriceVariation").AsDouble = flt_MaxPriceVariation;
                vl_params.Add("@prm_QuantityStepping").AsDouble = flt_QuantityStepping;
                vl_params.Add("@prm_PriceStepping").AsDouble = flt_PriceStepping;
                vl_params.Add("@prm_MinPrice").AsDouble = flt_MinPrice;
                vl_params.Add("@prm_MaxPrice").AsDouble = flt_MaxPrice;
                vl_params.Add("@prm_IsDefault").AsBoolean = IsDefault;
                int int_count = app.DB.Exec("insert_AssetSchedule", "Assets", vl_params);
                if (int_count <= 0) throw new Exception("AssetSchedule insert failed");

                int ID_AssetSchedule = app.DB.GetIdentity();
                updateTradeParameters(0, 0, 0, ID_AssetSchedule, vl_arguments);

                app.DB.CommitTransaction();

                res.RowsModified = int_count;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult editAssetSchedule(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_AssetSchedule"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AssetSchedule");
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");
            if (vl_arguments["PreOpeningTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PreOpeningTime");
            if (vl_arguments["OpeningTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OpeningTime");
            if (vl_arguments["PreClosingTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PreClosingTime");
            if (vl_arguments["ClosingTime"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ClosingTime");
            if (vl_arguments["MinQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MinQuantity");
            if (vl_arguments["MaxPriceVariation"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MaxPriceVariation");
            if (vl_arguments["QuantityStepping"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "QuantityStepping");
            if (vl_arguments["PriceStepping"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "PriceStepping");
            if (vl_arguments["isActive"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isActive");
            //if (vl_arguments["IsDefault"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "IsDefault");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_AssetSchedule = 0;
            try { ID_AssetSchedule = vl_arguments["ID_AssetSchedule"].AsInt32; }
            catch { res.ParamValidations["ID_AssetSchedule"].AsString = "Value not recognized"; }

            int ID_Asset = 0;
            try { ID_Asset = vl_arguments["ID_Asset"].AsInt32; }
            catch { res.ParamValidations["ID_Asset"].AsString = "Value not recognized"; }

            DateTime dt_StartDate = DateTime.Today;
            if (vl_arguments["StartDate"].ValueType == TVariantType.vtDateTime) dt_StartDate = vl_arguments["StartDate"].AsDateTime;
            else res.ParamValidations["StartDate"].AsString = "Value not recognized";

            DateTime dt_EndDate = DateTime.Today;
            if (vl_arguments["EndDate"].ValueType == TVariantType.vtDateTime) dt_EndDate = vl_arguments["EndDate"].AsDateTime;
            else res.ParamValidations["EndDate"].AsString = "Value not recognized";

            byte DaysOfWeek = 127;
            if (vl_arguments["DaysOfWeek"].ValueType == TVariantType.vtObject && vl_arguments["DaysOfWeek"].AsObject is TVariantList)
            {
                DaysOfWeek = 0;
                TVariantList vl_DaysOfWeek = vl_arguments["DaysOfWeek"].AsObject as TVariantList;
                for (int i = 0; i < vl_DaysOfWeek.Count; i++)
                {
                    switch (vl_DaysOfWeek[i].Name.Trim().ToUpper())
                    {
                        case "DAYSUNDAY":
                            if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 1;
                            break;
                        case "DAYMONDAY":
                            if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 2;
                            break;
                        case "DAYTUESDAY":
                            if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 4;
                            break;
                        case "DAYWEDNESDAY":
                            if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 8;
                            break;
                        case "DAYTHURSDAY":
                            if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 16;
                            break;
                        case "DAYFRIDAY":
                            if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 32;
                            break;
                        case "DAYSATURDAY":
                            if (vl_DaysOfWeek[i].AsBoolean) DaysOfWeek += 64;
                            break;
                    }
                }
            }
            else res.ParamValidations["DaysOfWeek"].AsString = "Value not recognized";

            DateTime dt_PreOpeningTime = DateTime.Now;
            try { dt_PreOpeningTime = vl_arguments["PreOpeningTime"].AsDateTime; }
            catch { res.ParamValidations["PreOpeningTime"].AsString = "Value not recognized"; }

            DateTime dt_OpeningTime = DateTime.Now;
            try { dt_OpeningTime = vl_arguments["OpeningTime"].AsDateTime; }
            catch { res.ParamValidations["OpeningTime"].AsString = "Value not recognized"; }

            DateTime dt_PreClosingTime = DateTime.Now;
            try { dt_PreClosingTime = vl_arguments["PreClosingTime"].AsDateTime; }
            catch { res.ParamValidations["PreClosingTime"].AsString = "Value not recognized"; }

            DateTime dt_ClosingTime = DateTime.Now;
            try { dt_ClosingTime = vl_arguments["ClosingTime"].AsDateTime; }
            catch { res.ParamValidations["ClosingTime"].AsString = "Value not recognized"; }

            bool isElectronicSession = true;
            if (vl_arguments["isElectronicSession"] != null)
            {
                if (vl_arguments["isElectronicSession"].ValueType == TVariantType.vtBoolean) isElectronicSession = vl_arguments["isElectronicSession"].AsBoolean;
                else res.ParamValidations["isElectronicSession"].AsString = "Value not recognized";
            }

            bool launchAutomatically = true;
            if (vl_arguments["launchAutomatically"] != null)
            {
                if (vl_arguments["launchAutomatically"].ValueType == TVariantType.vtBoolean) launchAutomatically = vl_arguments["launchAutomatically"].AsBoolean;
                else res.ParamValidations["launchAutomatically"].AsString = "Value not recognized";
            }

            double flt_MinQuantity = 0;
            if (vl_arguments["MinQuantity"].ValueType == TVariantType.vtString && vl_arguments["MinQuantity"].AsString == "none") ;
            else
            {
                try { flt_MinQuantity = vl_arguments["MinQuantity"].AsDouble; }
                catch { res.ParamValidations["MinQuantity"].AsString = "Value not recognized"; }
            }

            double flt_QuantityStepping = 0.000001;
            if (vl_arguments["QuantityStepping"].ValueType == TVariantType.vtString && vl_arguments["QuantityStepping"].AsString == "none") ;
            else
            {
                try { flt_QuantityStepping = vl_arguments["QuantityStepping"].AsDouble; }
                catch { res.ParamValidations["QuantityStepping"].AsString = "Value not recognized"; }
            }

            double flt_PriceStepping = 0.000001;
            if (vl_arguments["PriceStepping"].ValueType == TVariantType.vtString && vl_arguments["PriceStepping"].AsString == "none") ;
            else
            {
                try { flt_PriceStepping = vl_arguments["PriceStepping"].AsDouble; }
                catch { res.ParamValidations["PriceStepping"].AsString = "Value not recognized"; }
            }

            double flt_MaxPriceVariation = 0;
            if (vl_arguments["MaxPriceVariation"].ValueType == TVariantType.vtString && vl_arguments["MaxPriceVariation"].AsString == "none") ;
            else
            {
                try { flt_MaxPriceVariation = vl_arguments["MaxPriceVariation"].AsDouble; }
                catch { res.ParamValidations["MaxPriceVariation"].AsString = "Value not recognized"; }
            }

            double flt_MinPrice = 0;
            if (vl_arguments["MinPrice"] != null)
            {
                if (vl_arguments["MinPrice"].ValueType == TVariantType.vtString && vl_arguments["MinPrice"].AsString == "none") ;
                else
                {
                    if (vl_arguments["MinPrice"].ValueType == TVariantType.vtInt16 ||
                        vl_arguments["MinPrice"].ValueType == TVariantType.vtInt32 ||
                        vl_arguments["MinPrice"].ValueType == TVariantType.vtInt64 ||
                        vl_arguments["MinPrice"].ValueType == TVariantType.vtSingle ||
                        vl_arguments["MinPrice"].ValueType == TVariantType.vtDouble) flt_MinPrice = vl_arguments["MinPrice"].AsDouble;
                    else res.ParamValidations["MinPrice"].AsString = "Value not recognized";
                }
            }

            double flt_MaxPrice = 0;
            if (vl_arguments["MaxPrice"] != null)
            {
                if (vl_arguments["MaxPrice"].ValueType == TVariantType.vtString && vl_arguments["MaxPrice"].AsString == "none") ;
                else
                {
                    if (vl_arguments["MaxPrice"].ValueType == TVariantType.vtInt16 ||
                        vl_arguments["MaxPrice"].ValueType == TVariantType.vtInt32 ||
                        vl_arguments["MaxPrice"].ValueType == TVariantType.vtInt64 ||
                        vl_arguments["MaxPrice"].ValueType == TVariantType.vtSingle ||
                        vl_arguments["MaxPrice"].ValueType == TVariantType.vtDouble) flt_MaxPrice = vl_arguments["MaxPrice"].AsDouble;
                    else res.ParamValidations["MaxPrice"].AsString = "Value not recognized";
                }
            }

            bool isActive = false;
            try { isActive = vl_arguments["isActive"].AsBoolean; }
            catch { res.ParamValidations["isActive"].AsString = "Value not recognized"; }

            bool isDefault = false;
            if (vl_arguments["isDefault"] != null)
            {
                try { isDefault = vl_arguments["isDefault"].AsBoolean; }
                catch { res.ParamValidations["isDefault"].AsString = "Value not recognized"; }
            }

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_AssetSchedule").AsInt32 = ID_AssetSchedule;
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_StartDate").AsDateTime = dt_StartDate;
                vl_params.Add("@prm_EndDate").AsDateTime = dt_EndDate;
                vl_params.Add("@prm_DaysOfWeek").AsByte = DaysOfWeek;
                vl_params.Add("@prm_PreOpeningTime").AsDateTime = dt_PreOpeningTime;
                vl_params.Add("@prm_OpeningTime").AsDateTime = dt_OpeningTime;
                vl_params.Add("@prm_PreClosingTime").AsDateTime = dt_PreClosingTime;
                vl_params.Add("@prm_ClosingTime").AsDateTime = dt_ClosingTime;
                vl_params.Add("@prm_isElectronicSession").AsBoolean = isElectronicSession;
                vl_params.Add("@prm_launchAutomatically").AsBoolean = launchAutomatically;
                vl_params.Add("@prm_MinQuantity").AsDouble = flt_MinQuantity;
                vl_params.Add("@prm_MaxPriceVariation").AsDouble = flt_MaxPriceVariation;
                vl_params.Add("@prm_QuantityStepping").AsDouble = flt_QuantityStepping;
                vl_params.Add("@prm_PriceStepping").AsDouble = flt_PriceStepping;
                vl_params.Add("@prm_MinPrice").AsDouble = flt_MinPrice;
                vl_params.Add("@prm_MaxPrice").AsDouble = flt_MaxPrice;
                vl_params.Add("@prm_isDefault").AsBoolean = isDefault;
                vl_params.Add("@prm_isActive").AsBoolean = isActive;
                int int_count = app.DB.Exec("update_AssetSchedule", "Assets", vl_params);
                if (int_count <= 0) throw new Exception("AssetSchedule update failed");

                updateTradeParameters(0, 0, 0, ID_AssetSchedule, vl_arguments);

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                app.DB.Exec("update_AssetSession4Event", "AssetSessions", vl_params);

                app.DB.CommitTransaction();

                res.RowsModified = int_count;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult deleteAssetSchedule(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_AssetSchedule"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AssetSchedule");

            //  validation
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int idAssetSchedule = 0;
            try { idAssetSchedule = vl_arguments["ID_AssetSchedule"].AsInt32; }
            catch { res.ParamValidations["ID_AssetSchedule"].AsString = "Value not recognized"; }

            if (res.bInvalid()) return res;

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_AssetSchedule").AsInt32 = idAssetSchedule;

                int countDeleted = app.DB.Exec("delete_AssetSchedule", "Assets", vl_params);
                if (countDeleted <= 0) throw new Exception("Ring deletion failed");

                res.RowsModified = countDeleted;
                return res;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }


        public TDBExecResult addCurrency(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
                else res.ParamValidations["Name"].AsString = "Value not recognized";
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_Name").AsString = str_Name;
            int int_rows = app.DB.Exec("insert_Currency", "Currencies", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            int ID_Currency = app.DB.GetIdentity();
            if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
            {
                str_Name = "fld_Currency_Name_" + ID_Currency.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);
            }

            vl_params = new TVariantList();
            vl_params.Add("@prm_Name").AsString = str_Name;
            vl_params.Add("@prm_ID").AsInt32 = ID_Currency;
            app.DB.Exec("update_TextFields", "Currencies", vl_params);

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult editCurrency(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_Currency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Currency");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Currency = 0;
            if (vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt64) ID_Currency = vl_arguments["ID_Currency"].AsInt32;
            else res.ParamValidations["ID_Currency"].AsString = "Value not recognized";

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
                else res.ParamValidations["Name"].AsString = "Value not recognized";
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Currency;
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_Name").AsString = str_Name;
            int int_rows = app.DB.Exec("update_Currency", "Currencies", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
            {
                str_Name = "fld_Currency_Name_" + ID_Currency.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);
            }

            vl_params = new TVariantList();
            vl_params.Add("@prm_Name").AsString = str_Name;
            vl_params.Add("@prm_ID").AsInt32 = ID_Currency;
            app.DB.Exec("update_TextFields", "Currencies", vl_params);

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult deleteCurrency(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_Currency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Currency");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Currency = 0;
            if (vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt64) ID_Currency = vl_arguments["ID_Currency"].AsInt32;
            else res.ParamValidations["ID_Currency"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Currency;
            int int_rows = app.DB.Exec("delete_Currency", "Currencies", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult addMeasuringUnit(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
                else res.ParamValidations["Name"].AsString = "Value not recognized";
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_Name").AsString = str_Name;
            int int_rows = app.DB.Exec("insert_MeasuringUnit", "MeasuringUnits", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            int ID_MeasuringUnit = app.DB.GetIdentity();
            if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
            {
                str_Name = "fld_MeasuringUnit_Name_" + ID_MeasuringUnit.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);
            }

            vl_params = new TVariantList();
            vl_params.Add("@prm_Name").AsString = str_Name;
            vl_params.Add("@prm_ID").AsInt32 = ID_MeasuringUnit;
            app.DB.Exec("update_TextFields", "MeasuringUnits", vl_params);

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult editMeasuringUnit(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_MeasuringUnit"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_MeasuringUnit");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_MeasuringUnit = 0;
            if (vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt64) ID_MeasuringUnit = vl_arguments["ID_MeasuringUnit"].AsInt32;
            else res.ParamValidations["ID_MeasuringUnit"].AsString = "Value not recognized";

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
                else res.ParamValidations["Name"].AsString = "Value not recognized";
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_MeasuringUnit;
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_Name").AsString = str_Name;
            int int_rows = app.DB.Exec("update_MeasuringUnit", "MeasuringUnits", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
            {
                str_Name = "fld_MeasuringUnit_Name_" + ID_MeasuringUnit.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);
            }

            vl_params = new TVariantList();
            vl_params.Add("@prm_Name").AsString = str_Name;
            vl_params.Add("@prm_ID").AsInt32 = ID_MeasuringUnit;
            app.DB.Exec("update_TextFields", "MeasuringUnits", vl_params);
            
            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult deleteMeasuringUnit(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_MeasuringUnit"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_MeasuringUnit");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_MeasuringUnit = 0;
            if (vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_MeasuringUnit"].ValueType == TVariantType.vtInt64) ID_MeasuringUnit = vl_arguments["ID_MeasuringUnit"].AsInt32;
            else res.ParamValidations["ID_MeasuringUnit"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_MeasuringUnit;
            int int_rows = app.DB.Exec("delete_MeasuringUnit", "MeasuringUnits", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult addCPV(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_RO");
            if (vl_arguments["Name_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_EN");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string Name_RO = "";
            if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) Name_RO = vl_arguments["Name_RO"].AsString;
            else res.ParamValidations["Name_RO"].AsString = "Value not recognized";

            string Name_EN = "";
            if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) Name_EN = vl_arguments["Name_EN"].AsString;
            else res.ParamValidations["Name_EN"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_Name_RO").AsString = Name_RO;
            vl_params.Add("@prm_Name_EN").AsString = Name_EN;
            int int_rows = app.DB.Exec("insert_CPV", "CPV", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult editCPV(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_CPV"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_CPV");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_RO");
            if (vl_arguments["Name_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_EN");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_CPV = 0;
            if (vl_arguments["ID_CPV"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_CPV"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_CPV"].ValueType == TVariantType.vtInt64) ID_CPV = vl_arguments["ID_CPV"].AsInt32;
            else res.ParamValidations["ID_CPV"].AsString = "Value not recognized";

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string Name_RO = "";
            if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) Name_RO = vl_arguments["Name_RO"].AsString;
            else res.ParamValidations["Name_RO"].AsString = "Value not recognized";

            string Name_EN = "";
            if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) Name_EN = vl_arguments["Name_EN"].AsString;
            else res.ParamValidations["Name_EN"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_CPV;
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_Name_RO").AsString = Name_RO;
            vl_params.Add("@prm_Name_EN").AsString = Name_EN;
            int int_rows = app.DB.Exec("update_CPV", "CPV", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult deleteCPV(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_CPV"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_CPV");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_CPV = 0;
            if (vl_arguments["ID_CPV"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_CPV"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_CPV"].ValueType == TVariantType.vtInt64) ID_CPV = vl_arguments["ID_CPV"].AsInt32;
            else res.ParamValidations["ID_CPV"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_CPV;
            int int_rows = app.DB.Exec("delete_CPV", "CPV", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult addCAEN(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_RO");
            if (vl_arguments["Name_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_EN");
            if (vl_arguments["ParentCode"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ParentCode");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string Name_RO = "";
            if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) Name_RO = vl_arguments["Name_RO"].AsString;
            else res.ParamValidations["Name_RO"].AsString = "Value not recognized";

            string Name_EN = "";
            if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) Name_EN = vl_arguments["Name_EN"].AsString;
            else res.ParamValidations["Name_EN"].AsString = "Value not recognized";

            string ParentCode = "";
            if (vl_arguments["ParentCode"].ValueType == TVariantType.vtString) ParentCode = vl_arguments["ParentCode"].AsString;
            else res.ParamValidations["ParentCode"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_Name_RO").AsString = Name_RO;
            vl_params.Add("@prm_Name_EN").AsString = Name_EN;
            vl_params.Add("@prm_ParentCode").AsString = ParentCode;
            int int_rows = app.DB.Exec("insert_CAEN", "CAEN", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult editCAEN(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_CAEN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_CAEN");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_RO");
            if (vl_arguments["Name_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_EN");
            if (vl_arguments["ParentCode"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ParentCode");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_CAEN = 0;
            if (vl_arguments["ID_CAEN"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_CAEN"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_CAEN"].ValueType == TVariantType.vtInt64) ID_CAEN = vl_arguments["ID_CAEN"].AsInt32;
            else res.ParamValidations["ID_CAEN"].AsString = "Value not recognized";


            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string Name_RO = "";
            if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) Name_RO = vl_arguments["Name_RO"].AsString;
            else res.ParamValidations["Name_RO"].AsString = "Value not recognized";

            string Name_EN = "";
            if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) Name_EN = vl_arguments["Name_EN"].AsString;
            else res.ParamValidations["Name_EN"].AsString = "Value not recognized";

            string ParentCode = "";
            if (vl_arguments["ParentCode"].ValueType == TVariantType.vtString) ParentCode = vl_arguments["ParentCode"].AsString;
            else res.ParamValidations["ParentCode"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_CAEN;
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_Name_RO").AsString = Name_RO;
            vl_params.Add("@prm_Name_EN").AsString = Name_EN;
            int int_rows = app.DB.Exec("update_CAEN", "CAEN", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult deleteCAEN(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_CAEN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_CAEN");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_CAEN = 0;
            if (vl_arguments["ID_CAEN"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_CAEN"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_CAEN"].ValueType == TVariantType.vtInt64) ID_CAEN = vl_arguments["ID_CAEN"].AsInt32;
            else res.ParamValidations["ID_CAEN"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_CAEN;
            int int_rows = app.DB.Exec("delete_CAEN", "CAEN", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult addTerminal(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["Administrator"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Administrator");
            if (vl_arguments["StreetAddress"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StreetAddress");
            if (vl_arguments["City"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "City");
            if (vl_arguments["ID_County"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_County");
            //if (vl_arguments["Country"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Country");
            if (vl_arguments["Phone"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Phone");
            if (vl_arguments["Fax"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Fax");
            if (vl_arguments["isActive"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isActive");


            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            string Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string Name = "";
            if (vl_arguments["Name"].ValueType == TVariantType.vtString) Name = vl_arguments["Name"].AsString;
            else res.ParamValidations["Name"].AsString = "Value not recognized";

            string Administrator = "";
            if (vl_arguments["Administrator"].ValueType == TVariantType.vtString) Administrator = vl_arguments["Administrator"].AsString;
            else res.ParamValidations["Administrator"].AsString = "Value not recognized";

            string StreetAddress = "";
            if (vl_arguments["StreetAddress"].ValueType == TVariantType.vtString) StreetAddress = vl_arguments["StreetAddress"].AsString;
            else res.ParamValidations["StreetAddress"].AsString = "Value not recognized";

            string City = "";
            if (vl_arguments["City"].ValueType == TVariantType.vtString) City = vl_arguments["City"].AsString;
            else res.ParamValidations["City"].AsString = "Value not recognized";

            int ID_County = 0;
            if (vl_arguments["ID_County"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt64) ID_County = vl_arguments["ID_County"].AsInt32;
            else res.ParamValidations["ID_County"].AsString = "Value not recognized";

            string Country = "";
            if (vl_arguments["Country"] != null)
            {
                if (vl_arguments["Country"].ValueType == TVariantType.vtString) Country = vl_arguments["Country"].AsString;
                else res.ParamValidations["Country"].AsString = "Value not recognized";
            }

            string Phone = "";
            if (vl_arguments["Phone"].ValueType == TVariantType.vtString) Phone = vl_arguments["Phone"].AsString;
            else res.ParamValidations["Phone"].AsString = "Value not recognized";

            string Fax = "";
            if (vl_arguments["Fax"].ValueType == TVariantType.vtString) Fax = vl_arguments["Fax"].AsString;
            else res.ParamValidations["Fax"].AsString = "Value not recognized";

            bool isActive = false;
            if (vl_arguments["isActive"].ValueType == TVariantType.vtBoolean) isActive = vl_arguments["isActive"].AsBoolean;
            else res.ParamValidations["isActive"].AsString = "Value type not recognized";

            string PostalCode = "";
            if (vl_arguments["PostalCode"].ValueType == TVariantType.vtString) PostalCode = vl_arguments["PostalCode"].AsString;
            else res.ParamValidations["PostalCode"].AsString = "Value not recognized";

            string Website = "";
            if (vl_arguments["Website"].ValueType == TVariantType.vtString) Website = vl_arguments["Website"].AsString;
            else res.ParamValidations["Website"].AsString = "Value not recognized";

            string Chairman = "";
            if (vl_arguments["Chairman"].ValueType == TVariantType.vtString) Chairman = vl_arguments["Chairman"].AsString;
            else res.ParamValidations["Chairman"].AsString = "Value not recognized";

            string CEO = "";
            if (vl_arguments["CEO"].ValueType == TVariantType.vtString) CEO = vl_arguments["CEO"].AsString;
            else res.ParamValidations["CEO"].AsString = "Value not recognized";

            string Email = "";
            if (vl_arguments["Email"].ValueType == TVariantType.vtString) Email = vl_arguments["Email"].AsString;
            else res.ParamValidations["Email"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params = new TVariantList();
                vl_params.Add("@prm_StreetAddress").AsString = StreetAddress;
                vl_params.Add("@prm_City").AsString = City;
                vl_params.Add("@prm_ID_County").AsInt32 = ID_County;
                vl_params.Add("@prm_PostalCode").AsString = PostalCode;
                int int_count_Address = app.DB.Exec("insert_Address", "Addresses", vl_params);
                if (int_count_Address <= 0) throw new Exception("SQL execution error");
                int ID_Address = app.DB.GetIdentity();

                vl_params = new TVariantList();
                vl_params.Add("@prm_Code").AsString = Code;
                vl_params.Add("@prm_Name").AsString = Name;
                vl_params.Add("@prm_Administrator").AsString = Administrator;
                vl_params.Add("@prm_Chairman").AsString = Chairman;
                vl_params.Add("@prm_CEO").AsString = CEO;
                vl_params.Add("@prm_isActive").AsBoolean = isActive;
                vl_params.Add("@prm_Phone").AsString = Phone;
                vl_params.Add("@prm_Fax").AsString = Fax;
                vl_params.Add("@prm_Email").AsString = Email;
                vl_params.Add("@prm_Website").AsString = Website;
                vl_params.Add("@prm_ID_Address").AsInt32 = ID_Address;
                int int_count_Terminal = app.DB.Exec("insert_Terminal", "Terminals", vl_params);
                if (int_count_Terminal <= 0) throw new Exception("SQL execution error");
                int ID_Terminal = app.DB.GetIdentity();

                int int_rows = int_count_Address + int_count_Terminal;
                
                app.DB.CommitTransaction();
                
                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }            
        }

        public TDBExecResult editTerminal(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_Terminal"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Terminal");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["Administrator"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Administrator");
            if (vl_arguments["StreetAddress"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StreetAddress");
            if (vl_arguments["City"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "City");
            if (vl_arguments["ID_County"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_County");
            //if (vl_arguments["Country"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Country");
            if (vl_arguments["Phone"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Phone");
            if (vl_arguments["Fax"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Fax");
            if (vl_arguments["isActive"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isActive");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);


            int ID_Terminal = 0;
            if (vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt64) ID_Terminal = vl_arguments["ID_Terminal"].AsInt32;
            else res.ParamValidations["ID_Terminal"].AsString = "Value not recognized";

            string Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string Name = "";
            if (vl_arguments["Name"].ValueType == TVariantType.vtString) Name = vl_arguments["Name"].AsString;
            else res.ParamValidations["Name"].AsString = "Value not recognized";

            string Administrator = "";
            if (vl_arguments["Administrator"].ValueType == TVariantType.vtString) Administrator = vl_arguments["Administrator"].AsString;
            else res.ParamValidations["Administrator"].AsString = "Value not recognized";

            string StreetAddress = "";
            if (vl_arguments["StreetAddress"].ValueType == TVariantType.vtString) StreetAddress = vl_arguments["StreetAddress"].AsString;
            else res.ParamValidations["StreetAddress"].AsString = "Value not recognized";

            string City = "";
            if (vl_arguments["City"].ValueType == TVariantType.vtString) City = vl_arguments["City"].AsString;
            else res.ParamValidations["City"].AsString = "Value not recognized";

            int ID_County = 0;
            if (vl_arguments["ID_County"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt64) ID_County = vl_arguments["ID_County"].AsInt32;
            else res.ParamValidations["ID_County"].AsString = "Value not recognized";

            string Country = "";
            if (vl_arguments["Country"] != null)
            {
                if (vl_arguments["Country"].ValueType == TVariantType.vtString) Country = vl_arguments["Country"].AsString;
                else res.ParamValidations["Country"].AsString = "Value not recognized";
            }

            string Phone = "";
            if (vl_arguments["Phone"].ValueType == TVariantType.vtString) Phone = vl_arguments["Phone"].AsString;
            else res.ParamValidations["Phone"].AsString = "Value not recognized";

            string Fax = "";
            if (vl_arguments["Fax"].ValueType == TVariantType.vtString) Fax = vl_arguments["Fax"].AsString;
            else res.ParamValidations["Fax"].AsString = "Value not recognized";

            bool isActive = false;
            if (vl_arguments["isActive"].ValueType == TVariantType.vtBoolean) isActive = vl_arguments["isActive"].AsBoolean;
            else res.ParamValidations["isActive"].AsString = "Value type not recognized";

            string PostalCode = "";
            if (vl_arguments["PostalCode"].ValueType == TVariantType.vtString) PostalCode = vl_arguments["PostalCode"].AsString;
            else res.ParamValidations["PostalCode"].AsString = "Value not recognized";

            string Website = "";
            if (vl_arguments["Website"].ValueType == TVariantType.vtString) Website = vl_arguments["Website"].AsString;
            else res.ParamValidations["Website"].AsString = "Value not recognized";

            string Chairman = "";
            if (vl_arguments["Chairman"].ValueType == TVariantType.vtString) Chairman = vl_arguments["Chairman"].AsString;
            else res.ParamValidations["Chairman"].AsString = "Value not recognized";

            string CEO = "";
            if (vl_arguments["CEO"].ValueType == TVariantType.vtString) CEO = vl_arguments["CEO"].AsString;
            else res.ParamValidations["CEO"].AsString = "Value not recognized";

            string Email = "";
            if (vl_arguments["Email"].ValueType == TVariantType.vtString) Email = vl_arguments["Email"].AsString;
            else res.ParamValidations["Email"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");                
                                
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Terminal").AsInt32 = ID_Terminal;
                DataSet ds_terminal = app.DB.Select("select_TerminalByID_Terminal", "Terminals", vl_params);
                if (ds_terminal == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
                if (ds_terminal.Tables.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
                if (ds_terminal.Tables[0].Rows.Count == 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");

                int ID_Address = Convert.ToInt32(ds_terminal.Tables[0].Rows[0]["ID_Address"]);

                int int_count_Address = 0;
                if (StreetAddress != "")
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_StreetAddress").AsString = StreetAddress;
                    vl_params.Add("@prm_City").AsString = City;
                    vl_params.Add("@prm_ID_County").AsInt32 = ID_County;
                    vl_params.Add("@prm_PostalCode").AsString = PostalCode;

                    if (ID_Address > 0)
                    {
                        vl_params.Add("@prm_ID_Address").AsInt32 = ID_Address;
                        int_count_Address = app.DB.Exec("update_Address", "Addresses", vl_params);
                        if (int_count_Address < 0) throw new Exception("SQL execution error");
                    }
                    else
                    {
                        int_count_Address = app.DB.Exec("insert_Address", "Addresses", vl_params);
                        if (int_count_Address < 0) throw new Exception("SQL execution error");
                        ID_Address = app.DB.GetIdentity();
                    }
                }

                vl_params = new TVariantList();
                vl_params.Add("@prm_Code").AsString = Code;
                vl_params.Add("@prm_Name").AsString = Name;
                vl_params.Add("@prm_Administrator").AsString = Administrator;
                vl_params.Add("@prm_Chairman").AsString = Chairman;
                vl_params.Add("@prm_CEO").AsString = CEO;
                vl_params.Add("@prm_isActive").AsBoolean = isActive;
                vl_params.Add("@prm_Phone").AsString = Phone;
                vl_params.Add("@prm_Fax").AsString = Fax;
                vl_params.Add("@prm_Email").AsString = Email;
                vl_params.Add("@prm_Website").AsString = Website;
                vl_params.Add("@prm_ID").AsInt32 = ID_Terminal;
                
                int int_count_Terminal = app.DB.Exec("update_Terminal", "Terminals", vl_params);
                if (int_count_Terminal <= 0) throw new Exception("SQL execution error");                

                int int_rows = int_count_Address + int_count_Terminal;

                app.DB.CommitTransaction();

                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }            
        }

        public TDBExecResult deleteTerminal(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_Terminal"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Terminal");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Terminal = 0;
            if (vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Terminal"].ValueType == TVariantType.vtInt64) ID_Terminal = vl_arguments["ID_Terminal"].AsInt32;
            else res.ParamValidations["ID_Terminal"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Terminal;
            int int_rows = app.DB.Exec("delete_Terminal", "Terminals", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult addDocumentType(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
                else res.ParamValidations["Name"].AsString = "Value not recognized";
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_Name").AsString = str_Name;
            int int_rows = app.DB.Exec("insert_DocumentType", "Documents", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            int ID_DocumentType = app.DB.GetIdentity();
            if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
            {
                str_Name = "fld_DocumentType_Name_" + ID_DocumentType.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);
            }

            vl_params = new TVariantList();
            vl_params.Add("@prm_Name").AsString = str_Name;
            vl_params.Add("@prm_ID").AsInt32 = ID_DocumentType;
            app.DB.Exec("update_TextFields", "DocumentTypes", vl_params);

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult editDocumentType(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_DocumentType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_DocumentType");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_DocumentType = 0;
            if (vl_arguments["ID_DocumentType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_DocumentType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_DocumentType"].ValueType == TVariantType.vtInt64) ID_DocumentType = vl_arguments["ID_DocumentType"].AsInt32;
            else res.ParamValidations["ID_DocumentType"].AsString = "Value not recognized";

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
                else res.ParamValidations["Name"].AsString = "Value not recognized";
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_DocumentType;
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_Name").AsString = str_Name;
            int int_rows = app.DB.Exec("update_DocumentType", "Documents", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
            {
                str_Name = "fld_DocumentType_Name_" + ID_DocumentType.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);
            }

            vl_params = new TVariantList();
            vl_params.Add("@prm_Name").AsString = str_Name;
            vl_params.Add("@prm_ID").AsInt32 = ID_DocumentType;
            app.DB.Exec("update_TextFields", "DocumentTypes", vl_params);

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult deleteDocumentType(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_DocumentType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_DocumentType");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_DocumentType = 0;
            if (vl_arguments["ID_DocumentType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_DocumentType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_DocumentType"].ValueType == TVariantType.vtInt64) ID_DocumentType = vl_arguments["ID_DocumentType"].AsInt32;
            else res.ParamValidations["ID_DocumentType"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_DocumentType;
            int int_rows = app.DB.Exec("delete_DocumentType", "Documents", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult addDocument(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");                        
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");            
            if (vl_arguments["ID_DocumentType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_DocumentType");
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["isPublic"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isPublic");
            //if (vl_arguments["ID_CreatedByUser"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_CreatedByUser");
            if (vl_arguments["DocumentURL"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "DocumentURL");            

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Asset = 0;
            if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
            else res.ParamValidations["ID_Asset"].AsString = "Value not recognized";
            
            int ID_DocumentType = 0;
            if (vl_arguments["ID_DocumentType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_DocumentType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_DocumentType"].ValueType == TVariantType.vtInt64) ID_DocumentType = vl_arguments["ID_DocumentType"].AsInt32;
            else res.ParamValidations["ID_DocumentType"].AsString = "Value not recognized";

            string str_Name = "";
            if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
            else res.ParamValidations["Name"].AsString = "Value not recognized";

            bool isPublic = false;
            if (vl_arguments["isPublic"].ValueType == TVariantType.vtBoolean) isPublic = vl_arguments["isPublic"].AsBoolean;
            else res.ParamValidations["isPublic"].AsString = "Value type not recognized";

            int ID_CreatedByUser = session.ID_User;
            /*if (vl_arguments["ID_CreatedByUser"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_CreatedByUser"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_CreatedByUser"].ValueType == TVariantType.vtInt64) ID_CreatedByUser = vl_arguments["ID_CreatedByUser"].AsInt32;
            else res.ParamValidations["ID_CreatedByUser"].AsString = "Value not recognized";*/

            string documentURL = "";
            if (vl_arguments["DocumentURL"].ValueType == TVariantType.vtString) documentURL = vl_arguments["DocumentURL"].AsString;
            else res.ParamValidations["DocumentURL"].AsString = "Value not recognized";

            string FileName = "";
            if (vl_arguments["FileName"] != null)
            {
                if (vl_arguments["FileName"].ValueType == TVariantType.vtString) FileName = vl_arguments["FileName"].AsString;
            }

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            vl_params.Add("@prm_ID_DocumentType").AsInt32 = ID_DocumentType;
            vl_params.Add("@prm_Name").AsString = str_Name;
            vl_params.Add("@prm_isPublic").AsBoolean = isPublic;
            vl_params.Add("@prm_DateCreated").AsDateTime = DateTime.Now;
            vl_params.Add("@prm_LastModifiedDate").AsDateTime = DateTime.Now;
            vl_params.Add("@prm_ID_CreatedByUser").AsInt32 = ID_CreatedByUser;
            vl_params.Add("@prm_DocumentURL").AsString= documentURL;
            vl_params.Add("@prm_FileName").AsString = FileName;

            int int_rows = app.DB.Exec("insert_Document", "Documents", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult editDocument(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_Document"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Document");
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");
            if (vl_arguments["ID_DocumentType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_DocumentType");
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["isPublic"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isPublic");
            //if (vl_arguments["ID_CreatedByUser"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_CreatedByUser");
            //if (vl_arguments["DocumentURL"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "DocumentURL");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Document = 0;
            if (vl_arguments["ID_Document"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Document"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Document"].ValueType == TVariantType.vtInt64) ID_Document = vl_arguments["ID_Document"].AsInt32;
            else res.ParamValidations["ID_Document"].AsString = "Value not recognized";

            int ID_Asset = 0;
            if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
            else res.ParamValidations["ID_Asset"].AsString = "Value not recognized";

            int ID_DocumentType = 0;
            if (vl_arguments["ID_DocumentType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_DocumentType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_DocumentType"].ValueType == TVariantType.vtInt64) ID_DocumentType = vl_arguments["ID_DocumentType"].AsInt32;
            else res.ParamValidations["ID_DocumentType"].AsString = "Value not recognized";

            string str_Name = "";
            if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
            else res.ParamValidations["Name"].AsString = "Value not recognized";

            bool isPublic = false;
            if (vl_arguments["isPublic"].ValueType == TVariantType.vtBoolean) isPublic = vl_arguments["isPublic"].AsBoolean;
            else res.ParamValidations["isPublic"].AsString = "Value type not recognized";

            string documentURL = "";
            if (vl_arguments["DocumentURL"] != null)
            {
                if (vl_arguments["DocumentURL"].ValueType == TVariantType.vtString) documentURL = vl_arguments["DocumentURL"].AsString;
                else res.ParamValidations["DocumentURL"].AsString = "Value not recognized";
            }

            string FileName = "";
            if (vl_arguments["FileName"] != null)
            {
                if (vl_arguments["FileName"].ValueType == TVariantType.vtString) FileName = vl_arguments["FileName"].AsString;
            }

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            vl_params.Add("@prm_ID_DocumentType").AsInt32 = ID_DocumentType;
            vl_params.Add("@prm_Name").AsString = str_Name;
            vl_params.Add("@prm_isPublic").AsBoolean = isPublic;
            vl_params.Add("@prm_LastModifiedDate").AsDateTime = DateTime.Now;
            vl_params.Add("@prm_DocumentURL").AsString = documentURL;
            vl_params.Add("@prm_FileName").AsString = FileName;                 
            vl_params.Add("@prm_ID").AsInt32 = ID_Document;

            int int_rows = app.DB.Exec("update_Document", "Documents", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            if (documentURL != "")
            {
                int_rows = app.DB.Exec("update_DocumentURL", "Documents", vl_params);
                if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            }

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult deleteDocument(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_Document"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Document");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Document = 0;
            if (vl_arguments["ID_Document"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Document"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Document"].ValueType == TVariantType.vtInt64) ID_Document = vl_arguments["ID_Document"].AsInt32;
            else res.ParamValidations["ID_Document"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Document;
            int int_rows = app.DB.Exec("delete_Document", "Documents", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult addWarrantyType(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["isValid"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isValid");
            if (vl_arguments["isAvailable4Period"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isAvailable4Period");
            if (vl_arguments["isAvailable4Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isAvailable4Asset");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
                else res.ParamValidations["Name"].AsString = "Value not recognized";
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            bool isValid = false;
            if (vl_arguments["isValid"].ValueType == TVariantType.vtBoolean) isValid = vl_arguments["isValid"].AsBoolean;
            else res.ParamValidations["isValid"].AsString = "Value type not recognized";

            bool isAvailable4Period = false;
            if (vl_arguments["isAvailable4Period"].ValueType == TVariantType.vtBoolean) isAvailable4Period = vl_arguments["isAvailable4Period"].AsBoolean;
            else res.ParamValidations["isAvailable4Period"].AsString = "Value type not recognized";

            bool isAvailable4Asset = false;
            if (vl_arguments["isAvailable4Asset"].ValueType == TVariantType.vtBoolean) isAvailable4Asset = vl_arguments["isAvailable4Asset"].AsBoolean;
            else res.ParamValidations["isAvailable4Asset"].AsString = "Value type not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_Name").AsString = str_Name;
            vl_params.Add("@prm_isValid").AsBoolean= isValid;
            vl_params.Add("@prm_isAvailable4Period").AsBoolean = isAvailable4Period;
            vl_params.Add("@prm_isAvailable4Asset").AsBoolean = isAvailable4Asset;

            int int_rows = app.DB.Exec("insert_WarrantyType", "WarrantyTypes", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            int ID_WarrantyType = app.DB.GetIdentity();
            if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
            {
                str_Name = "fld_WarrantyType_Name_" + ID_WarrantyType.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);
            }

            vl_params = new TVariantList();
            vl_params.Add("@prm_Name").AsString = str_Name;
            vl_params.Add("@prm_ID").AsInt32 = ID_WarrantyType;
            app.DB.Exec("update_TextFields", "WarrantyTypes", vl_params);

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult editWarrantyType(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_WarrantyType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_WarrantyType");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null && vl_arguments["Name_EN"] == null && vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["isValid"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isValid");
            if (vl_arguments["isAvailable4Period"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isAvailable4Period");
            if (vl_arguments["isAvailable4Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isAvailable4Asset");
                              
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_WarrantyType = 0;
            if (vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt64) ID_WarrantyType = vl_arguments["ID_WarrantyType"].AsInt32;
            else res.ParamValidations["ID_WarrantyType"].AsString = "Value not recognized";

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string str_Name = "";
            if (vl_arguments["Name"] != null)
            {
                if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
                else res.ParamValidations["Name"].AsString = "Value not recognized";
            }

            string str_Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) str_Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string str_Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) str_Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            bool isValid = false;
            if (vl_arguments["isValid"].ValueType == TVariantType.vtBoolean) isValid = vl_arguments["isValid"].AsBoolean;
            else res.ParamValidations["isValid"].AsString = "Value type not recognized";

            bool isAvailable4Period = false;
            if (vl_arguments["isAvailable4Period"].ValueType == TVariantType.vtBoolean) isAvailable4Period = vl_arguments["isAvailable4Period"].AsBoolean;
            else res.ParamValidations["isAvailable4Period"].AsString = "Value type not recognized";

            bool isAvailable4Asset = false;
            if (vl_arguments["isAvailable4Asset"].ValueType == TVariantType.vtBoolean) isAvailable4Asset = vl_arguments["isAvailable4Asset"].AsBoolean;
            else res.ParamValidations["isAvailable4Asset"].AsString = "Value type not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_WarrantyType;
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_Name").AsString = str_Name;
            vl_params.Add("@prm_isValid").AsBoolean = isValid;
            vl_params.Add("@prm_isAvailable4Period").AsBoolean = isAvailable4Period;
            vl_params.Add("@prm_isAvailable4Asset").AsBoolean = isAvailable4Asset;

            int int_rows = app.DB.Exec("update_WarrantyType", "WarrantyTypes", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            if (str_Name == "" && (str_Name_EN != "" || str_Name_RO != ""))
            {
                str_Name = "fld_WarrantyType_Name_" + ID_WarrantyType.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = str_Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = str_Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);
            }

            vl_params = new TVariantList();
            vl_params.Add("@prm_Name").AsString = str_Name;
            vl_params.Add("@prm_ID").AsInt32 = ID_WarrantyType;
            app.DB.Exec("update_TextFields", "WarrantyTypes", vl_params);
            
            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult deleteWarrantyType(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_WarrantyType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_WarrantyType");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_WarrantyType = 0;
            if (vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt64) ID_WarrantyType = vl_arguments["ID_WarrantyType"].AsInt32;
            else res.ParamValidations["ID_WarrantyType"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_WarrantyType;
            int int_rows = app.DB.Exec("delete_WarrantyType", "WarrantyTypes", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult setWarrantyType2Ring(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_WarrantyType"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["isDeleted"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            int ID_WarrantyType = ((vl_arguments["ID_WarrantyType"] != null) && (vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtByte ||
                                                                    vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt16 ||
                                                                    vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt32 ||
                                                                    vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_WarrantyType"].AsInt32 : 0;

            int ID_Ring = ((vl_arguments["ID_Ring"] != null) && (vl_arguments["ID_Ring"].ValueType == TVariantType.vtByte ||
                                                        vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                                                        vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                                                        vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_Ring"].AsInt32 : 0;

            bool isDeleted = false;
            if (vl_arguments["isDeleted"].ValueType == TVariantType.vtBoolean) isDeleted = vl_arguments["isDeleted"].AsBoolean;

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            if (ID_WarrantyType == 0)
                res.ParamValidations["ID_WarrantyType"].AsString = "Value type not recognized";

            if (ID_Ring == 0)
                res.ParamValidations["ID_Ring"].AsString = "Value type not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "Database transaction error");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("@prm_ID_WarrantyType").AsInt32 = ID_WarrantyType;
                vl_params.Add("@prm_isDeleted").AsBoolean = isDeleted;
                int int_count = app.DB.Exec("set_RingXWarrantyType", "Rings", vl_params);
                if (int_count <= 0) throw new Exception("Ring WarrantyType set failed");
                app.DB.CommitTransaction();

                //if (isDeleted == false)
                //{
                //    //update all assets with this new kind of warrantytype
                //    vl_params = new TVariantList();
                //    vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                //    vl_params.Add("@prm_ID_Market").AsInt32 = -1;
                //    vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                //    vl_params.Add("@prm_ID").AsInt32 = -1;
                //    vl_params.Add("@prm_ID_AssetType").AsInt32 = -1;
                //    vl_params.Add("@prm_AnyStatus").AsBoolean = true;
                //    vl_params.Add("@prm_All").AsBoolean = true;
                //    DataSet ds_assets = getAssets(vl_params);
                //    if (app.DB.ValidDSRows(ds_assets))
                //    {
                //        for (int i = 0; i < ds_assets.Tables[0].Rows.Count; i++)
                //        {
                //            int assetID = Convert.ToInt32(ds_assets.Tables[0].Rows[i]["ID"]);
                //            vl_params = new TVariantList();
                //            vl_params.Add("ID_Asset").AsInt32 = assetID;
                //            vl_params.Add("ID_WarrantyType").AsInt32 = ID_WarrantyType;
                //            vl_params.Add("isDeleted").AsBoolean = isDeleted;
                //            setWarrantyType2Asset(vl_params);
                //        }
                //    }
                //}
            }
            catch
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Ring WarrantyType set failed");
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult setWarrantyTypePriority(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_WarrantyType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_WarrantyType");
            if (vl_arguments["ID_Ring"] == null && vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Ring/ID_Asset");
            if (vl_arguments["Offset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Offset");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_WarrrantyType = 0;
            if (vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt64) ID_WarrrantyType = vl_arguments["ID_WarrantyType"].AsInt32;
            else res.ParamValidations["ID_WarrantyType"].AsString = "Value not recognized";

            int ID_Ring = 0;
            int ID_Asset = 0;
            if (vl_arguments["ID_Ring"] != null)
            {
                if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
                else res.ParamValidations["ID_Ring"].AsString = "Value not recognized";
            }

            if (vl_arguments["ID_Asset"] != null)
            {
                if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
                else res.ParamValidations["ID_Asset"].AsString = "Value not recognized";
            }

            int Offset = 0;
            if (vl_arguments["Offset"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Offset"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Offset"].ValueType == TVariantType.vtInt64) Offset = vl_arguments["Offset"].AsInt32;
            else res.ParamValidations["Offset"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "Database transaction error");

            res = new TDBExecResult(TDBExecError.Success, "");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_WarrantyType").AsInt32 = ID_WarrrantyType;
                vl_params.Add("@prm_Offset").AsInt32 = Offset;

                if (ID_Ring != 0)
                {
                    vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;

                    app.DB.Exec("setWarrantyTypePriority", "RingsXWarrantyTypes", vl_params);
                }
                else if (ID_Asset != 0)
                {
                    vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;

                    app.DB.Exec("setWarrantyTypePriority", "AssetsXWarrantyTypes", vl_params);
                }

                app.DB.CommitTransaction();
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Warranty Type priority set failed");
            }            

            return res;
        }

        /*public TDBExecResult setWarrantyType2Ring(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_WarrantyType"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["isDeleted"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            int ID_WarrantyType = ((vl_arguments["ID_WarrantyType"] != null) && (vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtByte ||
                                                                    vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt16 ||
                                                                    vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt32 ||
                                                                    vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_WarrantyType"].AsInt32 : 0;

            int ID_Ring = ((vl_arguments["ID_Ring"] != null) && (vl_arguments["ID_Ring"].ValueType == TVariantType.vtByte ||
                                                        vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                                                        vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                                                        vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_Ring"].AsInt32 : 0;

            bool isDeleted = false;
            if (vl_arguments["isDeleted"].ValueType == TVariantType.vtBoolean) isDeleted = vl_arguments["isDeleted"].AsBoolean;

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            if (ID_WarrantyType == 0)
                res.ParamValidations["ID_WarrantyType"].AsString = "Value type not recognized";

            if (ID_Ring == 0)
                res.ParamValidations["ID_Ring"].AsString = "Value type not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "Database transaction error");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("@prm_ID_WarrantyType").AsInt32 = ID_WarrantyType;
                vl_params.Add("@prm_isDeleted").AsBoolean = isDeleted;
                int int_count = app.DB.Exec("set_RingXWarrantyType", "Rings", vl_params);
                if (int_count <= 0) throw new Exception("Ring WarrantyType set failed");
                app.DB.CommitTransaction();

                if (isDeleted == false)
                {
                    //update all assets with this new kind of warrantytype
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                    vl_params.Add("@prm_ID_Market").AsInt32 = -1;
                    vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                    vl_params.Add("@prm_ID").AsInt32 = -1;
                    vl_params.Add("@prm_ID_AssetType").AsInt32 = -1;
                    vl_params.Add("@prm_AnyStatus").AsBoolean = true;
                    vl_params.Add("@prm_All").AsBoolean = true;
                    DataSet ds_assets = getAssets(vl_params);
                    if (app.DB.ValidDSRows(ds_assets))
                    {
                        for (int i = 0; i < ds_assets.Tables[0].Rows.Count; i++)
                        {
                            int assetID = Convert.ToInt32(ds_assets.Tables[0].Rows[i]["ID"]);
                            vl_params = new TVariantList();
                            vl_params.Add("ID_Asset").AsInt32 = assetID;
                            vl_params.Add("ID_WarrantyType").AsInt32 = ID_WarrantyType;
                            vl_params.Add("isDeleted").AsBoolean = isDeleted;
                            setWarrantyType2Asset(vl_params);
                        }
                    }
                }
            }
            catch
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Ring WarrantyType set failed");
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }*/

        public TDBExecResult setWarrantyType2Asset(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_WarrantyType"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            if (vl_arguments["isDeleted"] == null) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            int ID_WarrantyType = ((vl_arguments["ID_WarrantyType"] != null) && (vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtByte ||
                                                                    vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt16 ||
                                                                    vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt32 ||
                                                                    vl_arguments["ID_WarrantyType"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_WarrantyType"].AsInt32 : 0;

            int ID_Asset = ((vl_arguments["ID_Asset"] != null) && (vl_arguments["ID_Asset"].ValueType == TVariantType.vtByte ||
                                                        vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                                                        vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                                                        vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64)) ? vl_arguments["ID_Asset"].AsInt32 : 0;

            bool isDeleted = false;
            if (vl_arguments["isDeleted"].ValueType == TVariantType.vtBoolean) isDeleted = vl_arguments["isDeleted"].AsBoolean;

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Some arguments have invalid values.");
            res.setParamValidations(vl_arguments);

            if (ID_WarrantyType == 0)
                res.ParamValidations["ID_WarrantyType"].AsString = "Value type not recognized";

            if (ID_Asset == 0)
                res.ParamValidations["ID_Asset"].AsString = "Value type not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "Database transaction error");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_ID_WarrantyType").AsInt32 = ID_WarrantyType;
                vl_params.Add("@prm_isDeleted").AsBoolean = isDeleted;
                int int_count = app.DB.Exec("set_AssetXWarrantyType", "Rings", vl_params);
                if (int_count <= 0) throw new Exception("Asset WarrantyType set failed");

                app.DB.CommitTransaction();
            }
            catch
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Asset WarrantyType set failed");
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }
        
        public TDBExecResult addWarranty(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_PaymentType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_PaymentType");
            if (vl_arguments["WarrantyNumber"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "WarrantyNumber");
            if (vl_arguments["ID_Client"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Client");
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");
            if (vl_arguments["ID_CreatedByUser"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_CreatedByUser");
            if (vl_arguments["ID_Currency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Currency");
            //if (vl_arguments["ValueInCurrency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ValueInCurrency");
            if (vl_arguments["ValueInCurrency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ValueInCurrency");
            if (vl_arguments["ExchangeRate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ExchangeRate");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_PaymentType = 0;
            if (vl_arguments["ID_PaymentType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_PaymentType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_PaymentType"].ValueType == TVariantType.vtInt64) ID_PaymentType = vl_arguments["ID_PaymentType"].AsInt32;
            else res.ParamValidations["ID_PaymentType"].AsString = "Value not recognized";

            string WarrantyNumber = "";
            if (vl_arguments["WarrantyNumber"].ValueType == TVariantType.vtString) WarrantyNumber = vl_arguments["WarrantyNumber"].AsString;
            //else res.ParamValidations["WarrantyNumber"].AsString = "Value not recognized";

            int ID_Client = 0;
            if (vl_arguments["ID_Client"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Client"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Client"].ValueType == TVariantType.vtInt64) ID_Client = vl_arguments["ID_Client"].AsInt32;
            else res.ParamValidations["ID_Client"].AsString = "Value not recognized";

            int ID_Asset = 0;
            if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
            else res.ParamValidations["ID_Asset"].AsString = "Value not recognized";

            int ID_CreatedByUser = 0;
            if (vl_arguments["ID_CreatedByUser"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_CreatedByUser"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_CreatedByUser"].ValueType == TVariantType.vtInt64) ID_CreatedByUser = vl_arguments["ID_CreatedByUser"].AsInt32;
            else res.ParamValidations["ID_CreatedByUser"].AsString = "Value not recognized";

            int ID_Currency = 0;
            if (vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt64) ID_Currency = vl_arguments["ID_Currency"].AsInt32;
            else res.ParamValidations["ID_Currency"].AsString = "Value not recognized";

            /*double ValueInRON = 0;
            if (vl_arguments["ValueInRON"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ValueInRON"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ValueInRON"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["ValueInRON"].ValueType == TVariantType.vtSingle ||
                vl_arguments["ValueInRON"].ValueType == TVariantType.vtDouble) ValueInRON = vl_arguments["ValueInRON"].AsDouble;
            else res.ParamValidations["ValueInRON"].AsString = "Unrecognized Value";
            if (ValueInRON <= 0 && res.ParamValidations["ValueInRON"].AsString == "") res.ParamValidations["ValueInRON"].AsString = " Value cannot be negative";*/

            double ValueInCurrency = 0;
            if (vl_arguments["ValueInCurrency"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ValueInCurrency"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ValueInCurrency"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["ValueInCurrency"].ValueType == TVariantType.vtSingle ||
                vl_arguments["ValueInCurrency"].ValueType == TVariantType.vtDouble) ValueInCurrency = vl_arguments["ValueInCurrency"].AsDouble;
            else res.ParamValidations["ValueInCurrency"].AsString = "Unrecognized Value";
            if (ValueInCurrency <= 0 && res.ParamValidations["ValueInCurrency"].AsString == "") res.ParamValidations["ValueInCurrency"].AsString = " Value cannot be negative";

            double ExchangeRate = 0;
            if (vl_arguments["ExchangeRate"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ExchangeRate"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ExchangeRate"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["ExchangeRate"].ValueType == TVariantType.vtSingle ||
                vl_arguments["ExchangeRate"].ValueType == TVariantType.vtDouble) ExchangeRate = vl_arguments["ExchangeRate"].AsDouble;
            else res.ParamValidations["ExchangeRate"].AsString = "Unrecognized Value";
            if (ExchangeRate <= 0 && res.ParamValidations["ExchangeRate"].AsString == "") res.ParamValidations["ExchangeRate"].AsString = " Value cannot be negative";

            //optional fields

            int ID_Agency = 0;
            if (vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt64) ID_Agency = vl_arguments["ID_Agency"].AsInt32;
            else res.ParamValidations["ID_Agency"].AsString = "Value not recognized";

            DateTime ValabilityStartDate = DateTime.Now.AddDays(-10000);
            if (vl_arguments["ValabilityStartDate"].ValueType == TVariantType.vtDateTime) ValabilityStartDate = vl_arguments["ValabilityStartDate"].AsDateTime;

            DateTime ValabilityEndDate = DateTime.Now.AddDays(-10000);
            if (vl_arguments["ValabilityEndDate"].ValueType == TVariantType.vtDateTime) ValabilityEndDate = vl_arguments["ValabilityEndDate"].AsDateTime;

            DateTime ExecutionDate = DateTime.Now.AddDays(-10000);
            if (vl_arguments["ExecutionDate"].ValueType == TVariantType.vtDateTime) ExecutionDate = vl_arguments["ExecutionDate"].AsDateTime;

            if (ExecutionDate < ValabilityStartDate) res.ParamValidations["ExecutionDate"].AsString = "Data de executare nu poate fi inainte de inceputul perioadei de valabilitate";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_OperationType").AsString = "CRED_WAR";
                vl_params.Add("@prm_isClosed").AsBoolean = true;
                vl_params.Add("@prm_isCanceled").AsBoolean = false;
                vl_params.Add("@prm_Reference").AsString = WarrantyNumber;

                int int_rows_operation = app.DB.Exec("insert_BO_Operation", "BO_Operations", vl_params);
                if (int_rows_operation <= 0) throw new Exception("SQL execution error");  

                int ID_Operation = app.DB.GetIdentity();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_PaymentType").AsInt32 = ID_PaymentType;
                vl_params.Add("@prm_PaymentDescription").AsString = "";
                vl_params.Add("@prm_Sum").AsDouble = ValueInCurrency;
                vl_params.Add("@prm_Percent").AsDouble = 1;
                vl_params.Add("@prm_isPayed").AsBoolean = true;
                vl_params.Add("@prm_PaymentDate").AsDateTime = DateTime.Now;
                vl_params.Add("@prm_ID_Currency").AsInt32 = ID_Currency;
                vl_params.Add("@prm_ValabilityStartDate").AsDateTime = ValabilityStartDate;
                vl_params.Add("@prm_ValabilityEndDate").AsDateTime = ValabilityEndDate;
                vl_params.Add("@prm_ExecutionDate").AsDateTime = ExecutionDate;
                vl_params.Add("@prm_ExchangeRate").AsDouble = ExchangeRate;

                int int_rows_payments = app.DB.Exec("insert_BO_OperationDetails_Payments", "BO_OperationDetails_Payments", vl_params);
                if (int_rows_payments <= 0) throw new Exception("SQL execution error");  

                int ID_OperationDetail = app.DB.GetIdentity();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Operation").AsInt32 = ID_Operation;
                vl_params.Add("@prm_ID_Parent").AsInt32 = 0;
                vl_params.Add("@prm_ID_InterNode").AsInt32 = 0;
                vl_params.Add("@prm_Date").AsDateTime = DateTime.Now;
                vl_params.Add("@prm_ObjectType").AsInt32 = 2; //Retailer logic. 1-TDA, 2-TDP
                vl_params.Add("@prm_Description").AsString = "Payment";
                vl_params.Add("@prm_ID_OperationDetail").AsInt32 = ID_OperationDetail;
                vl_params.Add("@prm_ID_ClientSRC").AsInt32 = ID_Client;
                vl_params.Add("@prm_ID_ClientDEST").AsInt32 = 0;
                vl_params.Add("@prm_ID_AgencySRC").AsInt32 = 0;
                vl_params.Add("@prm_ID_AgencyDEST").AsInt32 = 0;
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_ID_CreatedByUser").AsInt32 = ID_CreatedByUser;
                vl_params.Add("@prm_isCanceled").AsInt32 = 0;

                //int int_rows = app.DB.Exec("insert_Warranty", "Warranties", vl_params);  

                int int_rows_details = app.DB.Exec("insert_BO_OperationXDetails", "BO_OperationsXDetails", vl_params);
                if (int_rows_details <= 0) throw new Exception("SQL execution error");

                int int_rows = int_rows_operation + int_rows_payments + int_rows_details;

                app.DB.CommitTransaction();

                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }  

        }

        public TDBExecResult editWarranty(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_BOperation"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_BOperation");
            if (vl_arguments["ID_PaymentType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_PaymentType");
            if (vl_arguments["WarrantyNumber"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "WarrantyNumber");
            if (vl_arguments["ID_Client"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Client");
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");
            if (vl_arguments["ID_CreatedByUser"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_CreatedByUser");
            if (vl_arguments["ID_Currency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Currency");
            if (vl_arguments["ValueInCurrency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ValueInCurrency");
            if (vl_arguments["ExchangeRate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ExchangeRate");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_BOperation = 0;
            if (vl_arguments["ID_BOperation"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_BOperation"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_BOperation"].ValueType == TVariantType.vtInt64) ID_BOperation = vl_arguments["ID_BOperation"].AsInt32;
            else res.ParamValidations["ID_BOperation"].AsString = "Value not recognized";

            int ID_PaymentType = 0;
            if (vl_arguments["ID_PaymentType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_PaymentType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_PaymentType"].ValueType == TVariantType.vtInt64) ID_PaymentType = vl_arguments["ID_PaymentType"].AsInt32;
            else res.ParamValidations["ID_PaymentType"].AsString = "Value not recognized";

            string WarrantyNumber = "";
            if (vl_arguments["WarrantyNumber"].ValueType == TVariantType.vtString) WarrantyNumber = vl_arguments["WarrantyNumber"].AsString;

            int ID_Client = 0;
            if (vl_arguments["ID_Client"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Client"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Client"].ValueType == TVariantType.vtInt64) ID_Client = vl_arguments["ID_Client"].AsInt32;
            else res.ParamValidations["ID_Client"].AsString = "Value not recognized";

            int ID_Asset = 0;
            if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
            else res.ParamValidations["ID_Asset"].AsString = "Value not recognized";

            int ID_CreatedByUser = 0;
            if (vl_arguments["ID_CreatedByUser"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_CreatedByUser"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_CreatedByUser"].ValueType == TVariantType.vtInt64) ID_CreatedByUser = vl_arguments["ID_CreatedByUser"].AsInt32;
            else res.ParamValidations["ID_CreatedByUser"].AsString = "Value not recognized";

            int ID_Currency = 0;
            if (vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Currency"].ValueType == TVariantType.vtInt64) ID_Currency = vl_arguments["ID_Currency"].AsInt32;
            else res.ParamValidations["ID_Currency"].AsString = "Value not recognized";

            /*double ValueInRON = 0;
            if (vl_arguments["ValueInRON"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ValueInRON"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ValueInRON"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["ValueInRON"].ValueType == TVariantType.vtSingle ||
                vl_arguments["ValueInRON"].ValueType == TVariantType.vtDouble) ValueInRON = vl_arguments["ValueInRON"].AsDouble;
            else res.ParamValidations["ValueInRON"].AsString = "Unrecognized Value";
            if (ValueInRON <= 0 && res.ParamValidations["ValueInRON"].AsString == "") res.ParamValidations["ValueInRON"].AsString = " Value cannot be negative";*/

            double ValueInCurrency = 0;
            if (vl_arguments["ValueInCurrency"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ValueInCurrency"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ValueInCurrency"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["ValueInCurrency"].ValueType == TVariantType.vtSingle ||
                vl_arguments["ValueInCurrency"].ValueType == TVariantType.vtDouble) ValueInCurrency = vl_arguments["ValueInCurrency"].AsDouble;
            else res.ParamValidations["ValueInCurrency"].AsString = "Unrecognized Value";
            if (ValueInCurrency <= 0 && res.ParamValidations["ValueInCurrency"].AsString == "") res.ParamValidations["ValueInCurrency"].AsString = " Value cannot be negative";

            double ExchangeRate = 0;
            if (vl_arguments["ExchangeRate"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ExchangeRate"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ExchangeRate"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["ExchangeRate"].ValueType == TVariantType.vtSingle ||
                vl_arguments["ExchangeRate"].ValueType == TVariantType.vtDouble) ExchangeRate = vl_arguments["ExchangeRate"].AsDouble;
            else res.ParamValidations["ExchangeRate"].AsString = "Unrecognized Value";
            if (ExchangeRate <= 0 && res.ParamValidations["ExchangeRate"].AsString == "") res.ParamValidations["ExchangeRate"].AsString = " Value cannot be negative";

            //optional fields

            int ID_Agency = 0;
            if (vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Agency"].ValueType == TVariantType.vtInt64) ID_Agency = vl_arguments["ID_Agency"].AsInt32;
            else res.ParamValidations["ID_Agency"].AsString = "Value not recognized";

            DateTime ValabilityStartDate = DateTime.Now.AddDays(-10000);
            if (vl_arguments["ValabilityStartDate"].ValueType == TVariantType.vtDateTime) ValabilityStartDate = vl_arguments["ValabilityStartDate"].AsDateTime;

            DateTime ValabilityEndDate = DateTime.Now.AddDays(-10000);
            if (vl_arguments["ValabilityEndDate"].ValueType == TVariantType.vtDateTime) ValabilityEndDate = vl_arguments["ValabilityEndDate"].AsDateTime;

            DateTime ExecutionDate = DateTime.Now.AddDays(-10000);
            if (vl_arguments["ExecutionDate"].ValueType == TVariantType.vtDateTime) ExecutionDate = vl_arguments["ExecutionDate"].AsDateTime;

            if (ExecutionDate > ValabilityEndDate) res.ParamValidations["ExecutionDate"].AsString = "Data de executare nu poate fi mai mare decat finalul perioadei de valabilitate";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_Reference").AsString = WarrantyNumber;
                vl_params.Add("@prm_ID_BOperation").AsInt32 = ID_BOperation;

                int int_rows_operations = app.DB.Exec("update_BO_Operation", "BO_Operations", vl_params);
                if (int_rows_operations <= 0) throw new Exception("SQL execution error");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_ClientSRC").AsInt32 = ID_Client;
                vl_params.Add("@prm_ID_ClientDEST").AsInt32 = 0;
                vl_params.Add("@prm_ID_AgencySRC").AsInt32 = ID_Agency;
                vl_params.Add("@prm_ID_AgencyDEST").AsInt32 = ID_Agency;
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("@prm_ID_CreatedByUser").AsInt32 = ID_CreatedByUser;
                vl_params.Add("@prm_ID_BOperation").AsInt32 = ID_BOperation;

                int int_rows_details = app.DB.Exec("update_BO_OperationXDetails", "BO_OperationsXDetails", vl_params);
                if (int_rows_details <= 0) throw new Exception("SQL execution error");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Currency").AsInt32 = ID_Currency;
                vl_params.Add("@prm_Sum").AsDouble = ValueInCurrency;
                vl_params.Add("@prm_ExchangeRate").AsDouble = ExchangeRate;
                vl_params.Add("@prm_ValabilityStartDate").AsDateTime = ValabilityStartDate;
                vl_params.Add("@prm_ValabilityEndDate").AsDateTime = ValabilityEndDate;
                vl_params.Add("@prm_ExecutionDate").AsDateTime = ExecutionDate;
                vl_params.Add("@prm_ID_PaymentType").AsInt32 = ID_PaymentType;
                vl_params.Add("@prm_ID_BOperation").AsInt32 = ID_BOperation;

                int int_rows_payments = app.DB.Exec("update_BO_OperationDetails_Payments", "BO_OperationDetails_Payments", vl_params);
                if (int_rows_payments <= 0) throw new Exception("SQL execution error");                

                app.DB.CommitTransaction();

                int int_rows = int_rows_operations + int_rows_details + int_rows_payments;

                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }  
        }

        public TDBExecResult deleteWarranty(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_BOperation"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_BOperation");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_BOperation = 0;
            if (vl_arguments["ID_BOperation"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_BOperation"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_BOperation"].ValueType == TVariantType.vtInt64) ID_BOperation = vl_arguments["ID_BOperation"].AsInt32;
            else res.ParamValidations["ID_BOperation"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_BOperation;

            int int_rows = app.DB.Exec("delete_BO_OperationXDetails", "BO_OperationsXDetails", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            int_rows = app.DB.Exec("delete_BO_Operation", "BO_Operations", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult addTranslation(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["Label"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Label");
            if (vl_arguments["Value_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Value_EN");
            if (vl_arguments["Value_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Value_RO");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            string Label = "";
            if (vl_arguments["Label"].ValueType == TVariantType.vtString) Label = vl_arguments["Label"].AsString;
            else res.ParamValidations["Label"].AsString = "Value not recognized";

            string Value_EN = "";
            if (vl_arguments["Value_EN"].ValueType == TVariantType.vtString) Value_EN = vl_arguments["Value_EN"].AsString;
            else res.ParamValidations["Value_EN"].AsString = "Value not recognized";

            string Value_RO = "";
            if (vl_arguments["Value_RO"].ValueType == TVariantType.vtString) Value_RO = vl_arguments["Value_RO"].AsString;
            else res.ParamValidations["Value_RO"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Label").AsString = Label;
            vl_params.Add("@prm_ID_Translation").AsInt32 = -1;
            DataSet ds_Translation = app.DB.Select("select_Translations", "Translations", vl_params);
            if ((ds_Translation != null) && (ds_Translation.Tables.Count > 0) && (ds_Translation.Tables[0].Rows.Count > 0))
                return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Label-ul exista deja in baza de date!");

            vl_params = new TVariantList();
            vl_params.Add("@prm_Label").AsString = Label;
            vl_params.Add("@prm_Value_EN").AsString = Value_EN;
            vl_params.Add("@prm_Value_RO").AsString = Value_RO;
            int int_rows = app.DB.Exec("insert_Translation", "Translations", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult editTranslation(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_Translation"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Translation");
            if (vl_arguments["Label"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Label");
            if (vl_arguments["Value_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Value_EN");
            if (vl_arguments["Value_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Value_RO");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Translation = 0;
            if (vl_arguments["ID_Translation"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Translation"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Translation"].ValueType == TVariantType.vtInt64) ID_Translation = vl_arguments["ID_Translation"].AsInt32;
            else res.ParamValidations["ID_Translation"].AsString = "Value not recognized";

            string Label = "";
            if (vl_arguments["Label"].ValueType == TVariantType.vtString) Label = vl_arguments["Label"].AsString;
            else res.ParamValidations["Label"].AsString = "Value not recognized";

            string Value_EN = "";
            if (vl_arguments["Value_EN"].ValueType == TVariantType.vtString) Value_EN = vl_arguments["Value_EN"].AsString;
            else res.ParamValidations["Value_EN"].AsString = "Value not recognized";

            string Value_RO = "";
            if (vl_arguments["Value_RO"].ValueType == TVariantType.vtString) Value_RO = vl_arguments["Value_RO"].AsString;
            else res.ParamValidations["Value_RO"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;


            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Label").AsString = Label;
            vl_params.Add("@prm_ID_Translation").AsInt32 = -1;
            DataSet ds_Translation = app.DB.Select("select_Translations", "Translations", vl_params);
            if (app.DB.ValidDSRows(ds_Translation))
                if (Convert.ToInt32(ds_Translation.Tables[0].Rows[0]["ID"]) != ID_Translation)
                    return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Label-ul exista deja in baza de date!");
            
            vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Translation;
            vl_params.Add("@prm_Label").AsString = Label;
            vl_params.Add("@prm_Value_EN").AsString = Value_EN;
            vl_params.Add("@prm_Value_RO").AsString = Value_RO;
            int int_rows = app.DB.Exec("update_Translation", "Translations", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult deleteTranslation(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_Translation"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Translation");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Translation = 0;
            if (vl_arguments["ID_Translation"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Translation"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Translation"].ValueType == TVariantType.vtInt64) ID_Translation = vl_arguments["ID_Translation"].AsInt32;
            else res.ParamValidations["ID_Translation"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Translation;
            int int_rows = app.DB.Exec("delete_Translation", "Translations", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult addCounty(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["ID_Country"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Country");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            string str_Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) str_Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string Name = "";
            if (vl_arguments["Name"].ValueType == TVariantType.vtString) Name = vl_arguments["Name"].AsString;
            else res.ParamValidations["Name"].AsString = "Value not recognized";

            int ID_Country = 0;
            if (vl_arguments["ID_Country"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Country"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Country"].ValueType == TVariantType.vtInt64) ID_Country = vl_arguments["ID_Country"].AsInt32;
            else res.ParamValidations["ID_Country"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Code").AsString = str_Code;
            vl_params.Add("@prm_Name").AsString = Name;
            vl_params.Add("@prm_ID_Country").AsInt32 = ID_Country;
            int int_rows = app.DB.Exec("insert_County", "Counties", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult editCounty(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_County"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_County");
            if (vl_arguments["ID_County"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_County");
            if (vl_arguments["Code"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Code");
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_County = 0;
            if (vl_arguments["ID_County"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt64) ID_County = vl_arguments["ID_County"].AsInt32;
            else res.ParamValidations["ID_County"].AsString = "Value not recognized";


            string Code = "";
            if (vl_arguments["Code"].ValueType == TVariantType.vtString) Code = vl_arguments["Code"].AsString;
            else res.ParamValidations["Code"].AsString = "Value not recognized";

            string Name = "";
            if (vl_arguments["Name"].ValueType == TVariantType.vtString) Name = vl_arguments["Name"].AsString;
            else res.ParamValidations["Name"].AsString = "Value not recognized";

            int ID_Country = 0;
            if (vl_arguments["ID_Country"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Country"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Country"].ValueType == TVariantType.vtInt64) ID_Country = vl_arguments["ID_Country"].AsInt32;
            else res.ParamValidations["ID_Country"].AsString = "Value not recognized";


            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_County;
            vl_params.Add("@prm_Code").AsString = Code;
            vl_params.Add("@prm_Name").AsString = Name;
            vl_params.Add("@prm_ID_Country").AsInt32 = ID_Country;
            int int_rows = app.DB.Exec("update_County", "Counties", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult deleteCounty(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_County"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_County");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_County = 0;
            if (vl_arguments["ID_County"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_County"].ValueType == TVariantType.vtInt64) ID_County = vl_arguments["ID_County"].AsInt32;
            else res.ParamValidations["ID_County"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_County;
            int int_rows = app.DB.Exec("delete_County", "Counties", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }
        
        public TDBExecResult changePassword(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_User"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_User");

            //  validation
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_User = 0;
            try { ID_User = vl_arguments["ID_User"].AsInt32; }
            catch { res.ParamValidations["ID_User"].AsString = "Value not recognized"; }

            string LoginPassword = "";
            try { LoginPassword = (vl_arguments["LoginPassword"] == null) ? "" : vl_arguments["LoginPassword"].AsString; }
            catch { res.ParamValidations["LoginPassword"].AsString = "Value not recognized"; }

            string LoginPasswordConfirm = "";
            try { LoginPasswordConfirm = (vl_arguments["LoginPasswordConfirm"] == null) ? "" : vl_arguments["LoginPasswordConfirm"].AsString; }
            catch { res.ParamValidations["LoginPasswordConfirm"].AsString = "Value not recognized"; }

            if (LoginPassword != LoginPasswordConfirm)
                res.ParamValidations["LoginPassword"].AsString = "Login Password and Confirm Login Password doesn't match!";

            if (res.bInvalid()) return res;


            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_User").AsInt32 = ID_User;
                vl_params.Add("@prm_LoginPassword").AsString = LoginPassword;

                int int_updated = app.DB.Exec("update_UserPassword", "Users", vl_params);
                if (int_updated <= 0) throw new Exception("Password changing failed");

                res.RowsModified = int_updated;
                return res;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult addTransaction(TVariantList vl_arguments)
        {

            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Ring");
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");
            if (vl_arguments["ID_BuyClient"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_BuyClient");
            if (vl_arguments["ID_SellClient"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_SellClient");
            if (vl_arguments["ID_AgencyBuy"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AgencyBuy");
            if (vl_arguments["ID_AgencySell"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AgencySell");
            if (vl_arguments["NewOrderSell"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "NewOrderSell");
            if (vl_arguments["NewOrderBuy"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "NewOrderBuy");
            if (vl_arguments["OrderBuyQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderBuyQuantity");
            if (vl_arguments["OrderBuyPrice"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderBuyPrice");
            if (vl_arguments["IDOrderBuy"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "IDOrderBuy");
            if (vl_arguments["IDOrderBuyQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "IDOrderBuyQuantity");
            if (vl_arguments["BuyPosition"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "BuyPosition");
            if (vl_arguments["OrderBuyIsPartial"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderBuyIsPartial");
            if (vl_arguments["OrderBuyCreationDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderBuyCreationDate");
            if (vl_arguments["OrderSellQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderSellQuantity");
            if (vl_arguments["OrderSellPrice"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderSellPrice");
            if (vl_arguments["IDOrderSell"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "IDOrderSell");
            if (vl_arguments["IDOrderSellQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "IDOrderSellQuantity");
            if (vl_arguments["SellPosition"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "SellPosition");
            if (vl_arguments["OrderSellIsPartial"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderSellIsPartial");
            if (vl_arguments["OrderSellCreationDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderSellCreationDate");
            if (vl_arguments["TransactionDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TransactionDate");
            if (vl_arguments["TransactionQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TransactionQuantity");
            if (vl_arguments["TransactionPrice"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TransactionPrice");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Ring = 0;
            if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
            else res.ParamValidations["ID_Ring"].AsString = "Value not recognized";

            int ID_Asset = 0;
            if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
            else res.ParamValidations["ID_Asset"].AsString = "Value not recognized";

            int ID_BuyClient = 0;
            if (vl_arguments["ID_BuyClient"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_BuyClient"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_BuyClient"].ValueType == TVariantType.vtInt64) ID_BuyClient = vl_arguments["ID_BuyClient"].AsInt32;
            else res.ParamValidations["ID_BuyClient"].AsString = "Value not recognized";

            int ID_SellClient = 0;
            if (vl_arguments["ID_SellClient"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_SellClient"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_SellClient"].ValueType == TVariantType.vtInt64) ID_SellClient = vl_arguments["ID_SellClient"].AsInt32;
            else res.ParamValidations["ID_SellClient"].AsString = "Value not recognized";

            int ID_AgencyBuy = 0;
            if (vl_arguments["ID_AgencyBuy"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_AgencyBuy"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_AgencyBuy"].ValueType == TVariantType.vtInt64) ID_AgencyBuy = vl_arguments["ID_AgencyBuy"].AsInt32;
            else res.ParamValidations["ID_AgencyBuy"].AsString = "Value not recognized";

            int ID_AgencySell = 0;
            if (vl_arguments["ID_AgencySell"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_AgencySell"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_AgencySell"].ValueType == TVariantType.vtInt64) ID_AgencySell = vl_arguments["ID_AgencySell"].AsInt32;
            else res.ParamValidations["ID_AgencySell"].AsString = "Value not recognized";

            bool NewOrderSell = false;
            if (vl_arguments["NewOrderSell"].ValueType == TVariantType.vtBoolean) NewOrderSell = vl_arguments["NewOrderSell"].AsBoolean;
            else res.ParamValidations["NewOrderSell"].AsString = "Value type not recognized";

            bool NewOrderBuy = false;
            if (vl_arguments["NewOrderBuy"].ValueType == TVariantType.vtBoolean) NewOrderBuy = vl_arguments["NewOrderBuy"].AsBoolean;
            else res.ParamValidations["NewOrderBuy"].AsString = "Value type not recognized";

            double OrderBuyQuantity = 0;
            try { OrderBuyQuantity = vl_arguments["OrderBuyQuantity"].AsDouble; }
            catch { res.ParamValidations["OrderBuyQuantity"].AsString = "Value not recognized"; }

            double OrderBuyPrice = 0;
            try { OrderBuyPrice = vl_arguments["OrderBuyPrice"].AsDouble; }
            catch { res.ParamValidations["OrderBuyPrice"].AsString = "Value not recognized"; }

            int IDOrderBuy = 0;
            if (vl_arguments["IDOrderBuy"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["IDOrderBuy"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["IDOrderBuy"].ValueType == TVariantType.vtInt64) IDOrderBuy = vl_arguments["IDOrderBuy"].AsInt32;
            else res.ParamValidations["IDOrderBuy"].AsString = "Value not recognized";

            int IDOrderBuyQuantity = 0;
            if (vl_arguments["IDOrderBuyQuantity"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["IDOrderBuyQuantity"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["IDOrderBuyQuantity"].ValueType == TVariantType.vtInt64) IDOrderBuyQuantity = vl_arguments["IDOrderBuyQuantity"].AsInt32;
            else res.ParamValidations["IDOrderBuyQuantity"].AsString = "Value not recognized";

            int BuyPosition = 0;
            if (vl_arguments["BuyPosition"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["BuyPosition"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["BuyPosition"].ValueType == TVariantType.vtInt64) BuyPosition = vl_arguments["BuyPosition"].AsInt32;
            else res.ParamValidations["BuyPosition"].AsString = "Value not recognized";

            bool OrderBuyIsPartial = false;
            if (vl_arguments["OrderBuyIsPartial"].ValueType == TVariantType.vtBoolean) OrderBuyIsPartial = vl_arguments["OrderBuyIsPartial"].AsBoolean;
            else res.ParamValidations["OrderBuyIsPartial"].AsString = "Value type not recognized";

            DateTime OrderBuyCreationDate = DateTime.Now;
            if (vl_arguments["OrderBuyCreationDate"] != null)
                if (vl_arguments["OrderBuyCreationDate"].ValueType == TVariantType.vtDateTime)
                    OrderBuyCreationDate = vl_arguments["OrderBuyCreationDate"].AsDateTime;

            double OrderSellQuantity = 0;
            try { OrderSellQuantity = vl_arguments["OrderSellQuantity"].AsDouble; }
            catch { res.ParamValidations["OrderSellQuantity"].AsString = "Value not recognized"; }

            double OrderSellPrice = 0;
            try { OrderSellPrice = vl_arguments["OrderSellPrice"].AsDouble; }
            catch { res.ParamValidations["OrderSellPrice"].AsString = "Value not recognized"; }

            int IDOrderSell = 0;
            if (vl_arguments["IDOrderSell"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["IDOrderSell"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["IDOrderSell"].ValueType == TVariantType.vtInt64) IDOrderSell = vl_arguments["IDOrderSell"].AsInt32;
            else res.ParamValidations["IDOrderSell"].AsString = "Value not recognized";

            int IDOrderSellQuantity = 0;
            if (vl_arguments["IDOrderSellQuantity"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["IDOrderSellQuantity"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["IDOrderSellQuantity"].ValueType == TVariantType.vtInt64) IDOrderSellQuantity = vl_arguments["IDOrderSellQuantity"].AsInt32;
            else res.ParamValidations["IDOrderSellQuantity"].AsString = "Value not recognized";

            int SellPosition = 0;
            if (vl_arguments["SellPosition"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["SellPosition"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["SellPosition"].ValueType == TVariantType.vtInt64) SellPosition = vl_arguments["SellPosition"].AsInt32;
            else res.ParamValidations["SellPosition"].AsString = "Value not recognized";

            bool OrderSellIsPartial = false;
            if (vl_arguments["OrderSellIsPartial"].ValueType == TVariantType.vtBoolean) OrderSellIsPartial = vl_arguments["OrderSellIsPartial"].AsBoolean;
            else res.ParamValidations["OrderSellIsPartial"].AsString = "Value type not recognized";

            DateTime OrderSellCreationDate = DateTime.Now;
            if (vl_arguments["OrderSellCreationDate"] != null)
                if (vl_arguments["OrderSellCreationDate"].ValueType == TVariantType.vtDateTime)
                    OrderSellCreationDate = vl_arguments["OrderSellCreationDate"].AsDateTime;

            double TransactionQuantity = 0;
            try { TransactionQuantity = vl_arguments["TransactionQuantity"].AsDouble; }
            catch { res.ParamValidations["TransactionQuantity"].AsString = "Value not recognized"; }

            double TransactionPrice = 0;
            try { TransactionPrice = vl_arguments["TransactionPrice"].AsDouble; }
            catch { res.ParamValidations["TransactionPrice"].AsString = "Value not recognized"; }

            DateTime TransactionDate = DateTime.Now;
            if (vl_arguments["TransactionDate"] != null)
                if (vl_arguments["TransactionDate"].ValueType == TVariantType.vtDateTime)
                    TransactionDate = vl_arguments["TransactionDate"].AsDateTime;

            if (res.bInvalid()) return res;

            //some prechecks
            double buyQuantity = 0;
            double sellQuantity = 0;

            if (NewOrderBuy)
                buyQuantity = OrderBuyQuantity;
            else
                buyQuantity = IDOrderBuyQuantity;

            if (NewOrderSell)
                sellQuantity = OrderSellQuantity;
            else
                sellQuantity = IDOrderSellQuantity;
            //use case 1: verify if the transaction is partial but the buyer order accepts only total transaction
            if (!OrderBuyIsPartial)
            {
                if (TransactionQuantity < buyQuantity)
                    return new TDBExecResult(TDBExecError.SQLExecutionError, "Tranzactia nu poate fi partiala pentru ca ordinul de cumparare accepta doar tranzactionare totala");
            }

            //use case 2: verify if the transaction is partial but the seller order accepts only total transaction
            if (!OrderSellIsPartial)
            {
                if (TransactionQuantity < sellQuantity)
                    return new TDBExecResult(TDBExecError.SQLExecutionError, "Tranzactia nu poate fi partiala pentru ca ordinul de vanzare accepta doar tranzactionare totala");
            }

            //use case 3: verify if the transaction quantity exceeds the buyer order quantity            
            if (TransactionQuantity > buyQuantity)
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Tranzactia nu poate fi efectuata: cantitatea tranzactiei este mai mare decat cea a ordinului de cumparare.");

            //use case 4: verify if the transaction quantity exceeds the seller order quantity
            if (TransactionQuantity > sellQuantity)
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Tranzactia nu poate fi efectuata: cantitatea tranzactiei este mai mare decat cea a ordinului de vanzare.");


            //Step 1. Verify if we have new buy order. If there is a new order, then insert it and get the id
            int ID_BuyOrder = 0;
            if (!NewOrderBuy)
                ID_BuyOrder = IDOrderBuy;
            else
            {
                //we have to create the new order
                TVariantList vl_params = new TVariantList();
                vl_params.Add("ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("Direction").AsString = "B";
                vl_params.Add("Quantity").AsDouble = OrderBuyQuantity;
                vl_params.Add("Price").AsDouble = OrderBuyPrice;
                vl_params.Add("isInitial").AsBoolean = false;
                vl_params.Add("ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("ID_Client").AsInt32 = ID_BuyClient;
                vl_params.Add("ID_Agency").AsInt32 = ID_AgencyBuy;
                vl_params.Add("ExpirationDate").AsDateTime = OrderBuyCreationDate;

                TVariantList IDs = addOrder(vl_params).IDs;
                ID_BuyOrder = IDs["ID_Order"].AsInt32;
            }

            //Step 2. Verify if we have a new sell order. if there is a new order, then insert it and get the id
            int ID_SellOrder = 0;
            if (!NewOrderSell)
                ID_SellOrder = IDOrderSell;
            else
            {
                //we have to create the new order
                TVariantList vl_params = new TVariantList();
                vl_params.Add("ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("Direction").AsString = "S";
                vl_params.Add("Quantity").AsDouble = OrderSellQuantity;
                vl_params.Add("Price").AsDouble = OrderSellPrice;
                vl_params.Add("isInitial").AsBoolean = false;
                vl_params.Add("ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("ID_Client").AsInt32 = ID_SellClient;
                vl_params.Add("ID_Agency").AsInt32 = ID_AgencySell;
                vl_params.Add("ExpirationDate").AsDateTime = OrderSellCreationDate;

                TVariantList IDs = addOrder(vl_params).IDs;
                ID_SellOrder = IDs["ID_Order"].AsInt32;
            }
            //Step 3. Get sessions (just ringsession for now)
            int ID_RingSession = 0;
            DataSet ds_ringsession = getRingSession(ID_Ring);

            if (!(app.DB.ValidDS(ds_ringsession))) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            if (!app.DB.ValidDSRows(ds_ringsession)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            if (Convert.ToDateTime(ds_ringsession.Tables[0].Rows[0]["Date"]).Date != DateTime.Today) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            //if (ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "Opened" && ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "PreOpened") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            ID_RingSession = Convert.ToInt32(ds_ringsession.Tables[0].Rows[0]["ID"]);

            //Step 4. Insert the manual transaction
            TVariantList vl_prm = new TVariantList();
            vl_prm.Add("@prm_Date").AsDateTime = TransactionDate;
            vl_prm.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
            vl_prm.Add("@prm_ID_RingSession").AsInt32 = ID_RingSession; //trebuie aflat
            vl_prm.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            vl_prm.Add("@prm_ID_AssetSession").AsInt32 = 0; //trebuie aflat (?)
            vl_prm.Add("@prm_ID_BuyOrder").AsInt32 = ID_BuyOrder;
            vl_prm.Add("@prm_ID_SellOrder").AsInt32 = ID_SellOrder;
            vl_prm.Add("@prm_Quantity").AsDouble = TransactionQuantity;
            vl_prm.Add("@prm_Price").AsDouble = TransactionPrice;
            int int_rows = app.DB.Exec("insert_Transaction", "Transactions", vl_prm);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            //update isTransacted
            if (buyQuantity == TransactionQuantity)
            {
                vl_prm = new TVariantList();
                vl_prm.Add("@prm_ID").AsInt32 = ID_BuyOrder;
                vl_prm.Add("@prm_isTransacted").AsBoolean = true;
                int_rows = app.DB.Exec("update_SetOrderisTransacted", "Orders", vl_prm);
                if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            }

            if (sellQuantity == TransactionQuantity)
            {
                vl_prm = new TVariantList();
                vl_prm.Add("@prm_ID").AsInt32 = ID_SellOrder;
                vl_prm.Add("@prm_isTransacted").AsBoolean = true;
                int_rows = app.DB.Exec("update_SetOrderisTransacted", "Orders", vl_prm);
                if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            }

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult editTransaction(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_Transaction"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Transaction");
            if (vl_arguments["ID_Ring"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Ring");
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");
            if (vl_arguments["ID_BuyClient"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_BuyClient");
            if (vl_arguments["ID_SellClient"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_SellClient");
            if (vl_arguments["ID_AgencyBuy"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AgencyBuy");
            if (vl_arguments["ID_AgencySell"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AgencySell");
            if (vl_arguments["NewOrderSell"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "NewOrderSell");
            if (vl_arguments["NewOrderBuy"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "NewOrderBuy");
            if (vl_arguments["OrderBuyQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderBuyQuantity");
            if (vl_arguments["OrderBuyPrice"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderBuyPrice");
            if (vl_arguments["IDOrderBuy"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "IDOrderBuy");
            if (vl_arguments["IDOrderBuyQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "IDOrderBuyQuantity");
            if (vl_arguments["BuyPosition"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "BuyPosition");
            if (vl_arguments["OrderBuyIsPartial"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderBuyIsPartial");
            if (vl_arguments["OrderBuyCreationDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderBuyCreationDate");
            if (vl_arguments["OrderSellQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderSellQuantity");
            if (vl_arguments["OrderSellPrice"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderSellPrice");
            if (vl_arguments["IDOrderSell"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "IDOrderSell");
            if (vl_arguments["IDOrderSellQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "IDOrderSellQuantity");
            if (vl_arguments["SellPosition"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "SellPosition");
            if (vl_arguments["OrderSellIsPartial"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderSellIsPartial");
            if (vl_arguments["OrderSellCreationDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "OrderSellCreationDate");
            if (vl_arguments["TransactionDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TransactionDate");
            if (vl_arguments["TransactionQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TransactionQuantity");
            if (vl_arguments["TransactionPrice"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TransactionPrice");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Transaction = 0;
            if (vl_arguments["ID_Transaction"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Transaction"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Transaction"].ValueType == TVariantType.vtInt64) ID_Transaction = vl_arguments["ID_Transaction"].AsInt32;
            else res.ParamValidations["ID_Transaction"].AsString = "Value not recognized";

            int ID_Ring = 0;
            if (vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Ring"].ValueType == TVariantType.vtInt64) ID_Ring = vl_arguments["ID_Ring"].AsInt32;
            else res.ParamValidations["ID_Ring"].AsString = "Value not recognized";

            int ID_Asset = 0;
            if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
            else res.ParamValidations["ID_Asset"].AsString = "Value not recognized";

            int ID_BuyClient = 0;
            if (vl_arguments["ID_BuyClient"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_BuyClient"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_BuyClient"].ValueType == TVariantType.vtInt64) ID_BuyClient = vl_arguments["ID_BuyClient"].AsInt32;
            else res.ParamValidations["ID_BuyClient"].AsString = "Value not recognized";

            int ID_SellClient = 0;
            if (vl_arguments["ID_SellClient"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_SellClient"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_SellClient"].ValueType == TVariantType.vtInt64) ID_SellClient = vl_arguments["ID_SellClient"].AsInt32;
            else res.ParamValidations["ID_SellClient"].AsString = "Value not recognized";

            int ID_AgencyBuy = 0;
            if (vl_arguments["ID_AgencyBuy"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_AgencyBuy"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_AgencyBuy"].ValueType == TVariantType.vtInt64) ID_AgencyBuy = vl_arguments["ID_AgencyBuy"].AsInt32;
            else res.ParamValidations["ID_AgencyBuy"].AsString = "Value not recognized";

            int ID_AgencySell = 0;
            if (vl_arguments["ID_AgencySell"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_AgencySell"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_AgencySell"].ValueType == TVariantType.vtInt64) ID_AgencySell = vl_arguments["ID_AgencySell"].AsInt32;
            else res.ParamValidations["ID_AgencySell"].AsString = "Value not recognized";

            bool NewOrderSell = false;
            if (vl_arguments["NewOrderSell"].ValueType == TVariantType.vtBoolean) NewOrderSell = vl_arguments["NewOrderSell"].AsBoolean;
            else res.ParamValidations["NewOrderSell"].AsString = "Value type not recognized";

            bool NewOrderBuy = false;
            if (vl_arguments["NewOrderBuy"].ValueType == TVariantType.vtBoolean) NewOrderBuy = vl_arguments["NewOrderBuy"].AsBoolean;
            else res.ParamValidations["NewOrderBuy"].AsString = "Value type not recognized";

            double OrderBuyQuantity = 0;
            try { OrderBuyQuantity = vl_arguments["OrderBuyQuantity"].AsDouble; }
            catch { res.ParamValidations["OrderBuyQuantity"].AsString = "Value not recognized"; }

            double OrderBuyPrice = 0;
            try { OrderBuyPrice = vl_arguments["OrderBuyPrice"].AsDouble; }
            catch { res.ParamValidations["OrderBuyPrice"].AsString = "Value not recognized"; }

            int IDOrderBuy = 0;
            if (vl_arguments["IDOrderBuy"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["IDOrderBuy"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["IDOrderBuy"].ValueType == TVariantType.vtInt64) IDOrderBuy = vl_arguments["IDOrderBuy"].AsInt32;
            else res.ParamValidations["IDOrderBuy"].AsString = "Value not recognized";

            int IDOrderBuyQuantity = 0;
            if (vl_arguments["IDOrderBuyQuantity"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["IDOrderBuyQuantity"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["IDOrderBuyQuantity"].ValueType == TVariantType.vtInt64) IDOrderBuyQuantity = vl_arguments["IDOrderBuyQuantity"].AsInt32;
            else res.ParamValidations["IDOrderBuyQuantity"].AsString = "Value not recognized";

            int BuyPosition = 0;
            if (vl_arguments["BuyPosition"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["BuyPosition"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["BuyPosition"].ValueType == TVariantType.vtInt64) BuyPosition = vl_arguments["BuyPosition"].AsInt32;
            else res.ParamValidations["BuyPosition"].AsString = "Value not recognized";

            bool OrderBuyIsPartial = false;
            if (vl_arguments["OrderBuyIsPartial"].ValueType == TVariantType.vtBoolean) OrderBuyIsPartial = vl_arguments["OrderBuyIsPartial"].AsBoolean;
            else res.ParamValidations["OrderBuyIsPartial"].AsString = "Value type not recognized";

            DateTime OrderBuyCreationDate = DateTime.Now;
            if (vl_arguments["OrderBuyCreationDate"] != null)
                if (vl_arguments["OrderBuyCreationDate"].ValueType == TVariantType.vtDateTime)
                    OrderBuyCreationDate = vl_arguments["OrderBuyCreationDate"].AsDateTime;

            double OrderSellQuantity = 0;
            try { OrderSellQuantity = vl_arguments["OrderSellQuantity"].AsDouble; }
            catch { res.ParamValidations["OrderSellQuantity"].AsString = "Value not recognized"; }

            double OrderSellPrice = 0;
            try { OrderSellPrice = vl_arguments["OrderSellPrice"].AsDouble; }
            catch { res.ParamValidations["OrderSellPrice"].AsString = "Value not recognized"; }

            int IDOrderSell = 0;
            if (vl_arguments["IDOrderSell"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["IDOrderSell"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["IDOrderSell"].ValueType == TVariantType.vtInt64) IDOrderSell = vl_arguments["IDOrderSell"].AsInt32;
            else res.ParamValidations["IDOrderSell"].AsString = "Value not recognized";

            int IDOrderSellQuantity = 0;
            if (vl_arguments["IDOrderSellQuantity"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["IDOrderSellQuantity"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["IDOrderSellQuantity"].ValueType == TVariantType.vtInt64) IDOrderSellQuantity = vl_arguments["IDOrderSellQuantity"].AsInt32;
            else res.ParamValidations["IDOrderSellQuantity"].AsString = "Value not recognized";

            int SellPosition = 0;
            if (vl_arguments["SellPosition"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["SellPosition"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["SellPosition"].ValueType == TVariantType.vtInt64) SellPosition = vl_arguments["SellPosition"].AsInt32;
            else res.ParamValidations["SellPosition"].AsString = "Value not recognized";

            bool OrderSellIsPartial = false;
            if (vl_arguments["OrderSellIsPartial"].ValueType == TVariantType.vtBoolean) OrderSellIsPartial = vl_arguments["OrderSellIsPartial"].AsBoolean;
            else res.ParamValidations["OrderSellIsPartial"].AsString = "Value type not recognized";

            DateTime OrderSellCreationDate = DateTime.Now;
            if (vl_arguments["OrderSellCreationDate"] != null)
                if (vl_arguments["OrderSellCreationDate"].ValueType == TVariantType.vtDateTime)
                    OrderSellCreationDate = vl_arguments["OrderSellCreationDate"].AsDateTime;

            double TransactionQuantity = 0;
            try { TransactionQuantity = vl_arguments["TransactionQuantity"].AsDouble; }
            catch { res.ParamValidations["TransactionQuantity"].AsString = "Value not recognized"; }

            double TransactionPrice = 0;
            try { TransactionPrice = vl_arguments["TransactionPrice"].AsDouble; }
            catch { res.ParamValidations["TransactionPrice"].AsString = "Value not recognized"; }

            DateTime TransactionDate = DateTime.Now;
            if (vl_arguments["TransactionDate"] != null)
                if (vl_arguments["TransactionDate"].ValueType == TVariantType.vtDateTime)
                    TransactionDate = vl_arguments["TransactionDate"].AsDateTime;

            if (res.bInvalid()) return res;

            //some prechecks
            double buyQuantity = 0;
            double sellQuantity = 0;

            if (NewOrderBuy)
                buyQuantity = OrderBuyQuantity;
            else
                buyQuantity = IDOrderBuyQuantity;

            if (NewOrderSell)
                sellQuantity = OrderSellQuantity;
            else
                sellQuantity = IDOrderSellQuantity;
            //use case 1: verify if the transaction is partial but the buyer order accepts only total transaction
            if (!OrderBuyIsPartial)
            {
                if (TransactionQuantity < buyQuantity)
                    res.ParamValidations["Tranzaction"].AsString = "Tranzactia nu poate fi partiala pentru ca ordinul de cumparare accepta doar tranzactionare totala.";
            }

            //use case 2: verify if the transaction is partial but the seller order accepts only total transaction
            if (!OrderSellIsPartial)
            {
                if (TransactionQuantity < sellQuantity)
                    return new TDBExecResult(TDBExecError.SQLExecutionError, "Tranzactia nu poate fi partiala pentru ca ordinul de vanzare accepta doar tranzactionare totala");
            }

            //use case 3: verify if the transaction quantity exceeds the buyer order quantity            
            if (TransactionQuantity > buyQuantity)
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Tranzactia nu poate fi efectuata: cantitatea tranzactiei este mai mare decat cea a ordinului de cumparare.");

            //use case 4: verify if the transaction quantity exceeds the seller order quantity
            if (TransactionQuantity > sellQuantity)
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Tranzactia nu poate fi efectuata: cantitatea tranzactiei este mai mare decat cea a ordinului de vanzare.");

            /*//Step 1. Verify if we have new buy order. If there is a new order, then insert it and get the id
            int ID_BuyOrder = 0;
            if (!NewOrderBuy)
                ID_BuyOrder = IDOrderBuy;
            else
            {
                //we have to create the new order
                TVariantList vl_params = new TVariantList();
                vl_params.Add("ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("Direction").AsString = "B";
                vl_params.Add("Quantity").AsDouble = OrderBuyQuantity;
                vl_params.Add("Price").AsDouble = OrderBuyPrice;
                vl_params.Add("isInitial").AsBoolean = false;
                vl_params.Add("ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("ID_Client").AsInt32 = ID_BuyClient;
                vl_params.Add("ID_Agency").AsInt32 = ID_AgencyBuy;
                vl_params.Add("ExpirationDate").AsDateTime = OrderBuyCreationDate;

                TVariantList IDs = addOrder(vl_params).IDs;
                ID_BuyOrder = IDs["ID_Order"].AsInt32;
            }

            //Step 2. Verify if we have a new sell order. if there is a new order, then insert it and get the id
            int ID_SellOrder = 0;
            if (!NewOrderSell)
                ID_SellOrder = IDOrderSell;
            else
            {
                //we have to create the new order
                TVariantList vl_params = new TVariantList();
                vl_params.Add("ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("Direction").AsString = "S";
                vl_params.Add("Quantity").AsDouble = OrderSellQuantity;
                vl_params.Add("Price").AsDouble = OrderSellPrice;
                vl_params.Add("isInitial").AsBoolean = false;
                vl_params.Add("ID_Asset").AsInt32 = ID_Asset;
                vl_params.Add("ID_Client").AsInt32 = ID_SellClient;
                vl_params.Add("ID_Agency").AsInt32 = ID_AgencySell;
                vl_params.Add("ExpirationDate").AsDateTime = OrderSellCreationDate;

                TVariantList IDs = addOrder(vl_params).IDs;
                ID_SellOrder = IDs["ID_Order"].AsInt32;
            }
            //Step 3. Get sessions (just ringsession for now)
            int ID_RingSession = 0;
            DataSet ds_ringsession = getRingSession(ID_Ring);

            if (!(app.DB.ValidDS(ds_ringsession))) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            if (!app.DB.ValidDSRows(ds_ringsession)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            if (Convert.ToDateTime(ds_ringsession.Tables[0].Rows[0]["Date"]).Date != DateTime.Today) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            //if (ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "Opened" && ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "PreOpened") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
            ID_RingSession = Convert.ToInt32(ds_ringsession.Tables[0].Rows[0]["ID"]);
            */
            //Step 4. Insert the manual transaction
            TVariantList vl_prm = new TVariantList();
            vl_prm.Add("@prm_ID_Transaction").AsInt32 = ID_Transaction;
            vl_prm.Add("@prm_Date").AsDateTime = TransactionDate;
            vl_prm.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
            //vl_prm.Add("@prm_ID_RingSession").AsInt32 = ID_RingSession; //trebuie aflat
            vl_prm.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            //vl_prm.Add("@prm_ID_AssetSession").AsInt32 = 0; //trebuie aflat (?)
            //vl_prm.Add("@prm_ID_BuyOrder").AsInt32 = ID_BuyOrder;
            //vl_prm.Add("@prm_ID_SellOrder").AsInt32 = ID_SellOrder;
            vl_prm.Add("@prm_Quantity").AsDouble = TransactionQuantity;
            vl_prm.Add("@prm_Price").AsDouble = TransactionPrice;
            int int_rows = app.DB.Exec("update_Transaction", "Transactions", vl_prm);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            //update isTransacted
            /*vl_prm = new TVariantList();
            vl_prm.Add("@prm_ID").AsInt32 = ID_BuyOrder;
            if (buyQuantity == TransactionQuantity)
                vl_prm.Add("@prm_isTransacted").AsBoolean = true;
            else
                vl_prm.Add("@prm_isTransacted").AsBoolean = false;
            int_rows = app.DB.Exec("update_SetOrderisTransacted", "Orders", vl_prm);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            vl_prm = new TVariantList();
            vl_prm.Add("@prm_ID").AsInt32 = ID_SellOrder;
            if (sellQuantity == TransactionQuantity)
                vl_prm.Add("@prm_isTransacted").AsBoolean = true;
            else
                vl_prm.Add("@prm_isTransacted").AsBoolean = true;
            int_rows = app.DB.Exec("update_SetOrderisTransacted", "Orders", vl_prm);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");*/

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult deleteTransaction(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_Transaction"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_County");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Transaction = 0;
            if (vl_arguments["ID_Transaction"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Transaction"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Transaction"].ValueType == TVariantType.vtInt64) ID_Transaction = vl_arguments["ID_Transaction"].AsInt32;
            else res.ParamValidations["ID_Transaction"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Transaction;
            int int_rows = app.DB.Exec("delete_Transactions", "Transactions", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult confirmTransaction(TVariantList vl_arguments)
        {
            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");

            if (res.bInvalid()) return res;

            res = new TDBExecResult(TDBExecError.Success, "");

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "could not start transaction");

            try
            {
                app.DB.CommitTransaction();
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            }

            return res;
        }

        public TDBExecResult duplicateAsset(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Market"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Market");
            if (vl_arguments["ID_Asset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Asset");
            if (vl_arguments["all"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "all");
            if (vl_arguments["anystatus"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "anystatus");


            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Market = 0;
            if (vl_arguments["ID_Market"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Market"].ValueType == TVariantType.vtInt64) ID_Market = vl_arguments["ID_Market"].AsInt32;
            else res.ParamValidations["ID_Market"].AsString = "Value type not recognized";

            int ID_Asset = 0;
            if (vl_arguments["ID_Asset"].ValueType == TVariantType.vtByte ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Asset"].ValueType == TVariantType.vtInt64) ID_Asset = vl_arguments["ID_Asset"].AsInt32;
            else res.ParamValidations["ID_Asset"].AsString = "Value type not recognized";

            bool all = true;
            if (vl_arguments["all"] != null)
            {
                if (vl_arguments["all"].ValueType == TVariantType.vtBoolean) all = vl_arguments["all"].AsBoolean;
                else res.ParamValidations["all"].AsString = "Value not recognized";
            }

            bool anystatus = true;
            if (vl_arguments["all"] != null)
            {
                if (vl_arguments["anystatus"].ValueType == TVariantType.vtBoolean) anystatus = vl_arguments["anystatus"].AsBoolean;
                else res.ParamValidations["anystatus"].AsString = "Value not recognized";
            }

            if (res.bInvalid()) return res;

            //if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs = new TVariantList();

                int ID_Ring = -1;
                int ID_AssetType = -1;

                //step 1. get the asset info and duplicate it
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Bursary").AsInt32 = session.ID_Bursary;
                vl_params.Add("@prm_ID_Market").AsInt32 = ID_Market;
                vl_params.Add("@prm_ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("@prm_ID").AsInt32 = ID_Asset;
                vl_params.Add("@prm_ID_AssetType").AsInt32 = ID_AssetType;
                vl_params.Add("@prm_AnyStatus").AsBoolean = anystatus;
                vl_params.Add("@prm_All").AsBoolean = all;

                DataSet ds_assets = getAssets(vl_params);

                if (!(app.DB.ValidDS(ds_assets) && app.DB.ValidDS(ds_assets))) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

                if (!app.DB.ValidDSRows(ds_assets)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No asset found!");
                //if (ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "Opened" && ds_ringsession.Tables[0].Rows[0]["Status"].ToString() != "PreOpened") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No opened trading session");
                //ID_Asset = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID"]);
                ID_Market = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID_Market"]);
                ID_Ring = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID_Ring"]);
                ID_AssetType = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID_AssetType"]);
                string Code = Convert.ToString(ds_assets.Tables[0].Rows[0]["Code"]);
                string Name = Convert.ToString(ds_assets.Tables[0].Rows[0]["Name"]);
                string Description = Convert.ToString(ds_assets.Tables[0].Rows[0]["Description"]);
                string AuctionType = Convert.ToString(ds_assets.Tables[0].Rows[0]["AuctionType"]);
                int ID_Currency = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID_Currency"]);
                int ID_PaymentCurrency = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID_PaymentCurrency"]);
                int ID_MeasuringUnit = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID_MeasuringUnit"]);
                bool isSpotContract = Convert.ToBoolean(ds_assets.Tables[0].Rows[0]["isSpotContract"]);
                double SpotQuotation = Convert.ToDouble(ds_assets.Tables[0].Rows[0]["SpotQuotation"]);
                int ID_InitialOrder = Convert.ToInt32(ds_assets.Tables[0].Rows[0]["ID_InitialOrder"]);
                bool inheritRingSchedule = Convert.ToBoolean(ds_assets.Tables[0].Rows[0]["inheritRingSchedule"]);
                string Visibility = "initiated";
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

                //  other details
                string Instructions_RO = ds_assets.Tables[0].Rows[0]["Instructions_RO"].ToString();
                string Instructions_EN = ds_assets.Tables[0].Rows[0]["Instructions_EN"].ToString();
                string DeliveryTerm = ds_assets.Tables[0].Rows[0]["DeliveryTerm"].ToString();
                string DeliveryConditions = ds_assets.Tables[0].Rows[0]["DeliveryConditions"].ToString();
                string PackingMethod = ds_assets.Tables[0].Rows[0]["PackingMethod"].ToString();
                string PaymentMethod = ds_assets.Tables[0].Rows[0]["PaymentMethod"].ToString();
                string ContractTerm = ds_assets.Tables[0].Rows[0]["ContractTerm"].ToString();
                string WarrantyMethod = ds_assets.Tables[0].Rows[0]["WarrantyMethod"].ToString();
                int nr = 0;

                if (Code.Contains('_'))
                {
                    string[] parts = Code.Split('_');
                    Code = parts[0];
                }
                Code += "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                vl_params = new TVariantList();
                vl_params.Add("ID_Market").AsInt32 = ID_Market;
                vl_params.Add("ID_Ring").AsInt32 = ID_Ring;
                vl_params.Add("ID_AssetType").AsInt32 = ID_AssetType;
                vl_params.Add("Code").AsString = Code;
                vl_params.Add("Name").AsString = Name;
                vl_params.Add("Description").AsString = Description;
                vl_params.Add("AuctionType").AsString = AuctionType;
                vl_params.Add("ID_Currency").AsInt32 = ID_Currency;
                vl_params.Add("ID_PaymentCurrency").AsInt32 = ID_PaymentCurrency;
                vl_params.Add("ID_MeasuringUnit").AsInt32 = ID_MeasuringUnit;
                vl_params.Add("MeasuringUnit").AsString = "";
                vl_params.Add("isSpotContract").AsBoolean = isSpotContract;
                vl_params.Add("SpotQuotation").AsDouble = SpotQuotation;
                vl_params.Add("ID_InitialOrder").AsInt32 = 0;
                vl_params.Add("inheritRingSchedule").AsBoolean = inheritRingSchedule;
                vl_params.Add("Visibility").AsString = Visibility;
                vl_params.Add("isActive").AsBoolean = isActive;
                vl_params.Add("isDeleted").AsBoolean = isDeleted;
                vl_params.Add("isDefault").AsBoolean = isDefault;
                vl_params.Add("PartialFlagChangeAllowed").AsBoolean = PartialFlagChangeAllowed;
                vl_params.Add("InitialPriceMandatory").AsBoolean = InitialPriceMandatory;
                vl_params.Add("InitialPriceMaintenance").AsBoolean = InitialPriceMaintenance;
                vl_params.Add("DiminishedQuantityAllowed").AsBoolean = DiminishedQuantityAllowed;
                vl_params.Add("DiminishedPriceAllowed").AsBoolean = DiminishedPriceAllowed;
                vl_params.Add("DifferentialPriceAllowed").AsBoolean = DifferentialPriceAllowed;
                vl_params.Add("OppositeDirectionAllowed").AsBoolean = OppositeDirectionAllowed;
                vl_params.Add("DeltaT").AsInt32 = DeltaT;
                vl_params.Add("DeltaT1").AsInt32 = DeltaT1;

                //  add other details
                vl_params.Add("Instructions_RO").AsString = Instructions_RO;
                vl_params.Add("Instructions_EN").AsString = Instructions_EN;
                vl_params.Add("DeliveryTerm").AsString = DeliveryTerm;
                vl_params.Add("DeliveryConditions").AsString = DeliveryConditions;
                vl_params.Add("PackingMethod").AsString = PackingMethod;
                vl_params.Add("PaymentMethod").AsString = PaymentMethod;
                vl_params.Add("ContractTerm").AsString = ContractTerm;
                vl_params.Add("WarrantyMethod").AsString = WarrantyMethod;

                TDBExecResult exec = addAsset(vl_params);
                TVariantList IDs = exec.IDs;
                if (IDs == null)
                    return new TDBExecResult(TDBExecError.SQLExecutionError, "");

                int ID_NewAsset = IDs["ID_Asset"].AsInt32;
                int int_count = exec.RowsModified;


                //step2. verify if the original asset has initial order and if it does, then duplicate that initial order
                vl_params = new TVariantList();
                DataSet ds_initialOrders = getInitialOrder(ID_Asset);
                if (!(app.DB.ValidDS(ds_initialOrders))) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
                if (!app.DB.ValidDSRows(ds_initialOrders)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No initial orders found!");
                for (int i = 0; i < ds_initialOrders.Tables[0].Rows.Count; i++)
                {
                    int idAsset = Convert.ToInt32(ds_initialOrders.Tables[0].Rows[i]["ID_Asset"]);
                    if (idAsset == ID_Asset)
                    {
                        DateTime Date = DateTime.Now;
                        ID_Ring = Convert.ToInt32(ds_initialOrders.Tables[0].Rows[i]["ID_Ring"]);
                        int ID_RingSession = Convert.ToInt32(ds_initialOrders.Tables[0].Rows[i]["ID_RingSession"]);
                        int ID_AssetSession = Convert.ToInt32(ds_initialOrders.Tables[0].Rows[i]["ID_AssetSession"]);
                        int ID_Agency = Convert.ToInt32(ds_initialOrders.Tables[0].Rows[i]["ID_Agency"]);
                        int ID_Broker = Convert.ToInt32(ds_initialOrders.Tables[0].Rows[i]["ID_Broker"]);
                        int ID_Client = Convert.ToInt32(ds_initialOrders.Tables[0].Rows[i]["ID_Client"]);
                        bool Direction = Convert.ToBoolean(ds_initialOrders.Tables[0].Rows[i]["Direction"]);

                        double Quantity = 0;
                        double Price = double.NaN;

                        if (ds_initialOrders.Tables[0].Rows[0]["ID_Journal"] == DBNull.Value)
                        {
                            //  there is no history on the journal
                            Quantity = Convert.ToDouble(ds_initialOrders.Tables[0].Rows[i]["Quantity"]);
                            if (ds_initialOrders.Tables[0].Rows[i]["Price"] != DBNull.Value) Price = Convert.ToDouble(ds_initialOrders.Tables[0].Rows[i]["Price"]);
                        }
                        else
                        {
                            Quantity = Convert.ToDouble(ds_initialOrders.Tables[0].Rows[i]["JournalQuantity"]);
                            if (ds_initialOrders.Tables[0].Rows[i]["JournalPrice"] != DBNull.Value) Price = Convert.ToDouble(ds_initialOrders.Tables[0].Rows[i]["JournalPrice"]);
                        }
                        
                        bool PartialFlag = Convert.ToBoolean(ds_initialOrders.Tables[0].Rows[i]["PartialFlag"]);
                        DateTime ExpirationDate = DateTime.Now.AddDays(10);
                        bool isTransacted = Convert.ToBoolean(ds_initialOrders.Tables[0].Rows[i]["isTransacted"]);
                        bool isSuspended = Convert.ToBoolean(ds_initialOrders.Tables[0].Rows[i]["isSuspended"]);
                        isActive = Convert.ToBoolean(ds_initialOrders.Tables[0].Rows[i]["isActive"]);
                        bool isCanceled = Convert.ToBoolean(ds_initialOrders.Tables[0].Rows[i]["isSuspended"]);
                        bool isApproved = Convert.ToBoolean(ds_initialOrders.Tables[0].Rows[i]["isSuspended"]);


                        vl_params = new TVariantList();
                        vl_params.Add("ID_Ring").AsInt32 = ID_Ring;
                        vl_params.Add("ID_RingSession").AsInt32 = ID_RingSession;
                        vl_params.Add("ID_Asset").AsInt32 = ID_NewAsset;
                        vl_params.Add("ID_AssetSession").AsInt32 = ID_AssetSession;
                        vl_params.Add("ID_Agency").AsInt32 = ID_Agency;
                        vl_params.Add("ID_Broker").AsInt32 = ID_Broker;
                        vl_params.Add("ID_Client").AsInt32 = ID_Client;
                        vl_params.Add("Direction").AsString = Direction ? "S" : "B";
                        vl_params.Add("Quantity").AsDouble = Quantity;
                        if (double.IsNaN(Price)) vl_params.Add("Price").AsString = "none";
                        else vl_params.Add("Price").AsDouble = Price;
                        vl_params.Add("PartialFlag").AsBoolean = PartialFlag;
                        vl_params.Add("ExpirationDate").AsDateTime = ExpirationDate;
                        vl_params.Add("isActive").AsBoolean = true;
                        vl_params.Add("isInitial").AsBoolean = true;
                        exec = addOrder(vl_params);
                        int_count += exec.RowsModified;
                    }
                }


                //step 3 set the asset schedule
                vl_params = new TVariantList();
                vl_params.Add("ID_Asset").AsInt32 = ID_Asset;
                DataSet ds_assetschedules = getAssetsSchedules(vl_params);

                if (!(app.DB.ValidDS(ds_assetschedules))) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
                if (!app.DB.ValidDSRows(ds_assetschedules)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No asset schedules found!");

                //ID_Asset = Convert.ToInt32(ds_assetschedules.Tables[0].Rows[0]["ID_Asset"]);
                //DateTime StartDate = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["StartDate"]);
                //DateTime EndDate = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["EndDate"]);
                DateTime StartDate = DateTime.Now.AddDays(9);
                DateTime EndDate = DateTime.Now.AddDays(9);
                byte DaysOfWeek = Convert.ToByte(ds_assetschedules.Tables[0].Rows[0]["DaysOfWeek"]);
                DateTime PreOpeningTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["PreOpeningTime"].ToString());
                DateTime OpeningTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["OpeningTime"].ToString());
                DateTime PreClosingTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["PreClosingTime"].ToString());
                DateTime ClosingTime = Convert.ToDateTime(ds_assetschedules.Tables[0].Rows[0]["ClosingTime"].ToString());
                bool isElectronicSession = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["isElectronicSession"]);
                bool launchAutomatically = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["launchAutomatically"]);
                double QuantityStepping = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["QuantityStepping"]);
                double MinQuantity = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["MinQuantity"]);
                double PriceStepping = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["PriceStepping"]);
                double MaxPriceVariation = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["MaxPriceVariation"]);
                double MinPrice = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["MinPrice"]);
                double MaxPrice = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["MaxPrice"]);
                isActive = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["isActive"]);
                isDeleted = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["isDeleted"]);
                isDefault = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["isDefault"]);
                bool daySunday = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["daySunday"]);
                bool dayMonday = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["dayMonday"]);
                bool dayTuesday = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["dayTuesday"]);
                bool dayWednesday = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["dayWednesday"]);
                bool dayThursday = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["dayThursday"]);
                bool dayFriday = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["dayFriday"]);
                bool daySaturday = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["daySaturday"]);
                PartialFlagChangeAllowed = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["PartialFlagChangeAllowed"]);
                InitialPriceMandatory = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["InitialPriceMandatory"]);
                InitialPriceMaintenance = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["InitialPriceMaintenance"]);
                DiminishedQuantityAllowed = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["DiminishedQuantityAllowed"]);
                DiminishedPriceAllowed = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["DiminishedPriceAllowed"]);
                DifferentialPriceAllowed = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["DifferentialPriceAllowed"]);
                OppositeDirectionAllowed = Convert.ToBoolean(ds_assetschedules.Tables[0].Rows[0]["OppositeDirectionAllowed"]);
                DeltaT = Convert.ToInt32(ds_assetschedules.Tables[0].Rows[0]["DeltaT"]);
                DeltaT1 = Convert.ToInt32(ds_assetschedules.Tables[0].Rows[0]["DeltaT1"]);
                double SellWarrantyPercent = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["SellWarrantyPercent"]);
                double SellWarrantyMU = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["SellWarrantyMU"]);
                double SellWarrantyFixed = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["SellWarrantyFixed"]);
                double BuyWarrantyPercent = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["BuyWarrantyPercent"]);
                double BuyWarrantyMU = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["BuyWarrantyMU"]);
                double BuyWarrantyFixed = Convert.ToDouble(ds_assetschedules.Tables[0].Rows[0]["BuyWarrantyFixed"]);
                string DaysOfWeekSTR = Convert.ToString(ds_assetschedules.Tables[0].Rows[0]["DaysOfWeekSTR"]);

                TVariantList vl_daysofweek = new TVariantList();
                vl_daysofweek.Add("DAYSUNDAY").AsBoolean = daySunday;
                vl_daysofweek.Add("DAYMONDAY").AsBoolean = dayMonday;
                vl_daysofweek.Add("DAYTUESDAY").AsBoolean = dayTuesday;
                vl_daysofweek.Add("DAYWEDNESDAY").AsBoolean = dayWednesday;
                vl_daysofweek.Add("DAYTHURSDAY").AsBoolean = dayThursday;
                vl_daysofweek.Add("DAYFRIDAY").AsBoolean = dayFriday;
                vl_daysofweek.Add("DAYSATURDAY").AsBoolean = daySaturday;


                vl_params = new TVariantList();
                vl_params.Add("ID_Asset").AsInt32 = ID_NewAsset;
                vl_params.Add("StartDate").AsDateTime = StartDate;
                vl_params.Add("EndDate").AsDateTime = EndDate;
                vl_params.Add("DaysOfWeek").AsObject = vl_daysofweek;
                vl_params.Add("PreOpeningTime").AsDateTime = PreOpeningTime;
                vl_params.Add("OpeningTime").AsDateTime = OpeningTime;
                vl_params.Add("PreClosingTime").AsDateTime = PreClosingTime;
                vl_params.Add("ClosingTime").AsDateTime = ClosingTime;
                vl_params.Add("isElectronicSession").AsBoolean = isElectronicSession;
                vl_params.Add("launchAutomatically").AsBoolean = launchAutomatically;
                vl_params.Add("MinQuantity").AsDouble = MinQuantity;
                vl_params.Add("MaxPriceVariation").AsDouble = MaxPriceVariation;
                vl_params.Add("QuantityStepping").AsDouble = QuantityStepping;
                vl_params.Add("PriceStepping").AsDouble = PriceStepping;
                vl_params.Add("MinPrice").AsDouble = MinPrice;
                vl_params.Add("MaxPrice").AsDouble = MaxPrice;
                vl_params.Add("IsDefault").AsBoolean = false;
                vl_params.Add("PartialFlagChangeAllowed").AsBoolean = PartialFlagChangeAllowed;
                vl_params.Add("InitialPriceMandatory").AsBoolean = InitialPriceMandatory;
                vl_params.Add("InitialPriceMaintenance").AsBoolean = InitialPriceMaintenance;
                vl_params.Add("DiminishedQuantityAllowed").AsBoolean = DiminishedQuantityAllowed;
                vl_params.Add("DiminishedPriceAllowed").AsBoolean = DiminishedPriceAllowed;
                vl_params.Add("DifferentialPriceAllowed").AsBoolean = DifferentialPriceAllowed;
                vl_params.Add("OppositeDirectionAllowed").AsBoolean = OppositeDirectionAllowed;
                vl_params.Add("DeltaT").AsInt32 = DeltaT;
                vl_params.Add("DeltaT1").AsInt32 = DeltaT1;
                vl_params.Add("SellWarrantyPercent").AsDouble = SellWarrantyPercent;
                vl_params.Add("SellWarrantyMU").AsDouble = SellWarrantyMU;
                vl_params.Add("SellWarrantyFixed").AsDouble = SellWarrantyFixed;
                vl_params.Add("BuyWarrantyPercent").AsDouble = BuyWarrantyPercent;
                vl_params.Add("BuyWarrantyMU").AsDouble = BuyWarrantyMU;
                vl_params.Add("BuyWarrantyFixed").AsDouble = BuyWarrantyFixed;
                vl_params.Add("DaysOfWeekSTR").AsString = DaysOfWeekSTR;


                exec = addAssetSchedule(vl_params);
                int_count += exec.RowsModified;

                //step 4 set the asset trade parameters
                vl_params = new TVariantList();
                vl_params.Add("ID_Asset").AsInt32 = ID_Asset;
                DataSet ds_tradeparameters = getAssetTradeParameters(vl_params);

                if (!(app.DB.ValidDS(ds_tradeparameters))) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
                if (!app.DB.ValidDSRows(ds_tradeparameters)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "No asset trading params found!");

                PartialFlagChangeAllowed = Convert.ToBoolean(ds_tradeparameters.Tables[0].Rows[0]["PartialFlagChangeAllowed"]);
                InitialPriceMandatory = Convert.ToBoolean(ds_tradeparameters.Tables[0].Rows[0]["InitialPriceMandatory"]);
                InitialPriceMaintenance = Convert.ToBoolean(ds_tradeparameters.Tables[0].Rows[0]["InitialPriceMaintenance"]);
                DiminishedQuantityAllowed = Convert.ToBoolean(ds_tradeparameters.Tables[0].Rows[0]["DiminishedQuantityAllowed"]);
                DiminishedPriceAllowed = Convert.ToBoolean(ds_tradeparameters.Tables[0].Rows[0]["DiminishedPriceAllowed"]);
                DifferentialPriceAllowed = Convert.ToBoolean(ds_tradeparameters.Tables[0].Rows[0]["DifferentialPriceAllowed"]);
                OppositeDirectionAllowed = Convert.ToBoolean(ds_tradeparameters.Tables[0].Rows[0]["OppositeDirectionAllowed"]);
                SellWarrantyPercent = Convert.ToDouble(ds_tradeparameters.Tables[0].Rows[0]["SellWarrantyPercent"]);
                SellWarrantyMU = Convert.ToDouble(ds_tradeparameters.Tables[0].Rows[0]["SellWarrantyMU"]);
                SellWarrantyFixed = Convert.ToDouble(ds_tradeparameters.Tables[0].Rows[0]["SellWarrantyFixed"]);
                BuyWarrantyPercent = Convert.ToDouble(ds_tradeparameters.Tables[0].Rows[0]["BuyWarrantyPercent"]);
                BuyWarrantyMU = Convert.ToDouble(ds_tradeparameters.Tables[0].Rows[0]["BuyWarrantyMU"]);
                BuyWarrantyFixed = Convert.ToDouble(ds_tradeparameters.Tables[0].Rows[0]["BuyWarrantyFixed"]);

                vl_params = new TVariantList();
                vl_params.Add("PartialFlagChangeAllowed").AsBoolean = PartialFlagChangeAllowed;
                vl_params.Add("InitialPriceMandatory").AsBoolean = InitialPriceMandatory;
                vl_params.Add("InitialPriceMaintenance").AsBoolean = InitialPriceMaintenance;
                vl_params.Add("DiminishedQuantityAllowed").AsBoolean = DiminishedQuantityAllowed;
                vl_params.Add("DiminishedPriceAllowed").AsBoolean = DiminishedPriceAllowed;
                vl_params.Add("DifferentialPriceAllowed").AsBoolean = DifferentialPriceAllowed;
                vl_params.Add("OppositeDirectionAllowed").AsBoolean = OppositeDirectionAllowed;
                vl_params.Add("SellWarrantyPercent").AsDouble = SellWarrantyPercent;
                vl_params.Add("SellWarrantyMU").AsDouble = SellWarrantyMU;
                vl_params.Add("SellWarrantyFixed").AsDouble = SellWarrantyFixed;
                vl_params.Add("BuyWarrantyPercent").AsDouble = BuyWarrantyPercent;
                vl_params.Add("BuyWarrantyMU").AsDouble = BuyWarrantyMU;
                vl_params.Add("BuyWarrantyFixed").AsDouble = BuyWarrantyFixed;
                updateTradeParameters(0, ID_NewAsset, 0, 0, vl_params);

                //step 5. set asset clients
                vl_params = new TVariantList();
                vl_params.Add("ID_Asset").AsInt32 = ID_Asset;
                DataSet ds_clients = getAssetClients(vl_params);
                if (!(app.DB.ValidDS(ds_clients))) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
                if (app.DB.ValidDSRows(ds_clients))
                    for (int i = 0; i < ds_clients.Tables[0].Rows.Count; i++)
                    {
                        int idAsset = ID_NewAsset;
                        int idClient = Convert.ToInt32(ds_clients.Tables[0].Rows[i]["ID_Client"]);
                        bool canBuy = Convert.ToBoolean(ds_clients.Tables[0].Rows[i]["canBuy"]);
                        bool canSell = Convert.ToBoolean(ds_clients.Tables[0].Rows[i]["canSell"]);

                        vl_params = new TVariantList();
                        vl_params.Add("ID_Client").AsInt32 = idClient;
                        vl_params.Add("ID_Asset").AsInt32 = idAsset;
                        vl_params.Add("canBuy").AsBoolean = canBuy;
                        vl_params.Add("canSell").AsBoolean = canSell;
                        exec = setClient2Asset(vl_params);
                        int_count += exec.RowsModified;
                    }

                //step 6. set asset documents
                vl_params = new TVariantList();
                vl_params.Add("ID_Asset").AsInt32 = ID_Asset;
                DataSet ds_documents = getDocuments(vl_params);
                if (!(app.DB.ValidDS(ds_documents))) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
                if (app.DB.ValidDSRows(ds_documents))
                    for (int i = 0; i < ds_documents.Tables[0].Rows.Count; i++)
                    {
                        int ID_DocumentType = Convert.ToInt32(ds_documents.Tables[0].Rows[i]["ID_DocumentType"]);
                        Name = Convert.ToString(ds_documents.Tables[0].Rows[i]["Name"]);
                        bool isPublic = Convert.ToBoolean(ds_documents.Tables[0].Rows[i]["isPublic"]);
                        DateTime DateCreated = Convert.ToDateTime(ds_documents.Tables[0].Rows[i]["DateCreated"]);
                        DateTime LastModifiedDate = Convert.ToDateTime(ds_documents.Tables[0].Rows[i]["LastModifiedDate"]);
                        int ID_CreatedByUser = Convert.ToInt32(ds_documents.Tables[0].Rows[i]["ID_CreatedByUser"]);
                        string FileName = ds_documents.Tables[0].Rows[i]["FileName"].ToString();
                        string DocumentURL = Convert.ToString(ds_documents.Tables[0].Rows[i]["DocumentURL"]);

                        vl_params = new TVariantList();
                        vl_params.Add("ID_Asset").AsInt32 = ID_NewAsset;
                        vl_params.Add("ID_DocumentType").AsInt32 = ID_DocumentType;
                        vl_params.Add("Name").AsString = Name;
                        vl_params.Add("isPublic").AsBoolean = isPublic;
                        vl_params.Add("DateCreated").AsDateTime = DateTime.Now;
                        vl_params.Add("LastModifiedDate").AsDateTime = DateTime.Now;
                        vl_params.Add("ID_CreatedByUser").AsInt32 = ID_CreatedByUser;
                        vl_params.Add("FileName").AsString = FileName;
                        vl_params.Add("DocumentURL").AsString = DocumentURL;

                        exec = addDocument(vl_params);
                        int_count += exec.RowsModified;
                    }

                //app.DB.CommitTransaction();

                //step 7. set asset warranties
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
                DataSet dsAssetWarrantyTypes = app.DB.Select("select_AssetWarrantyType", "Rings", vl_params);
                if (app.DB.ValidDSRows(dsAssetWarrantyTypes))
                {
                    for (int i = 0; i < dsAssetWarrantyTypes.Tables[0].Rows.Count; i++)
                    {
                        int warrantyTypeID = Convert.ToInt32(dsAssetWarrantyTypes.Tables[0].Rows[i]["ID_WarrantyType"]);
                        vl_params = new TVariantList();
                        vl_params.Add("ID_Asset").AsInt32 = ID_NewAsset;
                        vl_params.Add("ID_WarrantyType").AsInt32 = warrantyTypeID;
                        vl_params.Add("isDeleted").AsBoolean = false;
                        setWarrantyType2Asset(vl_params);
                    }
                }

                res.RowsModified = int_count;
                res.IDs.Add("ID_Asset").AsInt32 = ID_NewAsset;
                return res;
            }
            catch (Exception exc)
            {
                //app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public DataSet getAssets(TVariantList vl_arguments)
        {
            DataSet ds = app.DB.Select("select_AssetsbyID_MarketandID_Ring", "Assets", vl_arguments);
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
            DataSet ds = app.DB.Select("select_AssetSchedulesDetailed", "AssetSchedules", vl_params);
            return ds;
        }

        public DataSet getInitialOrder(int ID_Asset/*TVariantList vl_arguments*/)
        {
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;

            return app.DB.Select("select_InitialOrders", "Orders", vl_params);
        }

        public DataSet getAssetTradeParameters(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Asset"] == null) return null;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Asset").AsInt32 = vl_arguments["ID_Asset"].AsInt32;

            DataSet ds = app.DB.Select("select_AssetTradeParameters", "Assets", vl_params);
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

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Document").AsInt32 = ID_Document;
            vl_params.Add("@prm_ID_Asset").AsInt32 = ID_Asset;
            DataSet ds = app.DB.Select("select_Documents", "Documents", vl_params);
            return ds;
        }

        public TDBExecResult editOperationsBuffer(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_BO_OperationBuffer"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_BO_OperationBuffer");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_BO_OperationBuffer = 0;
            if (vl_arguments["ID_BO_OperationBuffer"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_BO_OperationBuffer"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_BO_OperationBuffer"].ValueType == TVariantType.vtInt64) ID_BO_OperationBuffer = vl_arguments["ID_BO_OperationBuffer"].AsInt32;
            else res.ParamValidations["ID_BO_OperationBuffer"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_BO_OperationBuffer").AsInt32 = ID_BO_OperationBuffer;

                int int_rows = app.DB.Exec("update_BO_OperantionsBuffer", "BO_OperationsBuffer", vl_params);
                if (int_rows <= 0) throw new Exception("SQL execution error");

                app.DB.CommitTransaction();

                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public void updateProcedureOptions(int ID_Procedure, TVariantList vl_arguments)
        {
            int ID = 0;
            string Forms = "";
            string EconomicCapacity = "";
            string TechnicalCapacity = "";
            DateTime ClarificationRequestsDeadline = DateTime.Now;
            DateTime TendersReceiptDeadline = DateTime.Now;
            DateTime TendersOpeningDate = DateTime.Now;
            string ContestationsSubmission = "";
            string OtherInformation = "";
            string Necessity = "";

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Procedure").AsInt32 = ID_Procedure;
            DataSet ds = app.DB.Select("select_ProcedureOptions", "ProcedureOptions", vl_params);
            if (app.DB.ValidDSRows(ds))
            {
                ID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                Forms = ds.Tables[0].Rows[0]["Forms"].ToString();
                EconomicCapacity = ds.Tables[0].Rows[0]["EconomicCapacity"].ToString();
                TechnicalCapacity = ds.Tables[0].Rows[0]["TechnicalCapacity"].ToString();
                ClarificationRequestsDeadline = Convert.ToDateTime(ds.Tables[0].Rows[0]["ClarificationRequestsDeadline"]);
                TendersReceiptDeadline = Convert.ToDateTime(ds.Tables[0].Rows[0]["TendersReceiptDeadline"]);
                TendersOpeningDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["TendersOpeningDate"]);
                ContestationsSubmission = ds.Tables[0].Rows[0]["ContestationsSubmission"].ToString();
                OtherInformation = ds.Tables[0].Rows[0]["OtherInformation"].ToString();
                Necessity = ds.Tables[0].Rows[0]["Necessity"].ToString();
            }

            if (vl_arguments["Forms"] != null) Forms = vl_arguments["Forms"].AsString;
            if (vl_arguments["EconomicCapacity"] != null) EconomicCapacity = vl_arguments["EconomicCapacity"].AsString;
            if (vl_arguments["TechnicalCapacity"] != null) TechnicalCapacity = vl_arguments["TechnicalCapacity"].AsString;
            if (vl_arguments["ClarificationRequestsDeadline"] != null) ClarificationRequestsDeadline = vl_arguments["ClarificationRequestsDeadline"].AsDateTime;
            if (vl_arguments["TendersReceiptDeadline"] != null) TendersReceiptDeadline = vl_arguments["TendersReceiptDeadline"].AsDateTime;
            if (vl_arguments["TendersOpeningDate"] != null) TendersOpeningDate = vl_arguments["TendersOpeningDate"].AsDateTime;
            if (vl_arguments["ContestationsSubmission"] != null) ContestationsSubmission = vl_arguments["ContestationsSubmission"].AsString;
            if (vl_arguments["OtherInformation"] != null) OtherInformation = vl_arguments["OtherInformation"].AsString;
            if (vl_arguments["Necessity"] != null) Necessity = vl_arguments["Necessity"].AsString;

            vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Procedure").AsInt32 = ID_Procedure;
            vl_params.Add("@prm_Forms").AsString = Forms;
            vl_params.Add("@prm_EconomicCapacity").AsString = EconomicCapacity;
            vl_params.Add("@prm_TechnicalCapacity").AsString = TechnicalCapacity;
            vl_params.Add("@prm_ClarificationRequestsDeadline").AsDateTime = ClarificationRequestsDeadline;
            vl_params.Add("@prm_TendersReceiptDeadline").AsDateTime = TendersReceiptDeadline;
            vl_params.Add("@prm_TendersOpeningDate").AsDateTime = TendersOpeningDate;
            vl_params.Add("@prm_ContestationsSubmission").AsString = ContestationsSubmission;
            vl_params.Add("@prm_OtherInformation").AsString = OtherInformation;
            vl_params.Add("@prm_Necessity").AsString = Necessity;

            if (ID == 0) app.DB.Exec("insert_ProcedureOption", "ProcedureOptions", vl_params);
            else
            {
                vl_params.Add("@prm_ID").AsInt32 = ID;
                int count = app.DB.Exec("update_ProcedureOption", "ProcedureOptions", vl_params);
            }
        }

        public TDBExecResult addProcedure(TVariantList vl_arguments)
        {
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["Description"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Description");
            if (vl_arguments["Location"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Location");
            if (vl_arguments["Legislation"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Legislation");
            if (vl_arguments["Duration"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Duration");
            if (vl_arguments["TotalValue"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TotalValue");
            if (vl_arguments["ID_ContractType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_ContractType");
            if (vl_arguments["ID_ProcedureType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_ProcedureType");
            if (vl_arguments["ID_ProcedureCriterion"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_ProcedureCriterion");
            if (vl_arguments["ClassificationIDs"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ClassificationIDs");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Agency = 0;
            int ID_Broker = 0;
            int ID_Client = 0;

            UserValidation uv = new UserValidation(app);
            if (uv.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole))
            {
                if (vl_arguments["ID_Agency"] != null) ID_Agency = vl_arguments["ID_Agency"].AsInt32;
                if (vl_arguments["ID_Broker"] != null) ID_Broker = vl_arguments["ID_Broker"].AsInt32;
            }
            else ID_Broker = session.ID_Broker;

            if (vl_arguments["ID_Client"] != null) ID_Client = vl_arguments["ID_Client"].AsInt32;

            TVariantList vl_params;
            if (ID_Broker != 0)
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                DataSet ds_client = app.DB.Select("select_HousebyID_Broker", "Clients", vl_params);
                if (app.DB.ValidDSRows(ds_client))
                {
                    ID_Agency = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID_Agency"]);
                    if (ID_Client == 0) ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
                }
            }

            if (ID_Agency == 0 && (ID_Broker == 0 || ID_Client == 0)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Current user is not attached to a broker");

            string Name = "";
            if (vl_arguments["Name"].ValueType == TVariantType.vtString) Name = vl_arguments["Name"].AsString;
            else res.ParamValidations["Name"].AsString = "Value not recognized";

            string Description = "";
            if (vl_arguments["Description"].ValueType == TVariantType.vtString) Description = vl_arguments["Description"].AsString;
            else res.ParamValidations["Description"].AsString = "Value not recognized";

            string Location = "";
            if (vl_arguments["Location"].ValueType == TVariantType.vtString) Location = vl_arguments["Location"].AsString;
            else res.ParamValidations["Location"].AsString = "Value not recognized";

			string Legislation = "";
            if (vl_arguments["Legislation"].ValueType == TVariantType.vtString) Legislation = vl_arguments["Legislation"].AsString;
            else res.ParamValidations["Legislation"].AsString = "Value not recognized";

			int Duration = 0;
            if (vl_arguments["Duration"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Duration"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Duration"].ValueType == TVariantType.vtInt64) Duration = vl_arguments["Duration"].AsInt32;
            else res.ParamValidations["Duration"].AsString = "Value not recognized";

            double TotalValue = 0;
            if (vl_arguments["TotalValue"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["TotalValue"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["TotalValue"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["TotalValue"].ValueType == TVariantType.vtSingle ||
                vl_arguments["TotalValue"].ValueType == TVariantType.vtDouble) TotalValue = vl_arguments["TotalValue"].AsDouble;
            else res.ParamValidations["TotalValue"].AsString = "Value not recognized";

			int ID_ContractType = 0;
            if (vl_arguments["ID_ContractType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_ContractType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_ContractType"].ValueType == TVariantType.vtInt64) ID_ContractType = vl_arguments["ID_ContractType"].AsInt32;
            else res.ParamValidations["ID_ContractType"].AsString = "Value not recognized";

			int ID_ProcedureType = 0;
            if (vl_arguments["ID_ProcedureType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_ProcedureType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_ProcedureType"].ValueType == TVariantType.vtInt64) ID_ProcedureType = vl_arguments["ID_ProcedureType"].AsInt32;
            else res.ParamValidations["ID_ProcedureType"].AsString = "Value not recognized";
            
            int ID_ProcedureCriterion = 0;
            if (vl_arguments["ID_ProcedureCriterion"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_ProcedureCriterion"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_ProcedureCriterion"].ValueType == TVariantType.vtInt64) ID_ProcedureCriterion = vl_arguments["ID_ProcedureCriterion"].AsInt32;
            else res.ParamValidations["ID_ProcedureCriterion"].AsString = "Value not recognized";

            TVariantList vl_Classification = new TVariantList();
            if (vl_arguments["ClassificationIDs"].ValueType == TVariantType.vtObject)
                vl_Classification = (TVariantList)vl_arguments["ClassificationIDs"].AsObject;

            string Status = "draft";
            if (vl_arguments["Status"] != null)
            {
                if (vl_arguments["Status"].ValueType == TVariantType.vtString) Name = vl_arguments["Status"].AsString;
                else res.ParamValidations["Status"].AsString = "Value not recognized";
            }

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs = new TVariantList();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Agency").AsInt32 = ID_Agency;
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                vl_params.Add("@prm_Name").AsString = Name;
                vl_params.Add("@prm_Description").AsString = Description;
                vl_params.Add("@prm_ID_ContractType").AsInt32 = ID_ContractType;
                vl_params.Add("@prm_Location").AsString = Location;
                vl_params.Add("@prm_Legislation").AsString = Legislation;
                vl_params.Add("@prm_Duration").AsInt32 = Duration;
                vl_params.Add("@prm_TotalValue").AsDouble = TotalValue;
                vl_params.Add("@prm_ID_ProcedureType").AsInt32 = ID_ProcedureType;
                vl_params.Add("@prm_ID_ProcedureCriterion").AsInt32 = ID_ProcedureCriterion;
                vl_params.Add("@prm_Status").AsString = Status;
                int int_rows = app.DB.Exec("insert_Procedure", "Procedures", vl_params);
                if (int_rows <= 0) throw new Exception("SQL execution error");

                int ID_Procedure = 0;
                DataSet ds_last = app.DB.Select("select_LastProcedure", "Procedures", null);
                if (app.DB.ValidDSRows(ds_last)) ID_Procedure = Convert.ToInt32(ds_last.Tables[0].Rows[0]["ID"]);

                updateProcedureOptions(ID_Procedure, vl_arguments);

                //  Add ProcedureClassification
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Procedure").AsInt32 = ID_Procedure;
                vl_params.Add("@prm_ID_CPV").AsInt32 = 0;
                for (int i = 0; i < vl_Classification.Count; i++)
                {
                    int ID_CPV = vl_Classification[i].AsInt32;
                    vl_params["@prm_ID_CPV"].AsInt32 = ID_CPV;
                    app.DB.Exec("insert_ProcedureClassification", "ProcedureClassification", vl_params);
                }

                app.DB.CommitTransaction();

                res.RowsModified = int_rows;
                res.IDs.Add("ID_Procedure").AsInt32 = ID_Procedure;

                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult editProcedure(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Procedure"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Procedure");
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["Description"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Description");
            if (vl_arguments["Location"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Location");
            if (vl_arguments["Legislation"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Legislation");
            if (vl_arguments["Duration"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Duration");
            if (vl_arguments["TotalValue"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TotalValue");
            if (vl_arguments["ID_ContractType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_ContractType");
            if (vl_arguments["ID_ProcedureType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_ProcedureType");
            if (vl_arguments["ID_ProcedureCriterion"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_ProcedureCriterion");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Procedure = 0;
            if (vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt64) ID_Procedure = vl_arguments["ID_Procedure"].AsInt32;
            else res.ParamValidations["ID_Procedure"].AsString = "Value not recognized";

            string Name = "";
            if (vl_arguments["Name"].ValueType == TVariantType.vtString) Name = vl_arguments["Name"].AsString;
            else res.ParamValidations["Name"].AsString = "Value not recognized";

            string Description = "";
            if (vl_arguments["Description"].ValueType == TVariantType.vtString) Description = vl_arguments["Description"].AsString;
            else res.ParamValidations["Description"].AsString = "Value not recognized";

            string Location = "";
            if (vl_arguments["Location"].ValueType == TVariantType.vtString) Location = vl_arguments["Location"].AsString;
            else res.ParamValidations["Location"].AsString = "Value not recognized";

            string Legislation = "";
            if (vl_arguments["Legislation"].ValueType == TVariantType.vtString) Legislation = vl_arguments["Legislation"].AsString;
            else res.ParamValidations["Legislation"].AsString = "Value not recognized";

            int Duration = 0;
            if (vl_arguments["Duration"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Duration"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Duration"].ValueType == TVariantType.vtInt64) Duration = vl_arguments["Duration"].AsInt32;
            else res.ParamValidations["Duration"].AsString = "Value not recognized";

            double TotalValue = 0;
            if (vl_arguments["TotalValue"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["TotalValue"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["TotalValue"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["TotalValue"].ValueType == TVariantType.vtSingle ||
                vl_arguments["TotalValue"].ValueType == TVariantType.vtDouble) TotalValue = vl_arguments["TotalValue"].AsDouble;
            else res.ParamValidations["TotalValue"].AsString = "Value not recognized";

            int ID_ContractType = 0;
            if (vl_arguments["ID_ContractType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_ContractType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_ContractType"].ValueType == TVariantType.vtInt64) ID_ContractType = vl_arguments["ID_ContractType"].AsInt32;
            else res.ParamValidations["ID_ContractType"].AsString = "Value not recognized";

            int ID_ProcedureType = 0;
            if (vl_arguments["ID_ProcedureType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_ProcedureType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_ProcedureType"].ValueType == TVariantType.vtInt64) ID_ProcedureType = vl_arguments["ID_ProcedureType"].AsInt32;
            else res.ParamValidations["ID_ProcedureType"].AsString = "Value not recognized";

            int ID_ProcedureCriterion = 0;
            if (vl_arguments["ID_ProcedureCriterion"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_ProcedureCriterion"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_ProcedureCriterion"].ValueType == TVariantType.vtInt64) ID_ProcedureCriterion = vl_arguments["ID_ProcedureCriterion"].AsInt32;
            else res.ParamValidations["ID_ProcedureCriterion"].AsString = "Value not recognized";

            TVariantList vl_Classification = null;
            if (vl_arguments["ClassificationIDs"] != null && vl_arguments["ClassificationIDs"].ValueType == TVariantType.vtObject)
                vl_Classification = (TVariantList)vl_arguments["ClassificationIDs"].AsObject;

            string Status = "";
            if (vl_arguments["Status"] != null)
            {
                if (vl_arguments["Status"].ValueType == TVariantType.vtString) Name = vl_arguments["Status"].AsString;
                else res.ParamValidations["Status"].AsString = "Value not recognized";
            }

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_Procedure;
                vl_params.Add("@prm_Name").AsString = Name;
                vl_params.Add("@prm_Description").AsString = Description;
                vl_params.Add("@prm_ID_ContractType").AsInt32 = ID_ContractType;
                vl_params.Add("@prm_Location").AsString = Location;
                vl_params.Add("@prm_Legislation").AsString = Legislation;
                vl_params.Add("@prm_Duration").AsInt32 = Duration;
                vl_params.Add("@prm_TotalValue").AsDouble = TotalValue;
                vl_params.Add("@prm_ID_ProcedureType").AsInt32 = ID_ProcedureType;
                vl_params.Add("@prm_ID_ProcedureCriterion").AsInt32 = ID_ProcedureCriterion;
                vl_params.Add("@prm_Status").AsString = Status;

                int int_rows = app.DB.Exec("update_Procedure", "Procedures", vl_params);
                if (int_rows <= 0) throw new Exception("SQL execution error");

                updateProcedureOptions(ID_Procedure, vl_arguments);

                if (vl_Classification != null)
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_Procedure").AsInt32 = ID_Procedure;
                    app.DB.Exec("delete_ProcedureClassification", "ProcedureClassification", vl_params);

                    vl_params.Add("@prm_ID_CPV").AsInt32 = 0;
                    for (int i = 0; i < vl_Classification.Count; i++)
                    {
                        int ID_CPV = vl_Classification[i].AsInt32;
                        vl_params["@prm_ID_CPV"].AsInt32 = ID_CPV;
                        app.DB.Exec("insert_ProcedureClassification", "ProcedureClassification", vl_params);
                    }
                }

                app.DB.CommitTransaction();

                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult deleteProcedure(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Procedure"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Procedure");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Procedure = 0;
            if (vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt64) ID_Procedure = vl_arguments["ID_Procedure"].AsInt32;
            else res.ParamValidations["ID_Procedure"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_Procedure;

                int int_rows = app.DB.Exec("delete_Procedure", "Procedures", vl_params);
                if (int_rows <= 0) throw new Exception("SQL execution error");

                app.DB.CommitTransaction();

                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult addProcedureDocument(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_Procedure"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Procedure");
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["DocumentURL"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "DocumentURL");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Procedure = 0;
            if (vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt64) ID_Procedure = vl_arguments["ID_Procedure"].AsInt32;
            else res.ParamValidations["ID_Procedure"].AsString = "Value not recognized";

            int ID_CreatedByUser = session.ID_User;
            /*if (vl_arguments["ID_CreatedByUser"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_CreatedByUser"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_CreatedByUser"].ValueType == TVariantType.vtInt64) ID_CreatedByUser = vl_arguments["ID_CreatedByUser"].AsInt32;
            else res.ParamValidations["ID_CreatedByUser"].AsString = "Value not recognized";*/

            double Value = double.NaN;
            if (vl_arguments["Value"] != null)
            {
                if (vl_arguments["Value"].ValueType == TVariantType.vtString && vl_arguments["Value"].AsString == "none") ;
                else
                {
                    if (vl_arguments["Value"].ValueType == TVariantType.vtInt16 ||
                        vl_arguments["Value"].ValueType == TVariantType.vtInt32 ||
                        vl_arguments["Value"].ValueType == TVariantType.vtInt64 ||
                        vl_arguments["Value"].ValueType == TVariantType.vtSingle ||
                        vl_arguments["Value"].ValueType == TVariantType.vtDouble) Value = vl_arguments["Value"].AsDouble;
                    else res.ParamValidations["Value"].AsString = "Unrecognized Value";
                }
            }

            string str_Name = "";
            if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
            else res.ParamValidations["Name"].AsString = "Value not recognized";

            string documentURL = "";
            if (vl_arguments["DocumentURL"].ValueType == TVariantType.vtString) documentURL = vl_arguments["DocumentURL"].AsString;
            else res.ParamValidations["DocumentURL"].AsString = "Value not recognized";

            string FileName = "";
            if (vl_arguments["FileName"] != null)
            {
                if (vl_arguments["FileName"].ValueType == TVariantType.vtString) FileName = vl_arguments["FileName"].AsString;
            }

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Procedure").AsInt32 = ID_Procedure;
            vl_params.Add("@prm_Name").AsString = str_Name;
            vl_params.Add("@prm_DateCreated").AsDateTime = DateTime.Now;
            vl_params.Add("@prm_LastModifiedDate").AsDateTime = DateTime.Now;
            vl_params.Add("@prm_ID_CreatedByUser").AsInt32 = ID_CreatedByUser;
            vl_params.Add("@prm_DocumentURL").AsString = documentURL;
            vl_params.Add("@prm_FileName").AsString = FileName;
            vl_params.Add("@prm_Value").AsDouble = Value;

            int int_rows = app.DB.Exec("insert_ProcedureDocument", "ProcedureDocuments", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult editProcedureDocument(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_Document"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Document");
            if (vl_arguments["ID_Procedure"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Procedure");
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            //if (vl_arguments["DocumentURL"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "DocumentURL");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Document = 0;
            if (vl_arguments["ID_Document"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Document"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Document"].ValueType == TVariantType.vtInt64) ID_Document = vl_arguments["ID_Document"].AsInt32;
            else res.ParamValidations["ID_Document"].AsString = "Value not recognized";

            int ID_Procedure = 0;
            if (vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt64) ID_Procedure = vl_arguments["ID_Procedure"].AsInt32;
            else res.ParamValidations["ID_Procedure"].AsString = "Value not recognized";

            string str_Name = "";
            if (vl_arguments["Name"].ValueType == TVariantType.vtString) str_Name = vl_arguments["Name"].AsString;
            else res.ParamValidations["Name"].AsString = "Value not recognized";

            string documentURL = "";
            if (vl_arguments["DocumentURL"] != null)
            {
                if (vl_arguments["DocumentURL"].ValueType == TVariantType.vtString) documentURL = vl_arguments["DocumentURL"].AsString;
                else res.ParamValidations["DocumentURL"].AsString = "Value not recognized";
            }

            string FileName = "";
            if (vl_arguments["FileName"] != null)
            {
                if (vl_arguments["FileName"].ValueType == TVariantType.vtString) FileName = vl_arguments["FileName"].AsString;
            }

            double Value = double.NaN;
            if (vl_arguments["Value"] != null)
            {
                if (vl_arguments["Value"].ValueType == TVariantType.vtString && vl_arguments["Value"].AsString == "none") ;
                else
                {
                    if (vl_arguments["Value"].ValueType == TVariantType.vtInt16 ||
                        vl_arguments["Value"].ValueType == TVariantType.vtInt32 ||
                        vl_arguments["Value"].ValueType == TVariantType.vtInt64 ||
                        vl_arguments["Value"].ValueType == TVariantType.vtSingle ||
                        vl_arguments["Value"].ValueType == TVariantType.vtDouble) Value = vl_arguments["Value"].AsDouble;
                    else res.ParamValidations["Value"].AsString = "Unrecognized Value";
                }
            }

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Procedure").AsInt32 = ID_Procedure;
            vl_params.Add("@prm_Name").AsString = str_Name;
            vl_params.Add("@prm_LastModifiedDate").AsDateTime = DateTime.Now;
            vl_params.Add("@prm_DocumentURL").AsString = documentURL;
            vl_params.Add("@prm_FileName").AsString = FileName;
            vl_params.Add("@prm_Value").AsDouble = Value;
            vl_params.Add("@prm_ID").AsInt32 = ID_Document;

            int int_rows = app.DB.Exec("update_ProcedureDocument", "ProcedureDocuments", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            if (documentURL != "")
            {
                int_rows = app.DB.Exec("update_ProcedureDocumentURL", "ProcedureDocuments", vl_params);
                if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            }

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult deleteProcedureDocument(TVariantList vl_arguments)
        {
            if (vl_arguments == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "");
            if (vl_arguments["ID_Document"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Document");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Document = 0;
            if (vl_arguments["ID_Document"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Document"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Document"].ValueType == TVariantType.vtInt64) ID_Document = vl_arguments["ID_Document"].AsInt32;
            else res.ParamValidations["ID_Document"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Document;
            int int_rows = app.DB.Exec("delete_ProcedureDocument", "ProcedureDocuments", vl_params);
            if (int_rows <= 0) return new TDBExecResult(TDBExecError.SQLExecutionError, "");

            return new TDBExecResult(TDBExecError.Success, "", int_rows);
        }

        public TDBExecResult setFavouriteProcedure(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Procedure"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Procedure");

            int ID_Agency = 0;
            int ID_Broker = 0;
            int ID_Client = 0;

            UserValidation uv = new UserValidation(app);
            if (uv.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole))
            {
                if (vl_arguments["ID_Agency"] != null) ID_Agency = vl_arguments["ID_Agency"].AsInt32;
                if (vl_arguments["ID_Broker"] != null) ID_Broker = vl_arguments["ID_Broker"].AsInt32;
            }
            else ID_Broker = session.ID_Broker;

            if (vl_arguments["ID_Client"] != null) ID_Client = vl_arguments["ID_Client"].AsInt32;

            TVariantList vl_params;
            if (ID_Broker != 0)
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                DataSet ds_client = app.DB.Select("select_HousebyID_Broker", "Clients", vl_params);
                if (app.DB.ValidDSRows(ds_client))
                {
                    ID_Agency = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID_Agency"]);
                    if (ID_Client == 0) ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
                }
            }

            if (ID_Agency == 0 && (ID_Broker == 0 || ID_Client == 0)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Current user is not attached to a broker");

            int ID_Procedure = 0;
            if (vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt64) ID_Procedure = vl_arguments["ID_Procedure"].AsInt32;
            else return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "ID_Procedure invalid");

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");
            try
            {
                TDBExecResult res = new TDBExecResult(TDBExecError.Success, "");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                vl_params.Add("@prm_ID_Procedure").AsInt32 = ID_Procedure;

                int int_rows = app.DB.Exec("setFavouriteProcedure", "FavouriteProcedures", vl_params);
                if (int_rows < 0) throw new Exception("SQL execution error");

                app.DB.CommitTransaction();

                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult resetFavouriteProcedure(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Procedure"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Procedure");

            int ID_Agency = 0;
            int ID_Broker = 0;
            int ID_Client = 0;

            UserValidation uv = new UserValidation(app);
            if (uv.isAdministrator(session.ID_Bursary, session.ID_User, session.ID_UserRole))
            {
                if (vl_arguments["ID_Agency"] != null) ID_Agency = vl_arguments["ID_Agency"].AsInt32;
                if (vl_arguments["ID_Broker"] != null) ID_Broker = vl_arguments["ID_Broker"].AsInt32;
            }
            else ID_Broker = session.ID_Broker;

            if (vl_arguments["ID_Client"] != null) ID_Client = vl_arguments["ID_Client"].AsInt32;

            TVariantList vl_params;
            if (ID_Broker != 0)
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Broker").AsInt32 = ID_Broker;
                DataSet ds_client = app.DB.Select("select_HousebyID_Broker", "Clients", vl_params);
                if (app.DB.ValidDSRows(ds_client))
                {
                    ID_Agency = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID_Agency"]);
                    if (ID_Client == 0) ID_Client = Convert.ToInt32(ds_client.Tables[0].Rows[0]["ID"]);
                }
            }

            if (ID_Agency == 0 && (ID_Broker == 0 || ID_Client == 0)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Current user is not attached to a broker");

            int ID_Procedure = 0;
            if (vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt64) ID_Procedure = vl_arguments["ID_Procedure"].AsInt32;
            else return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "ID_Procedure invalid");

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");
            try
            {
                TDBExecResult res = new TDBExecResult(TDBExecError.Success, "");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Client").AsInt32 = ID_Client;
                vl_params.Add("@prm_ID_Procedure").AsInt32 = ID_Procedure;

                int int_rows = app.DB.Exec("resetFavouriteProcedure", "FavouriteProcedures", vl_params);
                if (int_rows < 0) throw new Exception("SQL execution error");

                app.DB.CommitTransaction();

                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult setProcedureStatus(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Procedure"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Procedure");
            if (vl_arguments["Status"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Status");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Procedure = 0;
            if (vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt64) ID_Procedure = vl_arguments["ID_Procedure"].AsInt32;
            else res.ParamValidations["ID_Procedure"].AsString = "Value not recognized";

            string Status = vl_arguments["Status"].AsString.Trim().ToLower();

            if (res.bInvalid()) return res;

            //  retrieve the existing procedure
            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_Procedure;
            DataSet ds = app.DB.Select("select_Procedures", "Procedures", vl_params);
            if (!app.DB.ValidDSRows(ds)) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "non existing procedure");

            DateTime DBNow = Convert.ToDateTime(ds.Tables[0].Rows[0]["DBNow"]);
            int ID_AgencyWinner = 0;

            string old_status = ds.Tables[0].Rows[0]["Status"].ToString().Trim().ToLower();
            switch (old_status)
            {
                case "draft":
                    if (Status != "review" && Status != "canceled") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "invalid requested status " + Status);
                    break;
                case "review":
                    if (Status != "approved" && Status != "canceled") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "invalid requested status " + Status);

                    break;
                case "approved":
                    if (Status != "closed" && Status != "canceled") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "invalid requested status " + Status);

                    if (Status == "closed")
                    {
                        if (Convert.ToDateTime(ds.Tables[0].Rows[0]["TendersOpeningDate"]) > DBNow) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "Data si ora deschiderii ofertelor nu este respectata");

                        if (vl_arguments["ID_AgencyWinner"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_AgencyWinner");
                        if (vl_arguments["ID_AgencyWinner"].ValueType != TVariantType.vtInt16 &&
                            vl_arguments["ID_AgencyWinner"].ValueType != TVariantType.vtInt32 &&
                            vl_arguments["ID_AgencyWinner"].ValueType != TVariantType.vtInt64) return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "unrecognized value ID_AgencyWinner");

                        ID_AgencyWinner = vl_arguments["ID_AgencyWinner"].AsInt32;
                    }

                    break;
                case "closed":
                    if (Status != "canceled") return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "invalid requested status " + Status);
                    break;
            }

            vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Procedure").AsInt32 = ID_Procedure;
            vl_params.Add("@prm_Status").AsString = Status;
            vl_params.Add("@prm_ID_AgencyWinner").AsInt32 = ID_AgencyWinner;
            int numrows = app.DB.Exec("update_ProcedureStatus", "Procedures", vl_params);

            return new TDBExecResult(TDBExecError.Success, "", numrows);
        }

        public TDBExecResult addForm(TVariantList vl_arguments)
        {
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["Step"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Step");
            if (vl_arguments["isActive"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isValid");
            if (vl_arguments["Template"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Template");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            string Name = "";
            if (vl_arguments["Name"].ValueType == TVariantType.vtString) Name = vl_arguments["Name"].AsString;
            else res.ParamValidations["Name"].AsString = "Value not recognized";

            int Step = 0;
            if (vl_arguments["Step"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Step"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Step"].ValueType == TVariantType.vtInt64) Step = vl_arguments["Step"].AsInt32;
            else res.ParamValidations["Step"].AsString = "Value not recognized";

            bool isActive = false;
            if (vl_arguments["isActive"].ValueType == TVariantType.vtBoolean) isActive = vl_arguments["isActive"].AsBoolean;
            else res.ParamValidations["isActive"].AsString = "Value not recognized";

            string Template = "";
            if (vl_arguments["Template"].ValueType == TVariantType.vtString) Template = vl_arguments["Template"].AsString;
            else res.ParamValidations["Template"].AsString = "Value not recognized";

            string ExportTemplate = "";
            if (vl_arguments["ExportTemplate"] != null)
            {
                if (vl_arguments["ExportTemplate"].ValueType == TVariantType.vtString) ExportTemplate = vl_arguments["ExportTemplate"].AsString;
                else res.ParamValidations["ExportTemplate"].AsString = "Value not recognized";
            }

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs = new TVariantList();

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = Name;
                vl_params.Add("@prm_Step").AsInt32 = Step;
                vl_params.Add("@prm_isActive").AsBoolean = isActive;
                vl_params.Add("@prm_Template").AsString = Template;
                vl_params.Add("@prm_ExportTemplate").AsString = ExportTemplate;
                vl_params.Add("@prm_Status").AsString = "pending";
                int int_rows = app.DB.Exec("insert_Form", "Forms", vl_params);
                if (int_rows <= 0) throw new Exception("SQL execution error");

                int ID_Form = app.DB.GetIdentity();

                app.DB.CommitTransaction();

                res.RowsModified = int_rows;
                res.IDs.Add("ID_Form").AsInt32 = ID_Form;

                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult editForm(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Form"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Form");
            if (vl_arguments["Name"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name");
            if (vl_arguments["Step"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Step");
            if (vl_arguments["isActive"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "isActive");
            //if (vl_arguments["Template"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Template");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Form = 0;
            if (vl_arguments["ID_Form"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Form"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Form"].ValueType == TVariantType.vtInt64) ID_Form = vl_arguments["ID_Form"].AsInt32;
            else res.ParamValidations["ID_Form"].AsString = "Value not recognized";

            string Name = "";
            if (vl_arguments["Name"].ValueType == TVariantType.vtString) Name = vl_arguments["Name"].AsString;
            else res.ParamValidations["Name"].AsString = "Value not recognized";

            int Step = 0;
            if (vl_arguments["Step"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["Step"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["Step"].ValueType == TVariantType.vtInt64) Step = vl_arguments["Step"].AsInt32;
            else res.ParamValidations["Step"].AsString = "Value not recognized";

            bool isActive = false;
            if (vl_arguments["isActive"].ValueType == TVariantType.vtBoolean) isActive = vl_arguments["isActive"].AsBoolean;
            else res.ParamValidations["isActive"].AsString = "Value not recognized";

            string Template = "";
            if (vl_arguments["Template"] != null)
            {
                if (vl_arguments["Template"].ValueType == TVariantType.vtString) Template = vl_arguments["Template"].AsString;
                else res.ParamValidations["Template"].AsString = "Value not recognized";
            }

            string ExportTemplate = "";
            if (vl_arguments["ExportTemplate"] != null)
            {
                if (vl_arguments["ExportTemplate"].ValueType == TVariantType.vtString) ExportTemplate = vl_arguments["ExportTemplate"].AsString;
                else res.ParamValidations["ExportTemplate"].AsString = "Value not recognized";
            }

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_Form;
                vl_params.Add("@prm_Name").AsString = Name;
                vl_params.Add("@prm_Step").AsInt32 = Step;
                vl_params.Add("@prm_isActive").AsBoolean = isActive;
                //vl_params.Add("@prm_Template").AsString = Template;
                //vl_params.Add("@prm_Status").AsString = "pending";
                int int_rows = app.DB.Exec("update_Form", "Forms", vl_params);
                if (int_rows <= 0) throw new Exception("SQL execution error");

                if (Template != "")
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID").AsInt32 = ID_Form;
                    vl_params.Add("@prm_Template").AsString = Template;
                    vl_params.Add("@prm_Status").AsString = "pending";
                    int_rows = app.DB.Exec("update_FormTemplate", "Forms", vl_params);
                    if (int_rows <= 0) throw new Exception("SQL execution error");
                }

                if (ExportTemplate != "")
                {
                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID").AsInt32 = ID_Form;
                    vl_params.Add("@prm_ExportTemplate").AsString = ExportTemplate;
                    int_rows = app.DB.Exec("update_ExportTemplate", "Forms", vl_params);
                    if (int_rows <= 0) throw new Exception("SQL execution error");
                }

                app.DB.CommitTransaction();
                res.RowsModified = int_rows;

                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult deleteForm(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Form"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Form");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_Form = 0;
            if (vl_arguments["ID_Form"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Form"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Form"].ValueType == TVariantType.vtInt64) ID_Form = vl_arguments["ID_Form"].AsInt32;
            else res.ParamValidations["ID_Form"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_Form;
                int int_rows = app.DB.Exec("delete_Form", "Forms", vl_params);
                if (int_rows <= 0) throw new Exception("SQL execution error");

                app.DB.CommitTransaction();
                res.RowsModified = int_rows;

                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult setFormData(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Procedure"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Procedure");

            int ID_Procedure = 0;
            if (vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_Procedure"].ValueType == TVariantType.vtInt64) ID_Procedure = vl_arguments["ID_Procedure"].AsInt32;
            else return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "ID_Procedure");

            for (int i = 0; i < vl_arguments.Count; i++)
            {
                TVariant var = vl_arguments[i];

                if (var.Name.StartsWith("@"))
                {
                    string Key = var.Name;
                    string Value = var.AsString;

                    TVariantList vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_Procedure").AsInt32 = ID_Procedure;
                    vl_params.Add("@prm_Key").AsString = Key;
                    vl_params.Add("@prm_Value").AsString = Value;
                    app.DB.Select("SetProcedureVariable", "Procedures", vl_params);
                }
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult addProcedureType(TVariantList vl_arguments)
        {
            if (vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_RO");
            if (vl_arguments["Name_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_EN");
            if (vl_arguments["Description_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Description_RO");
            if (vl_arguments["Description_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Description_EN");
            if (vl_arguments["ValueThreshold"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ValueThreshold");
            if (vl_arguments["ClarificationRequestsOffset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ClarificationRequestsOffset");
            if (vl_arguments["TendersReceiptOffset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TendersReceiptOffset");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            string Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string Description_RO = "";
            if (vl_arguments["Description_RO"] != null)
            {
                if (vl_arguments["Description_RO"].ValueType == TVariantType.vtString) Description_RO = vl_arguments["Description_RO"].AsString;
                else res.ParamValidations["Description_RO"].AsString = "Value type not recognized";
            }

            string Description_EN = "";
            if (vl_arguments["Description_EN"] != null)
            {
                if (vl_arguments["Description_EN"].ValueType == TVariantType.vtString) Description_EN = vl_arguments["Description_EN"].AsString;
                else res.ParamValidations["Description_EN"].AsString = "Value type not recognized";
            }

            double ValueThreshold = 0;
            if (vl_arguments["ValueThreshold"] != null)
            {
                if (vl_arguments["ValueThreshold"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ValueThreshold"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ValueThreshold"].ValueType == TVariantType.vtInt64 ||
                    vl_arguments["ValueThreshold"].ValueType == TVariantType.vtSingle ||
                    vl_arguments["ValueThreshold"].ValueType == TVariantType.vtDouble) ValueThreshold = vl_arguments["ValueThreshold"].AsDouble;
                else res.ParamValidations["ValueThreshold"].AsString = "Value not recognized";
            }

            double ClarificationRequestsOffset = 0;
            if (vl_arguments["ClarificationRequestsOffset"] != null)
            {
                if (vl_arguments["ClarificationRequestsOffset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ClarificationRequestsOffset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ClarificationRequestsOffset"].ValueType == TVariantType.vtInt64 ||
                    vl_arguments["ClarificationRequestsOffset"].ValueType == TVariantType.vtSingle ||
                    vl_arguments["ClarificationRequestsOffset"].ValueType == TVariantType.vtDouble) ClarificationRequestsOffset = vl_arguments["ClarificationRequestsOffset"].AsDouble;
                else res.ParamValidations["ClarificationRequestsOffset"].AsString = "Value not recognized";
            }

            double TendersReceiptOffset = 0;
            if (vl_arguments["TendersReceiptOffset"] != null)
            {
                if (vl_arguments["TendersReceiptOffset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["TendersReceiptOffset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["TendersReceiptOffset"].ValueType == TVariantType.vtInt64 ||
                    vl_arguments["TendersReceiptOffset"].ValueType == TVariantType.vtSingle ||
                    vl_arguments["TendersReceiptOffset"].ValueType == TVariantType.vtDouble) TendersReceiptOffset = vl_arguments["TendersReceiptOffset"].AsDouble;
                else res.ParamValidations["TendersReceiptOffset"].AsString = "Value not recognized";
            }

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = Name_RO;
                vl_params.Add("@prm_Description").AsString = Description_RO;
                vl_params.Add("@prm_ValueThreshold").AsDouble = ValueThreshold;
                vl_params.Add("@prm_ClarificationRequestsOffset").AsDouble = ClarificationRequestsOffset;
                vl_params.Add("@prm_TendersReceiptOffset").AsDouble = TendersReceiptOffset;

                int int_count_ProcedureType = app.DB.Exec("insert_ProcedureType", "ProcedureTypes", vl_params);
                if (int_count_ProcedureType == -1) throw new Exception("Procedure Type insert failed");

                int ID_ProcedureType = 0;
                DataSet ds_ProcedureType = app.DB.Select("select_LastProcedureType", "ProcedureTypes", null);
                if (app.DB.ValidDSRows(ds_ProcedureType)) ID_ProcedureType = Convert.ToInt32(ds_ProcedureType.Tables[0].Rows[0]["ID"]);

                string str_Name = "fld_ProcedureTypes_Name_" + ID_ProcedureType.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);

                string str_Description = "fld_ProcedureTypes_Description_" + ID_ProcedureType.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Description;
                vl_params.Add("@prm_Value_EN").AsString = Description_EN;
                vl_params.Add("@prm_Value_RO").AsString = Description_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);

                vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID").AsInt32 = ID_ProcedureType;
                app.DB.Exec("update_TextFields", "ProcedureTypes", vl_params);

                app.DB.CommitTransaction();

                res.RowsModified = int_count_ProcedureType;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult editProcedureType(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_ProcedureType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_ProcedureType");
            if (vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_RO");
            if (vl_arguments["Name_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_EN");
            if (vl_arguments["Description_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Description_RO");
            if (vl_arguments["Description_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Description_EN");
            if (vl_arguments["ValueThreshold"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ValueThreshold");
            if (vl_arguments["ClarificationRequestsOffset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ClarificationRequestsOffset");
            if (vl_arguments["TendersReceiptOffset"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TendersReceiptOffset");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_ProcedureType = 0;
            if (vl_arguments["ID_ProcedureType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_ProcedureType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_ProcedureType"].ValueType == TVariantType.vtInt64) ID_ProcedureType = vl_arguments["ID_ProcedureType"].AsInt32;
            else return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "ID_ProcedureType");

            string Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string Description_RO = "";
            if (vl_arguments["Description_RO"] != null)
            {
                if (vl_arguments["Description_RO"].ValueType == TVariantType.vtString) Description_RO = vl_arguments["Description_RO"].AsString;
                else res.ParamValidations["Description_RO"].AsString = "Value type not recognized";
            }

            string Description_EN = "";
            if (vl_arguments["Description_EN"] != null)
            {
                if (vl_arguments["Description_EN"].ValueType == TVariantType.vtString) Description_EN = vl_arguments["Description_EN"].AsString;
                else res.ParamValidations["Description_EN"].AsString = "Value type not recognized";
            }

            double ValueThreshold = 0;
            if (vl_arguments["ValueThreshold"] != null)
            {
                if (vl_arguments["ValueThreshold"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ValueThreshold"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ValueThreshold"].ValueType == TVariantType.vtInt64 ||
                    vl_arguments["ValueThreshold"].ValueType == TVariantType.vtSingle ||
                    vl_arguments["ValueThreshold"].ValueType == TVariantType.vtDouble) ValueThreshold = vl_arguments["ValueThreshold"].AsDouble;
                else res.ParamValidations["ValueThreshold"].AsString = "Value not recognized";
            }

            double ClarificationRequestsOffset = 0;
            if (vl_arguments["ClarificationRequestsOffset"] != null)
            {
                if (vl_arguments["ClarificationRequestsOffset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ClarificationRequestsOffset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ClarificationRequestsOffset"].ValueType == TVariantType.vtInt64 ||
                    vl_arguments["ClarificationRequestsOffset"].ValueType == TVariantType.vtSingle ||
                    vl_arguments["ClarificationRequestsOffset"].ValueType == TVariantType.vtDouble) ClarificationRequestsOffset = vl_arguments["ClarificationRequestsOffset"].AsDouble;
                else res.ParamValidations["ClarificationRequestsOffset"].AsString = "Value not recognized";
            }

            double TendersReceiptOffset = 0;
            if (vl_arguments["TendersReceiptOffset"] != null)
            {
                if (vl_arguments["TendersReceiptOffset"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["TendersReceiptOffset"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["TendersReceiptOffset"].ValueType == TVariantType.vtInt64 ||
                    vl_arguments["TendersReceiptOffset"].ValueType == TVariantType.vtSingle ||
                    vl_arguments["TendersReceiptOffset"].ValueType == TVariantType.vtDouble) TendersReceiptOffset = vl_arguments["TendersReceiptOffset"].AsDouble;
                else res.ParamValidations["TendersReceiptOffset"].AsString = "Value not recognized";
            }

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {

                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = Name_RO;
                vl_params.Add("@prm_Description").AsString = Description_RO;
                vl_params.Add("@prm_ValueThreshold").AsDouble = ValueThreshold;
                vl_params.Add("@prm_ClarificationRequestsOffset").AsDouble = ClarificationRequestsOffset;
                vl_params.Add("@prm_TendersReceiptOffset").AsDouble = TendersReceiptOffset;
                vl_params.Add("@prm_ID").AsInt32 = ID_ProcedureType;

                int int_count_ProcedureType = app.DB.Exec("update_ProcedureType", "ProcedureTypes", vl_params);
                if (int_count_ProcedureType == -1) throw new Exception("Procedure Type insert failed");

                string str_Name = "fld_ProcedureTypes_Name_" + ID_ProcedureType.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);

                string str_Description = "fld_ProcedureTypes_Description_" + ID_ProcedureType.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Description;
                vl_params.Add("@prm_Value_EN").AsString = Description_EN;
                vl_params.Add("@prm_Value_RO").AsString = Description_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);

                vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID").AsInt32 = ID_ProcedureType;
                app.DB.Exec("update_TextFields", "ProcedureTypes", vl_params);

                app.DB.CommitTransaction();

                res.RowsModified = int_count_ProcedureType;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult deleteProcedureType(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_ProcedureType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_ProcedureType");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_ProcedureType = 0;
            if (vl_arguments["ID_ProcedureType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_ProcedureType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_ProcedureType"].ValueType == TVariantType.vtInt64) ID_ProcedureType = vl_arguments["ID_ProcedureType"].AsInt32;
            else res.ParamValidations["ID_ProcedureType"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_ProcedureType;
                int int_rows = app.DB.Exec("delete_ProcedureType", "ProcedureTypes", vl_params);
                if (int_rows <= 0) throw new Exception("SQL execution error");

                app.DB.CommitTransaction();
                res.RowsModified = int_rows;

                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult addContractType(TVariantList vl_arguments)
        {
            if (vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_RO");
            if (vl_arguments["Name_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_EN");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            string Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }


            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = Name_RO;

                int int_count_ContractType = app.DB.Exec("insert_ContractType", "ContractTypes", vl_params);
                if (int_count_ContractType == -1) throw new Exception("Contract Type insert failed");

                int ID_ContractType = 0;
                DataSet ds_ContractType = app.DB.Select("select_LastContractType", "ContractTypes", null);
                if (app.DB.ValidDSRows(ds_ContractType)) ID_ContractType = Convert.ToInt32(ds_ContractType.Tables[0].Rows[0]["ID"]);

                string str_Name = "fld_ContractTypes_Name_" + ID_ContractType.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);

                vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_ID").AsInt32 = ID_ContractType;
                app.DB.Exec("update_TextFields", "ContractTypes", vl_params);

                app.DB.CommitTransaction();

                res.RowsModified = int_count_ContractType;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult editContractType(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_ContractType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_ContractType");
            if (vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_RO");
            if (vl_arguments["Name_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_EN");


            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_ContractType = 0;
            if (vl_arguments["ID_ContractType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_ContractType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_ContractType"].ValueType == TVariantType.vtInt64) ID_ContractType = vl_arguments["ID_ContractType"].AsInt32;
            else return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "ID_ContractType");

            string Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {

                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = Name_RO;
                vl_params.Add("@prm_ID").AsInt32 = ID_ContractType;

                int int_count_ContractType = app.DB.Exec("update_ContractType", "ContractTypes", vl_params);
                if (int_count_ContractType == -1) throw new Exception("Contract Type insert failed");

                string str_Name = "fld_ContractTypes_Name_" + ID_ContractType.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);

                vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_ID").AsInt32 = ID_ContractType;
                app.DB.Exec("update_TextFields", "ContractTypes", vl_params);

                app.DB.CommitTransaction();

                res.RowsModified = int_count_ContractType;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult deleteContractType(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_ContractType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_ContractType");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_ContractType = 0;
            if (vl_arguments["ID_ContractType"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_ContractType"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_ContractType"].ValueType == TVariantType.vtInt64) ID_ContractType = vl_arguments["ID_ContractType"].AsInt32;
            else res.ParamValidations["ID_ContractType"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_ContractType;
                int int_rows = app.DB.Exec("delete_ContractType", "ContractTypes", vl_params);
                if (int_rows <= 0) throw new Exception("SQL execution error");

                app.DB.CommitTransaction();
                res.RowsModified = int_rows;

                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult addProcedureLegislation(TVariantList vl_arguments)
        {
            if (vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_RO");
            if (vl_arguments["Name_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_EN");
            if (vl_arguments["Description_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Description_RO");
            if (vl_arguments["Description_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Description_EN");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            string Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string Description_RO = "";
            if (vl_arguments["Description_RO"] != null)
            {
                if (vl_arguments["Description_RO"].ValueType == TVariantType.vtString) Description_RO = vl_arguments["Description_RO"].AsString;
                else res.ParamValidations["Description_RO"].AsString = "Value type not recognized";
            }

            string Description_EN = "";
            if (vl_arguments["Description_EN"] != null)
            {
                if (vl_arguments["Description_EN"].ValueType == TVariantType.vtString) Description_EN = vl_arguments["Description_EN"].AsString;
                else res.ParamValidations["Description_EN"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = Name_RO;
                vl_params.Add("@prm_Description").AsString = Description_RO;

                int int_count_ProcedureLegislation = app.DB.Exec("insert_ProcedureLegislation", "ProcedureLegislations", vl_params);
                if (int_count_ProcedureLegislation == -1) throw new Exception("Procedure Legislation insert failed");

                int ID_ProcedureLegislation = 0;
                DataSet ds_ProcedureLegislation = app.DB.Select("select_LastProcedureLegislation", "ProcedureLegislations", null);
                if (app.DB.ValidDSRows(ds_ProcedureLegislation)) ID_ProcedureLegislation = Convert.ToInt32(ds_ProcedureLegislation.Tables[0].Rows[0]["ID"]);

                string str_Name = "fld_ProcedureLegislations_Name_" + ID_ProcedureLegislation.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);

                string str_Description = "fld_ProcedureLegislations_Description_" + ID_ProcedureLegislation.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Description;
                vl_params.Add("@prm_Value_EN").AsString = Description_EN;
                vl_params.Add("@prm_Value_RO").AsString = Description_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);

                vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID").AsInt32 = ID_ProcedureLegislation;
                app.DB.Exec("update_TextFields", "ProcedureLegislations", vl_params);

                app.DB.CommitTransaction();

                res.RowsModified = int_count_ProcedureLegislation;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult editProcedureLegislation(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_ProcedureLegislation"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_ProcedureLegislation");
            if (vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_RO");
            if (vl_arguments["Name_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_EN");
            if (vl_arguments["Description_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Description_RO");
            if (vl_arguments["Description_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Description_EN");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_ProcedureLegislation = 0;
            if (vl_arguments["ID_ProcedureLegislation"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_ProcedureLegislation"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_ProcedureLegislation"].ValueType == TVariantType.vtInt64) ID_ProcedureLegislation = vl_arguments["ID_ProcedureLegislation"].AsInt32;
            else return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "ID_ProcedureLegislation");

            string Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string Description_RO = "";
            if (vl_arguments["Description_RO"] != null)
            {
                if (vl_arguments["Description_RO"].ValueType == TVariantType.vtString) Description_RO = vl_arguments["Description_RO"].AsString;
                else res.ParamValidations["Description_RO"].AsString = "Value type not recognized";
            }

            string Description_EN = "";
            if (vl_arguments["Description_EN"] != null)
            {
                if (vl_arguments["Description_EN"].ValueType == TVariantType.vtString) Description_EN = vl_arguments["Description_EN"].AsString;
                else res.ParamValidations["Description_EN"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {

                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = Name_RO;
                vl_params.Add("@prm_Description").AsString = Description_RO;
                vl_params.Add("@prm_ID").AsInt32 = ID_ProcedureLegislation;

                int int_count_ProcedureLegislation = app.DB.Exec("update_ProcedureLegislation", "ProcedureLegislations", vl_params);
                if (int_count_ProcedureLegislation == -1) throw new Exception("Procedure Legislation insert failed");

                string str_Name = "fld_ProcedureLegislations_Name_" + ID_ProcedureLegislation.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);

                string str_Description = "fld_ProcedureLegislations_Description_" + ID_ProcedureLegislation.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Description;
                vl_params.Add("@prm_Value_EN").AsString = Description_EN;
                vl_params.Add("@prm_Value_RO").AsString = Description_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);

                vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID").AsInt32 = ID_ProcedureLegislation;
                app.DB.Exec("update_TextFields", "ProcedureLegislations", vl_params);

                app.DB.CommitTransaction();

                res.RowsModified = int_count_ProcedureLegislation;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult deleteProcedureLegislation(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_ProcedureLegislation"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_ProcedureLegislation");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_ProcedureLegislation = 0;
            if (vl_arguments["ID_ProcedureLegislation"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_ProcedureLegislation"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_ProcedureLegislation"].ValueType == TVariantType.vtInt64) ID_ProcedureLegislation = vl_arguments["ID_ProcedureLegislation"].AsInt32;
            else res.ParamValidations["ID_ProcedureLegislation"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_ProcedureLegislation;
                int int_rows = app.DB.Exec("delete_ProcedureLegislation", "ProcedureLegislations", vl_params);
                if (int_rows <= 0) throw new Exception("SQL execution error");

                app.DB.CommitTransaction();
                res.RowsModified = int_rows;

                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult addProcedureCriteria(TVariantList vl_arguments)
        {
            if (vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_RO");
            if (vl_arguments["Name_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_EN");
            if (vl_arguments["Description_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Description_RO");
            if (vl_arguments["Description_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Description_EN");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            string Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string Description_RO = "";
            if (vl_arguments["Description_RO"] != null)
            {
                if (vl_arguments["Description_RO"].ValueType == TVariantType.vtString) Description_RO = vl_arguments["Description_RO"].AsString;
                else res.ParamValidations["Description_RO"].AsString = "Value type not recognized";
            }

            string Description_EN = "";
            if (vl_arguments["Description_EN"] != null)
            {
                if (vl_arguments["Description_EN"].ValueType == TVariantType.vtString) Description_EN = vl_arguments["Description_EN"].AsString;
                else res.ParamValidations["Description_EN"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = Name_RO;
                vl_params.Add("@prm_Description").AsString = Description_RO;

                int int_count_ProcedureCriteria = app.DB.Exec("insert_ProcedureCriteria", "ProcedureCriteria", vl_params);
                if (int_count_ProcedureCriteria == -1) throw new Exception("Procedure Criteria insert failed");

                int ID_ProcedureCriteria = 0;
                DataSet ds_ProcedureLegislation = app.DB.Select("select_LastProcedureCriteria", "ProcedureCriteria", null);
                if (app.DB.ValidDSRows(ds_ProcedureLegislation)) ID_ProcedureCriteria = Convert.ToInt32(ds_ProcedureLegislation.Tables[0].Rows[0]["ID"]);

                string str_Name = "fld_ProcedureCriteria_Name_" + ID_ProcedureCriteria.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);

                string str_Description = "fld_ProcedureCriteria_Description_" + ID_ProcedureCriteria.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Description;
                vl_params.Add("@prm_Value_EN").AsString = Description_EN;
                vl_params.Add("@prm_Value_RO").AsString = Description_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);

                vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID").AsInt32 = ID_ProcedureCriteria;
                app.DB.Exec("update_TextFields", "ProcedureCriteria", vl_params);

                app.DB.CommitTransaction();

                res.RowsModified = int_count_ProcedureCriteria;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult editProcedureCriteria(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_ProcedureCriteria"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_ProcedureCriteria");
            if (vl_arguments["Name_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_RO");
            if (vl_arguments["Name_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Name_EN");
            if (vl_arguments["Description_RO"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Description_RO");
            if (vl_arguments["Description_EN"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Description_EN");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_ProcedureCriteria = 0;
            if (vl_arguments["ID_ProcedureCriteria"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_ProcedureCriteria"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_ProcedureCriteria"].ValueType == TVariantType.vtInt64) ID_ProcedureCriteria = vl_arguments["ID_ProcedureCriteria"].AsInt32;
            else return new TDBExecResult(TDBExecError.ValidationUnsuccesful, "ID_ProcedureCriteria");

            string Name_RO = "";
            if (vl_arguments["Name_RO"] != null)
            {
                if (vl_arguments["Name_RO"].ValueType == TVariantType.vtString) Name_RO = vl_arguments["Name_RO"].AsString;
                else res.ParamValidations["Name_RO"].AsString = "Value type not recognized";
            }

            string Name_EN = "";
            if (vl_arguments["Name_EN"] != null)
            {
                if (vl_arguments["Name_EN"].ValueType == TVariantType.vtString) Name_EN = vl_arguments["Name_EN"].AsString;
                else res.ParamValidations["Name_EN"].AsString = "Value type not recognized";
            }

            string Description_RO = "";
            if (vl_arguments["Description_RO"] != null)
            {
                if (vl_arguments["Description_RO"].ValueType == TVariantType.vtString) Description_RO = vl_arguments["Description_RO"].AsString;
                else res.ParamValidations["Description_RO"].AsString = "Value type not recognized";
            }

            string Description_EN = "";
            if (vl_arguments["Description_EN"] != null)
            {
                if (vl_arguments["Description_EN"].ValueType == TVariantType.vtString) Description_EN = vl_arguments["Description_EN"].AsString;
                else res.ParamValidations["Description_EN"].AsString = "Value type not recognized";
            }

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {

                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = Name_RO;
                vl_params.Add("@prm_Description").AsString = Description_RO;
                vl_params.Add("@prm_ID").AsInt32 = ID_ProcedureCriteria;

                int int_count_ProcedureLegislation = app.DB.Exec("update_ProcedureCriteria", "ProcedureCriteria", vl_params);
                if (int_count_ProcedureLegislation == -1) throw new Exception("Procedure Criteria insert failed");

                string str_Name = "fld_ProcedureCriteria_Name_" + ID_ProcedureCriteria.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Name;
                vl_params.Add("@prm_Value_EN").AsString = Name_EN;
                vl_params.Add("@prm_Value_RO").AsString = Name_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);

                string str_Description = "fld_ProcedureCriteria_Description_" + ID_ProcedureCriteria.ToString();
                vl_params = new TVariantList();
                vl_params.Add("@prm_Label").AsString = str_Description;
                vl_params.Add("@prm_Value_EN").AsString = Description_EN;
                vl_params.Add("@prm_Value_RO").AsString = Description_RO;
                app.DB.Exec("setTranslation", "Translations", vl_params);

                vl_params = new TVariantList();
                vl_params.Add("@prm_Name").AsString = str_Name;
                vl_params.Add("@prm_Description").AsString = str_Description;
                vl_params.Add("@prm_ID").AsInt32 = ID_ProcedureCriteria;
                app.DB.Exec("update_TextFields", "ProcedureCriteria", vl_params);

                app.DB.CommitTransaction();

                res.RowsModified = int_count_ProcedureLegislation;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public TDBExecResult deleteProcedureCriteria(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_ProcedureCriteria"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_ProcedureCriteria");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_ProcedureCriteria = 0;
            if (vl_arguments["ID_ProcedureCriteria"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["ID_ProcedureCriteria"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["ID_ProcedureCriteria"].ValueType == TVariantType.vtInt64) ID_ProcedureCriteria = vl_arguments["ID_ProcedureCriteria"].AsInt32;
            else res.ParamValidations["ID_ProcedureCriteria"].AsString = "Value not recognized";

            if (res.bInvalid()) return res;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.SQLExecutionError, "Transaction Failed");

            try
            {
                res = new TDBExecResult(TDBExecError.Success, "");

                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_ProcedureCriteria;
                int int_rows = app.DB.Exec("delete_ProcedureCriteria", "ProcedureCriteria", vl_params);
                if (int_rows <= 0) throw new Exception("SQL execution error");

                app.DB.CommitTransaction();
                res.RowsModified = int_rows;

                return new TDBExecResult(TDBExecError.Success, "", int_rows);
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "Queries on database failed");
            }
        }

        public int addREMITDataSource(TVariantList vl_arguments)
        {
            if (vl_arguments["DataSourceName"] == null) return -1;
            if (vl_arguments["DataSourceType"] == null) return -1;
            if (vl_arguments["isActive"] == null) return -1;

            string DataSourceName = vl_arguments["DataSourceName"].AsString;
            string DataSourceType = vl_arguments["DataSourceType"].AsString;
            bool isActive = vl_arguments["isActive"].AsBoolean;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_DataSourceName").AsString = DataSourceName;
            vl_params.Add("@prm_DataSourceType").AsString = DataSourceType;
            vl_params.Add("@prm_isActive").AsBoolean = isActive;

            return app.DB.Exec("insert_REMIT_DataSource", "REMIT_DataSources", vl_params);
        }

        public int editREMITDataSource(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_DataSource"] == null) return -1;
            if (vl_arguments["DataSourceName"] == null) return -1;
            if (vl_arguments["DataSourceType"] == null) return -1;
            if (vl_arguments["isActive"] == null) return -1;

            int ID_DataSource = vl_arguments["ID_DataSource"].AsInt32;
            string DataSourceName = vl_arguments["DataSourceName"].AsString;
            string DataSourceType = vl_arguments["DataSourceType"].AsString;
            bool isActive = vl_arguments["isActive"].AsBoolean;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_DataSource;
            vl_params.Add("@prm_DataSourceName").AsString = DataSourceName;
            vl_params.Add("@prm_DataSourceType").AsString = DataSourceType;
            vl_params.Add("@prm_isActive").AsBoolean = isActive;

            return app.DB.Exec("update_REMIT_DataSource", "REMIT_DataSources", vl_params);
        }

        public int deleteREMITDataSource(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_DataSource"] == null) return -1;
            int ID_DataSource = vl_arguments["ID_DataSource"].AsInt32;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_DataSource;
            return app.DB.Exec("delete_REMIT_DataSource", "REMIT_DataSources", vl_params);
        }

        public int addREMITContractName(TVariantList vl_arguments)
        {
            if (vl_arguments["Name"] == null) return -1;
            string Name = vl_arguments["Name"].AsString;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Name").AsString = Name;
            return app.DB.Exec("insert_REMIT_ContractName", "REMIT_ContractNames", vl_params);
        }

        public int editREMITContractName(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_ContractName"] == null) return -1;
            if (vl_arguments["Name"] == null) return -1;

            int ID_ContractName = vl_arguments["ID_ContractName"].AsInt32;
            string Name = vl_arguments["Name"].AsString;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_ContractName;
            vl_params.Add("@prm_Name").AsString = Name;
            return app.DB.Exec("update_REMIT_ContractName", "REMIT_ContractNames", vl_params);
        }

        public int deleteREMITContractName(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_ContractName"] == null) return -1;

            int ID_ContractName = vl_arguments["ID_ContractName"].AsInt32;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_ContractName;
            return app.DB.Exec("delete_REMIT_ContractName", "REMIT_ContractNames", vl_params);
        }

        public int addREMITContractType(TVariantList vl_arguments)
        {
            if (vl_arguments["Code"] == null) return -1;
            if (vl_arguments["Name"] == null) return -1;
            if (vl_arguments["enableTable1"] == null) return -1;
            if (vl_arguments["enableTable2"] == null) return -1;

            string Code = vl_arguments["Code"].AsString;
            string Name = vl_arguments["Name"].AsString;
            bool enableTable1 = vl_arguments["enableTable1"].AsBoolean;
            bool enableTable2 = vl_arguments["enableTable2"].AsBoolean;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Code").AsString = Code;
            vl_params.Add("@prm_Name").AsString = Name;
            vl_params.Add("@prm_enableTable1").AsBoolean = enableTable1;
            vl_params.Add("@prm_enableTable2").AsBoolean = enableTable2;
            return app.DB.Exec("insert_REMIT_ContractType", "REMIT_ContractTypes", vl_params);
        }

        public int editREMITContractType(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_ContractType"] == null) return -1;
            if (vl_arguments["Code"] == null) return -1;
            if (vl_arguments["Name"] == null) return -1;
            if (vl_arguments["enableTable1"] == null) return -1;
            if (vl_arguments["enableTable2"] == null) return -1;

            int ID_ContractType = vl_arguments["ID_ContractType"].AsInt32;
            string Code = vl_arguments["Code"].AsString;
            string Name = vl_arguments["Name"].AsString;
            bool enableTable1 = vl_arguments["enableTable1"].AsBoolean;
            bool enableTable2 = vl_arguments["enableTable2"].AsBoolean;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_ContractType;
            vl_params.Add("@prm_Code").AsString = Code;
            vl_params.Add("@prm_Name").AsString = Name;
            vl_params.Add("@prm_enableTable1").AsBoolean = enableTable1;
            vl_params.Add("@prm_enableTable2").AsBoolean = enableTable2;
            return app.DB.Exec("update_REMIT_ContractType", "REMIT_ContractTypes", vl_params);
        }

        public int deleteREMITContractType(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_ContractType"] == null) return -1;

            int ID_ContractType = vl_arguments["ID_ContractType"].AsInt32;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_ContractType;
            return app.DB.Exec("delete_REMIT_ContractType", "REMIT_ContractTypes", vl_params);
        }

        public int addREMITLoadType(TVariantList vl_arguments)
        {
            if (vl_arguments["Code"] == null) return -1;
            if (vl_arguments["Name"] == null) return -1;

            string Code = vl_arguments["Code"].AsString;
            string Name = vl_arguments["Name"].AsString;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_Code").AsString = Code;
            vl_params.Add("@prm_Name").AsString = Name;
            return app.DB.Exec("insert_REMIT_LoadType", "REMIT_LoadTypes", vl_params);
        }

        public int editREMITLoadType(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_LoadType"] == null) return -1;
            if (vl_arguments["Code"] == null) return -1;
            if (vl_arguments["Name"] == null) return -1;

            int ID_LoadType = vl_arguments["ID_LoadType"].AsInt32;
            string Code = vl_arguments["Code"].AsString;
            string Name = vl_arguments["Name"].AsString;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_LoadType;
            vl_params.Add("@prm_Code").AsString = Code;
            vl_params.Add("@prm_Name").AsString = Name;
            return app.DB.Exec("update_REMIT_LoadType", "REMIT_LoadTypes", vl_params);
        }

        public int deleteREMITLoadType(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_LoadType"] == null) return -1;

            int ID_LoadType = vl_arguments["ID_LoadType"].AsInt32;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_LoadType;
            return app.DB.Exec("delete_REMIT_LoadType", "REMIT_LoadTypes", vl_params);
        }

        public int uploadREMITFile(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_DataSource"] == null) return -1;
            if (vl_arguments["FileName"] == null) return -1;
            if (vl_arguments["FileContent"] == null) return -1;

            int ID_DataSource = vl_arguments["ID_DataSource"].AsInt32;
            string FileName = vl_arguments["FileName"].AsString;
            string FileContent = vl_arguments["FileContent"].AsString;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID_REMIT_DataSource").AsInt32 = ID_DataSource;
            vl_params.Add("@prm_InputDataName").AsString = FileName;
            vl_params.Add("@prm_InputData").AsString = FileContent;

            return app.DB.Exec("insert_REMIT_DataSourceHistory", "REMIT_DataSourceHistory", vl_params);
        }

        public int uploadREMITReceipt(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_DataSourceHistory"] == null) return -1;
            if (vl_arguments["FileName"] == null) return -1;
            if (vl_arguments["FileContent"] == null) return -1;

            int ID_DataSourceHistory = vl_arguments["ID_DataSourceHistory"].AsInt32;
            string FileName = vl_arguments["FileName"].AsString;
            string FileContent = vl_arguments["FileContent"].AsString;

            TVariantList vl_params = new TVariantList();
            vl_params.Add("@prm_ID").AsInt32 = ID_DataSourceHistory;
            vl_params.Add("@prm_ReceiptDataName").AsString = FileName;
            vl_params.Add("@prm_ReceiptData").AsString = FileContent;

            return app.DB.Exec("update_REMIT_ReceiptFile", "REMIT_DataSourceHistory", vl_params);
        }

        public TDBExecResult addREMITTable1Report(TVariantList vl_arguments)
        {
            //if (vl_arguments["ContractID"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ContractID");
            if (vl_arguments["ID_ContractName"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_ContractName");
            if (vl_arguments["ID_ContractType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_ContractType");
            if (vl_arguments["SettlementMethod"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing SettlementMethod");
            if (vl_arguments["DeliveryPoint"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing DeliveryPoint");
            if (vl_arguments["ParticipantIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ParticipantIdentifier");
            if (vl_arguments["ParticipantIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ParticipantIdentifierType");
            //if (vl_arguments["ParticipantMktID"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ParticipantMktID");
            if (vl_arguments["CounterpartIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing CounterpartIdentifier");
            if (vl_arguments["CounterpartIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing CounterpartIdentifierType");
            //if (vl_arguments["CounterpartMktID"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing CounterpartMktID");
            if (vl_arguments["TransactionID"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing TransactionID");
            if (vl_arguments["TransactionTimestamp"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing TransactionTimestamp");
            if (vl_arguments["Price"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing Price");
            if (vl_arguments["ID_Currency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_Currency");
            //if (vl_arguments["PriceFormula"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing PriceFormula");
            if (vl_arguments["NotionalAmount"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing NotionalAmount");
            if (vl_arguments["ID_NotionalCurrency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_NotionalCurrency");
            if (vl_arguments["Volume"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing Volume");
            if (vl_arguments["VolumeMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing VolumeMU");
            if (vl_arguments["TotalNotionalQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing TotalNotionalQuantity");
            if (vl_arguments["TotalNotionalQuantityMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing TotalNotionalQuantityMU");
            if (vl_arguments["DeliveryStartDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing DeliveryStartDate");
            if (vl_arguments["DeliveryEndDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing DeliveryEndDate");
            if (vl_arguments["ID_LoadType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_LoadType");
            if (vl_arguments["ActionType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ActionType");

            string ContractID = "";
            //ContractID = vl_arguments["ContractID"].AsString;

            int ID_ContractName = 0;
            ID_ContractName = vl_arguments["ID_ContractName"].AsInt32;

            int ID_ContractType = 0;
            ID_ContractType = vl_arguments["ID_ContractType"].AsInt32;

            string SettlementMethod = "";
            SettlementMethod = vl_arguments["SettlementMethod"].AsString;

            string DeliveryPoint = "";
            DeliveryPoint = vl_arguments["DeliveryPoint"].AsString;

            string ParticipantIdentifier = "";
            ParticipantIdentifier = vl_arguments["ParticipantIdentifier"].AsString;

            string ParticipantIdentifierType = "";
            ParticipantIdentifierType = vl_arguments["ParticipantIdentifierType"].AsString;

            /*string ParticipantMktID = "";
            ParticipantMktID = vl_arguments["ParticipantMktID"].AsString;*/

            string CounterpartIdentifier = "";
            CounterpartIdentifier = vl_arguments["CounterpartIdentifier"].AsString;

            string CounterpartIdentifierType = "";
            CounterpartIdentifierType = vl_arguments["CounterpartIdentifierType"].AsString;

            /*string CounterpartMktID = "";
            CounterpartMktID = vl_arguments["CounterpartMktID"].AsString;*/

            string BeneficiaryIdentifier = "";
            BeneficiaryIdentifier = vl_arguments["BeneficiaryIdentifier"].AsString;

            string BeneficiaryIdentifierType = "";
            BeneficiaryIdentifierType = vl_arguments["BeneficiaryIdentifierType"].AsString;

            string TransactionID = "";
            TransactionID = vl_arguments["TransactionID"].AsString;

            DateTime TransactionTimestamp = DateTime.Now;
            TransactionTimestamp = vl_arguments["TransactionTimestamp"].AsDateTime;

            string LinkedTransactionID = "";
            if (vl_arguments["LinkedTransactionID"] != null) LinkedTransactionID = vl_arguments["LinkedTransactionID"].AsString;

            string BuySellIndicator = "";
            BuySellIndicator = vl_arguments["BuySellIndicator"].AsString;

            double Price = 0;
            Price = vl_arguments["Price"].AsDouble;

            int ID_Currency = 0;
            ID_Currency = vl_arguments["ID_Currency"].AsInt32;

            /*string PriceFormula = "";
            PriceFormula = vl_arguments["PriceFormula"].AsString;*/

            double NotionalAmount = 0;
            NotionalAmount = vl_arguments["NotionalAmount"].AsDouble;

            int ID_NotionalCurrency = 0;
            ID_NotionalCurrency = vl_arguments["ID_NotionalCurrency"].AsInt32;

            double Volume = 0;
            Volume = vl_arguments["Volume"].AsDouble;

            string VolumeMU = "";
            VolumeMU = vl_arguments["VolumeMU"].AsString;

            double TotalNotionalQuantity = 0;
            TotalNotionalQuantity = vl_arguments["TotalNotionalQuantity"].AsDouble;

            string TotalNotionalQuantityMU = "";
            TotalNotionalQuantityMU = vl_arguments["TotalNotionalQuantityMU"].AsString;

            DateTime DeliveryStartDate = DateTime.Today;
            DeliveryStartDate = vl_arguments["DeliveryStartDate"].AsDateTime;

            DateTime DeliveryEndDate = DateTime.Today;
            DeliveryEndDate = vl_arguments["DeliveryEndDate"].AsDateTime;

            int ID_LoadType = 0;
            ID_LoadType = vl_arguments["ID_LoadType"].AsInt32;

            string ActionType = "";
            ActionType = vl_arguments["ActionType"].AsString;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");

            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_DataSourceType").AsString = "Table1";
                DataSet ds_DataSource = app.DB.Select("select_REMIT_DataSources", "REMIT_DataSources", vl_params);
                if (!app.DB.ValidDSRows(ds_DataSource)) throw new Exception("A suitable data source type not found");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_REMIT_DataSource").AsInt32 = Convert.ToInt32(ds_DataSource.Tables[0].Rows[0]["ID"]);
                vl_params.Add("@prm_InputDataName").AsString = "";
                vl_params.Add("@prm_InputData").AsString = "";
                if (app.DB.Exec("insert_REMIT_DataSourceHistory", "REMIT_DataSourceHistory", vl_params) < 1) throw new Exception("Cannot insert in DataSourceHistory");
                int ID_DataSourceHistory = app.DB.GetIdentity();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_DataSourceHistory").AsInt32 = ID_DataSourceHistory;
                vl_params.Add("@prm_ID_Agency").AsInt32 = session.ID_Agency;
                vl_params.Add("@prm_ContractID").AsString = ContractID;
                vl_params.Add("@prm_ActionType").AsString = ActionType;
                vl_params.Add("@prm_ID_ContractName").AsInt32 = ID_ContractName;
                vl_params.Add("@prm_ID_ContractType").AsInt32 = ID_ContractType;
                vl_params.Add("@prm_SettlementMethod").AsString = SettlementMethod;
                vl_params.Add("@prm_DeliveryPoint").AsString = DeliveryPoint;
                vl_params.Add("@prm_ParticipantIdentifier").AsString = ParticipantIdentifier;
                vl_params.Add("@prm_ParticipantIdentifierType").AsString = ParticipantIdentifierType;
                //vl_params.Add("@prm_ParticipantMktID").AsString = ParticipantMktID;
                vl_params.Add("@prm_CounterpartIdentifier").AsString = CounterpartIdentifier;
                vl_params.Add("@prm_CounterpartIdentifierType").AsString = CounterpartIdentifierType;
                //vl_params.Add("@prm_CounterpartMktID").AsString = CounterpartMktID;
                vl_params.Add("@prm_BeneficiaryIdentifier").AsString = BeneficiaryIdentifier;
                vl_params.Add("@prm_BeneficiaryIdentifierType").AsString = BeneficiaryIdentifierType;
                vl_params.Add("@prm_TransactionID").AsString = TransactionID;
                vl_params.Add("@prm_TransactionTimestamp").AsDateTime = TransactionTimestamp;
                vl_params.Add("@prm_LinkedTransactionID").AsString = LinkedTransactionID;
                vl_params.Add("@prm_BuySellIndicator").AsString = BuySellIndicator;
                vl_params.Add("@prm_Price").AsDouble = Price;
                //vl_params.Add("@prm_PriceFormula").AsString = PriceFormula;
                vl_params.Add("@prm_ID_Currency").AsInt32 = ID_Currency;
                vl_params.Add("@prm_NotionalAmount").AsDouble = NotionalAmount;
                vl_params.Add("@prm_ID_NotionalCurrency").AsInt32 = ID_NotionalCurrency;
                vl_params.Add("@prm_Volume").AsDouble = Volume;
                vl_params.Add("@prm_VolumeMU").AsString = VolumeMU;
                vl_params.Add("@prm_TotalNotionalQuantity").AsDouble = TotalNotionalQuantity;
                vl_params.Add("@prm_TotalNotionalQuantityMU").AsString = TotalNotionalQuantityMU;
                vl_params.Add("@prm_DeliveryStartDate").AsDateTime = DeliveryStartDate;
                vl_params.Add("@prm_DeliveryEndDate").AsDateTime = DeliveryEndDate;
                vl_params.Add("@prm_ID_LoadType").AsInt32 = ID_LoadType;

                if (app.DB.Exec("insert_REMIT_Table1Report", "REMIT_Table1Reports", vl_params) < 1) throw new Exception("Cannot insert Table1Reports");
                int ID_Contract = app.DB.GetIdentity();

                app.DB.CommitTransaction();

                return new TDBExecResult(TDBExecError.Success, "");
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult editREMITTable1Report(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Table1Report"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_Table1Report");
            //if (vl_arguments["ContractID"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ContractID");
            if (vl_arguments["ID_ContractName"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_ContractName");
            if (vl_arguments["ID_ContractType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_ContractType");
            if (vl_arguments["SettlementMethod"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing SettlementMethod");
            if (vl_arguments["DeliveryPoint"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing DeliveryPoint");
            if (vl_arguments["ParticipantIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ParticipantIdentifier");
            if (vl_arguments["ParticipantIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ParticipantIdentifierType");
            //if (vl_arguments["ParticipantMktID"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ParticipantMktID");
            if (vl_arguments["CounterpartIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing CounterpartIdentifier");
            if (vl_arguments["CounterpartIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing CounterpartIdentifierType");
            //if (vl_arguments["CounterpartMktID"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing CounterpartMktID");
            if (vl_arguments["TransactionID"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing TransactionID");
            if (vl_arguments["TransactionTimestamp"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing TransactionTimestamp");
            if (vl_arguments["Price"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing Price");
            if (vl_arguments["ID_Currency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_Currency");
            //if (vl_arguments["PriceFormula"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing PriceFormula");
            if (vl_arguments["NotionalAmount"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing NotionalAmount");
            if (vl_arguments["ID_NotionalCurrency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_NotionalCurrency");
            if (vl_arguments["Volume"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing Volume");
            if (vl_arguments["VolumeMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing VolumeMU");
            if (vl_arguments["TotalNotionalQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing TotalNotionalQuantity");
            if (vl_arguments["TotalNotionalQuantityMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing TotalNotionalQuantityMU");
            if (vl_arguments["DeliveryStartDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing DeliveryStartDate");
            if (vl_arguments["DeliveryEndDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing DeliveryEndDate");
            if (vl_arguments["ID_LoadType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_LoadType");
            if (vl_arguments["ActionType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ActionType");

            int ID_Table1Report = 0;
            ID_Table1Report = vl_arguments["ID_Table1Report"].AsInt32;

            string ContractID = "";
            //ContractID = vl_arguments["ContractID"].AsString;

            int ID_ContractName = 0;
            ID_ContractName = vl_arguments["ID_ContractName"].AsInt32;

            int ID_ContractType = 0;
            ID_ContractType = vl_arguments["ID_ContractType"].AsInt32;

            string SettlementMethod = "";
            SettlementMethod = vl_arguments["SettlementMethod"].AsString;

            string DeliveryPoint = "";
            DeliveryPoint = vl_arguments["DeliveryPoint"].AsString;

            string ParticipantIdentifier = "";
            ParticipantIdentifier = vl_arguments["ParticipantIdentifier"].AsString;

            string ParticipantIdentifierType = "";
            ParticipantIdentifierType = vl_arguments["ParticipantIdentifierType"].AsString;

            /*string ParticipantMktID = "";
            ParticipantMktID = vl_arguments["ParticipantMktID"].AsString;*/

            string CounterpartIdentifier = "";
            CounterpartIdentifier = vl_arguments["CounterpartIdentifier"].AsString;

            string CounterpartIdentifierType = "";
            CounterpartIdentifierType = vl_arguments["CounterpartIdentifierType"].AsString;

            /*string CounterpartMktID = "";
            CounterpartMktID = vl_arguments["CounterpartMktID"].AsString;*/

            string BeneficiaryIdentifier = "";
            BeneficiaryIdentifier = vl_arguments["BeneficiaryIdentifier"].AsString;

            string BeneficiaryIdentifierType = "";
            BeneficiaryIdentifierType = vl_arguments["BeneficiaryIdentifierType"].AsString;

            string TransactionID = "";
            TransactionID = vl_arguments["TransactionID"].AsString;

            DateTime TransactionTimestamp = DateTime.Now;
            TransactionTimestamp = vl_arguments["TransactionTimestamp"].AsDateTime;

            string LinkedTransactionID = "";
            if (vl_arguments["LinkedTransactionID"] != null) LinkedTransactionID = vl_arguments["LinkedTransactionID"].AsString;

            string BuySellIndicator = "";
            BuySellIndicator = vl_arguments["BuySellIndicator"].AsString;

            double Price = 0;
            Price = vl_arguments["Price"].AsDouble;

            int ID_Currency = 0;
            ID_Currency = vl_arguments["ID_Currency"].AsInt32;

            /*string PriceFormula = "";
            PriceFormula = vl_arguments["PriceFormula"].AsString;*/

            double NotionalAmount = 0;
            NotionalAmount = vl_arguments["NotionalAmount"].AsDouble;

            int ID_NotionalCurrency = 0;
            ID_NotionalCurrency = vl_arguments["ID_NotionalCurrency"].AsInt32;

            double Volume = 0;
            Volume = vl_arguments["Volume"].AsDouble;

            string VolumeMU = "";
            VolumeMU = vl_arguments["VolumeMU"].AsString;

            double TotalNotionalQuantity = 0;
            TotalNotionalQuantity = vl_arguments["TotalNotionalQuantity"].AsDouble;

            string TotalNotionalQuantityMU = "";
            TotalNotionalQuantityMU = vl_arguments["TotalNotionalQuantityMU"].AsString;

            DateTime DeliveryStartDate = DateTime.Today;
            DeliveryStartDate = vl_arguments["DeliveryStartDate"].AsDateTime;

            DateTime DeliveryEndDate = DateTime.Today;
            DeliveryEndDate = vl_arguments["DeliveryEndDate"].AsDateTime;

            int ID_LoadType = 0;
            ID_LoadType = vl_arguments["ID_LoadType"].AsInt32;

            string ActionType = "";
            ActionType = vl_arguments["ActionType"].AsString;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");

            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_Table1Report;
                vl_params.Add("@prm_ContractID").AsString = ContractID;
                vl_params.Add("@prm_ActionType").AsString = ActionType;
                vl_params.Add("@prm_ID_ContractName").AsInt32 = ID_ContractName;
                vl_params.Add("@prm_ID_ContractType").AsInt32 = ID_ContractType;
                vl_params.Add("@prm_SettlementMethod").AsString = SettlementMethod;
                vl_params.Add("@prm_DeliveryPoint").AsString = DeliveryPoint;
                vl_params.Add("@prm_ParticipantIdentifier").AsString = ParticipantIdentifier;
                vl_params.Add("@prm_ParticipantIdentifierType").AsString = ParticipantIdentifierType;
                //vl_params.Add("@prm_ParticipantMktID").AsString = ParticipantMktID;
                vl_params.Add("@prm_CounterpartIdentifier").AsString = CounterpartIdentifier;
                vl_params.Add("@prm_CounterpartIdentifierType").AsString = CounterpartIdentifierType;
                //vl_params.Add("@prm_CounterpartMktID").AsString = CounterpartMktID;
                vl_params.Add("@prm_BeneficiaryIdentifier").AsString = BeneficiaryIdentifier;
                vl_params.Add("@prm_BeneficiaryIdentifierType").AsString = BeneficiaryIdentifierType;
                vl_params.Add("@prm_TransactionID").AsString = TransactionID;
                vl_params.Add("@prm_TransactionTimestamp").AsDateTime = TransactionTimestamp;
                vl_params.Add("@prm_LinkedTransactionID").AsString = LinkedTransactionID;
                vl_params.Add("@prm_BuySellIndicator").AsString = BuySellIndicator;
                vl_params.Add("@prm_Price").AsDouble = Price;
                //vl_params.Add("@prm_PriceFormula").AsString = PriceFormula;
                vl_params.Add("@prm_ID_Currency").AsInt32 = ID_Currency;
                vl_params.Add("@prm_NotionalAmount").AsDouble = NotionalAmount;
                vl_params.Add("@prm_ID_NotionalCurrency").AsInt32 = ID_NotionalCurrency;
                vl_params.Add("@prm_Volume").AsDouble = Volume;
                vl_params.Add("@prm_VolumeMU").AsString = VolumeMU;
                vl_params.Add("@prm_TotalNotionalQuantity").AsDouble = TotalNotionalQuantity;
                vl_params.Add("@prm_TotalNotionalQuantityMU").AsString = TotalNotionalQuantityMU;
                vl_params.Add("@prm_DeliveryStartDate").AsDateTime = DeliveryStartDate;
                vl_params.Add("@prm_DeliveryEndDate").AsDateTime = DeliveryEndDate;
                vl_params.Add("@prm_ID_LoadType").AsInt32 = ID_LoadType;

                if (app.DB.Exec("update_REMIT_Table1Report", "REMIT_Table1Reports", vl_params) < 1) throw new Exception("Cannot update Table1Reports");

                app.DB.CommitTransaction();

                return new TDBExecResult(TDBExecError.Success, "");
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult submitREMITTable1Report(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Table1Report"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Table1Report");

            int ID_Table1Report = 0;
            ID_Table1Report = vl_arguments["ID_Table1Report"].AsInt32;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_Table1Report;

                if (app.DB.Exec("submit_REMIT_Table1Report", "REMIT_Table1Reports", vl_params) < 1) throw new Exception("cannot submit Table1Report");
                app.DB.CommitTransaction();
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, exc.Message);
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public int modifyREMITNonStandardContract(TVariantList vl_arguments)
        {
            return 1;
        }

        public int errorREMITNonStandardContract(TVariantList vl_arguments)
        {
            return 1;
        }

        public int cancelREMITNonStandardContract(TVariantList vl_arguments)
        {
            return 1;
        }

        public struct TTable2VolumeOptionality
        {
            public int RowID;
            public int ID;
            public double Capacity;
            public string CapacityMU;
            public DateTime StartDate;
            public DateTime EndDate;
        }

        public struct TTable2FixingIndexDetail
        {
            public int RowID;
            public int ID;
            public string FixingIndex;
            public int ID_FixingIndexContractType;
            public string FixingIndexSource;
            public DateTime FirstFixingDate;
            public DateTime LastFixingDate;
            public string FixingFrequency;
        }

        public TDBExecResult addREMITTable2Report(TVariantList vl_arguments)
        {
            if (vl_arguments["ContractID"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ContractID");
            if (vl_arguments["ContractDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ContractDate");
            if (vl_arguments["ActionType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ActionType");
            if (vl_arguments["ID_ContractName"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_ContractName");
            if (vl_arguments["ID_ContractType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_ContractType");
            if (vl_arguments["DeliveryPoint"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing DeliveryPoint");
            if (vl_arguments["ParticipantIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ParticipantIdentifier");
            if (vl_arguments["ParticipantIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ParticipantIdentifierType");
            if (vl_arguments["OtherParticipantIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing OtherParticipantIdentifier");
            if (vl_arguments["OtherParticipantIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing OtherParticipantIdentifierType");
            if (vl_arguments["BeneficiaryIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing BeneficiaryIdentifier");
            if (vl_arguments["BeneficiaryIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing BeneficiaryIdentifierType");
            if (vl_arguments["TradingCapacityType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing TradingCapacityType");
            if (vl_arguments["BuySellIndicator"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing BuySellIndicator");

            if (vl_arguments["DeliveryStartDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing DeliveryStartDate");
            if (vl_arguments["DeliveryEndDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing DeliveryEndDate");
            //if (vl_arguments["Volume"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing Volume");
            //if (vl_arguments["ID_VolumeMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_VolumeMU");
            if (vl_arguments["ID_LoadType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_LoadType");

            if (vl_arguments["Price"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing Price");
            if (vl_arguments["PriceFormula"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing PriceFormula");
            if (vl_arguments["ID_Currency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_Currency");
            if (vl_arguments["EstimatedNotionalAmount"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing EstimatedNotionalAmount");
            if (vl_arguments["ID_NotionalCurrency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_NotionalCurrency");
            if (vl_arguments["TotalNotionalQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing TotalNotionalQuantity");
            if (vl_arguments["TotalNotionalQuantityMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_TotalNotionalQuantityMU");
            if (vl_arguments["VolumeOptionality"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing VolumeOptionality");
            if (vl_arguments["VolumeOptionalityFrequency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing VolumeOptionalityFrequency");
            if (vl_arguments["TypeOfIndexPrice"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing TypeOfIndexPrice");
            if (vl_arguments["SettlementMethod"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing SettlementMethod");

            string ContractID = "";
            ContractID = vl_arguments["ContractID"].AsString;

            DateTime ContractDate = DateTime.Today;
            ContractDate = vl_arguments["ContractDate"].AsDateTime;

            int ID_ContractName = 0;
            ID_ContractName = vl_arguments["ID_ContractName"].AsInt32;

            int ID_ContractType = 0;
            ID_ContractType = vl_arguments["ID_ContractType"].AsInt32;

            string DeliveryPoint = "";
            DeliveryPoint = vl_arguments["DeliveryPoint"].AsString;

            string ParticipantIdentifier = "";
            ParticipantIdentifier = vl_arguments["ParticipantIdentifier"].AsString;

            string ParticipantIdentifierType = "";
            ParticipantIdentifierType = vl_arguments["ParticipantIdentifierType"].AsString;

            string OtherParticipantIdentifier = "";
            OtherParticipantIdentifier = vl_arguments["OtherParticipantIdentifier"].AsString;

            string OtherParticipantIdentifierType = "";
            OtherParticipantIdentifierType = vl_arguments["OtherParticipantIdentifierType"].AsString;

            string BeneficiaryIdentifier = "";
            BeneficiaryIdentifier = vl_arguments["BeneficiaryIdentifier"].AsString;

            string BeneficiaryIdentifierType = "";
            BeneficiaryIdentifierType = vl_arguments["BeneficiaryIdentifierType"].AsString;

            string TradingCapacityType = "";
            TradingCapacityType = vl_arguments["TradingCapacityType"].AsString;

            string BuySellIndicator = "";
            BuySellIndicator = vl_arguments["BuySellIndicator"].AsString;

            double Price = 0;
            Price = vl_arguments["Price"].AsDouble;

            int ID_Currency = 0;
            ID_Currency = vl_arguments["ID_Currency"].AsInt32;

            string PriceFormula = "";
            PriceFormula = vl_arguments["PriceFormula"].AsString;

            double EstimatedNotionalAmount = double.NaN;
            if (vl_arguments["EstimatedNotionalAmount"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["EstimatedNotionalAmount"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["EstimatedNotionalAmount"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["EstimatedNotionalAmount"].ValueType == TVariantType.vtSingle ||
                vl_arguments["EstimatedNotionalAmount"].ValueType == TVariantType.vtDouble) EstimatedNotionalAmount = vl_arguments["EstimatedNotionalAmount"].AsDouble;

            int ID_NotionalCurrency = 0;
            ID_NotionalCurrency = vl_arguments["ID_NotionalCurrency"].AsInt32;

            double TotalNotionalQuantity = double.NaN;
            if (vl_arguments["TotalNotionalQuantity"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["TotalNotionalQuantity"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["TotalNotionalQuantity"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["TotalNotionalQuantity"].ValueType == TVariantType.vtSingle ||
                vl_arguments["TotalNotionalQuantity"].ValueType == TVariantType.vtDouble) TotalNotionalQuantity = vl_arguments["TotalNotionalQuantity"].AsDouble;

            string TotalNotionalQuantityMU = "";
            TotalNotionalQuantityMU = vl_arguments["TotalNotionalQuantityMU"].AsString;

            DateTime DeliveryStartDate = DateTime.Today;
            DeliveryStartDate = vl_arguments["DeliveryStartDate"].AsDateTime;

            DateTime DeliveryEndDate = DateTime.Today;
            DeliveryEndDate = vl_arguments["DeliveryEndDate"].AsDateTime;

            string VolumeOptionality = "";
            VolumeOptionality = vl_arguments["VolumeOptionality"].AsString;

            string VolumeOptionalityFrequency = "";
            VolumeOptionalityFrequency = vl_arguments["VolumeOptionalityFrequency"].AsString;

            string TypeOfIndexPrice = "";
            TypeOfIndexPrice = vl_arguments["TypeOfIndexPrice"].AsString;

            string SettlementMethod = "";
            SettlementMethod = vl_arguments["SettlementMethod"].AsString;

            int ID_LoadType = 0;
            ID_LoadType = vl_arguments["ID_LoadType"].AsInt32;

            string ActionType = "";
            ActionType = vl_arguments["ActionType"].AsString;

            TVariantList VolumeOptionalities = null;
            TTable2VolumeOptionality[] obj_VolumeOptionalities = null;
            if (vl_arguments["VolumeOptionalities"] != null && vl_arguments["VolumeOptionalities"].ValueType == TVariantType.vtObject)
            {
                VolumeOptionalities = vl_arguments["VolumeOptionalities"].AsObject as TVariantList;
                obj_VolumeOptionalities = new TTable2VolumeOptionality[VolumeOptionalities.Count];
                for (int i = 0; i < VolumeOptionalities.Count; i++)
                {
                    if (VolumeOptionalities[i].ValueType != TVariantType.vtString) continue;
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    TTable2VolumeOptionality obj = serializer.Deserialize<TTable2VolumeOptionality>(VolumeOptionalities[i].AsString);

                    obj_VolumeOptionalities[i] = obj;
                }

            }

            TVariantList FixingIndexDetails = null;
            TTable2FixingIndexDetail[] obj_FixingIndexDetails = null;
            if (vl_arguments["FixingIndexDetails"] != null && vl_arguments["FixingIndexDetails"].ValueType == TVariantType.vtObject)
            {
                FixingIndexDetails = vl_arguments["FixingIndexDetails"].AsObject as TVariantList;
                obj_FixingIndexDetails = new TTable2FixingIndexDetail[FixingIndexDetails.Count];
                for (int i = 0; i < FixingIndexDetails.Count; i++)
                {
                    if (FixingIndexDetails[i].ValueType != TVariantType.vtString) continue;
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    TTable2FixingIndexDetail obj = serializer.Deserialize<TTable2FixingIndexDetail>(FixingIndexDetails[i].AsString);

                    obj_FixingIndexDetails[i] = obj;
                }
            }

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");

            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_DataSourceType").AsString = "Table2";
                DataSet ds_DataSource = app.DB.Select("select_REMIT_DataSources", "REMIT_DataSources", vl_params);
                if (!app.DB.ValidDSRows(ds_DataSource)) throw new Exception("A suitable data source type not found");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_REMIT_DataSource").AsInt32 = Convert.ToInt32(ds_DataSource.Tables[0].Rows[0]["ID"]);
                vl_params.Add("@prm_InputDataName").AsString = "";
                vl_params.Add("@prm_InputData").AsString = "";
                if (app.DB.Exec("insert_REMIT_DataSourceHistory", "REMIT_DataSourceHistory", vl_params) < 1) throw new Exception("Cannot insert in DataSourceHistory");
                int ID_DataSourceHistory = app.DB.GetIdentity();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_DataSourceHistory").AsInt32 = ID_DataSourceHistory;
                vl_params.Add("@prm_ID_Agency").AsInt32 = session.ID_Agency;
                vl_params.Add("@prm_ContractID").AsString = ContractID;
                vl_params.Add("@prm_ContractDate").AsDateTime = ContractDate;
                vl_params.Add("@prm_ActionType").AsString = ActionType;
                vl_params.Add("@prm_ID_ContractName").AsInt32 = ID_ContractName;
                vl_params.Add("@prm_ID_ContractType").AsInt32 = ID_ContractType;
                vl_params.Add("@prm_DeliveryPoint").AsString = DeliveryPoint;
                vl_params.Add("@prm_ParticipantIdentifier").AsString = ParticipantIdentifier;
                vl_params.Add("@prm_ParticipantIdentifierType").AsString = ParticipantIdentifierType;
                vl_params.Add("@prm_OtherParticipantIdentifier").AsString = OtherParticipantIdentifier;
                vl_params.Add("@prm_OtherParticipantIdentifierType").AsString = OtherParticipantIdentifierType;
                vl_params.Add("@prm_BeneficiaryIdentifier").AsString = BeneficiaryIdentifier;
                vl_params.Add("@prm_BeneficiaryIdentifierType").AsString = BeneficiaryIdentifierType;
                vl_params.Add("@prm_TradingCapacityType").AsString = TradingCapacityType;
                vl_params.Add("@prm_BuySellIndicator").AsString = BuySellIndicator;
                vl_params.Add("@prm_Price").AsDouble = Price;
                vl_params.Add("@prm_ID_Currency").AsInt32 = ID_Currency;
                vl_params.Add("@prm_PriceFormula").AsString = PriceFormula;
                vl_params.Add("@prm_EstimatedNotionalAmount").AsDouble = EstimatedNotionalAmount;
                vl_params.Add("@prm_ID_NotionalCurrency").AsInt32 = ID_NotionalCurrency;
                vl_params.Add("@prm_TotalNotionalQuantity").AsDouble = TotalNotionalQuantity;
                vl_params.Add("@prm_TotalNotionalQuantityMU").AsString = TotalNotionalQuantityMU;
                vl_params.Add("@prm_DeliveryStartDate").AsDateTime = DeliveryStartDate;
                vl_params.Add("@prm_DeliveryEndDate").AsDateTime = DeliveryEndDate;
                vl_params.Add("@prm_VolumeOptionality").AsString = VolumeOptionality;
                vl_params.Add("@prm_VolumeOptionalityFrequency").AsString = VolumeOptionalityFrequency;
                vl_params.Add("@prm_TypeOfIndexPrice").AsString = TypeOfIndexPrice;
                vl_params.Add("@prm_SettlementMethod").AsString = SettlementMethod;
                vl_params.Add("@prm_ID_LoadType").AsInt32 = ID_LoadType;

                if (app.DB.Exec("insert_REMIT_Table2Report", "REMIT_Table2Reports", vl_params) < 1) throw new Exception("Cannot insert NonStandardContract");
                int ID_Table2Report = app.DB.GetIdentity();

                //  update Table2VolumeOptionalities
                if (obj_VolumeOptionalities != null)
                    for (int i = 0; i < obj_VolumeOptionalities.Length; i++)
                    {
                        //if (obj_VolumeOptionalities[i] == null) continue;

                        vl_params = new TVariantList();
                        vl_params.Add("@prm_ID_Table2Report").AsInt32 = ID_Table2Report;
                        vl_params.Add("@prm_ID").AsDouble = obj_VolumeOptionalities[i].ID;
                        vl_params.Add("@prm_Capacity").AsDouble = obj_VolumeOptionalities[i].Capacity;
                        vl_params.Add("@prm_CapacityMU").AsString = obj_VolumeOptionalities[i].CapacityMU;
                        vl_params.Add("@prm_StartDate").AsDateTime = obj_VolumeOptionalities[i].StartDate;
                        vl_params.Add("@prm_EndDate").AsDateTime = obj_VolumeOptionalities[i].EndDate;
                        app.DB.Exec("update_REMIT_Table2VolumeOptionality", "REMIT_Table2Reports", vl_params);
                    }

                //  update Table2VolumeOptionalities
                if (obj_FixingIndexDetails != null)
                    for (int i = 0; i < obj_FixingIndexDetails.Length; i++)
                    {
                        //if (obj_FixingIndexDetails[i] == null) continue;

                        vl_params = new TVariantList();
                        vl_params.Add("@prm_ID_Table2Report").AsInt32 = ID_Table2Report;
                        vl_params.Add("@prm_ID").AsDouble = obj_FixingIndexDetails[i].ID;
                        vl_params.Add("@prm_FixingIndex").AsString = obj_FixingIndexDetails[i].FixingIndex;
                        vl_params.Add("@prm_ID_FixingIndexContractType").AsInt32 = obj_FixingIndexDetails[i].ID_FixingIndexContractType;
                        vl_params.Add("@prm_FixingIndexSource").AsString = obj_FixingIndexDetails[i].FixingIndexSource;
                        vl_params.Add("@prm_FirstFixingDate").AsDateTime = obj_FixingIndexDetails[i].FirstFixingDate;
                        vl_params.Add("@prm_LastFixingDate").AsDateTime = obj_FixingIndexDetails[i].LastFixingDate;
                        vl_params.Add("@prm_FixingFrequency").AsString = obj_FixingIndexDetails[i].FixingFrequency;
                        app.DB.Exec("update_REMIT_Table2FixingIndexDetail", "REMIT_Table2Reports", vl_params);
                    }

                app.DB.CommitTransaction();

                return new TDBExecResult(TDBExecError.Success, "");
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult editREMITTable2Report(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Table2Report"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_Table2Report");
            if (vl_arguments["ContractID"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ContractID");
            if (vl_arguments["ContractDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ContractDate");
            if (vl_arguments["ActionType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ActionType");
            if (vl_arguments["ID_ContractName"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_ContractName");
            if (vl_arguments["ID_ContractType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_ContractType");
            if (vl_arguments["DeliveryPoint"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing DeliveryPoint");
            if (vl_arguments["ParticipantIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ParticipantIdentifier");
            if (vl_arguments["ParticipantIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ParticipantIdentifierType");
            if (vl_arguments["OtherParticipantIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing OtherParticipantIdentifier");
            if (vl_arguments["OtherParticipantIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing OtherParticipantIdentifierType");
            if (vl_arguments["BeneficiaryIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing BeneficiaryIdentifier");
            if (vl_arguments["BeneficiaryIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing BeneficiaryIdentifierType");
            if (vl_arguments["TradingCapacityType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing TradingCapacityType");
            if (vl_arguments["BuySellIndicator"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing BuySellIndicator");

            if (vl_arguments["DeliveryStartDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing DeliveryStartDate");
            if (vl_arguments["DeliveryEndDate"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing DeliveryEndDate");
            //if (vl_arguments["Volume"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing Volume");
            //if (vl_arguments["ID_VolumeMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_VolumeMU");
            if (vl_arguments["ID_LoadType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_LoadType");

            if (vl_arguments["Price"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing Price");
            if (vl_arguments["PriceFormula"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing PriceFormula");
            if (vl_arguments["ID_Currency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_Currency");
            if (vl_arguments["EstimatedNotionalAmount"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing EstimatedNotionalAmount");
            if (vl_arguments["ID_NotionalCurrency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_NotionalCurrency");
            if (vl_arguments["TotalNotionalQuantity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing TotalNotionalQuantity");
            if (vl_arguments["TotalNotionalQuantityMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing ID_TotalNotionalQuantityMU");
            if (vl_arguments["VolumeOptionality"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing VolumeOptionality");
            if (vl_arguments["VolumeOptionalityFrequency"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing VolumeOptionalityFrequency");
            if (vl_arguments["TypeOfIndexPrice"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing TypeOfIndexPrice");
            if (vl_arguments["SettlementMethod"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Missing SettlementMethod");

            int ID_Table2Report = 0;
            ID_Table2Report = vl_arguments["ID_Table2Report"].AsInt32;

            string ContractID = "";
            ContractID = vl_arguments["ContractID"].AsString;

            DateTime ContractDate = DateTime.Today;
            ContractDate = vl_arguments["ContractDate"].AsDateTime;

            int ID_ContractName = 0;
            ID_ContractName = vl_arguments["ID_ContractName"].AsInt32;

            int ID_ContractType = 0;
            ID_ContractType = vl_arguments["ID_ContractType"].AsInt32;

            string DeliveryPoint = "";
            DeliveryPoint = vl_arguments["DeliveryPoint"].AsString;

            string ParticipantIdentifier = "";
            ParticipantIdentifier = vl_arguments["ParticipantIdentifier"].AsString;

            string ParticipantIdentifierType = "";
            ParticipantIdentifierType = vl_arguments["ParticipantIdentifierType"].AsString;

            string OtherParticipantIdentifier = "";
            OtherParticipantIdentifier = vl_arguments["OtherParticipantIdentifier"].AsString;

            string OtherParticipantIdentifierType = "";
            OtherParticipantIdentifierType = vl_arguments["OtherParticipantIdentifierType"].AsString;

            string BeneficiaryIdentifier = "";
            BeneficiaryIdentifier = vl_arguments["BeneficiaryIdentifier"].AsString;

            string BeneficiaryIdentifierType = "";
            BeneficiaryIdentifierType = vl_arguments["BeneficiaryIdentifierType"].AsString;

            string TradingCapacityType = "";
            TradingCapacityType = vl_arguments["TradingCapacityType"].AsString;

            string BuySellIndicator = "";
            BuySellIndicator = vl_arguments["BuySellIndicator"].AsString;

            double Price = 0;
            Price = vl_arguments["Price"].AsDouble;

            int ID_Currency = 0;
            ID_Currency = vl_arguments["ID_Currency"].AsInt32;

            string PriceFormula = "";
            PriceFormula = vl_arguments["PriceFormula"].AsString;

            double EstimatedNotionalAmount = Double.NaN;
            if (vl_arguments["EstimatedNotionalAmount"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["EstimatedNotionalAmount"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["EstimatedNotionalAmount"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["EstimatedNotionalAmount"].ValueType == TVariantType.vtSingle ||
                vl_arguments["EstimatedNotionalAmount"].ValueType == TVariantType.vtDouble) EstimatedNotionalAmount = vl_arguments["EstimatedNotionalAmount"].AsDouble;

            int ID_NotionalCurrency = 0;
            ID_NotionalCurrency = vl_arguments["ID_NotionalCurrency"].AsInt32;

            double TotalNotionalQuantity = double.NaN;
            if (vl_arguments["TotalNotionalQuantity"].ValueType == TVariantType.vtInt16 ||
                vl_arguments["TotalNotionalQuantity"].ValueType == TVariantType.vtInt32 ||
                vl_arguments["TotalNotionalQuantity"].ValueType == TVariantType.vtInt64 ||
                vl_arguments["TotalNotionalQuantity"].ValueType == TVariantType.vtSingle ||
                vl_arguments["TotalNotionalQuantity"].ValueType == TVariantType.vtDouble) TotalNotionalQuantity = vl_arguments["TotalNotionalQuantity"].AsDouble;

            string TotalNotionalQuantityMU = "";
            TotalNotionalQuantityMU = vl_arguments["TotalNotionalQuantityMU"].AsString;

            DateTime DeliveryStartDate = DateTime.Today;
            DeliveryStartDate = vl_arguments["DeliveryStartDate"].AsDateTime;

            DateTime DeliveryEndDate = DateTime.Today;
            DeliveryEndDate = vl_arguments["DeliveryEndDate"].AsDateTime;

            string VolumeOptionality = "";
            VolumeOptionality = vl_arguments["VolumeOptionality"].AsString;

            string VolumeOptionalityFrequency = "";
            VolumeOptionalityFrequency = vl_arguments["VolumeOptionalityFrequency"].AsString;

            string TypeOfIndexPrice = "";
            TypeOfIndexPrice = vl_arguments["TypeOfIndexPrice"].AsString;

            string SettlementMethod = "";
            SettlementMethod = vl_arguments["SettlementMethod"].AsString;

            int ID_LoadType = 0;
            ID_LoadType = vl_arguments["ID_LoadType"].AsInt32;

            string ActionType = "";
            ActionType = vl_arguments["ActionType"].AsString;

            TVariantList VolumeOptionalities = null;
            TTable2VolumeOptionality[] obj_VolumeOptionalities = null;
            if (vl_arguments["VolumeOptionalities"] != null && vl_arguments["VolumeOptionalities"].ValueType == TVariantType.vtObject)
            {
                VolumeOptionalities = vl_arguments["VolumeOptionalities"].AsObject as TVariantList;
                obj_VolumeOptionalities = new TTable2VolumeOptionality[VolumeOptionalities.Count];
                for (int i = 0; i < VolumeOptionalities.Count; i++)
                {
                    if (VolumeOptionalities[i].ValueType != TVariantType.vtString) continue;
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    TTable2VolumeOptionality obj = serializer.Deserialize < TTable2VolumeOptionality >(VolumeOptionalities[i].AsString);

                    obj_VolumeOptionalities[i] = obj;
                }

            }

            TVariantList FixingIndexDetails = null;
            TTable2FixingIndexDetail[] obj_FixingIndexDetails = null;
            if (vl_arguments["FixingIndexDetails"] != null && vl_arguments["FixingIndexDetails"].ValueType == TVariantType.vtObject)
            {
                FixingIndexDetails = vl_arguments["FixingIndexDetails"].AsObject as TVariantList;
                obj_FixingIndexDetails = new TTable2FixingIndexDetail[FixingIndexDetails.Count];
                for (int i = 0; i < FixingIndexDetails.Count; i++)
                {
                    if (FixingIndexDetails[i].ValueType != TVariantType.vtString) continue;
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    TTable2FixingIndexDetail obj = serializer.Deserialize < TTable2FixingIndexDetail > (FixingIndexDetails[i].AsString);

                    obj_FixingIndexDetails[i] = obj;
                }
            }

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");

            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_Table2Report;
                vl_params.Add("@prm_ContractID").AsString = ContractID;
                vl_params.Add("@prm_ContractDate").AsDateTime = ContractDate;
                vl_params.Add("@prm_ActionType").AsString = ActionType;
                vl_params.Add("@prm_ID_ContractName").AsInt32 = ID_ContractName;
                vl_params.Add("@prm_ID_ContractType").AsInt32 = ID_ContractType;
                vl_params.Add("@prm_DeliveryPoint").AsString = DeliveryPoint;
                vl_params.Add("@prm_ParticipantIdentifier").AsString = ParticipantIdentifier;
                vl_params.Add("@prm_ParticipantIdentifierType").AsString = ParticipantIdentifierType;
                vl_params.Add("@prm_OtherParticipantIdentifier").AsString = OtherParticipantIdentifier;
                vl_params.Add("@prm_OtherParticipantIdentifierType").AsString = OtherParticipantIdentifierType;
                vl_params.Add("@prm_BeneficiaryIdentifier").AsString = BeneficiaryIdentifier;
                vl_params.Add("@prm_BeneficiaryIdentifierType").AsString = BeneficiaryIdentifierType;
                vl_params.Add("@prm_TradingCapacityType").AsString = TradingCapacityType;
                vl_params.Add("@prm_BuySellIndicator").AsString = BuySellIndicator;
                vl_params.Add("@prm_Price").AsDouble = Price;
                vl_params.Add("@prm_ID_Currency").AsInt32 = ID_Currency;
                vl_params.Add("@prm_PriceFormula").AsString = PriceFormula;
                vl_params.Add("@prm_EstimatedNotionalAmount").AsDouble = EstimatedNotionalAmount;
                vl_params.Add("@prm_ID_NotionalCurrency").AsInt32 = ID_NotionalCurrency;
                vl_params.Add("@prm_TotalNotionalQuantity").AsDouble = TotalNotionalQuantity;
                vl_params.Add("@prm_TotalNotionalQuantityMU").AsString = TotalNotionalQuantityMU;
                vl_params.Add("@prm_DeliveryStartDate").AsDateTime = DeliveryStartDate;
                vl_params.Add("@prm_DeliveryEndDate").AsDateTime = DeliveryEndDate;
                vl_params.Add("@prm_VolumeOptionality").AsString = VolumeOptionality;
                vl_params.Add("@prm_VolumeOptionalityFrequency").AsString = VolumeOptionalityFrequency;
                vl_params.Add("@prm_TypeOfIndexPrice").AsString = TypeOfIndexPrice;
                vl_params.Add("@prm_SettlementMethod").AsString = SettlementMethod;
                vl_params.Add("@prm_ID_LoadType").AsInt32 = ID_LoadType;

                if (app.DB.Exec("update_REMIT_Table2Report", "REMIT_Table2Reports", vl_params) < 1) throw new Exception("Cannot insert NonStandardContract");

                //  update Table2VolumeOptionalities
                for (int i = 0; i < obj_VolumeOptionalities.Length; i++)
                {
                    //if (obj_VolumeOptionalities[i] == null) continue;

                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_Table2Report").AsInt32 = ID_Table2Report;
                    vl_params.Add("@prm_ID").AsDouble = obj_VolumeOptionalities[i].ID;
                    vl_params.Add("@prm_Capacity").AsDouble = obj_VolumeOptionalities[i].Capacity;
                    vl_params.Add("@prm_CapacityMU").AsString = obj_VolumeOptionalities[i].CapacityMU;
                    vl_params.Add("@prm_StartDate").AsDateTime = obj_VolumeOptionalities[i].StartDate;
                    vl_params.Add("@prm_EndDate").AsDateTime = obj_VolumeOptionalities[i].EndDate;
                    app.DB.Exec("update_REMIT_Table2VolumeOptionality", "REMIT_Table2Reports", vl_params);
                }

                //  update Table2VolumeOptionalities
                for (int i = 0; i < obj_FixingIndexDetails.Length; i++)
                {
                    //if (obj_FixingIndexDetails[i] == null) continue;

                    vl_params = new TVariantList();
                    vl_params.Add("@prm_ID_Table2Report").AsInt32 = ID_Table2Report;
                    vl_params.Add("@prm_ID").AsDouble = obj_FixingIndexDetails[i].ID;
                    vl_params.Add("@prm_FixingIndex").AsString = obj_FixingIndexDetails[i].FixingIndex;
                    vl_params.Add("@prm_ID_FixingIndexContractType").AsInt32 = obj_FixingIndexDetails[i].ID_FixingIndexContractType;
                    vl_params.Add("@prm_FixingIndexSource").AsString = obj_FixingIndexDetails[i].FixingIndexSource;
                    vl_params.Add("@prm_FirstFixingDate").AsDateTime = obj_FixingIndexDetails[i].FirstFixingDate;
                    vl_params.Add("@prm_LastFixingDate").AsDateTime = obj_FixingIndexDetails[i].LastFixingDate;
                    vl_params.Add("@prm_FixingFrequency").AsString = obj_FixingIndexDetails[i].FixingFrequency;
                    app.DB.Exec("update_REMIT_Table2FixingIndexDetail", "REMIT_Table2Reports", vl_params);
                }

                app.DB.CommitTransaction();
                return new TDBExecResult(TDBExecError.Success, "");
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, "");
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult submitREMITTable2Report(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_Table2Report"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_Table2Report");

            int ID_Table2Report = 0;
            ID_Table2Report = vl_arguments["ID_Table2Report"].AsInt32;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_Table2Report;

                if (app.DB.Exec("submit_REMIT_Table2Report", "REMIT_Table2Reports", vl_params) < 1) throw new Exception("cannot submit ID_Table2Report");
                app.DB.CommitTransaction();
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, exc.Message);
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult createREMITStorageReport(TVariantList vl_arguments)
        {
            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_DataSourceType").AsString = "NGStorage";
                DataSet ds_DataSource = app.DB.Select("select_REMIT_DataSources", "REMIT_DataSources", vl_params);
                if (!app.DB.ValidDSRows(ds_DataSource)) throw new Exception("A suitable data source type not found");

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_REMIT_DataSource").AsInt32 = Convert.ToInt32(ds_DataSource.Tables[0].Rows[0]["ID"]);
                vl_params.Add("@prm_InputDataName").AsString = "";
                vl_params.Add("@prm_InputData").AsString = "";
                if (app.DB.Exec("insert_REMIT_DataSourceHistory", "REMIT_DataSourceHistory", vl_params) < 1) throw new Exception("Cannot insert in DataSourceHistory");
                int ID_DataSourceHistory = app.DB.GetIdentity();

                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_DataSourceHistory").AsInt32 = ID_DataSourceHistory;
                vl_params.Add("@prm_ID_Agency").AsInt32 = session.ID_Agency;
                if (app.DB.Exec("insert_REMIT_StorageReport", "REMIT_StorageReports", vl_params) < 1) throw new Exception("cannot create StorageReport");
                int ID_StorageReport = app.DB.GetIdentity();

                app.DB.CommitTransaction();
                TDBExecResult res = new TDBExecResult(TDBExecError.Success, "");
                res.IDs = new TVariantList();
                res.IDs.Add("ID_StorageReport").AsInt32 = ID_StorageReport;
                return res;
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, exc.Message);
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult submitREMITStorageReport(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_StorageReport"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_StorageReport");

            int ID_StorageReport = 0;
            ID_StorageReport = vl_arguments["ID_StorageReport"].AsInt32;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_StorageReport;

                if (app.DB.Exec("submit_REMIT_StorageReport", "REMIT_StorageReports", vl_params) < 1) throw new Exception("cannot submit StorageReport");
                app.DB.CommitTransaction();
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, exc.Message);
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult checkREMITEmptyStorageReport(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_StorageReport"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_StorageReport");

            int ID_StorageReport = 0;
            ID_StorageReport = vl_arguments["ID_StorageReport"].AsInt32;

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_StorageReport;

                if (app.DB.Exec("check_REMIT_EmptyStorageReport", "REMIT_StorageReports", vl_params) < 1) throw new Exception("cannot check StorageReport");
                app.DB.CommitTransaction();
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, exc.Message);
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult addREMITStorageFacilityReport(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_StorageReport"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_StorageReport");
            if (vl_arguments["GasDayStart"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "GasDayStart");
            if (vl_arguments["GasDayEnd"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "GasDayEnd");
            if (vl_arguments["StorageFacilityIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityIdentifier");
            if (vl_arguments["StorageFacilityOperatorIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityOperatorIdentifier");
            if (vl_arguments["StorageType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageType");
            if (vl_arguments["Storage"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Storage");
            if (vl_arguments["Injection"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Injection");
            if (vl_arguments["Withdrawal"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Withdrawal");
            if (vl_arguments["TechnicalCapacity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TechnicalCapacity");
            if (vl_arguments["ContractedCapacity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ContractedCapacity");
            if (vl_arguments["AvailableCapacity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "AvailableCapacity");

            if (vl_arguments["StorageFacilityIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityIdentifierType");
            if (vl_arguments["StorageFacilityOperatorIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityOperatorIdentifierType");
            if (vl_arguments["StorageMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageMU");
            if (vl_arguments["InjectionMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "InjectionMU");
            if (vl_arguments["WithdrawalMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "WithdrawalMU");
            if (vl_arguments["TechnicalCapacityMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TechnicalCapacityMU");
            if (vl_arguments["ContractedCapacityMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ContractedCapacityMU");
            if (vl_arguments["AvailableCapacityMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "AvailableCapacityMU");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_StorageReport = 0;
            DateTime GasDayStart = DateTime.Now;
            DateTime GasDayEnd = DateTime.Now;
            string StorageFacilityIdentifier = "";
            string StorageFacilityOperatorIdentifier = "";
            string StorageType = "";
            double Storage = 0;
            double Injection = 0;
            double Withdrawal = 0;
            double TechnicalCapacity = 0;
            double ContractedCapacity = 0;
            double AvailableCapacity = 0;

            string StorageFacilityIdentifierType = "";
            string StorageFacilityOperatorIdentifierType = "";
            string StorageMU = "";
            string InjectionMU = "";
            string WithdrawalMU = "";
            string TechnicalCapacityMU = "";
            string ContractedCapacityMU = "";
            string AvailableCapacityMU = "";

            try
            {
                if (vl_arguments["ID_StorageReport"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_StorageReport"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_StorageReport"].ValueType == TVariantType.vtInt64) ID_StorageReport = vl_arguments["ID_StorageReport"].AsInt32;
                else res.ParamValidations["ID_StorageReport"].AsString = "Value type not recognized";

                if (vl_arguments["GasDayStart"].ValueType == TVariantType.vtDateTime) GasDayStart = vl_arguments["GasDayStart"].AsDateTime;
                else res.ParamValidations["GasDayStart"].AsString = "Argument is not a datetime value";

                if (vl_arguments["GasDayEnd"].ValueType == TVariantType.vtDateTime) GasDayEnd = vl_arguments["GasDayEnd"].AsDateTime;
                else res.ParamValidations["GasDayEnd"].AsString = "Argument is not a datetime value";

                StorageFacilityIdentifier = vl_arguments["StorageFacilityIdentifier"].AsString;

                StorageFacilityOperatorIdentifier = vl_arguments["StorageFacilityOperatorIdentifier"].AsString;

                StorageType = vl_arguments["StorageType"].AsString;
                if (StorageType.Trim() == "") res.ParamValidations["StorageType"].AsString = "Empty value given";

                Storage = vl_arguments["Storage"].AsDouble;
                Injection = vl_arguments["Injection"].AsDouble;
                Withdrawal = vl_arguments["Withdrawal"].AsDouble;
                TechnicalCapacity = vl_arguments["TechnicalCapacity"].AsDouble;
                ContractedCapacity = vl_arguments["ContractedCapacity"].AsDouble;
                AvailableCapacity = vl_arguments["AvailableCapacity"].AsDouble;

                StorageFacilityIdentifierType = vl_arguments["StorageFacilityIdentifierType"].AsString;
                StorageFacilityOperatorIdentifierType = vl_arguments["StorageFacilityOperatorIdentifierType"].AsString;
                StorageMU = vl_arguments["StorageMU"].AsString;
                InjectionMU = vl_arguments["InjectionMU"].AsString;
                WithdrawalMU = vl_arguments["WithdrawalMU"].AsString;
                TechnicalCapacityMU = vl_arguments["TechnicalCapacityMU"].AsString;
                ContractedCapacityMU = vl_arguments["ContractedCapacityMU"].AsString;
                AvailableCapacityMU = vl_arguments["AvailableCapacityMU"].AsString;

                if (res.bInvalid()) return res;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.ValidationUnsuccesful, exc.Message);
            }

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_StorageReport").AsInt32 = ID_StorageReport;
                vl_params.Add("@prm_GasDayStart").AsDateTime = GasDayStart;
                vl_params.Add("@prm_GasDayEnd").AsDateTime = GasDayEnd;
                vl_params.Add("@prm_StorageFacilityIdentifier").AsString = StorageFacilityIdentifier;
                vl_params.Add("@prm_StorageFacilityOperatorIdentifier").AsString = StorageFacilityOperatorIdentifier;
                vl_params.Add("@prm_StorageType").AsString = StorageType;
                vl_params.Add("@prm_Storage").AsDouble = Storage;
                vl_params.Add("@prm_Injection").AsDouble = Injection;
                vl_params.Add("@prm_Withdrawal").AsDouble = Withdrawal;
                vl_params.Add("@prm_TechnicalCapacity").AsDouble = TechnicalCapacity;
                vl_params.Add("@prm_ContractedCapacity").AsDouble = ContractedCapacity;
                vl_params.Add("@prm_AvailableCapacity").AsDouble = AvailableCapacity;

                vl_params.Add("@prm_StorageFacilityIdentifierType").AsString = StorageFacilityIdentifierType;
                vl_params.Add("@prm_StorageFacilityOperatorIdentifierType").AsString = StorageFacilityOperatorIdentifierType;
                vl_params.Add("@prm_StorageMU").AsString = StorageMU;
                vl_params.Add("@prm_InjectionMU").AsString = InjectionMU;
                vl_params.Add("@prm_WithdrawalMU").AsString = WithdrawalMU;
                vl_params.Add("@prm_TechnicalCapacityMU").AsString = TechnicalCapacityMU;
                vl_params.Add("@prm_ContractedCapacityMU").AsString = ContractedCapacityMU;
                vl_params.Add("@prm_AvailableCapacityMU").AsString = AvailableCapacityMU;

                if (app.DB.Exec("insert_REMIT_StorageFacilityReport", "REMIT_StorageFacilityReports", vl_params) < 1) throw new Exception("cannot insert StorageFacilityReport");
                app.DB.CommitTransaction();
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, exc.Message);
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult editREMITStorageFacilityReport(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_StorageFacilityReport"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_StorageFacilityReport");
            //if (vl_arguments["ID_StorageReport"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_StorageReport");
            if (vl_arguments["GasDayStart"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "GasDayStart");
            if (vl_arguments["GasDayEnd"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "GasDayEnd");
            if (vl_arguments["StorageFacilityIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityIdentifier");
            if (vl_arguments["StorageFacilityOperatorIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityOperatorIdentifier");
            if (vl_arguments["StorageType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageType");
            if (vl_arguments["Storage"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Storage");
            if (vl_arguments["Injection"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Injection");
            if (vl_arguments["Withdrawal"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Withdrawal");
            if (vl_arguments["TechnicalCapacity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TechnicalCapacity");
            if (vl_arguments["ContractedCapacity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ContractedCapacity");
            if (vl_arguments["AvailableCapacity"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "AvailableCapacity");

            if (vl_arguments["StorageFacilityIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityIdentifierType");
            if (vl_arguments["StorageFacilityOperatorIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityOperatorIdentifierType");
            if (vl_arguments["StorageMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageMU");
            if (vl_arguments["InjectionMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "InjectionMU");
            if (vl_arguments["WithdrawalMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "WithdrawalMU");
            if (vl_arguments["TechnicalCapacityMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "TechnicalCapacityMU");
            if (vl_arguments["ContractedCapacityMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ContractedCapacityMU");
            if (vl_arguments["AvailableCapacityMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "AvailableCapacityMU");

            TDBExecResult res = new TDBExecResult(TDBExecError.ValidationUnsuccesful, "");
            res.setParamValidations(vl_arguments);

            int ID_StorageFacilityReport = 0;
            //int ID_StorageReport = 0;
            DateTime GasDayStart = DateTime.Now;
            DateTime GasDayEnd = DateTime.Now;
            string StorageFacilityIdentifier = "";
            string StorageFacilityOperatorIdentifier = "";
            string StorageType = "";
            double Storage = 0;
            double Injection = 0;
            double Withdrawal = 0;
            double TechnicalCapacity = 0;
            double ContractedCapacity = 0;
            double AvailableCapacity = 0;

            string StorageFacilityIdentifierType = "";
            string StorageFacilityOperatorIdentifierType = "";
            string StorageMU = "";
            string InjectionMU = "";
            string WithdrawalMU = "";
            string TechnicalCapacityMU = "";
            string ContractedCapacityMU = "";
            string AvailableCapacityMU = "";

            try
            {
                if (vl_arguments["ID_StorageFacilityReport"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_StorageFacilityReport"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_StorageFacilityReport"].ValueType == TVariantType.vtInt64) ID_StorageFacilityReport = vl_arguments["ID_StorageFacilityReport"].AsInt32;
                else res.ParamValidations["ID_StorageFacilityReport"].AsString = "Value type not recognized";

                /*if (vl_arguments["ID_StorageReport"].ValueType == TVariantType.vtInt16 ||
                    vl_arguments["ID_StorageReport"].ValueType == TVariantType.vtInt32 ||
                    vl_arguments["ID_StorageReport"].ValueType == TVariantType.vtInt64) ID_StorageReport = vl_arguments["ID_StorageReport"].AsInt32;
                else res.ParamValidations["ID_StorageReport"].AsString = "Value type not recognized";*/

                if (vl_arguments["GasDayStart"].ValueType == TVariantType.vtDateTime) GasDayStart = vl_arguments["GasDayStart"].AsDateTime;
                else res.ParamValidations["GasDayStart"].AsString = "Argument is not a datetime value";

                if (vl_arguments["GasDayEnd"].ValueType == TVariantType.vtDateTime) GasDayEnd = vl_arguments["GasDayEnd"].AsDateTime;
                else res.ParamValidations["GasDayEnd"].AsString = "Argument is not a datetime value";

                StorageFacilityIdentifier = vl_arguments["StorageFacilityIdentifier"].AsString;

                StorageFacilityOperatorIdentifier = vl_arguments["StorageFacilityOperatorIdentifier"].AsString;

                StorageType = vl_arguments["StorageType"].AsString;
                if (StorageType.Trim() == "") res.ParamValidations["StorageType"].AsString = "Empty value given";

                Storage = vl_arguments["Storage"].AsDouble;
                Injection = vl_arguments["Injection"].AsDouble;
                Withdrawal = vl_arguments["Withdrawal"].AsDouble;
                TechnicalCapacity = vl_arguments["TechnicalCapacity"].AsDouble;
                ContractedCapacity = vl_arguments["ContractedCapacity"].AsDouble;
                AvailableCapacity = vl_arguments["AvailableCapacity"].AsDouble;

                StorageFacilityIdentifierType = vl_arguments["StorageFacilityIdentifierType"].AsString;
                StorageFacilityOperatorIdentifierType = vl_arguments["StorageFacilityOperatorIdentifierType"].AsString;
                StorageMU = vl_arguments["StorageMU"].AsString;
                InjectionMU = vl_arguments["InjectionMU"].AsString;
                WithdrawalMU = vl_arguments["WithdrawalMU"].AsString;
                TechnicalCapacityMU = vl_arguments["TechnicalCapacityMU"].AsString;
                ContractedCapacityMU = vl_arguments["ContractedCapacityMU"].AsString;
                AvailableCapacityMU = vl_arguments["AvailableCapacityMU"].AsString;

                if (res.bInvalid()) return res;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.ValidationUnsuccesful, exc.Message);
            }

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_StorageFacilityReport;
                //vl_params.Add("@prm_ID_StorageReport").AsInt32 = ID_StorageReport;
                vl_params.Add("@prm_GasDayStart").AsDateTime = GasDayStart;
                vl_params.Add("@prm_GasDayEnd").AsDateTime = GasDayEnd;
                vl_params.Add("@prm_StorageFacilityIdentifier").AsString = StorageFacilityIdentifier;
                vl_params.Add("@prm_StorageFacilityOperatorIdentifier").AsString = StorageFacilityOperatorIdentifier;
                vl_params.Add("@prm_StorageType").AsString = StorageType;
                vl_params.Add("@prm_Storage").AsDouble = Storage;
                vl_params.Add("@prm_Injection").AsDouble = Injection;
                vl_params.Add("@prm_Withdrawal").AsDouble = Withdrawal;
                vl_params.Add("@prm_TechnicalCapacity").AsDouble = TechnicalCapacity;
                vl_params.Add("@prm_ContractedCapacity").AsDouble = ContractedCapacity;
                vl_params.Add("@prm_AvailableCapacity").AsDouble = AvailableCapacity;

                vl_params.Add("@prm_StorageFacilityIdentifierType").AsString = StorageFacilityIdentifierType;
                vl_params.Add("@prm_StorageFacilityOperatorIdentifierType").AsString = StorageFacilityOperatorIdentifierType;
                vl_params.Add("@prm_StorageMU").AsString = StorageMU;
                vl_params.Add("@prm_InjectionMU").AsString = InjectionMU;
                vl_params.Add("@prm_WithdrawalMU").AsString = WithdrawalMU;
                vl_params.Add("@prm_TechnicalCapacityMU").AsString = TechnicalCapacityMU;
                vl_params.Add("@prm_ContractedCapacityMU").AsString = ContractedCapacityMU;
                vl_params.Add("@prm_AvailableCapacityMU").AsString = AvailableCapacityMU;

                if (app.DB.Exec("update_REMIT_StorageFacilityReport", "REMIT_StorageFacilityReports", vl_params) < 1) throw new Exception("cannot insert StorageFacilityReport");
                app.DB.CommitTransaction();
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, exc.Message);
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult addREMITStorageParticipantActivityReport(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_StorageReport"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_StorageReport");
            if (vl_arguments["GasDayStart"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "GasDayStart");
            if (vl_arguments["GasDayEnd"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "GasDayEnd");
            if (vl_arguments["StorageFacilityIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityIdentifier");
            if (vl_arguments["StorageFacilityOperatorIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityOperatorIdentifier");
            if (vl_arguments["MarketParticipantIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MarketParticipantIdentifier");
            if (vl_arguments["Storage"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Storage");

            if (vl_arguments["StorageFacilityIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityIdentifierType");
            if (vl_arguments["StorageFacilityOperatorIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityOperatorIdentifierType");
            if (vl_arguments["MarketParticipantIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MarketParticipantIdentifierType");
            if (vl_arguments["StorageMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageMU");

            int ID_StorageReport = 0;
            DateTime GasDayStart = DateTime.Now;
            DateTime GasDayEnd = DateTime.Now;
            string StorageFacilityIdentifier = "";
            string StorageFacilityOperatorIdentifier = "";
            string MarketParticipantIdentifier = "";
            double Storage = 0;

            string StorageFacilityIdentifierType = "";
            string StorageFacilityOperatorIdentifierType = "";
            string MarketParticipantIdentifierType = "";
            string StorageMU = "";

            try
            {
                ID_StorageReport = vl_arguments["ID_StorageReport"].AsInt32;
                GasDayStart = vl_arguments["GasDayStart"].AsDateTime;
                GasDayEnd = vl_arguments["GasDayEnd"].AsDateTime;
                StorageFacilityIdentifier = vl_arguments["StorageFacilityIdentifier"].AsString;
                StorageFacilityOperatorIdentifier = vl_arguments["StorageFacilityOperatorIdentifier"].AsString;
                MarketParticipantIdentifier = vl_arguments["MarketParticipantIdentifier"].AsString;
                Storage = vl_arguments["Storage"].AsDouble;

                StorageFacilityIdentifierType = vl_arguments["StorageFacilityIdentifierType"].AsString;
                StorageFacilityOperatorIdentifierType = vl_arguments["StorageFacilityOperatorIdentifierType"].AsString;
                MarketParticipantIdentifierType = vl_arguments["MarketParticipantIdentifierType"].AsString;
                StorageMU = vl_arguments["StorageMU"].AsString;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.ValidationUnsuccesful, exc.Message);
            }

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_StorageReport").AsInt32 = ID_StorageReport;
                vl_params.Add("@prm_GasDayStart").AsDateTime = GasDayStart;
                vl_params.Add("@prm_GasDayEnd").AsDateTime = GasDayEnd;
                vl_params.Add("@prm_StorageFacilityIdentifier").AsString = StorageFacilityIdentifier;
                vl_params.Add("@prm_StorageFacilityOperatorIdentifier").AsString = StorageFacilityOperatorIdentifier;
                vl_params.Add("@prm_MarketParticipantIdentifier").AsString = MarketParticipantIdentifier;
                vl_params.Add("@prm_Storage").AsDouble = Storage;

                vl_params.Add("@prm_StorageFacilityIdentifierType").AsString = StorageFacilityIdentifierType;
                vl_params.Add("@prm_StorageFacilityOperatorIdentifierType").AsString = StorageFacilityOperatorIdentifierType;
                vl_params.Add("@prm_MarketParticipantIdentifierType").AsString = MarketParticipantIdentifierType;
                vl_params.Add("@prm_StorageMU").AsString = StorageMU;

                if (app.DB.Exec("insert_REMIT_StorageParticipantActivityReport", "REMIT_StorageParticipantActivityReports", vl_params) < 1) throw new Exception("cannot insert StorageMarketParticipantReport");
                app.DB.CommitTransaction();
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, exc.Message);
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult editREMITStorageParticipantActivityReport(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_StorageParticipantActivityReport"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_StorageParticipantActivityReport");
            //if (vl_arguments["ID_StorageReport"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_StorageReport");
            if (vl_arguments["GasDayStart"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "GasDayStart");
            if (vl_arguments["GasDayEnd"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "GasDayEnd");
            if (vl_arguments["StorageFacilityIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityIdentifier");
            if (vl_arguments["StorageFacilityOperatorIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityOperatorIdentifier");
            if (vl_arguments["MarketParticipantIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MarketParticipantIdentifier");
            if (vl_arguments["Storage"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "Storage");

            if (vl_arguments["StorageFacilityIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityIdentifierType");
            if (vl_arguments["StorageFacilityOperatorIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityOperatorIdentifierType");
            if (vl_arguments["MarketParticipantIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "MarketParticipantIdentifierType");
            if (vl_arguments["StorageMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageMU");

            int ID_StorageParticipantActivityReport = 0;
            //int ID_StorageReport = 0;
            DateTime GasDayStart = DateTime.Now;
            DateTime GasDayEnd = DateTime.Now;
            string StorageFacilityIdentifier = "";
            string StorageFacilityOperatorIdentifier = "";
            string MarketParticipantIdentifier = "";
            double Storage = 0;

            string StorageFacilityIdentifierType = "";
            string StorageFacilityOperatorIdentifierType = "";
            string MarketParticipantIdentifierType = "";
            string StorageMU = "";

            try
            {
                ID_StorageParticipantActivityReport = vl_arguments["ID_StorageParticipantActivityReport"].AsInt32;
                //ID_StorageReport = vl_arguments["ID_StorageReport"].AsInt32;
                GasDayStart = vl_arguments["GasDayStart"].AsDateTime;
                GasDayEnd = vl_arguments["GasDayEnd"].AsDateTime;
                StorageFacilityIdentifier = vl_arguments["StorageFacilityIdentifier"].AsString;
                StorageFacilityOperatorIdentifier = vl_arguments["StorageFacilityOperatorIdentifier"].AsString;
                MarketParticipantIdentifier = vl_arguments["MarketParticipantIdentifier"].AsString;
                Storage = vl_arguments["Storage"].AsDouble;

                StorageFacilityIdentifierType = vl_arguments["StorageFacilityIdentifierType"].AsString;
                StorageFacilityOperatorIdentifierType = vl_arguments["StorageFacilityOperatorIdentifierType"].AsString;
                MarketParticipantIdentifierType = vl_arguments["MarketParticipantIdentifierType"].AsString;
                StorageMU = vl_arguments["StorageMU"].AsString;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.ValidationUnsuccesful, exc.Message);
            }

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_StorageParticipantActivityReport;
                //vl_params.Add("@prm_ID_StorageReport").AsInt32 = ID_StorageReport;
                vl_params.Add("@prm_GasDayStart").AsDateTime = GasDayStart;
                vl_params.Add("@prm_GasDayEnd").AsDateTime = GasDayEnd;
                vl_params.Add("@prm_StorageFacilityIdentifier").AsString = StorageFacilityIdentifier;
                vl_params.Add("@prm_StorageFacilityOperatorIdentifier").AsString = StorageFacilityOperatorIdentifier;
                vl_params.Add("@prm_MarketParticipantIdentifier").AsString = MarketParticipantIdentifier;
                vl_params.Add("@prm_Storage").AsDouble = Storage;

                vl_params.Add("@prm_StorageFacilityIdentifierType").AsString = StorageFacilityIdentifierType;
                vl_params.Add("@prm_StorageFacilityOperatorIdentifierType").AsString = StorageFacilityOperatorIdentifierType;
                vl_params.Add("@prm_MarketParticipantIdentifierType").AsString = MarketParticipantIdentifierType;
                vl_params.Add("@prm_StorageMU").AsString = StorageMU;

                if (app.DB.Exec("update_REMIT_StorageParticipantActivityReport", "REMIT_StorageParticipantActivityReports", vl_params) < 1) throw new Exception("cannot insert StorageMarketParticipantReport");
                app.DB.CommitTransaction();
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, exc.Message);
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult addREMITStorageUnavailabilityReport(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_StorageReport"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_StorageReport");
            if (vl_arguments["UnavailabilityNotificationTimestamp"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailabilityNotificationTimestamp");
            if (vl_arguments["StorageFacilityIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityIdentifier");
            if (vl_arguments["StorageFacilityOperatorIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityOperatorIdentifier");
            if (vl_arguments["UnavailabilityStart"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailabilityStart");
            if (vl_arguments["UnavailabilityEnd"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailabilityEnd");
            if (vl_arguments["UnavailabilityEndFlag"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailabilityEndFlag");
            if (vl_arguments["UnavailableVolume"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailableVolume");
            if (vl_arguments["UnavailableInjection"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailableInjection");
            if (vl_arguments["UnavailableWithdrawal"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailableWithdrawal");
            if (vl_arguments["UnavailabilityType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailabilityType");
            if (vl_arguments["UnavailabilityDescription"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailabilityDescription");

            if (vl_arguments["StorageFacilityIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityIdentifierType");
            if (vl_arguments["StorageFacilityOperatorIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityOperatorIdentifierType");
            if (vl_arguments["UnavailableVolumeMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailableVolumeMU");
            if (vl_arguments["UnavailableInjectionMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailableInjectionMU");
            if (vl_arguments["UnavailableWithdrawalMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailableWithdrawalMU");

            int ID_StorageReport = 0;
            DateTime UnavailabilityNotificationTimestamp = DateTime.Now;
            string StorageFacilityIdentifier = "";
            string StorageFacilityOperatorIdentifier = "";
            DateTime UnavailabilityStart = DateTime.Now;
            DateTime UnavailabilityEnd = DateTime.Now;
            string UnavailabilityEndFlag = "";
            double UnavailableVolume = 0;
            double UnavailableInjection = 0;
            double UnavailableWithdrawal = 0;
            string UnavailabilityType = "";
            string UnavailabilityDescription = "";

            string StorageFacilityIdentifierType = "";
            string StorageFacilityOperatorIdentifierType = "";
            string UnavailableVolumeMU = "";
            string UnavailableInjectionMU = "";
            string UnavailableWithdrawalMU = "";

            try
            {
                ID_StorageReport = vl_arguments["ID_StorageReport"].AsInt32;
                UnavailabilityNotificationTimestamp = vl_arguments["UnavailabilityNotificationTimestamp"].AsDateTime;
                StorageFacilityIdentifier = vl_arguments["StorageFacilityIdentifier"].AsString;
                StorageFacilityOperatorIdentifier = vl_arguments["StorageFacilityOperatorIdentifier"].AsString;
                UnavailabilityStart = vl_arguments["UnavailabilityStart"].AsDateTime;
                UnavailabilityEnd = vl_arguments["UnavailabilityEnd"].AsDateTime;
                UnavailabilityEndFlag = vl_arguments["UnavailabilityEndFlag"].AsString;
                UnavailableVolume = vl_arguments["UnavailableVolume"].AsDouble;
                UnavailableInjection = vl_arguments["UnavailableInjection"].AsDouble;
                UnavailableWithdrawal = vl_arguments["UnavailableWithdrawal"].AsDouble;
                UnavailabilityType = vl_arguments["UnavailabilityType"].AsString;
                UnavailabilityDescription = vl_arguments["UnavailabilityDescription"].AsString;

                StorageFacilityIdentifierType = vl_arguments["StorageFacilityIdentifierType"].AsString;
                StorageFacilityOperatorIdentifierType = vl_arguments["StorageFacilityOperatorIdentifierType"].AsString;
                UnavailableVolumeMU = vl_arguments["UnavailableVolumeMU"].AsString;
                UnavailableInjectionMU = vl_arguments["UnavailableInjectionMU"].AsString;
                UnavailableWithdrawalMU = vl_arguments["UnavailableWithdrawalMU"].AsString;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.ValidationUnsuccesful, exc.Message);
            }

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID_StorageReport").AsInt32 = ID_StorageReport;
                vl_params.Add("@prm_UnavailabilityNotificationTimestamp").AsDateTime = UnavailabilityNotificationTimestamp;
                vl_params.Add("@prm_StorageFacilityIdentifier").AsString = StorageFacilityIdentifier;
                vl_params.Add("@prm_StorageFacilityOperatorIdentifier").AsString = StorageFacilityOperatorIdentifier;
                vl_params.Add("@prm_UnavailabilityStart").AsDateTime = UnavailabilityStart;
                vl_params.Add("@prm_UnavailabilityEnd").AsDateTime = UnavailabilityEnd;
                vl_params.Add("@prm_UnavailabilityEndFlag").AsString = UnavailabilityEndFlag;
                vl_params.Add("@prm_UnavailableVolume").AsDouble = UnavailableVolume;
                vl_params.Add("@prm_UnavailableInjection").AsDouble = UnavailableInjection;
                vl_params.Add("@prm_UnavailableWithdrawal").AsDouble = UnavailableWithdrawal;
                vl_params.Add("@prm_UnavailabilityType").AsString = UnavailabilityType;
                vl_params.Add("@prm_UnavailabilityDescription").AsString = UnavailabilityDescription;

                vl_params.Add("@prm_StorageFacilityIdentifierType").AsString = StorageFacilityIdentifierType;
                vl_params.Add("@prm_StorageFacilityOperatorIdentifierType").AsString = StorageFacilityOperatorIdentifierType;
                vl_params.Add("@prm_UnavailableVolumeMU").AsString = UnavailableVolumeMU;
                vl_params.Add("@prm_UnavailableInjectionMU").AsString = UnavailableInjectionMU;
                vl_params.Add("@prm_UnavailableWithdrawalMU").AsString = UnavailableWithdrawalMU;

                if (app.DB.Exec("insert_REMIT_StorageUnavailabilityReport", "REMIT_StorageUnavailabilityReports", vl_params) < 1) throw new Exception("cannot insert StorageUnavailabilityReport");
                app.DB.CommitTransaction();
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, exc.Message);
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

        public TDBExecResult editREMITStorageUnavailabilityReport(TVariantList vl_arguments)
        {
            if (vl_arguments["ID_StorageUnavailabilityReport"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_StorageUnavailabilityReport");
            //if (vl_arguments["ID_StorageReport"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "ID_StorageReport");
            if (vl_arguments["UnavailabilityNotificationTimestamp"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailabilityNotificationTimestamp");
            if (vl_arguments["StorageFacilityIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityIdentifier");
            if (vl_arguments["StorageFacilityOperatorIdentifier"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityOperatorIdentifier");
            if (vl_arguments["UnavailabilityStart"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailabilityStart");
            if (vl_arguments["UnavailabilityEnd"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailabilityEnd");
            if (vl_arguments["UnavailabilityEndFlag"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailabilityEndFlag");
            if (vl_arguments["UnavailableVolume"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailableVolume");
            if (vl_arguments["UnavailableInjection"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailableInjection");
            if (vl_arguments["UnavailableWithdrawal"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailableWithdrawal");
            if (vl_arguments["UnavailabilityType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailabilityType");
            if (vl_arguments["UnavailabilityDescription"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailabilityDescription");

            if (vl_arguments["StorageFacilityIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityIdentifierType");
            if (vl_arguments["StorageFacilityOperatorIdentifierType"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "StorageFacilityOperatorIdentifierType");
            if (vl_arguments["UnavailableVolumeMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailableVolumeMU");
            if (vl_arguments["UnavailableInjectionMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailableInjectionMU");
            if (vl_arguments["UnavailableWithdrawalMU"] == null) return new TDBExecResult(TDBExecError.ArgumentMissing, "UnavailableWithdrawalMU");

            int ID_StorageUnavailabilityReport = 0;
            //int ID_StorageReport = 0;
            DateTime UnavailabilityNotificationTimestamp = DateTime.Now;
            string StorageFacilityIdentifier = "";
            string StorageFacilityOperatorIdentifier = "";
            DateTime UnavailabilityStart = DateTime.Now;
            DateTime UnavailabilityEnd = DateTime.Now;
            string UnavailabilityEndFlag = "";
            double UnavailableVolume = 0;
            double UnavailableInjection = 0;
            double UnavailableWithdrawal = 0;
            string UnavailabilityType = "";
            string UnavailabilityDescription = "";

            string StorageFacilityIdentifierType = "";
            string StorageFacilityOperatorIdentifierType = "";
            string UnavailableVolumeMU = "";
            string UnavailableInjectionMU = "";
            string UnavailableWithdrawalMU = "";
            
            try
            {
                ID_StorageUnavailabilityReport = vl_arguments["ID_StorageUnavailabilityReport"].AsInt32;
                //ID_StorageReport = vl_arguments["ID_StorageReport"].AsInt32;
                UnavailabilityNotificationTimestamp = vl_arguments["UnavailabilityNotificationTimestamp"].AsDateTime;
                StorageFacilityIdentifier = vl_arguments["StorageFacilityIdentifier"].AsString;
                StorageFacilityOperatorIdentifier = vl_arguments["StorageFacilityOperatorIdentifier"].AsString;
                UnavailabilityStart = vl_arguments["UnavailabilityStart"].AsDateTime;
                UnavailabilityEnd = vl_arguments["UnavailabilityEnd"].AsDateTime;
                UnavailabilityEndFlag = vl_arguments["UnavailabilityEndFlag"].AsString;
                UnavailableVolume = vl_arguments["UnavailableVolume"].AsDouble;
                UnavailableInjection = vl_arguments["UnavailableInjection"].AsDouble;
                UnavailableWithdrawal = vl_arguments["UnavailableWithdrawal"].AsDouble;
                UnavailabilityType = vl_arguments["UnavailabilityType"].AsString;
                UnavailabilityDescription = vl_arguments["UnavailabilityDescription"].AsString;

                StorageFacilityIdentifierType = vl_arguments["StorageFacilityIdentifierType"].AsString;
                StorageFacilityOperatorIdentifierType = vl_arguments["StorageFacilityOperatorIdentifierType"].AsString;
                UnavailableVolumeMU = vl_arguments["UnavailableVolumeMU"].AsString;
                UnavailableInjectionMU = vl_arguments["UnavailableInjectionMU"].AsString;
                UnavailableWithdrawalMU = vl_arguments["UnavailableWithdrawalMU"].AsString;
            }
            catch (Exception exc)
            {
                return new TDBExecResult(TDBExecError.ValidationUnsuccesful, exc.Message);
            }

            if (!app.DB.BeginTransaction()) return new TDBExecResult(TDBExecError.UnspecifiedDatabaseError, "");
            try
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_ID").AsInt32 = ID_StorageUnavailabilityReport;
                //vl_params.Add("@prm_ID_StorageReport").AsInt32 = ID_StorageReport;
                vl_params.Add("@prm_UnavailabilityNotificationTimestamp").AsDateTime = UnavailabilityNotificationTimestamp;
                vl_params.Add("@prm_StorageFacilityIdentifier").AsString = StorageFacilityIdentifier;
                vl_params.Add("@prm_StorageFacilityOperatorIdentifier").AsString = StorageFacilityOperatorIdentifier;
                vl_params.Add("@prm_UnavailabilityStart").AsDateTime = UnavailabilityStart;
                vl_params.Add("@prm_UnavailabilityEnd").AsDateTime = UnavailabilityEnd;
                vl_params.Add("@prm_UnavailabilityEndFlag").AsString = UnavailabilityEndFlag;
                vl_params.Add("@prm_UnavailableVolume").AsDouble = UnavailableVolume;
                vl_params.Add("@prm_UnavailableInjection").AsDouble = UnavailableInjection;
                vl_params.Add("@prm_UnavailableWithdrawal").AsDouble = UnavailableWithdrawal;
                vl_params.Add("@prm_UnavailabilityType").AsString = UnavailabilityType;
                vl_params.Add("@prm_UnavailabilityDescription").AsString = UnavailabilityDescription;

                vl_params.Add("@prm_StorageFacilityIdentifierType").AsString = StorageFacilityIdentifierType;
                vl_params.Add("@prm_StorageFacilityOperatorIdentifierType").AsString = StorageFacilityOperatorIdentifierType;
                vl_params.Add("@prm_UnavailableVolumeMU").AsString = UnavailableVolumeMU;
                vl_params.Add("@prm_UnavailableInjectionMU").AsString = UnavailableInjectionMU;
                vl_params.Add("@prm_UnavailableWithdrawalMU").AsString = UnavailableWithdrawalMU;

                if (app.DB.Exec("update_REMIT_StorageUnavailabilityReport", "REMIT_StorageUnavailabilityReports", vl_params) < 1) throw new Exception("cannot insert StorageUnavailabilityReport");
                app.DB.CommitTransaction();
            }
            catch (Exception exc)
            {
                app.DB.RollBackTransaction();
                return new TDBExecResult(TDBExecError.SQLExecutionError, exc.Message);
            }

            return new TDBExecResult(TDBExecError.Success, "");
        }

    }
}