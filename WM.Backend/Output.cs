using System;
using System.Collections.Generic;

namespace ConsoleHotKey {
    public class Output {
        private int X { get; set; }
        private int Y { get; set; }
        private int W { get; set; }
        private int H { get; set; }
        private List<IntPtr> Windows;
    }
}