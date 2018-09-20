using System;
using System.Runtime.InteropServices;

namespace ConsoleHotKey
{
    public class Window
    {
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        
        public IntPtr handle { get; set; }
        private int X { get; set; }
        private int Y { get; set; }
        private int W { get; set; }
        private int H { get; set; }

        public void destroy()
        {
            SendMessage(handle, (int) WindowsMessage.WM_SYSCOMMAND, (IntPtr) SysCommands.SC_CLOSE, IntPtr.Zero);
        }
    }
}