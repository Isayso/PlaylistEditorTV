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




      

    }
}
