using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.MsgClass
{
    internal class InformationMsgClass
    {
        public string InfoText;
        public void ShowingMsg()
        {
            MessageBox.Show(InfoText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
