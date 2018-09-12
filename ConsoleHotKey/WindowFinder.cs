using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleHotKey
{
    public class WindowFinder{
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        
        [DllImport("user32.dll", CharSet=CharSet.Unicode, SetLastError=true)]
        static extern int GetWindowText(IntPtr window, StringBuilder lpWindowName, int maxCount);

        public static string GetWindowTextA(IntPtr window){
            StringBuilder str = new StringBuilder();
            GetWindowText(window, str, 256);
            return str.ToString();
        }
        
        public static IntPtr FindWindowClass(string caption){
            return FindWindow(caption, null);
        }
        public static IntPtr FindWindowClassTitle(string classname, string title){
            return FindWindow(classname, title);
        }

    }
}