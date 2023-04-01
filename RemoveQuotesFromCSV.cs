using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RemoveQuotesFromCSV
{
    // Install instructions: https://www.c-sharpcorner.com/article/create-windows-services-in-c-sharp/#:~:text=Let's%20create%20a%20Windows%20Service%20in%20C%23%20using%20Visual%20Studio.&text=Open%20Visual%20Studio%2C%20click%20File,and%20click%20the%20OK%20button.&text=Go%20to%20Visual%20C%23%20%2D%3E%22,name%20and%20then%20click%20OK.
    public partial class RemoveQuotesFromCSV : ServiceBase
    {
        private FileSystemWatcher watcher;
        public RemoveQuotesFromCSV()
        {
            // Set up the file system watcher
            watcher = new FileSystemWatcher("C:\\a1");
            watcher.Filter = "export.csv";
            watcher.EnableRaisingEvents = true;

            // Add event handlers
            watcher.Created += new FileSystemEventHandler(OnFileCreated);
        }
        private void OnFileCreated(object source, FileSystemEventArgs e)
        {
            // Open the file and modify it
            string filePath = e.FullPath;
            string fileContents;

            using (StreamReader reader = new StreamReader(filePath))
            {
                fileContents = reader.ReadToEnd();
            }

            fileContents = fileContents.Replace("\"", "");

            string newFilePath = "C:\\a2\\export-final.csv";

            using (StreamWriter writer = new StreamWriter(newFilePath, false, System.Text.Encoding.UTF8))
            {
                writer.Write(fileContents);
            }

            // Delete the original file
            File.Delete(filePath);
        }
        protected override void OnStart(string[] args)
        {
            // Start the service
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            // Stop the service
            base.OnStop();
        }
    }
}
