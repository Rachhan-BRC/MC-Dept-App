using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp
{
    public partial class CreditForm : Form
    {
        SQLConnect cnn = new SQLConnect();
        public CreditForm()
        {
            InitializeComponent();
            this.cnn.Connection();
            this.Shown += CreditForm_Shown;
            this.PicMessenger.Click += PicMessenger_Click;
            this.PicTelegram.Click += PicTelegram_Click;
        }

        private void PicTelegram_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://t.me/Boeun_Rachhan");
        }
        private void PicMessenger_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://m.me/dara.vechhan.1");
        }
        private void CreditForm_Shown(object sender, EventArgs e)
        {
            LbAppNameAndVersion.Text = Assembly.GetExecutingAssembly().GetName().Name;
            LbVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            LbServer.Text = cnn.server.ToString();
            LbDatabase.Text = cnn.db.ToString();
        }

    }
}
