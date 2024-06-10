using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    internal class Node
    {
        public List<int> keys;
        public List<Node> children;

        public Node()
        {
            keys =  new List<int>();
            children = new List<Node>();
        }
    }
}
