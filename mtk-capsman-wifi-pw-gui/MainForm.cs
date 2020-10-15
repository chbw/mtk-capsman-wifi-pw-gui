using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tik4net;
using tik4net.Objects;
using tik4net.Objects.CapsMan;



namespace mtk_capsman_wifi_pw_gui
{
    public partial class MainForm : Form
    {
        private ITikConnection connection = null;
        private List<CapsManSecurity> capsManSecurities = new List<CapsManSecurity>();

        public MainForm()
        {
            InitializeComponent();

            txtHost.Text = Properties.Settings.Default.Host;
            txtUser.Text = Properties.Settings.Default.User;
            var pw = Properties.Settings.Default.Password;
            if (pw.Length > 0)
            {
                pw = Unprotect(pw);
                chkKeep.Checked = true;
            }
            txtPassword.Text = pw;
        }

        private string Protect(string insecureString)
        {
            return Convert.ToBase64String(
                ProtectedData.Protect(
                    Encoding.UTF8.GetBytes(insecureString),
                    null,
                    DataProtectionScope.CurrentUser)
                );
        }

        private string Unprotect(string secureString)
        {
            return Encoding.UTF8.GetString(
                ProtectedData.Unprotect(
                    Convert.FromBase64String(secureString),
                    null,
                    DataProtectionScope.CurrentUser
                    )
                );
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            capsManSecurities = new List<CapsManSecurity>();

            try
            {
                btnDisplayChangePassword.Enabled = false;
                connection = ConnectionFactory.OpenConnection(TikConnectionType.ApiSsl,
                    txtHost.Text, txtUser.Text, txtPassword.Text);
                if ((bool)chkKeep.Checked)
                {
                    Properties.Settings.Default.Host = txtHost.Text;
                    Properties.Settings.Default.User = txtUser.Text;
                    Properties.Settings.Default.Password = Protect(txtPassword.Text);
                }
                else
                {
                    Properties.Settings.Default.Host = "";
                    Properties.Settings.Default.User = "";
                    Properties.Settings.Default.Password = "";
                }
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                connection = null;
                lbSec.DataSource = capsManSecurities;
                lbSec.DisplayMember = "Comment";
                lbSec.ValueMember = "Id";
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                return;
            }

            var myCapsManSecurities = connection.LoadAll<CapsManSecurity>();

            // only use security configurations with a comment
            foreach (var s in myCapsManSecurities)
            {
                if (s.Comment.Length > 0)
                {
                    capsManSecurities.Add(s);
                    btnDisplayChangePassword.Enabled = true;
                }
            }

            lbSec.DataSource = capsManSecurities;
            lbSec.DisplayMember = "Comment";
            lbSec.ValueMember = "Id";
        }

        private void btnDisplayChangePassword_Click(object sender, EventArgs e)
        {
            if (lbSec.SelectedItem == null)
                return;
            var dialog = new PasswordInputDialog(
                "Password for " + (lbSec.SelectedItem as CapsManSecurity).Comment,
                (lbSec.SelectedItem as CapsManSecurity).Passphrase);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                if (dialog.Password.Length >= 8)
                {
                    (lbSec.SelectedItem as CapsManSecurity).Passphrase = dialog.Password;
                    connection.Save(lbSec.SelectedItem as CapsManSecurity);
                }
                else
                {
                    MessageBox.Show("Password too short (min. 8 characters)!", "Notice", MessageBoxButtons.OK);
                }
            }
        }
    }
}
