using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WM.Utils;
using Binding = WM.Utils.Binding;


namespace ConsoleHotKey{
    class Program {        
        #region enums
        enum MonitorOptions : uint
        {
            MONITOR_DEFAULTTONULL = 0x00000000,
            MONITOR_DEFAULTTOPRIMARY = 0x00000001,
            MONITOR_DEFAULTTONEAREST = 0x00000002
        }
        struct POINT {
            public short x;
            public short y;
        }
        #endregion

        #region DLLs
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetFocus(IntPtr hWnd);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        
        [DllImport("user32.dll")]
        static extern IntPtr GetFocus();
        
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        
        [DllImport("user32.dll")]
        static extern bool CloseWindow(IntPtr hWnd);
        
        [DllImport("kernel32.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetExitCodeProcess(IntPtr hProcess, out uint ExitCode);
        #endregion
        
        #region vars
        public static List<Output> outputs = new List<Output>();
        public static List<Keys> keysDown = new List<Keys>();
        public static List<int> workspaces = new List<int>();
        private static Desktop _desk { get; set; }
        private static string config = "ricerc";
        private static Dictionary<Int64, string> _runMap = new Dictionary<Int64, string>();
        private static Dictionary<Int64, int> _workspaceMap = new Dictionary<Int64, int>();
        private static Dictionary<Int64, int> _killMap = new Dictionary<Int64, int>();
        private static IntPtr _bar { get; set; }
        public static readonly ConfigurationManager ConfigurationManager = new ConfigurationManager();

        #endregion

        [STAThread]
        static void Main(string[] args) {
            foreach(var screen in Screen.AllScreens)
            {
                Output tmp = new Output
                    {X = screen.WorkingArea.X, Y = screen.WorkingArea.Y, W = screen.WorkingArea.Width, H = screen.WorkingArea.Height};
                tmp.ws[0].tree.Root.window.setSize(screen.WorkingArea.X, screen.WorkingArea.Y,
                    screen.WorkingArea.Width, screen.WorkingArea.Height);
                outputs.Add(tmp);
            }

            _bar = WindowFinder.FindWindowClassTitle(null, "riceWM");
            loadKeybinds();
           
            IntPtr hmon = (IntPtr) 67552;
            SetFocus(hmon);
            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyManager_HotKeyPressed);
            Console.ReadLine();
        }

        public static int getNextWorkspace()
        {
            int next = 0;
            if (workspaces.Count == 0)
            {
                next = 1;
                workspaces.Add(next);
            }
            else
            {
                workspaces.Sort();
                int len = workspaces.Count;
                int last = workspaces[len - 1];
                next = last + 1;
                workspaces.Add(next);
            }
            return next;
        }

        public void removeWorkspace(int ws)
        {
            workspaces.Remove(ws);
        }
        private static void loadKeybinds()
        {
            List<Binding> bindings = ConfigurationManager.Bindings;
            foreach (var binding in bindings)
            {
                uint hotKeyId = 0;
                string[] keys = binding.KeyCombination.Split('+');
                HotKeyManager.KeyModifiers[] mods = null;
                Keys key = 0;
                if (keys.Length == 2) {
                    mods = new[] {
                        (HotKeyManager.KeyModifiers) Enum.Parse(typeof(HotKeyManager.KeyModifiers), keys[0], true)
                    };
                    key = (Keys)Enum.Parse(typeof(Keys), keys[1], true);
                    hotKeyId = ((uint)key << 16) | (uint) mods[0];
                }
                else if (keys.Length == 3) {
                    mods = new[] {
                        (HotKeyManager.KeyModifiers) Enum.Parse(typeof(HotKeyManager.KeyModifiers), keys[0], true),
                        (HotKeyManager.KeyModifiers) Enum.Parse(typeof(HotKeyManager.KeyModifiers), keys[1], true)
                    };
                    key = (Keys)Enum.Parse(typeof(Keys), keys[2], true);
                    hotKeyId = ((uint)key << 16) | (uint)mods[0] | (uint)mods[1];
                }
                HotKeyManager.RegisterHotKey(key, mods);
                if (binding.Command.ToLower() == "run") {
                    _runMap.Add(hotKeyId, binding.Parameters);
                }
                else if (binding.Command.ToLower() == "workspace") {
                    //throw new NotImplementedException();
                    int wsNum = 0;
                    Int32.TryParse(binding.Parameters, out wsNum);
                    _workspaceMap.Add(hotKeyId, wsNum);
                }
                else if (binding.Command.ToLower() == "kill")
                {
                    _killMap.Add(hotKeyId, 0);
                }
            }
        }
        
        //Debugging purposes
        static void printFocus()
        {
            IntPtr fhwn = GetFocus();
            Console.Out.WriteLine("fhwn = {0}", fhwn);
            string title = WindowFinder.GetWindowTextA(fhwn);
            Console.Out.WriteLine("title = {0}", title);
            IntPtr forhwm = GetForegroundWindow();
            Console.Out.WriteLine("forhwm = {0}", forhwm);
            string forTitle = WindowFinder.GetWindowTextA(forhwm);
            Console.Out.WriteLine("forTitle = {0}", forTitle);
        }
        
        static void CloseFocusWindow()
        {
            IntPtr forhwm = GetForegroundWindow();
            SendMessage(forhwm, (int)WindowsMessage.WM_SYSCOMMAND, (IntPtr)SysCommands.SC_CLOSE, IntPtr.Zero);
        }

        static void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e) {
            foreach (var hotKey in _runMap) {
                if (hotKey.Key == e.id) {
                    if (hotKey.Value.Contains(' ')) {
                        string[] cmdaArgs = hotKey.Value.Split(' ');
                        System.Diagnostics.Process.Start(cmdaArgs[0], cmdaArgs[1]);
                        return;
                    }
                    else {
                        System.Diagnostics.Process.Start(hotKey.Value);
                        return;
                    }
                }
            }

            foreach (var ws in _workspaceMap) {
                if (ws.Key == e.id) {
                    SendMessage(_bar, (int) WindowsMessage.WM_BAR_WS, (IntPtr)ws.Value, (IntPtr)ws.Value);
                }
            }
            foreach (var ws in _killMap) {
                if (ws.Key == e.id) {
                    printFocus();
                    CloseFocusWindow();
                }
            }
        }
    }
}