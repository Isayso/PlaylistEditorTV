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
using PlaylistEditor.Properties;



//ToDo bug no move after sorting, no reload, binding problem? .NET problem
//ToDo selective dead link check?


namespace PlaylistEditor
{
    public partial class Form1 : Form
    {

        Stack<object[][]> undoStack = new Stack<object[][]>();
        Stack<object[][]> redoStack = new Stack<object[][]>();

        Boolean ignore = false;
        private CancellationTokenSource tokenSource;

        private player player;

        bool isModified = false;

        public string fullRowContent = "";
        public string fullCopyContent = "";
        public string fileName = "";
        public string line;
        private string path;

        public bool _isIt = true;
        public bool _found = false;
        public bool _savenow = false;
        public bool _taglink = false;
        public bool _isSingle = false;
        public bool _mark = false;

        const int mActionHotKeyID = 1;  //var for key hook listener


        //zoom of fonts
        public float zoomf = 1F;
        // private static readonly int ROWHEIGHT = 47;
        private static readonly float FONTSIZE = 9.163636F;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataRow dr;
        string vlcpath = Settings.Default.vlcpath;

        public int[] colShow = new int[6];


        public Form1()
        {
            InitializeComponent();

            this.Text = String.Format("PlaylistEditor TV " + " v{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 5));

#if DEBUG
            //  Clipboard.Clear();
            this.Text = String.Format("PlaylistEditor TV DEBUG" + " v{0}", Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 5));
#endif


            var spec_key = Settings.Default.specKey;  //for key listener
            var hotlabel = Settings.Default.hotkey;

            //Modifier keys codes: Alt = 1, Ctrl = 2, Shift = 4, Win = 8  must be added
            //   RegisterHotKey(this.Handle, mActionHotKeyID, 1, (int)Keys.Y);  //ALT-Y
            NativeMethods.RegisterHotKey(this.Handle, mActionHotKeyID, spec_key, hotlabel);  //ALT-Y


            plabel_Filename.Text = "";
            button_revert.Visible = false;


            //  dataGridView1.AllowUserToAddRows = true;
            dataGridView1.ShowCellToolTips = false;
            dataGridView1.DoubleBuffered(true);
            dataGridView1.BringToFront();
            //    dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;

            //command line arguments [1]
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1) //drag drop
            {
                plabel_Filename.Text = args[1];
                importDataset(args[1], false);
                button_revert.Visible = true;
            }
        }



        /// <summary>
        /// listener to hotkey for import of links from clipboard
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == mActionHotKeyID)
            {
                // key pressed matches our listener
                //if (dataGridView1.Rows.Count > 0)
                //{
                //    ClassHelp.PopupForm("editor not empty", "red", 1500);
                //    return;
                //}

                ClassHelp.PopupForm("List import.....", "blue", 1500);

                button_import.PerformClick();


            }
            base.WndProc(ref m);
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Modifiers == Keys.Control)
                {
                    switch (e.KeyCode)
                    {

                        case Keys.C:
                            if (dataGridView1.SelectedRows.Count > 0)
                            {
                                contextMenuStrip1.Items[5].Enabled = true;
                                CopyRow();
                            }
                            else toolStripCopy.PerformClick();
                            break;

                        case Keys.V:
                            contextMenuStrip1.Items[3].Enabled = true;
                            toolStripPaste.PerformClick();
                            break;


                        case Keys.R:
                            copyRowMenuItem.PerformClick();
                            break;

                        case Keys.I:
                            if (dataGridView1.SelectedRows.Count > 0 || dataGridView1.Rows.Count == 0
                                || (string.IsNullOrEmpty(fullRowContent) && ClassHelp.CheckClipboard()))
                                contextMenuStrip1.Items[5].Enabled = true;  //paste add

                            pasteRowMenuItem.PerformClick();
                            break;

                        case Keys.X:
                            if (dataGridView1.SelectedRows.Count > 0)
                            {
                                contextMenuStrip1.Items[4].Enabled = true;
                                cutRowMenuItem.PerformClick();
                            }
                            break;

                        case Keys.T:  //move line to top

                            MoveLineTop();
                            break;

                        case Keys.F:
                            button_search.PerformClick();
                            break;

                        case Keys.N:
                            var info = new ProcessStartInfo(Application.ExecutablePath);
                            Process.Start(info);
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
                    }
                }
                if (e.KeyCode == Keys.Delete && dataGridView1.IsCurrentCellInEditMode == false)
                {
                    button_delLine.PerformClick();
                }
                //if (e.KeyCode == Keys.F3 && dataGridView1.IsCurrentCellInEditMode == true)
                //{
                //    addUseragentCell.PerformClick();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Key press operation failed. " /*+ ex.Message*/, "Key press", MessageBoxButtons.OK, MessageBoxIcon.None);
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

            //  dataGridView1.RowTemplate.Height = (int)(ROWHEIGHT * f);

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isModified == true && dataGridView1.RowCount > 0)
            {
                DialogResult dialogSave = MessageBox.Show("Do you want to save your current playlist?",
                "Save Playlist", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogSave == DialogResult.Yes)
                    saveFileDialog1.ShowDialog();
                isModified = false;
            }

           

            Application.Exit();


        }


        /*--------------------------------------------------------------------------------*/
        // Menu Buttons
        /*--------------------------------------------------------------------------------*/

        private void button_search_Click(object sender, EventArgs e)
        {
            textBox_find.BringToFront();

            if (_isIt)
            {
                _isIt = !_isIt;
                textBox_find.Visible = true;
                this.ActiveControl = textBox_find;
            }
            else
            {
                _isIt = !_isIt;
                textBox_find.Visible = false;
            }

        }


        private void button_open_Click(object sender, EventArgs e)
        {
            if (_taglink) button_check.PerformClick();

            Cursor.Current = Cursors.WaitCursor;
            string openpath = Settings.Default.openpath;
            if (!string.IsNullOrEmpty(openpath) && !ClassHelp.MyDirectoryExists(openpath, 4000))
                openpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.InitialDirectory = openpath;
                openFileDialog1.RestoreDirectory = false;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    importDataset(openFileDialog1.FileName, false);
                    button_revert.Visible = true;
                }
                else  //cancel
                {
                    return;
                }

                Settings.Default.openpath = Path.GetDirectoryName(openFileDialog1.FileName);
                Settings.Default.Save();
            }
            Cursor.Current = Cursors.Default;
        }

        private void button_Info_Click(object sender, EventArgs e)
        {
            AboutBox1 a = new AboutBox1();
            a.ShowDialog();   //  ShowDialog gets focus, Show not
            //centre position on Infoform
        }

        private void button_settings_Click(object sender, EventArgs e)
        {
            settings s = new settings();
            s.ShowDialog();
        }


        /// <summary>
        /// import of playlist entries
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="append">false/true for append</param>
        public void importDataset(string filename, bool append)
        {


            if (!ClassHelp.FileIsIPTV(filename))
            {
                MessageBox.Show("File has wrong format!  ");
                return;
            }

            dt.TableName = "IPTV";


            dataGridView1.DataSource = dt;
            string[] col = new string[6];
            Array.Clear(colShow, 0, 6);

            StreamReader playlistFile = new StreamReader(filename);
            if (!append)  //append false
            {
                dt.Clear();  // row clear
                dt.Columns.Clear();  // col clear

                plabel_Filename.Text = filename;

                dt.Columns.Add("Name"); dt.Columns.Add("id"); dt.Columns.Add("Title");
                dt.Columns.Add("logo"); dt.Columns.Add("Name2"); dt.Columns.Add("Link");

            }

            while ((line = playlistFile.ReadLine()) != null)
            {
                line = line.Trim();  //remove spaces

                if (line.StartsWith("#EXTINF"))
                {

                    col[0] = ClassHelp.GetPartString(line, "tvg-name=\"", "\"");
                    CheckEntry(0);


                    col[1] = ClassHelp.GetPartString(line, "tvg-id=\"", "\"");
                    CheckEntry(1);


                    col[2] = ClassHelp.GetPartString(line, "group-title=\"", "\"");
                    CheckEntry(2);


                    col[3] = ClassHelp.GetPartString(line, "tvg-logo=\"", "\"");
                    CheckEntry(3);


                    col[4] = line.Split(',').Last();
                    if (string.IsNullOrEmpty(col[4])) col[4] = "N/A";


                    continue;

                }

                else if ( (line.StartsWith("ht") || line.StartsWith("plugin")) 
                    && (line.Contains("//") || line.Contains(":\\")) )
                {
                    col[5] = line;
                }

               

                else
                {
                    continue;  //if file has irregular linefeed.
                }


                try
                {
                    dr = dt.NewRow();
                    dr["Name"] = col[0].Trim(); dr["id"] = col[1].Trim(); dr["Title"] = col[2].Trim();
                    dr["logo"] = col[3].Trim(); dr["Name2"] = col[4].Trim(); dr["Link"] = col[5].Trim();
                    dt.Rows.Add(dr);
                }
                catch (ArgumentOutOfRangeException)
                {
                    MessageBox.Show("Argument out of range error. Wrong format.");
                    continue;
                }
            }
            playlistFile.Close();

            dataGridView1.AllowUserToAddRows = false;

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Wrong data structure! ", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (colShow[0] != 1) dataGridView1.Columns["Name"].Visible = false;
            if (colShow[1] != 1) dataGridView1.Columns["id"].Visible = false;
            if (colShow[2] != 1) dataGridView1.Columns["Title"].Visible = false;
            if (colShow[3] != 1) dataGridView1.Columns["logo"].Visible = false;
            colShow[4] = 1;
            colShow[5] = 1;

            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[5];
            dataGridView1.Rows[0].Selected = true;

            void CheckEntry(int v)
            {//issue #12
                if (string.IsNullOrEmpty(col[v]) || (col[v].Contains("N/A") && colShow[v] == 0))
                {
                    col[v] = "N/A";
                    colShow[v] = 0;
                }
                else
                {
                    colShow[v] = 1;
                }


            }

        }



        private void button_delLine_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    // int selectedRow = dataGridView1.SelectedRows[0].Index;
                    int selectedRow = dataGridView1.SelectedCells[0].RowIndex;

                    dt.Rows.RemoveAt(selectedRow);
                }
                toSave(true);

            }
            else
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

                //foreach (var cell in from DataGridViewRow row in dataGridView1.Rows
                //                     from DataGridViewCell cell in row.Cells
                //                     select cell)
                //{
                //    cell.Value = string.Empty;
                //}


                toSave(true);

            }




        }

        private void button_save_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;


            if ((ModifierKeys == Keys.Shift || _savenow) && !string.IsNullOrEmpty(plabel_Filename.Text)
                && ClassHelp.MyDirectoryExists(Path.GetDirectoryName(plabel_Filename.Text), 4000))
            {

                saveFileDialog1.FileName = plabel_Filename.Text;


                using (StreamWriter file = new StreamWriter(saveFileDialog1.FileName, false /*, Encoding.UTF8*/))   //false: file ovewrite
                {

                    file.NewLine = "\n";  // win: LF
                    file.WriteLine("#EXTM3U");
                    string writestring = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {//issue #12  write only visible columns
                        writestring = "#EXTINF:-1 ";
                        if (dataGridView1.Columns["Name"].Visible) writestring += "tvg-name=\"" + dt.Rows[i][0] + "\"";
                        if (dataGridView1.Columns["id"].Visible) writestring += " tvg-id=\"" + dt.Rows[i][1] + "\"";
                        if (dataGridView1.Columns["Title"].Visible) writestring += " group-title=\"" + dt.Rows[i][2] + "\"";
                        if (dataGridView1.Columns["logo"].Visible) writestring += " tvg-logo=\"" + dt.Rows[i][3] + "\"";

                        writestring += "," + dt.Rows[i][4];

                        file.WriteLine(writestring);
                        file.WriteLine(dt.Rows[i][5]);

                    }

                }
                toSave(false);
                button_revert.Visible = true;
                _savenow = false;

                ClassHelp.PopupForm("Playlist Saved", "green", 1500);


            }

            else if (saveFileDialog1.ShowDialog() == DialogResult.OK)  //open file dialog
            {
                plabel_Filename.Text = saveFileDialog1.FileName;

                using (StreamWriter file = new StreamWriter(saveFileDialog1.FileName, false /*, Encoding.UTF8*/))   //false: file ovewrite
                {

                    file.NewLine = "\n";  // win: LF
                    file.WriteLine("#EXTM3U");
                    string writestring = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {  //issue #12  write only visible columns
                        writestring = "#EXTINF:-1 ";
                        if (dataGridView1.Columns["Name"].Visible) writestring += "tvg-name=\"" + dt.Rows[i][0] + "\"";
                        if (dataGridView1.Columns["id"].Visible) writestring += " tvg-id=\"" + dt.Rows[i][1] + "\"";
                        if (dataGridView1.Columns["Title"].Visible) writestring += " group-title=\"" + dt.Rows[i][2] + "\"";
                        if (dataGridView1.Columns["logo"].Visible) writestring += " tvg-logo=\"" + dt.Rows[i][3] + "\"";

                        writestring += "," + dt.Rows[i][4];

                        file.WriteLine(writestring);
                        file.WriteLine(dt.Rows[i][5]);

                    }

                }

                toSave(false);

                button_revert.Visible = true;
                Cursor.Current = Cursors.Default;
            }
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
            //bug remove sorting -> .sort="" -> write table before
            //dt.DefaultView.Sort = "";
            //dataGridView1.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f => f.SortMode = DataGridViewColumnSortMode.NotSortable);

            MoveLine(1);
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            DataRow dr = dt.NewRow();


            if (dataGridView1.RowCount > 0)
            {
                int a = dataGridView1.SelectedCells[0].RowIndex;  // row index in a datatable

                dr[0] = "Name"; dr[1] = "id"; dr[2] = "Title"; dr[3] = "Logo";
                dr[4] = "Name2"; dr[5] = "Link";

                dt.Rows.InsertAt(dr, a);

            }
            else
            {

                dt.TableName = "IPTV";

                dt.Columns.Add("Name"); dt.Columns.Add("id"); dt.Columns.Add("Title");
                dt.Columns.Add("logo"); dt.Columns.Add("Name2"); dt.Columns.Add("Link");
                dr[0] = "Name"; dr[1] = "id"; dr[2] = "Title"; dr[3] = "Logo";
                dr[4] = "Name2"; dr[5] = "Link";

                dt.Rows.InsertAt(dr, 0);


                dataGridView1.DataSource = dt;
                dataGridView1.AllowUserToAddRows = false;
            }

            toSave(true);
        }


        private void button_vlc_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(vlcpath))
            {
                vlcpath = ClassHelp.GetVlcPath();
                if (string.IsNullOrEmpty(vlcpath))
                    ClassHelp.PopupForm("VLC player not found", "red", 3000);
                return;
            }

            if (dataGridView1.RowCount > 0 && dataGridView1.CurrentRow.Cells[5].Value.ToString().StartsWith("plugin"))
            {
                ClassHelp.PopupForm("Plugin links only work in Kodi ", "red", 3000);
                return;  //#18
            }
            else if (dataGridView1.RowCount > 0 && dataGridView1.CurrentRow.Cells[5].Value.ToString().Contains("|User"))
            {
                ClassHelp.PopupForm("User-Agent links only work in Kodi ", "red", 3000);
                return;  //#18
            }

                //if (_isSingle)
                //{
                //    //Process[] processes = null;
                //    //processes = Process.GetProcessesByName("vlc");
                //    //foreach (Process process in processes)
                //    //{
                //    //    process.Kill();
                //    //}
                //}


                if (dataGridView1.RowCount > 0 && !string.IsNullOrEmpty(vlcpath))
            {

                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                string param = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                ProcessStartInfo ps = new ProcessStartInfo();
                ps.FileName = vlcpath + "\\" + "vlc.exe";
                ps.ErrorDialog = false;

                if (_isSingle && Settings.Default.vlc_fullsreen)
                    ps.Arguments = " --one-instance --fullscreen --no-video-title-show " + param;

                else if (_isSingle && !Settings.Default.vlc_fullsreen)
                    ps.Arguments = " --one-instance --no-video-title-show " + param;

                else ps.Arguments = " --no-video-title-show " + param;

                ps.CreateNoWindow = true;
                ps.UseShellExecute = false;

                ps.RedirectStandardOutput = true;
                ps.WindowStyle = ProcessWindowStyle.Hidden;

                using (Process proc = new Process())
                {
                    proc.StartInfo = ps;

                    proc.Start();
                    //  proc.WaitForExit();

                }
                // Set cursor as default arrow
                Cursor.Current = Cursors.Default;

            }
        }




        private void button_del_all_Click(object sender, EventArgs e)
        {
            if (_taglink) button_check.PerformClick();

            if (dataGridView1.RowCount > 0)
            {
                switch (MessageBox.Show("Delete List?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.None))
                {
                    case DialogResult.Yes:

                        dt.Clear();
                        dt.Columns.Clear();
                        toSave(false);
                        plabel_Filename.Text = "";
                        button_revert.Visible = false;
                        break;

                    case DialogResult.No:

                        break;
                }

            }
        }

        private void button_revert_Click(object sender, EventArgs e)
        {
            if (_taglink) button_check.PerformClick();
            //message box -> delete all -> open filename
            switch (MessageBox.Show("Reload File?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.None))
            {
                case DialogResult.Yes:
                    importDataset(plabel_Filename.Text, false);
                    toSave(false);
                    break;

                case DialogResult.No:

                    break;
            }
        }

        /*--------------------------------------------------------------------------------*/
        // contextMenueStrip Entries
        /*--------------------------------------------------------------------------------*/

        private async void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0) return;

            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected = true;
            string jLink = dataGridView1.CurrentRow.Cells[5].Value.ToString();


            jLink = "{ \"jsonrpc\":\"2.0\",\"method\":\"Player.Open\",\"params\":{ \"item\":{ \"file\":\"" + jLink + "\"} },\"id\":0}";


            await ClassKodi.Run(jLink);

        }

        private void copyRowMenuItem_Click(object sender, EventArgs e)  //CTRL-R
        {

            if (dataGridView1.CurrentCell.Value != null && dataGridView1.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                try
                {
                    // Add the selection to the clipboard.

                    //    Clipboard.SetDataObject(this.dataGridView1.GetClipboardContent());

                    //issue #12
                    StringBuilder rowString = new StringBuilder();

                    foreach (DataGridViewRow row in dataGridView1.GetSelectedRows())
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            rowString.Append(dataGridView1[i, row.Index].Value.ToString().Trim()).Append("\t");
                        }
                        rowString.Append(dataGridView1[5, row.Index].Value.ToString().Trim());
                        rowString.Append("\r\n");
                    }
                    // Clipboard.SetText(rowString.ToString());
                    Clipboard.SetDataObject(rowString.ToString());
#if DEBUG
                    Console.WriteLine(Clipboard.GetText());
#endif
                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    MessageBox.Show("The Clipboard could not be accessed. Please try again.");
                    //  Clipboard.Clear();
                }
            }
        }

        private void CopyRow()
        {
            if (dataGridView1.CurrentCell.Value != null && dataGridView1.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {

                try
                {
                    StringBuilder rowString = new StringBuilder();

                    foreach (DataGridViewRow row in dataGridView1.GetSelectedRows())
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            rowString.Append(dataGridView1[i, row.Index].Value.ToString().Trim()).Append("\t");
                        }
                        rowString.Append(dataGridView1[5, row.Index].Value.ToString().Trim());
                        rowString.Append("\r\n");
                    }
                    // Clipboard.SetText(rowString.ToString());
                    Clipboard.SetDataObject(rowString.ToString());
                    fullRowContent = rowString.ToString();

                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    MessageBox.Show("The Clipboard could not be accessed. Please try again.");
                    Clipboard.Clear();
                }
            }
#if DEBUG
            Console.WriteLine("Copy " + Clipboard.GetText());
#endif


        }

        private void pasteRowMenuItem_Click(object sender, EventArgs e)  //CTRL-I
        {

            bool _dtEmpty = false;

            if (dataGridView1.RowCount == 0 && dataGridView1.ColumnCount == 0)
            {
                _dtEmpty = true;
                DataRow dr = dt.NewRow();

                dt.Columns.Add("Name"); dt.Columns.Add("id"); dt.Columns.Add("Title");
                dt.Columns.Add("logo"); dt.Columns.Add("Name2"); dt.Columns.Add("Link");
                dataGridView1.DataSource = dt;

            }

#if DEBUG
            Console.WriteLine(Clipboard.GetText());
#endif

            if (!string.IsNullOrEmpty(fullRowContent) && Clipboard.GetText() == fullRowContent)    //new bugfix 1.4.1  todo compare clipboard to fullrowcontent only not equal
            {
                try
                {
                    int a = 0;
                    if (!_dtEmpty) a = dataGridView1.SelectedCells[0].RowIndex;  //select row in a datatable

                    //   string[] pastedRows = Regex.Split(o.GetData(DataFormats.UnicodeText).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                    string[] pastedRows = Regex.Split(fullRowContent.TrimEnd("\r\n".ToCharArray()), "\r\n");
                    foreach (string pastedRow in pastedRows)
                    {
                        string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });

                        dr = dt.NewRow();
                        dr["Name"] = pastedRowCells[0]; dr["id"] = pastedRowCells[1]; dr["Title"] = pastedRowCells[2];
                        dr["logo"] = pastedRowCells[3]; dr["Name2"] = pastedRowCells[4]; dr["Link"] = pastedRowCells[5];

                        if (_dtEmpty)
                        {
                            dt.Rows.Add(dr);
                        }
                        else
                        {
                            dt.Rows.InsertAt(dr, a);  //insert above marked row  
                            a++;
                        }
                    }
                    toSave(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Paste operation failed. " + ex.Message, "Copy/Paste", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
            else // if ((string.IsNullOrEmpty(fullRowContent) || Clipboard.GetText() != fullRowContent) && Clipboard.ContainsText())  //todo null or not equal
            {
                DataObject o = (DataObject)Clipboard.GetDataObject();

                try
                {
                    int a = 0;
                    if (!_dtEmpty) a = dataGridView1.SelectedCells[0].RowIndex;  //select row in a datatable

                    string[] pastedRows = Regex.Split(o.GetData(DataFormats.UnicodeText).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                    //   string[] pastedRows = Regex.Split(fullRowContent.TrimEnd("\r\n".ToCharArray()), "\r\n");
                    foreach (string pastedRow in pastedRows)
                    {
                        string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });

                        dr = dt.NewRow();
                        dr["Name"] = pastedRowCells[0]; dr["id"] = pastedRowCells[1]; dr["Title"] = pastedRowCells[2];
                        dr["logo"] = pastedRowCells[3]; dr["Name2"] = pastedRowCells[4]; dr["Link"] = pastedRowCells[5];

                        if (_dtEmpty)
                        {
                            dt.Rows.Add(dr);
                        }
                        else
                        {
                            dt.Rows.InsertAt(dr, a);  //insert above marked row  
                            a++;
                        }
                    }
                    toSave(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Paste operation failed. " + ex.Message, "Copy/Paste", MessageBoxButtons.OK, MessageBoxIcon.None);
                }

            }
        }



        private void PasteRow()
        {
            bool _dtEmpty = false;

            if (dataGridView1.RowCount == 0 && dataGridView1.ColumnCount == 0)
            {
                _dtEmpty = true;
                DataRow dr = dt.NewRow();

                dt.Columns.Add("Name"); dt.Columns.Add("id"); dt.Columns.Add("Title");
                dt.Columns.Add("logo"); dt.Columns.Add("Name2"); dt.Columns.Add("Link");
                dataGridView1.DataSource = dt;

            }

            //  DataObject o = (DataObject)Clipboard.GetDataObject();

            //if (Clipboard.GetText() != fullRowContent)
            //{
            //    fullRowContent = Clipboard.GetText();
            //}

            if (!string.IsNullOrEmpty(fullRowContent) && Clipboard.GetText() == fullRowContent)    //new bugfix 1.4.1
            {
                try
                {
                    int a = 0;
                    if (!_dtEmpty) a = dataGridView1.SelectedCells[0].RowIndex;  //select row in a datatable

                    //   string[] pastedRows = Regex.Split(o.GetData(DataFormats.UnicodeText).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                    string[] pastedRows = Regex.Split(fullRowContent.TrimEnd("\r\n".ToCharArray()), "\r\n");

                    //  for (int i = 0; i < pastedRows.Count(); i++)

                    foreach (string pastedRow in pastedRows)
                    {
                        string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });

                        dr = dt.NewRow();
                        dr["Name"] = pastedRowCells[0]; dr["id"] = pastedRowCells[1]; dr["Title"] = pastedRowCells[2];
                        dr["logo"] = pastedRowCells[3]; dr["Name2"] = pastedRowCells[4]; dr["Link"] = pastedRowCells[5];

                        if (_dtEmpty)
                        {
                            dt.Rows.Add(dr);
                        }
                        else
                        {
                            dt.Rows.RemoveAt(a);       //overwrite
                            dt.Rows.InsertAt(dr, a);
                            a++;
                        }

                    }
                    toSave(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Paste operation failed. " + ex.Message, "Copy/Paste", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
            else // if (string.IsNullOrEmpty(fullRowContent) && Clipboard.ContainsText())
            {
                DataObject o = (DataObject)Clipboard.GetDataObject();

                try
                {
                    int a = 0;
                    if (!_dtEmpty) a = dataGridView1.SelectedCells[0].RowIndex;  //select row in a datatable

                    string[] pastedRows = Regex.Split(o.GetData(DataFormats.UnicodeText).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                    //   string[] pastedRows = Regex.Split(fullRowContent.TrimEnd("\r\n".ToCharArray()), "\r\n");
                    foreach (string pastedRow in pastedRows)
                    {
                        string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });

                        dr = dt.NewRow();
                        dr["Name"] = pastedRowCells[0]; dr["id"] = pastedRowCells[1]; dr["Title"] = pastedRowCells[2];
                        dr["logo"] = pastedRowCells[3]; dr["Name2"] = pastedRowCells[4]; dr["Link"] = pastedRowCells[5];

                        if (_dtEmpty)
                        {
                            dt.Rows.Add(dr);
                        }
                        else
                        {
                            if (dataGridView1.RowCount > 0) dt.Rows.RemoveAt(a);       //overwrite
                            dt.Rows.InsertAt(dr, a);  //insert above marked row  
                            a++;
                        }
                    }
                    toSave(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Paste operation failed. " + ex.Message, "Copy/Paste", MessageBoxButtons.OK, MessageBoxIcon.None);
                }

            }
        }



        private void cutRowMenuItem_Click(object sender, EventArgs e)   //CTRL-X
        {
            if (dataGridView1.CurrentCell.Value != null && dataGridView1.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {

                dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected = true;

                try
                {
                    // Add the selection to the clipboard.

                    //   Clipboard.SetDataObject(this.dataGridView1.GetClipboardContent());

                    //issue #12
                    StringBuilder rowString = new StringBuilder();

                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            rowString.Append(dataGridView1[i, row.Index].Value.ToString().Trim()).Append("\t");

                        }
                        rowString.Append(dataGridView1[5, row.Index].Value.ToString().Trim());
                        rowString.Append("\r\n");
                    }

                    Clipboard.SetDataObject(rowString.ToString());
                    fullRowContent = rowString.ToString();


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
                    MessageBox.Show("The Clipboard could not be accessed. Please try again.");
                    Clipboard.Clear();
                }
            }
        }

        private void pasteReplaceRowMenuItem_Click(object sender, EventArgs e)  //CRTL-V  from CTRL-R
        {

            bool _dtEmpty = false;

            if (dataGridView1.RowCount == 0 && dataGridView1.ColumnCount == 0)
            {
                _dtEmpty = true;
                DataRow dr = dt.NewRow();

                dt.Columns.Add("Name"); dt.Columns.Add("id"); dt.Columns.Add("Title");
                dt.Columns.Add("logo"); dt.Columns.Add("Name2"); dt.Columns.Add("Link");
                dataGridView1.DataSource = dt;

            }

            //     DataObject o = (DataObject)Clipboard.GetDataObject();
#if DEBUG
            Console.WriteLine(Clipboard.GetText());
#endif
            //todo check if clipboard has new content

            if (!string.IsNullOrEmpty(fullRowContent))
            {
                try
                {
                    int a = 0;
                    if (!_dtEmpty) a = dataGridView1.SelectedCells[0].RowIndex;  //select row in a datatable

                    //   string[] pastedRows = Regex.Split(o.GetData(DataFormats.UnicodeText).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                    string[] pastedRows = Regex.Split(fullRowContent.TrimEnd("\r\n".ToCharArray()), "\r\n");
                    foreach (string pastedRow in pastedRows)
                    {
                        string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });

                        dr = dt.NewRow();
                        dr["Name"] = pastedRowCells[0]; dr["id"] = pastedRowCells[1]; dr["Title"] = pastedRowCells[2];
                        dr["logo"] = pastedRowCells[3]; dr["Name2"] = pastedRowCells[4]; dr["Link"] = pastedRowCells[5];

                        if (_dtEmpty)
                        {
                            dt.Rows.Add(dr);
                        }
                        else
                        {
                            dt.Rows.RemoveAt(a);
                            dt.Rows.InsertAt(dr, a);
                            a++;
                        }
                    }
                    toSave(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Paste operation failed. " + ex.Message, "Copy/Paste", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
            }
        }

        private void toolStripCopy_Click(object sender, EventArgs e) //  CTRL-C
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                contextMenuStrip1.Items[5].Enabled = true;

                CopyRow();
                return;
            }

            //DataObject dataObj = dataGridView1.GetClipboardContent();
            //if (dataObj != null)
            //    Clipboard.SetDataObject(dataObj);

            //check selection range and set active cell to min values

            Int32 selectedCellCount = dataGridView1.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 1)
            {
                int minRow = dataGridView1.CurrentCell.RowIndex;
                int minCol = dataGridView1.CurrentCell.ColumnIndex;
                int x = 0; int maxRow = 0; int maxCol = 0;
                int y = 0;
                int[] array = new int[0];
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

            if (/*(dataGridView1.SelectedRows == 0 &&*/ ClassHelp.CheckClipboard())
            {
                PasteRow();
                return;
            }

            int leftshift = Settings.Default.leftshift;
            try
            {
                string s = Clipboard.GetText();
                //   string[] lines = s.Split('\n');  //bug ??? \r\n

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
                MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
            }
        }



        private void textBox_selectAll_Click(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        private void textBox_find_TextChanged(object sender, EventArgs e)
        {
            var colS = Settings.Default.colSearch;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.ClearSelection();
                _found = false;

                string _name = "";

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (colS == 6)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            if (row.Cells[0].Value != null)
                                _name = dt.Rows[row.Index][i].ToString().ToLower();

                            if (_name.Contains(textBox_find.Text.ToLower()) && textBox_find.Text != "")
                            {
                                dataGridView1.Rows[row.Index].Selected = true;
                                _found = true;
                                textBox_find.ForeColor = Color.Black;
                            }
                        }
                    }
                    else
                    {
                        if (row.Cells[0].Value != null)
                            _name = dt.Rows[row.Index][colS].ToString().ToLower();

                        if (_name.Contains(textBox_find.Text.ToLower()) && textBox_find.Text != "")
                        {
                            dataGridView1.Rows[row.Index].Selected = true;
                            _found = true;
                            textBox_find.ForeColor = Color.Black;
                        }
                    }



                }
                if (!_found)//text red 
                    textBox_find.ForeColor = Color.Red;
            }

            dataGridView1.Refresh();
        }



        private void button_dup_Click(object sender, EventArgs e)
        {
            var colD = Settings.Default.colDupli;

            dataGridView1.ClearSelection();

            if (dataGridView1.Rows.Count > 0)
            {

                for (int row = 0; row < dataGridView1.Rows.Count; row++)
                {
                    for (int a = 1; a < dataGridView1.Rows.Count - row; a++)
                    {
                        if (dataGridView1.Rows[row].Cells[colD].Value.Equals(dataGridView1.Rows[row + a].Cells[colD].Value))
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



        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
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
                            importDataset(fileName, false);
                            break;
                        }
                        else  //imoprt and add
                        {
                            importDataset(fileName, true);
                            toSave(true);
                            break;
                        }

                    }
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
            if (_taglink) button_check.PerformClick();

            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected = true;

            int i;
            for (i = 0; i < dataGridView1.ColumnCount; i++)
            {
                if (dataGridView1.Columns[dataGridView1.Columns[i].HeaderText].Visible) break;
            }

            if (dataGridView1.SelectedCells.Count > 0 && dataGridView1.SelectedRows.Count > 0)  //whole row must be selected
            {
                var row = dataGridView1.SelectedRows[0];
                var maxrow = dataGridView1.RowCount - 1;

                if (row != null)
                {
                    if ((row.Index == 0 && direction == -1) || (row.Index == maxrow && direction == 1)) return;  //check end of dataGridView1

                    var swapRow = dataGridView1.Rows[row.Index + direction];

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

                //  toSave(true);
            }
        }

        /// <summary>
        /// move the selected row to top of list
        /// </summary>
        public void MoveLineTop()
        {
            _taglink = false;
            button_check.BackColor = Color.MidnightBlue;
            colorclear();


            //  if (_taglink) button_check.PerformClick();

            dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected = true;

            if (dataGridView1.SelectedCells.Count > 0 && dataGridView1.SelectedRows.Count > 0)  //whole row must be selected
            {
                var row = dataGridView1.SelectedRows[0];
                var maxrow = dataGridView1.RowCount /*- 1*/;
                int n = 0;

                while (n < maxrow - 1)
                {
                    row = dataGridView1.SelectedRows[0];

                    if (row != null)
                    {
                        if ((row.Index == 0) || (row.Index == maxrow)) return;  //check end of dataGridView1

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

                toSave(true);
            }
        }

        /// <summary>
        /// changes icon if file is modified
        /// </summary>
        public void toSave(bool hasChanged)
        {

            if (isModified == hasChanged) return;

            isModified = hasChanged;

            if (hasChanged)
                button_save.BackgroundImage = Resources.content_save_modified;

            if (!hasChanged)
                button_save.BackgroundImage = Resources.content_save_1_;

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            toSave(true);
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {


            toSave(true);

            //if (dataGridView1.SortOrder.ToString() == "Descending") // Check if sorting is Descending
            //{
            //    dt.DefaultView.Sort = dataGridView1.SortedColumn.Name + " DESC"; // Get Sorted Column name and sort it in Descending order
            //}
            //else
            //{
            //    dt.DefaultView.Sort = dataGridView1.SortedColumn.Name + " ASC";  // Otherwise sort it in Ascending order
            //}
            dt = dt.DefaultView.ToTable(); // The Sorted View converted to DataTable and then assigned to table object.
            dt = dt.DefaultView.ToTable("IPTV");
        }




        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)  //when datagridview empty
            {
                button_open.PerformClick();
            }

        }



        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                playToolStripMenuItem.PerformClick();
            }
            else
            {
                if (dataGridView1.RowCount > 0 && !string.IsNullOrEmpty(vlcpath)) button_vlc.PerformClick();
            }

        }

        private async void Button_check_Click(object sender, EventArgs e)
        {

            if (!_taglink)
            {
                _taglink = true;
                button_check.BackColor = Color.LightSalmon;
            }
            else if (_taglink)
            {
                if (ModifierKeys == Keys.Control)
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

                _taglink = false;
                button_check.BackColor = Color.MidnightBlue;
                colorclear();
                //  cts.Cancel();
                return;
            }


            if (ModifierKeys == Keys.Control) _mark = true;
            else _mark = false; //select links


            if (!ClassHelp.CheckIPTVStream("http://www.google.com"))
            {
                MessageBox.Show("No internet connection found!");
                return;
            }

            dataGridView1.ClearSelection();

            //      Cursor.Current = Cursors.WaitCursor;
            //    dataGridView1.Enabled = false;
            button_check.Enabled = false;


            //var tokenSource = new CancellationTokenSource();
            //var token = tokenSource.Token;


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

                await Task.Run(() => RunStreamCheck(token, progress));

                popup.Close();

                tokenSource.Cancel();  //??
                tokenSource.Dispose();
                tokenSource = null;
            }

            button_check.Enabled = true;
        }


        /// <summary>
        /// event handler for popup window close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FormP_Closed(object sender, FormClosedEventArgs e)
        {
            popup popup = (popup)sender;

            tokenSource.Cancel();
        }

        private void RunStreamCheck(CancellationToken token, IProgress<string> progress)
        {

            string maxrows = dataGridView1.Rows.Count.ToString();

            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                var iLink = dataGridView1.Rows[item.Index].Cells[5].Value.ToString();

                progress.Report(item.Index.ToString() + " / " + maxrows);

                //#18 no plugin check -> grey background
                if (iLink.StartsWith("plugin") || iLink.Contains("|User"))
                {
                    dataGridView1.Rows[item.Index].Cells[5].Style.BackColor = Color.LightGray;
                    dataGridView1.FirstDisplayedScrollingRowIndex = item.Index;
                    continue;
                }

                if (!ClassHelp.CheckIPTVStream(iLink))
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (_mark) dataGridView1.Rows[item.Index].Selected = true;
                        dataGridView1.Rows[item.Index].Cells[i].Style.BackColor = Color.LightSalmon;
                    }
                    dataGridView1.FirstDisplayedScrollingRowIndex = item.Index;
                }
            }
        }


        private void colorclear()
        {
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                for (int j = 0; j < 6; j++)
                {
                    dataGridView1.Rows[item.Index].Cells[j].Style.BackColor = Color.White;
                }
            }
        }

        private void UndoButton_Click(object sender, EventArgs e)
        {
            if (dt.Rows.Count == 0) return;
            if (redoStack.Count == 0 || redoStack.LoadItem(dataGridView1))
            {
                redoStack.Push(dataGridView1.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).Select(r => r.Cells.Cast<DataGridViewCell>().Select(c => c.Value).ToArray()).ToArray());
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

                UndoButton.Enabled = undoStack.Count > 0;
                RedoButton.Enabled = redoStack.Count > 0;
            }
        }

        private void RedoButton_Click(object sender, EventArgs e)
        {

            if (dt.Rows.Count == 0) return;
            if (undoStack.Count == 0 || undoStack.LoadItem(dataGridView1))
            {
                undoStack.Push(dataGridView1.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).Select(r => r.Cells.Cast<DataGridViewCell>().Select(c => c.Value).ToArray()).ToArray());
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

                RedoButton.Enabled = redoStack.Count > 0;
                UndoButton.Enabled = undoStack.Count > 0;
            }
        }

        private void DataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (ignore) { return; }
            if (undoStack.LoadItem(dataGridView1))
            {
                undoStack.Push(dataGridView1.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).Select(r => r.Cells.Cast<DataGridViewCell>().Select(c => c.Value).ToArray()).ToArray());
            }
            UndoButton.Enabled = undoStack.Count > 1;
            RedoButton.Enabled = redoStack.Count > 1;
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


        private void singleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (/*dataGridView1.SelectedRows.Count > 0
                ||*/ this.WindowState == FormWindowState.Maximized) return;

            //get col width, index, hide all others, set form width to col width
            if (!_isSingle)
            {
                //int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                int columnIndex = 4;  //Name
 
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    if (i != columnIndex)
                        dataGridView1.Columns[dataGridView1.Columns[i].HeaderText].Visible = false;
                }

                this.Size = new Size(400, 422);

                dataGridView1.Size = new Size(382, 422 - 44);
                dataGridView1.Location = new Point(0, 0);

                contextMenuStrip1.Items[12].Text = "Editor mode";
                _isSingle = true;
        
                CreateOrShow();
             
            }
            else
            {
                player.Close();             
                

                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    if (colShow[i] == 1) dataGridView1.Columns[dataGridView1.Columns[i].HeaderText].Visible = true;
                }

                this.Size = new Size(1140, 422);

                dataGridView1.Size = new Size(1122, 319);
                dataGridView1.Location = new Point(0, 59);

                contextMenuStrip1.Items[12].Text = "Player mode";
                _isSingle = false;

             
               
            }
        }

         void Combo_Changed(object sender, EventArgs e)
        {
            
            ComboBox combo = (ComboBox)sender;
         
            var channel = combo.SelectedIndex;

            dataGridView1.CurrentCell = dataGridView1.Rows[channel].Cells[4];
            dataGridView1.Rows[channel].Selected = true;
          
            button_vlc.PerformClick();
        }

        private void CreateOrShow()
        {
            // if the form is not closed, show it
            if (player == null)
            {
                player = new player();
                player.comboBox1.SelectedIndexChanged += new EventHandler(Combo_Changed);
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    player.comboBox1.Items.Add(dt.Rows[i][4]);
                }
                player.Location = new Point(10, 10);
                player.StartPosition = FormStartPosition.Manual;
                // attach the handler
                player.FormClosed += ChildFormClosed;
            }

            // show it
            player.Show();
        }

        void ChildFormClosed(object sender, FormClosedEventArgs args)
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
                if (!string.IsNullOrEmpty(fullRowContent)
                    || (string.IsNullOrEmpty(fullRowContent) && ClassHelp.CheckClipboard()))
                    contextMenuStrip1.Items[5].Enabled = true;  //paste add

                else
                    contextMenuStrip1.Items[5].Enabled = false;
            }
            else  //open 
            {
                int[] itemsList = new int[] { 2, 7, 9, 10, 12 };

                for (int i = 0; i < itemsList.Length; i++)
                {
                    contextMenuStrip1.Items[itemsList[i]].Enabled = true;
                }

                if (dataGridView1.SelectedRows.Count > 0)
                {
                    contextMenuStrip1.Items[4].Enabled = true;  //cut
                    // contextMenuStrip1.Items[5].Enabled = true;
                }
                else
                {
                    contextMenuStrip1.Items[4].Enabled = false;
                    // contextMenuStrip1.Items[5].Enabled = false;  //paste add
                }

                if (Clipboard.ContainsText())
                    contextMenuStrip1.Items[3].Enabled = true;  //paste

                //   if (ClassHelp.CheckClipboard())
                if (!string.IsNullOrEmpty(fullRowContent))  //for paste to new window
                    contextMenuStrip1.Items[5].Enabled = true;  //paste add
                else if (string.IsNullOrEmpty(fullRowContent) && ClassHelp.CheckClipboard())
                    contextMenuStrip1.Items[5].Enabled = true;  //paste add

                //else if (string.IsNullOrEmpty(fullRowContent) && Clipboard.ContainsText())
                //    contextMenuStrip1.Items[5].Enabled = true;  //paste add

                else
                    contextMenuStrip1.Items[5].Enabled = false;

                if (this.WindowState == FormWindowState.Maximized)
                    contextMenuStrip1.Items[12].Enabled = false;  //no single col mode for fullscreen
            }
        }

        private void button_import_Click(object sender, EventArgs e)
        {
            //   if (ClassHelp.CheckClipboard() || dataGridView1.Rows.Count > 0) return;

            dt.TableName = "IPTV";

            dataGridView1.DataSource = dt;
            string[] col = new string[6];
            Array.Clear(colShow, 0, 6);

            if (dataGridView1.Rows.Count == 0 && dataGridView1.ColumnCount == 0)
            {
                dt.Clear();  // row clear
                dt.Columns.Clear();  // col clear

                dt.Columns.Add("Name"); dt.Columns.Add("id"); dt.Columns.Add("Title");
                dt.Columns.Add("logo"); dt.Columns.Add("Name2"); dt.Columns.Add("Link");
            }


            DataObject o = (DataObject)Clipboard.GetDataObject();

            if (Clipboard.ContainsText())
            {
                string line;

                using (StringReader playlistFile = new StringReader(o.GetData(DataFormats.UnicodeText).ToString()))
                {

                    while ((line = playlistFile.ReadLine()) != null)
                    {
                        line = line.Trim();  //remove spaces

                        if (line.StartsWith("#EXTINF"))
                        {

                            col[0] = ClassHelp.GetPartString(line, "tvg-name=\"", "\"");
                            CheckEntry(0);


                            col[1] = ClassHelp.GetPartString(line, "tvg-id=\"", "\"");
                            CheckEntry(1);


                            col[2] = ClassHelp.GetPartString(line, "group-title=\"", "\"");
                            CheckEntry(2);


                            col[3] = ClassHelp.GetPartString(line, "tvg-logo=\"", "\"");
                            CheckEntry(3);


                            col[4] = line.Split(',').Last();
                            if (string.IsNullOrEmpty(col[4])) col[4] = "N/A";


                            continue;

                        }




                        else if ( (line.StartsWith("ht") || line.StartsWith("plugin")) 
                            && (line.Contains("//") || line.Contains(":\\")) 
                            && !string.IsNullOrEmpty(col[0]) )
                        {
                            col[5] = line;
                        }

                        //else if (line.StartsWith("plugin")  //#18
                        //    && ((line.Contains("//") || line.Contains(":\\")))
                        //    && !string.IsNullOrEmpty(col[0]))
                        //{
                        //    if (!line.Contains("|User-Agent") && line.Contains(".m3u8") && Settings.Default.user_agent)
                        //        col[5] = line + "|User-Agent=Mozilla/5.0 (X11; Linux i686; rv:42.0) Gecko/20100101 Firefox/42.0 Iceweasel/42.0";
                        //    else col[5] = line;
                        //}

                        else
                        {
                            continue;  //if file has irregular linefeed.
                        }

                        try
                        {

                            dr = dt.NewRow();
                            dr["Name"] = col[0].Trim(); dr["id"] = col[1].Trim(); dr["Title"] = col[2].Trim();
                            dr["logo"] = col[3].Trim(); dr["Name2"] = col[4].Trim(); dr["Link"] = col[5].Trim();
                            dt.Rows.Add(dr);
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            MessageBox.Show("Argument out of range error. Wrong format.");
                            continue;
                        }
                    }
                }

                toSave(true);
            }



            //   dataGridView1.AllowUserToAddRows = false;  //???

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Wrong input! ", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (colShow[0] != 1) dataGridView1.Columns["Name"].Visible = false;
            if (colShow[1] != 1) dataGridView1.Columns["id"].Visible = false;
            if (colShow[2] != 1) dataGridView1.Columns["Title"].Visible = false;
            if (colShow[3] != 1) dataGridView1.Columns["logo"].Visible = false;
            colShow[4] = 1;
            colShow[5] = 1;


            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[5];
            dataGridView1.Rows[0].Selected = true;


            void CheckEntry(int v)
            {
                if (string.IsNullOrEmpty(col[v]) || (col[v].Contains("N/A") && colShow[v] == 0))
                {
                    col[v] = "N/A";
                    colShow[v] = 0;
                }
                else
                {
                    colShow[v] = 1;
                }


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            playToolStripMenuItem.PerformClick();
        }

        private void toolStripNumbering_Click(object sender, EventArgs e)
        {
            //cell region in id column -> frist cell must have number -> add
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //  e.Control.ContextMenu = new ContextMenu();
            e.Control.ContextMenuStrip = contextMenuStrip2;
        }

        private void editCellCopy_Click(object sender, EventArgs e)
        {
          //  toolStripCopy.PerformClick();

            if (dataGridView1.EditingControl is TextBox)
            {
                var textBox = (TextBox)dataGridView1.EditingControl;
                if (textBox.SelectedText != "") Clipboard.SetText(textBox.SelectedText);
                //textBox.SelectedText = "";
            }
        }

        private void editCellPaste_Click(object sender, EventArgs e)
        {
           // toolStripPaste.PerformClick();
            string s = Clipboard.GetText();
            if (dataGridView1.EditingControl is TextBox)
            {
                var textBox = (TextBox)dataGridView1.EditingControl;
                textBox.SelectedText = s;
            }
        }

        private void editCellCut_Click(object sender, EventArgs e)
        {          
            if (dataGridView1.EditingControl is TextBox)
            {
                var textBox = (TextBox)dataGridView1.EditingControl;
                if (textBox.SelectedText != "") Clipboard.SetText(textBox.SelectedText);
                textBox.SelectedText = "";
            }
        }

        private void addUseragentCell_Click(object sender, EventArgs e)
        {
            if (dataGridView1.EditingControl is TextBox)
            {
                var textBox = (TextBox)dataGridView1.EditingControl;
                if (textBox.Text.EndsWith("m3u8"))
                {
                    textBox.Text += "|User-Agent=Mozilla/5.0 (X11; Linux i686; rv:42.0) Gecko/20100101 Firefox/42.0 Iceweasel/42.0";
                }
            }
        }

        private void contextMenuStrip2_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // if (dataGridView1.IsCurrentCellInEditMode)
            if (dataGridView1.EditingControl is TextBox)
            {
                var textBox = (TextBox)dataGridView1.EditingControl;
                if (textBox.Text.EndsWith("m3u8"))
                {
                    contextMenuStrip2.Items[0].Enabled = true;                   
                }
                else
                {
                    contextMenuStrip2.Items[0].Enabled = false;
                }
            }
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 & e.ColumnIndex >= 0 /*& IsSelected*/)
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                string sw = textBox_find.Text;

                if (!string.IsNullOrEmpty(sw))
                {
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
                        if (((e.State & DataGridViewElementStates.Selected) != DataGridViewElementStates.None))
                        {
                            hl_brush = new SolidBrush(Color.DarkGoldenrod);
                        }
                        else
                        {
                            hl_brush = new SolidBrush(Color.Yellow);
                        }

                        e.Graphics.FillRectangle(hl_brush, hl_rect);

                        hl_brush.Dispose();
                    }
                }
                e.PaintContent(e.CellBounds);
            }
        }







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
    public static IEnumerable<DataGridViewRow> GetSelectedRows(this DataGridView source)
    {
        for (int i = source.SelectedRows.Count - 1; i >= 0; i--)
            yield return source.SelectedRows[i];
    }


}



