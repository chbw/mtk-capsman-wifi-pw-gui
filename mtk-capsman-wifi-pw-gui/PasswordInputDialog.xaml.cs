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
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace mtk_capsman_wifi_pw_gui
{
    /// <summary>
    /// Interaction logic for PasswordInputDialog.xaml
    /// </summary>
    public partial class PasswordInputDialog : Window
    {
        public PasswordInputDialog(string prompt, string defaultInput="")
        {
            InitializeComponent();
            lblPrompt.Content = prompt;
            txtPassword.Text = defaultInput;
        }

        private void btnDialogOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
            txtPassword.Focus();
        }

        public string Password { 
            get { return txtPassword.Text; }
        }
    }
}
