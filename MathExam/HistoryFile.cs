using System;
using System.IO;

namespace MathExam
{
    public class HistoryFile
    {
        //private static string Name = "hist.txt";

        private static string Filepath;
        //private static string path = ReturnHistoryFilePath();
        private FileStream stream;

        public HistoryFile()
        {
            stream = File.Open(ReturnHistoryFilePath(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        }
        
        private static string SetHistoryFileName()
        {
            string Name = ("history" + Convert.ToString(DateTime.Now.Day) + Convert.ToString(DateTime.Now.Month) +
                           Convert.ToString(DateTime.Now.Year) + Convert.ToString(DateTime.Now.Hour) +
                           Convert.ToString(DateTime.Now.Minute) + Convert.ToString(DateTime.Now.Second) + ".txt");
            return Name;
        }

        public string ReturnHistoryFilePath()
        {
            string path = "HistoryOfExamples\\" + SetHistoryFileName();
            Filepath = path;
            return path;
        }

        public string ReturnFilePath()
        {
            return Filepath;
        }

        public FileStream ReturnFileStream()
        {
            return stream;
        }
    }
}