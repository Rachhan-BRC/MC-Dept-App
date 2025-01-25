using MachineDeptApp.Admin;
using MachineDeptApp.InputTransferSemi;
using MachineDeptApp.MCPlans;
using MachineDeptApp.MCSDControl.SDRec;
using MachineDeptApp.MCSDControl;
using MachineDeptApp.NG_Input;
using MachineDeptApp.OBS;
using MachineDeptApp.SemiNGReq;
using MachineDeptApp.SemiPress;
using MachineDeptApp.SemiPress.SemiPress1;
using MachineDeptApp.SemiPress.SemiPress2;
using MachineDeptApp.SemiPress2;
using MachineDeptApp.TrackingPOS;
using MachineDeptApp.TransferData;
using MachineDeptApp.Inventory.KIT;
using MachineDeptApp.Inventory.MC_SD;
using MachineDeptApp.Inventory.Inprocess;
using MachineDeptApp.AllSection;
using MachineDeptApp.MCSDControl.WIR1__Wire_Stock_;
using MachineDeptApp.Inventory;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace MachineDeptApp
{
    public partial class MenuFormV2 : Form
    {
        SQLConnect cnn = new SQLConnect();
        string LogRole;
        string name;
        public static string UserForNextForm;

        DataTable dtRoot;
        DataTable dtChildRoot;
        DataTable dtChildRootofChild;
        DataTable dtOpenForm;
        
        public MenuFormV2()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.FormClosing += MenuFormV2_FormClosing;
            this.treeViewMenu.NodeMouseDoubleClick += TreeViewMenu_NodeMouseDoubleClick;
            this.treeViewMenu.NodeMouseClick += TreeViewMenu_NodeMouseClick;
            this.tabControlOpenForm.SelectedIndexChanged += TabControlOpenForm_SelectedIndexChanged;
            this.MdiChildActivate += MenuFormV2_MdiChildActivate;
            this.btnCheckForUpdate.Click += BtnCheckForUpdate_Click;
            
        }

        private void BtnCheckForUpdate_Click(object sender, EventArgs e)
        {
            DialogResult DSL = MessageBox.Show("ដើម្បីដំណើរការមុខងារនេះ កម្មវិធីនឹងតម្រូវឱ្យបិទ!\nតើអ្នកចង់បន្តដែរឬទេ?","Rachhan System",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (DSL == DialogResult.Yes)
            {
                try
                {
                    //Open UpdateChecker 
                    System.Diagnostics.Process.Start(Environment.CurrentDirectory.ToString() + @"\\UpdateChecker.exe");

                    //Close this App
                    Application.ExitThread();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("មានបញ្ហា!\n"+ex.Message,"Rachhan System",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }

        private void TreeViewMenu_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode currentClkNode = e.Node;
            if (currentClkNode.ImageIndex == 2)
            {
                currentClkNode.SelectedImageIndex = 3;
            }
            else
            {
                currentClkNode.SelectedImageIndex = currentClkNode.ImageIndex;
            }
        }

        private void MenuFormV2_MdiChildActivate(object sender, EventArgs e)
        {
            if (Application.OpenForms.Count - 2 < dtOpenForm.Rows.Count)
            {
                string indexRemove = "";
                for (int j = 1; j < Application.OpenForms.Count; j++)
                {
                    for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                    {
                        if (dtOpenForm.Rows[i][0].ToString() == Application.OpenForms[j].Name.ToString())
                        {

                        }
                        else
                        {
                            indexRemove = i.ToString();
                        }
                        
                    }
                }
                if (indexRemove.Trim() == "")
                {

                }
                else
                {
                    dtOpenForm.Rows.RemoveAt(Convert.ToInt32(indexRemove));
                    tabControlOpenForm.TabPages.RemoveAt(tabControlOpenForm.SelectedIndex);

                }
            }
            if (dtOpenForm.Rows.Count == 0)
            {
                panelOpenTab.Hide();
            }
            else
            {
                string TabName = "";
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (ActiveForm.ActiveMdiChild.Name.ToString() == dtOpenForm.Rows[i][0].ToString())
                    {
                        TabName = dtOpenForm.Rows[i][1].ToString();
                        break;
                    }
                }

                for (int i = 0; i < tabControlOpenForm.TabCount; i++)
                {
                    if (tabControlOpenForm.TabPages[i].Text.ToString() == TabName)
                    {
                        tabControlOpenForm.SelectedIndex=i;
                        break;
                    }
                }
            }
        }

        private void TabControlOpenForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlOpenForm.TabPages.Count>0)
            {
                string OpenAgain = "";
                //Check in dtChildRoot
                for (int i = 0; i < dtChildRoot.Rows.Count; i++)
                {
                    if (tabControlOpenForm.SelectedTab.Text.ToString() == dtChildRoot.Rows[i][1].ToString())
                    {
                        OpenAgain = dtChildRoot.Rows[i][2].ToString();
                    }
                }

                //Check in dtChildRootofChild
                for (int i = 0; i < dtChildRootofChild.Rows.Count; i++)
                {
                    if (tabControlOpenForm.SelectedTab.Text.ToString() == dtChildRootofChild.Rows[i][2].ToString())
                    {
                        OpenAgain = dtChildRootofChild.Rows[i][3].ToString();
                    }
                }

                foreach (Form f in Application.OpenForms)
                {
                    if (OpenAgain == f.Name.ToString())
                    {
                        f.Focus();
                        break;
                    }

                }
            }            
        }

        private void MenuFormV2_FormClosing(object sender, FormClosingEventArgs e)
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

        private void TreeViewMenu_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode currentClkNode = e.Node;

            int Before = dtOpenForm.Rows.Count;

            if (currentClkNode.Text == "បញ្ចូលទិន្នន័យតាមរយៈបាកូដ OBS")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                //If not yet open
                if (FoundOpened == 0)
                {
                    InputTransferSemiForm Itsf = new InputTransferSemiForm();
                    Itsf.MdiParent = MenuFormV2.ActiveForm;
                    Itsf.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលទិន្នន័យតាមរយៈបាកូដ OBS");
                    dtOpenForm.Rows.Add("InputTransferSemiForm", "បញ្ចូលទិន្នន័យតាមរយៈបាកូដ OBS");
                }            
                
            }
            if (currentClkNode.Text == "កែប្រែទិន្នន័យវេរចេញ")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    TransferDataEditForm Tdef = new TransferDataEditForm();
                    Tdef.MdiParent = MenuFormV2.ActiveForm;
                    Tdef.Show();
                    tabControlOpenForm.TabPages.Add("កែប្រែទិន្នន័យវេរចេញ");
                    dtOpenForm.Rows.Add("TransferDataEditForm", "កែប្រែទិន្នន័យវេរចេញ");
                }
                
            }
            if (currentClkNode.Text == "ទិន្នន័យវេរសឺមី")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    TransferDataForm Tdf = new TransferDataForm();
                    Tdf.MdiParent = MenuFormV2.ActiveForm;
                    Tdf.Show();
                    tabControlOpenForm.TabPages.Add("ទិន្នន័យវេរសឺមី");
                    dtOpenForm.Rows.Add("TransferDataForm", "ទិន្នន័យវេរសឺមី");
                }

                
            }
            if (currentClkNode.Text == "ឆែកទិន្នន័យ POS")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    POSTrackingSearchForm Ptsf = new POSTrackingSearchForm();
                    Ptsf.MdiParent = MenuFormV2.ActiveForm;
                    Ptsf.Show();
                    tabControlOpenForm.TabPages.Add("ឆែកទិន្នន័យ POS");
                    dtOpenForm.Rows.Add("POSTrackingSearchForm", "ឆែកទិន្នន័យ POS");
                }
                
            }
            if (currentClkNode.Text == "ទាញទិន្នន័យ POS ថ្មីពី OBS")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    RetriveDataFromOBSForm Rdfof = new RetriveDataFromOBSForm();
                    Rdfof.MdiParent = MenuFormV2.ActiveForm;
                    Rdfof.Show();
                    tabControlOpenForm.TabPages.Add("ទាញទិន្នន័យ POS ថ្មីពី OBS");
                    dtOpenForm.Rows.Add("RetriveDataFromOBSForm", "ទាញទិន្នន័យ POS ថ្មីពី OBS");
                }                
            }
            if (currentClkNode.Text == "បញ្ចូលដំណាក់កាល Auto (បាកូដ)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    SemiPressInputForm Spif = new SemiPressInputForm();
                    Spif.MdiParent = MenuFormV2.ActiveForm;
                    Spif.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលដំណាក់កាល Auto (បាកូដ)");
                    dtOpenForm.Rows.Add("SemiPressInputForm", "បញ្ចូលដំណាក់កាល Auto (បាកូដ)");
                }
            }
            if (currentClkNode.Text == "បញ្ចូលដំណាក់កាល Auto (ដោយដៃ)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    SemiPressInputByManualForm Spimf = new SemiPressInputByManualForm();
                    Spimf.MdiParent = MenuFormV2.ActiveForm;
                    Spimf.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលដំណាក់កាល Auto (ដោយដៃ)");
                    dtOpenForm.Rows.Add("SemiPressInputByManualForm", "បញ្ចូលដំណាក់កាល Auto (ដោយដៃ)");
                }
            }
            if (currentClkNode.Text == "ស្វែងរកទិន្នន័យ Auto")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    SemiPress1SearchForm Spsf = new SemiPress1SearchForm();
                    Spsf.MdiParent = MenuFormV2.ActiveForm;
                    Spsf.Show();
                    tabControlOpenForm.TabPages.Add("ស្វែងរកទិន្នន័យ Auto");
                    dtOpenForm.Rows.Add("SemiPress1SearchForm", "ស្វែងរកទិន្នន័យ Auto");
                }
            }
            if (currentClkNode.Text == "បញ្ចូលដំណាក់កាល Semi Press (បាកូដ)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }
                
                if (FoundOpened == 0)
                {
                    SemiPress2InputForm Sp2if = new SemiPress2InputForm();
                    Sp2if.MdiParent = MenuFormV2.ActiveForm;
                    Sp2if.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលដំណាក់កាល Semi Press (បាកូដ)");
                    dtOpenForm.Rows.Add("SemiPress2InputForm", "បញ្ចូលដំណាក់កាល Semi Press (បាកូដ)");
                }
            }
            if (currentClkNode.Text == "ស្វែងរកទិន្នន័យ Semi Press")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    SemiPress2SearchForm Sp2sf = new SemiPress2SearchForm();
                    Sp2sf.MdiParent = MenuFormV2.ActiveForm;
                    Sp2sf.Show();
                    tabControlOpenForm.TabPages.Add("ស្វែងរកទិន្នន័យ Semi Press");
                    dtOpenForm.Rows.Add("SemiPress2SearchForm", "ស្វែងរកទិន្នន័យ Semi Press");
                }
                
            }
            if (currentClkNode.Text == "ឆែកទិន្នន័យ")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    SemiPressSearchForm Spsf = new SemiPressSearchForm();
                    Spsf.MdiParent = MenuFormV2.ActiveForm;
                    Spsf.Show();
                    tabControlOpenForm.TabPages.Add("ឆែកទិន្នន័យ");
                    dtOpenForm.Rows.Add("SemiPressSearchForm", "ឆែកទិន្នន័យ");
                }
                
            }
            if (currentClkNode.Text == "បញ្ចូល NG")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    NGInputForm Nif = new NGInputForm();
                    Nif.MdiParent = MenuFormV2.ActiveForm;
                    Nif.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូល NG");
                    dtOpenForm.Rows.Add("NGInputForm", "បញ្ចូល NG");
                }
                
            }
            if (currentClkNode.Text == "NG Records")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    NGHistoryForm Nhf = new NGHistoryForm();
                    Nhf.MdiParent = MenuFormV2.ActiveForm;
                    Nhf.Show();
                    tabControlOpenForm.TabPages.Add("NG Records");
                    dtOpenForm.Rows.Add("NGHistoryForm", "NG Records");
                }
            }
            if (currentClkNode.Text == "Semi MstBOM")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    MstBOMForm Uf = new MstBOMForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("Semi MstBOM");
                    dtOpenForm.Rows.Add("MstBOMForm", "Semi MstBOM");
                }
            }
            if (currentClkNode.Text == "MstUncountable Material")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    MstUncountMatForm Mumf = new MstUncountMatForm();
                    Mumf.MdiParent = MenuFormV2.ActiveForm;
                    Mumf.Show();
                    tabControlOpenForm.TabPages.Add("MstUncountable Material");
                    dtOpenForm.Rows.Add("MstUncountMatForm", "MstUncountable Material");
                }
            }
            if (currentClkNode.Text == "ទិន្នន័យចេញចូលស្តុក OBS")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    OBSTransactionForm Otf = new OBSTransactionForm();
                    Otf.MdiParent = MenuFormV2.ActiveForm;
                    Otf.Show();
                    tabControlOpenForm.TabPages.Add("ទិន្នន័យចេញចូលស្តុក OBS");
                    dtOpenForm.Rows.Add("OBSTransactionForm", "ទិន្នន័យចេញចូលស្តុក OBS");
                }
                
            }
            if (currentClkNode.Text == "ទិន្នន័យស្តុក OBS")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    OBSStockForm Osf = new OBSStockForm();
                    Osf.MdiParent = MenuFormV2.ActiveForm;
                    Osf.Show();
                    tabControlOpenForm.TabPages.Add("ទិន្នន័យស្តុក OBS");
                    dtOpenForm.Rows.Add("OBSStockForm", "ទិន្នន័យស្តុក OBS");
                }

            }
            if (currentClkNode.Text == "User Account")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    UserForm Uf = new UserForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("User Account");
                    dtOpenForm.Rows.Add("UserForm", "User Account");

                }

            }
            if (currentClkNode.Text == "បញ្ចូល Semi NG REQ")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    SemiNGReqInputForm Uf = new SemiNGReqInputForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូល Semi NG REQ");
                    dtOpenForm.Rows.Add("SemiNGReqInputForm", "បញ្ចូល Semi NG REQ");
                }

                
            }
            if (currentClkNode.Text == "ស្វែងរកទិន្នន័យ Semi NG REQ")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    SemiNGReqSearchForm Uf = new SemiNGReqSearchForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ស្វែងរកទិន្នន័យ Semi NG REQ");
                    dtOpenForm.Rows.Add("SemiNGReqSearchForm", "ស្វែងរកទិន្នន័យ Semi NG REQ");
                }

            }
            if (currentClkNode.Text == "គណនា/បំពេញគម្រោងម៉ាស៊ីន")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    GenerateMCPlansForm Uf = new GenerateMCPlansForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("គណនា/បំពេញគម្រោងម៉ាស៊ីន");
                    dtOpenForm.Rows.Add("GenerateMCPlansForm", "គណនា/បំពេញគម្រោងម៉ាស៊ីន");
                }
                
            }
            if (currentClkNode.Text == "Import POS")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    RMStatusForPlanForm Uf = new RMStatusForPlanForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("Import POS");
                    dtOpenForm.Rows.Add("RMStatusForPlanForm", "Import POS");
                }                
            }
            if (currentClkNode.Text == "MC Mtr Plan")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    MCPlanStatusSearchForm Uf = new MCPlanStatusSearchForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("MC Mtr Plan");
                    dtOpenForm.Rows.Add("MCPlanStatusSearchForm", "MC Mtr Plan");
                }
                
            }
            if (currentClkNode.Text == "MC List")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    MachineTypeMasterForm Uf = new MachineTypeMasterForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("MC List");
                    dtOpenForm.Rows.Add("MachineTypeMasterForm", "MC List");
                }

            }
            if (currentClkNode.Text == "MC Type vs Item")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    MasterItemForm Uf = new MasterItemForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("MC Type vs Item");
                    dtOpenForm.Rows.Add("MasterItemForm", "MC Type vs Item");
                }
            }
            if (currentClkNode.Text == "Input MC Type to the Plan")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    _1InputMCStatusForm Uf = new _1InputMCStatusForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("Input MC Type to the Plan");
                    dtOpenForm.Rows.Add("_1InputMCStatusForm", "Input MC Type to the Plan");
                }
            }
            if (currentClkNode.Text == "បញ្ចូលស្ថានភាព MQC")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    _2InputMQCStatusForm Uf = new _2InputMQCStatusForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលស្ថានភាព MQC");
                    dtOpenForm.Rows.Add("_2InputMQCStatusForm", "បញ្ចូលស្ថានភាព MQC");
                }
            }
            if (currentClkNode.Text == "បញ្ចូលទទួលពី Kitting Room")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    SDReceiveForm Uf = new SDReceiveForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលទទួលពី Kitting Room");
                    dtOpenForm.Rows.Add("SDReceiveForm", "បញ្ចូលទទួលពី Kitting Room");
                }
            }
            if (currentClkNode.Text == "ទិន្នន័យស្តុក MC")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    MCStockSearchForm Uf = new MCStockSearchForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ទិន្នន័យស្តុក MC");
                    dtOpenForm.Rows.Add("MCStockSearchForm", "ទិន្នន័យស្តុក MC");
                }
            }
            if (currentClkNode.Text == "ផ្លាស់ប្ដូរ/ខ្ចី")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    SDBorrowForm Uf = new SDBorrowForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ផ្លាស់ប្ដូរ/ខ្ចី");
                    dtOpenForm.Rows.Add("SDBorrowForm", "ផ្លាស់ប្ដូរ/ខ្ចី");
                }
            }
            if (currentClkNode.Text == "បញ្ចូលសង")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    SDPayBackForm Uf = new SDPayBackForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលសង");
                    dtOpenForm.Rows.Add("SDPayBackForm", "បញ្ចូលសង");
                }
            }
            if (currentClkNode.Text == "បញ្ចូលវេរចេញទៅផលិត")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    SDTransferForm Uf = new SDTransferForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលវេរចេញទៅផលិត");
                    dtOpenForm.Rows.Add("SDTransferForm", "បញ្ចូលវេរចេញទៅផលិត");
                }
            }
            if (currentClkNode.Text == "ទិន្នន័យវេរចេញ/ចូល")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    MCStockTransactionSearchForm Uf = new MCStockTransactionSearchForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ទិន្នន័យវេរចេញ/ចូល");
                    dtOpenForm.Rows.Add("MCStockTransactionSearchForm", "ទិន្នន័យវេរចេញ/ចូល");
                }
            }
            if (currentClkNode.Text == "ឆែកទិន្នន័យ POS (MC KIT)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    MCPOSDetailsSearchForm Uf = new MCPOSDetailsSearchForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ឆែកទិន្នន័យ POS (MC KIT)");
                    dtOpenForm.Rows.Add("MCPOSDetailsSearchForm", "ឆែកទិន្នន័យ POS (MC KIT)");
                }
            }
            if (currentClkNode.Text == "POS Remark")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    _3EditPOSMCDetailForm Uf = new _3EditPOSMCDetailForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("POS Remark");
                    dtOpenForm.Rows.Add("_3EditPOSMCDetailForm", "POS Remark");
                }
            }
            if (currentClkNode.Text == "SLOT List")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    SLOTMasterForm Uf = new SLOTMasterForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("SLOT List");
                    dtOpenForm.Rows.Add("SLOTMasterForm", "SLOT List");
                }
            }
            if (currentClkNode.Text == "Master Check (MC vs Item)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    MasterItemCheckForm Uf = new MasterItemCheckForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("Master Check (MC vs Item)");
                    dtOpenForm.Rows.Add("MasterItemCheckForm", "Master Check (MC vs Item)");
                }
            }
            if (currentClkNode.Text == "ឆែកទិន្នន័យ POS (1 SET)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    MCPOSDetailsBySetForm Uf = new MCPOSDetailsBySetForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ឆែកទិន្នន័យ POS (1 SET)");
                    dtOpenForm.Rows.Add("MCPOSDetailsBySetForm", "ឆែកទិន្នន័យ POS (1 SET)");
                }
            }
            if (currentClkNode.Text == "POS Data Check")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    POSDataCheckAmountForm Uf = new POSDataCheckAmountForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("POS Data Check");
                    dtOpenForm.Rows.Add("POSDataCheckAmountForm", "POS Data Check");
                }

            }
            if (currentClkNode.Text == "បញ្ចូលទិន្នន័យវេរ")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    AllSectionMCTransferForm Uf = new AllSectionMCTransferForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលទិន្នន័យវេរ");
                    dtOpenForm.Rows.Add("AllSectionMCTransferForm", "បញ្ចូលទិន្នន័យវេរ");
                }
            }
            if (currentClkNode.Text == "បញ្ចូលទិន្នន័យទទួល")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    AllSectionMCReceiveForm Uf = new AllSectionMCReceiveForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលទិន្នន័យទទួល");
                    dtOpenForm.Rows.Add("AllSectionMCReceiveForm", "បញ្ចូលទិន្នន័យទទួល");
                }
            }
            if (currentClkNode.Text == "ទិន្នន័យវេរ/ទទួល")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    AllSectionMCTransactionForm Uf = new AllSectionMCTransactionForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ទិន្នន័យវេរ/ទទួល");
                    dtOpenForm.Rows.Add("AllSectionMCTransactionForm", "ទិន្នន័យវេរ/ទទួល");
                }

            }
            if (currentClkNode.Text == "ស្ថានភាពទូទៅ")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    AllSectionMCOverviewForm Uf = new AllSectionMCOverviewForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ស្ថានភាពទូទៅ");
                    dtOpenForm.Rows.Add("AllSectionMCOverviewForm", "ស្ថានភាពទូទៅ");
                }
            }
            if (currentClkNode.Text == "បញ្ចូលស្តុក SD MC")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    WireStockINForm Uf = new WireStockINForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលស្តុក SD MC");
                    dtOpenForm.Rows.Add("WireStockINForm", "បញ្ចូលស្តុក SD MC");
                }
            }
            if (currentClkNode.Text == "វេរចេញ")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    WireTransferForm Uf = new WireTransferForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("វេរចេញ");
                    dtOpenForm.Rows.Add("WireTransferForm", "វេរចេញ");
                }
            }
            if (currentClkNode.Text == "បញ្ចូលស្តុក/ដកចេញ")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    WireReceiveAndIssueForm Uf = new WireReceiveAndIssueForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលស្តុក/ដកចេញ");
                    dtOpenForm.Rows.Add("WireReceiveAndIssueForm", "បញ្ចូលស្តុក/ដកចេញ");
                }
            }
            if (currentClkNode.Text == "គណនា NG Ratio បន្ថែម")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    NGRatioCalcForm Uf = new NGRatioCalcForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("គណនា NG Ratio បន្ថែម");
                    dtOpenForm.Rows.Add("NGRatioCalcForm", "គណនា NG Ratio បន្ថែម");
                }
            }
            if (currentClkNode.Text == "Master RM Uncountable")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    UncountableRMMasterForm Uf = new UncountableRMMasterForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("Master RM Uncountable");
                    dtOpenForm.Rows.Add("UncountableRMMasterForm", "Master RM Uncountable");
                }
            }
            if (currentClkNode.Text == "ព្រីនឡាប៊ែលវត្ថុធាតុដើមនៅសល់")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    WireRemainingCalcAndPrintTag Uf = new WireRemainingCalcAndPrintTag();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ព្រីនឡាប៊ែលវត្ថុធាតុដើមនៅសល់");
                    dtOpenForm.Rows.Add("WireRemainingCalcAndPrintTag", "ព្រីនឡាប៊ែលវត្ថុធាតុដើមនៅសល់");
                }
            }
            if (currentClkNode.Text == "បញ្ចូលរាប់ស្តុក (KIT)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    KITCountingForm Uf = new KITCountingForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលរាប់ស្តុក (KIT)");
                    dtOpenForm.Rows.Add("KITCountingForm", "បញ្ចូលរាប់ស្តុក (KIT)");
                }
            }
            if (currentClkNode.Text == "ព្រីនឡាប៊ែល NG")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    NGRatePrintLabelForm Uf = new NGRatePrintLabelForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ព្រីនឡាប៊ែល NG");
                    dtOpenForm.Rows.Add("NGRatePrintLabelForm", "ព្រីនឡាប៊ែល NG");
                }
            }
            if (currentClkNode.Text == "អាប់ដេតរាប់ស្តុក (KIT)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    KITUpdateForm Uf = new KITUpdateForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("អាប់ដេតរាប់ស្តុក (KIT)");
                    dtOpenForm.Rows.Add("KITUpdateForm", "អាប់ដេតរាប់ស្តុក (KIT)");
                }
            }
            if (currentClkNode.Text == "លុបរាប់ស្តុក (KIT)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    KITDeleteForm Uf = new KITDeleteForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("លុបរាប់ស្តុក (KIT)");
                    dtOpenForm.Rows.Add("KITDeleteForm", "លុបរាប់ស្តុក (KIT)");
                }
            }
            if (currentClkNode.Text == "បញ្ចូលរាប់ស្តុក (MC SD)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    MCSDCountingForm Uf = new MCSDCountingForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលរាប់ស្តុក (MC SD)");
                    dtOpenForm.Rows.Add("MCSDCountingForm", "បញ្ចូលរាប់ស្តុក (MC SD)");
                }
            }
            if (currentClkNode.Text == "អាប់ដេតរាប់ស្តុក (MC SD)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    MCSDUpdateForm Uf = new MCSDUpdateForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("អាប់ដេតរាប់ស្តុក (MC SD)");
                    dtOpenForm.Rows.Add("MCSDUpdateForm", "អាប់ដេតរាប់ស្តុក (MC SD)");
                }
            }
            if (currentClkNode.Text == "លុបរាប់ស្តុក (MC SD)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    MCSDDeleteForm Uf = new MCSDDeleteForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("លុបរាប់ស្តុក (MC SD)");
                    dtOpenForm.Rows.Add("MCSDDeleteForm", "លុបរាប់ស្តុក (MC SD)");
                }
            }
            if (currentClkNode.Text == "ទិន្នន័យរាប់ស្តុក (KIT)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    KITInventoryDataForm Uf = new KITInventoryDataForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ទិន្នន័យរាប់ស្តុក (KIT)");
                    dtOpenForm.Rows.Add("KITInventoryDataForm", "ទិន្នន័យរាប់ស្តុក (KIT)");
                }
            }
            if (currentClkNode.Text == "ទិន្នន័យរាប់ស្តុក (MC SD)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    MCSDInventoryDataForm Uf = new MCSDInventoryDataForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ទិន្នន័យរាប់ស្តុក (MC SD)");
                    dtOpenForm.Rows.Add("MCSDInventoryDataForm", "ទិន្នន័យរាប់ស្តុក (MC SD)");
                }
            }
            if (currentClkNode.Text == "បញ្ចូលរាប់ស្តុក (MC Inprocess)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    InprocessCountingForm Uf = new InprocessCountingForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលរាប់ស្តុក (MC Inprocess)");
                    dtOpenForm.Rows.Add("InprocessCountingForm", "បញ្ចូលរាប់ស្តុក (MC Inprocess)");
                }
            }
            if (currentClkNode.Text == "អាប់ដេតរាប់ស្តុក (MC Inprocess)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    InprocessUpdateForm Uf = new InprocessUpdateForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("អាប់ដេតរាប់ស្តុក (MC Inprocess)");
                    dtOpenForm.Rows.Add("InprocessUpdateForm", "អាប់ដេតរាប់ស្តុក (MC Inprocess)");
                }
            }
            if (currentClkNode.Text == "លុបរាប់ស្តុក (MC Inprocess)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    InprocessDeleteForm Uf = new InprocessDeleteForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("លុបរាប់ស្តុក (MC Inprocess)");
                    dtOpenForm.Rows.Add("InprocessDeleteForm", "លុបរាប់ស្តុក (MC Inprocess)");
                }
            }
            if (currentClkNode.Text == "ទិន្នន័យរាប់ស្តុក (MC Inprocess)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    InprocessInventoryDataForm Uf = new InprocessInventoryDataForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ទិន្នន័យរាប់ស្តុក (MC Inprocess)");
                    dtOpenForm.Rows.Add("InprocessInventoryDataForm", "ទិន្នន័យរាប់ស្តុក (MC Inprocess)");
                }
            }
            if (currentClkNode.Text == "Combine Inventory")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    InventoryCombineForm Uf = new InventoryCombineForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("Combine Inventory");
                    dtOpenForm.Rows.Add("InventoryCombineForm", "Combine Inventory");
                }
            }
            if (currentClkNode.Text == "ព្រីនឡាប៊ែល NG (SD MC)")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    NGRatePrintLabelSDWireForm Uf = new NGRatePrintLabelSDWireForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ព្រីនឡាប៊ែល NG (SD MC)");
                    dtOpenForm.Rows.Add("NGRatePrintLabelSDWireForm", "ព្រីនឡាប៊ែល NG (SD MC)");
                }
            }
            if (currentClkNode.Text == "NG Search")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    NGInprocessSearchForm Uf = new NGInprocessSearchForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("NG Search");
                    dtOpenForm.Rows.Add("NGInprocessSearchForm", "NG Search");
                }
            }
            if (currentClkNode.Text == "បញ្ចូលស្តុក KIT")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    KITStockINForm Uf = new KITStockINForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("បញ្ចូលស្តុក KIT");
                    dtOpenForm.Rows.Add("KITStockINForm", "បញ្ចូលស្តុក KIT");
                }
            }
            if (currentClkNode.Text == "គណនា/វេរស្តុកសម្រាប់គម្រោង")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    WireCalcForProduction Uf = new WireCalcForProduction();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("គណនា/វេរស្តុកសម្រាប់គម្រោង");
                    dtOpenForm.Rows.Add("WireCalcForProduction", "គណនា/វេរស្តុកសម្រាប់គម្រោង");
                }
            }
            if (currentClkNode.Text == "ស្វែងរកទិន្នន័យវេរស្តុកសម្រាប់គម្រោង")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    KITandSDForProductionSearch Uf = new KITandSDForProductionSearch();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ស្វែងរកទិន្នន័យវេរស្តុកសម្រាប់គម្រោង");
                    dtOpenForm.Rows.Add("KITandSDForProductionSearch", "ស្វែងរកទិន្នន័យវេរស្តុកសម្រាប់គម្រោង");
                }
            }
            if (currentClkNode.Text == "ស្តុកកាត")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    StockCardForm Uf = new StockCardForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ស្តុកកាត");
                    dtOpenForm.Rows.Add("StockCardForm", "ស្តុកកាត");
                }
            }
            if (currentClkNode.Text == "ទទួលស្តុកពី MC Inprocess")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    WireStockReturnForm Uf = new WireStockReturnForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("ទទួលស្តុកពី MC Inprocess");
                    dtOpenForm.Rows.Add("WireStockReturnForm", "ទទួលស្តុកពី MC Inprocess");
                }
            }
            if (currentClkNode.Text == "NG Adjust Search")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    NGAdjustSearchForm Uf = new NGAdjustSearchForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("NG Adjust Search");
                    dtOpenForm.Rows.Add("NGAdjustSearchForm", "NG Adjust Search");
                }
            }
            if (currentClkNode.Text == "MC Inprocess - Total View")
            {
                //Check if already open >> Focus on that Form
                int FoundOpened = 0;
                for (int i = 0; i < dtOpenForm.Rows.Count; i++)
                {
                    if (treeViewMenu.SelectedNode.Text.ToString() == dtOpenForm.Rows[i][1].ToString())
                    {
                        tabControlOpenForm.SelectedIndex = i;
                        FoundOpened++;
                        break;
                    }
                }

                if (FoundOpened == 0)
                {
                    MCInprocessStockSearchForm Uf = new MCInprocessStockSearchForm();
                    Uf.MdiParent = MenuFormV2.ActiveForm;
                    Uf.Show();
                    tabControlOpenForm.TabPages.Add("MC Inprocess - Total View");
                    dtOpenForm.Rows.Add("MCInprocessStockSearchForm", "MC Inprocess - Total View");
                }
            }

            int After = dtOpenForm.Rows.Count;

            if (Before < After)
            {
                tabControlOpenForm.SelectedIndex = dtOpenForm.Rows.Count - 1;
            }
            if (dtOpenForm.Rows.Count > 0)
            {
                panelOpenTab.Visible = true;
                panelOpenTab.Show();
            }

        }

        private void MenuFormV2_Load(object sender, EventArgs e)
        {
            this.Text = Assembly.GetExecutingAssembly().GetName().Name + " by Rachhan";
            treeViewMenu.Nodes.Clear();
            //Add root Node
            dtRoot = new DataTable();
            dtRoot.Columns.Add("RootName");
            dtRoot.Rows.Add("បញ្ចូលទិន្នន័យវេរសឺមីទៅ WIP room");
            dtRoot.Rows.Add("ទិន្នន័យវេរសឺមីទៅ WIP room");
            dtRoot.Rows.Add("គ្រប់គ្រងទិន្នន័យ Semi Press");
            dtRoot.Rows.Add("NG");
            dtRoot.Rows.Add("គ្រប់គ្រងទិន្នន័យ Semi NG REQ");
            dtRoot.Rows.Add("គ្រប់គ្រងទិន្នន័យ SD MC");
            dtRoot.Rows.Add("គ្រប់គ្រងគម្រោងរបស់ម៉ាស៊ីន");
            dtRoot.Rows.Add("រាប់ស្តុក");
            //dtRoot.Rows.Add("គ្រប់គ្រង Semi Uncomplete");
            dtRoot.Rows.Add("Admin");
            foreach (DataRow row in dtRoot.Rows)
            {
                TreeNode rootNode = new TreeNode(row["RootName"].ToString());
                treeViewMenu.Nodes.Add(rootNode);

            }

            //Add child Node
            dtChildRoot = new DataTable();
            dtChildRoot.Columns.Add("RootID");
            dtChildRoot.Columns.Add("ChildName");
            dtChildRoot.Columns.Add("UserFormName");
            //1
            dtChildRoot.Rows.Add(0, "បញ្ចូលទិន្នន័យតាមរយៈបាកូដ OBS", "InputTransferSemiForm");

            //2
            dtChildRoot.Rows.Add(1, "កែប្រែទិន្នន័យវេរចេញ", "TransferDataEditForm");
            dtChildRoot.Rows.Add(1, "ទិន្នន័យវេរសឺមី", "TransferDataForm");
            dtChildRoot.Rows.Add(1, "ឆែកទិន្នន័យ POS", "POSTrackingSearchForm");

            //3
            dtChildRoot.Rows.Add(2, "បញ្ចូលដំណាក់កាល Auto", "");
            dtChildRoot.Rows.Add(2, "បញ្ចូលដំណាក់កាល Semi Press", "");
            dtChildRoot.Rows.Add(2, "ឆែកទិន្នន័យ", "SemiPressSearchForm");

            //4
            dtChildRoot.Rows.Add(3, "បញ្ចូល NG", "NGInputForm");
            dtChildRoot.Rows.Add(3, "NG Records", "NGHistoryForm");
            dtChildRoot.Rows.Add(3, "NG Search", "NGInprocessSearchForm");
            dtChildRoot.Rows.Add(3, "NG Adjust Search", "NGAdjustSearchForm");
            dtChildRoot.Rows.Add(3, "Semi MstBOM", "MstBOMForm");
            dtChildRoot.Rows.Add(3, "MstUncountable Material", "MstUncountMatForm");
            dtChildRoot.Rows.Add(3, "គណនា NG Ratio បន្ថែម", "NGRatioCalcForm");

            //5
            dtChildRoot.Rows.Add(4, "បញ្ចូល Semi NG REQ", "SemiNGReqInputForm");
            dtChildRoot.Rows.Add(4, "ស្វែងរកទិន្នន័យ Semi NG REQ", "SemiNGReqSearchForm");

            //6
            dtChildRoot.Rows.Add(5, "គ្រប់គ្រងវត្ថុធាតុដើម(ខនិកទ័រ)", "");
            dtChildRoot.Rows.Add(5, "គ្រប់គ្រងវត្ថុធាតុដើម(ខ្សែភ្លើង/ធើមីណល)", "");
            dtChildRoot.Rows.Add(5, "វេរចេញ", "WireTransferForm");
            dtChildRoot.Rows.Add(5, "បញ្ចូលស្តុក/ដកចេញ", "WireReceiveAndIssueForm");
            dtChildRoot.Rows.Add(5, "ស្វែងរកទិន្នន័យវេរស្តុកសម្រាប់គម្រោង", "KITandSDForProductionSearch");
            dtChildRoot.Rows.Add(5, "MC Inprocess - Total View", "MCInprocessStockSearchForm");
            dtChildRoot.Rows.Add(5, "ស្តុកកាត", "StockCardForm");
            dtChildRoot.Rows.Add(5, "ទិន្នន័យវេរចេញ/ចូល", "MCStockTransactionSearchForm");
            dtChildRoot.Rows.Add(5, "ទិន្នន័យស្តុក MC", "MCStockSearchForm");

            //7
            dtChildRoot.Rows.Add(6, "Import POS", "RMStatusForPlanForm");
            dtChildRoot.Rows.Add(6, "Input MC Type to the Plan", "_1InputMCStatusForm");
            dtChildRoot.Rows.Add(6, "MC Mtr Plan", "MCPlanStatusSearchForm");
            dtChildRoot.Rows.Add(6, "POS Remark", "_3EditPOSMCDetailForm");
            dtChildRoot.Rows.Add(6, "POS Data Check", "POSDataCheckAmountForm");

            //8
            dtChildRoot.Rows.Add(7, "MC Kitting Inventory", "");
            dtChildRoot.Rows.Add(7, "MC SD  Inventory", "");
            dtChildRoot.Rows.Add(7, "MC Inprocess Inventory", "");
            dtChildRoot.Rows.Add(7, "Combine Inventory", "InventoryCombineForm");


            //8
            //dtChildRoot.Rows.Add(7, "បញ្ចូលទិន្នន័យវេរ", "AllSectionMCTransferForm");
            //dtChildRoot.Rows.Add(7, "បញ្ចូលទិន្នន័យទទួល", "AllSectionMCReceiveForm");
            //dtChildRoot.Rows.Add(7, "ទិន្នន័យវេរ/ទទួល", "AllSectionMCTransactionForm");
            //dtChildRoot.Rows.Add(7, "ស្ថានភាពទូទៅ", "AllSectionMCOverviewForm");

            //The last one
            dtChildRoot.Rows.Add(8, "User Account", "UserForm");
            dtChildRoot.Rows.Add(8, "MC List", "MachineTypeMasterForm");
            dtChildRoot.Rows.Add(8, "MC Type vs Item", "MasterItemForm");
            dtChildRoot.Rows.Add(8, "SLOT List", "SLOTMasterForm");
            dtChildRoot.Rows.Add(8, "Master Check (MC vs Item)", "MasterItemCheckForm");
            dtChildRoot.Rows.Add(8, "Master RM Uncountable", "UncountableRMMasterForm");

            foreach (DataRow row1 in dtChildRoot.Rows) 
            {
                TreeNode ChildrootNode = new TreeNode(row1[1].ToString());
                treeViewMenu.Nodes[Convert.ToInt32(row1[0].ToString())].Nodes.Add(ChildrootNode);
                Font regularFont = new Font("Khmer OS Battambang", 11, FontStyle.Regular);
                ChildrootNode.NodeFont = regularFont;

            }


            //Add child of child Node
            dtChildRootofChild = new DataTable();
            dtChildRootofChild.Columns.Add("RootID");
            dtChildRootofChild.Columns.Add("ChildRootID");
            dtChildRootofChild.Columns.Add("ChildName");
            dtChildRootofChild.Columns.Add("UserFormName");
            //4,1
            dtChildRootofChild.Rows.Add(2, 0, "បញ្ចូលដំណាក់កាល Auto (បាកូដ)", "SemiPressInputForm");
            dtChildRootofChild.Rows.Add(2, 0, "បញ្ចូលដំណាក់កាល Auto (ដោយដៃ)", "SemiPressInputByManualForm");
            dtChildRootofChild.Rows.Add(2, 0, "ស្វែងរកទិន្នន័យ Auto", "SemiPress1SearchForm");
            //4,2
            dtChildRootofChild.Rows.Add(2, 1, "បញ្ចូលដំណាក់កាល Semi Press (បាកូដ)", "SemiPress2InputForm");
            dtChildRootofChild.Rows.Add(2, 1, "ស្វែងរកទិន្នន័យ Semi Press", "SemiPress2SearchForm");

            //6,1
            dtChildRootofChild.Rows.Add(5, 0, "បញ្ចូលទទួលពី Kitting Room", "SDReceiveForm"); 
            dtChildRootofChild.Rows.Add(5, 0, "បញ្ចូលស្តុក KIT", "KITStockINForm");
            dtChildRootofChild.Rows.Add(5, 0, "ផ្លាស់ប្ដូរ/ខ្ចី", "SDBorrowForm");
            dtChildRootofChild.Rows.Add(5, 0, "បញ្ចូលសង", "SDPayBackForm");
            dtChildRootofChild.Rows.Add(5, 0, "បញ្ចូលវេរចេញទៅផលិត", "SDTransferForm");
            dtChildRootofChild.Rows.Add(5, 0, "ឆែកទិន្នន័យ POS (MC KIT)", "MCPOSDetailsSearchForm");
            dtChildRootofChild.Rows.Add(5, 0, "ឆែកទិន្នន័យ POS (1 SET)", "MCPOSDetailsBySetForm");
            dtChildRootofChild.Rows.Add(5, 0, "ព្រីនឡាប៊ែល NG", "NGRatePrintLabelForm");


            //6,2
            dtChildRootofChild.Rows.Add(5, 1, "បញ្ចូលស្តុក SD MC", "WireStockINForm");
            dtChildRootofChild.Rows.Add(5, 1, "គណនា/វេរស្តុកសម្រាប់គម្រោង", "WireCalcForProduction");
            dtChildRootofChild.Rows.Add(5, 1, "ទទួលស្តុកពី MC Inprocess", "WireStockReturnForm");
            dtChildRootofChild.Rows.Add(5, 1, "ព្រីនឡាប៊ែលវត្ថុធាតុដើមនៅសល់", "WireRemainingCalcAndPrintTag");
            dtChildRootofChild.Rows.Add(5, 1, "ព្រីនឡាប៊ែល NG (SD MC)", "NGRatePrintLabelSDWireForm");

            //8,1
            dtChildRootofChild.Rows.Add(7, 0, "បញ្ចូលរាប់ស្តុក (KIT)", "KITCountingForm");
            dtChildRootofChild.Rows.Add(7, 0, "អាប់ដេតរាប់ស្តុក (KIT)", "KITUpdateForm");
            dtChildRootofChild.Rows.Add(7, 0, "លុបរាប់ស្តុក (KIT)", "KITDeleteForm");
            dtChildRootofChild.Rows.Add(7, 0, "ទិន្នន័យរាប់ស្តុក (KIT)", "KITInventoryDataForm");

            //8,2
            dtChildRootofChild.Rows.Add(7, 1, "បញ្ចូលរាប់ស្តុក (MC SD)", "MCSDCountingForm");
            dtChildRootofChild.Rows.Add(7, 1, "អាប់ដេតរាប់ស្តុក (MC SD)", "MCSDUpdateForm");
            dtChildRootofChild.Rows.Add(7, 1, "លុបរាប់ស្តុក (MC SD)", "MCSDDeleteForm");
            dtChildRootofChild.Rows.Add(7, 1, "ទិន្នន័យរាប់ស្តុក (MC SD)", "MCSDInventoryDataForm");

            //8,3
            dtChildRootofChild.Rows.Add(7, 2, "បញ្ចូលរាប់ស្តុក (MC Inprocess)", "InprocessCountingForm");
            dtChildRootofChild.Rows.Add(7, 2, "អាប់ដេតរាប់ស្តុក (MC Inprocess)", "InprocessUpdateForm");
            dtChildRootofChild.Rows.Add(7, 2, "លុបរាប់ស្តុក (MC Inprocess)", "InprocessDeleteForm");
            dtChildRootofChild.Rows.Add(7, 2, "ទិន្នន័យរាប់ស្តុក (MC Inprocess)", "InprocessInventoryDataForm");


            foreach (DataRow row2 in dtChildRootofChild.Rows)
            {
                TreeNode ChildrootNode = new TreeNode(row2[2].ToString());
                treeViewMenu.Nodes[Convert.ToInt32(row2[0].ToString())].Nodes[Convert.ToInt32(row2[1].ToString())].Nodes.Add(ChildrootNode);
                Font regularFont = new Font("Khmer OS Battambang", 10, FontStyle.Regular);
                ChildrootNode.NodeFont = regularFont;

            }

            dtOpenForm = new DataTable();
            dtOpenForm.Columns.Add("TabFormName");
            dtOpenForm.Columns.Add("TabName");

            //Check Date
            DateTime dt = DateTime.Now;
            string Year = dt.ToString("yyyy");
            if (Convert.ToInt64(Year) < 2020)
            {
                MessageBox.Show("សូមកំណត់ម៉ោងឱ្យបានត្រឹមត្រូវមុនប្រើប្រាស់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.treeViewMenu.Enabled = false;
            }
            else if (Convert.ToInt64(Year) > 2028)
            {
                MessageBox.Show("កម្មវិធីនេះត្រូវបានផុតកំណត់ហើយ!​ \nសូមទាក់ទងទៅកាន់​ រ៉ាឆាន់ !", "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.treeViewMenu.Enabled = false;
            }
            else
            {                
                LbLoginID.Text = LoginForm.NameForNextForm;         
                UserForNextForm = LoginForm.NameForNextForm;
                if (LoginForm.RoleForNextForm != "Admin")
                {
                    int AdminFuncCount = treeViewMenu.Nodes[treeViewMenu.Nodes.Count - 1].Nodes.Count;
                    for (int i = AdminFuncCount - 1; i > -1; i--)
                    {
                        if (i == 0)
                        {
                            treeViewMenu.Nodes[treeViewMenu.Nodes.Count - 1].Nodes[i].Remove();
                        }
                    }
                }

                //Add image Form to all child 
                int parentRoot = treeViewMenu.Nodes.Count;
                for (int i = 0; i < parentRoot; i++)
                {
                    int ChildRoot = treeViewMenu.Nodes[i].Nodes.Count;
                    for (int j = 0; j < ChildRoot; j++)
                    {
                        treeViewMenu.Nodes[i].Nodes[j].ImageIndex = 2;
                    }

                }

                //Special case for root index 3
                int SepecialparentRoot = treeViewMenu.Nodes[2].Nodes.Count - 1;
                for (int i = 0; i < SepecialparentRoot; i++)
                {
                    treeViewMenu.Nodes[2].Nodes[i].ImageIndex = 1;
                    int ChildRoot = treeViewMenu.Nodes[2].Nodes[i].Nodes.Count;
                    for (int j = 0; j < ChildRoot; j++)
                    {
                        treeViewMenu.Nodes[2].Nodes[i].Nodes[j].ImageIndex = 2;
                    }

                }

                //Special case for root index 6
                int SepecialparentRoot6 = treeViewMenu.Nodes[5].Nodes.Count - 1;
                for (int i = 0; i < SepecialparentRoot6; i++)
                {
                    if (i < 2)
                    {
                        treeViewMenu.Nodes[5].Nodes[i].ImageIndex = 1;
                    }
                    int ChildRoot = treeViewMenu.Nodes[5].Nodes[i].Nodes.Count;
                    for (int j = 0; j < ChildRoot; j++)
                    {
                        treeViewMenu.Nodes[5].Nodes[i].Nodes[j].ImageIndex = 2;

                    }
                }

                //Special case for root index 8
                int SepecialparentRoot7 = treeViewMenu.Nodes[treeViewMenu.Nodes.Count - 2].Nodes.Count - 1;
                for (int i = 0; i < SepecialparentRoot7; i++)
                {
                    if (i < 3)
                    {
                        treeViewMenu.Nodes[treeViewMenu.Nodes.Count - 2].Nodes[i].ImageIndex = 1;
                    }
                    int ChildRoot = treeViewMenu.Nodes[treeViewMenu.Nodes.Count - 2].Nodes[i].Nodes.Count;
                    for (int j = 0; j < ChildRoot; j++)
                    {
                        treeViewMenu.Nodes[treeViewMenu.Nodes.Count - 2].Nodes[i].Nodes[j].ImageIndex = 2;
                    }
                }
            }
        }

        private void btnUnhide_Click(object sender, EventArgs e)
        {
            btnHide.Visible = true;
            btnUnhide.Visible = false;
            treeViewMenu.Show();
            panelLeft.Size = new Size(233, 440);
            splitterMenuVSMdi.Show();
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            btnUnhide.Visible = true;
            btnHide.Visible = false;
            treeViewMenu.Hide();
            panelLeft.Size = new Size(38, 440);
            splitterMenuVSMdi.Hide();
        }

        private void btnHideHeader_Click(object sender, EventArgs e)
        {
            panelHeaderTable.Hide();
            btnHideHeader.Visible = false;
            btnUnhideHeader.Visible = true;
            panelHeader.Size= new Size(1184, 28);
        }

        private void btnCredit_Click(object sender, EventArgs e)
        {
            CreditForm cf = new CreditForm();
            cf.ShowDialog();

        }

        private void btnUnhideHeader_Click(object sender, EventArgs e)
        {
            panelHeaderTable.Show();
            btnHideHeader.Visible=true;
            btnUnhideHeader.Visible=false;
            panelHeader.Size = new Size(1184, 52);
        }

    }
}
