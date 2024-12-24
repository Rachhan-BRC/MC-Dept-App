using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.OBS
{
    public partial class OBSTransactionWIPResultForm : Form
    {
        OBSTransactionForm fgrid;
        public OBSTransactionWIPResultForm( OBSTransactionForm fg)
        {
            InitializeComponent();
            this.fgrid = fg;
        }

        private void OBSTransactionWIPResultForm_Load(object sender, EventArgs e)
        {            
            for (int i = 0; i < fgrid.dtWIPResult.Rows.Count; i++)
            {                
                if (fgrid.dtWIPResult.Rows[i][0].ToString() == OBSTransactionForm.POSResultSelected)
                {
                    LbWIPCode.Text = fgrid.dtWIPResult.Rows[i][7].ToString();
                    LbWIPName.Text = fgrid.dtWIPResult.Rows[i][8].ToString();
                    LbPIN.Text = fgrid.dtWIPResult.Rows[i][9].ToString();
                    LbWire.Text = fgrid.dtWIPResult.Rows[i][10].ToString();
                    LbLength.Text = fgrid.dtWIPResult.Rows[i][11].ToString();

                    string POSN = fgrid.dtWIPResult.Rows[i][0].ToString();
                    int BoxN = Convert.ToInt32(fgrid.dtWIPResult.Rows[i][3].ToString());
                    double Qty = Convert.ToDouble(fgrid.dtWIPResult.Rows[i][2].ToString());
                    DateTime RegDate = Convert.ToDateTime(fgrid.dtWIPResult.Rows[i][5].ToString());
                    string RegBy = fgrid.dtWIPResult.Rows[i][13].ToString();

                    dgvPOSResult.Rows.Add(POSN, BoxN, Qty, RegDate, RegBy);

                }
            }
        }
    }
}
