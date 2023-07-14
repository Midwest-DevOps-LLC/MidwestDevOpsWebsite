using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.Utility.Standard
{
    public static class LogHandler
    {
        internal static string _LogPath
        {
            get; set;
        }

        public static string LogPath
        {
            get
            {
                if (String.IsNullOrEmpty(_LogPath))
                {
                    return "log.log";
                }
                else
                {
                    return _LogPath;
                }
            }
            set
            {
                _LogPath = value;
            }
        }

        public static void SaveLog(Log log)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(LogPath, true))
            {
                sw.WriteLine("# " + log.time + " #");

                if (!String.IsNullOrEmpty(log.text))
                    sw.WriteLine(log.text);

                if (log.exception != null)
                    sw.WriteLine(log.exception.Message + log.exception.StackTrace + log.exception.Source + log.exception.InnerException + log.exception.TargetSite);

                sw.Close();
            }
        }

        public class Log
        {
            public DateTime time = DateTime.UtcNow;
            public string text;
            public Exception exception;
        }

        public static void SaveException(this Exception e)
        {
            SaveLog(new Log() { exception = e, text = "Exception Occured in MDO Utility", time = DateTime.UtcNow });
        }
    }
}
