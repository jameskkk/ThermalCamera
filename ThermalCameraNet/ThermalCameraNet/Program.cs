using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThermalCameraNet
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LogUnit.InitLogManager();
            //ServicePointManager.DefaultConnectionLimit = 100;

            if (Properties.Settings.Default.UnhandledException)
            {
                LogUnit.Log.Info("Support UnhandledException!");
                // Add handler for UI thread exceptions
                Application.ThreadException += new ThreadExceptionEventHandler(UIThreadException);
                // Force all WinForms errors to go through handler
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                // This handler is for catching non-UI thread exceptions
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        private static void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                LogUnit.Log.Error("UIThreadException():" + ex.Message + ", " + Environment.NewLine + ex.StackTrace);
                MessageBox.Show("Unhadled domain exception:\n" + ex.Message + ", " + Environment.NewLine + ex.StackTrace);
            }
            catch (Exception exc)
            {
                try
                {
                    MessageBox.Show("Fatal exception happend inside UnhadledExceptionHandler: \n\n"
                        + exc.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }

            // It should terminate our main thread so Application.Exit() is unnecessary here
        }

        private static void UIThreadException(object sender, ThreadExceptionEventArgs t)
        {
            try
            {
                LogUnit.Log.Error("UIThreadException():" + t.Exception.Message + Environment.NewLine + t.Exception.StackTrace);
                MessageBox.Show("Unhandled exception catched.\n Application is going to close now.");
            }
            catch
            {
                try
                {
                    MessageBox.Show("Fatal exception happend inside UIThreadException handler",
                        "Fatal Windows Forms Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                }
                finally
                {
                    Application.Exit();
                }
            }

            // Here we can decide if we want to end our application or do something else
            Application.Exit();
        }
    }
}
