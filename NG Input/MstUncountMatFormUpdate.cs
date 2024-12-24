using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.NG_Input
{
    public partial class MstUncountMatFormUpdate : Form
    {
        SQLConnect cnn = new SQLConnect();
        MstUncountMatForm fgrid;
        string Username;

        public MstUncountMatFormUpdate(MstUncountMatForm fg)
        {
            InitializeComponent();
            this.fgrid = fg;
            cnn.Connection();
            this.txtKG.KeyPress += TxtKG_KeyPress;
            this.txtKG.Leave += TxtKG_Leave;
            this.txtLength.KeyPress += TxtLength_KeyPress;
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

        private void TxtLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
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

        private void MstUncountMatFormUpdate_Load(object sender, EventArgs e)
        {
            txtCode.Text = MstUncountMatForm.CodeForNextForm;
            Username = MstUncountMatForm.UsernameForNextForm;
            //add all data to control box
            SqlCommand cmd = new SqlCommand("Select * From (Select ItemCode, ItemName from tbMasterItem) t1 INNER JOIN (Select * from MstUncountMat WHERE LowItemCode='"+txtCode.Text+"') t2 ON t1.ItemCode=t2.LowItemCode", cnn.con);
            try
            {
                cnn.con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    double KG = Convert.ToDouble(dr.GetValue(3).ToString());
                    double M = Convert.ToDouble(dr.GetValue(4).ToString());
                    txtItems.Text = dr.GetValue(1).ToString();
                    txtKG.Text = String.Format("{0:#,###.000}",KG);
                    txtLength.Text = String.Format("{0:#,###}", M);
                    
                }
            }
            catch
            {
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            cnn.con.Close();

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            PicKG.Visible = false;
            PicLength.Visible = false;
            
            if (txtKG.Text.Trim() == "" || txtLength.Text.Trim() == "")
            {                
                //2
                //1,2
                if (txtKG.Text.Trim() == "" && txtLength.Text.Trim() == "")
                {
                    PicKG.Visible = true;
                    PicLength.Visible = true;
                    txtKG.Focus();

                }
                
                //1
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
                DialogResult DLR = MessageBox.Show("Do you want to update this?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    try
                    {
                        cnn.con.Open();
                        string query = "UPDATE MstUncountMat SET " +
                                            "TotalKG='" + Convert.ToDouble(txtKG.Text.ToString()) + "'," +
                                            "TotalM='" + Convert.ToDouble(txtLength.Text.ToString()) + "'," +
                                            "UpdateDate='" + DateTime.Now + "'," +
                                            "UpdateBy=N'" + Username + "'" +
                                            "WHERE LowItemCode = '" + txtCode.Text + "';";
                        SqlCommand cmd = new SqlCommand(query, cnn.con);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Successfully updated !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        int row = Convert.ToInt16(fgrid.dgvRMUncount.CurrentRow.Index);
                        fgrid.dgvRMUncount.Rows[row].Cells[2].Value = Convert.ToDouble(txtKG.Text.ToString());
                        fgrid.dgvRMUncount.Rows[row].Cells[3].Value = Convert.ToDouble(txtLength.Text.ToString());
                        fgrid.dgvRMUncount.Rows[row].Cells[6].Value = Convert.ToDateTime(DateTime.Now);
                        fgrid.dgvRMUncount.Rows[row].Cells[7].Value = Username;
                        fgrid.dgvRMUncount.ClearSelection();
                        this.Close();

                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message+"\nSomething wrong ! Please check the connection first !\nOr contact to developer !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
