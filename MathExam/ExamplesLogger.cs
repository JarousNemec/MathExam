using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace MathExam
{
    public class ExamplesLogger
    {
        static StreamWriter streamOUT;
        private HistoryFile hf;
        public ExamplesLogger()
        {
            hf = new HistoryFile();
            streamOUT = new StreamWriter(hf.ReturnFileOUTStream());
        }
        public void Write(string s)
        {
          streamOUT.Write(s);
          streamOUT.Flush();
        }

    }
}