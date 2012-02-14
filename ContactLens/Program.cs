using System;
using System.Windows.Forms;
using Utilities;

namespace ContactLens
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var logger = new ExceptionLogger();            
            logger.AddLogger(new EventLogLogger());
            Application.Run(new FrmReplacementMenu());
        }
    }
}
