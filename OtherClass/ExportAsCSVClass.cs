using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MachineDeptApp.OtherClass
{
    internal class ExportAsCSVClass
    {
        public string CSVErrorT = "";
        public void ExportAsCSV(DataGridView dgvData, string FName, int cIndex)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "CSV file (*.csv)|*.csv";
            saveDialog.FileName = FName;
            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    //Write Column name
                    int columnCount = dgvData.Columns.Count;
                    string columnNames = "";

                    //String array for Csv
                    string[] outputCsv;
                    outputCsv = new string[dgvData.Rows.Count + 1];

                    //Set Column Name
                    for (int i = cIndex; i < columnCount; i++)
                    {
                        if (dgvData.Columns[i].Visible == true)
                        {
                            columnNames += dgvData.Columns[i].HeaderText.ToString() + ",";
                        }
                    }
                    outputCsv[0] += columnNames;

                    //Row of data 
                    for (int i = 1; (i - 1) < dgvData.Rows.Count; i++)
                    {
                        for (int j = cIndex; j < columnCount; j++)
                        {
                            if (dgvData.Columns[j].Visible == true)
                            {
                                string Value = "";
                                if (dgvData.Rows[i - 1].Cells[j].Value != null)
                                {
                                    Value = dgvData.Rows[i - 1].Cells[j].Value.ToString();
                                }
                                //Fix don't separate if it contain '\n' or ','
                                Value = "\"" + Value.Replace("\"", "\"\"") + "\"";
                                outputCsv[i] += Value + ",";
                            }
                        }
                    }

                    File.WriteAllLines(saveDialog.FileName, outputCsv, Encoding.UTF8);
                    Cursor.Current = Cursors.Default;
                    CSVErrorT = "OK";
                }
                catch (Exception ex)
                {
                    Cursor.Current = Cursors.Default;
                    CSVErrorT = "មានបញ្ហា!\n" + ex.Message;
                }
            }
        }
    }
}
