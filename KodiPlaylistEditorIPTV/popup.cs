using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaylistEditor
{
    public partial class popup : Form
    {
        public event Action Canceled;

        public popup()
        {
            InitializeComponent();
           // this.CenterToParent();
          
        }

        private void popup_Load(object sender, EventArgs e)
        {
            
        }


        public void updateProgressBar(string updatedTextToDisplay)
        {
           // MyProgressBarControl.Value++;
            label1.Text = updatedTextToDisplay;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (Canceled != null)
                Canceled();
        }


        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

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
    }
}
