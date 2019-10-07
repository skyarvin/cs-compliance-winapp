using System;
using System.Threading;
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
            const string appName = "SkydevCSTool";
            bool createdNew;
            mutex = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                //app is already running! Exiting the application  
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            
            Application.ThreadException += new ThreadExceptionEventHandler(chucha);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(frmMain.MyCommonExceptionHandlingMethod);
            var Login = new frmLogin();
            Login.Show();
            Application.Run();
            
        }


        public static void chucha(object sender, ThreadExceptionEventArgs t)
        {
            Console.WriteLine("chucha");
        }

       
    }
}
