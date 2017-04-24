using System;
using System.Data;
using System.Xml;
using Business.Common;

namespace Business.DataModule
{

	public enum TDBConnectionType
	{
		con_NONE = 0,
		con_MSSQL
	}

	public enum TDBSQLType
	{
		sql_NONE = 0,
		sql_SELECT,
		sql_INSERT,
		sql_DELETE,
		sql_UPDATE
	}

	public struct TDBSQL
	{
		public string ID;
		public string SQL;
		public TDBSQLType SQLType;
		public string DataCollection;

		public TDBSQL(string ID, string SQL, TDBSQLType SQLType, string DataCollection)
		{
			this.ID = ID;
			this.SQL = SQL;
			this.SQLType = SQLType;
			this.DataCollection = DataCollection;
		}
	}
    
	//////////////////////////////////////////////////////////////
	//	Class:			TDBDataLayer							//
	//	Author:			Dan-Alexandru AVASI						//
	//	Date:			20050908								//
	//	Description:	SQL storage mechanism and execution.	//
	//////////////////////////////////////////////////////////////
	public class TDBDataLayer
	{
		private TDBAbstractConnection sql_Connection;
		private TDBConnectionType sql_ConnectionType;

		private DataSet SQLS;

		/****************************************************************************
		*	Function:		TDBDataLayer											*
		*	Date:			20050908												*
		*	Author:			Dan-Alexandru AVASI										*
		*	Arguments:		ConnectionType: TDBConnectionType						*
		*	Result:			none													*
		*	Description:	CONSTRUCTOR												*
		*	History:		20050908 -	Created										*
		****************************************************************************/
		public TDBDataLayer(TDBConnectionType ConnectionType, string XMLFile, TVariantList vl_params)
		{
			//
			// TODO: Add constructor logic here
			//

			sql_Connection = null;
			sql_ConnectionType = ConnectionType;

			SQLS = new DataSet("SQLS");

			switch(sql_ConnectionType)
			{
				case TDBConnectionType.con_NONE:
					break;
				case TDBConnectionType.con_MSSQL:
					sql_Connection = new TDBMSSQLConnection(XMLFile, vl_params);
					break;
			}
		}

        public TDBDataLayer(TDBConnectionType ConnectionType, string XMLFile): this(ConnectionType, XMLFile, null)
        {
        }

		/****************************************************************************
		*	Function:		TDBDataLayer.GenerateSQLSStructure						*
		*	Date:			20050913												*
		*	Author:			Dan-Alexandru AVASI										*
		*	Arguments:		none													*
		*	Result:			void													*
		*	Description:	Creates the SQLS dataset's structure					*
		*	History:		20050913 -	Created										*
		****************************************************************************/
		protected void GenerateSQLSStructure(string str_DataCollection)
		{
			SQLS.Tables.Add(str_DataCollection);

			SQLS.Tables[str_DataCollection].Columns.Add("ID", Type.GetType("System.String"));
			SQLS.Tables[str_DataCollection].Columns.Add("SQLType", Type.GetType("System.Int32"));
			SQLS.Tables[str_DataCollection].Columns.Add("SQL", Type.GetType("System.String"));
		}

        public string[] SQLCollections
        {
            get
            {
                string[] result = new string[SQLS.Tables.Count];
                for (int i = 0; i < result.Length; i++)
                    result[i] = SQLS.Tables[i].TableName;

                return result;
            }
        }

        public string[] SQLs(string DataCollection)
        {
            if (SQLS.Tables[DataCollection] == null) return new string[0];

            string[] result = new string[SQLS.Tables[DataCollection].Rows.Count];
            for (int i = 0; i < result.Length; i++)
                result[i] = SQLS.Tables[DataCollection].Rows[i]["ID"].ToString();

            return result;
        }

        /****************************************************************************
        *	Function:		TDBDataLayer.GetSQLIndex								*
        *	Date:			20050913												*
        *	Author:			Dan-Alexandru AVASI										*
        *	Arguments:		ID: string												*
        *					DataCollection: string									*
        *	Result:			integer - the row index of the sql if it is founded		*
        *	Description:	Searches the required SQL by ID and DataCollection		*
        *	History:		20050913 -	Created										*
        ****************************************************************************/
        public int GetSQLIndex(string ID, string DataCollection)
		{
			try
			{
				if(SQLS.Tables[DataCollection] != null)
				{
					bool b = false;
					int i = -1;

					while(!b && i < SQLS.Tables[DataCollection].Rows.Count - 1)
					{
						i++;

						if(SQLS.Tables[DataCollection].Rows[i]["ID"].ToString().ToUpper() == ID.ToUpper()) b = true;
					}

					if(b) return i;
					else return -1;
				}
				else return -1;
			}
			catch
			{
				return -1;
			}
		}

		/****************************************************************************
		*	Function:		TDBDataLayer.GetSQL										*
		*	Date:			20050913												*
		*	Author:			Dan-Alexandru AVASI										*
		*	Arguments:		ID: string												*
		*					DataCollection: string									*
		*	Result:			string - the SQL itself									*
		*	Description:	Searches the required SQL by ID and DataCollection and	*
		*					returns the sql string if founded						*
		*	History:		20050913 -	Created										*
		****************************************************************************/
		public string GetSQL(string ID, string DataCollection)
		{
			try
			{
				int Index = GetSQLIndex(ID, DataCollection);

				if(Index > -1) return SQLS.Tables[DataCollection].Rows[Index]["SQL"].ToString();
				else return "";
			}
			catch
			{
				return "";
			}
		}

		/****************************************************************************
		*	Function:		TDBDataLayer.GetSQLType									*
		*	Date:			20050913												*
		*	Author:			Dan-Alexandru AVASI										*
		*	Arguments:		ID: string												*
		*					DataCollection: string									*
		*	Result:			TDBSQLType - the type of the sql						*
		*	Description:	Searches the required SQL by ID and DataCollection and	*
		*					returns the sql type if founded							*
		*	History:		20050913 -	Created										*
		****************************************************************************/
		public TDBSQLType GetSQLType(string ID, string DataCollection)
		{
			try
			{
				int Index = GetSQLIndex(ID, DataCollection);

				if(Index > -1) return (TDBSQLType)Convert.ToInt32(SQLS.Tables[DataCollection].Rows[Index]["SQLType"]);
				else return TDBSQLType.sql_NONE;
			}
			catch
			{
				return TDBSQLType.sql_NONE;
			}
		}

		/****************************************************************************
		*	Function:		TDBDataLayer.GetSQLCollection							*
		*	Date:			20050913												*
		*	Author:			Dan-Alexandru AVASI										*
		*	Arguments:		DataCollection: string									*
		*	Result:			array of TDBSQL - the queries list belonging to the		*
		*					indicated Collection									*
		*	Description:	Retrieves a list of queries belonging to the specified	*
		*					DataCollection if any is found.							*
		*	History:		20050913 -	Created										*
		****************************************************************************/
		public TDBSQL[] GetSQLCollection(string DataCollection)
		{
			int i, Count = 0;
			if(SQLS.Tables[DataCollection] != null)
			{
				Count = SQLS.Tables[DataCollection].Rows.Count;
				TDBSQL[] sqls = new TDBSQL[Count];				
				for(i = 0; i < Count; i++)
				{
					sqls[i].ID = SQLS.Tables[DataCollection].Rows[i]["ID"].ToString();
					sqls[i].DataCollection = DataCollection;
					sqls[i].SQLType = (TDBSQLType)Convert.ToInt32(SQLS.Tables[DataCollection].Rows[i]["SQLType"]);
					sqls[i].SQL = SQLS.Tables[DataCollection].Rows[i]["SQL"].ToString();
				}

				return sqls;
			}
			else return null;
		}

		/****************************************************************************
		*	Function:		TDBDataLayer.ClearSQL									*
		*	Date:			20050913												*
		*	Author:			Dan-Alexandru AVASI										*
		*	Arguments:		none													*
		*	Result:			void													*
		*	Description:	Clears the SQLS structure								*
		*	History:		20050913 -	Created										*
		****************************************************************************/
		public void ClearSQLS()
		{
			SQLS.Tables[0].Clear();
		}

		/****************************************************************************
		*	Function:		TDBDataLayer.MergeXML									*
		*	Date:			20050913												*
		*	Author:			Dan-Alexandru AVASI										*
		*	Arguments:		str_XMLFile: string										*
		*	Result:			void													*
		*	Description:	Loads data from specified XML file into SQLS			*
		*	History:		20050913 -	Created										*
		****************************************************************************/
		public void MergeXML(string str_XMLFile)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(str_XMLFile);
			
			int Count = doc.FirstChild.ChildNodes.Count;
			int i;
			for(i = 0; i < Count; i++)
			{
				string str_DataCollection = doc.FirstChild.ChildNodes[i].Name;
				if(SQLS.Tables[str_DataCollection] == null) GenerateSQLSStructure(str_DataCollection);
			}

			SQLS.ReadXml(str_XMLFile);
		}

        public bool Connect()
        {
            return sql_Connection.Connect();
        }

        public void Disconnect()
        {
            sql_Connection.Disconnect();
        }

		/****************************************************************************
		*	Function:		TDBDataLayer.Select										*
		*	Date:			20050913												*
		*	Author:			Dan-Alexandru AVASI										*
		*	Arguments:		str_ID: string											*
		*					str_DataCollection: string								*
		*					vl_params: TVariantList									*
		*	Result:			DataSet													*
		*	Description:	Executes a SELECT query on SQL server indicated throug	*
		*					str_ID and str_DataCollection and with vl_params as		*
		*					parameters												*
		*	History:		20050913 -	Created										*
		****************************************************************************/
		public virtual DataSet Select(string str_ID, string str_DataCollection, TVariantList vl_params)
		{
			try
			{
				string str_sql = GetSQL(str_ID, str_DataCollection);
				if(str_sql != "") return sql_Connection.Select(str_sql, vl_params);
				else return null;
			}
			catch (Exception exc)
			{
                if (TExceptionManager.ExcManager != null) TExceptionManager.ExcManager.ProcessException(exc);
                return null;
			}
		}

		/****************************************************************************
		*	Function:		TDBDataLayer.Exec										*
		*	Date:			20050913												*
		*	Author:			Dan-Alexandru AVASI										*
		*	Arguments:		str_ID: string											*
		*					str_DataCollection: string								*
		*					vl_params: TVariantList									*
		*	Result:			int - the number of records affected					*
		*	Description:	Executes an INSERT, DELETE or UPDATE query on SQL server*
		*					indicated throug str_ID and str_DataCollection and with *
		*					vl_params as parameters									*
		*	History:		20050913 -	Created										*
		****************************************************************************/
		public virtual int Exec(string str_ID, string str_DataCollection, TVariantList vl_params)
		{
			try
			{
				string str_sql = GetSQL(str_ID, str_DataCollection);
				if(str_sql != "") return sql_Connection.Exec(str_sql, vl_params);
				else return -1;
			}
			catch (Exception exc)
			{
                if (TExceptionManager.ExcManager != null) TExceptionManager.ExcManager.ProcessException(exc);
                return -1;
			}
		}

		public int GetIdentity()
		{
			return sql_Connection.GetIdentity();
		}

		public bool BeginTransaction()
		{
			return sql_Connection.BeginTransaction();
		}

		public void CommitTransaction()
		{
			sql_Connection.CommitTransaction();
		}

		public void RollBackTransaction()
		{
			sql_Connection.RollBackTransaction();
		}

	}
}
