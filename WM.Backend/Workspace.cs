namespace ConsoleHotKey
{
    public class Workspace
    {
        public int num { get; set; }
        public Tree tree;

        public Workspace(int num)
        {
            tree = new Tree();
            this.num = num;
        }
    }
}