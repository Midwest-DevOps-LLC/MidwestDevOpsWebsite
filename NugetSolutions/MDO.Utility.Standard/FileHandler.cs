using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MDO.Utility.Standard
{
    public static class FileHandler
    {
        //TODO: have this file have the default locations of the logfile and the connection files. allow ability to add own files to th list and edit existing

        //also make custom exception in new class

        public static string ApplicationFolderName;

        public static string FolderPath
        {
            get
            {
                //Create nessasary folders
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                var mdoPath = Path.Combine(documentsPath, "MidwestDevOps");

                if (Directory.Exists(mdoPath) == false)
                {
                    Directory.CreateDirectory(mdoPath);
                }

                var applicationFolderPath = Path.Combine(mdoPath, ApplicationFolderName);

                if (Directory.Exists(applicationFolderPath) == false)
                {
                    Directory.CreateDirectory(applicationFolderPath);
                }

                return applicationFolderPath;
            }
        }

        public static bool WriteToFile(string fileName, string line, bool append)
        {
            var ret = false;

            //Create nessasary folders
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var mdoPath = Path.Combine(documentsPath, "MidwestDevOps");

            if (Directory.Exists(mdoPath) == false)
            {
                Directory.CreateDirectory(mdoPath);
            }

            var applicationFolderPath = Path.Combine(mdoPath, ApplicationFolderName);

            if (Directory.Exists(applicationFolderPath) == false)
            {
                Directory.CreateDirectory(applicationFolderPath);
            }

            //Write to the file
            var filePath = Path.Combine(applicationFolderPath, fileName);

            using (StreamWriter sw = new StreamWriter(filePath, append))
            {
                sw.WriteLine(line);
            }

            return ret;
        }

        public static string ReadFromFile(string fileName)
        {
            string ret = "";

            //Create nessasary folders
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var mdoPath = Path.Combine(documentsPath, "MidwestDevOps");

            if (Directory.Exists(mdoPath) == false)
            {
                Directory.CreateDirectory(mdoPath);
            }

            var applicationFolderPath = Path.Combine(mdoPath, ApplicationFolderName);

            if (Directory.Exists(applicationFolderPath) == false)
            {
                Directory.CreateDirectory(applicationFolderPath);
            }

            //Write to the file
            var filePath = Path.Combine(applicationFolderPath, fileName);

            if (File.Exists(filePath) == false)
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write("");
                }
            }

            using (StreamReader sr = new StreamReader(filePath))
            {
                ret = sr.ReadToEnd();
            }

            return ret;
        }
    }
}
