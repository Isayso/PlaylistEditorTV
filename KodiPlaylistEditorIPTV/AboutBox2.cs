using System;
using System.Windows.Forms;

namespace PlaylistEditor
{
    public partial class AboutBox2 : Form
    {
        public AboutBox2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("bc1q0cte24tuax2kx25kypeqtewk73rvggqtqw9pzc");
        }

        private void donateButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=8FF26SM3X8UAN");
        }
    }
}
