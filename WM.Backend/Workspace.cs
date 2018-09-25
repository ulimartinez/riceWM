namespace ConsoleHotKey
{
    public class Workspace
    {
        public int num { get; set; }
        public Tree tree;

        public Workspace(int num, int X, int Y, int W, int H)
        {
            this.tree = new Tree(X, Y, W, H);
            this.num = num;
        }
    }
}