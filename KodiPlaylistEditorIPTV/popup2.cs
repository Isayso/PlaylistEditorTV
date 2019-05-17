// GNU GENERAL PUBLIC LICENSE                       Version 3, 29 June 2007
using System.Windows.Forms;

namespace PlaylistEditor
{
    public partial class popup2 : Form
    {
        public popup2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, System.EventArgs e)
        {

        }
        public void color(string backgcl)
        {

            switch (backgcl)
            {
                case "green":
                    this.BackColor = System.Drawing.Color.DarkGreen;
                    //label1.Text = "Kodi says: OK";
                    break;

                case "red":
                    this.BackColor = System.Drawing.Color.DarkRed;
                    //label1.Text = "Kodi says: ERROR";
                    break;

                case "blue":
                    this.BackColor = System.Drawing.Color.MidnightBlue;
                   // label1.Text = "YouTube Link added";
                    break;

            }



        }
    }
}
