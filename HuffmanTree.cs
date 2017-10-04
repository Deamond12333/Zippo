using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zippo
{
    class HuffmanTree
    {

    }

    class HuffmanTreeNode
    {
        public char Symbol { get; set; }
        public HuffmanTreeNode Parent { get; set; }
        public HuffmanTreeNode LeftChild { get; set; }
        public HuffmanTreeNode RightChild { get; set; }
        public int Weight { get; set; }
    }
}
