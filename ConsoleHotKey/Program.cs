using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Gma.UserActivityMonitor;

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
        #endregion
        
        #region vars
        public static List<Keys> keysDown = new List<Keys>();
        static Desktop _desk;
        private static string config = ".ricerc";
        private static Dictionary<Int64, string> _runMap = new Dictionary<Int64, string>();
        private static Dictionary<Int64, int> _workspaceMap = new Dictionary<Int64, int>();
        private static IntPtr _bar;
        #endregion
        
        static void Main(string[] args)
        {
            foreach(var screen in Screen.AllScreens) {
                // For each screen, add the screen properties to a list box.
                Console.Out.WriteLine("Device Name: " + screen.DeviceName);
                Console.Out.WriteLine("Bounds: " + screen.Bounds.ToString());
                Console.Out.WriteLine("Type: " + screen.GetType().ToString());
                Console.Out.WriteLine("Working Area: " + screen.WorkingArea.ToString());
                Console.Out.WriteLine("Primary Screen: " + screen.Primary.ToString());
            }

            _bar = WindowFinder.FindWindowClassTitle(null, "riceWM");
            StreamReader reader = File.OpenText(config);
            string line;
            while ((line = reader.ReadLine()) != null) {
                //split the line by spaces
                uint hotKeyId = 0;
                var parts = Regex.Split(line, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                if (parts[0] == "bind") {
                    if (parts.Length < 3) {
                        Console.Out.WriteLine("Bind must have 3 components, skipping");
                        continue;
                    }
                    string[] keys = parts[1].Split('+');
                    if (keys.Length < 2) {
                        Console.Out.WriteLine("Key bind must have a modifier and a key, skipping");
                        continue;
                    }

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
                    int i = HotKeyManager.RegisterHotKey(key, mods);
                    var command = Regex.Split(parts[2], "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*):(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    //string[] command = parts[2].Split(':');
                    if (command.Length > 0) {
                        if (command[0].ToLower() == "run") {
                            _runMap.Add(hotKeyId, command[1]);
                        }
                        else if (command[0].ToLower() == "workspace") {
                            //throw new NotImplementedException();
                            int wsNum = 0;
                            Int32.TryParse(command[1], out wsNum);
                            _workspaceMap.Add(hotKeyId, wsNum);
                        }
                    }
                }
            }
            POINT pt = new POINT();
            pt.x = 1;
            pt.y = 1;
            IntPtr hmon = MonitorFromPoint(pt, MonitorOptions.MONITOR_DEFAULTTONEAREST);
            SetFocus(hmon);
            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyManager_HotKeyPressed);
            Console.ReadLine();
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
                    SendMessage(_bar, 0x165, (IntPtr)ws.Value, (IntPtr)ws.Value);
                }
            }
        }
    }
}