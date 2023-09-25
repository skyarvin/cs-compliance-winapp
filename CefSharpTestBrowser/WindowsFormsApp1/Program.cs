using CSTool.Models;
using CSTool.Properties;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    static class Program
    {
        private static Mutex mutex = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            const string appName = "CSTool";
            bool createdNew;
            mutex = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                //app is already running! Exiting the application  
                return;
            }
            Resolve_missing_assembly();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            var Login = new frmLogin();            
            Login.Show();
            Application.Run();
            
        }

        private static void Resolve_missing_assembly()
        {
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs e)
            {
                AssemblyName requestedName = new AssemblyName(e.Name);
                try
                {
                    return Assembly.LoadFrom(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + $"\\SysWOW64\\stfm\\{requestedName.Name}.dll");
                }
                catch
                {
                    return null;
                }
            };
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string message = String.Concat(
                "App Version: v.", Globals.CurrentVersion(), "\n",
                "IP Address: ", Globals.MyIP ?? "0.0.0.0", "\n",
                "Profile: ", Settings.Default.email, "\n",
                "Error: ", e.ExceptionObject.ToString(), "\n"
            );
            Emailer emailer = new Emailer { subject = "CSCB WinApp Unexpected Error" , message = message};
            emailer.Send();
            MessageBox.Show("The application has encountered an unexpected error. Please seek a support for further assistance.");
        }
    }
}
