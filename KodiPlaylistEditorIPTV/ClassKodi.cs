// GNU GENERAL PUBLIC LICENSE                       Version 3, 29 June 2007
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace PlaylistEditor
{
    class ClassKodi
    {
        private static readonly HttpClient _Client = new HttpClient();


        public static async Task Run(string link)
        {
            string kodiIP = Properties.Settings.Default.rpi;
            string kodiUser = Properties.Settings.Default.username;
            string kodiPort = Properties.Settings.Default.port;
            //  string kodiPass = Properties.Settings.Default.password; https://stackoverflow.com/questions/12657792/how-to-securely-save-username-password-local
            byte[] plaintext = null;
            string kodiPass = "";

            if (Properties.Settings.Default.cipher != null && Properties.Settings.Default.entropy != null)
            {
                plaintext = ProtectedData.Unprotect(Properties.Settings.Default.cipher, Properties.Settings.Default.entropy,
                                                    DataProtectionScope.CurrentUser);
                kodiPass = ClassHelp.ByteArrayToString(plaintext);
            }



            var values = new Dictionary<string, string>
            {
                { kodiUser,kodiPass}

            };

            string url = "http://" + kodiIP + ":" + kodiPort + "/jsonrpc?request=";


            //url = "http://192.168.178.91:8080/jsonrpc"; //?request=";

            try
            {
                var response = await Request(HttpMethod.Post, url, link, values);
                string responseText = await response.Content.ReadAsStringAsync();

                if (responseText.Contains("OK") /*&& link.Contains("Playlist.Add")*/)
                {
                    // Form pop = new popup2();
                    ClassHelp.PopupForm("Kodi response: OK", "green", 1800);




#if DEBUG
                    MessageBox.Show(responseText);
                    Console.WriteLine(responseText);
                    Console.ReadLine();
#endif


                }
                else if (responseText.Contains("error") /*&& link.Contains("Playlist.Add")*/)
                {
                    ClassHelp.PopupForm("Kodi response: ERROR", "red", 1300);


                }

                kodiPass = "";  //to be safe
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kodi not responding. " + ex.Message, "Play", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }



        }


        /// <summary>
        /// Makes an async HTTP Request
        /// </summary>
        /// <param name="pMethod">Those methods you know: GET, POST, HEAD, etc...</param>
        /// <param name="pUrl">Very predictable...</param>
        /// <param name="pJsonContent">String data to POST on the server</param>
        /// <param name="pHeaders">If you use some kind of Authorization you should use this</param>
        /// <returns></returns>
        static async Task<HttpResponseMessage> Request(HttpMethod pMethod, string pUrl, string pJsonContent, Dictionary<string, string> pHeaders)
        {
            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = pMethod;
            httpRequestMessage.RequestUri = new Uri(pUrl);
            foreach (var head in pHeaders)
            {
                httpRequestMessage.Headers.Add(head.Key, head.Value);
            }
            switch (pMethod.Method)
            {
                case "POST":
                    HttpContent httpContent = new StringContent(pJsonContent, Encoding.UTF8, "application/json");

                    httpRequestMessage.Content = httpContent;
                    break;

            }

            return await _Client.SendAsync(httpRequestMessage);
        }

    }

}
