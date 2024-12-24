using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.MsgClass
{
    internal class ErrorMsgClass
    {
        public string AlertText;
        public void ShowingMsg()
        {
            MessageBox.Show(AlertText, "Rachhan System", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
