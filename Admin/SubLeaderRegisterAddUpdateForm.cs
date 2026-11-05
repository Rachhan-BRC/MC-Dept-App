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

namespace MachineDeptApp.Admin
{
    public partial class SubLeaderRegisterAddUpdateForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        SQLConnect cnnMC = new SQLConnect();
        string AddOrUpdate, ErrorText;
        DataGridView dgvMain;
        DataTable dtAssyStaff;


        public SubLeaderRegisterAddUpdateForm(string type, DataGridView dgv)
        {
            InitializeComponent();
            this.cnn.Connection();
            this.cnnMC.Connection();
            this.AddOrUpdate = type;
            this.dgvMain = dgv;
            this.Shown += SubLeaderRegisterAddUpdateForm_Shown;
            this.LbStartD.Click += LbStartD_Click;
            this.chkStartD.CheckedChanged += ChkStartD_CheckedChanged;
            this.btnSave.EnabledChanged += BtnSave_EnabledChanged;
            this.btnSave.Click += BtnSave_Click;

            this.txtID.TextChanged += TxtID_TextChanged;
            this.txtName.TextChanged += CheckForBtnSave;
            this.txtNameKH.TextChanged += CheckForBtnSave;
            this.cboSex.TextChanged += CheckForBtnSave;
            this.cboPosition.TextChanged += CheckForBtnSave;

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (AddOrUpdate != "Update")
                AddToDB();
            else
                UpdateToDB();
        }
        private void TxtID_TextChanged(object sender, EventArgs e)
        {
            if (txtID.Text.Trim() != "")
            {
                string ID = txtID.Text;
                ID = ID.ToUpper().Replace("MKH-","");
                bool Found = false;

                //Taking From TD
                if (txtID.ReadOnly == false)
                {
                    try
                    {
                        cnnMC.con.Open();
                        string SQLQuery = "SELECT * FROM [TD_Database].[dbo].[Employee_list] WHERE Staff_No = 'MKH-" + ID + "' ";
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnMC.con);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            txtNFC.Text = dt.Rows[0]["Card_ID"].ToString();
                            txtName.Text = dt.Rows[0]["Name"].ToString().ToUpper();
                            txtNameKH.Text = dt.Rows[0]["Khmer_Name"].ToString();
                            if (dt.Rows[0]["Sex"].ToString() == "Male")
                                cboSex.Text = "ប្រុស";
                            else
                                cboSex.Text = "ស្រី";
                            cboPosition.Text = dt.Rows[0]["Position"].ToString();
                            if (dt.Rows[0]["Started"].ToString().Trim() != "")
                            {
                                chkStartD.Checked = true;
                                dtpStartD.Value = Convert.ToDateTime(dt.Rows[0]["Started"]);
                            }
                            else
                                chkStartD.Checked = false;
                            Found = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorText = "Taking From TD DB : " + ex.Message;
                    }
                    cnnMC.con.Close();
                }
                //Taking From Assy DB
                else
                {
                    try
                    {
                        cnn.con.Open();
                        string SQLQuery = "SELECT * FROM MstSubL WHERE SubL_IDNo = 'MKH-" + ID + "' ";
                        SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            txtNFC.Text = dt.Rows[0]["NFC_ID"].ToString();
                            txtName.Text = dt.Rows[0]["Name"].ToString().ToUpper();
                            txtNameKH.Text = dt.Rows[0]["NameKH"].ToString();
                            if(dt.Rows[0]["Sex"].ToString() == "Male")
                                cboSex.Text = "ប្រុស";
                            else
                                cboSex.Text = "ស្រី";
                            cboPosition.Text = dt.Rows[0]["Position"].ToString();
                            if (dt.Rows[0]["StartDate"].ToString().Trim() != "")
                            {
                                chkStartD.Checked = true;
                                dtpStartD.Value = Convert.ToDateTime(dt.Rows[0]["StartDate"]);
                            }
                            else
                                chkStartD.Checked = false;
                            cboStaff.Text = dt.Rows[0]["StaffNameKH"].ToString();                            
                            Found = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorText = "Taking From Assy DB : " + ex.Message;
                    }
                    cnn.con.Close();
                }

                if(Found==true)
                    txtID.Text = ID;
            }
            CheckForBtnSave(sender,e);
        }
        private void BtnSave_EnabledChanged(object sender, EventArgs e)
        {
            if (btnSave.Enabled == true)
                btnSaveGRAY.SendToBack();
            else
                btnSaveGRAY.BringToFront();
        }
        private void LbStartD_Click(object sender, EventArgs e)
        {
            if(chkStartD.Checked == true)
                chkStartD.Checked = false;
            else
                chkStartD.Checked = true;
        }
        private void ChkStartD_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStartD.Checked == true)
            {
                dtpStartD.Enabled = true; 
                dtpStartD.Focus();
            }
            else
                dtpStartD.Enabled = false;
        }
        private void SubLeaderRegisterAddUpdateForm_Shown(object sender, EventArgs e)
        {
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            txtID.Select(txtID.Text.Length, 0);

            //Take MstAssyStaff
            try
            {
                cnnMC.con.Open();
                string SQLQuery = "SELECT Staff_No, Name, Khmer_Name, Sex, Started, Line, Card_ID FROM [TD_Database].[dbo].[Employee_list] " +
                    "\nWHERE Department = 'Machine' AND Position = 'Staff' " +
                    "\nORDER BY Staff_No ASC";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnnMC.con);
                dtAssyStaff = new DataTable();
                sda.Fill(dtAssyStaff);
                foreach (DataRow row in dtAssyStaff.Rows)
                {
                    cboStaff.Items.Add(row["Khmer_Name"]);
                }
            }
            catch (Exception ex)
            {
                ErrorText = "Taking Mst Machine Staff : " + ex.Message;
            }
            cnnMC.con.Close();
            if (AddOrUpdate == "Update")
            {
                this.Text = "Update Sub-Leader";
                txtID.ReadOnly = true;
                txtID.BackColor = Color.White;
                txtID.ForeColor = Color.Blue;
                txtID.Text = dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["IDCard"].Value.ToString();
                txtName.Focus();
                txtName.Select(txtName.Text.Length, 0);
            }

            Cursor = Cursors.Default;

            if (ErrorText.Trim() != "")
            {
                MessageBox.Show("មានបញ្ហា!\n"+ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        //Method
        private void CheckForBtnSave(object sender, EventArgs e)
        {
            int FoundOK = 0;
            if (txtID.Text.Trim() != "")
                FoundOK++;
            if(txtName.Text.Trim()!="")
                FoundOK++;
            if (txtNameKH.Text.Trim() != "")
                FoundOK++;
            if (cboSex.Text.Trim() != "")
                FoundOK++;
            if (cboPosition.Text.Trim() != "")
                FoundOK++;

            if (FoundOK == 5)
                btnSave.Enabled = true;
            else
                btnSave.Enabled = false;
        }        
        private void AddToDB()
        {
            //Checking Duplicate 
            ErrorText = "";
            Cursor = Cursors.WaitCursor;
            DataTable dtCheckingDup = new DataTable();
            try
            {
                cnn.con.Open();
                string SQLQuery = "SELECT * FROM MstSubL WHERE SubL_IDNo = 'MKH-"+txtID.Text+"' ";
                SqlDataAdapter sda = new SqlDataAdapter(SQLQuery, cnn.con);
                sda.Fill(dtCheckingDup);
            }
            catch (Exception ex)
            {
                ErrorText = "Checking Duplicate : " + ex.Message;
            }
            cnn.con.Close();
            Cursor = Cursors.Default;

            //Save to DB > Iif No Duplicate
            if (ErrorText.Trim()=="" && dtCheckingDup.Rows.Count == 0)
            {
                DialogResult DSL = MessageBox.Show("តើអ្នកចង់បន្ថែម Sub-Leader នេះមែនឬទេ?", MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (DSL == DialogResult.Yes)
                {
                    ErrorText = "";
                    Cursor = Cursors.WaitCursor;

                    string ID = "MKH-"+ txtID.Text;
                    string NFC = txtNFC.Text;
                    string Name = txtName.Text.ToUpper();
                    string NameKH = txtNameKH.Text;
                    string Sex = cboSex.Text;
                    if (Sex == "ប្រុស")
                        Sex = "Male";
                    else
                        Sex = "Female";
                    string Position = cboPosition.Text;
                    string StartDate = "";
                    if (chkStartD.Checked == true)
                        StartDate = dtpStartD.Value.ToString();
                    DateTime RegDate = DateTime.Now;
                    string RegBy = MenuFormV2.UserForNextForm;
                    string StaffName = "";
                    string StaffNameKH = cboStaff.Text;
                    if (cboStaff.Text.Trim() != "")
                    {
                        if (StaffNameKH == "ហ៊ឺន រ៉ា")
                            StaffName = "Ra";
                        else if (StaffNameKH == "គិន ចន្ធូ")
                            StaffName = "Thou";
                        else
                        {
                            foreach (DataRow row in dtAssyStaff.Rows)
                            {
                                if (row["Khmer_Name"].ToString() == StaffNameKH)
                                {
                                    StaffName = row["Name"].ToString();
                                    break;
                                }
                            }
                        }
                    }


                    //Save to DB
                    try
                    {
                        cnn.con.Open();
                        string SQLQuery = "INSERT INTO MstSubL (SubL_IDNo, Name, NameKH, Sex, StartDate, Position, NFC_ID, RegDate, RegBy, UpdateDate, UpdateBy, StaffName, StaffNameKH) " +
                                                                                             "VALUES (@Id, @N, @NKH, @sex, @Sdate, @Pos, @NFC, @RegD, @RegB, @UpD, @UpB, @Staff, @StaffKH)";
                        SqlCommand cmd = new SqlCommand(SQLQuery, cnn.con);
                        cmd.Parameters.AddWithValue("@Id", ID);
                        cmd.Parameters.AddWithValue("@N", Name);
                        cmd.Parameters.AddWithValue("@NKH", NameKH);
                        cmd.Parameters.AddWithValue("@sex", Sex);

                        if(StartDate.Trim()!="")
                            cmd.Parameters.AddWithValue("@Sdate", Convert.ToDateTime(StartDate));
                        else
                            cmd.Parameters.AddWithValue("@Sdate", DBNull.Value);

                        cmd.Parameters.AddWithValue("@Pos", Position);

                        if (NFC.Trim() != "")
                            cmd.Parameters.AddWithValue("@NFC", NFC);
                        else
                            cmd.Parameters.AddWithValue("@NFC", DBNull.Value);

                        if (StaffName.Trim() != "")
                        {
                            cmd.Parameters.AddWithValue("@Staff", StaffName);
                            cmd.Parameters.AddWithValue("@StaffKH", StaffNameKH);

                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Staff", DBNull.Value);
                            cmd.Parameters.AddWithValue("@StaffKH", DBNull.Value);
                        }

                        cmd.Parameters.AddWithValue("@RegD", RegDate);
                        cmd.Parameters.AddWithValue("@RegB", RegBy);
                        cmd.Parameters.AddWithValue("@UpD", RegDate);
                        cmd.Parameters.AddWithValue("@UpB", RegBy);
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        ErrorText = "Save to DB : " + ex.Message;
                    }
                    cnn.con.Close();

                    if (ErrorText.Trim() == "")
                    {
                        dgvMain.Rows.Add();
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["IDCard"].Value = ID;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["EngName"].Value = Name;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["KhName"].Value = NameKH;
                        if (StartDate.Trim() != "")
                            dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["StartWork"].Value = Convert.ToDateTime(StartDate);
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["Position"].Value = Position;
                        if (Sex == "Male")
                            dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["SubLSex"].Value = "ប្រុស";
                        else
                            dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["SubLSex"].Value = "ស្រី";
                        if (NFC.Trim() != "")
                            dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["SubLNFC"].Value = NFC;

                        if (StaffName.Trim() != "")
                            dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["ControlByStaff"].Value = cboStaff.Text;

                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["SubLRegDate"].Value = RegDate;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["SubLRegBy"].Value = MenuFormV2.UserForNextForm;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["SubLUpdateDate"].Value = RegDate;
                        dgvMain.Rows[dgvMain.Rows.Count - 1].Cells["SubLUpdateBy"].Value = MenuFormV2.UserForNextForm;
                        dgvMain.ClearSelection();
                        dgvMain.CurrentCell = null;
                    }

                    Cursor = Cursors.Default;

                }
            }

            if (ErrorText.Trim() == "")
            {
                if (dtCheckingDup.Rows.Count == 0)
                {
                    MessageBox.Show("បន្ថែមរួចរាល់!" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtID.Text = "";
                    txtNFC.Text = "";
                    txtName.Text = "";
                    txtNameKH.Text = "";
                    cboPosition.Text = "";
                    cboSex.Text = "";
                    chkStartD.Checked = false;
                    cboStaff.Text = "";
                    txtID.Focus();
                }
                else
                    MessageBox.Show("Sub-Leader នេះមានរួចហើយ!" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateToDB()
        {
            DialogResult DSL = MessageBox.Show("តើអ្នកចង់អាប់ដេតមែនឬទេ?", MenuFormV2.MsgTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DSL == DialogResult.Yes)
            {
                ErrorText = "";
                Cursor = Cursors.WaitCursor;

                string ID = "MKH-" + txtID.Text;
                string NFC = txtNFC.Text;
                string Name = txtName.Text.ToUpper();
                string NameKH = txtNameKH.Text;
                string Sex = cboSex.Text;
                if (Sex == "ប្រុស")
                    Sex = "Male";
                else
                    Sex = "Female";
                string Position = cboPosition.Text;
                string StartDate = "";
                if (chkStartD.Checked == true)
                    StartDate = dtpStartD.Value.ToString();
                DateTime UpdateDate = DateTime.Now;
                string UpdateBy = MenuFormV2.UserForNextForm;

                string StaffName = "";
                string StaffNameKH = cboStaff.Text;
                if (cboStaff.Text.Trim() != "")
                {
                    if (StaffNameKH == "ហ៊ឺន រ៉ា")
                        StaffName = "Ra";
                    else if (StaffNameKH == "គិន ចន្ធូ")
                        StaffName = "Thou";
                    else
                    {
                        foreach (DataRow row in dtAssyStaff.Rows)
                        {
                            if (row["Khmer_Name"].ToString() == StaffNameKH)
                            {
                                StaffName = row["Name"].ToString();
                                break;
                            }
                        }
                    }
                }

                //Update to DB
                try
                {
                    cnn.con.Open();
                    string SQLQuery = "UPDATE MstSubL SET Name = @N, NameKH = @NKH, Sex = @sex, StartDate = @Sdate, Position = @Pos, NFC_ID = @NFC, UpdateDate = @UpD, UpdateBy = @UpB, StaffName = @Staff, StaffNameKH = @StaffKH " +
                                                "WHERE  SubL_IDNo = @Id";
                    SqlCommand cmd = new SqlCommand(SQLQuery, cnn.con);
                    cmd.Parameters.AddWithValue("@Id", ID);
                    cmd.Parameters.AddWithValue("@N", Name);
                    cmd.Parameters.AddWithValue("@NKH", NameKH);
                    cmd.Parameters.AddWithValue("@sex", Sex);

                    if (StartDate.Trim() != "")
                        cmd.Parameters.AddWithValue("@Sdate", Convert.ToDateTime(StartDate));
                    else
                        cmd.Parameters.AddWithValue("@Sdate", DBNull.Value);

                    cmd.Parameters.AddWithValue("@Pos", Position);

                    if (NFC.Trim() != "")
                        cmd.Parameters.AddWithValue("@NFC", NFC);
                    else
                        cmd.Parameters.AddWithValue("@NFC", DBNull.Value);

                    if (StaffName.Trim() != "")
                    {
                        cmd.Parameters.AddWithValue("@Staff", StaffName);
                        cmd.Parameters.AddWithValue("@StaffKH", StaffNameKH);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Staff", DBNull.Value);
                        cmd.Parameters.AddWithValue("@StaffKH", DBNull.Value);
                    }

                    cmd.Parameters.AddWithValue("@UpD", UpdateDate);
                    cmd.Parameters.AddWithValue("@UpB", UpdateBy);
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    ErrorText = "Update to DB : " + ex.Message;
                }
                cnn.con.Close();

                Cursor = Cursors.Default;

                if (ErrorText.Trim() == "")
                {
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["EngName"].Value = Name;
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["KhName"].Value = NameKH;

                    if (StartDate.Trim() != "")
                        dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["StartWork"].Value = Convert.ToDateTime(StartDate);
                    else
                        dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["StartWork"].Value = null;

                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["Position"].Value = Position;

                    if (Sex == "Male")
                        dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["SubLSex"].Value = "ប្រុស";
                    else
                        dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["SubLSex"].Value = "ស្រី";

                    if (NFC.Trim() != "")
                        dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["SubLNFC"].Value = NFC;
                    else
                        dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["SubLNFC"].Value = null;

                    if (StaffName.Trim() != "")
                        dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["ControlByStaff"].Value = cboStaff.Text;
                    else
                        dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["ControlByStaff"].Value = null;

                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["SubLUpdateDate"].Value = UpdateDate;
                    dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["SubLUpdateBy"].Value = MenuFormV2.UserForNextForm;
                    //dgvMain.Rows[dgvMain.CurrentCell.RowIndex].Cells["LunchShift"].Value = row[""];

                    MessageBox.Show("អាប់ដេតរួចរាល់!",MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                    MessageBox.Show("មានបញ្ហា!\n" + ErrorText, MenuFormV2.MsgTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

    }
}
