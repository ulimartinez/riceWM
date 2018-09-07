using System;
using System.Runtime.InteropServices;

namespace windowManager{
    public class Window{
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        
        public static IntPtr FindWindow(string caption){
            return FindWindow(String.Empty, caption);
        }
        
    }
}