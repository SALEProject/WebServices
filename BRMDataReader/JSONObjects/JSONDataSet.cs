using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Business.JSONObjects
{
    public class JSONColumn
    {
        public string Name;
        public string Type;
    }

    public class JSONDataRow
    {
        public DataRow row;

        public JSONDataRow(DataRow row)
        {
            this.row = row;
        }

        internal class JSONDataRowConverter : JavaScriptConverter
        {
            public override IEnumerable<Type> SupportedTypes
            {
                get { return new Type[] { typeof(DataRow) }; }
            }

            public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
            {
                DataRow dataRow = obj as DataRow;
                Dictionary<string, object> propValues = new Dictionary<string, object>();

                if (dataRow != null)
                {
                    foreach (DataColumn dc in dataRow.Table.Columns)
                    {
                        propValues.Add(dc.ColumnName, dataRow[dc]);
                    }
                }

                return propValues;
            }
        }

        public string Serialize()
        {
            /*JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<JavaScriptConverter> converters = new List<JavaScriptConverter>();
            converters.Add(new JSONDataRowConverter());
            serializer.RegisterConverters(converters);
            return serializer.Serialize(this.row);*/

            return JSONInterface.Serialize(this.row);
        }
    }

    public class JSONDataSet
    {
        public string Name;
        public JSONColumn[] Columns;
        public DataRow[] Rows;

        public JSONDataSet(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
            {
                this.Name = "";
                Columns = new JSONColumn[0];
                Rows = new DataRow[0];
            }

            this.Name = ds.Tables[0].TableName;

            this.Columns = new JSONColumn[ds.Tables[0].Columns.Count];
            for (int i = 0; i < this.Columns.Length; i++)
                this.Columns[i] = new JSONColumn() { Name = ds.Tables[0].Columns[i].ColumnName, Type = ds.Tables[0].Columns[i].DataType.Name };

            this.Rows = new DataRow[ds.Tables[0].Rows.Count];
            for (int i = 0; i < this.Rows.Length; i++)
            {
                //var row = new { Name
                this.Rows[i] = ds.Tables[0].Rows[i];
            }
        }

        internal class JSONDataRowConverter : JavaScriptConverter
        {
            public override IEnumerable<Type> SupportedTypes
            {
                get { return new Type[] { typeof(DataRow) }; }
            }

            public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
            {
                DataRow dataRow = obj as DataRow;
                Dictionary<string, object> propValues = new Dictionary<string, object>();

                if (dataRow != null)
                {
                    foreach (DataColumn dc in dataRow.Table.Columns)
                    {
                        propValues.Add(dc.ColumnName, dataRow[dc]);
                    }
                }

                return propValues;
            }
        }

        public string Serialize()
        {
            /*JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<JavaScriptConverter> converters = new List<JavaScriptConverter>();
            converters.Add(new JSONDataRowConverter());
            serializer.RegisterConverters(converters);
            return serializer.Serialize(this);*/
            return JSONInterface.Serialize(this);
        }

    }
}