using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace ConsoleHotKey{
    class Program {
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

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetFocus(IntPtr hWnd);
        
        static Desktop _desk;
        private static string config = "rice.config";
        
        static void Main(string[] args)
        {
            foreach(var screen in Screen.AllScreens)
            {
                // For each screen, add the screen properties to a list box.
                Console.Out.WriteLine("Device Name: " + screen.DeviceName);
                Console.Out.WriteLine("Bounds: " + screen.Bounds.ToString());
                Console.Out.WriteLine("Type: " + screen.GetType().ToString());
                Console.Out.WriteLine("Working Area: " + screen.WorkingArea.ToString());
                Console.Out.WriteLine("Primary Screen: " + screen.Primary.ToString());
            }
            StreamReader reader = File.OpenText(config);
            string line;
            while ((line = reader.ReadLine()) != null) 
            {
                //split the line by spaces
                string[] items = line.Split(' ');
                if (items[0] == "bind") {
                    string[] keys = items[1].Split('+');
                    if (keys.Length < 2) {
                        Console.Out.WriteLine("Key bind must have a modifier and a key");
                    }
                    else {
                        HotKeyManager.KeyModifiers mod = (HotKeyManager.KeyModifiers)Enum.Parse(typeof(HotKeyManager.KeyModifiers), keys[0], true);
                        Keys key = (Keys)Enum.Parse(typeof(Keys), keys[1], true);
                        Console.Out.WriteLine(HotKeyManager.RegisterHotKey(key, new[]{mod}));
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
            Console.WriteLine("Hit me!");
            if (e.Key == Keys.Q) {
                //deleting the desktop
                _desk.Dispose();

            }
            else if (e.Key == Keys.D1) {
                //creating a desktop
                _desk = new Desktop("test2");
                _desk.show();
            }
            else if (e.Key == Keys.F) {
                //testing oppening a firefox
                System.Diagnostics.Process.Start("firefox.exe", "-new-window http://www.google.com");
            }
            else if (e.Key == Keys.H) {
                _desk.SwitchToOrginal();
            }
        }
    }
}