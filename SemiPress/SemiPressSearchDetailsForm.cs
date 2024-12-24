using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.SemiPress
{
    public partial class SemiPressSearchDetailsForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SemiPressSearchForm fgrid;
        DataTable dtSemiAuto;
        DataTable dtSemiPress;

        public SemiPressSearchDetailsForm(SemiPressSearchForm fg)
        {
            InitializeComponent();
            this.fgrid = fg;
            cnn.Connection();

        }

        private void SemiPressSearchDetailsForm_Load(object sender, EventArgs e)
        {
            string POS = SemiPressSearchForm.POSNoSelectedForNextForm.ToString();
            string Code = SemiPressSearchForm.CodeSelectedForNextForm.ToString();
            string Items = SemiPressSearchForm.ItemsSelectedForNextForm.ToString();
            double Qty = Convert.ToDouble(SemiPressSearchForm.QtySelectedForNextForm.ToString());
            this.Text = POS+"  ||  "+ Items + "  ||  " +Qty.ToString("N0");

            //SemiAuto
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * FROM "+
                                                                            "(Select ItemCode, ItemName From tbMasterItem Where ItemCode = '"+Code+"') t1 "+
                                                                            "INNER JOIN "+
                                                                            "(Select PosNo, WipCode, BoxNo, Qty, Remarks, RegDate From tbSemiPress Where PosNo = '"+POS+"' AND DeleteState = 0) t2 "+
                                                                            "ON t1.ItemCode = t2.WipCode", cnn.con);
                dtSemiAuto = new DataTable();
                da.Fill(dtSemiAuto);
                //to get rows
                double Total = 0;
                foreach (DataRow row in dtSemiAuto.Rows)
                {
                    Total = Total + Convert.ToDouble(row[5].ToString());
                    dgvSemiAuto.Rows.Add(row[2], row[0], row[1], row[4], Convert.ToDouble(row[5].ToString()), row[6],Convert.ToDateTime(row[7].ToString()));

                }

                if (dgvSemiAuto.Rows.Count > 0)
                {
                    int lastrowofDgv = dgvSemiAuto.RowCount;
                    dgvSemiAuto.Rows.Insert(lastrowofDgv, "", "", "", "សរុប​ ", Total);
                    dgvSemiAuto.Rows[lastrowofDgv].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvSemiAuto.Rows[lastrowofDgv].DefaultCellStyle.Font = new System.Drawing.Font("Khmer OS Battambang", 11, FontStyle.Bold);

                }
            }
            catch
            {
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dgvSemiAuto.ClearSelection();

            //SemiPress
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("Select * FROM " +
                                                                            "(Select ItemCode, ItemName From tbMasterItem Where ItemCode = '" + Code + "') t1 " +
                                                                            "INNER JOIN " +
                                                                            "(Select PosNo, WipCode, BoxNo, Qty, Remarks, RegDate From tbSemiPress2 Where PosNo = '" + POS + "' AND DeleteState = 0) t2 " +
                                                                            "ON t1.ItemCode = t2.WipCode", cnn.con);
                dtSemiPress = new DataTable();
                da.Fill(dtSemiPress);
                //to get rows
                double Total = 0;
                foreach (DataRow row in dtSemiPress.Rows)
                {
                    Total = Total + Convert.ToDouble(row[5].ToString());
                    dgvSemiPress.Rows.Add(row[2], row[0], row[1], row[4], Convert.ToDouble(row[5].ToString()), row[6], Convert.ToDateTime(row[7].ToString()));

                }
                if (dgvSemiPress.Rows.Count > 0)
                {
                    int lastrowofDgv = dgvSemiPress.RowCount;
                    dgvSemiPress.Rows.Insert(lastrowofDgv, "", "", "", "សរុប​ ", Total);
                    dgvSemiPress.Rows[lastrowofDgv].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvSemiPress.Rows[lastrowofDgv].DefaultCellStyle.Font = new System.Drawing.Font("Khmer OS Battambang", 11, FontStyle.Bold);

                }
            }
            catch
            {
                MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dgvSemiPress.ClearSelection();

        }
    }
}
