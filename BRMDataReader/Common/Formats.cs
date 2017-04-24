using System;
using System.Collections.Specialized;

namespace Business.Common
{
	/// <summary>
	/// Summary description for Formats.
	/// </summary>
	public class TFormats
	{
		public TFormats()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static string DecimalSeparator = ".";
		public static string ThousandSeparator = ",";

		public static DateTime EStrToDateTime(string str)
		{
			try
			{
				return Convert.ToDateTime(str);
			}
			catch
			{
				System.Globalization.DateTimeFormatInfo fmt = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat;
				DateTime dt = Convert.ToDateTime(str, fmt);
				return dt;
			}
		}

		//////////////////////////////////////////////////////////
		//  Internall use method for spliting strings by operators
		public static StringCollection SplitString(string str, string op)
		{
			StringCollection lst = new StringCollection();

			string s = str;
			int p = s.IndexOf(op);
			while(p != -1)
			{
				string ss = s.Substring(0, p);
				s = s.Remove(0, p + op.Length);
				if(ss.Trim() != "") lst.Add(ss.Trim());
				p = s.IndexOf(op);
			}

			if(s.Trim() != "") lst.Add(s.Trim());

			return lst;
		}

		public static string[] SplitStringIntoArray(string str, string op)
		{
			StringCollection lst = SplitString(str, op);

			string[] res = new string[lst.Count];
			lst.CopyTo(res, 0);

			return res;
		}

		public static double EStrFractionToFloat(string str)
		{
			StringCollection str_terms = SplitString(str.Trim()," ");
			if (str_terms.Count > 2) throw new Exception("Unrecognized format");

			string str_term1 = "0";
			string str_term2 = "0";

			if (str_terms.Count == 2 ) 
			{
				str_term1 = str_terms[0];
				str_term2 = str_terms[1];
			}
			else str_term2 = str_terms[0];
			
			double dbl_term1 = Convert.ToDouble(str_term1);

			StringCollection str_factors = SplitString(str_term2, "/");
			if (str_factors.Count != 2) throw new Exception("Unrecognized format");
			
			string str_factor1 = str_factors[0];
			string str_factor2 = str_factors[1];

			double dbl_factor1 = Convert.ToDouble(str_factor1);
			double dbl_factor2 = Convert.ToDouble(str_factor2);

			if (dbl_factor2 == 0) throw new Exception("Division by zero");
			
			return dbl_term1 + dbl_factor1 / dbl_factor2;
		}

		public static double EStrToFloat(string str)
		{
			try
			{
				System.Globalization.NumberFormatInfo fmt = new System.Globalization.NumberFormatInfo();
				fmt.CurrencyDecimalSeparator = DecimalSeparator;
				fmt.CurrencyGroupSeparator = ThousandSeparator;
				fmt.NumberDecimalSeparator = DecimalSeparator;
				fmt.NumberGroupSeparator = ThousandSeparator;
				double dbl = Convert.ToDouble(str, fmt);
				return dbl;
			}
			catch
			{
				try
				{
					System.Globalization.NumberFormatInfo fmt = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat;
					double dbl = Convert.ToDouble(str, fmt);
					return dbl;
				}
				catch
				{
					try
					{
						return Convert.ToDouble(str);
					}
					catch
					{
						try
						{
							System.Globalization.NumberFormatInfo fmt = new System.Globalization.NumberFormatInfo();
							fmt.CurrencyDecimalSeparator = ",";
							fmt.CurrencyGroupSeparator = ".";
							fmt.NumberDecimalSeparator = ",";
							fmt.NumberGroupSeparator = ".";
							double dbl = Convert.ToDouble(str, fmt);
							return dbl;
						}
						catch
						{
							try
							{
								System.Globalization.NumberFormatInfo fmt = new System.Globalization.NumberFormatInfo();
								fmt.CurrencyDecimalSeparator = ".";
								fmt.CurrencyGroupSeparator = ",";
								fmt.NumberDecimalSeparator = ".";
								fmt.NumberGroupSeparator = ",";
								double dbl = Convert.ToDouble(str, fmt);
								return dbl;
							}
							catch
							{
								return EStrFractionToFloat(str);
							}
						}
					}
				}
			}
		}

	}

}
