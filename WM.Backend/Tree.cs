using System;
using System.Reflection.Emit;
using System.Xml;

namespace ConsoleHotKey
{
    public class Node
    {
        public Window window = new Window();
        public Node left;
        public Node right;
        public bool vsplit { get; set; }
        public bool hasWindow { get; set; }
    }

    public class Tree
    {
        public Node Root;

        public Tree(Node root)
        {
            this.Root = root;
        }
        public Tree()
        {
            this.Root = new Node();
        }

        public Window addAfter(IntPtr handle)
        {
            return addAfterHelper(this.Root, handle);
        }
        private Window addAfterHelper(Node root, IntPtr focusHandle)
        {
            if (root != null)
            {
                if (root.window.handle == focusHandle)
                {
                    //split into two
                    root.hasWindow = false;
                    Window cur = root.window;
                    root.left = new Node();
                    root.left.window.handle = cur.handle;
                    root.right = new Node();

                    if (root.vsplit)
                    {
                        //split vertically
                        root.left.window.X = cur.X;
                        root.right.window.X = cur.X + cur.W / 2;

                        root.left.window.Y = cur.Y;
                        root.right.window.Y = cur.Y;

                        root.left.window.W = cur.W / 2;
                        root.right.window.W = cur.W / 2;

                        root.left.window.H = cur.H;
                        root.right.window.H = cur.H;
                    }
                    else
                    {
                        //split horizontally
                        root.left.window.X = cur.X;
                        root.right.window.X = cur.X;

                        root.left.window.Y = cur.Y;
                        root.right.window.Y = cur.Y + cur.H/2;

                        root.left.window.W = cur.W;
                        root.right.window.W = cur.W;

                        root.left.window.H = cur.H / 2;
                        root.right.window.H = cur.H / 2;
                    }

                    return root.right.window;
                }
                else
                {
                    Window fromChild;
                    fromChild = addAfterHelper(root.left, focusHandle);
                    if (fromChild == null)
                    {
                        fromChild = addAfterHelper(root.right, focusHandle);                    
                    }
                    return fromChild;
                }
            }
            else
            {
                return null;
            }
            
        }
        public Node insert(Node root, Window window)
        {
            if (root == null)
            {
                root = new Node{ hasWindow = true};
                root.window = window;
            }
            else
            {
                if (root.left != null)
                {
                    
                }
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