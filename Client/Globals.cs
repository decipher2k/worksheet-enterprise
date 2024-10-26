using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WorksheetLog
{
    public static class Globals
    {
        public static String host = "";
        public static String ID = "";
        public static String apipass="";

        static Globals()
        {
            try
            {
                host = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\hostname.txt");
            }
            catch (Exception) { }

            try
            {
                ID = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\id.txt");
            }
            catch (Exception) { }

            try
            {
                apipass = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\apipass.txt");
            }
            catch (Exception) { }


            if (host == "" || apipass == "")
            {
                MessageBox.Show("Configuration error: host not set. Please contact your administrator.");
                Application.Exit();
            }
            if (ID == "")
            {
                Initialize.Init();
                try
                {
                    ID = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\id.txt");
                }
                catch (Exception) { }
            }
            if (ID == "")
            {
                MessageBox.Show("Error starting Program. Exiting.");
                Application.Exit();
            }
        }
    }
}
