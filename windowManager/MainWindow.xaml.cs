using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;
using System.Windows.Interop;

namespace windowManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow{
        
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        
        private HwndSource source;
        private const int HOTKEY_ID = 9000;
        
        //Modifiers:
        private const uint MOD_NONE = 0x0000; //[NONE]
        private const uint MOD_ALT = 0x0001; //ALT
        private const uint MOD_CONTROL = 0x0002; //CTRL
        private const uint MOD_SHIFT = 0x0004; //SHIFT
        private const uint MOD_WIN = 0x0008; //WINDOWS
//CAPS LOCK:
        private const uint VK_CAPITAL = 0x14;
    
        public MainWindow(){
            //RegisterHotKey();
            InitializeComponent();
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
  
            IntPtr handle = new WindowInteropHelper(this).Handle;
            source = HwndSource.FromHwnd(handle);
            source.AddHook(HwndHook);
  
            RegisterHotKey(handle, HOTKEY_ID, MOD_CONTROL, Keys.A.GetHashCode()); //CTRL + CAPS_LOCK
        }
        
        protected override void OnClosed(EventArgs e)
        {
            source.RemoveHook(HwndHook);
            source = null;
            UnregisterHotKey();
            base.OnClosed(e);
        }

//        private void RegisterHotKey()
//        {
//            var helper = new WindowInteropHelper(this);
//            const int VK_F10 = 0x79;
//            const int MOD_CTRL = 0x0002;
//            if(!RegisterHotKey(helper.Handle, HOTKEY_ID, 4, Keys.A.GetHashCode()))
//            {
//                // handle error
//                Console.Out.WriteLine("Error");
//            }
//        }

        private void UnregisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            UnregisterHotKey(helper.Handle, HOTKEY_ID);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            Console.Out.WriteLine("Got a hook, checking if it's ok");
            switch(msg){
                case WM_HOTKEY:
                    Console.Out.WriteLine("It's a hotkey");
                    switch(wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            Console.Out.WriteLine("It's our hotkey");
                            OnHotKeyPressed();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private void OnHotKeyPressed()
        {
            // do stuff
            Console.Out.WriteLine("It worked?");
            Process[] processlist = Process.GetProcesses(Environment.MachineName);

            foreach (Process process in processlist){
                IntPtr handle = process.MainWindowHandle;
                if (handle != IntPtr.Zero){
                    //TODO: import the definitions of the flags for last parameter
                    SetWindowPos(handle, 0, 0, 0, 0, 0, 0x0001);
                }
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    Console.WriteLine("Process: {0} ID: {1} Window title: {2}, handle: {4}, Other handle: {3}", process.ProcessName, process.Id, process.MainWindowTitle, process.Handle, process.MainWindowHandle);
                }
            }
        }
    }
}