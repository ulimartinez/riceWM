using System;
using System.Collections.Generic;

namespace ConsoleHotKey {
    public class Output {
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        private List<Workspace> ws;

        public Workspace getWorkspaceByNum(int i)
        {
            foreach (var curWs in ws)
            {
                if (i == curWs.num)
                {
                    return curWs;
                }
            }
            return null;
        }
    }
}