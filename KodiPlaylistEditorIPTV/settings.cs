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
          
           
            comboBox1.SelectedIndex = Properties.Settings.Default.colSearch;
            comboBox2.SelectedIndex = Properties.Settings.Default.colDupli;
            textBox1.Text = "0";
        
            
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
           this.Close();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
       
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
