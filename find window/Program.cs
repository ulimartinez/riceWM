using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;

namespace find_window
{
    internal class Program
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName,int nMaxCount);
        
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);


        public static void Main(string[] args){

            Process[] processlist = Process.GetProcesses(Environment.MachineName);

            foreach (Process process in processlist){
                IntPtr handle = process.MainWindowHandle;
                if (handle != IntPtr.Zero){
                    //TODO: import the definitions of the flags for last parameter
                    SetWindowPos(handle, 0, 0, 0, 0, 0, 0);
                }
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    Console.WriteLine("Process: {0} ID: {1} Window title: {2}, handle: {4}, Other handle: {3}", process.ProcessName, process.Id, process.MainWindowTitle, process.Handle, process.MainWindowHandle);
                }
            }
        }
    }
}