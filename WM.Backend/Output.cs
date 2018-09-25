using System;
using System.Collections.Generic;

namespace ConsoleHotKey {
    public class Output {
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public List<Workspace> ws;

        public Output(int x, int y, int w, int h)
        {
            ws = new List<Workspace>();
            this.X = x;
            this.Y = y;
            this.W = w;
            this.H = h;
            ws.Add(new Workspace(Program.getNextWorkspace(), X, Y, W, H));
        }

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

        public bool cointainsPoint(int x, int y)
        {
            return x >= X && x <= X + W && y >= Y && y <= Y + H;
        }
    }
}