using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ConsoleHotKey{
    class Program {
        static Desktop desk;
        
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
            HotKeyManager.RegisterHotKey(Keys.D1, HotKeyManager.KeyModifiers.Alt);
            HotKeyManager.RegisterHotKey(Keys.Q, HotKeyManager.KeyModifiers.Alt);
            HotKeyManager.RegisterHotKey(Keys.F, HotKeyManager.KeyModifiers.Alt);
            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyManager_HotKeyPressed);
            Console.ReadLine();      
        }

        static void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            Console.WriteLine("Hit me!");
            if (e.Key == Keys.Q) {
                //deleting the desktop
                desk.Dispose();
                
            }
            else if (e.Key == Keys.D1) {
                //creating a desktop
                desk = new Desktop("one");
                desk.show();
            }
            else if (e.Key == Keys.F) {
                System.Diagnostics.Process.Start("firefox.exe", "-new-window http://www.google.com");
            }
        }
    }
}