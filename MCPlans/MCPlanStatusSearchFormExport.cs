using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.MCPlans
{
    public partial class MCPlanStatusSearchFormExport : Form
    {
        public MCPlanStatusSearchFormExport()
        {
            InitializeComponent();
            this.btnAll.Click += BtnAll_Click;
            this.btnKIT.Click += BtnKIT_Click;
        }

        private void BtnKIT_Click(object sender, EventArgs e)
        {
            MCPlanStatusSearchForm.TypeOfExport = "1";
            this.Close();
        }

        private void BtnAll_Click(object sender, EventArgs e)
        {
            MCPlanStatusSearchForm.TypeOfExport = "0";
            this.Close();
        }
    }
}
