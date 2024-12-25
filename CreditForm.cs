using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp
{
    public partial class CreditForm : Form
    {
        public CreditForm()
        {
            InitializeComponent();
            this.Shown += CreditForm_Shown;
        }

        private void CreditForm_Shown(object sender, EventArgs e)
        {
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://t.me/Boeun_Rachhan");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://m.me/dara.vechhan.1");
        }
    }
}
