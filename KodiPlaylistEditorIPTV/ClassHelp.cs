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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
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
    public class ColList
    {
        public string Name { get; set; }
        public bool Visible { get; set; }
    }
    internal static class ClassHelp
    {
        public static List<CheckList> checkList = new List<CheckList>();
        public static List<ColList> columnList = new List<ColList>();

        public static string fileHeader = null;

        public static string Message { get; private set; }
        public static object AfterReceive { get; private set; }


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
            string[] registry_key = { @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall" };
            try  //issue #58
            {
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
            }
            catch
            {
                return "";  //no vlc found
            }
            return "";  //no vlc found
        }

        /// <summary>
        /// check if internet connection is alive
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
                int results1 = sr.Read(buffer, 0, 1023);
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
        /// check if link is alive and store result in class checkList
        /// </summary>
        /// <param name="uri">link to check</param>
        /// <returns>class checkList</returns>
        public static int CheckIPTVStream(string uri)
        {
            int errorcode = 0;

            if (uri.StartsWith("rt") /*|| uri.StartsWith("ud")*/) errorcode = 410;  //rtmp check not implemented  issue #61
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
        /// checks if a full row is in clipboard
        /// </summary>
        /// <returns></returns>
        public static bool CheckClipboard()
        {
            try
            {
                DataObject o = (DataObject)Clipboard.GetDataObject();

                if (Clipboard.ContainsText())
                {
                    string content = o.GetData(DataFormats.UnicodeText).ToString();
                    if (content.StartsWith("FULLROW"))
                    {
                        return true;
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Paste operation failed. (check clip) " + ex.Message, "Copy/Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            return false;
        }

        //public static bool CheckClipboard3(List<string> cols)
        //{
        //    DataObject o = (DataObject)Clipboard.GetDataObject();

        //    if (Clipboard.ContainsText())
        //    {
        //        try
        //        {
        //            string[] pastedRows = System.Text.RegularExpressions.Regex
        //                .Split(o.GetData(DataFormats.UnicodeText).ToString()
        //                .TrimEnd("\r\n".ToCharArray()), "\r\n");

        //            pastedRows[0] = pastedRows[0].Trim('\t');

        //            string[] pastedRowCells = pastedRows[0].Split(new char[] { '\t' });  //TODO 0 or 1

        //            if (cols.Count.Equals(0))
        //            {
        //                List<ColList> elements = new List<ColList>();
        //                elements = ClassHelp.SeekFileElements(pastedRows[0]);

        //                if (pastedRowCells.Length == elements.Count + 2) return true;   //TODO more columns

        //            }
        //            else
        //            {
        //                if (pastedRowCells.Length == cols.Count) return true;
        //            }

        //            // if (pastedRowCells.Length == 6) return true;   //TODO more columns
        //            // if (pastedRowCells.Length == cols.Count) return true;   //TODO more columns
        //            // check for visible rows
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Paste operation failed. (check clip) " + ex.Message, "Copy/Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }
        //    }

        //    return false;
        //}


        /// <summary>
        /// get stream name with ffprobe
        /// </summary>
        /// <param name="linkUrl">link url</param>
        /// <param name="ffprobepath">full path to ffprobe.exe</param>
        /// <returns>Stream_name tag</returns>
        public static string GetFFrobeStreamName(string linkUrl, string ffprobepath)
        {
            string method = string.Format("{0}.{1}",
              MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name);

            if (linkUrl.StartsWith("ud"))
            {
#if DEBUG
                //bool udpanswer = UDPSentRec(linkUrl);
                //ClassLog.LogWriter.LogWrite(method + "\n UDP Answer 1 " + udpanswer);

                bool udpanswer = ReceiveUDP2(linkUrl);
                ClassLog.LogWriter.LogWrite(method + "\n UDP Answer 1 " + udpanswer);
                bool udpanswer2 = ReceiveUDP(linkUrl);
                ClassLog.LogWriter.LogWrite(method + "\n UDP Answer 2 " + udpanswer2);
                if (!udpanswer && !udpanswer2) return null;

#else

             //   if (!UDPSentRec(linkUrl)) 
                    return null;

#endif

            }

            string output = null, streamName = null;

            try
            {
                //* Create Process
                using (Process proc = new Process())
                {
                    proc.StartInfo.FileName = "cmd.exe";
                    proc.StartInfo.Arguments = "/c " + ffprobepath + " -v quiet -print_format json -show_programs \"" + linkUrl + "\"";
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                    proc.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;

                    //  var encod = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage);


                    proc.ErrorDataReceived += new DataReceivedEventHandler(ErrorOutputHandler);

                    proc.Start();

                    //* Read one element asynchronously
                    proc.BeginErrorReadLine();

                    //* Read the other one synchronously
                    output = proc.StandardOutput.ReadToEnd().Replace("\r\n", "");

                    if (proc.WaitForExit(6000))
                    {
#if DEBUG

                        ClassLog.LogWriter.LogWrite(method + "\n Wait for exit OK! " + linkUrl);
#endif

                        Console.WriteLine("OK!");

                    }
                    //else Console.WriteLine("Timeout!");
                    else ClassLog.LogWriter.LogWrite("Timeout!");

                    if (!proc.HasExited)
                    {
                        if (!proc.Responding)
                            proc.Kill();
                        ClassLog.LogWriter.LogWrite(method + "\n killed! " + linkUrl);

                    }
#if DEBUG
                    ClassLog.LogWriter.LogWrite(output);
#endif



                }

                Regex regex1 = new Regex("service_name\": \"([^\"]*)");
                streamName = regex1.Match(output).Groups[1].ToString().Trim();  //[0] codec_long_name + result   [1] result

#if DEBUG
                //regex1 = new Regex("codec_name\": \"([^\"]*)");
                //streamName = regex1.Match(output).Groups[1].ToString().Trim();  //[0] codec_long_name + result   [1] result

                //  MessageBox.Show( output /*StreamName*/, "Key press", MessageBoxButtons.OK, MessageBoxIcon.None);
#endif

            }
            catch
            {
                return streamName = "";
            }

            return streamName; // StreamName;

        }


        static void ErrorOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            Console.WriteLine(outLine.Data);
        }

       
        static bool ReceiveUDP(string udpLink)
        {
            var match = Regex.Match(udpLink, @"\b(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\b:\d{1,5}");
            string udpIP = match.Captures[0].Value;

            Int32.TryParse(udpIP.Split(':')[1], out int udpport);
            var ipAddress = udpIP.Split(':')[0];



            try
            {
                IPAddress mcastAddress = IPAddress.Parse(ipAddress);
                int mcastPort = udpport;

                ClassUDP2.TestMulticastOption s = new ClassUDP2.TestMulticastOption();
                // Start a multicast group.
                s.StartMulticast();

                s.mcastPort = udpport;
                s.mcastAddress = mcastAddress;

                // Display MulticastOption properties.
                s.MulticastOptionProperties();

                // Receive broadcast messages.
                s.ReceiveBroadcastMessages();


                ClassUDP.UDPSocket t = new ClassUDP.UDPSocket();
                t.Server(ipAddress, udpport);

                ClassUDP.UDPSocket c = new ClassUDP.UDPSocket();
                c.Client(ipAddress, udpport);
                c.Send("TEST!");

            }
            catch
            {
                return false;
            }


            return true;
        }
        static bool ReceiveUDP2(string udpLink)
        {
            var match = Regex.Match(udpLink, @"\b(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\b:\d{1,5}");
            string udpIP = match.Captures[0].Value;

            Int32.TryParse(udpIP.Split(':')[1], out int udpport);
            var ipAddress = udpIP.Split(':')[0];

            try
            {
                Socket sock = new Socket(AddressFamily.InterNetwork,
                            SocketType.Dgram, ProtocolType.Udp);

                sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                sock.LingerState = new LingerOption(true, 0);

                Console.WriteLine("Ready to receive…");
                IPEndPoint iep = new IPEndPoint(IPAddress.Any, udpport);
                EndPoint ep = (EndPoint)iep;
                sock.Bind(iep);
                sock.SetSocketOption(SocketOptionLevel.IP,
                           SocketOptionName.AddMembership,
                           new MulticastOption(IPAddress.Parse(ipAddress)));

                byte[] data = new byte[1024];

                while (true)
                {
                    // this blocks until some bytes are received
                    int recv = sock.ReceiveFrom(data, ref ep);
                    string stringData = Encoding.ASCII.GetString(data, 0, recv);
                    Console.WriteLine("received: {0} from: {1}", stringData, ep.ToString());
                }
            }
            catch
            {
                return false;
            }


            return true;
        }
        static bool ReceiveUDP3(string udpLink)
        {
            var match = Regex.Match(udpLink, @"\b(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\b:\d{1,5}");
            string udpIP = match.Captures[0].Value;

            Int32.TryParse(udpIP.Split(':')[1], out int udpport);
            var ipAddress = udpIP.Split(':')[0];

            try
            {
                ClassUDP.UDPSocket s = new ClassUDP.UDPSocket();
                s.Server(ipAddress, udpport);

                ClassUDP.UDPSocket c = new ClassUDP.UDPSocket();
                c.Client(ipAddress, udpport);
                c.Send("TEST!");

            }
            catch
            {
                return false;
            }


            return true;
        }

        /// <summary>
        /// get Path for ffprobe
        /// </summary>
        /// <returns>location of ffprobe</returns>
        public static string GetFfprobePath()
        {
            string ffpPath = NativeMethods.GetFullPathFromWindows("ffprobe.exe");

            if (!string.IsNullOrEmpty(ffpPath))
            {
                return ffpPath;
            }
            else if (File.Exists(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ffprobe.exe"))
            {
                return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ffprobe.exe";
            }
            return null;
        }


        public static string GetFFrobeStreamName2(string linkUrl, string ffprobepath)
        {
            string output = null;

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/c " + ffprobepath + " -v quiet -print_format json -show_programs \"" + linkUrl + "\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                //  process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage);  //russia: 866
                process.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                process.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;



                process.EnableRaisingEvents = true;

                process.Exited += process_Exited;
                process.OutputDataReceived += ProccesOutputDataReceived;
                process.ErrorDataReceived += ProccesErrorDataReceived;

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }

            return output;

            void process_Exited(object sender, EventArgs e)
            {
                // Handle exit here
            }

            void ProccesErrorDataReceived(object sender, DataReceivedEventArgs e)
            {
                // Handle error here
            }

            void ProccesOutputDataReceived(object sender, DataReceivedEventArgs e)
            {
                // Handle output here using e.Data
                //output = e.Data.Append.Replace("\r\n", "");
                while (e.Data != null)
                    output += e.Data;

                //  return output;


            }

        }

        public static List<ColList> SeekFileElements(string fullstr)
        {
            columnList.Clear();

            string[] regArray = { "tvg-name", "tvg-id", "tvg-title", "tvg-logo", "tvg-chno", "tvg-shift",
                "group-title", "radio", "catchup", "catchup-source", "catchup-days", "catchup-correction",
                "provider", "provider-type", "provider-logo", "provider-countries", "provider-languages",
                "media", "media-dir", "media-size"};

            for (int i = 0; i < regArray.Length; i++)
            {
                if (fullstr.ContainsElement(regArray[i] + "=\"([^\"]*)"))
                {

                    columnList.Add(new ColList
                    {
                        Name = regArray[i],
                        Visible = true,
                    });

                }

            }

            return columnList;
        }

        private static bool ContainsElement(this string input, string regString)
        {
            var match = Regex.Match(input, regString);

            if (match.Success) return true;

            return false;
        }



        //here new methods
    }
}