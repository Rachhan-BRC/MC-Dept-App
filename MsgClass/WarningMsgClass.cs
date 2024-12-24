using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.MsgClass
{
    internal class WarningMsgClass
    {
        public string WarningText;
        public void ShowingMsg()
        {
            MessageBox.Show(WarningText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
