using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PlaylistEditor
{
    public partial class player : Form 
    {
        public DataGridView Dgv { get; set; }
        private int mouseEnterCount = 0;
        private double opc;
        //public BindingList<string> bindinglist = new BindingList<string>();
        //public BindingSource bSource = new BindingSource();

        public player()
        {
            InitializeComponent();
            TopMost = true;
            button_Top.BackColor = Color.DeepSkyBlue;
            button_Top.ForeColor = Color.Black;
            this.MaximizeBox = false;

            opc = Properties.Settings.Default.opacity;
            this.Opacity = opc;

            //bSource.DataSource = bindinglist;
            //comboBox1.DataSource = bSource;



            //for (int i = 0; i < Dgv.Rows.Count; i++)
            //{
            //    bindinglist.Add(Dgv.Rows[i].Cells[4].Value.ToString());
            //}
           
           
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
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var channel = comboBox1.SelectedIndex;

            //invoke EventHandler
        }

        private void ComboBox_Click(object sender, EventArgs e)
        {

            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();

            for (int i = 0; i < Dgv.Rows.Count; i++)
            {
                comboBox1.Items.Add(Dgv.Rows[i].Cells[4].Value.ToString());
            }
            comboBox1.EndUpdate();
           // bindinglist.ResetBindings();
            comboBox1.DroppedDown = true;
        }

        private void ComboBox_Click2(object sender, EventArgs e)
        {
            ComboBox obj = sender as ComboBox;
            obj.DroppedDown = true;
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

            await ClassKodi.Run2(jLink);
        }

        private void player_MouseHover(object sender, EventArgs e)
        {
            if (++mouseEnterCount == 1)
            {
                this.Opacity = 1.0;
            }
        }

        private void player_MouseLeave(object sender, EventArgs e)
        {

            if (--mouseEnterCount == 0)
            {
                this.Opacity = opc; // Properties.Settings.Default.opacity;
            } 
        }

        private void player_MouseEnter(object sender, EventArgs e)
        {
            if (++mouseEnterCount == 1)
            {
                this.Opacity = 1;
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // comboBox1.Focus();
                e.Handled = false;
            }
        }
    }
   

}
