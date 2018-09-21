using System;
using System.Reflection.Emit;

namespace ConsoleHotKey
{
    public class Node
    {
        public Window window;
        public Node left;
        public Node right;
        public bool vsplit { get; set; }
    }

    public class Tree
    {
        public Node insert(Node root, Window window)
        {
            if (root == null)
            {
                root = new Node();
                root.window = window;
            }
            else if (root.left != null)
            {
                root.left = insert(root.left, window);
            }
            else
            {
                root.right = insert(root.right, window);
            }

            return root;
        }

        public void traverse(Node root)
        {
            if (root == null)
            {
                return;
            }
            traverse(root.left);
            traverse(root.right);
        }

        public void swapChildren(Node root)
        {
            Node tmp = root.left;
            root.left = root.right;
            root.right = tmp;
        }
    }
}