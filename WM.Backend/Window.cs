using System;
using System.Runtime.InteropServices;

namespace ConsoleHotKey
{
    public class Window
    {
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        
        public IntPtr handle { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }

        public void destroy()
        {
            if (handle != null)
            {
                SendMessage(handle, (int) WindowsMessage.WM_SYSCOMMAND, (IntPtr) SysCommands.SC_CLOSE, IntPtr.Zero);                
            }
        }
    }
}