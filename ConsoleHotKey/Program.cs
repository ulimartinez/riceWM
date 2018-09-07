using System;
using System.Windows.Forms;

namespace ConsoleHotKey
{
    class Program
    {
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
            HotKeyManager.RegisterHotKey(Keys.A, KeyModifiers.Alt);
            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyManager_HotKeyPressed);
            Console.ReadLine();      
        }

        static void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            Console.WriteLine("Hit me!");
        }
    }
}