using System;
using System.Runtime.InteropServices;
using ConsoleHotKey.Enums;

namespace ConsoleHotKey
{
    public class Window
    {
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        
        [DllImport("user32.dll", SetLastError=true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);
        
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
                handle = IntPtr.Zero;
            }
        }

        public void render()
        {
            if (handle != IntPtr.Zero)
            {
                SetWindowPos(handle, (IntPtr) SpecialWindowHandles.HWND_TOP, X, Y, W, H, SetWindowPosFlags.SWP_SHOWWINDOW);                
            }
        }

        public void setSize(int workingAreaX, int workingAreaY, int workingAreaWidth, int workingAreaHeight)
        {
            X = workingAreaX;
            Y = workingAreaY;
            W = workingAreaWidth;
            H = workingAreaHeight;
        }
    }
}