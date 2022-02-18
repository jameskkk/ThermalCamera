using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThermalCameraNet
{
    public class LogUnit
    {
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string LogConfiguration = Application.StartupPath + @"\log4net.config";

        public static void InitLogManager()
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(LogConfiguration));
        }

        public static void DeleteLogFile()
        {
            string basePath = Application.StartupPath;
            foreach (string file in Directory.GetFiles(basePath))
            {
                if (Path.GetExtension(file).Contains("log"))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        LogUnit.Log.Error("DeleteLogFile() Exception: " + ex.Message);
                    }
                }
            }
        }

        public static void CleanLogFile()
        {
            string basePath = Application.StartupPath;
            foreach (string file in Directory.GetFiles(basePath))
            {
                if (Path.GetExtension(file).Contains("log"))
                {
                    FileStream fs = null;
                    try
                    {
                        fs = new FileStream(file, FileMode.Truncate, FileAccess.ReadWrite);
                    }
                    catch (Exception ex)
                    {
                        LogUnit.Log.Error("CleanLogFile() Exception: " + ex.Message);
                        List<Process> lstProcess = FileUnit.WhoIsLocking(file);
                        foreach (Process Process in lstProcess)
                        {
                            LogUnit.Log.Info("Lock process is " + Process.ProcessName);
                        }
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Close();
                        }
                    }
                }
            }
        }

        public static int GetExceptionLineNumber(Exception ex)
        {
            StackTrace trace = new StackTrace(ex, true);
            var stackFrame = trace.GetFrame(trace.FrameCount - 1);
            var lineNumber = stackFrame.GetFileLineNumber();

            return lineNumber;
        }
    }
}
