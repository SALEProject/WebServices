using System;
using System.Data;
using System.Data.SqlClient;
using Business.Common;

namespace Business.DataModule
{
	//////////////////////////////////////////////////////////////
	//	Class:			TDBAbstractConnection					//
	//	Author:			Dan-Alexandru AVASI						//
	//	Date:			20050907								//
	//	Description:	abstract connection class definition	//
	//////////////////////////////////////////////////////////////
	public abstract class TDBAbstractConnection
	{
		public TDBAbstractConnection()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	
		public abstract bool Connect();
        public abstract void Disconnect();
		public abstract DataSet Select(string str_sql, TVariantList var_params);
		public abstract int Exec(string str_sql, TVariantList var_params);
		public abstract int GetIdentity();

		//  Transaction routines
		public abstract bool BeginTransaction();
		public abstract void CommitTransaction();
		public abstract void RollBackTransaction();
	}
}
