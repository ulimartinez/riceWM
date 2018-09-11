﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

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
        #endregion
        
        #region vars
        static Desktop _desk;
        private static string config = "rice.config";
        private static Dictionary<Int64, string> _map = new Dictionary<Int64, string>();
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
            StreamReader reader = File.OpenText(config);
            string line;
            while ((line = reader.ReadLine()) != null) {
                //split the line by spaces
                uint hotKeyId = 0;
                string[] items = line.Split(' ');
                if (items[0] == "bind") {
                    if (items.Length < 3) {
                        Console.Out.WriteLine("Bind must have 3 components, skipping");
                        continue;
                    }
                    string[] keys = items[1].Split('+');
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
                    HotKeyManager.RegisterHotKey(key, mods);
                    string[] command = items[2].Split(':');
                    if (command.Length > 0) {
                        if (command[0].ToLower() == "run") {
                            _map.Add(hotKeyId, command[1]);
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

        static void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            foreach (var hotKey in _map) {
                if (hotKey.Key == e.id) {
                    System.Diagnostics.Process.Start(hotKey.Value);
                }
            }
        }
    }
}