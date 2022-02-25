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
    public partial class EditHeader : Form
    {
        public string headerText { get; set; }
        public EditHeader(string fileh)
        {
            InitializeComponent();

            textBox1.Text = fileh;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.headerText = textBox1.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
