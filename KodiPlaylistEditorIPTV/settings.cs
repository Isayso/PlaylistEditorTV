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
using System.Text;
using System.Windows.Forms;
using PlaylistEditor.Properties;

namespace PlaylistEditor
{

    public partial class settings : Form
    {
        public string serverName;
        public  bool isLinux ;
        public bool replaceDrive ;
        static readonly int unicode = Settings.Default.hotkey;
        static char character = (char)unicode;
        string hotText = character.ToString();


        public settings()
        {
            InitializeComponent();

            textBox2.Text = Settings.Default.rpi;
            textBox_Port.Text = Settings.Default.port;
            textBox_Username.Text = Settings.Default.username;

            checkBox_vlc.Checked = Settings.Default.vlc_fullsreen;
           // checkBox_plugin.Checked = Settings.Default.user_agent;

            comboBox1.SelectedIndex = Settings.Default.colSearch;
            comboBox2.SelectedIndex = Settings.Default.colDupli;
            textBox1.Text = "0";
            textBox_userAgent.Text = Settings.Default.user_agent;
           

            //password
            if (Settings.Default.cipher != null && Settings.Default.entropy != null)
            {
                byte[] plaintext = ProtectedData.Unprotect(Settings.Default.cipher, Settings.Default.entropy,
                  DataProtectionScope.CurrentUser);
                textBox_Password.Text = ClassHelp.ByteArrayToString(plaintext);
            }
            else
            {
                textBox_Password.Text = "";
            }


            textBox_hot.Text = hotText;
            setHotkeyInt();

        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
           this.Close();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            getHotkeyInt();

            Settings.Default.rpi = textBox2.Text;
            Settings.Default.port = textBox_Port.Text;
            Settings.Default.username = textBox_Username.Text;
            Settings.Default.vlc_fullsreen = checkBox_vlc.Checked;
            //  Settings.Default.user_agent = checkBox_plugin.Checked;
            Settings.Default.user_agent = textBox_userAgent.Text;



            // Data to protect. Convert a string to a byte[] using Encoding.UTF8.GetBytes().
            byte[] plaintext = Encoding.Default.GetBytes(textBox_Password.Text); ;


            // Generate additional entropy (will be used as the Initialization vector)
            byte[] entropy = new byte[20];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(entropy);
            }

            byte[] ciphertext = ProtectedData.Protect(plaintext, entropy,
                DataProtectionScope.CurrentUser);

            //https://stackoverflow.com/questions/1766610/how-to-store-int-array-in-application-settings
            Settings.Default.cipher = ciphertext;
            Settings.Default.entropy = entropy;
            //  write preferences settings
            Settings.Default.Save();
        }

      
        private void textBox_hot_Click(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.colSearch = comboBox1.SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.colDupli = comboBox2.SelectedIndex;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(textBox1.Text, out int val))
            {
                textBox1.Text = "";
            }
            Settings.Default.leftshift = val;

        }

        private void ComboBox_Click(object sender, EventArgs e)
        {
            ComboBox obj = sender as ComboBox;
            obj.DroppedDown = true;
        }

        private void getHotkeyInt()
        {

            //bin from checkboxes?? 
            int spec_a = checkBox_a.Checked ? 1 : 0;
            int spec_c = checkBox_c.Checked ? 2 : 0;
            int spec_s = checkBox_s.Checked ? 4 : 0;
            int spec_w = checkBox_w.Checked ? 8 : 0;

            byte[] charByte = Encoding.ASCII.GetBytes(textBox_hot.Text.ToString());
#if DEBUG
            Console.WriteLine(charByte[0]);
#endif
            int spec_key = spec_a + spec_c + spec_s + spec_w;
            Settings.Default.specKey = spec_key;
            Settings.Default.hotkey = charByte[0];
            //         NativeMethods.RegisterHotKey(this.Handle, 1, spec_key, charByte[0]);  //ALT-Y

        }

        private void setHotkeyInt()
        {
            checkBox_a.Checked = false;
            checkBox_c.Checked = false;
            checkBox_s.Checked = false;
            checkBox_w.Checked = false;

            //Modifier keys codes: Alt = 1, Ctrl = 2, Shift = 4, Win = 8  must be added
            var spec_key = Settings.Default.specKey;
            var binary = Convert.ToString(spec_key, 2);
            binary = binary.PadLeft(4, '0');
            char a = binary[3]; if (a.Equals('1')) checkBox_a.Checked = true;
            char c = binary[2]; if (c.Equals('1')) checkBox_c.Checked = true;
            char s = binary[1]; if (s.Equals('1')) checkBox_s.Checked = true;
            char w = binary[0]; if (w.Equals('1')) checkBox_w.Checked = true;

            var hotlabel = (char)Settings.Default.hotkey;
            textBox_hot.Text = hotlabel.ToString();


        }

        private void textBox_hot_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }
    }
}
