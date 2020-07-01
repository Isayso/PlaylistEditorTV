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
            this.pasteReplaceRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.cutRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripFill = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button_kodi = new PlaylistEditor.MyButton();
            this.button_import = new PlaylistEditor.MyButton();
            this.RedoButton = new PlaylistEditor.MyButton();
            this.UndoButton = new PlaylistEditor.MyButton();
            this.button_check = new PlaylistEditor.MyButton();
            this.button_vlc = new PlaylistEditor.MyButton();
            this.button_revert = new PlaylistEditor.MyButton();
            this.button_dup = new PlaylistEditor.MyButton();
            this.button_search = new PlaylistEditor.MyButton();
            this.button_moveDown = new RepeatingButton();
            this.button_moveUp = new RepeatingButton();
            this.button_del_all = new PlaylistEditor.MyButton();
            this.button_settings = new PlaylistEditor.MyButton();
            this.button_add = new PlaylistEditor.MyButton();
            this.button_Info = new PlaylistEditor.MyButton();
            this.button_delLine = new PlaylistEditor.MyButton();
            this.button_save = new PlaylistEditor.MyButton();
            this.button_open = new PlaylistEditor.MyButton();
            this.textBox_find = new System.Windows.Forms.TextBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addUseragentCell = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.editCellCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.editCellPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.editCellCut = new System.Windows.Forms.ToolStripMenuItem();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.plabel_Filename = new PathLabel();
            this.button_refind = new PlaylistEditor.MyButton();
            this.button_clearfind = new PlaylistEditor.MyButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
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
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
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
            this.dataGridView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView1_CellPainting);
            this.dataGridView1.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellValidated);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_ColumnHeaderMouseClick);
            this.dataGridView1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
            this.dataGridView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragDrop);
            this.dataGridView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragEnter);
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyRowMenuItem,
            this.pasteReplaceRowMenuItem,
            this.toolStripCopy,
            this.toolStripPaste,
            this.cutRowMenuItem,
            this.pasteRowMenuItem,
            this.toolStripSeparator2,
            this.playToolStripMenuItem,
            this.toolStripSeparator3,
            this.hideToolStripMenuItem,
            this.showToolStripMenuItem,
            this.toolStripSeparator4,
            this.toolStripFill});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(226, 262);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // copyRowMenuItem
            // 
            this.copyRowMenuItem.Enabled = false;
            this.copyRowMenuItem.Name = "copyRowMenuItem";
            this.copyRowMenuItem.ShortcutKeyDisplayString = "Ctrl+R";
            this.copyRowMenuItem.Size = new System.Drawing.Size(225, 24);
            this.copyRowMenuItem.Text = "Copy Row";
            this.copyRowMenuItem.Visible = false;
            this.copyRowMenuItem.Click += new System.EventHandler(this.toolStripFill_Click);
            // 
            // pasteReplaceRowMenuItem
            // 
            this.pasteReplaceRowMenuItem.Enabled = false;
            this.pasteReplaceRowMenuItem.Name = "pasteReplaceRowMenuItem";
            this.pasteReplaceRowMenuItem.ShortcutKeyDisplayString = "Ctrl+V";
            this.pasteReplaceRowMenuItem.Size = new System.Drawing.Size(225, 24);
            this.pasteReplaceRowMenuItem.Text = "Paste Row";
            this.pasteReplaceRowMenuItem.Visible = false;
            this.pasteReplaceRowMenuItem.Click += new System.EventHandler(this.pasteReplaceRowMenuItem_Click);
            // 
            // toolStripCopy
            // 
            this.toolStripCopy.Name = "toolStripCopy";
            this.toolStripCopy.ShortcutKeyDisplayString = "Ctrl+C";
            this.toolStripCopy.Size = new System.Drawing.Size(225, 24);
            this.toolStripCopy.Text = "Copy";
            this.toolStripCopy.Click += new System.EventHandler(this.toolStripCopy_Click);
            // 
            // toolStripPaste
            // 
            this.toolStripPaste.Enabled = false;
            this.toolStripPaste.Name = "toolStripPaste";
            this.toolStripPaste.ShortcutKeyDisplayString = "Ctrl+V";
            this.toolStripPaste.Size = new System.Drawing.Size(225, 24);
            this.toolStripPaste.Text = "Paste";
            this.toolStripPaste.Click += new System.EventHandler(this.toolStripPaste_Click);
            // 
            // cutRowMenuItem
            // 
            this.cutRowMenuItem.Enabled = false;
            this.cutRowMenuItem.Name = "cutRowMenuItem";
            this.cutRowMenuItem.ShortcutKeyDisplayString = "Ctrl+X";
            this.cutRowMenuItem.Size = new System.Drawing.Size(225, 24);
            this.cutRowMenuItem.Text = "Cut Row";
            this.cutRowMenuItem.Click += new System.EventHandler(this.cutRowMenuItem_Click);
            // 
            // pasteRowMenuItem
            // 
            this.pasteRowMenuItem.Enabled = false;
            this.pasteRowMenuItem.Name = "pasteRowMenuItem";
            this.pasteRowMenuItem.ShortcutKeyDisplayString = "Ctrl+I";
            this.pasteRowMenuItem.Size = new System.Drawing.Size(225, 24);
            this.pasteRowMenuItem.Text = "Paste Insert Row";
            this.pasteRowMenuItem.Click += new System.EventHandler(this.pasteRowMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(222, 6);
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+P";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(225, 24);
            this.playToolStripMenuItem.Text = "Kodi play";
            this.playToolStripMenuItem.ToolTipText = "Ctrl double click on cell";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(222, 6);
            // 
            // hideToolStripMenuItem
            // 
            this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            this.hideToolStripMenuItem.Size = new System.Drawing.Size(225, 24);
            this.hideToolStripMenuItem.Text = "Hide column";
            this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(225, 24);
            this.showToolStripMenuItem.Text = "Show all columns";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(222, 6);
            // 
            // toolStripFill
            // 
            this.toolStripFill.Enabled = false;
            this.toolStripFill.Name = "toolStripFill";
            this.toolStripFill.Size = new System.Drawing.Size(225, 24);
            this.toolStripFill.Text = "Fill cells from Clipboard";
            this.toolStripFill.Click += new System.EventHandler(this.toolStripFill_Click);
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
            // button_kodi
            // 
            this.button_kodi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_kodi.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_kodi.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_kodi.BackgroundImage")));
            this.button_kodi.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_kodi.FlatAppearance.BorderSize = 0;
            this.button_kodi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_kodi.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_kodi.Location = new System.Drawing.Point(820, 12);
            this.button_kodi.Margin = new System.Windows.Forms.Padding(0);
            this.button_kodi.Name = "button_kodi";
            this.button_kodi.Size = new System.Drawing.Size(38, 37);
            this.button_kodi.TabIndex = 65;
            this.toolTip1.SetToolTip(this.button_kodi, "play with Kodi\r\nCtrl+p\r\nCtrl + double click cell");
            this.button_kodi.UseVisualStyleBackColor = true;
            this.button_kodi.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_import
            // 
            this.button_import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_import.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_import.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_import.BackgroundImage")));
            this.button_import.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_import.FlatAppearance.BorderSize = 0;
            this.button_import.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_import.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_import.Location = new System.Drawing.Point(889, 14);
            this.button_import.Margin = new System.Windows.Forms.Padding(0);
            this.button_import.Name = "button_import";
            this.button_import.Size = new System.Drawing.Size(25, 37);
            this.button_import.TabIndex = 64;
            this.toolTip1.SetToolTip(this.button_import, "Import full list from clipboard");
            this.button_import.UseVisualStyleBackColor = true;
            this.button_import.Click += new System.EventHandler(this.button_import_Click);
            // 
            // RedoButton
            // 
            this.RedoButton.BackColor = System.Drawing.Color.MidnightBlue;
            this.RedoButton.BackgroundImage = global::PlaylistEditor.Properties.Resources.redo;
            this.RedoButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.RedoButton.FlatAppearance.BorderSize = 0;
            this.RedoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RedoButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RedoButton.Location = new System.Drawing.Point(118, 30);
            this.RedoButton.Margin = new System.Windows.Forms.Padding(0);
            this.RedoButton.Name = "RedoButton";
            this.RedoButton.Size = new System.Drawing.Size(33, 19);
            this.RedoButton.TabIndex = 63;
            this.toolTip1.SetToolTip(this.RedoButton, "redo");
            this.RedoButton.UseVisualStyleBackColor = true;
            this.RedoButton.Click += new System.EventHandler(this.RedoButton_Click);
            // 
            // UndoButton
            // 
            this.UndoButton.BackColor = System.Drawing.Color.MidnightBlue;
            this.UndoButton.BackgroundImage = global::PlaylistEditor.Properties.Resources.undo;
            this.UndoButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.UndoButton.FlatAppearance.BorderSize = 0;
            this.UndoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UndoButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.UndoButton.Location = new System.Drawing.Point(118, 6);
            this.UndoButton.Margin = new System.Windows.Forms.Padding(0);
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(33, 19);
            this.UndoButton.TabIndex = 62;
            this.toolTip1.SetToolTip(this.UndoButton, "undo");
            this.UndoButton.UseVisualStyleBackColor = true;
            this.UndoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // button_check
            // 
            this.button_check.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_check.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_check.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_check.BackgroundImage")));
            this.button_check.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_check.FlatAppearance.BorderSize = 0;
            this.button_check.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_check.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_check.Location = new System.Drawing.Point(931, 14);
            this.button_check.Margin = new System.Windows.Forms.Padding(0);
            this.button_check.Name = "button_check";
            this.button_check.Size = new System.Drawing.Size(25, 37);
            this.button_check.TabIndex = 38;
            this.toolTip1.SetToolTip(this.button_check, "check for invalid links\r\n+ ctrl select all links\r\n+ shift-ctrl select orange link" +
        "s");
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
            this.button_vlc.ImeMode = System.Windows.Forms.ImeMode.NoControl;
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
            this.button_revert.ImeMode = System.Windows.Forms.ImeMode.NoControl;
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
            this.button_dup.ImeMode = System.Windows.Forms.ImeMode.NoControl;
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
            this.button_search.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_search.Location = new System.Drawing.Point(1009, 14);
            this.button_search.Margin = new System.Windows.Forms.Padding(0);
            this.button_search.Name = "button_search";
            this.button_search.Size = new System.Drawing.Size(25, 37);
            this.button_search.TabIndex = 33;
            this.toolTip1.SetToolTip(this.button_search, "search\r\nCtrl+F");
            this.button_search.UseVisualStyleBackColor = true;
            this.button_search.Click += new System.EventHandler(this.button_search_Click);
            // 
            // button_moveDown
            // 
            this.button_moveDown.BackgroundImage = global::PlaylistEditor.Properties.Resources.arrow_down_bold_1_;
            this.button_moveDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_moveDown.FlatAppearance.BorderSize = 0;
            this.button_moveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_moveDown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_moveDown.Location = new System.Drawing.Point(233, 11);
            this.button_moveDown.Margin = new System.Windows.Forms.Padding(0);
            this.button_moveDown.Name = "button_moveDown";
            this.button_moveDown.Size = new System.Drawing.Size(30, 32);
            this.button_moveDown.TabIndex = 32;
            this.button_moveDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button_moveDown, "move row down\r\nCtrl-2\r\nCtrl-click move to bottom\r\nCtrl-B move to bottom");
            this.button_moveDown.UseVisualStyleBackColor = true;
            this.button_moveDown.Click += new System.EventHandler(this.button_moveDown_Click);
            // 
            // button_moveUp
            // 
            this.button_moveUp.BackgroundImage = global::PlaylistEditor.Properties.Resources.arrow_up_bold_1_;
            this.button_moveUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_moveUp.FlatAppearance.BorderSize = 0;
            this.button_moveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_moveUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_moveUp.Location = new System.Drawing.Point(203, 9);
            this.button_moveUp.Margin = new System.Windows.Forms.Padding(0);
            this.button_moveUp.Name = "button_moveUp";
            this.button_moveUp.Size = new System.Drawing.Size(30, 32);
            this.button_moveUp.TabIndex = 31;
            this.button_moveUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button_moveUp, "move row up\r\nCtrl-1\r\nCtrl-click move to top\r\nCtrl+T move to top");
            this.button_moveUp.UseVisualStyleBackColor = true;
            this.button_moveUp.Click += new System.EventHandler(this.button_moveUp_Click);
            // 
            // button_del_all
            // 
            this.button_del_all.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_del_all.BackgroundImage")));
            this.button_del_all.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_del_all.FlatAppearance.BorderSize = 0;
            this.button_del_all.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_del_all.ImeMode = System.Windows.Forms.ImeMode.NoControl;
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
            // button_settings
            // 
            this.button_settings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_settings.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_settings.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_settings.BackgroundImage")));
            this.button_settings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_settings.FlatAppearance.BorderSize = 0;
            this.button_settings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_settings.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_settings.Location = new System.Drawing.Point(1049, 15);
            this.button_settings.Margin = new System.Windows.Forms.Padding(0);
            this.button_settings.Name = "button_settings";
            this.button_settings.Size = new System.Drawing.Size(25, 37);
            this.button_settings.TabIndex = 28;
            this.toolTip1.SetToolTip(this.button_settings, "Settings");
            this.button_settings.UseVisualStyleBackColor = true;
            this.button_settings.Click += new System.EventHandler(this.button_settings_Click);
            // 
            // button_add
            // 
            this.button_add.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_add.BackgroundImage")));
            this.button_add.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_add.FlatAppearance.BorderSize = 0;
            this.button_add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_add.ImeMode = System.Windows.Forms.ImeMode.NoControl;
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
            // button_Info
            // 
            this.button_Info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Info.BackColor = System.Drawing.Color.MidnightBlue;
            this.button_Info.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_Info.BackgroundImage")));
            this.button_Info.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_Info.FlatAppearance.BorderSize = 0;
            this.button_Info.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Info.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_Info.Location = new System.Drawing.Point(1087, 15);
            this.button_Info.Margin = new System.Windows.Forms.Padding(0);
            this.button_Info.Name = "button_Info";
            this.button_Info.Size = new System.Drawing.Size(25, 37);
            this.button_Info.TabIndex = 24;
            this.toolTip1.SetToolTip(this.button_Info, "info");
            this.button_Info.UseVisualStyleBackColor = true;
            this.button_Info.Click += new System.EventHandler(this.button_Info_Click);
            // 
            // button_delLine
            // 
            this.button_delLine.BackgroundImage = global::PlaylistEditor.Properties.Resources.close_box_outline_1_;
            this.button_delLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_delLine.FlatAppearance.BorderSize = 0;
            this.button_delLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_delLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_delLine.Location = new System.Drawing.Point(166, 11);
            this.button_delLine.Margin = new System.Windows.Forms.Padding(2);
            this.button_delLine.Name = "button_delLine";
            this.button_delLine.Size = new System.Drawing.Size(30, 32);
            this.button_delLine.TabIndex = 2;
            this.button_delLine.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button_delLine, "delete row or cells");
            this.button_delLine.UseVisualStyleBackColor = true;
            this.button_delLine.Click += new System.EventHandler(this.button_delLine_Click);
            // 
            // button_save
            // 
            this.button_save.BackgroundImage = global::PlaylistEditor.Properties.Resources.content_save_1_;
            this.button_save.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_save.FlatAppearance.BorderSize = 0;
            this.button_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_save.ImeMode = System.Windows.Forms.ImeMode.NoControl;
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
            this.button_open.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_open.Location = new System.Drawing.Point(9, 2);
            this.button_open.Margin = new System.Windows.Forms.Padding(2);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(45, 49);
            this.button_open.TabIndex = 0;
            this.button_open.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.button_open, "open m3u\r\nCtrl+N open new window\r\n\r\n");
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // textBox_find
            // 
            this.textBox_find.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_find.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox_find.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.74545F);
            this.textBox_find.Location = new System.Drawing.Point(740, 64);
            this.textBox_find.MaximumSize = new System.Drawing.Size(400, 300);
            this.textBox_find.MaxLength = 16;
            this.textBox_find.Name = "textBox_find";
            this.textBox_find.Size = new System.Drawing.Size(359, 31);
            this.textBox_find.TabIndex = 34;
            this.textBox_find.Visible = false;
            this.textBox_find.TextChanged += new System.EventHandler(this.textBox_find_TextChange);
            this.textBox_find.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_find_KeyPress);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addUseragentCell,
            this.toolStripSeparator1,
            this.editCellCopy,
            this.editCellPaste,
            this.editCellCut});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(173, 106);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
            // 
            // addUseragentCell
            // 
            this.addUseragentCell.Name = "addUseragentCell";
            this.addUseragentCell.ShortcutKeyDisplayString = "";
            this.addUseragentCell.Size = new System.Drawing.Size(172, 24);
            this.addUseragentCell.Text = "add user-agent";
            this.addUseragentCell.Click += new System.EventHandler(this.addUseragentCell_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // editCellCopy
            // 
            this.editCellCopy.Name = "editCellCopy";
            this.editCellCopy.ShortcutKeyDisplayString = "Ctrl-C";
            this.editCellCopy.Size = new System.Drawing.Size(172, 24);
            this.editCellCopy.Text = "Copy";
            this.editCellCopy.Click += new System.EventHandler(this.editCellCopy_Click);
            // 
            // editCellPaste
            // 
            this.editCellPaste.Name = "editCellPaste";
            this.editCellPaste.ShortcutKeyDisplayString = "Ctrl-V";
            this.editCellPaste.Size = new System.Drawing.Size(172, 24);
            this.editCellPaste.Text = "Paste";
            this.editCellPaste.Click += new System.EventHandler(this.editCellPaste_Click);
            // 
            // editCellCut
            // 
            this.editCellCut.Name = "editCellCut";
            this.editCellCut.ShortcutKeyDisplayString = "Ctrl-X";
            this.editCellCut.Size = new System.Drawing.Size(172, 24);
            this.editCellCut.Text = "Cut";
            this.editCellCut.Click += new System.EventHandler(this.editCellCut_Click);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Gray;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Italic);
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(454, 169);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(257, 60);
            this.label6.TabIndex = 67;
            this.label6.Text = "Double Click to open file\r\nDrag \'n Drop file to open or append\r\nCtrl-N to open ne" +
    "w window";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Italic);
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(1054, 68);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 23);
            this.label1.TabIndex = 68;
            this.label1.Text = "Row";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Visible = false;
            this.label1.Click += new System.EventHandler(this.label_click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Italic);
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(992, 68);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(60, 23);
            this.label2.TabIndex = 69;
            this.label2.Text = "Name2";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Visible = false;
            this.label2.Click += new System.EventHandler(this.label_click);
            // 
            // plabel_Filename
            // 
            this.plabel_Filename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plabel_Filename.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.78182F);
            this.plabel_Filename.ForeColor = System.Drawing.SystemColors.Control;
            this.plabel_Filename.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.plabel_Filename.Location = new System.Drawing.Point(405, 16);
            this.plabel_Filename.Name = "plabel_Filename";
            this.plabel_Filename.Size = new System.Drawing.Size(355, 23);
            this.plabel_Filename.TabIndex = 26;
            this.plabel_Filename.Text = "pathLabel1";
            // 
            // button_refind
            // 
            this.button_refind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_refind.BackColor = System.Drawing.Color.White;
            this.button_refind.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_refind.BackgroundImage")));
            this.button_refind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_refind.FlatAppearance.BorderSize = 0;
            this.button_refind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_refind.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.74545F);
            this.button_refind.ForeColor = System.Drawing.SystemColors.Control;
            this.button_refind.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_refind.Location = new System.Drawing.Point(967, 66);
            this.button_refind.Margin = new System.Windows.Forms.Padding(2);
            this.button_refind.Name = "button_refind";
            this.button_refind.Size = new System.Drawing.Size(23, 27);
            this.button_refind.TabIndex = 70;
            this.button_refind.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_refind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_refind.UseVisualStyleBackColor = false;
            this.button_refind.Visible = false;
            this.button_refind.Click += new System.EventHandler(this.button_refind_Click);
            // 
            // button_clearfind
            // 
            this.button_clearfind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_clearfind.BackColor = System.Drawing.Color.White;
            this.button_clearfind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_clearfind.FlatAppearance.BorderSize = 0;
            this.button_clearfind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_clearfind.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.74545F);
            this.button_clearfind.ForeColor = System.Drawing.SystemColors.Control;
            this.button_clearfind.Image = ((System.Drawing.Image)(resources.GetObject("button_clearfind.Image")));
            this.button_clearfind.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button_clearfind.Location = new System.Drawing.Point(944, 66);
            this.button_clearfind.Margin = new System.Windows.Forms.Padding(2);
            this.button_clearfind.Name = "button_clearfind";
            this.button_clearfind.Size = new System.Drawing.Size(23, 27);
            this.button_clearfind.TabIndex = 66;
            this.button_clearfind.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_clearfind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button_clearfind.UseVisualStyleBackColor = false;
            this.button_clearfind.Visible = false;
            this.button_clearfind.Click += new System.EventHandler(this.button_clearfind_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(1122, 378);
            this.Controls.Add(this.button_refind);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button_clearfind);
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
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PlaylistEditor.MyButton button_open;
        private PlaylistEditor.MyButton button_save;
        private PlaylistEditor.MyButton button_delLine;
        private PlaylistEditor.MyButton button_Info;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
        private PathLabel plabel_Filename;
        private PlaylistEditor.MyButton button_add;
        private PlaylistEditor.MyButton button_settings;
        private PlaylistEditor.MyButton button_del_all;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyRowMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteRowMenuItem;
        private RepeatingButton button_moveUp;
        private RepeatingButton button_moveDown;
        private PlaylistEditor.MyButton button_search;
        private System.Windows.Forms.TextBox textBox_find;
        private PlaylistEditor.MyButton button_dup;
        private System.Windows.Forms.ToolStripMenuItem toolStripCopy;
        private System.Windows.Forms.ToolStripMenuItem toolStripPaste;
        private System.Windows.Forms.ToolStripMenuItem cutRowMenuItem;
        private PlaylistEditor.MyButton button_revert;
        private PlaylistEditor.MyButton button_vlc;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private PlaylistEditor.MyButton button_check;
        private PlaylistEditor.MyButton UndoButton;
        private PlaylistEditor.MyButton RedoButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private PlaylistEditor.MyButton button_import;
        private PlaylistEditor.MyButton button_kodi;
        private System.Windows.Forms.ToolStripMenuItem pasteReplaceRowMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem addUseragentCell;
        private System.Windows.Forms.ToolStripMenuItem editCellCopy;
        private System.Windows.Forms.ToolStripMenuItem editCellPaste;
        private System.Windows.Forms.ToolStripMenuItem editCellCut;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.DataGridView dataGridView1;
        private PlaylistEditor.MyButton button_clearfind;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolStripFill;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private MyButton button_refind;
    }
}

