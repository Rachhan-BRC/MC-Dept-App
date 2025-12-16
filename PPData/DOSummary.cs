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

namespace MachineDeptApp.SparePartControll
{
    public partial class DoSummary : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnectOBS oBS = new SQLConnectOBS();
        DataTable dt = new DataTable();
        public DoSummary()
        {
            InitializeComponent();
            cnn.Connection();
            oBS.Connection();
            btnSearch.Click += BtnSearch_Click;
            dgvDo.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            tbS3Ncap.TextChanged += TbS3Ncap_TextChanged;
            tbAssyCap.TextChanged += TbS3Ncap_TextChanged;
            tbS3Ncap.KeyPress += TbS3Ncap_KeyPress;
            tbAssyCap.KeyPress += TbS3Ncap_KeyPress;
            dtpShipmentEnd.ValueChanged += DtpShipmentStart_ValueChanged;
            dtpShipmentStart.ValueChanged += DtpShipmentStart_ValueChanged;
        }

        private void DtpShipmentStart_ValueChanged(object sender, EventArgs e)
        {
            cbShipement.Checked = true;
        }

    
        private void TbS3Ncap_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Block the key
            }
            throw new NotImplementedException();
        }

        private void TbS3Ncap_TextChanged(object sender, EventArgs e)
        {
            for (int i = 1; i < dgvDo.Columns.Count - 1; i++)
            {
                if (double.TryParse((string)dgvDo.Rows[5].Cells[i].Value, out double QtyS3N) && double.TryParse(tbS3Ncap.Text, out double S3NCap))
                {
                    dgvDo.Rows[7].Cells[i].Value = Convert.ToDouble(QtyS3N / S3NCap).ToString("N2");
                }
                if (double.TryParse((string)dgvDo.Rows[6].Cells[i].Value, out double QtyAssy) && double.TryParse(tbAssyCap.Text, out double AssyCap))
                {
                    dgvDo.Rows[8].Cells[i].Value = Convert.ToDouble(QtyAssy / AssyCap).ToString("N2");
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            dgvDo.Columns.Clear();
            dgvDo.Columns.Add("DoAmount", " Do Amount");
            dt.Rows.Clear();
            dt.Columns.Clear();
            dt.Columns.Add();
            dt.Columns.Add();
            dt.Columns.Add();
            dt.Columns.Add();
            string query = "";
            if (cbShipement.Checked == true)
            {
                query = " where ShipmentDate between '" + dtpShipmentStart.Value.ToString("yyyy-MM-dd") + "'  and '" + dtpShipmentEnd.Value.ToString("yyyy-MM-dd") + "' ";
            } else
            {
                query = " WHERE ShipmentDate >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)  AND ShipmentDate < DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) + 1, 0) ";
            }

            try { 
            SqlDataAdapter ShipmentData = new SqlDataAdapter("select Code, Item , ShipmentDate, Qty from [PPDeptDB].dbo.tbDeliveryOrder " + query + " order by Code", cnn.con);
            DataTable ShipmentDoData = new DataTable();
            ShipmentData.Fill(ShipmentDoData);
            SqlDataAdapter shipmentDate = new SqlDataAdapter(" select ShipmentDate from [PPDeptDB].dbo.tbDeliveryOrder " + query + "Group By ShipmentDate order by ShipmentDate", cnn.con);
            DataTable dtshipmentDate = new DataTable();
            shipmentDate.Fill(dtshipmentDate);
            SqlDataAdapter sqlTypeAssyS3N = new SqlDataAdapter("select Code , LineCode from [PPDeptDB].dbo.tbFGMater ", cnn.con);
            DataTable dtAssyS3N = new DataTable();
            sqlTypeAssyS3N.Fill(dtAssyS3N);

            SqlDataAdapter sqlMaster = new SqlDataAdapter("  SELECT T1.ItemCode, T1.UnitPrice, T1.EffDate FROM (SELECT * FROM mstsalesprice WHERE DelFlag=0) T1  " +
                "  INNER JOIN (SELECT ItemCode, MAX(EffDate) AS EffDate FROM mstsalesprice WHERE DelFlag=0 AND EffDate <= '" + DateTime.Now.Date.ToString("yyyy-MM-dd") + " 23:59:59' GROUP BY ItemCode) T2  " +
                "  ON T1.ItemCode=T2.ItemCode AND T1.EffDate=T2.EffDate ", oBS.conOBS);
            DataTable dataUP = new DataTable();

            sqlMaster.Fill(dataUP);
            ShipmentDoData.Columns.Add("LineCode");
            ShipmentDoData.Columns.Add("UP");
                //  Console.WriteLine(ShipmentDoData.Rows.Count);
                foreach (DataRow row in ShipmentDoData.Rows)
                {
                    foreach (DataRow dr in dtAssyS3N.Rows)
                    {
                        if (dr[0].ToString() == row[0].ToString())
                        {
                            row["LineCode"] = dr[1];
                            //Console.WriteLine(Convert.ToInt32(dr[1].ToString()));
                            ShipmentDoData.AcceptChanges();
                            break;
                        }
                    }
                    foreach (DataRow dr in dataUP.Rows)
                    {
                        if (dr[0].ToString() == row[0].ToString())
                        {
                            row["UP"] = dr[1];
                            ShipmentDoData.AcceptChanges();
                            // Console.WriteLine(dr[1].ToString());
                            break;
                        }
                    }
                }
                foreach (DataRow row in dtshipmentDate.Rows)
                {
                    dgvDo.Columns.Add(row[0].ToString(), Convert.ToDateTime(row[0].ToString()).ToString("dd-MM-yyyy"));
                }
                dgvDo.Rows.Add("S3N");
                dgvDo.Rows.Add("ASSY");
                dgvDo.Rows.Add("TOTAL");
                dgvDo.Rows.Add("S3N%");
                dgvDo.Rows.Add("ASSY%");
                dgvDo.Rows.Add("S3N Qty");
                dgvDo.Rows.Add("ASSY Qty");
                dgvDo.Rows.Add("S3N Day");
                dgvDo.Rows.Add("ASSY Day");

                double upKS3N = 0;
                double upKAssy = 0;
                double upKtotal = 0;
                for (int i = 1; i < dgvDo.Columns.Count; i++)
                {
                    double S3NQty = 0;
                    double AssyQty = 0;
                    double totalQty = 0;
                    double s3nUpK = 0;
                    double assyUpK = 0;
                    double totalUpK = 0;
                    double itemQtyS3N = 0;
                    double itemQtyAssy = 0;
                    foreach (DataRow row in ShipmentDoData.Rows)
                    {
                        if (row[2].ToString() == Convert.ToDateTime(dgvDo.Columns[i].Name.ToString()).ToString())
                        {
                            double value = 0;
                            double itemQty = 0;
                            if (row["UP"] != DBNull.Value) // Check for DBNull
                            {
                                value = Convert.ToDouble(row["UP"]);
                            }
                            if (row["Qty"] != DBNull.Value)
                            {
                                itemQty = Convert.ToDouble(row["Qty"].ToString());
                            }

                            totalQty += itemQty * value;
                            totalUpK += itemQty * value;
                            if (row["LineCode"].ToString() == "ASSY")
                            {
                                itemQtyAssy += itemQty;
                                AssyQty += itemQty * value;
                                assyUpK += itemQty * value;
                            }
                            else if (row["LineCode"].ToString() == "S3N")
                            {
                                itemQtyS3N += itemQty;
                                S3NQty += itemQty * value;
                                s3nUpK += itemQty * value;

                            }
                        }
                    }
                    upKS3N += s3nUpK;
                    upKAssy += assyUpK;
                    upKtotal += totalUpK;
                    double S3Nper = Convert.ToDouble(S3NQty) / Convert.ToDouble(totalQty) * 100;
                    double Assyper = Convert.ToDouble(AssyQty) / Convert.ToDouble(totalQty) * 100;
                    dgvDo.Rows[0].Cells[Convert.ToDateTime(dgvDo.Columns[i].Name.ToString()).ToString()].Value = S3NQty.ToString("N2");
                    dgvDo.Rows[1].Cells[Convert.ToDateTime(dgvDo.Columns[i].Name.ToString()).ToString()].Value = AssyQty.ToString("N2");
                    dgvDo.Rows[2].Cells[Convert.ToDateTime(dgvDo.Columns[i].Name.ToString()).ToString()].Value = totalQty.ToString("N2");
                    dgvDo.Rows[3].Cells[Convert.ToDateTime(dgvDo.Columns[i].Name.ToString()).ToString()].Value = S3Nper.ToString("N2") + " %";
                    dgvDo.Rows[4].Cells[Convert.ToDateTime(dgvDo.Columns[i].Name.ToString()).ToString()].Value = Assyper.ToString("N2") + " %";
                    dgvDo.Rows[5].Cells[Convert.ToDateTime(dgvDo.Columns[i].Name.ToString()).ToString()].Value = itemQtyS3N.ToString("N2");
                    dgvDo.Rows[6].Cells[Convert.ToDateTime(dgvDo.Columns[i].Name.ToString()).ToString()].Value = itemQtyAssy.ToString("N2");
                    dgvDo.Rows[7].Cells[Convert.ToDateTime(dgvDo.Columns[i].Name.ToString()).ToString()].Value = itemQtyS3N.ToString("N2");
                    dgvDo.Rows[8].Cells[Convert.ToDateTime(dgvDo.Columns[i].Name.ToString()).ToString()].Value = itemQtyS3N.ToString("N2");

                }
                dgvDo.Columns.Add("Unit", "Total Amount");
                dgvDo.Rows[0].Cells["Unit"].Value = Convert.ToDouble(upKS3N / 1000).ToString("N2") + " K$";
                dgvDo.Rows[1].Cells["Unit"].Value = Convert.ToDouble(upKAssy / 1000).ToString("N2") + " K$";
                dgvDo.Rows[2].Cells["Unit"].Value = Convert.ToDouble(upKtotal / 1000).ToString("N2") + " K$";
                dgvDo.Rows[3].Cells["Unit"].Value = Convert.ToDouble(upKS3N / upKtotal * 100).ToString("N2") + " %";
                dgvDo.Rows[4].Cells["Unit"].Value = Convert.ToDouble(upKAssy / upKtotal * 100).ToString("N2") + " %";

                foreach (DataRow row in ShipmentDoData.Rows)
                {
                    if (row["UP"] == DBNull.Value)
                    {
                        dt.Rows.Add(row["Code"], row["Item"], row["LineCode"], row["UP"]);
                    }
                }
                dgvDo.Rows[2].DefaultCellStyle.BackColor = Color.LightBlue;
                dgvDo.Rows[3].DefaultCellStyle.BackColor = Color.LightGreen;
                dgvDo.Rows[4].DefaultCellStyle.BackColor = Color.GreenYellow;
                dgvDo.Rows[7].DefaultCellStyle.BackColor = Color.LightYellow;
                dgvDo.Rows[8].DefaultCellStyle.BackColor = Color.LightPink;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
