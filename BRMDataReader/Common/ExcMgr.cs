#region Using directives
using System;
using System.IO;
using System.Collections.Specialized;
#endregion

namespace Business.Common
{
	/// <summary>
	/// Summary description for CExceptionManager.
	/// </summary>
	public class TExceptionManager
	{

		private string	FLogFile;
		private bool	FUseLogFile;

		private bool	FDisplayErrors;

		private bool	FVerbose;

        public TExceptionManager(string LogFileName)
        {
            FLogFile = LogFileName;
            FUseLogFile = false;
            FDisplayErrors = false;
            FVerbose = false;
        }

		public TExceptionManager()
		{
			//
			// TODO: Add constructor logic here
			//

			FLogFile		= "";
			FUseLogFile		= false;
			FDisplayErrors	= false;
			FVerbose		= false;
		}

		public string LogFile
		{
			get
			{
				return FLogFile;
			}
			set
			{
				FLogFile = value;
			}
		}

		public bool UseLogFile
		{
			get
			{
				return FUseLogFile;
			}
			set
			{
				FUseLogFile = value;
			}
		}

		public bool DisplayErrors
		{
			get
			{
				return FDisplayErrors;
			}
			set
			{
				FDisplayErrors = value;
			}
		}

		public bool Verbose
		{
			get
			{
				return FVerbose;
			}
			set
			{
				FVerbose = value;
			}
		}

		public void WriteToLog(string Text)
		{
			if(FUseLogFile && (FLogFile != ""))
			{
                StreamWriter sw = null;
                if (!File.Exists(FLogFile))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(FLogFile));
                    sw = File.CreateText(FLogFile);
                }
                else sw = File.AppendText(FLogFile);
                 
				using (sw) 
				{
					sw.WriteLine(DateTime.Now.ToString() + " " + Text);
				}
			}
		}

		public void ProcessException(Exception e, string CustomMessage, string CustomSTR)
		{
			StringCollection scl_Exception;
			
			scl_Exception = new StringCollection();

			scl_Exception.Add("Date/Time:    " + DateTime.Now.ToString());
			scl_Exception.Add("Message:      " + e.Message);

			if(CustomMessage != "") scl_Exception.Add("CustomMessage:" + CustomMessage);
			
			if(FVerbose)
			{
				scl_Exception.Add("Sender:       " + e.Source);
				scl_Exception.Add("StackTrace:   " + e.StackTrace.Trim());
				scl_Exception.Add("Invoking Method:");
				scl_Exception.Add("++Name:            " + e.TargetSite.Name);
				scl_Exception.Add("  Abstract:        " + e.TargetSite.IsAbstract.ToString());
				scl_Exception.Add("  Assembly:        " + e.TargetSite.IsAssembly.ToString());
				scl_Exception.Add("  Constructor:     " + e.TargetSite.IsConstructor.ToString());
				scl_Exception.Add("  Family:          " + e.TargetSite.IsFamily.ToString());
				scl_Exception.Add("  Family&Assembly: " + e.TargetSite.IsFamilyAndAssembly.ToString());
				scl_Exception.Add("  Famili!Assembly: " + e.TargetSite.IsFamilyOrAssembly.ToString());
				scl_Exception.Add("  Final:           " + e.TargetSite.IsFinal.ToString());
				scl_Exception.Add("  HidebySig:       " + e.TargetSite.IsHideBySig.ToString());
				scl_Exception.Add("  Private:         " + e.TargetSite.IsPrivate.ToString());
				scl_Exception.Add("  Public:          " + e.TargetSite.IsPublic.ToString());
				scl_Exception.Add("  SpecialName:     " + e.TargetSite.IsSpecialName.ToString());
				scl_Exception.Add("  Static:          " + e.TargetSite.IsStatic.ToString());
				scl_Exception.Add("  Virtual:         " + e.TargetSite.IsVirtual.ToString());
			}

			if(CustomSTR != "") scl_Exception.Add("Custom:     " + CustomSTR);

			if(FDisplayErrors)
			{
				/*if(CustomMessage != "")
				{
					CMsgBox.Error(CustomMessage);
				}
				else
				{
					CMsgBox.Error(e.Message);
				}*/
			}

			if(FUseLogFile && (FLogFile != ""))
			{
				using (StreamWriter sw = File.AppendText(FLogFile)) 
				{
					int i;

					sw.WriteLine();
					sw.WriteLine("Exception-----------------------------------------------");

					for(i = 0; i < scl_Exception.Count; i++)
						sw.WriteLine(scl_Exception[i]);

					sw.WriteLine("ENDException--------------------------------------------");
					sw.WriteLine();
				}
				
			}
		}

		public void ProcessException(Exception e)
		{
			ProcessException(e, "", "");
		}


		public void ProcessException(Exception e, string CustomSTR)
		{
			ProcessException(e, "", CustomSTR);
		}

		public void ProcessException_CustomMessage(Exception e, string CustomMessage)
		{
			ProcessException(e, CustomMessage, "");
		}

        public static TExceptionManager ExcManager;
        public static string ExcManager_File = "Logs\\Main.Log";

        public static TExceptionManager GetExcManager()
        {
            if (ExcManager != null) return ExcManager;
            else 
            {
                try
                {
                    ExcManager = new TExceptionManager(ExcManager_File);
                    ExcManager.FUseLogFile = true;
                    ExcManager.FDisplayErrors = false;
                    ExcManager.FVerbose = true;
                    return ExcManager;
                    
                }
                catch
                {
                    return null;
                }
            }
        }
	}
}
