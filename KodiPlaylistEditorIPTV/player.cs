using System;
using System.Drawing;
using System.Windows.Forms;

namespace PlaylistEditor
{
    public partial class player : Form 
    {
        //static Form1 myForm = new Form1();
        // // Form1 myForm = Application.OpenForms.OfType<Form1>().ElementAt<Form1>(0); //Get current open Form2
        // DataGridView data =  myForm.dataGridView1;
        public DataGridView Dgv { get; set; }

        public player()
        {
            InitializeComponent();
            TopMost = true;
            button_Top.BackColor = Color.DeepSkyBlue;
            button_Top.ForeColor = Color.Black;

           // DataGridView data = form1_.dataGridView1;

          //  data.Rows.Count

            //comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

        }
       
        //move window with mouse down
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var channel = comboBox1.SelectedIndex;
           // int i;
        }

        private void ComboBox_Click(object sender, EventArgs e)
        {
           
            //  funData();
            comboBox1.Items.Clear();
            for (int i = 0; i < Dgv.Rows.Count; i++)
            {
                comboBox1.Items.Add(Dgv.Rows[i].Cells[4].Value.ToString());
            }

            comboBox1.DroppedDown = true;
            //ComboBox obj = sender as ComboBox;
            //obj.DroppedDown = true;
        }

        //public void funData(DataGridView data)
        //{
        //    comboBox1.Items.Clear();
        //    for (int i = 0; i < data.Rows.Count; i++)
        //    {
        //        comboBox1.Items.Add(data.Rows[i].Cells[4].Value.ToString());
        //    }
        //    comboBox1.DroppedDown = true;

        //    //  label1.Text = txtForm1.Text;
        //}

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
            this.Close();
        }
    }
}
