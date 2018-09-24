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
        public bool isRoot { get; set; }
    }

    public class Tree
    {
        public readonly Node Root;

        public Tree(int x, int y, int w, int h)
        {
            Root = new Node {window = {X = x, Y = y, W = w, H = h}, hasWindow = false, isRoot = true, vsplit = true};
        }

        public void addAfter(IntPtr focus_handle, IntPtr new_handle)
        {
            if (contains_handle(Root, focus_handle))
            {
                addAfterHelper(Root, focus_handle, new_handle);
            }
            else
            {
                addHelper(Root, new_handle);
            }
        }

        private void addHelper(Node root, IntPtr handle)
        {
            if (root != null)
            {
                if (root.hasWindow)
                {
                    if (root.left == null && root.right == null)
                    {
                        //split into two
                        //root.hasWindow = false;
                        Window cur = root.window;
                        root.left = new Node();
                        root.left.hasWindow = true;
                        root.left.window.handle = cur.handle;
                        root.right = new Node();
                        root.right.window.handle = handle;
                        root.right.hasWindow = true;

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
                    }
                    else if(root.right == null)
                    {
                        addHelper(root.left, handle);
                    }
                    else if (root.left == null)
                    {
                        addHelper(root.right, handle);
                    }
                    else
                    {
                        addHelper(root.left, handle);
                    }
                }
                else
                {
                    root.hasWindow = true;
                    root.window.handle = handle;
                }
            }
            else
            {
                return;
            }
        }
        private bool addAfterHelper(Node root, IntPtr focusHandle, IntPtr new_handle)
        {
            if (root != null)
            {
                if (root.window.handle == focusHandle)
                {
                    if (root.left == null && root.right == null)
                    {
                        //split into two
                        //root.hasWindow = false;
                        Window cur = root.window;
                        root.left = new Node();
                        root.left.hasWindow = true;
                        root.left.window.handle = cur.handle;
                        root.right = new Node();
                        root.right.hasWindow = true;
                        root.right.window.handle = new_handle;

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
                    }
                    else if(root.right == null)
                    {
                        addAfterHelper(root.left, focusHandle, new_handle);
                    }
                    else if (root.left == null)
                    {
                        addAfterHelper(root.right, focusHandle, new_handle);
                    }
                    else
                    {
                        addAfterHelper(root.left, focusHandle, new_handle);
                    }
                    

                    return true;
                }
                else
                {
                    bool fromChild = false;
                    fromChild = addAfterHelper(root.left, focusHandle, new_handle);
                    if (!fromChild)
                    {
                        fromChild = addAfterHelper(root.right, focusHandle, new_handle);                    
                    }
                    return fromChild;
                }
            }
            else
            {
                return false;
            }
            
        }

        private bool contains_handle(Node root, IntPtr handle)
        {
            if (root == null)
            {
                return false;
            }
            if (root.window.handle == handle)
            {
                return true;
            }
            else
            {
                return contains_handle(root.left, handle) || contains_handle(root.right, handle);
            }
        }

        public void setVsplit(IntPtr focused, bool vsplit)
        {
            setVsplitHelper(Root, focused, vsplit);
        }

        private void setVsplitHelper(Node root, IntPtr fhandle, bool vsplit)
        {
            if (root == null)
            {
                return;
            }

            if (root.window.handle == fhandle)
            {
                root.vsplit = vsplit;
            }
            else
            {
                setVsplitHelper(root.left, fhandle, vsplit);
                setVsplitHelper(root.right, fhandle, vsplit);
            }
        }

        public void render()
        {
            renderHelper(Root);
        }

        private void renderHelper(Node root)
        {
            if (root == null)
            {
                return;
            }
            root.window.render();
            renderHelper(root.left);
            renderHelper(root.right);
        }

        public void swapChildren(Node root)
        {
            Node tmp = root.left;
            root.left = root.right;
            root.right = tmp;
        }
    }
}