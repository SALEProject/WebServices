/********************************************************
*														*
*	@PROJECT:	Churchill - DataModule Engine			*
*				OPIS - Common Module					*
*	@AUTHOR:	Dan-Alexandru AVASI						*
*	@EMAIL:		office@mstn.ro							*
*	@COPYRIGHT:	Copyright (c) 1998 - 2003, 4E Software	*
*				www.4esoft.com							*
*	@HISTORY:	2004.09.10 - Created					*
*				2004.09.12 - Added Value property		*
*				2004.09.13 - Added Name & ValueType		*
*							properties in CVariant.		*
*							Added CVariantList			*
*				2004.09.14 - Added ValueType in CVariant*
*							automatic conversions for Db*
*				2004.09.27 - Added new Add in CVarList	*
*				2005.09.19 - Added new routines for		*
*							custom variants creation.	*
*				2005.09.20 - Added automatically type	*
*							recognition when the Value	*
*							is assigned					*
*														*
********************************************************/
using System;
using System.Data;

namespace Business.Common
{

	public enum TVariantType {vtBoolean, vtByte, vtChar, vtDateTime, vtDecimal, vtDouble,
								vtInt16, vtInt32, vtInt64, vtSByte, vtSingle, vtString,
								vtUInt16, vtUInt32, vtUInt64, vtObject, vtImage};

	/// <summary>
	/// CVariant represents a class capable of working with different types of data
	/// similar to Delphi.
	/// </summary>  
	public class TVariant
	{
		// private declarations

		private string			FName;
		private Object			FValue;
		private TVariantType	FValueType;

		// public declarations

		public TVariant()
		{
			//
			// TODO: Add constructor logic here
			//
			FValue = new Object();
		}

		public TVariant(string Name)
		{
			FValue = new Object();
			FName = Name;
		}

		// properties

		public string Name
		{
			get
			{
				return FName;
			}
			set
			{
				FName = value;
			}
		}

        //---------------------------------------------------------------------------------
        //  Added on 20070317
        public TVariantType ValueType
        {
            get
            {
                return FValueType;
            }
            set
            {
                FValueType = value;
            }
        }
        //---------------------------------------------------------------------------------

        public bool AsBoolean
		{
			get
			{
				try
				{
					return Convert.ToBoolean(FValue);
				}
				catch
				{
					return false;
				}
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtBoolean;
			}
		}

		public byte AsByte
		{
			get
			{
				try
				{
					return Convert.ToByte(FValue);
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtByte;
			}
		}
		
		public char AsChar
		{
			get
			{
				try
				{
					return Convert.ToChar(FValue);
				}
				catch
				{
					return '\u0000';
				}
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtChar;
			}
		}

		public DateTime AsDateTime
		{
			get
			{
				try
				{
					return Convert.ToDateTime(FValue);
				}
					/*catch
					{
						return ;
					}*/
				finally
				{
				}
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtDateTime;
			}
		}

		public decimal AsDecimal
		{
			get
			{
				try
				{
					return Convert.ToDecimal(FValue);
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtDecimal;
			}
		}

		public double AsDouble
		{
			get
			{
				try
				{
					return Convert.ToDouble(FValue);
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtDouble;
			}
		}

		public short AsInt16
		{
			get
			{
				try
				{
					return Convert.ToInt16(FValue);
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtInt16;
			}
		}

		public int AsInt32
		{
			get
			{
				try
				{
					return Convert.ToInt32(FValue);
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtInt32;
			}
		}

		public long AsInt64
		{
			get
			{
				try
				{
					return Convert.ToInt64(FValue);
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtInt64;
			}
		}

		public sbyte AsSByte
		{
			get
			{
				try
				{
					return Convert.ToSByte(FValue);
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtSByte;
			}
		}

		public float AsSingle
		{
			get
			{
				try
				{
					return Convert.ToSingle(FValue);
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtSingle;
			}
		}

		public string AsString
		{
			get
			{
				try
				{
					return Convert.ToString(FValue);
				}
				catch
				{
					return "";
				}
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtString;
			}
		}

		public ushort AsUInt16
		{
			get
			{
				try
				{
					return Convert.ToUInt16(FValue);
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtUInt16;
			}
		}

		public uint AsUInt32
		{
			get
			{
				try
				{
					return Convert.ToUInt32(FValue);
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtUInt32;
			}
		}

		public ulong AsUInt64
		{
			get
			{
				try
				{
					return Convert.ToUInt64(FValue);
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtUInt64;
			}
		}

		public Object Value
		{
			get
			{
				return FValue;
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtObject;

				//	try to find the actual type of the variant
				if(FValue is bool) FValueType = TVariantType.vtBoolean;
				if(FValue is byte) FValueType = TVariantType.vtByte;
				if(FValue is char) FValueType = TVariantType.vtChar;
				if(FValue is DateTime) FValueType = TVariantType.vtDateTime;
				if(FValue is decimal) FValueType = TVariantType.vtDecimal;
				if(FValue is double) FValueType = TVariantType.vtDouble;
				if(FValue is Int16) FValueType = TVariantType.vtInt16;
				if(FValue is Int32) FValueType = TVariantType.vtInt32;
				if(FValue is Int64) FValueType = TVariantType.vtInt64;
				if(FValue is SByte) FValueType = TVariantType.vtSByte;
				if(FValue is Single) FValueType = TVariantType.vtSingle;
				if(FValue is string) FValueType = TVariantType.vtString;
				if(FValue is UInt16) FValueType = TVariantType.vtUInt16;
				if(FValue is UInt32) FValueType = TVariantType.vtUInt32;
				if(FValue is UInt64) FValueType = TVariantType.vtUInt64;
				if(FValue is byte[]) FValueType = TVariantType.vtImage;
			}
		}

		public byte[] AsImage
		{
			get
			{
				return (FValue as byte[]);
			}
			set
			{
				FValue = value;
				FValueType = TVariantType.vtImage;
			}

		}

        public object AsObject
        {
            get
            {
                try
                {
                    return FValue;
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                FValue = value;
                FValueType = TVariantType.vtObject;
            }
        }
 
		public SqlDbType DbType
		{
			get
			{
				switch(FValueType)
				{
					case TVariantType.vtBoolean	: return SqlDbType.Bit;
					case TVariantType.vtByte	: return SqlDbType.TinyInt;
					case TVariantType.vtChar	: return SqlDbType.SmallInt;
					case TVariantType.vtDateTime: return SqlDbType.DateTime;
					case TVariantType.vtDecimal	: return SqlDbType.Decimal;
					case TVariantType.vtDouble	: return SqlDbType.Float;
					case TVariantType.vtInt16	: return SqlDbType.SmallInt;
					case TVariantType.vtInt32	: return SqlDbType.Int;
					case TVariantType.vtInt64	: return SqlDbType.BigInt;
					case TVariantType.vtObject	: return SqlDbType.Variant;
					//case CVariantType.vtSByte	: return SqlDbType.;
					case TVariantType.vtSingle	: return SqlDbType.Real;
					case TVariantType.vtString	: return SqlDbType.VarChar;
					case TVariantType.vtUInt16	: return SqlDbType.Int;
					case TVariantType.vtUInt32	: return SqlDbType.Int;
					case TVariantType.vtUInt64	: return SqlDbType.Int;
					case TVariantType.vtImage	: return SqlDbType.Image;
					default						: return SqlDbType.Variant;
				}
			}
		}

		public Object DbValue
		{
			get
			{
				switch(FValueType)
				{
					case TVariantType.vtBoolean	: return AsBoolean;
					case TVariantType.vtByte	: return AsByte;
					case TVariantType.vtChar	: return AsChar;
					case TVariantType.vtDateTime: return AsDateTime;
					case TVariantType.vtDecimal	: return AsDecimal;
					case TVariantType.vtDouble	: return AsDouble;
					case TVariantType.vtInt16	: return AsInt16;
					case TVariantType.vtInt32	: return AsInt32;
					case TVariantType.vtInt64	: return AsInt64;
					case TVariantType.vtObject	: return Value;
					case TVariantType.vtSByte	: return AsSByte;
					case TVariantType.vtSingle	: return AsSingle;
					case TVariantType.vtString	: return AsString;
					case TVariantType.vtUInt16	: return AsUInt16;
					case TVariantType.vtUInt32	: return AsUInt32;
					case TVariantType.vtUInt64	: return AsUInt64;
					case TVariantType.vtImage	: return AsImage;
                    default: if (FValue == null) return DBNull.Value; else return Value;
				}
			}
		}

		///////////////////////////////////////////////////////////////////////////////////
		// Added by Dan-Alexandru AVASI													 //			
		// Date: 20050919																 //
		// Description: new routines to permit creation of customized variants in one go //
		///////////////////////////////////////////////////////////////////////////////////

		public static TVariant varBoolean(string Name, bool Value)
		{
			TVariant v = new TVariant(Name);
			v.AsBoolean = Value;
			return v;
		}

		public static TVariant varByte(string Name, byte Value)
		{
			TVariant v = new TVariant(Name);
			v.AsByte = Value;
			return v;
		}

		public static TVariant varChar(string Name, char Value)
		{
			TVariant v = new TVariant(Name);
			v.AsChar = Value;
			return v;
		}

		public static TVariant varDateTime(string Name, DateTime Value)
		{
			TVariant v = new TVariant(Name);
			v.AsDateTime = Value;
			return v;
		}

		public static TVariant varDecimal(string Name, decimal Value)
		{
			TVariant v = new TVariant(Name);
			v.AsDecimal = Value;
			return v;
		}

		public static TVariant varDouble(string Name, double Value)
		{
			TVariant v = new TVariant(Name);
			v.AsDouble = Value;
			return v;
		}

		public static TVariant varInt16(string Name, Int16 Value)
		{
			TVariant v = new TVariant(Name);
			v.AsInt16 = Value;
			return v;
		}

		public static TVariant varInt32(string Name, Int32 Value)
		{
			TVariant v = new TVariant(Name);
			v.AsInt32 = Value;
			return v;
		}

		public static TVariant varInt64(string Name, Int64 Value)
		{
			TVariant v = new TVariant(Name);
			v.AsInt64 = Value;
			return v;
		}

		public static TVariant varSByte(string Name, SByte Value)
		{
			TVariant v = new TVariant(Name);
			v.AsSByte = Value;
			return v;
		}

		public static TVariant varSingle(string Name, Single Value)
		{
			TVariant v = new TVariant(Name);
			v.AsSingle = Value;
			return v;
		}

		public static TVariant varString(string Name, string Value)
		{
			TVariant v = new TVariant(Name);
			v.AsString = Value;
			return v;
		}

		public static TVariant varUInt16(string Name, UInt16 Value)
		{
			TVariant v = new TVariant(Name);
			v.AsUInt16 = Value;
			return v;
		}

		public static TVariant varUInt32(string Name, UInt32 Value)
		{
			TVariant v = new TVariant(Name);
			v.AsUInt32 = Value;
			return v;
		}

		public static TVariant varUInt64(string Name, UInt64 Value)
		{
			TVariant v = new TVariant(Name);
			v.AsUInt64 = Value;
			return v;
		}

		//	End addition on 20050919
		///////////////////////////////////////////////////////////////////////////////////

        //---------------------------------------------------------------------------------
        //  Added on 20070317
        public TVariant Clone()
        {
            TVariant res = new TVariant(FName);
            res.ValueType = FValueType;
            res.Value = FValue;

            return res;
        }
        //---------------------------------------------------------------------------------
	}

	/// <summary>
	/// CVariantList - dinamic array of CVariant. It comprises basic functions for 
	///					addition, deletion, search by index and name
	/// </summary>
	public class TVariantList
	{
		private TVariant[] FVariants;

		public TVariantList()
		{
			FVariants = new TVariant[0];
		}

        public TVariantList Clone()
        {
            TVariantList res = new TVariantList();
            for(int i = 0; i <  this.Count; i++) res.Add(this[i].Clone());
            return res;
        }

		public void Add(TVariant Value)
		{
			int int_len;
			int i;

			int_len = FVariants.Length;
			TVariant[] var_temp = new TVariant[int_len + 1];
			for(i = 0; i < FVariants.Length; i++) var_temp[i] = FVariants[i];
			var_temp[int_len] = Value;
			FVariants = var_temp;
		}

		public TVariant Add(string Name)
		{
			TVariant var = new TVariant(Name);

			Add(var);

			return var;
		}

        public void Insert(TVariant Value, int Index)
        {
            int int_len;
            int i;

            if ((Index == -1) || (Index > FVariants.Length)) return;

            int_len = FVariants.Length;
            TVariant[] var_temp = new TVariant[int_len + 1];
            for (i = 0; i < Index; i++) 
            {
                var_temp[i] = FVariants[i];
            }
            
            var_temp[Index] = Value;

            for (i = Index; i < var_temp.Length - 1; i++)
            {
                var_temp[i + 1] = FVariants[i];
            }

            FVariants = var_temp;
        }

        public TVariant Insert(string Name, int Index)
        {
            TVariant var = new TVariant(Name);

            Insert(var, Index);

            return var;
        }

		public void Delete(int Index)
		{
			int int_len;
			int i;

			int_len = FVariants.Length;
			TVariant[] var_temp = new TVariant[int_len - 1];
			for(i = 0; i < Index; i++) var_temp[i] = FVariants[i];
			for(i = Index; i < var_temp.Length; i++) var_temp[i] = FVariants[i + 1];
			FVariants = var_temp;
		}

		public int IndexOf(string Name)
		{
			bool b;
			int i;

			i = -1;
			b = false;
			while(!b && i < FVariants.Length - 1)
			{
				i++;

				if(FVariants[i].Name == Name) b = true;
			}

			if(b)
			{
				return i;
			}
			else return -1;
		}

		public TVariant this [int Index]
		{
			get
			{
				if(Index >= 0 && Index < FVariants.Length)
				{
					return FVariants[Index];
				}
				else return null;
			}
			set
			{
				if(Index >= 0 && Index < FVariants.Length)
				{
					FVariants[Index] = value;
				}
			}
		}

		public TVariant this [string Name]
		{
			get
			{
				int int_pos;

				int_pos = IndexOf(Name);
				if(int_pos > -1)
				{
					return FVariants[int_pos];
				}
				else return null;
			}
			set
			{
				int int_pos;

				int_pos = IndexOf(Name);
				if(int_pos > -1)
				{
					FVariants[int_pos] = value;
				}
			}
		}

		public int Count
		{
			get
			{
				return FVariants.Length;
			}
		}
	}
}
