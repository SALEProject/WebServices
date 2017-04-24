using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace BRMDataReader
{
    public class JSONDataSetSerializer
    {
        public JSONDataSetSerializer()
        {
        }

        public string Serialize(object value)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();

            List<JavaScriptConverter> converters = new List<JavaScriptConverter>();

            if (value != null)
            {
                Type type = value.GetType();
                if (type == typeof(DataTable) || type == typeof(DataRow) || type == typeof(DataSet))
                {
                    converters.Add(new JSONDataRowConverter());
                    converters.Add(new JSONDataTableConverter());
                    converters.Add(new JSONDataSetConverter());
                }

                if (converters.Count > 0)
                    ser.RegisterConverters(converters);
            }
            ser.MaxJsonLength = 20971520;
            return ser.Serialize(value);
        }

        public object Deserialize(string jsonText, Type valueType)
        {
            // *** Have to use Reflection with a 'dynamic' non constant type instance
            JavaScriptSerializer ser = new JavaScriptSerializer();


            object result = ser.GetType()
                               .GetMethod("Deserialize")
                               .MakeGenericMethod(valueType)
                              .Invoke(ser, new object[1] { jsonText });
            return result;
        }

    }


    internal class JSONDataTableConverter : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new Type[] { typeof(DataTable) }; }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type,
                                           JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }


        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            DataTable table = obj as DataTable;

            // *** result 'object'
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (table != null)
            {
                // *** We'll represent rows as an array/listType
                List<object> rows = new List<object>();

                foreach (DataRow row in table.Rows)
                {
                    rows.Add(row);  // Rely on DataRowConverter to handle
                }
                result["Rows"] = rows;

                return result;
            }

            return new Dictionary<string, object>();
        }
    }

    internal class JSONDataRowConverter : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new Type[] { typeof(DataRow) }; }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type,
                                           JavaScriptSerializer serializer)
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

    internal class JSONDataSetConverter : JavaScriptConverter
    {
        public override IEnumerable<Type> SupportedTypes
        {
            get { return new Type[] { typeof(DataSet) }; }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type,
                                           JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            DataSet dataSet = obj as DataSet;
            Dictionary<string, object> tables = new Dictionary<string, object>();

            if (dataSet != null)
            {
                foreach (DataTable dt in dataSet.Tables)
                {
                    tables.Add(dt.TableName, dt);
                }
            }

            return tables;
        }
    }

}