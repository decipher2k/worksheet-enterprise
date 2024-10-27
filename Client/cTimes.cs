using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Net;
using System.Management;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Web;

namespace WorksheetLog
{
    [Serializable()]
    public class cTimes
    {


        public bool connectOnWLAN = false;
        public String WLANID = "";
        public Hashtable Dates = new Hashtable();

        public cTimes()
        {
        }

        public void Export(DateTime von, DateTime bis)
        {
            Hashtable exportDates = new Hashtable();
            ArrayList sortedDate = new ArrayList();
            foreach (String date in Dates.Keys)
            {
                if (toDateTime(date) >= von && toDateTime(date) <= bis)
                {
                    sortedDate.Add(toDateTime(date));
                }
            }
            sortedDate = sort(sortedDate);
            output(sortedDate);
            //File.Move(".\\data.dat", ".\\data.dat." + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString().Replace(":","."));
        }

        private void output(ArrayList sortedDate)
        {
            String Ret = "Time Sheet\r\n--------------------------------------------------\r\n\r\n";

            foreach (DateTime date in sortedDate)
            {
                String sDate = date.ToShortDateString();
                foreach(Hashtable timeSlice in ((ArrayList)Dates[sDate]))
                {
                    String von = ((String)timeSlice["Start"]);
                    String bis = ((String)timeSlice["End"]);
                    Ret += sDate + "\t" + von + "-" + bis + "\r\n";
                }
                Ret += "\r\n";
            }
            
            File.WriteAllText(".\\output.txt", Ret);
            Process.Start("notepad.exe", ".\\output.txt");
        }

        private ArrayList sort(ArrayList sortedDate)
        {
            for (int x = 0; x < sortedDate.Count; x++)
            {
                for (int i = 0; i < sortedDate.Count; i++)
                {
                    if ((DateTime)sortedDate[x] > (DateTime)sortedDate[i])
                    {
                        DateTime temp = (DateTime)sortedDate[x];
                        sortedDate[x] = sortedDate[i];
                        sortedDate[i] = temp;
                    }
                }
            }
            return sortedDate;
        }

        private DateTime toDateTime(String date)
        {
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "dd.MM.yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {            
                myDate = DateTime.ParseExact(date, "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture);
            }
            return myDate;
        }

        //Serializing the List
        public void Save(cTimes emps, String filename)
        {
            //Create the stream to add object into it.
            System.IO.Stream ms = File.OpenWrite(filename);
            //Format the object as Binary

            BinaryFormatter formatter = new BinaryFormatter();
            //It serialize the employee object
            formatter.Serialize(ms, emps);
            ms.Flush();
            ms.Close();
            ms.Dispose();
        }

        public cTimes Load(String filename)
        {
            //Format the object as Binary
            BinaryFormatter formatter = new BinaryFormatter();

            //Reading the file from the server
            FileStream fs = File.Open(filename, FileMode.Open);

            object obj = formatter.Deserialize(fs);
            cTimes emps = (cTimes)obj;
            fs.Flush();
            fs.Close();
            fs.Dispose();
            return emps;

        }


        public String genHardwareID()
        {
            return FingerPrint.Value();
        }

        public String getDateFormat()
        {
            String date = "01.01.1970";
            DateTime myDate;
            try
            {
                myDate = DateTime.ParseExact(date, "dd.MM.yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                return "de";
            }
            catch (Exception ex)
            {
            }
            return "us";
        }

        public void addTimestamp()
        {
            
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
           
           

            String secret = "";
            secret += genHardwareID();
            secret += "///";
            secret += DateTime.Now.ToShortDateString();
            
            WebClient wc = new WebClient();
            String currentKey=wc.DownloadString(Globals.host+"/api.php?action=query&id="+Globals.ID+"&apipass="+Globals.apipass);

            secret = HttpUtility.UrlEncode( base64(xor(secret, currentKey)) );
            
            String res=wc.DownloadString(Globals.host+"/api.php?action=update&secret="+secret+"&id="+Globals.ID+"&dateformat="+getDateFormat() + "&apipass=" + Globals.apipass);
            

        }

        public String xor(String text, String key)
        {
                var result = new StringBuilder();
                for (int c = 0; c < text.Length; c++)
                    result.Append((char)((uint)text[c] ^ (uint)key[c % key.Length]));

                return result.ToString();    
        }

        public String base64(String plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public void beginTimespan()
        {
         /*   if(Dates[DateTime.Now.Date.ToShortDateString()]==null)            
                Dates[DateTime.Now.Date.ToShortDateString()] = new ArrayList();
            Hashtable h = new Hashtable();
            h.Add("Start", DateTime.Now.TimeOfDay.ToString().Split('.')[0]);
            h.Add("End", DateTime.Now.TimeOfDay.ToString().Split('.')[0]);
            ((ArrayList)Dates[DateTime.Now.Date.ToShortDateString()]).Add(h);*/
        }

    }
}
