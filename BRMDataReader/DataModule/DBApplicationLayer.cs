using System;
using System.Collections;
using System.Data;
using System.Globalization;
using Business.Common;

namespace Business.DataModule
{
    //  result structure for the extended version of the exec function
    public enum TDBExecError
    {
        Success,
        ProcedureNotFound,
        ArgumentMissing,
        ValidationUnsuccesful,
        SQLExecutionError,
        UnspecifiedDatabaseError
    }

    public struct TDBExecResult
    {
        public TDBExecError ErrorCode;
        public string Message;
        public int RowsModified;
        public TVariantList IDs;
        public TVariantList ParamValidations;

        public TDBExecResult(TDBExecError ErrorCode, string Message)
        {
            this.ErrorCode = ErrorCode;
            this.Message = Message;
            this.RowsModified = 0;
            this.IDs = null;
            this.ParamValidations = null;
        }

        public TDBExecResult(TDBExecError ErrorCode, string Message, int RowsModified)
        {
            this.ErrorCode = ErrorCode;
            this.Message = Message;
            this.RowsModified = RowsModified;
            this.IDs = null;
            this.ParamValidations = null;
        }

        public void setParamValidations(TVariantList vl_params)
        {
            ParamValidations = new TVariantList();
            for (int i = 0; i < vl_params.Count; i++) ParamValidations.Add(vl_params[i].Name).AsString = "";
        }

        public bool bInvalid()
        {
            if (ParamValidations == null) return false;

            bool b = false;
            int i = -1;
            while (!b && i < ParamValidations.Count - 1)
            {
                i++;
                if (ParamValidations[i].AsString != "") b = true;
            }

            return b;
        }
    }

	//  Delegate function types for custom data module procedures
	public delegate DataSet TDBSelectFunction(TVariantList vl_params);
	public delegate int TDBExecFunction(TVariantList vl_params);
    public delegate TDBExecResult TDBExecFunctionExtended(TVariantList vl_params);

	public class TDBFunction
	{
		public string SQLID;
		public string DataCollection;
		public object Function;
	}

    public enum TDBFunctionType
    {
        DBFunctionDontCare,
        DBFunctionSelect,
        DBFunctionExec,
        DBFunctionExecExtended
    }

	/********************************************************
	*	@Class:			TDBApplicationLayer					*
	*	@Author:		Dan-Alexandru AVASI					*
	*	@Date:			20051230							*
	*	@Description:	The DataModule Application Layer.	*
	*					Implements custom data processing	*
	*					routines specific for an app.		*
	*					Inherits the DataLayer.				*
	********************************************************/
	public class TDBApplicationLayer : TDBDataLayer
	{
		private ArrayList FApplicationFunctions;
        public TDBFunction[] ApplicationFunctions
        {
            get
            {
                TDBFunction[] result = new TDBFunction[FApplicationFunctions.Count];
                for (int i = 0; i < FApplicationFunctions.Count; i++)
                {
                    result[i] = (TDBFunction)FApplicationFunctions[i];
                }
                return result;
            }
        }

        public string[] ApplicationCollections
        {
            get
            {
                ArrayList str_collections = new ArrayList();
                for (int i = 0; i < FApplicationFunctions.Count; i++)
                {
                    string str_DataCollection = ((TDBFunction)FApplicationFunctions[i]).DataCollection;
                    if (!str_collections.Contains(str_DataCollection)) str_collections.Add(str_DataCollection);
                }

                string[] result = new string[str_collections.Count];
                for (int i = 0; i < result.Length; i++) result[i] = (string)str_collections[i];

                return result;
            }
        }

        public string[] ApplicationSQLs(string DataCollection, TDBFunctionType FunctionType = TDBFunctionType.DBFunctionDontCare)
        {
            ArrayList str_sqls = new ArrayList();
            for (int i = 0; i < FApplicationFunctions.Count; i++)
            {
                string str_DataCollection = ((TDBFunction)FApplicationFunctions[i]).DataCollection;
                string str_sql = ((TDBFunction)FApplicationFunctions[i]).SQLID;
                if (str_DataCollection == DataCollection)
                {
                    switch (FunctionType)
                    {
                        case TDBFunctionType.DBFunctionDontCare:
                            str_sqls.Add(str_sql);
                            break;
                        case TDBFunctionType.DBFunctionSelect:
                            if (((TDBFunction)FApplicationFunctions[i]).Function is TDBSelectFunction) str_sqls.Add(str_sql);
                            break;
                        case TDBFunctionType.DBFunctionExec:
                            if (((TDBFunction)FApplicationFunctions[i]).Function is TDBExecFunction) str_sqls.Add(str_sql);
                            break;
                        case TDBFunctionType.DBFunctionExecExtended:
                            if (((TDBFunction)FApplicationFunctions[i]).Function is TDBExecFunctionExtended) str_sqls.Add(str_sql);
                            break;
                    }
                }
            }

            string[] result = new string[str_sqls.Count];
            for (int i = 0; i < str_sqls.Count; i++) result[i] = (string)str_sqls[i];
            return result;
        }

		/****************************************************************************
		*	@Function:		TDBApplicationLayer.TDBApplicationLayer					*
		*	@Date:			20051230												*
		*	@Author:		Dan-Alexandru AVASI										*
		*	@Arguments:		ConnectionType: TDBConnectionType						*
		*					XMLFile: string											*
		*	@Result:		none													*
		*	@Description:	Creates the instance and passes the parameters to the	*
		*					underlying layers.										*
		*	@History:		20051230 -	Created										*
		****************************************************************************/
		public TDBApplicationLayer(TDBConnectionType ConnectionType, string XMLFile, TVariantList vl_params): base(ConnectionType, XMLFile, vl_params)
		{
			//
			// TODO: Add constructor logic here
			//			

			FApplicationFunctions = new ArrayList();
			RegisterFunctions();
		}

        public TDBApplicationLayer(TDBConnectionType ConnectionType, string XMLFile)
            : this(ConnectionType, XMLFile, null)
        {
        }

		//  Clears the functions array
		public void Clear()
		{
			FApplicationFunctions.Clear();
		}

		/****************************************************************************
		*	@Function:		TDBApplicationLayer.FindFunction						*
		*	@Date:			20051230												*
		*	@Author:		Dan-Alexandru AVASI										*
		*	@Arguments:		SQLID: string											*
		*					DataCollection: string									*
		*	@Result:		int														*
		*	@Description:	Searches for the custom function identified and returns	*
		*					its index if found.										*
		*	@History:		20051230 -	Created										*
		****************************************************************************/
		public int FindFunction(string SQLID, string DataCollection)
		{
			bool b = false;
			int i = -1;

			while(!b && i < FApplicationFunctions.Count - 1)
			{
				i++;

				if((FApplicationFunctions[i] as TDBFunction).SQLID.ToUpper() == SQLID.ToUpper() &&
					(FApplicationFunctions[i] as TDBFunction).DataCollection.ToUpper() == DataCollection.ToUpper()) b = true;
			}

			if(b) return i; else return -1;
		}

		/****************************************************************************
		*	@Function:		TDBApplicationLayer.AddDBFunction						*
		*	@Date:			20051230												*
		*	@Author:		Dan-Alexandru AVASI										*
		*	@Arguments:		SQLID: string											*
		*					DataCollection: string									*
		*					Function: object										*
		*	@Result:		void													*
		*	@Description:	Adds a new custom function into array					*
		*	@History:		20051230 -	Created										*
		****************************************************************************/
		public void AddDBFunction(string SQLID, string DataCollection, object Function)
		{
			TDBFunction func = new TDBFunction();
			func.SQLID = SQLID;
			func.DataCollection = DataCollection;
			func.Function = Function;				 

			FApplicationFunctions.Add(func);
		}

		/****************************************************************************
		*	@Function:		TDBApplicationLayer.Select								*
		*	@Date:			20051230												*
		*	@Author:		Dan-Alexandru AVASI										*
		*	@Arguments:		str_ID: string											*
		*					str_DataCollection: string								*
		*					vl_params: TVariantList									*
		*	@Result:		DataSet													*
		*	@Description:	Searches for a custom function. If found it will be		*
		*					called with the given parameters. If not, parameters	*
		*					will be sent to the underlying layer.					*
		*	@History:		20051230 -	Created										*
		****************************************************************************/
		public override DataSet Select(string str_ID, string str_DataCollection, TVariantList vl_params)
		{
			try
			{
				int Index = FindFunction(str_ID, str_DataCollection);
				if(Index > -1) return ((FApplicationFunctions[Index] as TDBFunction).Function as TDBSelectFunction)(vl_params);
				else return base.Select(str_ID, str_DataCollection, vl_params);
			}
			catch(Exception exc)
			{
				return null;
			}
		}
			
		/****************************************************************************
		*	@Function:		TDBApplicationLayer.Exec								*
		*	@Date:			20051230												*
		*	@Author:		Dan-Alexandru AVASI										*
		*	@Arguments:		str_ID: string											*
		*					str_DataCollection: string								*
		*					vl_params: TVariantList									*
		*	@Result:		int														*
		*	@Description:	Searches for a custom function. If found it will be		*
		*					called with the given parameters. If not, parameters	*
		*					will be sent to the underlying layer.					*
		*	@History:		20051230 -	Created										*
		****************************************************************************/
		public override int Exec(string str_ID, string str_DataCollection, TVariantList vl_params)
		{
			try
			{
				int Index = FindFunction(str_ID, str_DataCollection);
				if(Index > -1) return ((FApplicationFunctions[Index] as TDBFunction).Function as TDBExecFunction)(vl_params);
				else return base.Exec(str_ID, str_DataCollection, vl_params);
			}
			catch(Exception exc)
			{
				return -1;
			}
		}

        public TDBExecResult ExecExtended(string str_ID, string str_DataCollection, TVariantList vl_params)
        {
            try
            {
                int idx = FindFunction(str_ID, str_DataCollection);
                if (idx == -1) return new TDBExecResult() { ErrorCode = TDBExecError.ProcedureNotFound, RowsModified = 0, IDs = null, ParamValidations = null };
                if (!((FApplicationFunctions[idx] as TDBFunction).Function is TDBExecFunctionExtended)) return new TDBExecResult() { ErrorCode = TDBExecError.ProcedureNotFound, RowsModified = 0, IDs = null, ParamValidations = null };

                return ((FApplicationFunctions[idx] as TDBFunction).Function as TDBExecFunctionExtended)(vl_params);
            }
            catch (Exception exc)
            {
                return new TDBExecResult() { ErrorCode = TDBExecError.UnspecifiedDatabaseError, RowsModified = 0, IDs = null, ParamValidations = null };
            }
        }
			
		///////////////////////////////////////////////////////
		//	User Defined methods below
		///////////////////////////////////////////////////////

		protected void RegisterFunctions()
		{
			//AddDBFunction("get_ProduseAverage", "Produse", new TDBSelectFunction(Get_DataSet_ProductsAverage));
		}

		////////////////////////////////////////////////////////////////
		// Utilities Routines

		//////////////////////////////////////////////////////////
		//  Utility method. Checks the validity of a dataset
		public bool ValidDS(DataSet ds)
		{
			if(ds == null) return false;
			if(ds.Tables.Count == 0) return false;

			return true;
		}

		//////////////////////////////////////////////////////////
		//  Utility method. Checks the validity of a dataset and its rows
		public bool ValidDSRows(DataSet ds)
		{
			if(ds == null) return false;
			if(ds.Tables.Count == 0) return false;
			if(ds.Tables[0].Rows.Count == 0) return false;

			return true;
		}

	}
}
