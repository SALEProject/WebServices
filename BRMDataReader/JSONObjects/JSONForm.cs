using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Business.Common;

namespace Business.JSONObjects
{
    public class JSONForm
    {
        public string Type = "";
        public string DataType = "";
        public string Validation = "";
        public bool isMandatory = false;
        public string Dependency = "";
        public string Field = "";
        public string Values = "";
        public string Html = "";
        public string Style = "";
        public string Data = "";
        public JSONForm[] Items = null;

        public void LoadForm(int ID_Procedure, int ID_Form, int ID_Field, Business.DataModule.TDBApplicationLayer DB)
        {
            if (ID_Form <= 0) return;

            TVariantList vl_params = null;
            if (ID_Field != 0)
            {
                vl_params = new TVariantList();
                vl_params.Add("@prm_ID_Procedure").AsInt32 = ID_Procedure;
                vl_params.Add("@prm_ID_Form").AsInt32 = ID_Form;
                vl_params.Add("@prm_ID").AsInt32 = ID_Field;
                DataSet ds_field = DB.Select("select_Field", "FormFields", vl_params);

                if (DB.ValidDSRows(ds_field))
                {
                    this.Type = ds_field.Tables[0].Rows[0]["Type"].ToString();
                    this.DataType = ds_field.Tables[0].Rows[0]["DataType"].ToString();
                    this.Validation = ds_field.Tables[0].Rows[0]["Validation"].ToString();
                    this.isMandatory = Convert.ToBoolean(ds_field.Tables[0].Rows[0]["isMandatory"]);
                    this.Dependency = ds_field.Tables[0].Rows[0]["Dependency"].ToString();
                    this.Field = ds_field.Tables[0].Rows[0]["Field"].ToString();
                    this.Values = ds_field.Tables[0].Rows[0]["Values"].ToString();
                    this.Html = ds_field.Tables[0].Rows[0]["Html"].ToString();
                    this.Style = ds_field.Tables[0].Rows[0]["Style"].ToString();
                    this.Data = ds_field.Tables[0].Rows[0]["Data"].ToString();
                }
            }

            //  extract items
            vl_params = new TVariantList();
            vl_params.Add("@prm_ID_Form").AsInt32 = ID_Form;
            vl_params.Add("@prm_ID_Parent").AsInt32 = ID_Field;
            DataSet ds_items = DB.Select("select_Items", "FormFields", vl_params);
            if (DB.ValidDS(ds_items))
            {
                this.Items = new JSONForm[ds_items.Tables[0].Rows.Count];
                for (int i = 0; i < ds_items.Tables[0].Rows.Count; i++)
                {
                    JSONForm frm = new JSONForm();
                    frm.LoadForm(ID_Procedure, ID_Form, Convert.ToInt32(ds_items.Tables[0].Rows[i]["ID"]), DB);
                    this.Items[i] = frm;
                }
            }
        }

        public string Serialize()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //List<JavaScriptConverter> converters = new List<JavaScriptConverter>();
            //converters.Add(new JSONDataRowConverter());
            //serializer.RegisterConverters(converters);
            return serializer.Serialize(this);
        }
    }
}