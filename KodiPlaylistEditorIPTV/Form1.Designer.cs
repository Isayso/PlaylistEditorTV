namespace PlaylistEditor
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button_dup = new System.Windows.Forms.Button();
            this.button_search = new System.Windows.Forms.Button();
            this.button_del_all = new System.Windows.Forms.Button();
            this.button_add = new System.Windows.Forms.Button();
            this.button_delLine = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.button_open = new System.Windows.Forms.Button();
            this.buttonR_MoveDown = new RepeatingButton();
            this.buttonR_moveUp = new RepeatingButton();
            this.button_revert = new System.Windows.Forms.Button();
            this.textBox_find = new System.Windows.Forms.TextBox();
            this.button_settings = new System.Windows.Forms.Button();
            this.button_Info = new System.Windows.Forms.Button();
            this.plabel_Filename = new PathLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowDrop = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.Gray;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.163636F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Location = new System.Drawing.Point(-3, 56);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1126, 310);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragDrop);
            this.dataGridView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragEnter);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyRowMenuItem,
            this.pasteRowMenuItem,
            this.cutRowMenuItem,
            this.toolStripSeparator1,
            this.toolStripCopy,
            this.toolStripPaste});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(197, 130);
            // 
            // copyRowMenuItem
            // 
            this.copyRowMenuItem.Name = "copyRowMenuItem";
            this.copyRowMenuItem.ShortcutKeyDisplayString = "Strg+R";
            this.copyRowMenuItem.Size = new System.Drawing.Size(196, 24);
            this.copyRowMenuItem.Text = "Copy Row";
            this.copyRowMenuItem.Click += new System.EventHandler(this.copyRowMenuItem_Click);
            // 
            // pasteRowMenuItem
            // 
            this.pasteRowMenuItem.Name = "pasteRowMenuItem";
            this.pasteRowMenuItem.ShortcutKeyDisplayString = "Strg+I";
            this.pasteRowMenuItem.Size = new System.Drawing.Size(196, 24);
            this.pasteRowMenuItem.Text = "Insert Row";
            this.pasteRowMenuItem.Click += new System.EventHandler(this.pasteRowMenuItem_Click);
            // 
            // cutRowMenuItem
            // 
            this.cutRowMenuItem.Name = "cutRowMenuItem";
            this.cutRowMenuItem.ShortcutKeyDisplayString = "Strg+X";
            this.cutRowMenuItem.Size = new System.Drawing.Size(196, 24);
            this.cutRowMenuItem.Text = "Cut Row";
            this.cutRowMenuItem.Click += new System.EventHandler(this.cutRowMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
            // 
            // toolStripCopy
            // 
            this.toolStripCopy.Name = "toolStripCopy";
            this.toolStripCopy.ShortcutKeyDisplayString = "Strg+C";
            this.toolStripCopy.Size = new System.Drawing.Size(196, 24);
            this.toolStripCopy.Text = "Copy Cells";
            this.toolStripCopy.Click += new System.EventHandler(this.toolStripCopy_Click);
            // 
            // toolStripPaste
            // 
            this.toolStripPaste.Name = "toolStripPaste";
            this.toolStripPaste.ShortcutKeyDisplayString = "Strg+V";
            this.toolStripPaste.Size = new System.Drawing.Size(196, 24);
            this.toolStripPaste.Text = "Paste Cells";
            this.toolStripPaste.Click += new System.EventHandler(this.toolStripPaste_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "m3u";
            this.openFileDialog1.Filter = "Playlist|*.m3u";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "m3u";
            this.saveFileDialog1.Filter = "Playlist|*.m3u";
            // 
            // button_dup
            // 
            this.button_dup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_dup.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_dup.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_dup.BackgroundImage")));
            this.button_dup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_dup.FlatAppearance.BorderSize = 0;
            this.button_dup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_dup.Location = new System.Drawing.Point(970, 14);
            this.button_dup.Margin = new System.Windows.Forms.Padding(0);
            this.button_dup.Name = "button_dup";
            this.button_dup.Size = new System.Drawing.Size(25, 37);
            this.button_dup.TabIndex = 35;
            this.toolTip1.SetToolTip(this.button_dup, "find duplicates");
            this.button_dup.UseVisualStyleBackColor = true;
            this.button_dup.Click += new System.EventHandler(this.button_dup_Click);
            // 
            // button_search
            // 
            this.button_search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_search.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_search.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_search.BackgroundImage")));
            this.button_search.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_search.FlatAppearance.BorderSize = 0;
            this.button_search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_search.Location = new System.Drawing.Point(1009, 14);
            this.button_search.Margin = new System.Windows.Forms.Padding(0);
            this.button_search.Name = "button_search";
            this.button_search.Size = new System.Drawing.Size(25, 37);
            this.button_search.TabIndex = 33;
            this.toolTip1.SetToolTip(this.button_search, "search");
            this.button_search.UseVisualStyleBackColor = true;
            this.button_search.Click += new System.EventHandler(this.button_search_Click);
            // 
            // button_del_all
            // 
            this.button_del_all.BackgroundImage = global::PlaylistEditor.Properties.Resources.delete_forever_outline;
            this.button_del_all.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_del_all.FlatAppearance.BorderSize = 0;
            this.button_del_all.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_del_all.Location = new System.Drawing.Point(284, 10);
            this.button_del_all.Margin = new System.Windows.Forms.Padding(2);
            this.button_del_all.Name = "button_del_all";
            this.button_del_all.Size = new System.Drawing.Size(30, 32);
            this.button_del_all.TabIndex = 29;
            this.button_del_all.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button_del_all, "delete list");
            this.button_del_all.UseVisualStyleBackColor = true;
            this.button_del_all.Click += new System.EventHandler(this.button_del_all_Click);
            // 
            // button_add
            // 
            this.button_add.BackgroundImage = global::PlaylistEditor.Properties.Resources.playlist_plus;
            this.button_add.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_add.FlatAppearance.BorderSize = 0;
            this.button_add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_add.Location = new System.Drawing.Point(241, 11);
            this.button_add.Margin = new System.Windows.Forms.Padding(2);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(30, 32);
            this.button_add.TabIndex = 27;
            this.button_add.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button_add, "add row");
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // button_delLine
            // 
            this.button_delLine.BackgroundImage = global::PlaylistEditor.Properties.Resources.close_box_outline__1_;
            this.button_delLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_delLine.FlatAppearance.BorderSize = 0;
            this.button_delLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_delLine.Location = new System.Drawing.Point(138, 11);
            this.button_delLine.Margin = new System.Windows.Forms.Padding(2);
            this.button_delLine.Name = "button_delLine";
            this.button_delLine.Size = new System.Drawing.Size(30, 32);
            this.button_delLine.TabIndex = 2;
            this.button_delLine.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button_delLine, "delete selected rows");
            this.button_delLine.UseVisualStyleBackColor = true;
            this.button_delLine.Click += new System.EventHandler(this.button_delLine_Click);
            // 
            // button_save
            // 
            this.button_save.BackgroundImage = global::PlaylistEditor.Properties.Resources.content_save__1_;
            this.button_save.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_save.FlatAppearance.BorderSize = 0;
            this.button_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_save.Location = new System.Drawing.Point(58, 2);
            this.button_save.Margin = new System.Windows.Forms.Padding(2);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(45, 49);
            this.button_save.TabIndex = 1;
            this.button_save.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button_save, "save m3u");
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_open
            // 
            this.button_open.BackgroundImage = global::PlaylistEditor.Properties.Resources.open_in_app__1_;
            this.button_open.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_open.FlatAppearance.BorderSize = 0;
            this.button_open.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_open.Location = new System.Drawing.Point(9, 2);
            this.button_open.Margin = new System.Windows.Forms.Padding(2);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(45, 49);
            this.button_open.TabIndex = 0;
            this.button_open.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button_open, "open m3u");
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // buttonR_MoveDown
            // 
            this.buttonR_MoveDown.BackgroundImage = global::PlaylistEditor.Properties.Resources.arrow_down_bold__1_;
            this.buttonR_MoveDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonR_MoveDown.FlatAppearance.BorderSize = 0;
            this.buttonR_MoveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonR_MoveDown.Location = new System.Drawing.Point(205, 11);
            this.buttonR_MoveDown.Margin = new System.Windows.Forms.Padding(0);
            this.buttonR_MoveDown.Name = "buttonR_MoveDown";
            this.buttonR_MoveDown.Size = new System.Drawing.Size(30, 32);
            this.buttonR_MoveDown.TabIndex = 32;
            this.buttonR_MoveDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonR_MoveDown, "move row down");
            this.buttonR_MoveDown.UseVisualStyleBackColor = true;
            this.buttonR_MoveDown.Click += new System.EventHandler(this.button_moveDown_Click);
            // 
            // buttonR_moveUp
            // 
            this.buttonR_moveUp.BackgroundImage = global::PlaylistEditor.Properties.Resources.arrow_up_bold__1_;
            this.buttonR_moveUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonR_moveUp.FlatAppearance.BorderSize = 0;
            this.buttonR_moveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonR_moveUp.Location = new System.Drawing.Point(175, 9);
            this.buttonR_moveUp.Margin = new System.Windows.Forms.Padding(0);
            this.buttonR_moveUp.Name = "buttonR_moveUp";
            this.buttonR_moveUp.Size = new System.Drawing.Size(30, 32);
            this.buttonR_moveUp.TabIndex = 31;
            this.buttonR_moveUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.buttonR_moveUp, "move row up");
            this.buttonR_moveUp.UseVisualStyleBackColor = true;
            this.buttonR_moveUp.Click += new System.EventHandler(this.button_moveUp_Click);
            // 
            // button_revert
            // 
            this.button_revert.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_revert.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_revert.BackgroundImage")));
            this.button_revert.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_revert.FlatAppearance.BorderSize = 0;
            this.button_revert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_revert.Location = new System.Drawing.Point(375, 9);
            this.button_revert.Margin = new System.Windows.Forms.Padding(0);
            this.button_revert.Name = "button_revert";
            this.button_revert.Size = new System.Drawing.Size(25, 37);
            this.button_revert.TabIndex = 36;
            this.toolTip1.SetToolTip(this.button_revert, "reload file");
            this.button_revert.UseVisualStyleBackColor = true;
            this.button_revert.Visible = false;
            this.button_revert.Click += new System.EventHandler(this.button_revert_Click);
            // 
            // textBox_find
            // 
            this.textBox_find.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_find.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.78182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_find.Location = new System.Drawing.Point(934, 66);
            this.textBox_find.Name = "textBox_find";
            this.textBox_find.Size = new System.Drawing.Size(167, 28);
            this.textBox_find.TabIndex = 34;
            this.textBox_find.Text = "find";
            this.textBox_find.Visible = false;
            this.textBox_find.Click += new System.EventHandler(this.textBox_selectAll_Click);
            this.textBox_find.TextChanged += new System.EventHandler(this.textBox_find_TextChanged);
            // 
            // button_settings
            // 
            this.button_settings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_settings.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_settings.BackgroundImage = global::PlaylistEditor.Properties.Resources.settings_outline;
            this.button_settings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_settings.FlatAppearance.BorderSize = 0;
            this.button_settings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_settings.Location = new System.Drawing.Point(1049, 15);
            this.button_settings.Margin = new System.Windows.Forms.Padding(0);
            this.button_settings.Name = "button_settings";
            this.button_settings.Size = new System.Drawing.Size(25, 37);
            this.button_settings.TabIndex = 28;
            this.button_settings.UseVisualStyleBackColor = true;
            this.button_settings.Click += new System.EventHandler(this.button_settings_Click);
            // 
            // button_Info
            // 
            this.button_Info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Info.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_Info.BackgroundImage = global::PlaylistEditor.Properties.Resources.information_outline;
            this.button_Info.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_Info.FlatAppearance.BorderSize = 0;
            this.button_Info.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Info.Location = new System.Drawing.Point(1087, 15);
            this.button_Info.Margin = new System.Windows.Forms.Padding(0);
            this.button_Info.Name = "button_Info";
            this.button_Info.Size = new System.Drawing.Size(25, 37);
            this.button_Info.TabIndex = 24;
            this.button_Info.UseVisualStyleBackColor = true;
            this.button_Info.Click += new System.EventHandler(this.button_Info_Click);
            // 
            // plabel_Filename
            // 
            this.plabel_Filename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plabel_Filename.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.78182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.plabel_Filename.ForeColor = System.Drawing.SystemColors.Control;
            this.plabel_Filename.Location = new System.Drawing.Point(405, 16);
            this.plabel_Filename.Name = "plabel_Filename";
            this.plabel_Filename.Size = new System.Drawing.Size(483, 23);
            this.plabel_Filename.TabIndex = 26;
            this.plabel_Filename.Text = "pathLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(1122, 364);
            this.Controls.Add(this.button_revert);
            this.Controls.Add(this.button_dup);
            this.Controls.Add(this.textBox_find);
            this.Controls.Add(this.button_search);
            this.Controls.Add(this.buttonR_MoveDown);
            this.Controls.Add(this.buttonR_moveUp);
            this.Controls.Add(this.button_del_all);
            this.Controls.Add(this.button_settings);
            this.Controls.Add(this.button_add);
            this.Controls.Add(this.plabel_Filename);
            this.Controls.Add(this.button_Info);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button_delLine);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.button_open);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Playlist Editor TV";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_open;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_delLine;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button_Info;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
        private PathLabel plabel_Filename;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Button button_settings;
        private System.Windows.Forms.Button button_del_all;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyRowMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteRowMenuItem;
        private RepeatingButton buttonR_moveUp;
        private RepeatingButton buttonR_MoveDown;
        private System.Windows.Forms.Button button_search;
        private System.Windows.Forms.TextBox textBox_find;
        private System.Windows.Forms.Button button_dup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripPaste;
        private System.Windows.Forms.ToolStripMenuItem cutRowMenuItem;
        private System.Windows.Forms.Button button_revert;
    }
}

