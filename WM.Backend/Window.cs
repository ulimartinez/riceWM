using System;

namespace ConsoleHotKey
{
    public class Window
    {
        public IntPtr handle { get; set; }
        private int X { get; set; }
        private int Y { get; set; }
        private int W { get; set; }
        private int H { get; set; }
    }
}