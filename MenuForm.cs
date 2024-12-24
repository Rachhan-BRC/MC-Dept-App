using MachineDeptApp.Admin;
using MachineDeptApp.InputTransferSemi;
using MachineDeptApp.SemiPress;
using MachineDeptApp.SemiPress.SemiPress1;
using MachineDeptApp.SemiPress.SemiPress2;
using MachineDeptApp.TransferData;
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

namespace MachineDeptApp
{
    public partial class MenuForm : System.Windows.Forms.Form
    {
        SQLConnect cnn = new SQLConnect();
        string LogRole;
        string name;
        public static string NameForNextForm = "";

        public MenuForm()
        {
            InitializeComponent();
            cnn.Connection();
            this.treeView1.DoubleClick += TreeView1_DoubleClick;
            this.FormClosing += MenuForm_FormClosing;

        }

        private void MenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult DLS = MessageBox.Show("តើអ្នកចង់ចាកចេញពីកម្មវិធីនេះមែន ឬទេ?", "Rachhan System", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (DLS == DialogResult.Yes)
            {
                Application.ExitThread();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void TreeView1_DoubleClick(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Text == "User Account")
            {
                NameForNextForm = LbLoginID.Text;
                this.Hide();
                UserForm uf = new UserForm();
                uf.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "បញ្ចូលទិន្នន័យតាមរយៈបាកូដ OBS")
            {
                NameForNextForm = LbLoginID.Text;
                this.Hide();
                InputTransferSemiForm itsf = new InputTransferSemiForm();
                itsf.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "បញ្ចូលទិន្នន័យដោយដៃ")
            {
                NameForNextForm = LbLoginID.Text;
                this.Hide();
                InputTransferSemiByManaulForm itsbmf = new InputTransferSemiByManaulForm();
                itsbmf.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "ទិន្នន័យវេរសឺមី")
            {
                this.Hide();
                TransferDataForm tdf = new TransferDataForm();
                tdf.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "កែប្រែទិន្នន័យវេរចេញ")
            {
                NameForNextForm = LbLoginID.Text;
                this.Hide();
                TransferDataEditForm tdef = new TransferDataEditForm();
                tdef.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "ទាញទិន្នន័យ POS ថ្មីពី OBS")
            {
                NameForNextForm = LbLoginID.Text;
                this.Hide();
                TrackingPOS.RetriveDataFromOBSForm Rdfof = new TrackingPOS.RetriveDataFromOBSForm();
                Rdfof.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "ឆែកទិន្នន័យ POS")
            {
                NameForNextForm = LbLoginID.Text;
                this.Hide();
                TrackingPOS.POSTrackingSearchForm Ptsf = new TrackingPOS.POSTrackingSearchForm();
                Ptsf.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "Semi MstBOM")
            {
                NameForNextForm = LbLoginID.Text;
                this.Hide();
                NG_Input.MstBOMForm mbf = new NG_Input.MstBOMForm();
                mbf.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "បញ្ចូល NG")
            {
                NameForNextForm = LbLoginID.Text;
                this.Hide();
                NG_Input.NGInputForm Nif = new NG_Input.NGInputForm();
                Nif.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "MstUncountable Material")
            {
                NameForNextForm = LbLoginID.Text;
                this.Hide();
                NG_Input.MstUncountMatForm Mumf = new NG_Input.MstUncountMatForm();
                Mumf.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "NG Records")
            {
                NameForNextForm = LbLoginID.Text;
                this.Hide();
                NG_Input.NGHistoryForm Nhf = new NG_Input.NGHistoryForm();
                Nhf.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "បញ្ចូលដំណាក់កាល Auto (បាកូដ)")
            {
                NameForNextForm = LbLoginID.Text;
                this.Hide();
                SemiPress.SemiPressInputForm Spif = new SemiPress.SemiPressInputForm();
                Spif.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "បញ្ចូលដំណាក់កាល Auto (ដោយដៃ)")
            {
                NameForNextForm = LbLoginID.Text;
                this.Hide();
                SemiPress.SemiPressInputByManualForm Spibmf = new SemiPress.SemiPressInputByManualForm();
                Spibmf.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "ស្វែងរកទិន្នន័យ Auto")
            {
                NameForNextForm = LbLoginID.Text;
                this.Hide();
                SemiPress1SearchForm Sp1sf = new SemiPress1SearchForm();
                Sp1sf.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "បញ្ចូលដំណាក់កាល Semi Press (បាកូដ)")
            {
                NameForNextForm = LbLoginID.Text;
                this.Hide();
                SemiPress2.SemiPress2InputForm Sp2if = new SemiPress2.SemiPress2InputForm();
                Sp2if.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "ស្វែងរកទិន្នន័យ Semi Press")
            {
                NameForNextForm = LbLoginID.Text;
                this.Hide();
                SemiPress2SearchForm Sp2sf = new SemiPress2SearchForm();
                Sp2sf.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "ឆែកទិន្នន័យ")
            {
                //NameForNextForm = LbLoginID.Text;
                this.Hide();
                SemiPressSearchForm Spsf = new SemiPressSearchForm();
                Spsf.ShowDialog();
                this.Close();

            }
            else if (treeView1.SelectedNode.Text == "ទិន្នន័យចេញចូលស្តុក OBS")
            {
                this.Hide();
                OBS.OBSTransactionForm Otf = new OBS.OBSTransactionForm();
                Otf.ShowDialog();
                this.Close();

            }
            else
            {

            }
        }

        private void btnCredit_Click(object sender, EventArgs e)
        {
            CreditForm cf = new CreditForm();
            cf.ShowDialog();

        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
           //Check Date
           DateTime dt = DateTime.Now;
            string Year = dt.ToString("yyyy");
            if (Convert.ToInt64(Year) < 2020)
            {
                MessageBox.Show("សូមកំណត់ម៉ោងឱ្យបានត្រឹមត្រូវមុនប្រើប្រាស់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.treeView1.Enabled = false;
            }
            else if (Convert.ToInt64(Year) > 2024)
            {
                MessageBox.Show("កម្មវិធីនេះត្រូវបានផុតកំណត់ហើយ!​ \nសូមទាក់ទងទៅកាន់​ រ៉ាឆាន់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.treeView1.Enabled = false;
            }
            else
            {
                ToolTip toolTip1 = new ToolTip();
                // Set up the delays for the ToolTip.
                toolTip1.AutoPopDelay = 5000;
                toolTip1.InitialDelay = 500;
                toolTip1.ReshowDelay = 250;
                // Force the ToolTip text to be displayed whether or not the form is active.
                toolTip1.ShowAlways = true;
                // Set up the ToolTip text for the Button and Checkbox.
                toolTip1.SetToolTip(this.btnCredit, "ក្រេដីត");
                toolTip1.SetToolTip(this.LbLogo, "Marunix (Cambodia) Co., Ltd");

                LbLoginID.Text = LoginForm.IDValueForNextForm;
                try
                {
                    cnn.con.Open();
                    string id = LbLoginID.Text.ToString();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tbUser WHERE ID = '" + id + "';", cnn.con);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Role");
                    LogRole = ds.Tables["Role"].Rows[0][2].ToString();
                    name = ds.Tables["Role"].Rows[0][1].ToString();
                    LbLoginID.Text = name;

                    if (LogRole != "Admin")
                    {
                        //remove Admin 
                        treeView1.Nodes[6].Remove();
                        //treeView1.Nodes[2].Nodes[1].Remove();

                    }
                    else
                    {
                        //treeView1.Nodes[1].Nodes[0].Remove();
                        
                    }
                }
                catch
                {
                    MessageBox.Show("មានបញ្ហាអ្វីមួយ ! សូមពិនិត្យមលើការភ្ជាប់បណ្ដាញ \nឬ​ក៏សួរទៅកាន់រ៉ាឆាន់!", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cnn.con.Close();
            }            
        }

    }
}
