using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MathExam
{
    internal class Program
    {
        static int SW_SHOW = 5;
        static int SW_HIDE = 0;
        
        public static void Main(string[] args)
        {
            IntPtr myWindow = GetConsoleWindow();
            ShowWindow(myWindow, SW_HIDE);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            Application.Run(new MathExam());
        }
        
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
    }
}