using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace MachineDeptApp
{

    public partial class StockINOut : Form
    {
        SQLConnect con = new SQLConnect();
        DataTable dtMst = new DataTable();

        string Dept = "MC", ErrorText = "";

        public StockINOut()
        {
            InitializeComponent();
            this.con.Connection();
            this.Shown += StockINOut_Shown;
            this.btnAdd.MouseEnter += BtnAdd_MouseEnter;
            this.btnAdd.MouseLeave += BtnAdd_MouseLeave;
            this.btnAddPic.MouseEnter += BtnAdd_MouseEnter;
            this.btnAddPic.MouseLeave += BtnAdd_MouseLeave;
            this.btnAdd.Click += BtnAdd_Click;
            this.btnAddPic.Click += BtnAdd_Click;
            this.btnShowSearch.Click += BtnShowSearch_Click;    


            this.rdbStockOut.CheckedChanged += RdbStockOut_CheckedChanged;
        }

        private void BtnShowSearch_Click(object sender, EventArgs e)
        {
            if (panelSearch.Visible)
                panelSearch.Visible = false;
            else
            {
                //TakingMst();
                if (dtMst.Rows.Count >= 10)
                {
                    panelSearch.Size = new Size(631, 189);
                    txtSearch.Visible = true;
                }
                else
                {
                    //panelSearch.Size = new Size(631, (dtMstComOrMiniName.Rows.Count * 22) + dtMstComOrMiniName.Rows.Count + splitter1.Size.Height + 2);
                    txtSearch.Visible = false;
                }
                //Console.WriteLine("Location : " + panelSearch.Location.X + ", "+ panelSearch.Location.Y);
                //Console.WriteLine("PanelComAndMini H : " + panelSearch.Size.Height);
                //Console.WriteLine("PanelBody H : " + panelBody.Size.Height);
                //Console.WriteLine("tabEdit H : " + tabContrlEdit.Size.Height);
                //panelSearch.Location = new Point(131, panelBody.Size.Height - tabContrlEdit.Size.Height - panelSearch.Height + 73);
                panelSearch.Visible = true;
                dgvSearch.ClearSelection();
                dgvSearch.Focus();
            }
        }

        private void RdbStockOut_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbStockOut.Checked)
            {
                LbQty.Text = rdbStockOut.Text +" Qty";
                LbQty.ForeColor = Color.Red;
                LbStockRemain.Visible = true;
            }
            else
            {
                LbQty.Text = rdbStockIN.Text + " Qty";
                LbQty.ForeColor = Color.Green;
                LbStockRemain.Visible = false;
            }
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            CheckBeforeAdd();
            int FoundError = 0;
            foreach (Control ctrl in GrbAdd.Controls)
            {
                if (ctrl is PictureBox pic && ctrl.Name.ToUpper().Contains("PICALERT") && pic.Visible == true)
                {
                    FoundError++;
                }
            }
            if (FoundError == 0)
            {

            }
        }
        private void BtnAdd_MouseLeave(object sender, EventArgs e)
        {
            Color NewColor = Color.White;
            btnAdd.BackColor = NewColor;
            btnAddPic.BackColor = NewColor;
        }
        private void BtnAdd_MouseEnter(object sender, EventArgs e)
        {
            Color NewColor = Color.LightBlue;
            btnAdd.BackColor = NewColor;
            btnAddPic.BackColor = NewColor;
        }


        private void StockINOut_Shown(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            this.panelSearch.BringToFront();

            TakingMst();
            if (ErrorText.Trim() == "")
            {
                foreach (DataRow row in dtMst.Rows)
                {
                    dgvSearch.Rows.Add(row["Code"].ToString(), row["Part_No"].ToString(), row["Part_Name"].ToString());
                }
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
                MessageBox.Show("មានបញ្ហា!\n"+ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

        }


        //Method
        private async void CheckBeforeAdd()
        {
            PicAlertCode.Visible = false;
            PicAlertQty.Visible = false;
            PicAlertRemark.Visible = false;
            var tasksBlink = new List<Task>();

            if (txtPartNo.Text.Trim() == "")
                tasksBlink.Add(BlinkPictureBox(PicAlertCode));

            if (txtQty.Text.Trim() == "")
            {
                toolTip1.SetToolTip(PicAlertQty, "សូមបញ្ចូល " + LbQty.Text);
                tasksBlink.Add(BlinkPictureBox(PicAlertQty));
            }
            else
            {
                try
                {
                    double Qty = Convert.ToDouble(txtQty.Text);
                    double StockQty = Convert.ToDouble(LbStockRemain.Text.Replace("/ ", ""));
                    if (Qty > 0)
                    {
                        //Comparing Stock if > Transaction is Stock-Out
                        if (rdbStockOut.Checked)
                        {
                            if (Qty > StockQty)
                            {
                                toolTip1.SetToolTip(PicAlertQty, "ចំនួនស្តុកមិនគ្រប់គ្រាន់ទេ!");
                                tasksBlink.Add(BlinkPictureBox(PicAlertQty));
                            }
                        }
                    }
                    else
                    {
                        toolTip1.SetToolTip(PicAlertQty, "ចំនួនត្រូវតែធំជាង 0!");
                        tasksBlink.Add(BlinkPictureBox(PicAlertQty));
                    }
                }
                catch
                {
                    toolTip1.SetToolTip(PicAlertQty, "អ្នកបញ្ចូលខុសទម្រង់ហើយ!\nសូមបញ្ចូលជាចំនួនលេខ!");
                    tasksBlink.Add(BlinkPictureBox(PicAlertQty));
                }
            }

            if (txtRemark.Text.Trim() == "")
                tasksBlink.Add(BlinkPictureBox(PicAlertRemark));

            await Task.WhenAll(tasksBlink);
        }
        private async Task BlinkPictureBox(PictureBox pictureBox)
        {
            pictureBox.Visible = false;
            for (int i = 0; i < 7; i++)
            {
                pictureBox.Visible = !pictureBox.Visible;
                await Task.Delay(300);
            }
            pictureBox.Visible = true;
        }
        private void TakingMst()
        {
            //Taking Mst
            dtMst = new DataTable();
            try
            {
                con.con.Open();
                string SQLQuery = "SELECT * FROM MstMCSparePart WHERE Dept = '" + Dept + "' ORDER BY Code";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, con.con);
                sda.Fill(dtMst);
            }
            catch (Exception ex)
            {
                ErrorText = "Taking Mst : " + ex.Message;
            }
            con.con.Close();
        }
    }
}
