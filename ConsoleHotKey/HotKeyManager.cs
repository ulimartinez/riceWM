using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using Gma.UserActivityMonitor;

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
            Ctrl = 2,
            Shift = 4,
            Win = 8,
            NoRepeat = 0x4000
        }
        #endregion
        
        #region vars
        public static List<Keys> keysDown = new List<Keys>();
        private static int _id = 0;
        private static volatile MessageWindow _wnd;
        private static volatile IntPtr _hwnd;
        private static ManualResetEvent _windowReadyEvent = new ManualResetEvent(false);
        private static List<uint> registeredWinKeys = new List<uint>();
        #endregion
        
        public static event EventHandler<HotKeyEventArgs> HotKeyPressed;

        public static int RegisterHotKey(Keys key, KeyModifiers[] modifiers) {
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
            if (!RegisterHotKey(hwnd, id, modifiers, key)) {
                registeredWinKeys.Add((key << 16) | modifiers);
            }
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

        static HotKeyManager(){
            Thread messageLoop = new Thread(delegate()
            {
                Application.Run(new MessageWindow());
            });
            messageLoop.Name = "MessageLoopThread";
            messageLoop.IsBackground = true;
            messageLoop.Start();      
        }
        
        public static bool WIN()
        {
            //return keysDown.Contains(Keys.LShiftKey)
            if (keysDown.Contains(Keys.LWin) || 
                keysDown.Contains(Keys.RWin))
            {
                return true;
            }
            return false;
        }

        private class MessageWindow : Form{
            public MessageWindow()
            {
                _wnd = this;
                _hwnd = this.Handle;
                _windowReadyEvent.Set();
                HookManager.KeyDown += HookManager_KeyDown;
                HookManager.KeyUp += HookManager_KeyUp;
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
            
            private void HookManager_KeyDown(object sender, KeyEventArgs e) {
                HotKeyEventArgs hk_e;
                if(keysDown.Contains(e.KeyCode) == false)
                {
                    keysDown.Add(e.KeyCode);
                }

                uint modifiers = 0;
                if (e.Alt) {
                    modifiers |= (uint)KeyModifiers.Alt;
                }
                if (e.Shift) {
                    modifiers |= (uint)KeyModifiers.Shift;
                }
                if (e.Control) {
                    modifiers |= (uint) KeyModifiers.Ctrl;
                }
                if (WIN()) {
                    modifiers |= (uint) KeyModifiers.Win;
                }
                uint keyId = ((uint) e.KeyCode << 16) | (uint) modifiers;
                foreach (var registeredkeyId in registeredWinKeys) {
                    if (keyId == registeredkeyId) {
                        e.Handled = true;
                        hk_e = new HotKeyEventArgs((IntPtr)keyId);
                        OnHotKeyPressed(hk_e);
                    }
                }
            }
            private void HookManager_KeyUp(object sender, KeyEventArgs e)
            {
                while(keysDown.Contains(e.KeyCode))
                {
                    keysDown.Remove(e.KeyCode);
                }
            }

            private const int WM_HOTKEY = 0x312;
        }
    }


    public class HotKeyEventArgs : EventArgs{
        public readonly Keys Key;
        public readonly HotKeyManager.KeyModifiers Modifiers;
        public readonly uint id;

        public HotKeyEventArgs(Keys key, HotKeyManager.KeyModifiers modifiers){
            Key = key;
            Modifiers = modifiers;
            id = ((uint) key << 16) | (uint) modifiers;
        }

        public HotKeyEventArgs(IntPtr hotKeyParam){
            id = (uint)hotKeyParam.ToInt64();
            Key = (Keys)((id & 0xffff0000) >> 16);
            Modifiers = (HotKeyManager.KeyModifiers)(id & 0x0000ffff);
        }
    }
}