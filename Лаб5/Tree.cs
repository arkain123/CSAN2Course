using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    internal class Tree
    {
        private Node root;

        public Tree()
        {
            root = new Node();
        }

        public void Insert(int key)
        {
            if (root.keys.Count() == 3)
            {
                Node new_root = new Node();
                new_root.children.Add(root);
                split_child(new_root, 0);
                root = new_root;
            }
            insert_non_full(root, key);
        }

        void insert_non_full(Node node, int key)
        {
            if (node.children.Count() == 0)
            {
                node.keys.Add(key);
                node.keys.Sort();
            }
            else
            {
                int i = node.keys.Count() - 1;
                while (i >= 0 && key < node.keys[i])
                {
                    i--;
                }
                i++;
                if (node.children[i].keys.Count() == 3)
                {
                    split_child(node, i);
                    if (key > node.keys[i])
                    {
                        i++;
                    }
                }
                insert_non_full(node.children[i], key);
            }
        }

        void split_child(Node parent, int i)
        {
            Node node = parent.children[i];
            Node new_node = new Node();
            new_node.keys.Add(node.keys[2]);
            node.keys.Remove(node.keys[2]);
            if (!(node.children.Count() == 0))
            {
                new_node.children.Add(node.children[2]);
                new_node.children.Add(node.children[3]);
                node.children.Remove(node.children[2]);
                node.children.Remove(node.children[3]);
                node.children.Remove(node.children[4]);
            }
            parent.keys.Insert(0 + i, node.keys[1]);
            node.keys.Remove(node.keys[1]);
            parent.children.Insert(i + 1, new_node);
        }
    }
}
