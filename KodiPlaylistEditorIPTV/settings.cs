//  MIT License
//  Copyright (c) 2018 github/isayso
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
//  files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy,
//  modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
//  subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace PlaylistEditor
{

    public partial class settings : Form
    {
        public string serverName;
        public  bool isLinux ;
        public bool replaceDrive ;
        static int unicode = Properties.Settings.Default.hotkey;
        static char character = (char)unicode;
        string hotText = character.ToString();


        public settings()
        {
            InitializeComponent();

            textBox2.Text = Properties.Settings.Default.rpi;
            textBox_Port.Text = Properties.Settings.Default.port;
            textBox_Username.Text = Properties.Settings.Default.username;

            comboBox1.SelectedIndex = Properties.Settings.Default.colSearch;
            comboBox2.SelectedIndex = Properties.Settings.Default.colDupli;
            textBox1.Text = "0";

            //password
            if (Properties.Settings.Default.cipher != null && Properties.Settings.Default.entropy != null)
            {
                byte[] plaintext = ProtectedData.Unprotect(Properties.Settings.Default.cipher, Properties.Settings.Default.entropy,
                  DataProtectionScope.CurrentUser);
                textBox_Password.Text = ClassHelp.ByteArrayToString(plaintext);
            }
            else
            {
                textBox_Password.Text = "";
            }


        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
           this.Close();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.rpi = textBox2.Text;
            Properties.Settings.Default.port = textBox_Port.Text;
            Properties.Settings.Default.username = textBox_Username.Text;
            
            // Data to protect. Convert a string to a byte[] using Encoding.UTF8.GetBytes().
            byte[] plaintext = System.Text.ASCIIEncoding.Default.GetBytes(textBox_Password.Text); ;


            // Generate additional entropy (will be used as the Initialization vector)
            byte[] entropy = new byte[20];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }

            byte[] ciphertext = ProtectedData.Protect(plaintext, entropy,
                DataProtectionScope.CurrentUser);

            //https://stackoverflow.com/questions/1766610/how-to-store-int-array-in-application-settings
            Properties.Settings.Default.cipher = ciphertext;
            Properties.Settings.Default.entropy = entropy;
            //  write preferences settings
            Properties.Settings.Default.Save();
        }

      
        private void textBox_hot_Click(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.colSearch = comboBox1.SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.colDupli = comboBox2.SelectedIndex;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           int val = 0;

            if (!Int32.TryParse(textBox1.Text, out val))
            {
                textBox1.Text = "";
            }
            Properties.Settings.Default.leftshift = val;

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
