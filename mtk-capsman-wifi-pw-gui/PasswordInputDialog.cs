using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mtk_capsman_wifi_pw_gui
{
    public partial class PasswordInputDialog : Form
    {
        public PasswordInputDialog(string prompt, string defaultInput = "")
        {
            InitializeComponent();
            lblPrompt.Text = prompt;
            txtPassword.Text = defaultInput;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void PasswordInputDialog_Shown(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
            txtPassword.Focus();
        }

        public string Password
        {
            get { return txtPassword.Text; }
        }
    }
}
