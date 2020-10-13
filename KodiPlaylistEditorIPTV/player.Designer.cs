namespace PlaylistEditor
{
    partial class player
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(player));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button_kodi = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_Top = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.comboBox1 = new PlaylistEditor.MyComboBox();
            this.SuspendLayout();
            // 
            // button_kodi
            // 
            resources.ApplyResources(this.button_kodi, "button_kodi");
            this.button_kodi.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_kodi.FlatAppearance.BorderSize = 0;
            this.button_kodi.Name = "button_kodi";
            this.button_kodi.TabStop = false;
            this.toolTip1.SetToolTip(this.button_kodi, resources.GetString("button_kodi.ToolTip"));
            this.button_kodi.UseVisualStyleBackColor = true;
            this.button_kodi.Click += new System.EventHandler(this.button_kodi_Click);
            // 
            // button_cancel
            // 
            resources.ApplyResources(this.button_cancel, "button_cancel");
            this.button_cancel.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_cancel.FlatAppearance.BorderSize = 0;
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.TabStop = false;
            this.toolTip1.SetToolTip(this.button_cancel, resources.GetString("button_cancel.ToolTip"));
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_Top
            // 
            resources.ApplyResources(this.button_Top, "button_Top");
            this.button_Top.FlatAppearance.BorderSize = 0;
            this.button_Top.Name = "button_Top";
            this.button_Top.TabStop = false;
            this.toolTip1.SetToolTip(this.button_Top, resources.GetString("button_Top.ToolTip"));
            this.button_Top.UseVisualStyleBackColor = true;
            this.button_Top.Click += new System.EventHandler(this.button_Top_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // comboBox1
            // 
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox1.BackColor = System.Drawing.Color.MidnightBlue;
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_SelectionChangeCommitted);
            this.comboBox1.Click += new System.EventHandler(this.ComboBox_Click);
            this.comboBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox1_KeyDown);
            // 
            // player
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.Controls.Add(this.button_kodi);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_Top);
            this.Controls.Add(this.comboBox1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "player";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.popup_MouseDown);
            this.MouseEnter += new System.EventHandler(this.playerCombo_MouseEnter);
            this.Move += new System.EventHandler(this.player_Move);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button_kodi;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_Top;
        public PlaylistEditor.MyComboBox comboBox1;
        private System.Windows.Forms.Timer timer1;
    }
}