using MachineDeptApp.Admin;
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
    public partial class MstUncountMatForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        DataTable dtRmUncount;

        public static string CodeForNextForm = "";
        public static string UsernameForNextForm = "";
        SqlCommand cmd = new SqlCommand();
        string CodeSelected;


        public MstUncountMatForm()
        {
            InitializeComponent();
            cnn.Connection();
            this.dgvRMUncount.CellClick += DgvRMUncount_CellClick;

        }

        private void DgvRMUncount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CodeSelected = "";
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvRMUncount.Rows[e.RowIndex];
                CodeSelected = row.Cells[0].Value.ToString();

            }
            else
            {

            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            UsernameForNextForm = MenuFormV2.UserForNextForm;
            MstUncountMatFormAdd Mumfa = new MstUncountMatFormAdd(this);
            Mumfa.ShowDialog();

        }

        private void MstUncountMatForm_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dgvRMUncount.Rows.Clear();
            if (txtItemname.Text.Trim() !="")
            {
                try
                {
                    cnn.con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * From (\tSelect * From MstUncountMat) t1 INNER JOIN (Select ItemCode, ItemName From tbMasterItem Where LEN(ItemCode)<5 AND ItemName Like '%"+txtItemname.Text+"%') t2 ON t1.LowItemCode=t2.ItemCode", cnn.con);
                    dtRmUncount = new DataTable();
                    da.Fill(dtRmUncount);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnn.con.Close();

                //Add to dgv
                if (dtRmUncount.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRmUncount.Rows.Count; i++)
                    {
                        dgvRMUncount.Rows.Add(dtRmUncount.Rows[i][0].ToString(), dtRmUncount.Rows[i][8].ToString(), Convert.ToDouble(dtRmUncount.Rows[i][1].ToString()), Convert.ToDouble(dtRmUncount.Rows[i][2].ToString()), Convert.ToDateTime(dtRmUncount.Rows[i][3].ToString()), dtRmUncount.Rows[i][4].ToString(), Convert.ToDateTime(dtRmUncount.Rows[i][5].ToString()), dtRmUncount.Rows[i][6].ToString());

                    }
                    dgvRMUncount.ClearSelection();
                }
            }
            else
            {
                try
                {
                    cnn.con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select * From (	Select * From MstUncountMat) t1 INNER JOIN (Select ItemCode, ItemName From tbMasterItem Where LEN(ItemCode)<5) t2 ON t1.LowItemCode=t2.ItemCode", cnn.con);
                    dtRmUncount = new DataTable();
                    da.Fill(dtRmUncount);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!\n" + ex.Message, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnn.con.Close();

                //Add to dgv
                if (dtRmUncount.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRmUncount.Rows.Count; i++)
                    {
                        dgvRMUncount.Rows.Add(dtRmUncount.Rows[i][0].ToString(), dtRmUncount.Rows[i][8].ToString(),Convert.ToDouble(dtRmUncount.Rows[i][1].ToString()), Convert.ToDouble(dtRmUncount.Rows[i][2].ToString()), Convert.ToDateTime(dtRmUncount.Rows[i][3].ToString()), dtRmUncount.Rows[i][4].ToString(), Convert.ToDateTime(dtRmUncount.Rows[i][5].ToString()), dtRmUncount.Rows[i][6].ToString());

                    }
                    dgvRMUncount.ClearSelection();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CodeSelected == "")
            {

            }
            else
            {
                CodeForNextForm = CodeSelected;
                CodeSelected = "";
                UsernameForNextForm = MenuFormV2.UserForNextForm;
                MstUncountMatFormUpdate Mumfu = new MstUncountMatFormUpdate(this);
                Mumfu.ShowDialog();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CodeSelected == "")
            {

            }
            else
            {
                DialogResult DLR = MessageBox.Show("Do you want to delete this ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DLR == DialogResult.Yes)
                {
                    try
                    {
                        cnn.con.Open();
                        //Delete in SaleHis                     
                        SqlCommand cmd = new SqlCommand("DELETE FROM MstUncountMat WHERE LowItemCode ='" + CodeSelected + "';", cnn.con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully deleted !", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        int selectedIndex = dgvRMUncount.CurrentCell.RowIndex;
                        if (selectedIndex > -1)
                        {
                            dgvRMUncount.Rows.RemoveAt(selectedIndex);
                            dgvRMUncount.Refresh();
                        }
                        dgvRMUncount.ClearSelection();
                        CodeSelected = "";

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
