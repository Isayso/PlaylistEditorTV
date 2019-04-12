using Microsoft.Win32;
using System.IO;

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
        /// function to get the path of installed vlc
        /// </summary>
        /// <returns>path or empty</returns>
        public static string GetVlcPath()
        {
            string VlcPath = "";
            object line;
            string softwareinstallpath = string.Empty;
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

                                VlcPath = subKey.GetValue("InstallLocation").ToString();
                                
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
