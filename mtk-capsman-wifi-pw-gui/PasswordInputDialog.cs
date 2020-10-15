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
