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
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.singleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button_check = new System.Windows.Forms.Button();
            this.button_vlc = new System.Windows.Forms.Button();
            this.button_revert = new System.Windows.Forms.Button();
            this.button_dup = new System.Windows.Forms.Button();
            this.button_search = new System.Windows.Forms.Button();
            this.button_del_all = new System.Windows.Forms.Button();
            this.button_add = new System.Windows.Forms.Button();
            this.button_delLine = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.button_open = new System.Windows.Forms.Button();
            this.UndoButton = new System.Windows.Forms.Button();
            this.RedoButton = new System.Windows.Forms.Button();
            this.button_import = new System.Windows.Forms.Button();
            this.button_moveDown = new RepeatingButton();
            this.button_moveUp = new RepeatingButton();
            this.button_kodi = new System.Windows.Forms.Button();
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
            this.dataGridView1.Location = new System.Drawing.Point(0, 59);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 47;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1122, 319);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            this.dataGridView1.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellValidated);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            this.dataGridView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragDrop);
            this.dataGridView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragEnter);
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
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
            this.toolStripPaste,
            this.toolStripSeparator2,
            this.playToolStripMenuItem,
            this.toolStripSeparator3,
            this.hideToolStripMenuItem,
            this.showToolStripMenuItem,
            this.toolStripSeparator4,
            this.singleToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(203, 244);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // copyRowMenuItem
            // 
            this.copyRowMenuItem.Name = "copyRowMenuItem";
            this.copyRowMenuItem.ShortcutKeyDisplayString = "Ctrl+R";
            this.copyRowMenuItem.Size = new System.Drawing.Size(202, 24);
            this.copyRowMenuItem.Text = "Copy Row";
            this.copyRowMenuItem.Click += new System.EventHandler(this.copyRowMenuItem_Click);
            // 
            // pasteRowMenuItem
            // 
            this.pasteRowMenuItem.Enabled = false;
            this.pasteRowMenuItem.Name = "pasteRowMenuItem";
            this.pasteRowMenuItem.ShortcutKeyDisplayString = "Ctrl+I";
            this.pasteRowMenuItem.Size = new System.Drawing.Size(202, 24);
            this.pasteRowMenuItem.Text = "Paste Row";
            this.pasteRowMenuItem.Click += new System.EventHandler(this.pasteRowMenuItem_Click);
            // 
            // cutRowMenuItem
            // 
            this.cutRowMenuItem.Name = "cutRowMenuItem";
            this.cutRowMenuItem.ShortcutKeyDisplayString = "Ctrl+X";
            this.cutRowMenuItem.Size = new System.Drawing.Size(202, 24);
            this.cutRowMenuItem.Text = "Cut Row";
            this.cutRowMenuItem.Click += new System.EventHandler(this.cutRowMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(199, 6);
            // 
            // toolStripCopy
            // 
            this.toolStripCopy.Name = "toolStripCopy";
            this.toolStripCopy.ShortcutKeyDisplayString = "Ctrl+C";
            this.toolStripCopy.Size = new System.Drawing.Size(202, 24);
            this.toolStripCopy.Text = "Copy Cells";
            this.toolStripCopy.Click += new System.EventHandler(this.toolStripCopy_Click);
            // 
            // toolStripPaste
            // 
            this.toolStripPaste.Name = "toolStripPaste";
            this.toolStripPaste.ShortcutKeyDisplayString = "Ctrl+V";
            this.toolStripPaste.Size = new System.Drawing.Size(202, 24);
            this.toolStripPaste.Text = "Paste Cells";
            this.toolStripPaste.Click += new System.EventHandler(this.toolStripPaste_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(199, 6);
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+P";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(202, 24);
            this.playToolStripMenuItem.Text = "Kodi play";
            this.playToolStripMenuItem.ToolTipText = "Ctrl double click on cell";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(199, 6);
            // 
            // hideToolStripMenuItem
            // 
            this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            this.hideToolStripMenuItem.Size = new System.Drawing.Size(202, 24);
            this.hideToolStripMenuItem.Text = "Hide column";
            this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(202, 24);
            this.showToolStripMenuItem.Text = "Show all columns";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(199, 6);
            // 
            // singleToolStripMenuItem
            // 
            this.singleToolStripMenuItem.Name = "singleToolStripMenuItem";
            this.singleToolStripMenuItem.Size = new System.Drawing.Size(202, 24);
            this.singleToolStripMenuItem.Text = "Single column mode";
            this.singleToolStripMenuItem.Click += new System.EventHandler(this.singleToolStripMenuItem_Click);
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
            // button_check
            // 
            this.button_check.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_check.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_check.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_check.BackgroundImage")));
            this.button_check.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_check.FlatAppearance.BorderSize = 0;
            this.button_check.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_check.Location = new System.Drawing.Point(931, 14);
            this.button_check.Margin = new System.Windows.Forms.Padding(0);
            this.button_check.Name = "button_check";
            this.button_check.Size = new System.Drawing.Size(25, 37);
            this.button_check.TabIndex = 38;
            this.toolTip1.SetToolTip(this.button_check, "check for invalid links\r\n+ ctrl select links");
            this.button_check.UseVisualStyleBackColor = true;
            this.button_check.Click += new System.EventHandler(this.Button_check_Click);
            // 
            // button_vlc
            // 
            this.button_vlc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_vlc.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_vlc.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_vlc.BackgroundImage")));
            this.button_vlc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_vlc.FlatAppearance.BorderSize = 0;
            this.button_vlc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_vlc.Location = new System.Drawing.Point(773, 12);
            this.button_vlc.Margin = new System.Windows.Forms.Padding(0);
            this.button_vlc.Name = "button_vlc";
            this.button_vlc.Size = new System.Drawing.Size(38, 37);
            this.button_vlc.TabIndex = 37;
            this.toolTip1.SetToolTip(this.button_vlc, "play with vlc\r\ndouble click cell");
            this.button_vlc.UseVisualStyleBackColor = true;
            this.button_vlc.Click += new System.EventHandler(this.button_vlc_Click);
            // 
            // button_revert
            // 
            this.button_revert.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_revert.BackgroundImage = global::PlaylistEditor.Properties.Resources.reload;
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
            this.toolTip1.SetToolTip(this.button_dup, "find duplicates\r\n+shift remove duplicates");
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
            this.toolTip1.SetToolTip(this.button_search, "search\r\nCtrl+F");
            this.button_search.UseVisualStyleBackColor = true;
            this.button_search.Click += new System.EventHandler(this.button_search_Click);
            // 
            // button_del_all
            // 
            this.button_del_all.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_del_all.BackgroundImage")));
            this.button_del_all.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_del_all.FlatAppearance.BorderSize = 0;
            this.button_del_all.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_del_all.Location = new System.Drawing.Point(312, 10);
            this.button_del_all.Margin = new System.Windows.Forms.Padding(2);
            this.button_del_all.Name = "button_del_all";
            this.button_del_all.Size = new System.Drawing.Size(30, 32);
            this.button_del_all.TabIndex = 29;
            this.button_del_all.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button_del_all, "delete list\r\nCtrl+N open new window");
            this.button_del_all.UseVisualStyleBackColor = true;
            this.button_del_all.Click += new System.EventHandler(this.button_del_all_Click);
            // 
            // button_add
            // 
            this.button_add.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_add.BackgroundImage")));
            this.button_add.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_add.FlatAppearance.BorderSize = 0;
            this.button_add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_add.Location = new System.Drawing.Point(269, 11);
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
            this.button_delLine.BackgroundImage = global::PlaylistEditor.Properties.Resources.close_box_outline_1_;
            this.button_delLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_delLine.FlatAppearance.BorderSize = 0;
            this.button_delLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_delLine.Location = new System.Drawing.Point(166, 11);
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
            this.button_save.BackgroundImage = global::PlaylistEditor.Properties.Resources.content_save_1_;
            this.button_save.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_save.FlatAppearance.BorderSize = 0;
            this.button_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_save.Location = new System.Drawing.Point(58, 2);
            this.button_save.Margin = new System.Windows.Forms.Padding(2);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(45, 49);
            this.button_save.TabIndex = 1;
            this.button_save.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button_save, "save as\r\n+shift save overwrite\r\nCtrl+S save");
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_open
            // 
            this.button_open.BackgroundImage = global::PlaylistEditor.Properties.Resources.open_in_app_1_;
            this.button_open.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_open.FlatAppearance.BorderSize = 0;
            this.button_open.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_open.Location = new System.Drawing.Point(9, 2);
            this.button_open.Margin = new System.Windows.Forms.Padding(2);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(45, 49);
            this.button_open.TabIndex = 0;
            this.button_open.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button_open, "open m3u\r\ndouble click empty background\r\n");
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // UndoButton
            // 
            this.UndoButton.BackColor = System.Drawing.Color.MidnightBlue;
            this.UndoButton.BackgroundImage = global::PlaylistEditor.Properties.Resources.undo;
            this.UndoButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.UndoButton.FlatAppearance.BorderSize = 0;
            this.UndoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UndoButton.Location = new System.Drawing.Point(118, 6);
            this.UndoButton.Margin = new System.Windows.Forms.Padding(0);
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(33, 19);
            this.UndoButton.TabIndex = 62;
            this.toolTip1.SetToolTip(this.UndoButton, "undo");
            this.UndoButton.UseVisualStyleBackColor = true;
            this.UndoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // RedoButton
            // 
            this.RedoButton.BackColor = System.Drawing.Color.MidnightBlue;
            this.RedoButton.BackgroundImage = global::PlaylistEditor.Properties.Resources.redo;
            this.RedoButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RedoButton.FlatAppearance.BorderSize = 0;
            this.RedoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RedoButton.Location = new System.Drawing.Point(118, 30);
            this.RedoButton.Margin = new System.Windows.Forms.Padding(0);
            this.RedoButton.Name = "RedoButton";
            this.RedoButton.Size = new System.Drawing.Size(33, 19);
            this.RedoButton.TabIndex = 63;
            this.toolTip1.SetToolTip(this.RedoButton, "redo");
            this.RedoButton.UseVisualStyleBackColor = true;
            this.RedoButton.Click += new System.EventHandler(this.RedoButton_Click);
            // 
            // button_import
            // 
            this.button_import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_import.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_import.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_import.BackgroundImage")));
            this.button_import.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_import.FlatAppearance.BorderSize = 0;
            this.button_import.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_import.Location = new System.Drawing.Point(889, 14);
            this.button_import.Margin = new System.Windows.Forms.Padding(0);
            this.button_import.Name = "button_import";
            this.button_import.Size = new System.Drawing.Size(25, 37);
            this.button_import.TabIndex = 64;
            this.toolTip1.SetToolTip(this.button_import, "Import full list from clipboard");
            this.button_import.UseVisualStyleBackColor = true;
            this.button_import.Click += new System.EventHandler(this.button_import_Click);
            // 
            // button_moveDown
            // 
            this.button_moveDown.BackgroundImage = global::PlaylistEditor.Properties.Resources.arrow_down_bold_1_;
            this.button_moveDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_moveDown.FlatAppearance.BorderSize = 0;
            this.button_moveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_moveDown.Location = new System.Drawing.Point(233, 11);
            this.button_moveDown.Margin = new System.Windows.Forms.Padding(0);
            this.button_moveDown.Name = "button_moveDown";
            this.button_moveDown.Size = new System.Drawing.Size(30, 32);
            this.button_moveDown.TabIndex = 32;
            this.button_moveDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button_moveDown, "move row down");
            this.button_moveDown.UseVisualStyleBackColor = true;
            this.button_moveDown.Click += new System.EventHandler(this.button_moveDown_Click);
            // 
            // button_moveUp
            // 
            this.button_moveUp.BackgroundImage = global::PlaylistEditor.Properties.Resources.arrow_up_bold_1_;
            this.button_moveUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_moveUp.FlatAppearance.BorderSize = 0;
            this.button_moveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_moveUp.Location = new System.Drawing.Point(203, 9);
            this.button_moveUp.Margin = new System.Windows.Forms.Padding(0);
            this.button_moveUp.Name = "button_moveUp";
            this.button_moveUp.Size = new System.Drawing.Size(30, 32);
            this.button_moveUp.TabIndex = 31;
            this.button_moveUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button_moveUp, "move row up\r\n+ Ctrl move to top\r\nCtrl+T move to top");
            this.button_moveUp.UseVisualStyleBackColor = true;
            this.button_moveUp.Click += new System.EventHandler(this.button_moveUp_Click);
            // 
            // button_kodi
            // 
            this.button_kodi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_kodi.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_kodi.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_kodi.BackgroundImage")));
            this.button_kodi.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_kodi.FlatAppearance.BorderSize = 0;
            this.button_kodi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_kodi.Location = new System.Drawing.Point(820, 12);
            this.button_kodi.Margin = new System.Windows.Forms.Padding(0);
            this.button_kodi.Name = "button_kodi";
            this.button_kodi.Size = new System.Drawing.Size(38, 37);
            this.button_kodi.TabIndex = 65;
            this.toolTip1.SetToolTip(this.button_kodi, "play with Kodi\r\nCtrl+p\r\nCtrl + double click cell");
            this.button_kodi.UseVisualStyleBackColor = true;
            this.button_kodi.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox_find
            // 
            this.textBox_find.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_find.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.78182F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_find.Location = new System.Drawing.Point(934, 66);
            this.textBox_find.MaximumSize = new System.Drawing.Size(167, 28);
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
            this.button_settings.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_settings.BackgroundImage")));
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
            this.button_Info.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_Info.BackgroundImage")));
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
            this.plabel_Filename.Size = new System.Drawing.Size(385, 23);
            this.plabel_Filename.TabIndex = 26;
            this.plabel_Filename.Text = "pathLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(1122, 378);
            this.Controls.Add(this.button_kodi);
            this.Controls.Add(this.button_import);
            this.Controls.Add(this.RedoButton);
            this.Controls.Add(this.UndoButton);
            this.Controls.Add(this.button_check);
            this.Controls.Add(this.button_vlc);
            this.Controls.Add(this.button_revert);
            this.Controls.Add(this.button_dup);
            this.Controls.Add(this.textBox_find);
            this.Controls.Add(this.button_search);
            this.Controls.Add(this.button_moveDown);
            this.Controls.Add(this.button_moveUp);
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
        private RepeatingButton button_moveUp;
        private RepeatingButton button_moveDown;
        private System.Windows.Forms.Button button_search;
        private System.Windows.Forms.TextBox textBox_find;
        private System.Windows.Forms.Button button_dup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripPaste;
        private System.Windows.Forms.ToolStripMenuItem cutRowMenuItem;
        private System.Windows.Forms.Button button_revert;
        private System.Windows.Forms.Button button_vlc;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.Button button_check;
        private System.Windows.Forms.Button UndoButton;
        private System.Windows.Forms.Button RedoButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem singleToolStripMenuItem;
        private System.Windows.Forms.Button button_import;
        private System.Windows.Forms.Button button_kodi;
    }
}

