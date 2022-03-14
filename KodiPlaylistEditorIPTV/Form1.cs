//  MIT License
//  Copyright (c) 2018 github/isayso
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
//  files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy,
//  modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
//  subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using PlaylistEditor.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PlaylistEditor.ClassHelp;

namespace PlaylistEditor
{
    public partial class Form1 : Form
    {
        private Stack<object[][]> undoStack = new Stack<object[][]>();
        private Stack<object[][]> redoStack = new Stack<object[][]>();

        private Boolean ignore = false;
        private CancellationTokenSource tokenSource;

        private player player;

        private bool isModified = false;

        private string fullRowContent = "",
                       _sort = "",
                       fileHeader = "#EXTM3U",  //for #EXTM3U tags
                       line;

        public string path;
        public static string myCulture;

        private bool _isIt = true,
                     _found = false,
                     _savenow = false,
                     _linkchecked = false,
                     _isSingle = false,
                     _controlpressed = false,
                     _ffprobefound = false,
                     _endofLoop = false; //loop of move to top finished


        private const int mActionHotKeyID = 1;  //var for key hook listener

        //zoom of fonts
        public float zoomf = 1F;

        private const float FONTSIZE = 9.163636F;

        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();
        private DataRow dr;
        private string vlcpath = Settings.Default.vlcpath,
                        ffpPath = null;

        private string[] linktypes = new[] { "ht", "plugin", "rt", "ud", "mm" }; //Types of links in Column "Link"

        public List<string> columnNames = new List<string>();

        public List<string> cNameArr = new List<string>();  //search box


        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;

        public Form1()
        {
            myCulture = Settings.Default.localize;
            if (string.IsNullOrEmpty(myCulture)) myCulture = "en-US";

            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo(myCulture);
            Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(myCulture);

            InitializeComponent();

            this.Text = String.Format("PlaylistEditor TV " + " v{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 5));

#if DEBUG
            //  Clipboard.Clear();
            this.Text = String.Format("PlaylistEditor TV DEBUG" + " v{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 7));
#endif

            if (Settings.Default.UpgradeRequired)
            {
                //Settings.Default.Reset();
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();
            }

            var spec_key = Settings.Default.specKey;  //for key listener
            var hotlabel = Settings.Default.hotkey;

            //Modifier keys codes: Alt = 1, Ctrl = 2, Shift = 4, Win = 8  must be added
            //   RegisterHotKey(this.Handle, mActionHotKeyID, 1, (int)Keys.Y);  //ALT-Y
            if (Settings.Default.hotkey_enable)
                NativeMethods.RegisterHotKey(this.Handle, mActionHotKeyID, spec_key, hotlabel);  //ALT-Y

            // check for external progs
            //check for vlc.exe
            if (!MyFileExists(vlcpath + "\\" + "vlc.exe", 5000))  // vlcpath + "\\" + "vlc.exe";
            {
                vlcpath = GetVlcPath();
            }

#if DEBUG
            // _ffprobefound = ClassHelp.CheckForFfprobe();
            ffpPath = ClassHelp.GetFfprobePath();
            _ffprobefound = !string.IsNullOrEmpty(ffpPath);
#else     
            _ffprobefound = false; //for future versions
#endif

            plabel_Filename.Text = "";
            button_revert.Visible = false;

            //  dataGridView1.AllowUserToAddRows = true;

            dataGridView1.ShowCellToolTips = false;
            dataGridView1.DoubleBuffered(true);
            // dataGridView1.BringToFront();
            //    dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;

            DataGridStyle();

            // context menu 3 options
            cm3Scrollbar.Checked = Settings.Default.scrollbar;
            cm3EditF2.Checked = Settings.Default.F2_edit;

            if (Settings.Default.scrollbar)
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            else
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (Settings.Default.F2_edit)
                dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;

            //command line arguments [1]
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1) //drag drop
            {
                plabel_Filename.Text = args[1];
                ImportDataset(args[1], false);
                button_revert.Visible = true;
            }

            //TODO col number
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Settings.Default.filestart && !Settings.Default.nostart)  //nostart for ctrl-N
            {
                plabel_Filename.Text = Settings.Default.startfile;
                //check if path exist
                if (MyFileExists(plabel_Filename.Text, 5000))
                {
                    ImportDataset(plabel_Filename.Text, false);
                    button_revert.Visible = true;

                    if (Settings.Default.autoplayer)
                    {
                        button_vlc.PerformClick();
                    }
                }
            }

            if (Settings.Default.F2Size.Width == 0 || Settings.Default.F2Size.Height == 0
                || Settings.Default.nostart)
            {
                // first start
                this.Size = new Size(1140, 422);
            }
            else
            {
                if (Settings.Default.ZoomFactor != 0) ZoomGrid(Settings.Default.ZoomFactor);
                this.Location = Settings.Default.F2Location;
                this.Size = Settings.Default.F2Size;
            }

            if (dataGridView1.RowCount == 0) dataGridView1.ContextMenuStrip = contextMenuStrip1;

            Settings.Default.nostart = false;
            Settings.Default.Save();
        }

        /// <summary>
        /// listener to hotkey for import of links from clipboard
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == mActionHotKeyID)
            {
                NotificationBox.Show("List import.....", 1500, NotificationMsg.DONE);

                button_import.PerformClick();
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// set contextmenu to all column headers
        /// </summary>
        private void SetHeaderContextMenu()
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.ContextMenuStrip = contextMenuStrip3;
                // column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Modifiers == Keys.Control)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.B:
                            MoveLineBottom();
                            break;

                        case Keys.C:
                            if (dataGridView1.SelectedRows.Count > 0)  //Full row
                            {
                                contextMenuStrip1.Items["pasteRowMenuItem"].Enabled = true;
                                CopyFullRow();
                            }
                            else toolStripCopy.PerformClick();
                            break;

                        case Keys.V:
                            contextMenuStrip1.Items["toolStripPaste"].Enabled = true;
                            toolStripPaste.PerformClick();   //#35
                            break;

                        case Keys.I:
                            if (dataGridView1.SelectedRows.Count > 0 || dataGridView1.Rows.Count == 0
                                || (string.IsNullOrEmpty(fullRowContent) && CheckClipboard()))
                                contextMenuStrip1.Items["pasteRowMenuItem"].Enabled = true;  //paste add

                            pasteRowMenuItem.PerformClick();
                            break;

                        case Keys.X:
                            if (dataGridView1.SelectedRows.Count > 0)
                            {
                                contextMenuStrip1.Items["cutRowMenuItem"].Enabled = true;
                                cutRowMenuItem.PerformClick();
                            }
                            break;

                        case Keys.Z:
                            UndoButton.PerformClick();
                            break;

                        case Keys.T:  //move line to top
                            MoveLineTop();
                            break;

                        case Keys.F:
                            button_search.PerformClick();
                            break;

                        case Keys.N:
                            Settings.Default.nostart = true;
                            Settings.Default.Save();
                            var deffile = new ProcessStartInfo(Application.ExecutablePath);
                            Process.Start(deffile);
                            break;

                        case Keys.P:
                            playToolStripMenuItem.PerformClick();
                            break;

                        case Keys.S:
                            _savenow = true;
                            button_save.PerformClick();
                            break;

                        case Keys.Add:    //change font size
                            zoomf += 0.1F;
                            ZoomGrid(zoomf);
                            break;

                        case Keys.Oemplus:      //change font size
                            zoomf += 0.1F;
                            ZoomGrid(zoomf);
                            break;

                        case Keys.Subtract:    //change font size
                            zoomf -= 0.1F;
                            ZoomGrid(zoomf);
                            break;

                        case Keys.OemMinus:     //change font size
                            zoomf -= 0.1F;
                            ZoomGrid(zoomf);
                            break;

                        case Keys.D1:
                            MoveLine(-1);
                            break;

                        case Keys.D2:
                            MoveLine(1);
                            break;
                    }
                }
                if (e.KeyCode == Keys.Delete && dataGridView1.IsCurrentCellInEditMode == false)
                {
                    button_delLine.PerformClick();
                }
                if (e.KeyCode == Keys.F2)
                {
                    _endofLoop = true;
                    dataGridView1.BeginEdit(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Key press operation failed. " + ex.Message, "Key press", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        /// <summary>
        /// change font size of datagrid
        /// </summary>
        /// <param name="f">change factor float</param>
        public void ZoomGrid(float f)
        {
            dataGridView1.Font = new Font(dataGridView1.Font.FontFamily,
                                         FONTSIZE * f, dataGridView1.Font.Style);

            Settings.Default.ZoomFactor = f;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isModified == true && dataGridView1.RowCount > 0)
            {
                DialogResult dialogSave = MessageBox.Show(Mess.Do_you_want_to_save_your_current_playlist, Mess.Save_Playlist,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogSave == DialogResult.Yes)
                {
                    button_save.PerformClick();
                    isModified = false;
                }
                if (dialogSave == DialogResult.Cancel) e.Cancel = true;
            }

            NativeMethods.UnregisterHotKey(this.Handle, mActionHotKeyID);

            //Settings.Default.F2Location = this.Location;
            //Settings.Default.F2Size = this.Size;

            if (this.WindowState == FormWindowState.Normal)
            {
                // save location and size if the state is normal
                Properties.Settings.Default.F2Location = this.Location;
                Properties.Settings.Default.F2Size = this.Size;
            }
            else
            {
                // save the RestoreBounds if the form is minimized or maximized!
                Properties.Settings.Default.F2Location = this.RestoreBounds.Location;
                Properties.Settings.Default.F2Size = this.RestoreBounds.Size;
            }

            Settings.Default.Save();
        }

        #region menu buttons

        private void button_search_Click(object sender, EventArgs e)
        {
            cNameArr.Clear();

            foreach (DataColumn c in dt.Columns)
            {
                cNameArr.Add(c.ColumnName);
            }

            cNameArr.Add("All");

            textBox_find.BringToFront();

            if (Settings.Default.findresult == 0) lblRowCheck.Text = "Row";
            else lblRowCheck.Text = "Cell";

            lblColCheck.Text = cNameArr[cNameArr.Count - 1];

            if (_isIt)
            {
                _isIt = !_isIt;
                textBox_find.Visible = true;
                button_clearfind.Visible = true; lblRowCheck.Visible = true; lblColCheck.Visible = true;
                this.ActiveControl = textBox_find;
                button_clearfind.BringToFront(); lblRowCheck.BringToFront(); lblColCheck.BringToFront();
                button_refind.BringToFront(); button_refind.Visible = true;
            }
            else  //close textbox_find
            {
                _isIt = !_isIt;
                textBox_find.Visible = false;
                button_clearfind.Visible = false; lblRowCheck.Visible = false; lblColCheck.Visible = false;
                button_refind.Visible = false;

            }
        }

        private void label_click(object sender, EventArgs e)
        {
            int playswitch = Settings.Default.findresult;
            int colswitch = Settings.Default.colSearch;  //TODO
            //int colswitch = 1;  //TODO

            Label obj = sender as Label;

            if (obj.Name == "lblRowCheck")
            {
                switch (playswitch)
                {
                    case 0:
                        lblRowCheck.Text = "Cell";
                        playswitch = 1;
                        break;

                    case 1:
                        lblRowCheck.Text = "Row";
                        playswitch = 0;
                        break;
                }
                Settings.Default.findresult = playswitch;

                textBox_find_TextChange(sender, e);
            }

            if (obj.Name == "lblColCheck")
            {
                colswitch++; if (colswitch >= cNameArr.Count) colswitch = 0;

                lblColCheck.Text = cNameArr[colswitch];
                Settings.Default.colSearch = colswitch;

                textBox_find_TextChange(sender, e);
            }
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            if (_linkchecked) button_check.PerformClick();

            if (isModified == true && dataGridView1.RowCount > 0)
            {
                DialogResult dialogSave = MessageBox.Show(Mess.Do_you_want_to_save_your_current_playlist,
                Mess.Save_Playlist, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogSave == DialogResult.Yes)
                {
                    button_save.PerformClick();
                    isModified = false;
                }
                if (dialogSave == DialogResult.Cancel) return;
            }

            Cursor.Current = Cursors.WaitCursor;

            string openpath = Settings.Default.openpath;
            if (!string.IsNullOrEmpty(openpath) && !MyDirectoryExists(openpath, 4000))
                openpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.InitialDirectory = openpath;
                openFileDialog1.RestoreDirectory = false;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //undoStack.Clear(); redoStack.Clear(); toSave(false); ShowReUnDo(0);//reset stacks
                    toSave(false, true);
                    ImportDataset(openFileDialog1.FileName, false);
                    button_revert.Visible = true;
                }
                else  //cancel
                {
                    return;
                }

                Settings.Default.openpath = Path.GetDirectoryName(openFileDialog1.FileName);
                Settings.Default.Save();
            }

            fillPlayer(); //send list to player

            Cursor.Current = Cursors.Default;
        }

        private void button_Info_Click(object sender, EventArgs e)
        {
            using (AboutBox1 a = new AboutBox1())
            {
                a.ShowDialog();   //  ShowDialog gets focus, Show not
                                  //centre position on Infoform
            }
        }

        private void button_settings_Click(object sender, EventArgs e)
        {

            using (settings s = new settings(columnNames))
            {
                s.ShowDialog();

                if (Settings.Default.findresult == 0) lblRowCheck.Text = "Row";
                else lblRowCheck.Text = "Cell";

            }

            //scrollbar change
            cm3Scrollbar.Checked = Settings.Default.scrollbar;

            cm3EditF2.Checked = Settings.Default.F2_edit;

        }

        /// <summary>
        /// import of playlist entries
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="append">false/true for append</param>

        public void ImportDataset(string filename, bool append)
        {
            if (filename == null) return;

            StreamReader playlistFile = new StreamReader(filename, Encoding.UTF8);
            line = playlistFile.ReadLine();  //first line

            if (line.StartsWith("#EXTM3U"))
            {
                fileHeader = line;
            }
            else if (line.StartsWith("#EXTCPlayListM3U::M3U"))
            {
                MessageBox.Show(Mess.File_has_wrong_format_or_does_not_exist_);
                return;

            }

            if (!append)  //append false 
            {
                dt.Clear();  // row clear
                dt.Columns.Clear();  // col clear

                plabel_Filename.Text = filename;
            }


            dt.TableName = "IPTV";

            string fullTxt = playlistFile.ReadToEnd();  //read rest of file
            string[] fileRows = fullTxt.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            List<ColList> elements = new List<ColList>();
            elements = ClassHelp.SeekFileElements(fullTxt);
            CreateDataTable(elements);


            //   RowData rowData = new RowData();

            dataGridView1.DataSource = dt;

            ///Drag n Drop file

            Cursor.Current = Cursors.WaitCursor;


            for (int i = 0; i < fileRows.Length; i++)
            {

                if (fileRows[i].StartsWith("#EXTINF"))
                {
                    dr = dt.NewRow();

                    for (int j = 0; j < dt.Columns.Count - 2; j++)
                    {
                        string header = dt.Columns[j].ToString();
                        var match = Regex.Match(fileRows[i], header + "=\"([^\"]*)\"").Groups[1];

                        if (match.Success)
                        {
                            string udpIP = match.Captures[0].Value;
                            dr[header] = udpIP;
                            continue;
                        }

                    }
                    dr["Name2"] = fileRows[i].Split(',').Last().Trim();

                    continue;
                }
                else if ((linktypes.Any(fileRows[i].StartsWith))
                    && (fileRows[i].Contains("//") || fileRows[i].Contains(":\\")))//issue #32 issue #61
                {

                    dr["Link"] = fileRows[i];

                    try { dt.Rows.Add(dr); }
                    catch { continue; }
                }
            }


            playlistFile.Close();
            playlistFile.Dispose();


            dataGridView1.AllowUserToAddRows = false;

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show(Mess.Wrong_data_structure, Mess.File_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dataGridView1.Rows[0].Selected = true;
            //checkList.Clear(); //to reset Repaint

            label_central.SendToBack();

            Cursor.Current = Cursors.Default;
        }



        private void button_delLine_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.InvSelectedRows())
                {
                    dt.Rows.RemoveAt(row.Index);
                }
                toSave(true);
            }
            else  //delete cells only
            {
                Int32 selectedCellCount = dataGridView1.GetCellCount(DataGridViewElementStates.Selected);
                if (selectedCellCount > 0)
                {
                    for (int i = 0; i < selectedCellCount; i++)
                    {
                        dataGridView1.Rows[dataGridView1.SelectedCells[i].RowIndex]
                            .Cells[dataGridView1.SelectedCells[i].ColumnIndex].Value = string.Empty;
                    }
                }

                toSave(true);
            }
        }


        private void button_save_Click(object sender, EventArgs e)
        {
            bool _shift = false;

            if (ModifierKeys == Keys.Shift) _shift = true;

            Cursor.Current = Cursors.WaitCursor;

            saveFileDialog1.FileName = plabel_Filename.Text;


            if (TestHiddenColumns())
            {
                DialogResult dialogHidden = MessageBox.Show(Mess.Hidden_Columns_will_not_be_saved, Mess.Proceed,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogHidden == DialogResult.Yes)
                {
                    button_save.PerformClick();
                    isModified = false;
                }
                if (dialogHidden == DialogResult.No) return;

            }


            if ((_shift || _savenow) && !string.IsNullOrEmpty(plabel_Filename.Text)
                && MyDirectoryExists(Path.GetDirectoryName(plabel_Filename.Text), 4000))
            {
                saveFileDialog1.FileName = plabel_Filename.Text;

                using (StreamWriter file = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8))   //false: file ovewrite
                {
                    file.NewLine = "\n";  // win: LF
                                          // file.WriteLine("#EXTM3U");
                    file.WriteLine(fileHeader);
                    string writestring = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {//issue #12  write only visible columns
                        writestring = "#EXTINF:-1 ";

                        for (int j = 0; j < dt.Columns.Count - 2; j++)
                        {
                            string header = dt.Columns[j].ToString();

                            if (dataGridView1.Columns[header].Visible)
                                writestring += " " + header + "=\"" + dt.Rows[i][j] + "\"";

                        }
                        writestring += "," + dt.Rows[i][dt.Columns.Count - 2];  //Name2

                        file.WriteLine(writestring);
                        file.WriteLine(dt.Rows[i][dt.Columns.Count - 1]);
                    }
                }
                //undoStack.Clear(); redoStack.Clear(); ShowReUnDo(0); toSave(false);
                toSave(false, true);
                button_revert.Visible = true;
                _savenow = false;

                NotificationBox.Show(this, Mess.Playlist_Saved, 1500, NotificationMsg.OK, Position.Parent);
            }
            else if (saveFileDialog1.ShowDialog() == DialogResult.OK)  //open file dialog
            {
                plabel_Filename.Text = saveFileDialog1.FileName;

                using (StreamWriter file = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8))   //false: file ovewrite
                {
                    file.NewLine = "\n";  // win: LF
                    file.WriteLine(fileHeader);
                    string writestring = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {  //issue #12  write only visible columns
                        writestring = "#EXTINF:-1 ";

                        for (int j = 0; j < dt.Columns.Count - 2; j++)
                        {
                            string header = dt.Columns[j].ToString();

                            if (dataGridView1.Columns[header].Visible)
                                writestring += header + "=\"" + dt.Rows[i][j] + "\"";

                        }
                        writestring += "," + dt.Rows[i][dt.Columns.Count - 2];  //Name2

                        file.WriteLine(writestring);
                        file.WriteLine(dt.Rows[i][dt.Columns.Count - 1]);
                    }
                }

                //undoStack.Clear(); redoStack.Clear(); ShowReUnDo(0); toSave(false);
                toSave(false, true);
                button_revert.Visible = true;
                Cursor.Current = Cursors.Default;
            }
        }

        private bool TestHiddenColumns()
        {
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                if (dataGridView1.Columns[dataGridView1.Columns[i].HeaderText].Visible == false) return true;
            }
            return false;
        }

        private void button_moveUp_Click(object sender, EventArgs e)
        {
            if ((ModifierKeys == Keys.Control))
            {
                MoveLineTop();
            }
            else
            {
                MoveLine(-1);
            }
        }

        private void button_moveDown_Click(object sender, EventArgs e)
        {
            if ((ModifierKeys == Keys.Control))
            {
                MoveLineBottom();
            }
            else
            {
                MoveLine(1);
            }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            DataRow dr = dt.NewRow();

            if (dataGridView1.RowCount == 0 && dataGridView1.ColumnCount > 0) //delete all
            {
                dt.Clear();
                dt.Columns.Clear();
                toSave(false);
                plabel_Filename.Text = "";
                button_revert.Visible = false;
            }

            if (dataGridView1.RowCount > 0)
            {
                if (dataGridView1.SelectedRows.Count == 0) return;

                int a = dataGridView1.SelectedCells[0].RowIndex;  // row index in a datatable

                DataRow newBlankRow = dt.NewRow();
                dt.Rows.InsertAt(newBlankRow, a);

            }
            else  //Grid empty
            {
                dt.TableName = "IPTV";

                dt.Columns.Add("group-title"); dt.Columns.Add("tvg-logo");
                dt.Columns.Add("Name2"); dt.Columns.Add("Link");
                dt.Rows.InsertAt(dr, 0);

                dataGridView1.DataSource = dt;
                dataGridView1.AllowUserToAddRows = false;

                SetHeaderContextMenu();
            }
            label_central.SendToBack();

            toSave(true);
        }

        private void button_vlc_Click(object sender, EventArgs e)
        {

            string vlclink = dataGridView1.CurrentRow.Cells["Link"].Value.ToString();
            //   if (!vlclink.StartsWith("http") && !vlclink.StartsWith("rtmp")) return; //issue #32

            if (string.IsNullOrEmpty(vlcpath))
            {
                vlcpath = GetVlcPath();
                if (string.IsNullOrEmpty(vlcpath))
                    NotificationBox.Show(this, Mess.VLC_player_not_found, 3000, NotificationMsg.ERROR, Position.Parent);

                return;
            }
            else if (dataGridView1.RowCount > 0 && vlclink.StartsWith("plugin"))
            {
                NotificationBox.Show(this, Mess.Plugin_links_only_work_in_Kodi, 3000, NotificationMsg.ERROR, Position.Parent);

                return;  //#18
            }
            else if (dataGridView1.RowCount > 0 && vlclink.StartsWith("rtmp"))
            {
                NotificationBox.Show(this, Mess.Plugin_links_only_work_in_Kodi, 3000, NotificationMsg.ERROR, Position.Parent);

                return;  //#61
            }
            else if (dataGridView1.RowCount > 0 && vlclink.Contains("|User"))
            {
                NotificationBox.Show(this, Mess.User_Agent_links_only_work_in_Kodi, 3000, NotificationMsg.ERROR, Position.Parent);

                return;  //#18
            }


            if (player == null)
            {
                CreatePlayer();
            }
            else
            {
                fillPlayer();
            }

            if (dataGridView1.RowCount > 0)
            {
                player.comboBox1.SelectedIndex = dataGridView1.CurrentRow.Index;  //trigger eventHandler
            }
        }

        private void PlayOnVlc()
        {
            if (dataGridView1.RowCount > 0 && !string.IsNullOrEmpty(vlcpath))
            {
                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                string param = dataGridView1.CurrentRow.Cells["Link"].Value.ToString();
                vlcpath = vlcpath + "\\";

                ProcessStartInfo ps = new ProcessStartInfo();
                ps.FileName = Path.Combine(vlcpath, "vlc.exe");  //  ps.FileName = vlcpath + "\\" + "vlc.exe";

                ps.ErrorDialog = false;

                if (_isSingle && Settings.Default.vlc_fullsreen)
                    ps.Arguments = " --one-instance --fullscreen --no-video-title-show " + "\"" + param + "\"";

                else if (_isSingle && !Settings.Default.vlc_fullsreen)
                    ps.Arguments = " --one-instance --no-video-title-show " + "\"" + param + "\"";//+ param;

                else ps.Arguments = " --no-video-title-show " + param;

                ps.Arguments += " --no-qt-error-dialogs";

#if DEBUG
                //   MessageBox.Show("param: " + ps.Arguments.ToString());
#endif

                ps.CreateNoWindow = true;
                ps.UseShellExecute = false;

                ps.RedirectStandardOutput = true;
                ps.WindowStyle = ProcessWindowStyle.Hidden;

                using (Process proc = new Process())
                {
                    proc.StartInfo = ps;

                    proc.Start();
                }
                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;
                _isSingle = false;
            }
        }

        private void button_del_all_Click(object sender, EventArgs e)
        {
            if (_linkchecked) button_check.PerformClick();

            if (dataGridView1.RowCount > 0)
            {
                switch (MessageBox.Show(Mess.Delete_List, Mess.Warning, MessageBoxButtons.YesNo, MessageBoxIcon.None))
                {
                    case DialogResult.Yes:

                        dt.Clear();
                        dt.Columns.Clear();
                        //undoStack.Clear(); redoStack.Clear(); ShowReUnDo(0); toSave(false);
                        toSave(false, true);
                        plabel_Filename.Text = "";
                        button_revert.Visible = false;
                        label_central.BringToFront();
                        dataGridView1.ContextMenuStrip = contextMenuStrip1;
                        break;

                    case DialogResult.No:

                        break;
                }
            }
        }

        private void button_revert_Click(object sender, EventArgs e)
        {
            if (_linkchecked) button_check.PerformClick();
            //message box -> delete all -> open filename
            if (plabel_Filename.Text == "") return;

            switch (MessageBox.Show(Mess.Reload_File, Mess.Warning, MessageBoxButtons.YesNo, MessageBoxIcon.None))
            {
                case DialogResult.Yes:
                    ImportDataset(plabel_Filename.Text, false);
                    //undoStack.Clear(); redoStack.Clear(); ShowReUnDo(0); toSave(false);
                    toSave(false, true);
                    break;

                case DialogResult.No:

                    break;
            }
        }

        private void button_dup_Click(object sender, EventArgs e)
        {
            int colD;
            try
            {
                colD = Settings.Default.colDupli2;
            }
            catch
            {
                colD = 0;
            }

            //var scolID = "Link";

            //if (colD == 0) scolID = "Name2";

            var scolID = columnNames[colD];

            dataGridView1.ClearSelection();

            if (dataGridView1.Rows.Count > 0)
            {
                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    for (int a = 1; a < dataGridView1.Rows.Count - row; a++)
                    {
                        if (dataGridView1.Rows[row].Cells[scolID].Value.Equals(dataGridView1.Rows[row + a].Cells[scolID].Value))
                        {
                            dataGridView1.Rows[row + a].Selected = true;
                            dataGridView1.FirstDisplayedScrollingRowIndex = row + a;
                        }
                    }
                }
            }

            if (ModifierKeys == Keys.Shift)
            {
                button_delLine.PerformClick();
            }
        }

        private void button_clearfind_Click(object sender, EventArgs e)
        {
            textBox_find.Clear();
            textBox_find.Focus();
        }

        private async void Button_check_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0) return;

            bool _altpressed = false; bool _ctrlshiftpressed = false;
            _controlpressed = false;

            switch (ModifierKeys)
            {
                case Keys.Control:
                    _controlpressed = true;
                    break;

                case Keys.Alt:
                    _altpressed = true;
                    break;

                case (Keys.Control | Keys.Shift):
                    _ctrlshiftpressed = true;
                    break;
            }

            if (checkList.Count > 0 && !_linkchecked && !_altpressed
                && Int32.TryParse(checkList[0].Url, out int xx) && xx >= dataGridView1.Rows.Count)
            {
                RepaintRows();
                button_check.BackColor = Color.LightSalmon;
                _linkchecked = true;
                return;
            }

            if (!_linkchecked)
            {
                _linkchecked = true;
                button_check.BackColor = Color.LightSalmon;
            }
            else if (_linkchecked)
            {
                if (_controlpressed)
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (dataGridView1.Rows[row.Index].Cells[0].Style.BackColor == Color.LightSalmon)
                        {
                            dataGridView1.Rows[row.Index].Selected = true;
                        }
                    }
                    return;
                }
                else if (_ctrlshiftpressed)
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (dataGridView1.Rows[row.Index].Cells[0].Style.BackColor == SystemColors.InactiveCaption
                            || dataGridView1.Rows[row.Index].Cells[0].Style.BackColor == Settings.Default.Error403) //SystemColors.ControlLight) //   Color.LightGray)
                        {
                            dataGridView1.Rows[row.Index].Selected = true;
                        }
                    }
                    return;
                }

                _linkchecked = false;
                button_check.BackColor = Color.MidnightBlue;
                colorclear();

                return;
            }

            if (CheckINetConn("http://www.google.com") != 0)
            {
                MessageBox.Show(Mess.No_internet_connection_found);
                return;
            }

            dataGridView1.ClearSelection();

            button_check.Enabled = false;

            if (dataGridView1.Rows.Count > 0)
            {
                colorclear();

                popup popup = new popup();

                popup.FormClosed += new FormClosedEventHandler(FormP_Closed);

                var x = Location.X + (Width - popup.Width) / 2;
                var y = Location.Y + (Height - popup.Height) / 2;
                popup.Location = new Point(Math.Max(x, 0), Math.Max(y, 0));
                popup.StartPosition = FormStartPosition.Manual;
                popup.Owner = this;  //child over parent

                popup.Show();

                Progress<string> progress = new Progress<string>();
                progress.ProgressChanged += (_, text) =>
                    popup.updateProgressBar(text);

                tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;

                await RunStreamCheck2(token, progress);

                popup.Close();

                tokenSource.Cancel();
                tokenSource.Dispose();
                tokenSource = null;
            }

            UseWaitCursor = false;
            Cursor.Current = Cursors.Default;
            //  MessageBox.Show(" Check done");
            RepaintRows();

            button_check.Enabled = true;
        }

        private void RepaintRows()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 1; j < checkList.Count; j++)
                {
                    if (dataGridView1.Rows[i].Cells["Link"].Value.ToString() == checkList[j].Url)
                    {
                        for (int k = 0; k < dataGridView1.ColumnCount; k++)
                        {
                            switch (checkList[j].ErrorCode)
                            {
                                case 0:
                                    dataGridView1.Rows[i].Cells[k].Style.BackColor = SystemColors.Window;
                                    break;

                                case 403:
                                    dataGridView1.Rows[i].Cells[k].Style.BackColor = Settings.Default.Error403;
                                    break;

                                case 410:
                                    dataGridView1.Rows[i].Cells[k].Style.BackColor = SystemColors.InactiveCaption; //  Color.LightGray;
                                    break;

                                case int n when (n != 0 && n != 403 && n != 410):
                                    dataGridView1.Rows[i].Cells[k].Style.BackColor = Color.LightSalmon;
                                    break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        #endregion menu buttons

        #region context menu

        private async void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0) return;

            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected = true;
            string jLink = dataGridView1.CurrentRow.Cells["Link"].Value.ToString();

            //json string Kodi
            jLink = "{ \"jsonrpc\":\"2.0\",\"method\":\"Player.Open\",\"params\":{ \"item\":{ \"file\":\"" + jLink + "\"} },\"id\":0}";

            await ClassKodi.RunOnKodi(jLink);
        }

        private void CopyFullRow()
        {
            try
            {

                // get col header
                StringBuilder rowString = new StringBuilder();

                rowString.Append("FULLROW");  //magic string

                foreach (DataColumn c in dt.Columns)
                {
                    rowString.Append("\t");
                    rowString.Append(c.ColumnName).Append("=\"");  // =\" because element detection

                }
                rowString.Append("\r\n");  //end of line 0

                foreach (DataGridViewRow row in dataGridView1.InvSelectedRows())
                {
                    for (int i = 0; i < dataGridView1.ColumnCount - 1; i++)
                    {
                        rowString.Append(dataGridView1[i, row.Index].Value.ToString().Trim()).Append("\t");
                    }
                    rowString.Append(dataGridView1["Link", row.Index].Value.ToString().Trim());  //to avoid \t
                    rowString.Append("\r\n");
                }

                Clipboard.SetText(rowString.ToString());
                //Clipboard.SetDataObject(rowString.ToString());  //cannot DEBUG
                fullRowContent = rowString.ToString();
            }
            catch (System.Runtime.InteropServices.ExternalException)
            {
                MessageBox.Show(Mess.The_Clipboard_could_not_be_accessed__Please_try_again);
                Clipboard.Clear();
            }



#if DEBUG
            Console.WriteLine("Copy: " + Clipboard.GetText());
#endif
        }

        private void pasteRowMenuItem_Click(object sender, EventArgs e)  //CTRL-I
        {
            // bool _dtEmpty = false;
            if (!CheckClipboard()) return;

            //option 1: empty grid after crtl-N
            if (dataGridView1.RowCount == 0 && dataGridView1.ColumnCount == 0)  //after ctrl-N
            {
                PasteFullRow();
                label_central.SendToBack();
                return;
            }

            DataObject o = (DataObject)Clipboard.GetDataObject();
            string clipText = o.GetData(DataFormats.UnicodeText).ToString();

            //option 2: insert
            PasteMethod(clipText, true);


#if DEBUG
            Console.WriteLine(Clipboard.GetText());
#endif

        }

        private void PasteMethod(string clipText, bool insert, bool emptyGrid = false)
        {
            int a = 0;

            try
            {
                List<ColList> elements = new List<ColList>();
                elements = ClassHelp.SeekFileElements(clipText);
                CreateDataTable(elements);

                fullRowContent = clipText.RemoveFirstLines(1);  //remove header info line 0

                if (!emptyGrid) a = dataGridView1.SelectedCells[0].RowIndex; ;  //start row to copy

                string[] pastedRows = Regex.Split(fullRowContent.TrimEnd("\r\n".ToCharArray()), "\r\n");

                foreach (string pastedRow in pastedRows)
                {
                    string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });

                    dr = dt.NewRow();

                    for (int i = 0; i < pastedRowCells.Length; i++)
                    {
                        dr[i] = pastedRowCells[i];
                    }
                    if (emptyGrid)
                    {
                        dt.Rows.Add(dr);

                    }
                    else
                    {
                        if (!insert) dt.Rows.RemoveAt(a);       //overwrite
                        dt.Rows.InsertAt(dr, a);
                        a++;

                    }

                }

                toSave(true);

            }
            catch (Exception ex)
            {
                MessageBox.Show(Mess.Paste_operation_failed + ex.Message, Mess.Copy_Paste, MessageBoxButtons.OK, MessageBoxIcon.None);
            }

        }



        /// <summary>
        /// paste fullrow from clipboard with overwrite
        /// </summary>
        private void PasteFullRow()
        {

            DataObject o = (DataObject)Clipboard.GetDataObject();
            string clipText = o.GetData(DataFormats.UnicodeText).ToString();


            //option 1: empty grid after crtl-N
            if (dataGridView1.RowCount == 0 && dataGridView1.ColumnCount == 0) //after ctrl-N
            {
                dataGridView1.DataSource = dt;
                label_central.SendToBack();

                PasteMethod(clipText, false, true);
            }

            //option 2: with overwrite of existing cells
            else
            {
                PasteMethod(clipText, false);

            }

        }

        private void cutRowMenuItem_Click(object sender, EventArgs e)   //CTRL-X
        {
            if (dataGridView1.CurrentCell.Value != null && dataGridView1.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected = true;

                try
                {
                    CopyFullRow();

                    button_delLine.PerformClick();
#if DEBUG
                    Console.WriteLine(Clipboard.GetText());
#endif
                    //del line
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        int selectedRow = dataGridView1.SelectedRows[0].Index;

                        dt.Rows.RemoveAt(selectedRow);
                    }
                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    MessageBox.Show(Mess.The_Clipboard_could_not_be_accessed__Please_try_again);
                    Clipboard.Clear();
                }
            }
        }

        private void toolStripCopy_Click(object sender, EventArgs e) //  CTRL-C
        {
            if (dataGridView1.SelectedRows.Count > 0)  //Full rows
            {
                contextMenuStrip1.Items["pasteRowMenuItem"].Enabled = true;

                CopyFullRow();
                return;
            }

            //check selection range and set active cell to min values

            Int32 selectedCellCount = dataGridView1.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 1)
            {
                int minRow = dataGridView1.CurrentCell.RowIndex;
                int minCol = dataGridView1.CurrentCell.ColumnIndex;
                int maxRow = 0, maxCol = 0;
                int x, y;   //rows, columns
                int[] array = Array.Empty<int>();
                int j = 0;

                for (int i = 0; i < selectedCellCount; i++)
                {
                    x = dataGridView1.SelectedCells[i].RowIndex;
                    if (x < minRow) minRow = x;
                    if (x > maxRow) maxRow = x;

                    y = dataGridView1.SelectedCells[i].ColumnIndex;
                    if (y < minCol) minCol = y;
                    if (y > maxCol) maxCol = y;

                    Array.Resize(ref array, array.Length + 2);

                    array[i + j] = x;
                    array[i + 1 + j] = y;
                    j += 1;
                }

                //set active cell
                dataGridView1.CurrentCell = dataGridView1.Rows[minRow].Cells[minCol];

                for (int i = 0; i <= array.Length - 2; i += 2)
                {
                    x = array[i];

                    y = array[i + 1];

                    dataGridView1.Rows[x].Cells[y].Selected = true;
                    //for manual clipboard
                    // fullCopyContent += dataGridView1.Rows[x].Cells[y].Value.ToString();
                }

                StringBuilder cpString = new StringBuilder();

                for (int i = minRow; i <= maxRow; i++)
                {
                    for (int k = minCol; k <= maxCol; k++)
                    {
                        cpString.Append(dataGridView1.Rows[i].Cells[k].Value.ToString().Trim());
                        if (k < maxCol) cpString.Append("\t");
                    }
                    cpString.Append("\r\n");
                }

                Clipboard.SetText(cpString.ToString());
            }
            else if (selectedCellCount == 1)
            {
                Clipboard.SetText(dataGridView1.SelectedCells[0].Value.ToString() + "\r\n");
            }
        }

        private void toolStripPaste_Click(object sender, EventArgs e)   //ctrl+v
        {
            label_central.SendToBack();   //TODO 

            if (CheckClipboard())  //full rows
            {
                PasteFullRow();
                return;
            }
            else if (dataGridView1.SelectedCells.Count > 1)  //no rows just many cells
            {
                FillCells();
            }

            int leftshift = Settings.Default.leftshift;
            try
            {
                string s = Clipboard.GetText();

                string[] lines = Regex.Split(s.TrimEnd("\r\n".ToCharArray()), "\r\n");

                int iRow = dataGridView1.CurrentCell.RowIndex;
                int iCol = dataGridView1.CurrentCell.ColumnIndex;
                DataGridViewCell oCell;

                if (iRow + lines.Length > dataGridView1.Rows.Count - 1)  //true on last line
                {
                    bool bFlag = false;
                    foreach (string sEmpty in lines)
                    {
                        if (sEmpty == "")
                        {
                            bFlag = true;
                        }
                    }

                    dr = dt.NewRow();
                    int iNewRows = iRow + lines.Length - dataGridView1.Rows.Count;
                    if (iNewRows > 0)
                    {
                        if (bFlag)
                            dt.Rows.Add(iNewRows);
                        else
                            dt.Rows.Add(iNewRows + 1);
                    }
                    else if (iNewRows == 0 && iRow != dataGridView1.Rows.Count - 1)
                        dt.Rows.Add(iNewRows + 1);
                }
                foreach (string line in lines)
                {
                    if (iRow < dataGridView1.RowCount && line.Length > 0)
                    {
                        string[] sCells = line.Split('\t');
                        for (int i = 0; i < sCells.GetLength(0); ++i)
                        {
                            if (iCol + i < this.dataGridView1.ColumnCount)
                            {
                                oCell = dataGridView1[iCol + i, iRow];
                                oCell.Value = Convert.ChangeType(sCells[i]/*.Replace("\r", "")*/.Remove(0, leftshift), oCell.ValueType);
                            }
                            else
                            {
                                break;
                            }
                        }
                        iRow++;
                    }
                    else
                    {
                        break;
                    }
                }
                // Clipboard.Clear();
            }
            catch (FormatException)
            {
                MessageBox.Show(Mess.The_data_you_pasted_is_in_the_wrong_format_for_the_cell);
                return;
            }

            toSave(true);
        }

        private void FillCells()
        {
            if (!CheckClipboard())
            {
                string s = Clipboard.GetText();
                DataGridViewCell oCell;

                foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
                {
                    oCell = dataGridView1[cell.ColumnIndex, cell.RowIndex];
                    oCell.Value = Convert.ChangeType(s.Trim(), oCell.ValueType);  //#35
                }
                toSave(true);
            }
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.nostart = true;
            Settings.Default.Save();
            var deffile = new ProcessStartInfo(Application.ExecutablePath);
            Process.Start(deffile);
        }

        private void cm1Number_Click(object sender, EventArgs e)
        {
            DataGridViewCell oCell;
            Int32 n = 1; bool chknum = true;

            foreach (DataGridViewCell cell in dataGridView1.InvSelectedCells())
            {
                oCell = dataGridView1[cell.ColumnIndex, cell.RowIndex];

                if (chknum)
                {
                    var isNumeric = int.TryParse(oCell.Value.ToString(), out Int32 z);
                    if (isNumeric) n = z;
                    chknum = false;
                }

                oCell.Value = Convert.ChangeType(n.ToString(), oCell.ValueType);
                //oCell.Value = Convert.ChangeType(Convert.ToInt32(n), oCell.ValueType);  //test for sort
                n += 1;
            }
            toSave(true);

            //foreach (DataGridViewCell cell in dataGridView1.InvSelectedCells())
            //{
            //    dataGridView1[cell.ColumnIndex, cell.RowIndex].Value = 
            //        Convert.ToInt32(dataGridView1[cell.ColumnIndex, cell.RowIndex].Value);
            //}


        }

        private void cms1GetName_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            DataGridViewCell oCell;
            string cellName = null;
            bool save = false;


            foreach (DataGridViewCell cell in dataGridView1.InvSelectedCells())
            {
                oCell = dataGridView1[cell.ColumnIndex, cell.RowIndex];

                //if (cell.ColumnIndex.Equals(4) || cell.ColumnIndex.Equals(0))
                {
                    // cellName = ClassHelp.GetStreamName(dataGridView1[5, cell.RowIndex].Value.ToString(), ffpPath);
                    cellName = ClassHelp.GetFFrobeStreamName(dataGridView1["Link", cell.RowIndex].Value.ToString(), ffpPath);
                }

                if (!string.IsNullOrEmpty(cellName))
                {
                    oCell.Value = Convert.ChangeType(cellName.ToString(), oCell.ValueType);
                    save = true;
                }
            }
            toSave(save);
            Cursor.Current = Cursors.Default;


        }

        private void AddColEntries()
        {
            string[] regArray = { "tvg-name", "tvg-id", "tvg-title", "tvg-logo", "tvg-chno", "tvg-shift",
                "group-title", "radio", "catchup", "catchup-source", "catchup-days", "catchup-correction",
                "provider", "provider-type", "provider-logo", "provider-countries", "provider-languages",
                "media", "media-dir", "media-size"};
        }

        private void cm3EditF2_CheckStateChanged(object sender, EventArgs e)
        {
            if (cm3EditF2.Checked)
                dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            else
                dataGridView1.EditMode = DataGridViewEditMode.EditOnF2;

            Settings.Default.F2_edit = cm3EditF2.Checked;
            dataGridView1.Refresh();
        }

        private void cm3Scrollbar_CheckStateChanged(object sender, EventArgs e)
        {
            if (cm3Scrollbar.Checked)
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            else
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            Settings.Default.scrollbar = cm3Scrollbar.Checked;
            dataGridView1.Refresh();
        }

        #endregion context menu

        private void textBox_find_TextChange(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_find.Text))
            {
                dataGridView1.ClearSelection();
                dataGridView1.Refresh(); return;
            }

            var colS = Settings.Default.colSearch;
            int findresult = Settings.Default.findresult;  //0: Row   1: Cell

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.ClearSelection();
                _found = false;

                string _name = "";
                List<string> _searchlist = new List<string>();

                if (textBox_find.Text.ToLower().Contains(' '))
                {
                    string[] _search = textBox_find.Text.ToLower().Split(' ');

                    for (int i = 0; i < _search.Length; i++)
                        if (!string.IsNullOrEmpty(_search[i])) _searchlist.Add(_search[i].Trim());
                }
                else
                {
                    _searchlist.Add(textBox_find.Text.ToLower().Trim());
                }

                foreach (DataGridViewRow row in dataGridView1.InvRows())
                {
                    if (colS == dataGridView1.ColumnCount)  //if search in all cells
                    {
                        for (int i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            if (row.Cells[0].Value != null)
                                _name = dt.Rows[row.Index][i].ToString().ToLower();

                            if (!_searchlist.All(x => _name.Contains(x)))  //logical AND
                            {
                                continue;
                            }

                            if (findresult == 0) //sel Rows
                                dataGridView1.Rows[row.Index].Selected = true;
                            else
                                dataGridView1.Rows[row.Index].Cells[i].Selected = true;

                            dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;

                            _found = true;
                            textBox_find.ForeColor = SystemColors.WindowText; //Color.Black;
                        }
                    }
                    else
                    {
                        if (row.Cells[0].Value != null)
                            _name = dt.Rows[row.Index][colS].ToString().ToLower();

                        if (!_searchlist.All(x => _name.Contains(x)))  //logical AND
                            continue;

                        if (findresult == 0)
                        {
                            dataGridView1.Rows[row.Index].Selected = true;
                        }
                        else
                            dataGridView1.Rows[row.Index].Cells[colS].Selected = true;

                        dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;

                        _found = true;
                        textBox_find.ForeColor = SystemColors.WindowText; //Color.Black;
                    }
                }
                if (!_found)//text red
                    textBox_find.ForeColor = Color.Red;
            }

            dataGridView1.Refresh();
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
            {

                // The mouse locations are relative to the screen, so they must be 
                // converted to client coordinates.
                Point clientPoint = dataGridView1.PointToClient(new Point(e.X, e.Y));

                rowIndexOfItemUnderMouseToDrop =
                    dataGridView1.HitTest(clientPoint.X, clientPoint.Y).RowIndex;


                // If the drag operation was a copy then add the row to the other control.
                if (e.Effect == DragDropEffects.Move)
                {
                    if (rowIndexOfItemUnderMouseToDrop < 0)
                    {
                        return;
                    }

                    DataRow dr = dt.NewRow();

                    for (int i = 0; i < dataGridView1.ColumnCount; i++)
                    {
                        dr[i] = dataGridView1[i, rowIndexFromMouseDown].Value.ToString();
                    }

                    dt.Rows.RemoveAt(rowIndexFromMouseDown);
                    dt.Rows.InsertAt(dr, rowIndexOfItemUnderMouseToDrop);


                    toSave(true);
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string dirName, shortName, driveName, extName;

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                foreach (string fileName in files)
                {
                    this.path = fileName;

                    dirName = Path.GetDirectoryName(fileName);
                    shortName = Path.GetFileName(fileName);
                    driveName = Path.GetPathRoot(fileName);
                    extName = Path.GetExtension(fileName);

                    if (extName.Equals(".m3u"))
                    {
                        button_revert.Visible = true;

                        if (dataGridView1.RowCount == 0)
                        {
                            ImportDataset(fileName, false);
                            break;
                        }
                        else  //imoprt and add
                        {
                            ImportDataset(fileName, true);
                            toSave(true);
                            break;
                        }
                    }
                    label_central.SendToBack();

                    toSave(true);
                }
            }
        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// move the selected line up or down
        /// </summary>
        /// <param name="direction">-1 up 1 down</param>
        public void MoveLine(int direction)
        {
            if (_linkchecked) button_check.PerformClick();

            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected = true;

            int i;
            for (i = 0; i < dataGridView1.ColumnCount; i++)
            {
                if (dataGridView1.Columns[dataGridView1.Columns[i].HeaderText].Visible) break;
            }

            if (dataGridView1.SelectedCells.Count > 0 && dataGridView1.SelectedRows.Count > 0)  //whole row must be selected
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                var maxrow = dataGridView1.RowCount - 1;

                if (row != null
                    && !((row.Index == 0 && direction == -1) || (row.Index == maxrow && direction == 1)))
                {
                    DataGridViewRow swapRow = dataGridView1.Rows[row.Index + direction];

                    object[] values = new object[swapRow.Cells.Count];

                    foreach (DataGridViewCell cell in swapRow.Cells)
                    {
                        values[cell.ColumnIndex] = cell.Value;
                        cell.Value = row.Cells[cell.ColumnIndex].Value;
                    }

                    foreach (DataGridViewCell cell in row.Cells)
                        cell.Value = values[cell.ColumnIndex];

                    dataGridView1.Rows[row.Index + direction].Selected = true;
                    dataGridView1.Rows[row.Index].Selected = false;

                    //get first not hidden col and scroll to it  //issue #12

                    dataGridView1.CurrentCell = dataGridView1.Rows[row.Index + direction].Cells[i];  //scroll automatic to cell
                }
            }
            toSave(true);
        }

        /// <summary>
        /// move the selected row to top of list
        /// </summary>
        public void MoveLineTop()
        {
            _linkchecked = false;
            _endofLoop = false;
            button_check.BackColor = Color.MidnightBlue;
            colorclear();

            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected = true;

            if (dataGridView1.SelectedCells.Count > 0 && dataGridView1.SelectedRows.Count > 0)  //whole row must be selected
            {
                var maxrow = dataGridView1.RowCount /*- 1*/;
                int n = 0;

                while (n < maxrow - 1)
                {
                    DataGridViewRow row = dataGridView1.SelectedRows[0];

                    if (row != null)
                    {
                        if ((row.Index == 0) || (row.Index == maxrow)) break; // return;  //check end of dataGridView1

                        var swapRow = dataGridView1.Rows[row.Index - 1];

                        object[] values = new object[swapRow.Cells.Count];

                        foreach (DataGridViewCell cell in swapRow.Cells)
                        {
                            values[cell.ColumnIndex] = cell.Value;
                            cell.Value = row.Cells[cell.ColumnIndex].Value;
                        }

                        foreach (DataGridViewCell cell in row.Cells)
                            cell.Value = values[cell.ColumnIndex];

                        dataGridView1.Rows[row.Index].Selected = false;
                        dataGridView1.Rows[row.Index - 1].Selected = true;
                    }
                    n += 1;
                }
                _endofLoop = true;
                toSave(true);
            }
        }

        public void MoveLineBottom()
        {
            _linkchecked = false;
            _endofLoop = false;
            button_check.BackColor = Color.MidnightBlue;
            colorclear();

            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected = true;

            if (dataGridView1.SelectedCells.Count > 0 && dataGridView1.SelectedRows.Count > 0)  //whole row must be selected
            {
                var maxrow = dataGridView1.RowCount; // - 1;
                int n = 0;

                while (n < maxrow - 1)
                {
                    DataGridViewRow row = dataGridView1.SelectedRows[0];

                    if (row != null)
                    {
                        if (row.Index == maxrow - 1) break; // return;  //check end of dataGridView1

                        var swapRow = dataGridView1.Rows[row.Index + 1];

                        object[] values = new object[swapRow.Cells.Count];

                        foreach (DataGridViewCell cell in swapRow.Cells)
                        {
                            values[cell.ColumnIndex] = cell.Value;
                            cell.Value = row.Cells[cell.ColumnIndex].Value;
                        }

                        foreach (DataGridViewCell cell in row.Cells)
                            cell.Value = values[cell.ColumnIndex];

                        dataGridView1.Rows[row.Index].Selected = false;
                        dataGridView1.Rows[row.Index + 1].Selected = true;
                    }
                    n += 1;
                }
                _endofLoop = true;
                toSave(true);
            }
        }

        /// <summary>
        /// change icon and flag for saving file
        /// </summary>
        /// <param name="hasChanged">true if grid modified vs file</param>
        /// <param name="reset">reset undo/redo stack</param>
        public void toSave(bool hasChanged, bool reset = false)
        {
            if (reset)
            {
                undoStack.Clear(); redoStack.Clear(); ShowReUnDo(0);
            }

            isModified = hasChanged;

            if (hasChanged)
            {
                // button_save.BackgroundImage = Resources.content_save_modified;
                button_save.Image = Resources.content_save_modified_r;
                DataGridView1_CellValidated(null, null);
            }

            if (!hasChanged)
                // button_save.BackgroundImage = Resources.content_save_1_;
                button_save.Image = Resources.content_save_r;
        }

        /// <summary>
        /// fills combobox of player form with data
        /// </summary>
        private void fillPlayer()
        {
            if (player != null)
            {
                player.comboBox1.Items.Clear();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    player.comboBox1.Items.Add(dt.Rows[i]["Name2"]);
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!_endofLoop) return;  //avoid lag with player open

            toSave(true);

            _endofLoop = false;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    contextMenuStrip3.Show(dataGridView1, dataGridView1.PointToClient(Cursor.Position));
            //}
            if (e.Button == MouseButtons.Left)
            {

                toSave(true);

                if (_sort == "desc")
                {
                    _sort = "asc";
                    dataGridView1.Sort(dataGridView1.Columns[e.ColumnIndex], System.ComponentModel.ListSortDirection.Descending);
                }
                else
                {
                    _sort = "desc";
                    dataGridView1.Sort(dataGridView1.Columns[e.ColumnIndex], System.ComponentModel.ListSortDirection.Ascending);
                }

                dt = dt.DefaultView.ToTable(); // The Sorted View converted to DataTable and then assigned to table object.
                dt = dt.DefaultView.ToTable("IPTV");

                //#25 rebind after sort
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();

                SetHeaderContextMenu();

                if (_linkchecked) RepaintRows();  //#41
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Programmatic;
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            //if (dataGridView1.Rows.Count == 0)  //when datagridview empty
            //{
            //    button_open.PerformClick();
            //}
            //else
            //{
            //    if (dataGridView1.RowCount > 0 && !string.IsNullOrEmpty(vlcpath)) button_vlc.PerformClick();
            //}
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!Settings.Default.dclick) return;  //disable

            if (ModifierKeys == Keys.Control)
            {
                playToolStripMenuItem.PerformClick();
            }
            else
            {
                if (dataGridView1.RowCount > 0 && !string.IsNullOrEmpty(vlcpath))
                    button_vlc.PerformClick();
            }
        }

        /// <summary>
        /// event handler for popup window close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormP_Closed(object sender, FormClosedEventArgs e)
        {
            popup popup = (popup)sender;

            tokenSource.Cancel();
        }

        /// <summary>
        /// Check if Link responds
        /// /// </summary>
        /// <param name="token"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        private async Task RunStreamCheck2(CancellationToken token, IProgress<string> progress)
        {
            checkList.Clear();

            string maxrows = dataGridView1.Rows.Count.ToString();

            checkList.Add(new CheckList
            {
                Url = maxrows
            });

            SemaphoreSlim semaphoreObject = new SemaphoreSlim(Settings.Default.maxthread, Settings.Default.maxthread);
            Check streamcheck = new Check();

            List<Task> trackedTasks = new List<Task>();

            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (token.IsCancellationRequested) break;

                var iLink = dataGridView1.Rows[item.Index].Cells["Link"].Value.ToString();

                if (iLink.StartsWith("plugin") || iLink.StartsWith("ud")/* || iLink.Contains("|User")*/)   //plugin will not be checked
                {
                    dataGridView1.Rows[item.Index].Cells["Link"].Style.BackColor = SystemColors.InactiveCaption; //Color.LightGray;
                    dataGridView1.FirstDisplayedScrollingRowIndex = item.Index;
                    continue;
                }

                await semaphoreObject.WaitAsync();
                trackedTasks.Add(Task.Run(() =>
                {
                    try { streamcheck.streamchk(iLink); }
                    catch (Exception) { semaphoreObject.Release(); }
                    finally { semaphoreObject.Release(); }
                }));

                progress.Report(checkList.Count.ToString() + " / " + maxrows);
            }

            await Task.WhenAll(trackedTasks);  //wait for all tasks to finish
        }

        /// <summary>
        /// reset all color settings
        /// </summary>
        private void colorclear()
        {
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    dataGridView1.Rows[item.Index].Cells[j].Style.BackColor = SystemColors.Window;
                }
            }
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            if (dt.Rows.Count == 0) return;
            int Index = dataGridView1.CurrentCell.RowIndex;


            if (redoStack.Count == 0 || redoStack.LoadItem(dataGridView1))
            {
                redoStack.Push(dataGridView1.Rows.Cast<DataGridViewRow>()
                    .Where(r => !r.IsNewRow).Select(r => r.Cells.Cast<DataGridViewCell>()
                    .Select(c => c.Value).ToArray()).ToArray());
            }

            if (undoStack.Count > 0)
            {
                object[][] gridrows = undoStack.Pop();
                while (gridrows.ItemEquals(dataGridView1.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).ToArray()))
                {
                    {
                        try
                        {
                            gridrows = undoStack.Pop();
                        }
                        catch (Exception) { }
                    }
                }
                ignore = true;

                dt.Clear();  // row clear

                for (int x = 0; x <= gridrows.GetUpperBound(0); x++)
                {
                    dt.Rows.Add(gridrows[x]);
                }

                ignore = false;

                ShowReUnDo(0);

                dataGridView1.CurrentCell = dataGridView1[0, Index];

            }
        }

        private void RedoButton_Click(object sender, EventArgs e)
        {
            if (dt.Rows.Count == 0) return;
            if (undoStack.Count == 0 || undoStack.LoadItem(dataGridView1))
            {
                undoStack.Push(dataGridView1.Rows.Cast<DataGridViewRow>()
                    .Where(r => !r.IsNewRow).Select(r => r.Cells.Cast<DataGridViewCell>()
                    .Select(c => c.Value).ToArray()).ToArray());
            }
            if (redoStack.Count > 0)
            {
                object[][] gridrows = redoStack.Pop();

                while (gridrows.ItemEquals(dataGridView1.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).ToArray()))
                {
                    gridrows = redoStack.Pop();
                }
                ignore = true;
                dt.Clear();
                for (int x = 0; x <= gridrows.GetUpperBound(0); x++)
                {
                    dt.Rows.Add(gridrows[x]);
                }

                ignore = false;

                ShowReUnDo(0);
            }
        }

        private void DataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (ignore) { return; }
            if (undoStack.LoadItem(dataGridView1))
            {
                undoStack.Push(dataGridView1.Rows.Cast<DataGridViewRow>()
                    .Where(r => !r.IsNewRow)
                    .Select(r => r.Cells.Cast<DataGridViewCell>()
                    .Select(c => c.Value).ToArray()).ToArray());
            }
            ShowReUnDo(1);
        }

        private void ShowReUnDo(int x)
        {
            if (undoStack.Count > x)
            {
                UndoButton.Enabled = true;
                UndoButton.Image = Resources.undo_r;
            }
            else
            {
                UndoButton.Enabled = false;
                UndoButton.Image = Resources.undo_fade_r;
            }
            if (redoStack.Count > x)
            {
                RedoButton.Enabled = true;
                RedoButton.Image = Resources.redo_r;
            }
            else
            {
                RedoButton.Enabled = false;
                RedoButton.Image = Resources.redo_fade_r;
            }
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        { // #11
            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                dataGridView1.Columns[dataGridView1.Columns[cell.ColumnIndex].HeaderText].Visible = false;
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        { // #11
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[dataGridView1.Columns[i].HeaderText].Visible = true;
            }
        }

        /// <summary>
        /// Event Handler of player combobox.
        /// Gets Combobox entry and plays on vlc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Combo_Changed(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;

            var channel = combo.SelectedIndex;

#if DEBUG
            //  MessageBox.Show("channel: " + channel);
#endif

            if (channel < 0) return;

            dataGridView1.CurrentCell = dataGridView1.Rows[channel].Cells["Name2"];
            dataGridView1.Rows[channel].Selected = true;

            _isSingle = true;

            PlayOnVlc();

            player.Opacity = Settings.Default.opacity;
        }

        /// <summary>
        /// shows or create player form
        /// </summary>
        private void CreatePlayer()
        {
            // if the form is not closed, show it
            if (player == null)
            {
                player = new player();
                player.comboBox1.SelectedIndexChanged += new EventHandler(Combo_Changed);  //combo changed
                player.FormClosed += new FormClosedEventHandler(player_FormClosed);  //form closed

                player.Dgv = this.dataGridView1;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    player.comboBox1.Items.Add(dt.Rows[i]["Name2"]);
                }

                if (Settings.Default.F1Location.X == 0 && Settings.Default.F1Location.Y == 0)
                {
                    // first start
                    player.Location = new Point(10, 10);
                }
                else
                {
                    player.Location = Settings.Default.F1Location;
                }
                player.StartPosition = FormStartPosition.Manual;
                // attach the handler
                player.FormClosed += ChildFormClosed;
            }

            // show it
            player.Show();
        }

        private void player_FormClosed(object sender, FormClosedEventArgs e)
        {
            //close vlc
            try
            {
                Process[] processes = null;
                processes = Process.GetProcessesByName("vlc");
                foreach (Process process in processes)
                {
                    process.Kill();
                }
            }
            catch (ArgumentException)
            {
            }
        }

        private void ChildFormClosed(object sender, FormClosedEventArgs args)
        {
            // detach the handler
            player.FormClosed -= ChildFormClosed;

            // let GC collect it (and this way we can tell if it's closed)
            player = null;
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)  //empty grid
            {
                for (int i = 0; i < contextMenuStrip1.Items.Count; i++)  //0,1 enabled
                {
                    contextMenuStrip1.Items[i].Enabled = false;
                }
                if (!string.IsNullOrEmpty(fullRowContent))  //for paste to new window
                    contextMenuStrip1.Items["pasteRowMenuItem"].Enabled = true;  //paste add
                else if (string.IsNullOrEmpty(fullRowContent) && CheckClipboard())
                    contextMenuStrip1.Items["pasteRowMenuItem"].Enabled = true;  //paste add
                else
                    contextMenuStrip1.Items["pasteRowMenuItem"].Enabled = false;

                //if (!string.IsNullOrEmpty(fullRowContent)
                //    || (string.IsNullOrEmpty(fullRowContent) && CheckClipboard(columnNames)))  //TODO
                //    contextMenuStrip1.Items["pasteRowMenuItem"].Enabled = true;  //paste add
                //else
                //    contextMenuStrip1.Items["pasteRowMenuItem"].Enabled = false;

                contextMenuStrip1.Items["cms1NewWindow"].Enabled = true;
            }
            else  //open menu
            {
                string[] itemsNList = new string[] { "toolStripCopy", "playToolStripMenuItem",
                    "hideToolStripMenuItem", "showToolStripMenuItem"};

                for (int i = 0; i < itemsNList.Length; i++)
                {
                    contextMenuStrip1.Items[itemsNList[i]].Enabled = true;
                }

                if (dataGridView1.SelectedRows.Count > 0)
                {
                    contextMenuStrip1.Items["cutRowMenuItem"].Enabled = true;  //cut
                }
                else
                {
                    contextMenuStrip1.Items["cutRowMenuItem"].Enabled = false;
                }

                //ffprobe only when found  //TODO
                if (_ffprobefound)
                {
                    if (dataGridView1.SelectedCells.Cast<DataGridViewCell>().Select(c => c.ColumnIndex).Distinct().Count() == 1)

                        contextMenuStrip1.Items["cms1GetName"].Visible = true;
                    else
                        contextMenuStrip1.Items["cms1GetName"].Visible = false;

                }

                //Numbering only in rows
                if (dataGridView1.SelectedCells.Count > 1)
                {
                    Int32 selectedCellCount = dataGridView1.GetCellCount(DataGridViewElementStates.Selected);

                    int minCol = dataGridView1.CurrentCell.ColumnIndex;
                    int maxCol = 0;
                    int y;   //columns

                    for (int i = 0; i < selectedCellCount; i++)
                    {
                        y = dataGridView1.SelectedCells[i].ColumnIndex;
                        if (y < minCol) minCol = y;
                        if (y > maxCol) maxCol = y;
                    }

                    if (minCol == maxCol)
                        contextMenuStrip1.Items["cms1Number"].Enabled = true;
                    else
                        contextMenuStrip1.Items["cms1Number"].Enabled = false;
                }
                else
                    contextMenuStrip1.Items["cms1Number"].Enabled = false;

                if (Clipboard.ContainsText())
                {
                    contextMenuStrip1.Items["toolStripPaste"].Enabled = true;  //paste
                }

                //if (!string.IsNullOrEmpty(fullRowContent))  //for paste to new window
                //    contextMenuStrip1.Items["pasteRowMenuItem"].Enabled = true;  //paste add
                //else if (string.IsNullOrEmpty(fullRowContent) && CheckClipboard(columnNames))
                //    contextMenuStrip1.Items["pasteRowMenuItem"].Enabled = true;  //paste add
                //else
                //    contextMenuStrip1.Items["pasteRowMenuItem"].Enabled = false;
            }
        }


        private void button_import_Click(object sender, EventArgs e)  //TODO
        {
            if (dataGridView1.RowCount > 0)
            {
                MessageBox.Show(Mess.Import_only_on_empty);
                return;
            }

            dt.TableName = "IPTV";

            checkList.Clear();

            dataGridView1.DataSource = dt;

            DataObject o = (DataObject)Clipboard.GetDataObject();

            if (Clipboard.ContainsText())
            {
                // string line;

                using (StringReader playlistFile = new StringReader(o.GetData(DataFormats.UnicodeText).ToString()))
                {
                    Cursor.Current = Cursors.WaitCursor;

                    line = playlistFile.ReadLine();  //first line

                    if (line.StartsWith("#EXTM3U"))
                    {
                        fileHeader = line;
                    }

                    string fullTxt = playlistFile.ReadToEnd();  //read rest of file
                    string[] fileRows = fullTxt.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    List<ColList> elements = new List<ColList>();
                    elements = ClassHelp.SeekFileElements(fullTxt);

                    CreateDataTable(elements);

                    for (int i = 0; i < fileRows.Length; i++)
                    {

                        if (fileRows[i].StartsWith("#EXTINF"))
                        {
                            dr = dt.NewRow();

                            for (int j = 0; j < dt.Columns.Count - 2; j++)
                            {
                                string header = dt.Columns[j].ToString();
                                var match = Regex.Match(fileRows[i], header + "=\"([^\"]*)\"").Groups[1];

                                if (match.Success)
                                {
                                    string udpIP = match.Captures[0].Value;
                                    dr[header] = udpIP;
                                    continue;
                                }

                            }
                            dr["Name2"] = fileRows[i].Split(',').Last().Trim();

                            continue;
                        }
                        else if (linktypes.Any(fileRows[i].StartsWith)
                                && (fileRows[i].Contains("//") || fileRows[i].Contains(":\\")))//issue #32 issue #61

                        {
                            dr["Link"] = fileRows[i];

                            try { dt.Rows.Add(dr); }
                            catch { continue; }
                        }
                    }






                    Cursor.Current = Cursors.Default;
                }
                label_central.SendToBack();

                toSave(true);
            }

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show(Mess.Wrong_input, Mess.File_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            playToolStripMenuItem.PerformClick();
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.ContextMenuStrip = contextMenuStrip2;
        }

        private void editCellCopy_Click(object sender, EventArgs e)
        {
            if (dataGridView1.EditingControl is TextBox textBox)
            {
                //  TextBox textBox = (TextBox)dataGridView1.EditingControl;

                if (!string.IsNullOrEmpty(textBox.SelectedText))
                    Clipboard.SetText(textBox.SelectedText);
            }
        }

        private void editCellPaste_Click(object sender, EventArgs e)
        {
            string s = Clipboard.GetText();
            if (dataGridView1.EditingControl is TextBox)
            {
                TextBox textBox = (TextBox)dataGridView1.EditingControl;
                textBox.SelectedText = s;
            }
        }

        private void dataGridView1_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                contextMenuStrip3.Show(dataGridView1, dataGridView1.PointToClient(Cursor.Position));
            }
            else
            {
                contextMenuStrip1.Show(dataGridView1, dataGridView1.PointToClient(Cursor.Position));

            }
        }

        private void DataGridStyle()
        {
            dataGridView1.EnableHeadersVisualStyles = false;  //to make header style take effect

            DataGridViewCellStyle column_header_cell_style = new DataGridViewCellStyle();
            column_header_cell_style.BackColor = SystemColors.ControlLight;
            column_header_cell_style.ForeColor = Color.Black;
            //column_header_cell_style.SelectionBackColor = Color.Chocolate;
            //column_header_cell_style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //column_header_cell_style.Font = new Font("Tahoma", 12, FontStyle.Bold, GraphicsUnit.Pixel);  //set in ZoomGrid


            this.dataGridView1.ColumnHeadersDefaultCellStyle = column_header_cell_style;
        }

        private void cm1NewColumn_Click(object sender, EventArgs e)
        {
            cm1ColCombo.Items.Clear();
        }

        private void cm1ColCombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip5_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dataGridView1.ColumnCount == 0)
            {
                for (int i = 0; i < contextMenuStrip5.Items.Count; i++)  //0,1 enabled
                {
                    contextMenuStrip5.Items[i].Enabled = false;
                }
            }
            else
            {
                cm5ColumNames.Items.Clear();

                foreach (string s in columnNames)
                {
                    cm5ColumNames.Items.Add(s);
                }

                cm5ColumNames.SelectedIndex = Settings.Default.colDupli2;

            }
        }

        private void cm5ColumNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.colDupli2 = cm5ColumNames.SelectedIndex;
            Settings.Default.Save();
        }

        private void cm5StartSearchDupli_Click(object sender, EventArgs e)
        {
            button_dup.PerformClick();
        }

        private void cm4EditFIleHeader_Click(object sender, EventArgs e)
        {
            using (EditHeader h = new EditHeader(fileHeader))
            {
                var result = h.ShowDialog();

                if (result == DialogResult.OK)
                {
                    fileHeader = h.headerText + "\n"; // Environment.NewLine;
                }

            }

        }

        private void cm1AddColumn_Click(object sender, EventArgs e)
        {
            string addColumns = cm1ColCombo.Text;
            if (string.IsNullOrEmpty(addColumns)) return;
            dt.Columns.Add(addColumns).SetOrdinal(0);

            //reorder the Columns
            foreach (DataColumn c in dt.Columns)
            {
                dataGridView1.Columns[c.ColumnName].DisplayIndex = dt.Columns.IndexOf(c);
            }

            cm1ColCombo.Items.Clear();
        }

        private void cm1ColCombo_Click(object sender, EventArgs e)
        {
            cm1ColCombo.Items.Clear();
            List<string> droplist = new List<string>();

            string[] regArray = { "tvg-name", "tvg-id", "tvg-title", "tvg-logo", "tvg-chno", "tvg-shift",
                "group-title", "radio", "catchup", "catchup-source", "catchup-days", "catchup-correction",
                "provider", "provider-type", "provider-logo", "provider-countries", "provider-languages",
                "media", "media-dir", "media-size"};

            droplist.AddRange(regArray);

            List<string> regArray2 = new List<string>();

            foreach (DataColumn c in dt.Columns)
            {
                regArray2.Add(c.ColumnName);
            }

            var result = droplist.Except(regArray2);

            foreach (string s in result)
            {
                cm1ColCombo.Items.Add(s);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!Settings.Default.dclick) return;  //disable

            if (ModifierKeys == Keys.Control)
            {
                playToolStripMenuItem.PerformClick();
            }
            else
            {
                if (dataGridView1.RowCount > 0 && !string.IsNullOrEmpty(vlcpath))
                    button_vlc_Click(sender, e);
                // button_vlc.PerformClick();
            }

        }


        private void editCellCut_Click(object sender, EventArgs e)
        {
            if (dataGridView1.EditingControl is TextBox)
            {
                TextBox textBox = (TextBox)dataGridView1.EditingControl;
                if (textBox.SelectedText != "") Clipboard.SetText(textBox.SelectedText);
                textBox.SelectedText = "";
            }
        }

        private void addUseragentCell_Click(object sender, EventArgs e)
        {
            if (dataGridView1.EditingControl is TextBox)
            {
                TextBox textBox = (TextBox)dataGridView1.EditingControl;
                if (textBox.Text.EndsWith("m3u8"))
                { //#18
                    textBox.Text += "|User-Agent=" + Settings.Default.user_agent;
                    // textBox.Text += "|User-Agent=Mozilla/5.0 (X11; Linux i686; rv:42.0) Gecko/20100101 Firefox/42.0 Iceweasel/42.0";
                }
            }
        }

        private void contextMenuStrip2_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {  //opens when edit cell active
            if (dataGridView1.EditingControl is TextBox)
            {
                TextBox textBox = (TextBox)dataGridView1.EditingControl;
                if (textBox.Text.EndsWith("m3u8"))
                {
                    contextMenuStrip2.Items["addUseragentCell"].Enabled = true;
                }
                else
                {
                    contextMenuStrip2.Items["addUseragentCell"].Enabled = false;
                }
            }
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // if (Debugger.IsAttached) return;  //#37 test

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 /*& IsSelected*/)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                string[] _search = textBox_find.Text.ToLower().Split(' ');
                string sw = _search[0].Trim();

                if (!string.IsNullOrEmpty(sw))
                {
                    for (int i = 0; i < _search.Length; i++)
                    {
                        sw = _search[i].Trim();
                        PaintCells(sw, i);
                    }
                }
                e.PaintContent(e.CellBounds);
            }

            void PaintCells(string sw, int s_length)
            {
                Color[] colors = new Color[] { Color.Orange, Color.Yellow, Color.GreenYellow };

                string val = (string)e.FormattedValue;
                int sindx = val.ToLower().IndexOf(sw.ToLower());

                if (sindx >= 0)
                {
                    Rectangle hl_rect = new Rectangle();
                    hl_rect.Y = e.CellBounds.Y + 2;
                    hl_rect.Height = e.CellBounds.Height - 5;

                    string sBefore = val.Substring(0, sindx);
                    string sWord = val.Substring(sindx, sw.Length);
                    Size s1 = TextRenderer.MeasureText(e.Graphics, sBefore, e.CellStyle.Font, e.CellBounds.Size);
                    Size s2 = TextRenderer.MeasureText(e.Graphics, sWord, e.CellStyle.Font, e.CellBounds.Size);

                    if (s1.Width > 5)
                    {
                        hl_rect.X = e.CellBounds.X + s1.Width - 5;
                        hl_rect.Width = s2.Width - 6;
                    }
                    else
                    {
                        hl_rect.X = e.CellBounds.X + 2;
                        hl_rect.Width = s2.Width - 6;
                    }

                    SolidBrush hl_brush = default(SolidBrush);
                    if ((e.State & DataGridViewElementStates.Selected) != DataGridViewElementStates.None)
                    {
                        hl_brush = new SolidBrush(Color.DarkGoldenrod);
                    }
                    else if (s_length < 3)
                    {
                        hl_brush = new SolidBrush(colors[s_length]);
                    }
                    else
                    {
                        hl_brush = new SolidBrush(Color.Yellow);
                    }

                    e.Graphics.FillRectangle(hl_brush, hl_rect);

                    hl_brush.Dispose();
                }
            }
        }

        private void button_refind_Click(object sender, EventArgs e)
        {
            textBox_find_TextChange(sender, e);
        }


        private void textBox_find_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)27)
            {
                textBox_find.Visible = false;
                button_clearfind.Visible = false; lblRowCheck.Visible = false; lblColCheck.Visible = false;
                button_refind.Visible = false;
            }
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = dataGridView1.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                    dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;

        }

        private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    // Proceed with the drag and drop, passing in the list item.
                    DragDropEffects dropEffect = dataGridView1.DoDragDrop(
                    dataGridView1.Rows[rowIndexFromMouseDown],
                    DragDropEffects.Move);
                }
            }
        }

        private void dataGridView1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            toSave(true);
        }


        private void CreateDataTable(List<ColList> elements)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                dt.Clear();  // row clear
                dt.Columns.Clear();  // col clear

                foreach (ColList name in elements)
                {
                    dt.Columns.Add(name.Name.ToString());
                }

                dt.Columns.Add("Name2");
                dt.Columns.Add("Link");
            }
            else  //Add new Column
            {
                if (dataGridView1.ColumnCount - 2 < elements.Count)
                {
                    string allheaders = "";

                    for (int j = 0; j < dataGridView1.ColumnCount - 2; j++)
                    {
                        allheaders += dataGridView1.Columns[j].ToString();
                    }

                    foreach (ColList name in elements)
                    {
                        if (!allheaders.Contains(name.Name.ToString()))
                        {
                            dt.Columns.Add(name.Name.ToString()).SetOrdinal(0);

                        }
                    }

                    //reorder the Columns
                    foreach (DataColumn c in dt.Columns)
                    {
                        dataGridView1.Columns[c.ColumnName].DisplayIndex = dt.Columns.IndexOf(c);
                    }


                }
            }
            SetHeaderContextMenu();

            columnNames.Clear();

            foreach (DataColumn c in dt.Columns)
            {
                columnNames.Add(c.ColumnName);
            }



        }

        //here new methods

    }
}

/// <summary>
/// class for streamcheck semaphore
/// </summary>
internal class Check
{
    public void streamchk(string ipUrl)
    {
        CheckIPTVStream(ipUrl);

        return;
    }
}


/// <summary>
/// DataGridView Method extensions
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// double buffer on for large files speed up
    /// </summary>
    /// <param name="dgv"></param>
    /// <param name="setting"></param>
    public static void DoubleBuffered(this DataGridView dgv, bool setting)
    {
        //http://bitmatic.com/c/fixing-a-slow-scrolling-datagridview

        Type dgvType = dgv.GetType();
        PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
            BindingFlags.Instance | BindingFlags.NonPublic);
        pi.SetValue(dgv, setting, null);
    }

    /// <summary>
    /// reverse order of selected rows for foreach
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<DataGridViewRow> InvSelectedRows(this DataGridView source)
    {
        for (int i = source.SelectedRows.Count - 1; i >= 0; i--)
            yield return source.SelectedRows[i];
    }

    /// <summary>
    /// inverse order of selected cells for foreach
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<DataGridViewCell> InvSelectedCells(this DataGridView source)
    {
        for (int i = source.SelectedCells.Count - 1; i >= 0; i--)
            yield return source.SelectedCells[i];
    }

    /// <summary>
    /// inverse order of rows for foreach
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<DataGridViewRow> InvRows(this DataGridView source)
    {
        for (int i = source.Rows.Count - 1; i >= 0; i--)
            yield return source.Rows[i];
    }

    /// <summary>
    /// inverse order of rows for foreach
    /// </summary>
    /// <param name="source">string</param>
    /// <returns>string</returns>
    public static IEnumerable<string> InvRows(this string[] source)  //#44
    {
        for (int i = source.Length - 1; i >= 0; i--)
            yield return source[i];
    }

    /// <summary>
    /// removes first lines of string
    /// </summary>
    /// <param name="text"></param>
    /// <param name="linesCount"></param>
    /// <returns></returns>
    public static string RemoveFirstLines(this string text, int linesCount)
    {
        var lines = Regex.Split(text, "\r\n|\r|\n").Skip(linesCount);
        return string.Join(Environment.NewLine, lines.ToArray());
    }
}