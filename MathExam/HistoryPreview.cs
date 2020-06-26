using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MathExam
{
    public class HistoryPreview
    {
        private TextBox HistoryViewer;

        private FolderBrowserDialog folderBrowserDialog1;
        private OpenFileDialog openFileDialog1;

        private string openFileName;
        private string folderName;
        private bool fileOpened;

        public HistoryPreview()
        {
            InicializaceFileFinder();
        }

        public void ReadHistoryFile()
        {
            MathExam.HistoryViewer.Clear();
            string fileName = SelectFilePath();

/*
            foreach (var readLine in File.ReadLines(fileName))
            {
                MathExam.HistoryViewer.AppendText(readLine + Environment.NewLine);
                Console.WriteLine(readLine);
            }
            */
            FileStream stream = new HistoryFile().ReturnFileStream();
            using (stream)
            {
                
                using (StreamReader reader = new StreamReader(stream))
                {
                    string[] readText = File.ReadAllLines(fileName);
                    foreach (string s in readText)
                    {
                        MathExam.HistoryViewer.AppendText(s + Environment.NewLine);
                        Console.WriteLine(s);
                    }
                }
            }
            
            /*
              // Open the file to read from.
            string[] readText = File.ReadAllLines(fileName);
             foreach (string s in readText)
            {
                MathExam.HistoryViewer.AppendText(s + Environment.NewLine);
                Console.WriteLine(s);
            }*/
        }

        private void InicializaceFileFinder()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();


            // Set the help text description for the FolderBrowserDialog.
            this.folderBrowserDialog1.Description =
                "Select the directory that you want to use as the default.";

            // Do not allow the user to create new files via the FolderBrowserDialog.
            this.folderBrowserDialog1.ShowNewFolderButton = false;

            // Default to the My Documents folder.
            this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
        }

        private string SelectFilePath()
        {
            if (!fileOpened)
            {
                openFileDialog1.InitialDirectory = folderBrowserDialog1.SelectedPath;
                openFileDialog1.FileName = null;
            }

            // Display the openFile dialog.
            DialogResult result = openFileDialog1.ShowDialog();

            // OK button was pressed.
            if (result == DialogResult.OK)
            {
                openFileName = openFileDialog1.FileName;
                string extension;

                extension = Path.GetExtension(openFileName);
                if (extension == ".txt")
                {
                    Console.WriteLine("txt File");
                }

                try
                {
                    Console.WriteLine(openFileName);

                    fileOpened = true;
                }
                catch (Exception exp)
                {
                    MessageBox.Show("An error occurred while attempting to load the file. The error is:"
                                    + System.Environment.NewLine + exp.ToString() + System.Environment.NewLine);
                    fileOpened = false;
                }
            }

            if (openFileName == null)
            {
                openFileName = "C:\\CSharpFiles\\MathExam\\MathExam\bin\\Debug\\HistoryOfExamples\\test.txt";
            }

            return openFileName;
        }
    }
}