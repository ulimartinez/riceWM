using System;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Xml;

namespace ConsoleHotKey
{
    public class Node
    {
        public Window window = new Window();
        public Node left;
        public Node right;
        public bool vsplit { get; set; }

        public bool hasWindow => window.handle != IntPtr.Zero;

        public bool isRoot { get; set; }
    }

    public class Tree
    {
        public Node Root;

        public Tree(int x, int y, int w, int h)
        {
            Root = new Node {window = {X = x, Y = y, W = w, H = h}, isRoot = true, vsplit = true};
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
        
        //this method should be used only to split the root
        private void addHelper(Node root, IntPtr handle)
        {
            if (root != null)
            {
                //root shouldn't have window here, shouled always be false?
                if (root.hasWindow)
                {
                    //if node doesn't have children
                    if (root.left == null && root.right == null)
                    {
                        //split into two
                        //root.hasWindow = false;
                        Window cur = root.window;
                        root.left = new Node();
                        root.left.window.handle = cur.handle;
                        root.right = new Node();
                        root.right.window.handle = handle;
                        root.window.handle = IntPtr.Zero;

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
                    root.window.handle = handle;
                }
            }
            else
            {
                return;
            }
        }
        
        private void addAfterHelper(Node root, IntPtr focusHandle, IntPtr new_handle)
        {
            if (root != null)
            {
                if (root.window.handle == focusHandle)
                {
                    //should be redundant because only leaf nodes are supposed to have handles in their windows
                    if (root.left == null && root.right == null)
                    {
                        //split into two
                        //root.hasWindow = false;
                        Window cur = root.window;
                        root.left = new Node();
                        root.left.window.handle = cur.handle;
                        root.right = new Node();
                        root.right.window.handle = new_handle;
                        root.window.handle = IntPtr.Zero;

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
                }
                else if (contains_handle(root.left, focusHandle))
                {
                    addAfterHelper(root.left, focusHandle, new_handle);
                }
                else
                {
                    addAfterHelper(root.right, focusHandle, new_handle);
                }
            }
            else
            {
                return;
            }
            
        }

        public void deleteNode(IntPtr delHandle)
        {
            if (Root.window.handle == delHandle)
            {
                Root.window.destroy();
            }
            else
            {
                Root = delNode(Root, delHandle);
            }
            render();
        }

        private Node delNode(Node root, IntPtr delHandle)
        {
            //after finding the deletion node, make the sibling the new parent
            
            //base case for assigning the nodes recursively
            if (root.left == null && root.right == null)
                return root;
            
            Window cur = root.window;
            //case where the left node is the node we are deleting
            if (root.left.window.handle == delHandle)
            {
                //make the right child the new root, and return the reference
                fixNodeSize(root.right, cur.X, cur.Y, cur.W, cur.H);
                root.left.window.destroy();
                return root.right;
            }
            //case where right node is one we are deleting
            if (root.right.window.handle == delHandle)
            {
                //same as above
                fixNodeSize(root.left, cur.X, cur.Y, cur.W, cur.H);
                root.right.window.destroy();
                return root.left;
            }
            //case where neither child is the one we are deleting, we need to call recurively on both of them.
            root.left = delNode(root.left, delHandle);
            root.right = delNode(root.right, delHandle);
            return root;
        }

        private void fixNodeSize(Node root, int x, int y, int w, int h)
        {
            if (root == null)
                return;
            
            root.window.X = x;
            root.window.Y = y;
            root.window.W = w;
            root.window.H = h;

            if (root.vsplit)
            {
                fixNodeSize(root.left, x, y, w/2, h);
                fixNodeSize(root.right, x+w/2, y, w/2, h);
            }
            else
            {
                fixNodeSize(root.left, x, y, w, h/2);
                fixNodeSize(root.right, x, y+h/2, w, h/2);
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