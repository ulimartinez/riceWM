using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConsoleHotKey{
    public static class HotKeyManager{
        
        #region DLLs
        [DllImport("user32", SetLastError=true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        #endregion
        
        #region enum
        [Flags]
        public enum KeyModifiers{
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8,
            NoRepeat = 0x4000
        }
        #endregion
        
        #region vars
        private static int _id = 0;
        #endregion
        
        public static event EventHandler<HotKeyEventArgs> HotKeyPressed;

        public static int RegisterHotKey(Keys key, KeyModifiers[] modifiers){
            _windowReadyEvent.WaitOne();
            int id = _id;
            uint mods = 0;
            foreach (var mod in modifiers) {
                mods |= (uint)mod;
            }
            _wnd.Invoke(new RegisterHotKeyDelegate(RegisterHotKeyInternal), _hwnd, id, mods, (uint)key);
            System.Threading.Interlocked.Increment(ref _id);
            return id;
        }

        public static void UnregisterHotKey(int id){
            _wnd.Invoke(new UnRegisterHotKeyDelegate(UnRegisterHotKeyInternal), _hwnd, id);
        }

        delegate void RegisterHotKeyDelegate(IntPtr hwnd, int id, uint modifiers, uint key);
        delegate void UnRegisterHotKeyDelegate(IntPtr hwnd, int id);

        private static void RegisterHotKeyInternal(IntPtr hwnd, int id, uint modifiers, uint key){
            RegisterHotKey(hwnd, id, modifiers, key);      
        }

        private static void UnRegisterHotKeyInternal(IntPtr hwnd, int id)
        {
            UnregisterHotKey(_hwnd, id);
        }    

        private static void OnHotKeyPressed(HotKeyEventArgs e){
            if (HotKeyManager.HotKeyPressed != null){
                HotKeyManager.HotKeyPressed(null, e);
            }
        }

        private static volatile MessageWindow _wnd;
        private static volatile IntPtr _hwnd;
        private static ManualResetEvent _windowReadyEvent = new ManualResetEvent(false);
        static HotKeyManager(){
            Thread messageLoop = new Thread(delegate()
            {
                Application.Run(new MessageWindow());
            });
            messageLoop.Name = "MessageLoopThread";
            messageLoop.IsBackground = true;
            messageLoop.Start();      
        }

        private class MessageWindow : Form{
            public MessageWindow()
            {
                _wnd = this;
                _hwnd = this.Handle;
                _windowReadyEvent.Set();
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_HOTKEY)
                {
                    HotKeyEventArgs e = new HotKeyEventArgs(m.LParam);
                    HotKeyManager.OnHotKeyPressed(e);
                }

                base.WndProc(ref m);
            }

            protected override void SetVisibleCore(bool value)
            {
                // Ensure the window never becomes visible
                base.SetVisibleCore(false);
            }

            private const int WM_HOTKEY = 0x312;
        }
    }


    public class HotKeyEventArgs : EventArgs{
        public readonly Keys Key;
        public readonly HotKeyManager.KeyModifiers Modifiers;
        public readonly int id;

        public HotKeyEventArgs(Keys key, HotKeyManager.KeyModifiers modifiers){
            this.Key = key;
            this.Modifiers = modifiers;
        }

        public HotKeyEventArgs(IntPtr hotKeyParam){
            uint param = (uint)hotKeyParam.ToInt64();
            Key = (Keys)((param & 0xffff0000) >> 16);
            Modifiers = (HotKeyManager.KeyModifiers)(param & 0x0000ffff);
        }
    }
}