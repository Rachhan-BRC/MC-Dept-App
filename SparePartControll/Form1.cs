using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineDeptApp.SparePartControll
{
    public partial class RequestForm : Form
    {
        public RequestForm()
        {
            InitializeComponent();
            this.btnInvoice.Click += BtnInvoice_Click;
        }

        private void BtnInvoice_Click(object sender, EventArgs e)
        {
            InvoiceForm Reg = new InvoiceForm();
            Reg.ShowDialog();
        }
    }
}
