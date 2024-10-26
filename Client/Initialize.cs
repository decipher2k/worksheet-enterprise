using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace WorksheetLog
{
    public static class Initialize
    {
        public static void Init()
        {
            MessageBox.Show("Initializing Program.");
            String ret=Interaction.InputBox("Init Password (please contact your administrator if you don't know it.)", "Init Password", "");
            MessageBox.Show(FingerPrint.Value());
            String result = (new WebClient()).DownloadString(Globals.host + "/api.php?action=init&pass="+ret+"&hwid="+FingerPrint.Value()+"&apipass="+Globals.apipass);
            if (result != "FAIL")
            {            
                String[] ret1 = result.Split(';');
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\id.txt",ret1[0]);
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\apipass.txt", ret1[1]);
            }
            else
            {
                MessageBox.Show("Error. Please contact administrator.");
                Application.Exit();
            }
        }
    }
}
