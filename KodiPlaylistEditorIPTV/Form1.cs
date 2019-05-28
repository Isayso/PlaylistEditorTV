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
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;





namespace PlaylistEditor
{
    public partial class Form1 : Form
    {
        bool isModified = false;

      
        public string fileName = "";
        public string line;       
        private string path;
       
        public bool _isIt = true;
        public bool _found = false;
           

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataRow dr;

        string vlcpath = Properties.Settings.Default.vlcpath;




        public Form1()
        {
            InitializeComponent();

            this.Text = String.Format("PlaylistEditor TV " + " v{0}" , Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0, 5));

            var spec_key = Properties.Settings.Default.specKey;
            var hotlabel = Properties.Settings.Default.hotkey;
           
            
           
            if (!string.IsNullOrEmpty(vlcpath))
            {
                button_vlc.Visible = true;
            }
            else if (string.IsNullOrEmpty(vlcpath))  //first run
            {
                vlcpath = ClassHelp.GetVlcPath();
                if (string.IsNullOrEmpty(vlcpath)) button_vlc.Visible = false; //no vlc installed
            }
           

            plabel_Filename.Text = "";
            button_revert.Visible = false;


            //  dataGridView1.AllowUserToAddRows = true;


            //command line arguments [1]
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1) //drag drop
            {
                plabel_Filename.Text = args[1];
                importDataset(args[1], false);
                button_revert.Visible = true;
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
                        case Keys.C:
                            toolStripCopy.PerformClick();
                            break;

                        case Keys.V:
                            toolStripPaste.PerformClick();
                            break;
                      
                        case Keys.F:
                            button_search.PerformClick();
                            break;

                        case Keys.R:
                            copyRowMenuItem.PerformClick();
                            break;

                        case Keys.I:
                            pasteRowMenuItem.PerformClick();
                            break;

                        case Keys.X:
                            cutRowMenuItem.PerformClick();
                            break;

                        case Keys.P:
                            playToolStripMenuItem.PerformClick();
                            break;
                    }
                }
                if (e.KeyCode == Keys.Delete)
                {
                    button_delLine.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Copy/paste operation failed. " + ex.Message, "Copy/Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

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
            Cursor.Current = Cursors.WaitCursor;
            string openpath = Properties.Settings.Default.openpath;
            if (!string.IsNullOrEmpty(openpath) && !ClassHelp.MyDirectoryExists(openpath, 4000)) openpath = "c:\\";

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.InitialDirectory = openpath;
                openFileDialog1.RestoreDirectory = false;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    importDataset(openFileDialog1.FileName, false);
                    button_revert.Visible = true;
                }
                else  //cancel
                {
                    return;
                }
                
                Properties.Settings.Default.openpath = Path.GetDirectoryName(openFileDialog1.FileName);
                Properties.Settings.Default.Save();
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

                if (line.StartsWith("#EXTINF"))
                {
                   
                    col[0] = ClassHelp.GetPartString(line, "tvg-name=\"", "\"");
                 
                    if (col[0] == "") col[0] = "Name N/A";
                    

                     col[1] = ClassHelp.GetPartString(line, "tvg-id=\"", "\"");
                 
                    if (col[1] == "") col[1] = "id N/A";
                   

                     col[2] = ClassHelp.GetPartString(line, "group-title=\"", "\"");
                   
                    if (col[2] == "") col[2] = "Title N/A";
                    

                     col[3] = ClassHelp.GetPartString(line, "tvg-logo=\"", "\"");
                   
                    if (col[3] == "") col[3] = "logo N/A";

                  
                     col[4] = line.Split(',').Last();
                    
                    if (col[4] == "") col[4] = "Name2 N/A";

                    continue;

                }

                else if (line.StartsWith("ht") && (line.Contains("//") || line.Contains(":\\")))
                {
                     col[5] = line;
                }
                //else if (line.StartsWith("#") || line.StartsWith("."))
                //{

                //    continue;
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
                    catch (System.ArgumentOutOfRangeException)
                    {
                        MessageBox.Show("Argument out of range error. Wrong format.");
                        continue;
                    }
            }
            playlistFile.Close();

            dataGridView1.AllowUserToAddRows = false;

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Wrong file structure! ", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }

        private void button_save_Click(object sender, EventArgs e)  
        {
            saveFileDialog1.FileName = plabel_Filename.Text;

            if (Control.ModifierKeys == Keys.Shift && !string.IsNullOrEmpty(plabel_Filename.Text))
            {
                // ((Control)sender).Hide();

                using (StreamWriter file = new StreamWriter(saveFileDialog1.FileName, false /*, Encoding.UTF8*/))   //false: file ovewrite
                {

                    file.NewLine = "\n";  // win: LF
                    file.WriteLine("#EXTM3U");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        file.WriteLine("#EXTINF:-1 tvg-name=\"" + dt.Rows[i][0] + "\" tvg-id=\"" + dt.Rows[i][1] + "\" group-title=\"" + dt.Rows[i][2] + "\" tvg-logo=\"" + dt.Rows[i][3] + "\"," + dt.Rows[i][4]);
                        file.WriteLine(dt.Rows[i][5]);

                    }

                }
                toSave(false);
                button_revert.Visible = true;
                
            }

            else if (saveFileDialog1.ShowDialog() == DialogResult.OK)  
            {
                plabel_Filename.Text = saveFileDialog1.FileName;
               // try
               // {
                    using (StreamWriter file = new StreamWriter(saveFileDialog1.FileName, false /*, Encoding.UTF8*/))   //false: file ovewrite
                    {
                        
                        file.NewLine = "\n";  // win: LF
                        file.WriteLine("#EXTM3U");

                        for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //ToDo # remove, "," remove?
                        //dt.Rows[i][0].Replace("#", " ").Replace(",", " ");
                        file.WriteLine("#EXTINF:-1 tvg-name=\"" + dt.Rows[i][0] + "\" tvg-id=\"" + dt.Rows[i][1] + "\" group-title=\"" + dt.Rows[i][2] + "\" tvg-logo=\"" + dt.Rows[i][3] + "\"," + dt.Rows[i][4]);
                            file.WriteLine(dt.Rows[i][5]);
                            
                        }
                     
                    }
                    
                    toSave(false);
                //}
                //catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException)
                //{
                //    MessageBox.Show("Write Error " + ex);
                //}
                button_revert.Visible = true;
            }
        }



        private void button_moveUp_Click(object sender, EventArgs e)
        {
            MoveLine(-1); 
        }

        private void button_moveDown_Click(object sender, EventArgs e)
        {           
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
            
            string param = "";

            if (dataGridView1.RowCount > 0 && !string.IsNullOrEmpty(vlcpath))
            {

                // Set cursor as hourglass
                Cursor.Current = Cursors.WaitCursor;

                param = dataGridView1.CurrentRow.Cells[5].Value.ToString();
               
                System.Diagnostics.ProcessStartInfo ps = new System.Diagnostics.ProcessStartInfo();
                ps.FileName = vlcpath + "\\" + "vlc.exe";
                ps.ErrorDialog = false;
                ps.Arguments =  " " + param;
                
                ps.CreateNoWindow = true; 
                ps.UseShellExecute = false; 

                ps.RedirectStandardOutput = true; 
                ps.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden; 

                using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
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
            if (dataGridView1.RowCount > 0)
            {
                switch (MessageBox.Show("Delete List?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
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
            //message box -> delete all -> open filename
            switch (MessageBox.Show("Reload File?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
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

            //  if (ClassHelp.PingHost(rpi_ip,22))
            await ClassKodi.Run(jLink);

        }

        private void copyRowMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            //copy selection to whatever
            if (dataGridView1.CurrentCell.Value != null && dataGridView1.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {

                dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected = true;

                try
               {
                    // Add the selection to the clipboard.
                   
                    Clipboard.SetDataObject(this.dataGridView1.GetClipboardContent());
#if DEBUG
                    Console.WriteLine(Clipboard.GetText());  
#endif
               }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    MessageBox.Show("The Clipboard could not be accessed. Please try again.");
                    Clipboard.Clear();
                }
            }
        }

        private void pasteRowMenuItem_Click(object sender, EventArgs e)
        {
            
            bool _dtEmpty = false;

            if (dataGridView1.RowCount == 0)
            {
               _dtEmpty = true;
                DataRow dr = dt.NewRow();
              
                dt.Columns.Add("Name"); dt.Columns.Add("id"); dt.Columns.Add("Title");
                dt.Columns.Add("logo"); dt.Columns.Add("Name2"); dt.Columns.Add("Link");
                dataGridView1.DataSource = dt;

            }

            DataObject o = (DataObject)Clipboard.GetDataObject();
            
           
            if (Clipboard.ContainsText())
            {
                try
                {
                    int a = 0;
                    if (!_dtEmpty) a = dataGridView1.SelectedCells[0].RowIndex;  //select row in a datatable

                    string[] pastedRows = Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                    foreach (string pastedRow in pastedRows)
                    {
                        string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });

                        for (int i = 0; i < pastedRowCells.Length; i++)
                        {
                            if (pastedRowCells[i] != "")
                            {
                                dr = dt.NewRow();
                                dr["Name"] = pastedRowCells[i]; dr["id"] = pastedRowCells[i + 1]; dr["Title"] = pastedRowCells[i + 2];
                                dr["logo"] = pastedRowCells[i + 3]; dr["Name2"] = pastedRowCells[i + 4]; dr["Link"] = pastedRowCells[i + 5];

                                if (_dtEmpty)
                                {
                                    dt.Rows.Add(dr);
                                }
                                else
                                {

                                    dt.Rows.InsertAt(dr, a);  
                                }

                                i += 5;
                            }

                        }

                    }
                    toSave(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Paste operation failed. " + ex.Message, "Copy/Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }







        private void cutRowMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            //copy selection to whatever
            if (dataGridView1.CurrentCell.Value != null && dataGridView1.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {

               
                dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Selected = true;

                try
                {
                    // Add the selection to the clipboard.

                    Clipboard.SetDataObject(this.dataGridView1.GetClipboardContent());
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

        private void toolStripCopy_Click(object sender, EventArgs e)
        {
           
            //Copy to clipboard
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void toolStripPaste_Click(object sender, EventArgs e)   //ctrl+v
        {
           
            if (dataGridView1.RowCount == 0)       // when empty
            {
                pasteRowMenuItem.PerformClick();
                return;
            }

             

            int leftshift = Properties.Settings.Default.leftshift;
            try
            {
                string s = Clipboard.GetText();
                string[] lines = s.Split('\n');

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

                    int iNewRows = iRow + lines.Length - dataGridView1.Rows.Count;
                    if (iNewRows > 0)     
                    {
                        if (bFlag)
                            dataGridView1.Rows.Add(iNewRows);
                        else
                            dataGridView1.Rows.Add(iNewRows + 1);
                    }
                    else if (iNewRows == 0 && iRow != dataGridView1.Rows.Count -1 )
                        dataGridView1.Rows.Add(iNewRows + 1);  
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
                                oCell.Value = Convert.ChangeType(sCells[i].Replace("\r", "").Remove(0, leftshift), oCell.ValueType);
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
                Clipboard.Clear();
            }
            catch (FormatException)
            {
                MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
            }


        }



        private void textBox_selectAll_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;
            textBox.SelectAll();
        }

        private void textBox_find_TextChanged(object sender, EventArgs e)
        {
            var colS = Properties.Settings.Default.colSearch;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.ClearSelection();
                _found = false;

                string _name ="";

               
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                   
                    if (row.Cells[0].Value != null)
                    _name = dt.Rows[row.Index][colS].ToString().ToLower();
                   

                    if (_name.Contains(textBox_find.Text.ToLower()) && textBox_find.Text != "")
                    {
                        dataGridView1.Rows[row.Index].Selected = true;

                      
                        _found = true;
                        textBox_find.ForeColor = System.Drawing.Color.Black;
                    }


                }
                if (!_found)//text red 
                    textBox_find.ForeColor = System.Drawing.Color.Red;
            }


        }



        private void button_dup_Click(object sender, EventArgs e)
        {

            var colD = Properties.Settings.Default.colDupli;

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

            if (Control.ModifierKeys == Keys.Shift)
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
        /// move the marked line up or down
        /// </summary>
        /// <param name="direction">-1 up 1 down</param>
        public void MoveLine(int direction)
        {
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
                    dataGridView1.CurrentCell = dataGridView1.Rows[row.Index + direction].Cells[0];  //scroll automatic to cell
                }
                toSave(true);
            }  
        }
        /// <summary>
        /// changes icon if file is modified
        /// </summary>
        public void toSave(bool hasChanged)
        {
            isModified = hasChanged;

            if (hasChanged)
                button_save.BackgroundImage = Properties.Resources.content_save_modified;

            if (!hasChanged)
                button_save.BackgroundImage = Properties.Resources.content_save_1_;

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            toSave(true);
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            toSave(true);
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                button_open.PerformClick();
            }
                
        }



        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.RowCount > 0 && !string.IsNullOrEmpty(vlcpath)) button_vlc.PerformClick();
        }
    }
}

