using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.TrackingPOS
{
    public partial class RetriveDataFromOBSForm : Form
    {
        SQLConnectOBS cnnOBS = new SQLConnectOBS();
        SQLConnect cnn = new SQLConnect();
        DataTable OBSItem;
        SqlCommand cmd;

        public RetriveDataFromOBSForm()
        {
            InitializeComponent();
            cnn.Connection();
            cnnOBS.Connection();
            progressBar1.Visible = false;
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                Thread.Sleep(10);
                backgroundWorker1.WorkerReportsProgress = true;
                backgroundWorker1.ReportProgress(i);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value= e.ProgressPercentage;
            LbPercent.Text = e.ProgressPercentage.ToString() + " % "; 
            if (LbPercent.Text=="100 % ")
            {
                MessageBox.Show("ទាញទិន្នន័យ POS ពី OBS រួចរាល់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LbPercent.Text = "";

            }
        }

        private void btnRetrive_Click(object sender, EventArgs e)
        {
            DialogResult DLR = MessageBox.Show("តើអ្នកចង់ចាប់ផ្ដើមទាញទិន្នន័យ POS ពី OBS មែនដែរឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLR == DialogResult.Yes)
            {
                LbFunctions.Text = "ទាញទិន្នន័យចេញពី OBS";
                try
                {
                    cnnOBS.conOBS.Open();
                    SqlDataAdapter da2 = new SqlDataAdapter("SELECT ItemCode, ItemName, Remark, Remark2, Remark3, Remark4 FROM mstitem Where ItemType='0' OR ItemType='1' OR ItemType='2' Order By CreateDate ASC, ItemType ASC;", cnnOBS.conOBS);
                    OBSItem = new DataTable();
                    da2.Fill(OBSItem);
                    backgroundWorker1.RunWorkerAsync();
                    progressBar1.Show();
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnnOBS.conOBS.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (OBSItem.Rows.Count > 0)
            {
                DialogResult DLR = MessageBox.Show("តើអ្នកចង់ចាប់ផ្ដើមរក្សាទុកទិន្នន័យ POS មែនដែរឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    LbFunctions.Text = "រក្សាទុកទិន្នន័យ Items";
                    LbFunctions.Refresh();
                    string Error = "";
                    //Delete Old Data
                    try
                    {
                        cnn.con.Open();
                        cmd = new SqlCommand("DELETE FROM tbMasterItem;", cnn.con);
                        cmd.ExecuteNonQuery();

                    }
                    catch
                    {
                        MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    cnn.con.Close();
                                        
                    var oneHundred1 = OBSItem.Rows.Count;
                    int error = 0;
                    for (int i = 0; i < OBSItem.Rows.Count; i++)
                    {
                        try
                        {
                            cnn.con.Open();
                            try
                            {
                                cmd = new SqlCommand("INSERT INTO tbMasterItem (ItemCode, ItemName, Remarks, Remarks1, Remarks2, Remarks3) VALUES (@ic, @in, @rm, @rm1, @rm2, @rm3);", cnn.con);
                                cmd.Parameters.AddWithValue("@ic", OBSItem.Rows[i][0].ToString());
                                cmd.Parameters.AddWithValue("@in", OBSItem.Rows[i][1].ToString());
                                cmd.Parameters.AddWithValue("@rm", OBSItem.Rows[i][2].ToString());
                                cmd.Parameters.AddWithValue("@rm1", OBSItem.Rows[i][3].ToString());
                                cmd.Parameters.AddWithValue("@rm2", OBSItem.Rows[i][4].ToString());
                                cmd.Parameters.AddWithValue("@rm3", OBSItem.Rows[i][5].ToString());
                                cmd.ExecuteNonQuery();

                            }
                            catch
                            {

                            }
                            var currentPercent = ((100 * i) / oneHundred1);
                            progressBar1.Value = currentPercent;
                            LbPercent.Text = currentPercent + " % ";
                            LbPercent.Invalidate();
                            LbPercent.Update();

                        }
                        catch(Exception ex)
                        {
                            Error = Error + " " + ex.ToString();
                            error = error++;
                        }
                        cnn.con.Close();

                    }
                    if (error == 0)
                    {
                        progressBar1.Value = 100;
                        LbPercent.Text = 100 + " % ";                        
                        MessageBox.Show("រក្សាទុកទិន្នន័យរួចរាល់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        progressBar1.Value = 100;
                        LbPercent.Text = 100 + " % ";
                        MessageBox.Show("មានបញ្ហា!"+Error, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }        
        }

        private void RetriveDataFromOBSForm_Load(object sender, EventArgs e)
        {
            ToolTip toolTip1 = new ToolTip();
            // Set up the delays for the ToolTip.
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 250;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTip1.ShowAlways = true;
            // Set up the ToolTip text for the Button and Checkbox.
            toolTip1.SetToolTip(this.btnRetrive, "ទាញទិន្នន័យ POS ពី OBS");
            toolTip1.SetToolTip(this.btnSave, "រក្សាទុកទិន្នន័យ POS");
            
        }
    
    }
}
