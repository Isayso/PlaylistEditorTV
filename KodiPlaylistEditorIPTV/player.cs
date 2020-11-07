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
using System.Drawing;
using System.Windows.Forms;

namespace PlaylistEditor
{
    public partial class player : Form 
    {
        public DataGridView Dgv { get; set; }
        private readonly double opc;
        private double opacity = 1;

        public player()
        {
            InitializeComponent();
            TopMost = true;
            button_Top.BackColor = Color.DeepSkyBlue;
            button_Top.ForeColor = Color.Black;
            this.MaximizeBox = false;

            opc = Properties.Settings.Default.opacity;
            this.Opacity = opc;

            MouseMove += OnMouseMove;
            MouseLeave += OnMouseLeave;

            HookMouseMove(this.Controls);

        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Control ctl = sender as Control;
            if (ctl != null)
            {
                opacity = 1;
                timer1.Enabled = false;
                this.Opacity = 1;
               
            }
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            timer1.Enabled = true;

        }

        /// <summary>
        /// hook event to all controls on form
        /// </summary>
        /// <param name="ctls"></param>
        private void HookMouseMove(Control.ControlCollection ctls)
        {
            foreach (Control ctl in ctls)
            {
                ctl.MouseMove += OnMouseMove;
                HookMouseMove(ctl.Controls);
            }
        }


        private void player_Move(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        //move window with mouse down
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();

        private void popup_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                // Checks if Y = 0, if so maximize the form
                if (this.Location.Y == 0) { this.WindowState = FormWindowState.Maximized; }

                Properties.Settings.Default.F1Location = this.Location;
                Properties.Settings.Default.Save();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //invoke EventHandler
            timer1.Enabled = true;
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void ComboBox_Click(object sender, EventArgs e)
        {
            comboBox1.DroppedDown = true;
        }


        private void button_Top_Click(object sender, EventArgs e)
        {
            if (TopMost == false)
            {
                TopMost = true;
                button_Top.BackColor = Color.DeepSkyBlue;
                button_Top.ForeColor = Color.Black;
            }
            else if (TopMost == true)
            {
                TopMost = false;
                button_Top.BackColor = Color.MidnightBlue;
                button_Top.ForeColor = SystemColors.Control;
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Opacity = opc;
            Properties.Settings.Default.F1Location = this.Location;
           
            Properties.Settings.Default.Save();

            this.Close();
        }

        private async void button_kodi_Click(object sender, EventArgs e)
        {           
            string jLink = Dgv.Rows[comboBox1.SelectedIndex].Cells[5].Value.ToString();

            //json string Kodi
            jLink = "{ \"jsonrpc\":\"2.0\",\"method\":\"Player.Open\",\"params\":{ \"item\":{ \"file\":\"" + jLink + "\"} },\"id\":0}";

            await ClassKodi.RunOnKodi(jLink);
        }


        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            comboBox1.DroppedDown = false;

            if (e.KeyCode == Keys.Enter)
            {
                comboBox1.Focus();
            }
        }

        private void playerCombo_MouseEnter(object sender, EventArgs e)
        {

            if (!CompItemsWithBox())
            {
                comboBox1.BeginUpdate();
                comboBox1.Items.Clear();

                for (int i = 0; i < Dgv.Rows.Count; i++)
                {
                    comboBox1.Items.Add(Dgv.Rows[i].Cells[4].Value.ToString());
                }
                comboBox1.EndUpdate();
            }
        }

        /// <summary>
        /// compares datagrid with combobox entries
        /// </summary>
        /// <returns>bool</returns>
        private bool CompItemsWithBox()
        {
            for (int i = 0; i < Dgv.Rows.Count; i++)
            {
                if (comboBox1.Items[i].ToString() != Dgv.Rows[i].Cells[4].Value.ToString())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// timer to fade out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            opacity -= 0.03;
            this.Opacity = opacity;
            if (opacity < opc)
            {
                timer1.Enabled = false;
            }
            Invalidate();
        }

        private void player_Load(object sender, EventArgs e)
        {

        }
    }
   

}
