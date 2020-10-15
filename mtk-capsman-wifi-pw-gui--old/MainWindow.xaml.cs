/*
   Copyright 2020 Christoph B. Wurzinger

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using tik4net;
using tik4net.Objects;
using tik4net.Objects.CapsMan;

namespace tik4net.Objects.CapsMan
{
    [TikEntity("/caps-man/security")]
    public class CapsManSecurity
    {
        [TikProperty(".id", IsReadOnly = true, IsMandatory = true)]
        public string Id { get; private set; }

        [TikProperty("comment")]
        public string Comment { get; set; }

        [TikProperty("passphrase")]
        public string Passphrase { get; set; }
    }
}

namespace mtk_capsman_wifi_pw_gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtHost.Text = Properties.Settings.Default.Host;
            txtUser.Text = Properties.Settings.Default.User;
            var pw = Properties.Settings.Default.Password;
            if (pw.Length > 0)
            {
                pw = Unprotect(pw);
                chkKeep.IsChecked = true;
            }
            txtPassword.Password = pw;
        }

        private ITikConnection connection = null;
        private List<CapsManSecurity> capsManSecurities = new List<CapsManSecurity>();

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            capsManSecurities = new List<CapsManSecurity>();

            try
            {
                btnDisplayChangePassword.IsEnabled = false;
                connection = ConnectionFactory.OpenConnection(TikConnectionType.ApiSsl,
                    txtHost.Text, txtUser.Text, txtPassword.Password);
                if ((bool)chkKeep.IsChecked)
                {
                    Properties.Settings.Default.Host = txtHost.Text;
                    Properties.Settings.Default.User = txtUser.Text;
                    Properties.Settings.Default.Password = Protect(txtPassword.Password);
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
                lbSec.ItemsSource = capsManSecurities;
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
                return;
            }

            var myCapsManSecurities = connection.LoadAll<CapsManSecurity>();

            // only use security configurations with a comment
            foreach (var s in myCapsManSecurities)
            {
                if (s.Comment.Length > 0)
                {
                    capsManSecurities.Add(s);
                    btnDisplayChangePassword.IsEnabled = true;
                }
            }

            lbSec.ItemsSource = capsManSecurities;
        }


        private void btnDisplayChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (lbSec.SelectedItem == null)
                return;
            var dialog = new PasswordInputDialog(
                "Password for " + (lbSec.SelectedItem as CapsManSecurity).Comment,
                (lbSec.SelectedItem as CapsManSecurity).Passphrase);
            if (dialog.ShowDialog() == true)
            {
                if (dialog.Password.Length >= 8)
                {
                    (lbSec.SelectedItem as CapsManSecurity).Passphrase = dialog.Password;
                    connection.Save(lbSec.SelectedItem as CapsManSecurity);
                }
                else
                {
                    MessageBox.Show("Password too short (min. 8 characters)!", "Notice", MessageBoxButton.OK);
                }
            }
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
    }
}