using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaylistEditor
{
    static class ClassHelp
    {
        /// <summary>
        /// detects if fietype is video or IPTV
        /// </summary>
        /// <param name="filename">filename</param>
        /// <returns>true if IPTV</returns>
        public static bool FileIsIPTV(string filename)
        {
            try
            {
                string line;
                using (StreamReader playlistFile = new StreamReader(filename))
                {
                    while ((line = playlistFile.ReadLine()) != null)
                    {
                        if (line.StartsWith("#EXTM3U"))
                        {
                            return true;  //is IPTV
                        }
                        else if (line.StartsWith("#EXTCPlayListM3U::M3U"))
                        {
                            return false;  //is Video
                        }

                    }
                    return false;
                }
            }
            catch (Exception )
            {
                return false;
            }
            

        }

        /// <summary>
        /// returns string between 2 stings
        /// </summary>
        /// <param name="fullstr"></param>
        /// <param name="startstr"></param>
        /// <param name="endstr"></param>
        /// <returns>substring between 2 strings</returns>
        public static string GetPartString(string fullstr, string startstr, string endstr)
        {
            int start, end;
            if (fullstr.Contains(startstr) && fullstr.Contains(endstr))
            {
                start = fullstr.IndexOf(startstr, 0) + startstr.Length;
                end = fullstr.IndexOf(endstr, start);
                return fullstr.Substring(start, end - start);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// byte to string / string to byte
        /// </summary>
        /// <param name="arr"></param>
        /// <returns>string</returns>
        public static string ByteArrayToString(byte[] arr)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(arr);
        }

        //public static byte[] StringToByteArray(string str)
        //{
        //    System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
        //    return enc.GetBytes(str);
        //}

        /// <summary>
        /// shows a popup form
        /// </summary>
        /// <param name="label">text to show</param>
        /// <param name="color">green OR blue OR red</param>
        /// <param name="delay">show time</param>
        public static async void PopupForm(string label, string color, int delay)
        {
            await PopupDelay(label, color, delay);
            
        }


        /// <summary>
        /// async thread counter to show popup form
        /// </summary>
        /// <param name="item">text, color[green,blue,red], showtime</param>
        public static async Task PopupDelay(string label, string color, int delay)
        {
            popup2 pop = new popup2();
            pop.Show();
            pop.label1.Text = label;
            pop.color(color);

            await Task.Delay(delay);

            pop.Close();

        }

        /// <summary>
        /// checks if Diectory exists with timeout
        /// </summary>
        /// <param name="openpath">path</param>
        /// <param name="timeout">timeout</param>
        /// <returns>true/false</returns>
        public static bool MyDirectoryExists(string openpath, int timeout)
        {
            var task = new Task<bool>(() => { var info = new DirectoryInfo(openpath); return info.Exists; });
            task.Start();

            return task.Wait(timeout) && task.Result;

        }

        public static bool MyFileExists(string uri, int timeout)
        {
            var task = new Task<bool>(() =>
            {
                var fi = new FileInfo(uri);
                return fi.Exists;
            });
            task.Start();
            //return task.Result;
            return task.Wait(timeout) && task.Result;
        }
        /// <summary>
        /// function to get the path of installed vlc
        /// </summary>
        /// <returns>path or empty</returns>
        public static string GetVlcPath()
        {
            object line;
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (var baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (var key = baseKey.OpenSubKey(registry_key))
                {
                    foreach (string subkey_name in key.GetSubKeyNames())
                    {
                        using (var subKey = key.OpenSubKey(subkey_name))
                        {
                            line = subKey.GetValue("DisplayName");
                            if (line != null && (line.ToString().ToUpper().Contains("VLC")))
                            {

                                string VlcPath = subKey.GetValue("InstallLocation").ToString();

                                Properties.Settings.Default.vlcpath = VlcPath;
                                Properties.Settings.Default.Save();
                                return VlcPath;
                                
                            }
                        }
                    }
                }
            }
            return  "";  //no vlc found
           
        }

    /// <summary>
    /// method to get first 1k of stream data to check if stream alive
    /// </summary>
    /// <param name="uri">URL to check</param>
    /// <returns>bool</returns>
        public static bool CheckIPTVStream(string uri)
        {
            try
            {
               
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri) as HttpWebRequest; 
                req.Timeout = 6000; //set the timeout
           
                req.ContentType = "application/x-www-form-urlencoded";
             //   req.KeepAlive = true;
          //issue #15
                req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";

                if (uri.Contains("|User-Agent") && uri.Contains(".m3u8"))
                {//#18
                    req.UserAgent = uri.Split('=').Last();
                }


                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                StreamReader sr = new StreamReader(resp.GetResponseStream());
                // results = sr.ReadToEnd();
                char[] buffer = new char[1024];
                int results1 = sr.Read(buffer,0,1023);
                sr.Close();

               
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static bool LoadItem(this Stack<object[][]> instance, DataGridView dgv)
        {
            if (instance.Count == 0)
            {
                return true;
            }
            object[][] rows = instance.Peek();
            return !ItemEquals(rows, dgv.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).ToArray());
        }

        public static bool ItemEquals(this object[][] instance, DataGridViewRow[] dgvRows)
        {
            if (instance.Count() != dgvRows.Count())
            {
                return false;
            }
            return !Enumerable.Range(0, instance.GetLength(0)).Any(x => !instance[x].SequenceEqual(dgvRows[x].Cells.Cast<DataGridViewCell>().Select(c => c.Value).ToArray()));
        }

        /// <summary>
        /// checks if a full row (6) is in clipboard
        /// </summary>
        /// <returns></returns>
        public static bool CheckClipboard()
        {
            DataObject o = (DataObject)Clipboard.GetDataObject();

            if (Clipboard.ContainsText())
            {
                try
                {

                    string[] pastedRows = System.Text.RegularExpressions.Regex.Split(o.GetData(DataFormats.UnicodeText).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                    string[] pastedRowCells = pastedRows[0].Split(new char[] { '\t' });

                    if (pastedRowCells.Length == 6)  return true;  
                    // check for visible rows
                   

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Paste operation failed. " + ex.Message, "Copy/Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            return false;
        }



        //here new methods
    }

  

}
