namespace Lab5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Node root = new Node();

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
                node.children.Remove(node.children[2]);
                //node.children.Remove(node.children[2]);
            }
            parent.keys.Insert(0 + i, node.keys[1]);
            node.keys.Remove(node.keys[1]);
            parent.children.Insert(i + 1, new_node);
        }

        void Print_tree(Node node, string indent = "", string position = "R----")
        {
            TextAdd(indent);
            TextAdd(position);
            for (int i = 0; i < node.keys.Count(); i++)
            {
                TextAdd(node.keys[i].ToString());
                TextAdd(" ");
            }
            TextAdd("\n");
            if (!(node.children.Count() == 0))
            {
                for (int i = 0; i < node.children.Count(); i++)
                {
                    if (i == 0)
                    {
                        Print_tree(node.children[i], indent + "     ", "L----");
                    }
                    else
                    {
                        Print_tree(node.children[i], indent + "     ", "R----");
                    }
                }
            }
        }

        void Print_tree()
        {
            Print_tree(root);
        }

        public void TextAdd(string text)
        {
            textBox1.Text += text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<int> keys = new List<int>();
            keys.Add(4);
            keys.Add(6);
            keys.Add(2);
            keys.Add(8);
            keys.Add(11);
            keys.Add(15);
            keys.Add(12);
            keys.Add(21);
            keys.Add(17);
            keys.Add(13);
            keys.Add(35);
            keys.Add(37);
            keys.Add(45);
            keys.Add(87);
            keys.Add(92);
            keys.Add(29);
            keys.Add(77);
            keys.Add(76);
            keys.Add(75);
            keys.Add(79);
            for (int i = 0; i < keys.Count(); i++)
            {
                Insert(keys[i]);
                Print_tree();
                TextAdd("\n");
            }
        }
    }
}
