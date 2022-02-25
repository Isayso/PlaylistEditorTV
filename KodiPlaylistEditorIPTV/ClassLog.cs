using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistEditor
{
    internal static class ClassLog
    {
        public class LogWriter
        {
            private static string m_exePath = string.Empty;
            public LogWriter(string logMessage)
            {
                LogWrite(logMessage);
            }
            public static void LogWrite(string logMessage)
            {
                m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                try
                {
                    using (StreamWriter w = File.AppendText(m_exePath + "\\" + "PlaylistEditorlog.txt"))
                    {
                        Log(logMessage, w);
                    }
                }
                catch (Exception ex)
                {
                }
            }

            public static void Log(string logMessage, TextWriter txtWriter)
            {
                try
                {
                    txtWriter.Write("\r\nLog Entry : ");
                    txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                        DateTime.Now.ToLongDateString());
                    txtWriter.WriteLine("  :");
                    txtWriter.WriteLine("  :{0}", logMessage);
                    txtWriter.WriteLine("-------------------------------");
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
