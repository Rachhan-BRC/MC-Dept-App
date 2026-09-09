using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MachineDeptApp
{
    public partial class OBSMatRequestImportForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        string ErrorText = "";

        public OBSMatRequestImportForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Shown += OBSMatRequestImportForm_Shown;
            this.btnImport.Click += BtnImport_Click;
            this.dgvSearch.CellPainting += DgvSearch_CellPainting;
            this.btnSave.Click += BtnSave_Click;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            CheckingBeforeSave();
            if (!PicAlertPOS.Visible)
            {
                DialogResult DLR = MessageBox.Show("តើអ្នករក្សាទុកទិន្នន័យនេះមែនទេ?", MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    LbStatus.Text = "កំពុងរក្សាទុក . . . .";
                    LbStatus.Refresh();

                    ErrorText = "";
                    string User = MenuFormV2.UserForNextForm;
                    DateTime RegNow = DateTime.Now;

                    //Save to DB
                    try
                    {
                        if (cnn.con.State != ConnectionState.Open)
                        {
                            cnn.con.Open();
                        }

                        foreach (DataGridViewRow row in dgvSearch.Rows)
                        {
                            bool OBS = Convert.ToBoolean(row.Cells["OBSCheck"].Value);
                            bool Registered = Convert.ToBoolean(row.Cells["AlreadyRegister"].Value);
                            if (OBS && !Registered)
                            {
                                SqlCommand cmd = new SqlCommand(@"INSERT INTO tbOBSMatRequest
                                (MatReqNo, ItemCode, ItemName, Maker, RMType, PackSize, PackQty, TTLReqQty, Remarks, ShipDate, RegDate, RegBy)
                                SELECT @MatReqNo, @ItemCode, @ItemName, @Maker, @RMType, @PackSize, @PackQty, @TTLReqQty, @Remarks, @ShipDate, @RegDate, @RegBy
                                WHERE NOT EXISTS (SELECT 1 FROM tbOBSMatRequest WHERE MatReqNo = @MatReqNo)", cnn.con);
                                cmd.Parameters.AddWithValue("@MatReqNo", row.Cells["Barcode"].Value.ToString());
                                cmd.Parameters.AddWithValue("@ItemCode", row.Cells["CodeNo"].Value.ToString());
                                cmd.Parameters.AddWithValue("@ItemName", row.Cells["Description"].Value.ToString());
                                cmd.Parameters.AddWithValue("@Maker", row.Cells["Maker"].Value.ToString());
                                cmd.Parameters.AddWithValue("@RMType", row.Cells["Type"].Value.ToString());
                                cmd.Parameters.AddWithValue("@PackSize", Convert.ToInt32(row.Cells["Pack1Qty"].Value));
                                cmd.Parameters.AddWithValue("@PackQty", Convert.ToInt32(row.Cells["Pack"].Value));
                                cmd.Parameters.AddWithValue("@TTLReqQty", Convert.ToInt32(row.Cells["TotalQty"].Value));
                                cmd.Parameters.AddWithValue("@Remarks", txtDocNo.Text.Trim());
                                cmd.Parameters.AddWithValue("@ShipDate", dtpShipDate.Value.Date);
                                cmd.Parameters.AddWithValue("@RegDate", RegNow);
                                cmd.Parameters.AddWithValue("@RegBy", User);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorText = "Save to DB : \n" + ex.Message;
                    }
                    finally
                    {
                        if (cnn.con.State == ConnectionState.Open)
                        {
                            cnn.con.Close();
                        }
                    }

                    Cursor = Cursors.Default;

                    if (ErrorText.Trim() == "")
                    {
                        LbStatus.Text = "រក្សាទុករួចរាល់";
                        MessageBox.Show("រក្សាទុករួចរាល់!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        LbStatus.Text = "រក្សាទុកមានបញ្ហា!";
                        MessageBox.Show("មានបញ្ហា!" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void DgvSearch_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow drow = dgvSearch.Rows[e.RowIndex];
                bool.TryParse(drow.Cells["OBSCheck"].Value?.ToString() ?? "False", out bool OBS);
                bool.TryParse(drow.Cells["AlreadyRegister"].Value?.ToString() ?? "False", out bool Registered);
                drow.DefaultCellStyle.ForeColor = (OBS && !Registered) ? dgvSearch.AlternatingRowsDefaultCellStyle.ForeColor : Color.Gray;
            }
        }
        private void BtnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFD = new OpenFileDialog { 
                Title = "Choose Excel File",
                Multiselect = false
            };
            openFD.Filter = "Excel|*.xlsx;";
            if (openFD.ShowDialog() == DialogResult.OK && openFD.FileName.Trim() != "")
            {
                ErrorText = "";
                Cursor = Cursors.WaitCursor;
                LbStatus.Text = "កំពុងអានទិន្នន័យ . . .";
                LbStatus.Refresh();
                dgvSearch.Rows.Clear();
                foreach(DataGridViewColumn col in dgvSearch.Columns)
                    col.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.None;
                txtDocNo.Text = "";
                btnSave.Enabled = false;

                Excel.Application xlApp = null;
                Excel.Workbook xlWorkbook = null;
                int FoundToBeSave = 0;

                try
                {
                    xlApp = new Excel.Application();
                    xlWorkbook = xlApp.Workbooks.Open(openFD.FileName);
                    Excel.Worksheet xlWorksheet = (Excel.Worksheet)xlWorkbook.Worksheets[1];
                    Excel.Range xlRange = xlWorksheet.UsedRange;

                    //Header info -> row2: A="POS : ", B=POS value ... G="ISSUE DATE", H=date value
                    txtDocNo.Text = (xlRange.Cells[2, 2] as Excel.Range).Text.ToString().Trim();

                    //Data rows start at row4 -> A:CodeNo, B:Description, C:Maker, D:Type, E:Pack1Qty, F:Pack, G:TotalQty, H:Barcode
                    for (int xlRow = 4; xlRow <= xlRange.Rows.Count; xlRow++)
                    {
                        string codeNo = (xlRange.Cells[xlRow, 1] as Excel.Range).Text.ToString().Trim();
                        if (codeNo == "")
                        {
                            continue;
                        }

                        string description = (xlRange.Cells[xlRow, 2] as Excel.Range).Text.ToString().Trim();
                        string maker = (xlRange.Cells[xlRow, 3] as Excel.Range).Text.ToString().Trim();
                        string type = (xlRange.Cells[xlRow, 4] as Excel.Range).Text.ToString().Trim();
                        double pack1Qty = Convert.ToDouble((xlRange.Cells[xlRow, 5] as Excel.Range).Value2 ?? 0d);
                        double pack = Convert.ToDouble((xlRange.Cells[xlRow, 6] as Excel.Range).Value2 ?? 0d);
                        double totalQty = Convert.ToDouble((xlRange.Cells[xlRow, 7] as Excel.Range).Value2 ?? 0d);
                        string barcode = (xlRange.Cells[xlRow, 8] as Excel.Range).Text.ToString().Trim();

                        DataGridViewRow row = dgvSearch.Rows[dgvSearch.Rows.Add()];
                        row.HeaderCell.Value = (row.Index + 1).ToString();
                        row.Cells["CodeNo"].Value = codeNo;
                        row.Cells["Description"].Value = description;
                        row.Cells["Maker"].Value = maker;
                        row.Cells["Type"].Value = type;
                        row.Cells["Pack1Qty"].Value = pack1Qty;
                        row.Cells["Pack"].Value = pack;
                        bool OBSStatus = GetOBSStatus(barcode.Replace("*", "")), RegisteredStatus = GetRegisterStatus(barcode.Replace("*", ""));
                        row.Cells["OBSCheck"].Value = OBSStatus;
                        row.Cells["AlreadyRegister"].Value = RegisteredStatus;
                        row.Cells["TotalQty"].Value = totalQty;
                        row.Cells["Barcode"].Value = barcode.Replace("*","");

                        if(OBSStatus && !RegisteredStatus)
                        {
                            FoundToBeSave++;
                        }

                    }

                    xlApp.DisplayAlerts = false;
                    xlWorkbook.Close();
                    xlApp.Quit();
                }
                catch (Exception ex)
                {
                    ErrorText = ex.Message;
                    if (xlWorkbook != null)
                    {
                        xlApp.DisplayAlerts = false;
                        xlWorkbook.Close();
                    }
                    xlApp?.Quit();
                }
                finally
                {
                    //Kill all Excel background process
                    var processes = from p in Process.GetProcessesByName("EXCEL")
                                    select p;
                    foreach (var process in processes)
                    {
                        if (process.MainWindowTitle.ToString().Trim() == "")
                            process.Kill();
                    }
                }

                Cursor = Cursors.Default;

                if (ErrorText.Trim() == "")
                {
                    if (dgvSearch.Rows.Count > 0)
                    {
                        LbStatus.Text = "ការអានរួចរាល់ !";
                        dgvSearch.ClearSelection();
                        if(FoundToBeSave > 0)
                        {
                            btnSave.Enabled = true;
                            tStripActionBtn.Focus();
                            tStripActionBtn.Items["btnSave"].Select();
                        }
                        else
                            MessageBox.Show("ការអានរួចរាល់, ប៉ុន្តែគ្មានទិន្នន័យដែលត្រូវរក្សាទុកទេ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    }
                    else
                    {
                        LbStatus.Text = "ការអានរួចរាល់, ប៉ុន្តែគ្មានទិន្នន័យ !";
                        MessageBox.Show("ការអានរួចរាល់, ប៉ុន្តែគ្មានទិន្នន័យទេ!", MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    LbStatus.Text = "ការអានបរាជ័យ !";
                    MessageBox.Show("Something wrong!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OBSMatRequestImportForm_Shown(object sender, EventArgs e)
        {
            this.dgvSearch.RowHeadersDefaultCellStyle.Font = dgvSearch.AlternatingRowsDefaultCellStyle.Font;
            Color CustColor = Color.Orange;
            this.dgvSearch.Columns["OBSCheck"].HeaderCell.Style.BackColor = CustColor;
            this.dgvSearch.Columns["AlreadyRegister"].HeaderCell.Style.BackColor = CustColor;
        }

        //Method
        private bool GetOBSStatus(string MATNo)
        {
            bool Found = false;
            try
            {
                if (cnn.con.State != ConnectionState.Open)
                {
                    cnn.con.Open();
                }
                SqlCommand cmd = new SqlCommand(@"SELECT COUNT(MatReqNo) AS FoundQty FROM vw_OBSMatReq WHERE MatReqNo = @MatReqNo", cnn.con);
                cmd.Parameters.AddWithValue("@MatReqNo", MATNo);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int FoundQty = reader.GetInt32(reader.GetOrdinal("FoundQty"));
                        if (FoundQty > 0)
                            Found = true;
                    }
                }
            }
            catch{ }
            finally
            {
                if (cnn.con.State == ConnectionState.Open)
                {
                    cnn.con.Close();
                }
            }

            return Found;
        }
        private bool GetRegisterStatus(string MATNo)
        {
            bool Found = false;
            try
            {
                if (cnn.con.State != ConnectionState.Open)
                {
                    cnn.con.Open();
                }
                SqlCommand cmd = new SqlCommand(@"SELECT COUNT(MatReqNo) AS FoundQty FROM tbOBSMatRequest WHERE MatReqNo = @MatReqNo", cnn.con);
                cmd.Parameters.AddWithValue("@MatReqNo", MATNo);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int FoundQty = reader.GetInt32(reader.GetOrdinal("FoundQty"));
                        if (FoundQty > 0)
                            Found = true;
                    }
                }
            }
            catch { }
            finally
            {
                if (cnn.con.State == ConnectionState.Open)
                {
                    cnn.con.Close();
                }
            }

            return Found;
        }
        private async void CheckingBeforeSave()
        {
            var tasksBlink = new List<Task>();
            PicAlertPOS.Visible = false;
            if (txtDocNo.Text.Trim() == "")
                tasksBlink.Add(BlinkPictureBox(PicAlertPOS));
            await Task.WhenAll(tasksBlink);
        }
        private async Task BlinkPictureBox(PictureBox pictureBox)
        {
            pictureBox.Visible = false;
            for (int i = 0; i < 7; i++)
            {
                pictureBox.Visible = !pictureBox.Visible;
                await Task.Delay(350);
            }
            pictureBox.Visible = true;
        }
    }
}
