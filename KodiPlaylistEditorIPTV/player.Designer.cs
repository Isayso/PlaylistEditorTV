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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button_Top = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_kodi = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.MidnightBlue;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.78182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(43, 10);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.comboBox1.Size = new System.Drawing.Size(241, 30);
            this.comboBox1.TabIndex = 61;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox1.Click += new System.EventHandler(this.ComboBox_Click);
            // 
            // button_Top
            // 
            this.button_Top.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_Top.BackgroundImage")));
            this.button_Top.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_Top.FlatAppearance.BorderSize = 0;
            this.button_Top.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Top.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.74545F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Top.Location = new System.Drawing.Point(7, 9);
            this.button_Top.Margin = new System.Windows.Forms.Padding(2);
            this.button_Top.Name = "button_Top";
            this.button_Top.Size = new System.Drawing.Size(30, 32);
            this.button_Top.TabIndex = 62;
            this.button_Top.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button_Top, "Set to Foreground");
            this.button_Top.UseVisualStyleBackColor = true;
            this.button_Top.Click += new System.EventHandler(this.button_Top_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_cancel.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_cancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_cancel.BackgroundImage")));
            this.button_cancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_cancel.FlatAppearance.BorderSize = 0;
            this.button_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_cancel.Location = new System.Drawing.Point(288, 6);
            this.button_cancel.Margin = new System.Windows.Forms.Padding(0);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(33, 37);
            this.button_cancel.TabIndex = 63;
            this.toolTip1.SetToolTip(this.button_cancel, "cancel");
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_kodi
            // 
            this.button_kodi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_kodi.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_kodi.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_kodi.BackgroundImage")));
            this.button_kodi.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_kodi.FlatAppearance.BorderSize = 0;
            this.button_kodi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_kodi.Location = new System.Drawing.Point(320, 9);
            this.button_kodi.Margin = new System.Windows.Forms.Padding(0);
            this.button_kodi.Name = "button_kodi";
            this.button_kodi.Size = new System.Drawing.Size(34, 31);
            this.button_kodi.TabIndex = 66;
            this.toolTip1.SetToolTip(this.button_kodi, "play with Kodi\r\nCtrl+p\r\nCtrl + double click cell");
            this.button_kodi.UseVisualStyleBackColor = true;
            this.button_kodi.Click += new System.EventHandler(this.button_kodi_Click);
            // 
            // player
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(356, 55);
            this.Controls.Add(this.button_kodi);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_Top);
            this.Controls.Add(this.comboBox1);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "player";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "player";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.popup_MouseDown);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button_Top;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_kodi;
    }
}