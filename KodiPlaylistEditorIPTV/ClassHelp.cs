using Microsoft.Win32;
using System.IO;
using System.Threading.Tasks;

namespace PlaylistEditor
{
    class ClassHelp
    {
        /// <summary>
        /// detects if fietype is video or IPTV
        /// </summary>
        /// <param name="filename">filename</param>
        /// <returns>true if IPTV</returns>
        public static bool FileIsIPTV(string filename)
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

        public static byte[] StringToByteArray(string str)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(str);
        }

        public static async void PopupForm(string label, string color, int delay)
        {
            await PopupDelay(label, color, delay);
            
        }


        /// <summary>
        /// async thread counter to show popup form
        /// </summary>
        /// <param name="item"></param>
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


    }
}
