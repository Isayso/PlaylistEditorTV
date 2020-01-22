using System;
using System.Drawing;
using System.Windows.Forms;

namespace PlaylistEditor
{
    public partial class player : Form 
    {
        public DataGridView Dgv { get; set; }

        public player()
        {
            InitializeComponent();
            TopMost = true;
            button_Top.BackColor = Color.DeepSkyBlue;
            button_Top.ForeColor = Color.Black;
            this.MaximizeBox = false;

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
           // int i;
        }

        private void ComboBox_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.BeginUpdate();
            for (int i = 0; i < Dgv.Rows.Count; i++)
            {
                comboBox1.Items.Add(Dgv.Rows[i].Cells[4].Value.ToString());
            }
            comboBox1.EndUpdate();
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
    }
   

}
