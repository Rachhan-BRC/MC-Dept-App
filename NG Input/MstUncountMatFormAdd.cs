using MachineDeptApp.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.NG_Input
{
    public partial class MstUncountMatFormAdd : Form
    {
        SQLConnect cnn = new SQLConnect();
        SqlCommand cmd;
        MstUncountMatForm fgrid;
        string user = MstUncountMatForm.UsernameForNextForm;

        public MstUncountMatFormAdd(MstUncountMatForm fg)
        {
            InitializeComponent();
            cnn.Connection();
            this.fgrid = fg;
            this.CboItems.SelectedIndexChanged += CboItems_SelectedIndexChanged;
            this.CboItems.TextChanged += CboItems_TextChanged;
            this.txtKG.KeyPress += TxtKG_KeyPress;
            this.txtLength.KeyPress += TxtLength_KeyPress;
            this.txtKG.Leave += TxtKG_Leave;
            this.txtLength.Leave += TxtLength_Leave;

        }

        private void TxtLength_Leave(object sender, EventArgs e)
        {
            if (txtLength.Text.Trim() != "")
            {
                try
                {
                    double roundKG = Convert.ToDouble(txtLength.Text);
                    txtLength.Text = String.Format("{0:#,###}", Math.Round(roundKG, 3, MidpointRounding.AwayFromZero));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtLength.Text = "";
                }
            }
            else
            {

            }
        }

        private void TxtKG_Leave(object sender, EventArgs e)
        {
            if (txtKG.Text.Trim() != "")
            {
                try
                {
                    double roundKG = Convert.ToDouble(txtKG.Text);
                    txtKG.Text = String.Format("{0:#,###.000}", Math.Round(roundKG, 3, MidpointRounding.AwayFromZero));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtKG.Text = "";
                }
            }
            else
            {

            }
        }

        private void TxtLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }            
        }

        private void TxtKG_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void CboItems_TextChanged(object sender, EventArgs e)
        {
            txtCode.Text = "";
        }

        private void CboItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            cnn.con.Close();
            if (CboItems.Text.Trim() != "")
            {
                //add all data to control box
                SqlDataAdapter sda = new SqlDataAdapter("Select ItemCode From tbMasterItem Where Len(ItemCode)<5 AND ItemName='" + CboItems.Text + "'", cnn.con);
                try
                {
                    cnn.con.Open();
                    //Fill the DataTable with records from Table.
                    DataTable RMCode = new DataTable();
                    sda.Fill(RMCode);
                    if (RMCode.Rows.Count > 0)
                    {
                        txtCode.Text = RMCode.Rows[0][0].ToString();
                        txtKG.Focus();
                    }                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnn.con.Close();    
            }
        }

        private void MstUncountMatFormAdd_Load(object sender, EventArgs e)
        {            
            //add all data to control box
            SqlDataAdapter sda = new SqlDataAdapter("Select ItemCode, ItemName From tbMasterItem Where Len(ItemCode)<5", cnn.con);
            try
            {
                cnn.con.Open();
                //Fill the DataTable with records from Table.
                DataTable dt = new DataTable();
                sda.Fill(dt);

                //Insert the Default Item to DataTable.
                DataRow row = dt.NewRow();
                row[0] = "";
                dt.Rows.InsertAt(row, 0);

                //Assign DataTable as DataSource.
                CboItems.DataSource = dt;
                CboItems.DisplayMember = "ItemName";

                //Set AutoCompleteMode.
                CboItems.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                CboItems.AutoCompleteSource = AutoCompleteSource.ListItems;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            PicKG.Visible = false;
            PicLength.Visible = false;
            PicItem.Visible = false;
            
            if (txtCode.Text.Trim() == "" || txtKG.Text.Trim() == "" || txtLength.Text.Trim() == "")
            {                
                //3
                //1,2,3
                if (txtCode.Text.Trim() == "" && txtKG.Text.Trim() == "" && txtLength.Text.Trim() == "")
                {
                    PicKG.Visible = true;
                    PicItem.Visible = true;
                    PicLength.Visible = true;
                    CboItems.Focus();

                }                
                //2
                //1,2
                else if (txtCode.Text.Trim() == "" && txtKG.Text.Trim() == "")
                {
                    PicKG.Visible = true;
                    PicItem.Visible = true;
                    CboItems.Focus();

                }
                //1,3
                else if (txtCode.Text.Trim() == "" && txtLength.Text.Trim() == "")
                {
                    PicItem.Visible = true;
                    PicLength.Visible = true;
                    CboItems.Focus();

                }
                //2,3
                else if (txtKG.Text.Trim() == "" && txtLength.Text.Trim() == "")
                {
                    PicKG.Visible = true;
                    PicLength.Visible = true;
                    txtKG.Focus();

                }

                //1
                else if (txtCode.Text.Trim() == "")
                {
                    PicItem.Visible = true;
                    CboItems.Focus();

                }
                else if (txtKG.Text.Trim() == "")
                {
                    PicKG.Visible = true;
                    txtKG.Focus();

                }
                else if (txtLength.Text.Trim() == "")
                {
                    PicLength.Visible = true;
                    txtLength.Focus();

                }
                
                else
                {

                }
            }
            else
            {
                DialogResult DLR = MessageBox.Show("Do you want to this into the system?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    try
                    {
                        cnn.con.Open();
                        cmd = new SqlCommand("INSERT INTO MstUncountMat (LowItemCode, TotalKG, TotalM, RegDate, RegBy, UpdateDate, UpdateBy) VALUES (@Cd, @KG, @M, @RDate, @RBy, @UDate, @UBy)", cnn.con);
                        cmd.Parameters.AddWithValue("@Cd", txtCode.Text.ToString());
                        cmd.Parameters.AddWithValue("@KG",Convert.ToDouble(txtKG.Text));
                        cmd.Parameters.AddWithValue("@M", Convert.ToDouble(txtLength.Text));
                        cmd.Parameters.AddWithValue("@RDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@RBy", user);
                        cmd.Parameters.AddWithValue("@UDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UBy", user);
                        try
                        {

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Successfully added !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            fgrid.dgvRMUncount.Rows.Add(txtCode.Text, CboItems.Text, Convert.ToDouble(txtKG.Text.ToString()), Convert.ToDouble(txtLength.Text.ToString()), DateTime.Now, user, DateTime.Now, user);
                            CboItems.Text = "";
                            txtCode.Text = "";
                            txtKG.Text = "";
                            txtLength.Text = "";
                            CboItems.Focus();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("This Code already exists, please recheck it !"+ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Something wrong ! Please check the connection first !\nOr contact to developer !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    cnn.con.Close();
                }
                else
                {

                }
            }
        }

    }
}
