using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business;
using Business.Common;
using Business.DataModule;
using Business.JSONObjects;

namespace BRMDataReader
{
    public class FormsDBModule : AbstractDBModule
    {
        private TBusiness app;
        private Session session;

        public FormsDBModule(TBusiness app, Session session) : base()
        {
            this.app = app;
            this.session = session;
            if (app == null) return;

            string serverPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
            if (!serverPath.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                serverPath += System.IO.Path.DirectorySeparatorChar;
            app.DB.MergeXML(serverPath + "Modules" + System.IO.Path.DirectorySeparatorChar + "FormsDBModule.xml");

            app.DB.AddDBFunction("getFormData", "Forms", new TDBSelectFunction(_getFormData));
        }

        public DataSet _getFormData(TVariantList vl_arguments)
        {
            return new DataSet("structured");
        }

        public JSONForm getFormData(TVariantList vl_arguments)
        {
            JSONForm form = new JSONForm();
           
            if (vl_arguments["ID_Procedure"] == null) return form;
            if (vl_arguments["ID_Form"] == null && vl_arguments["Step"] == null) return form;

            int ID_Procedure = vl_arguments["ID_Procedure"].AsInt32;

            int ID_Form = 0;
            if (vl_arguments["ID_Form"] != null) ID_Form = vl_arguments["ID_Form"].AsInt32;
            else if (vl_arguments["Step"] != null)
            {
                TVariantList vl_params = new TVariantList();
                vl_params.Add("@prm_Step").AsInt32 = vl_arguments["Step"].AsInt32;
                DataSet ds = app.DB.Select("select_FormbyStep", "Forms", vl_params);
                if (app.DB.ValidDSRows(ds)) ID_Form = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
            }

            if (ID_Form <= 0) return form;

            int ID_FormField = 0;
            if (vl_arguments["ID_FormField"] != null) ID_FormField = vl_arguments["ID_FormField"].AsInt32;

            form.LoadForm(ID_Procedure, ID_Form, 0, app.DB);
            return form;
        }


    }
}