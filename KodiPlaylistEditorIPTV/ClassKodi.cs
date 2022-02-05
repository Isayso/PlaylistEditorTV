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
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaylistEditor
{
    internal class ClassKodi
    {
        public static async Task<bool> RunOnKodi(string link)
        {
            string kodiIP = Settings.Default.rpi;
            string kodiUser = Settings.Default.username.Trim();
            string kodiPort = Settings.Default.port;
            //  string kodiPass = Properties.Settings.Default.password; https://stackoverflow.com/questions/12657792/how-to-securely-save-username-password-local
            byte[] plaintext = null;
            string kodiPass = "";

            if (Settings.Default.cipher != null && Settings.Default.entropy != null)
            {
                plaintext = ProtectedData.Unprotect(Settings.Default.cipher, Settings.Default.entropy,
                                                    DataProtectionScope.CurrentUser);
                kodiPass = ClassHelp.ByteArrayToString(plaintext);
            }

            kodiPass = kodiPass.Trim();

            var values = new Dictionary<string, string>
            {
                {kodiUser,kodiPass}
            };

            string url = "http://" + kodiIP + ":" + kodiPort + "/jsonrpc?request=";

            //url = "http://192.168.178.91:8080/jsonrpc"; //?request=";

            try
            {
                using (var webClient = new WebClient())
                {
                    // Required to prevent HTTP 401: Unauthorized messages
                    webClient.Credentials = new NetworkCredential(kodiUser, kodiPass);
                    // API Doc: http://kodi.wiki/view/JSON-RPC_API/v6
                    //  var json = "{\"jsonrpc\":\"2.0\",\"method\":\"GUI.ShowNotification\",\"params\":{\"title\":\"This is the title of the message\",\"message\":\"This is the body of the message\"},\"id\":1}";
                    string response = webClient.UploadString($"http://{kodiIP}:{kodiPort}/jsonrpc", "POST", link);

                    if (response.Contains("OK") /*&& link.Contains("Playlist.Add")*/)
                    {
                        NotificationBox.Show(Mess.Kodi_response__OK, 1300, NotificationMsg.OK);

#if DEBUG
                        MessageBox.Show(response);
                        Console.WriteLine(response);
                        Console.ReadLine();
#endif
                    }
                    else if (response.Contains("error") /*&& link.Contains("Playlist.Add")*/)
                    {
                        NotificationBox.Show(Mess.Kodi_response__ERROR, 1300, NotificationMsg.ERROR);

                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Mess.Kodi_not_responding + ex.Message, Mess.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
    }
}