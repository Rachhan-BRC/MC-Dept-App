using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.MsgClass
{
    internal class QuestionMsgClass
    {
        public string QAText;
        public bool UserClickedYes = false;
        public MessageBoxIcon MsgIcon = MessageBoxIcon.Question;
        public void ShowingMsg()
        {
            DialogResult DSL = MessageBox.Show(QAText, "Rachhan System", MessageBoxButtons.YesNo, MsgIcon);
            if (DSL == DialogResult.Yes)
            {
                UserClickedYes = true;
            }
        }
    }
}
