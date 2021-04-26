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

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaylistEditor
{
    public class CheckList
    {
        public string Url { get; set; }
        public int ErrorCode { get; set; }
    }

    static class ClassHelp
    {
        public static List<CheckList> checkList = new List<CheckList>();

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

        public static FileData GetFileData(string fullstr)
        {
            FileData fileData = new FileData();

            Regex regex1 = new Regex("tvg-name =\"([^\"]*)");
            fileData.Name = regex1.Match(fullstr).Groups[1].ToString().Trim();
            if (string.IsNullOrEmpty(fileData.Name)) fileData.Name = "N/A";

            Regex regex2 = new Regex("tvg-id=\"([^\"]*)");
            fileData.Id = regex2.Match(fullstr).Groups[1].ToString().Trim();
            if (string.IsNullOrEmpty(fileData.Id)) fileData.Id = "N/A";

            Regex regex3 = new Regex("group-title=\"([^\"]*)");
            fileData.Title = regex3.Match(fullstr).Groups[1].ToString().Trim();
            if (string.IsNullOrEmpty(fileData.Title)) fileData.Title = "N/A";

            Regex regex4 = new Regex("tvg-logo=\"([^\"]*)");
            fileData.Logo = regex4.Match(fullstr).Groups[1].ToString().Trim();
            if (string.IsNullOrEmpty(fileData.Logo)) fileData.Logo = "N/A";

            fileData.Name2 = fullstr.Split(',').Last().Trim();
            if (string.IsNullOrEmpty(fileData.Name2)) fileData.Name2 = "N/A";


            return fileData;
            
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

        /// <summary>
        /// checks if File Exists with timeout
        /// </summary>
        /// <param name="uri">full filename</param>
        /// <param name="timeout">timeout</param>
        /// <returns></returns>
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
            string [] registry_key = { @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall" };
            using (var baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                for (int i = 0; i < 2; i++)
                {
                    using (var key = baseKey.OpenSubKey(registry_key[i]))
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
            }
            return  "";  //no vlc found


        }

    /// <summary>
    /// method to check if internet connection is alive
    /// </summary>
    /// <param name="uri">URL to check</param>
    /// <returns>errorcode</returns>
        public static int CheckINetConn(string uri)
        {

            try
            {
               
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri) as HttpWebRequest; 
                req.Timeout = 6000; //set the timeout
           
                req.ContentType = "application/x-www-form-urlencoded";
                //   req.KeepAlive = true;
                //https://deviceatlas.com/blog/list-smart-tv-user-agent-strings
                //issue #15
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; rv:78.0) Gecko/20100101 Firefox/78.0" +
                "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";
                //req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) " +
                //    "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";
                    //+ " AppleTV/tvOS/9.1.1"
                    //+ " AppleCoreMedia/1.0.0.12B466 (Apple TV; U; CPU OS 8_1_3 like Mac OS X; en_us)";

                //req.UserAgent = "Mozilla / 5.0(iPhone; CPU iPhone OS 13_1 like Mac OS X) " +
                //    "AppleWebKit / 605.1.15(KHTML, like Gecko) Version / 13.0.1 Mobile / 15E148";


                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                StreamReader sr = new StreamReader(resp.GetResponseStream());

                char[] buffer = new char[1024];
                int results1 = sr.Read(buffer,0,1023);
                if (System.Diagnostics.Debugger.IsAttached)
                    Console.WriteLine("buffer : {0}", results1);

                sr.Close();
               
            }
            catch (WebException e)  
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        Console.WriteLine("Status Code : {0}", (int)((HttpWebResponse)e.Response).StatusCode);
                        Console.WriteLine("Status Description : {0}", ((HttpWebResponse)e.Response).StatusDescription);
                    }

                    return (int)((HttpWebResponse)e.Response).StatusCode;
                }
                return 401;  //Timeout error
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached)
                    Console.WriteLine("ex Code : {0}", ex.Message);
                return 401;
            }

            return 0;
        }

        /// <summary>
        /// method to check if link is alive and store result in class checkList
        /// </summary>
        /// <param name="uri">link to check</param>
        /// <returns>class checkList</returns>
        public static int CheckIPTVStream2(string uri)
        {
            int errorcode = 0;

            if (uri.StartsWith("rt")) errorcode = 410;  //rtmp check not implemented
            else   //issue #41
            {
                try
                {

                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri) as HttpWebRequest;

                    req.Timeout = Properties.Settings.Default.timeout; //set the timeout #47

                    req.ContentType = "application/x-www-form-urlencoded";
                    //   req.KeepAlive = true;
                    //https://deviceatlas.com/blog/list-smart-tv-user-agent-strings
                    req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; rv:78.0) Gecko/20100101 Firefox/78.0" +
                        "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";

                    //issue #15
                    //req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) " +
                    //    "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";
                    //+ " AppleTV/tvOS/9.1.1"
                    //+ " AppleCoreMedia/1.0.0.12B466 (Apple TV; U; CPU OS 8_1_3 like Mac OS X; en_us)";

                    //req.UserAgent = "Mozilla / 5.0(iPhone; CPU iPhone OS 13_1 like Mac OS X) " +
                    //    "AppleWebKit / 605.1.15(KHTML, like Gecko) Version / 13.0.1 Mobile / 15E148";

                    if (uri.Contains("|User-Agent") && uri.Contains(".m3u8"))  //#18
                    {
                        req.UserAgent = uri.Split('=').Last();
                    }


                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                    StreamReader sr = new StreamReader(resp.GetResponseStream());

                    char[] buffer = new char[1024];
                    int results1 = sr.Read(buffer, 0, 1023);
                    if (System.Diagnostics.Debugger.IsAttached)
                        Console.WriteLine("buffer : {0}", results1);

                    sr.Close();

                }
                catch (WebException e)  //#34
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        if (System.Diagnostics.Debugger.IsAttached)
                        {
                            Console.WriteLine("Status Code : {0}", (int)((HttpWebResponse)e.Response).StatusCode);
                            Console.WriteLine("Status Description : {0}", ((HttpWebResponse)e.Response).StatusDescription);
                        }

                        errorcode = (int)((HttpWebResponse)e.Response).StatusCode;
                    }
                    else errorcode = 401;  //Timeout error
                }
                catch (Exception ex)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                        Console.WriteLine("ex Code : {0}", ex.Message);
                    errorcode = 401;
                }
            }

            checkList.Add(new CheckList
            {
                Url = uri,
                ErrorCode = errorcode
            });



            return 0;
        }


        /// <summary>
        /// load on undo stack
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="dgv"></param>
        /// <returns></returns>
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
            return !Enumerable.Range(0, instance.GetLength(0)).Any(x => !instance[x].SequenceEqual(dgvRows[x]
                .Cells.Cast<DataGridViewCell>().Select(c => c.Value).ToArray()));
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

                    string[] pastedRows = System.Text.RegularExpressions.Regex
                        .Split(o.GetData(DataFormats.UnicodeText).ToString()
                        .TrimEnd("\r\n".ToCharArray()), "\r\n");

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
