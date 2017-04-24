using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using Business.Common;

namespace Business.DataModule
{
	//////////////////////////////////////////////////////////////
	//	Class:			TDBMSSQLConnection						//
	//	Author:			Dan-Alexandru AVASI						//
	//	Date:			20050908								//
	//	Description:	implements low level access routines	//
	//					to an Microsoft SQL Server.				//
	//////////////////////////////////////////////////////////////
	public class TDBMSSQLConnection : TDBAbstractConnection
	{
		private string srv_ConnectionString;
		private SqlConnection srv_Connection;
		private SqlTransaction srv_Transaction;

		/****************************************************************************
		*	@Function:		TDBMSSQLConnection.TDBMSSQLConnection					*
		*	@Date:			20050908												*
		*	@Author:		Dan-Alexandru AVASI										*
		*	@Arguments:		str_Server: string										*
		*					str_Database: string									*
		*					str_User: string										*
		*					str_Password: string									*
		*	@Result:		none													*
		*	@Description:	CONSTRUCTOR. Initializes the database connection		*
		*	@History:		20050908 -	Created										*
		****************************************************************************/
		public TDBMSSQLConnection(string str_Server, string str_Database, string str_User, string str_Password, bool Pooling = false)
		{
			//
			// TODO: Add constructor logic here
			//

			srv_ConnectionString =	"server="		+ str_Server + 
									";database="	+ str_Database + 
									";uid="			+ str_User +
									";pwd="			+ str_Password;
            if (Pooling) srv_ConnectionString += "; Pooling=true";

			//srv_Connection = new SqlConnection(str_ConnectionString);
			//srv_Connection.Open();
		}

		/****************************************************************************
		*	@Function:		TDBMSSQLConnection.TDBMSSQLConnection - overloaded		*
		*	@Date:			20051213												*
		*	@Author:		Dan-Alexandru AVASI										*
		*	@Arguments:		XMLFile: string											*
		*	@Result:		none													*
		*	@Description:	CONSTRUCTOR. Initializes the database connection based	*
		*					on parameters founded in the XMLFile					*
		*	@History:		20051213 -	Created										*
		****************************************************************************/
		public TDBMSSQLConnection(string XMLFile, TVariantList vl_params)
		{
			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(XMLFile);

				string str_Server = "";
				string str_Database = "";
				string str_User = "";
				string str_Password = "";
                bool Pooling = false;

				for(int i = 0; i < doc.DocumentElement.ChildNodes.Count; i++)
				{
					string str_name = doc.DocumentElement.ChildNodes[i].Name;
					switch(str_name.ToUpper())
					{
						case "SERVER":
							str_Server = doc.DocumentElement.ChildNodes[i].InnerText;
							break;
						case "DATABASE":
							str_Database = doc.DocumentElement.ChildNodes[i].InnerText;
							break;
						case "USER":
							str_User = doc.DocumentElement.ChildNodes[i].InnerText;
							break;
						case "PASSWORD":
							str_Password = doc.DocumentElement.ChildNodes[i].InnerText;
							break;
                        case "POOLING":
                            Pooling = true;
                            break;
					}
				}

                //  check params
                if (vl_params != null)
                {
                    if (vl_params[str_Server] != null && vl_params[str_Server].ValueType == TVariantType.vtString) str_Server = vl_params[str_Server].AsString;
                    if (vl_params[str_Database] != null && vl_params[str_Database].ValueType == TVariantType.vtString) str_Database = vl_params[str_Database].AsString;
                    if (vl_params[str_User] != null && vl_params[str_User].ValueType == TVariantType.vtString) str_User = vl_params[str_User].AsString;
                    if (vl_params[str_Password] != null && vl_params[str_Password].ValueType == TVariantType.vtString) str_Password = vl_params[str_Password].AsString;
                }

				srv_ConnectionString =	"server="		+ str_Server + 
					";database="	+ str_Database + 
					";uid="			+ str_User +
					";pwd="			+ str_Password;
                if (Pooling) srv_ConnectionString += "; Pooling=true";
                //srv_ConnectionString += "; Context Connection=true";
                srv_ConnectionString += "; MultipleActiveResultSets=True";

				//srv_Connection = new SqlConnection(str_ConnectionString);
				//srv_Connection.Open();
			}
			catch(Exception exc)
			{
				throw exc;
			}
		}

        public TDBMSSQLConnection(string XMLFile): this(XMLFile, null)
        {
        }

		public string ConnectionString
		{
			get
			{
				return srv_ConnectionString;
			}
			set
			{
				srv_ConnectionString = value;
				//srv_Connection.ConnectionString = value;
			}
		}

		/****************************************************************************
		*	@Function:		TDBMSSQLConnection.Connect								*
		*	@Date:			20050908												*
		*	@Author:		Dan-Alexandru AVASI										*
		*	@Arguments:		none													*
		*	@Result:		Boolean													*
		*	@Description:	Tries to connect. returns false if unsuccesfull			*
		*	@History:		20050908 -	Created										*
		****************************************************************************/
		public override bool Connect()
		{
			try
			{
                srv_Connection = new SqlConnection(srv_ConnectionString);
                srv_Connection.Open();

                return srv_Connection.State == ConnectionState.Open || srv_Connection.State == ConnectionState.Connecting;

				//srv_Connection.Open();
				//return true;
			}
			catch
			{
				return false;
			}
		}

        public override void Disconnect()
        {
            if (srv_Connection != null)
            {
                try
                {
                    srv_Connection.Close();
                    srv_Connection.Dispose();
                    srv_Connection = null;
                }
                catch (Exception exc)
                {
                }
            }
        }

		/****************************************************************************
		*	@Function:		TDBMSSQLConnection.Select								*
		*	@Date:			20050908												*
		*	@Author:		Dan-Alexandru AVASI										*
		*	@Arguments:		str_sql: string	-	the sql statement					*
		*					var_params		-	sql parameters						*
		*	@Result:		DataSet													*
		*	@Description:	Executes a SELECT statement on the server, returning the*
		*					result into a dataset.									*
		*	@History:		20050908 -	Created										*
		****************************************************************************/
		public override DataSet Select(string str_sql, TVariantList var_params)
		{
			try
			{              
                //  replace query parts first
                if (var_params != null)
                    for (int i = 0; i < var_params.Count; i++)
                    {
                        TVariant var = var_params[i];
                        if (var.Name[0] == '#') str_sql = str_sql.Replace(var.Name, var.AsString);
                    }

                //using (SqlConnection srv_Connection = new SqlConnection(srv_ConnectionString))
                {
                    using (SqlCommand srv_cmd = new SqlCommand(str_sql, srv_Connection))
                    {
                        srv_cmd.CommandType = CommandType.Text;
                        if (srv_Transaction != null) srv_cmd.Transaction = srv_Transaction;

                        if (var_params != null)
                        {
                            for (int i = 0; i < var_params.Count; i++)
                            {
                                string str_prm_name;
                                SqlDbType sdt_prm_type;
                                Object obj_prm_value;

                                str_prm_name = var_params[i].Name;
                                sdt_prm_type = var_params[i].DbType;
                                obj_prm_value = var_params[i].DbValue;

                                if (str_prm_name[0] != '#') srv_cmd.Parameters.Add(str_prm_name, sdt_prm_type).Value = obj_prm_value;
                            }

                        }

                        //if (srv_Connection.State != ConnectionState.Open) srv_Connection.Open();
                        if (srv_Connection.State != ConnectionState.Open && srv_Connection.State != ConnectionState.Connecting) srv_Connection.Open();

                        using (SqlDataAdapter srv_adapter = new SqlDataAdapter(srv_cmd))
                        {
                            DataSet cds = new DataSet();
                            srv_adapter.Fill(cds);
                            //srv_Connection.Close();

                            return cds;
                        }
                    }
                }
			}
			catch(Exception exc)
			{
                if (TExceptionManager.ExcManager != null)
                {
                    TExceptionManager.ExcManager.WriteToLog("The following sql failed:");
                    TExceptionManager.ExcManager.WriteToLog(str_sql);
                    TExceptionManager.ExcManager.ProcessException(exc);
                }
				return null;
			}
		}

		/****************************************************************************
		*	@Function:		TDBMSSQLConnection.Exec									*
		*	@Date:			20050908												*
		*	@Author:		Dan-Alexandru AVASI										*
		*	@Arguments:		str_sql: string	-	the sql statement					*
		*					var_params		-	sql parameters						*
		*	@Result:		int														*
		*	@Description:	Executes an EXEC statement on the server, returning the	*
		*					number of rows modified.								*
		*	@History:		20050908 -	Created										*
		****************************************************************************/
		public override int Exec(string str_sql, TVariantList var_params)
		{
			try
			{
                //using (SqlConnection srv_Connection = new SqlConnection(srv_ConnectionString))
                {
                    using (SqlCommand srv_cmd = new SqlCommand(str_sql, srv_Connection))
                    {
                        srv_cmd.CommandType = CommandType.Text;
                        if (srv_Transaction != null) srv_cmd.Transaction = srv_Transaction;

                        if (var_params != null)
                        {
                            int i;

                            for (i = 0; i < var_params.Count; i++)
                            {
                                string str_prm_name;
                                SqlDbType sdt_prm_type;
                                Object obj_prm_value;

                                str_prm_name = var_params[i].Name;
                                sdt_prm_type = var_params[i].DbType;
                                obj_prm_value = var_params[i].DbValue;

                                if (obj_prm_value is double && double.IsNaN((double)obj_prm_value)) srv_cmd.Parameters.Add(str_prm_name, sdt_prm_type).Value = DBNull.Value;
                                else srv_cmd.Parameters.Add(str_prm_name, sdt_prm_type).Value = obj_prm_value;
                            }

                        }

                        if (srv_Connection.State != ConnectionState.Open && srv_Connection.State != ConnectionState.Connecting) srv_Connection.Open();

                        try
                        {
                            return srv_cmd.ExecuteNonQuery();
                        }
                        finally
                        {
                            //srv_Connection.Close();
                        }
                    }
                }
			}
			catch(Exception exc)
			{
                if (TExceptionManager.ExcManager != null)
                {
                    TExceptionManager.ExcManager.WriteToLog("The following sql failed:");
                    TExceptionManager.ExcManager.WriteToLog(str_sql);
                    TExceptionManager.ExcManager.ProcessException(exc);
                }
                return -1;
			}
		}
	
		/****************************************************************************
		*	@Function:		TDBMSSQLConnection.GetIdentity							*
		*	@Date:			20050908												*
		*	@Author:		Dan-Alexandru AVASI										*
		*	@Arguments:		none													*
		*	@Result:		int														*
		*	@Description:	Return the ID of the last row modified					*
		*	@History:		20050908 -	Created										*
		****************************************************************************/
		public override int GetIdentity()
		{
            //using (SqlConnection srv_Connection = new SqlConnection(srv_ConnectionString))
            {
                using (SqlCommand srv_cmd = new SqlCommand("SELECT @@IDENTITY", srv_Connection))
                {
                    srv_cmd.CommandType = CommandType.Text;
                    if (srv_Transaction != null) srv_cmd.Transaction = srv_Transaction;

                    if (srv_Connection.State != ConnectionState.Open && srv_Connection.State != ConnectionState.Connecting) srv_Connection.Open();
                    int Identity = 0;
                    Identity = Convert.ToInt32(srv_cmd.ExecuteScalar());
                    //srv_Connection.Close();

                    return Identity;
                }
            }
		}

		//  Transaction routines

		public override bool BeginTransaction()
		{
            try
            {
                srv_Transaction = srv_Connection.BeginTransaction();
                return true;
            }
            catch (Exception exc)
            {
                if (TExceptionManager.ExcManager != null)
                {
                    TExceptionManager.ExcManager.WriteToLog("Transaction cannot be initiated");
                    TExceptionManager.ExcManager.ProcessException(exc);
                }
                return false;
            }
		}

		public override void CommitTransaction()
		{
			if (srv_Transaction == null) return;

			srv_Transaction.Commit();
            srv_Transaction.Dispose();
			srv_Transaction = null;			
		}

		public override void RollBackTransaction()
		{
			if (srv_Transaction == null) return;

			srv_Transaction.Rollback();
            srv_Transaction.Dispose();
            srv_Transaction = null;			
		}

	}
}
