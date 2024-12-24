using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace MachineDeptApp
{
    internal class ErrorReportAsTxt
    {
        //Create txt file for report Errors
        public static string PrintError = "";
        public static string y = DateTime.Now.ToString("yyyy");
        public static string m = DateTime.Now.ToString("MM");
        public static string d = DateTime.Now.ToString("dd");
        public static string hh = DateTime.Now.ToString("hh");
        public static string mm = DateTime.Now.ToString("mm");
        public static string ss = DateTime.Now.ToString("ss");
        public static string FileName = "Error" + y + m + d + hh + mm + ss;
        public static string ErrorPath = (Environment.CurrentDirectory).ToString() + @"\ErrorReport\" + FileName + ".txt";

        public void Output()
        {
            //ឆែករកមើល Folder បើគ្មាន => បង្កើត
            string SavePath = (Environment.CurrentDirectory).ToString() + @"\ErrorReport";
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
            using (FileStream fs = File.Create(ErrorPath))
            {
                fs.Write(Encoding.UTF8.GetBytes(PrintError), 0, PrintError.Length);
            }
        }
    }
}
