using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.Inventory.Inprocess
{
    public partial class InprocessCountingTypeForm : Form
    {
        public InprocessCountingTypeForm()
        {
            InitializeComponent();
            this.PicConnector.MouseEnter += PicConnector_MouseEnter;
            this.PicConnector.MouseLeave += PicConnector_MouseLeave;
            this.PicConnector.Click += PicConnector_Click;
            this.LbConnector.MouseEnter += LbConnector_MouseEnter;
            this.LbConnector.MouseLeave += LbConnector_MouseLeave;
            this.LbConnector.Click += LbConnector_Click;
            this.PicWIP.MouseEnter += PicWIP_MouseEnter;
            this.PicWIP.MouseLeave += PicWIP_MouseLeave;
            this.PicWIP.Click += PicWIP_Click;
            this.LbWIP.MouseEnter += LbWIP_MouseEnter;
            this.LbWIP.MouseLeave += LbWIP_MouseLeave;
            this.LbWIP.Click += LbWIP_Click;
            this.PicWireTerminal.MouseEnter += PicWireTerminal_MouseEnter;
            this.PicWireTerminal.MouseLeave += PicWireTerminal_MouseLeave;
            this.PicWireTerminal.Click += PicWireTerminal_Click;
            this.LbWireTerminal.MouseEnter += LbWireTerminal_MouseEnter;
            this.LbWireTerminal.MouseLeave += LbWireTerminal_MouseLeave;
            this.LbWireTerminal.Click += LbWireTerminal_Click;

        }

        private void LbWireTerminal_MouseEnter(object sender, EventArgs e)
        {
            WireTerminalFocus();
        }
        private void LbWireTerminal_MouseLeave(object sender, EventArgs e)
        {
            WireTerminalLostFocus();
        }
        private void LbWireTerminal_Click(object sender, EventArgs e)
        {
            WireTerminalClick();
        }
        private void PicWireTerminal_MouseEnter(object sender, EventArgs e)
        {
            WireTerminalFocus();
        }
        private void PicWireTerminal_MouseLeave(object sender, EventArgs e)
        {
            WireTerminalLostFocus();
        }
        private void PicWireTerminal_Click(object sender, EventArgs e)
        {
            WireTerminalClick();
        }
        private void LbWIP_MouseEnter(object sender, EventArgs e)
        {
            WIPFocus();
        }
        private void LbWIP_MouseLeave(object sender, EventArgs e)
        {
            WIPLostFocus();
        }
        private void LbWIP_Click(object sender, EventArgs e)
        {
            WIPClick();
        }
        private void PicWIP_MouseEnter(object sender, EventArgs e)
        {
            WIPFocus();
        }
        private void PicWIP_MouseLeave(object sender, EventArgs e)
        {
            WIPLostFocus();
        }
        private void PicWIP_Click(object sender, EventArgs e)
        {
            WIPClick();
        }
        private void LbConnector_MouseLeave(object sender, EventArgs e)
        {
            ConnectorLostFocus();
        }
        private void LbConnector_MouseEnter(object sender, EventArgs e)
        {
            ConnectorFocus();
        }
        private void LbConnector_Click(object sender, EventArgs e)
        {
            ConnectorClick();
        }
        private void PicConnector_MouseEnter(object sender, EventArgs e)
        {
            ConnectorFocus();
        }
        private void PicConnector_MouseLeave(object sender, EventArgs e)
        {
            ConnectorLostFocus();
        }
        private void PicConnector_Click(object sender, EventArgs e)
        {
            ConnectorClick();
        }

        //Function
        private void ConnectorFocus()
        {
            LbConnector.ForeColor = Color.Gold;
        }
        private void ConnectorLostFocus()
        {
            LbConnector.ForeColor = Color.Black;
        }
        private void WIPFocus()
        {
            LbWIP.ForeColor = Color.Gold;
        }
        private void WIPLostFocus()
        {
            LbWIP.ForeColor = Color.Black;
        }
        private void WireTerminalFocus()
        {
            LbWireTerminal.ForeColor = Color.Gold;
        }
        private void WireTerminalLostFocus()
        {
            LbWireTerminal.ForeColor = Color.Black;
        }
        private void ConnectorClick()
        {
            InprocessCountingForm.CountType = "POS";
            InprocessCountingForm.CountTypeChanged = 1;
            this.Close();
        }
        private void WIPClick()
        {
            InprocessCountingForm.CountType = "Semi";
            InprocessCountingForm.CountTypeChanged = 1;
            this.Close();
        }
        private void WireTerminalClick()
        {
            InprocessCountingForm.CountType = "Stock Card";
            InprocessCountingForm.CountTypeChanged = 1;
            this.Close();
        }
    }
}
