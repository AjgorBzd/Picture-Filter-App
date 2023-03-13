using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cg_project1._1
{
    public partial class FilterNameForm : System.Windows.Forms.Form
    {
        public FilterNameForm()
        {
            InitializeComponent();
        }

        public string FilterName
        {
            get { return NameTextBox.Text; }
        }

        private void OKNameButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CancelNameButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
