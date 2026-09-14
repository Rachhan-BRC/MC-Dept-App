using System;
using System.Data;

namespace MachineDeptApp
{
    internal class ConsoleDatatableClass
    {
        public ConsoleDatatableClass(DataTable dt) 
        {
            string CText = "";
            foreach (DataColumn col in dt.Columns)
            {
                CText += col.ColumnName.ToString() + "\t";
            }
            Console.WriteLine(CText);
            foreach (DataRow row in dt.Rows)
            {
                CText = "";
                foreach (DataColumn col in dt.Columns)
                {
                    CText += row[col.ColumnName].ToString() + "\t";
                }
                Console.WriteLine(CText);
            }
        }
    }
}
