using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineDeptApp.OtherClass
{
    internal class OBSMatReqPrintClass
    {
        public static string SavePath { private set; get; }
        public static string ErrorText { private set; get; }
        public void PrintExcelOut(DataTable dt)
        {
            SavePath = ""; ErrorText = "";
        }
    }
}
