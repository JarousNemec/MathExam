using System.CodeDom.Compiler;
using System.IO;
using System.Net.Mime;
using System.Text;

namespace MathExam
{
    public class ExamplesLogger
    {
        //static HistoryFile hf = new HistoryFile();

            
        //private static StreamWriter tw = File.AppendText(hf.ReturnHistoryFilePath());
        public bool Isclose;
        
        public static void Write(string s)
        {
          /*  FileStream stream = HistoryFile.ReturnFileStream();
                
            using (stream)
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(s);
                }
            }*/
        }

    }
}