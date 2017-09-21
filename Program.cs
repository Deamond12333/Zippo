using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace Zippo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the file path (preferably .txt format and with russians symbols): ");
            string filePath = Console.ReadLine();
            Console.WriteLine("Reading...");
            byte[] array;
            try
            {
                FileStream stream = new FileStream(filePath, FileMode.Open);
                array = new byte[(int)stream.Length];
                stream.Read(array, 0, (int)stream.Length);
                stream.Close();
                Console.Write("complete!");
            }
            catch
            {
                Console.WriteLine("Reading complete with errors...");
                Console.ReadKey();
                return;
            }
            string file = Encoding.UTF8.GetString(array);
            Console.WriteLine("Counting a symbols...");
            Dictionary<char, int> dictionary = new Dictionary<char, int>();
            for (int i = 0; i < file.Length; ++i)
            {
                if (dictionary.ContainsKey(file[i])) dictionary[file[i]]++;
                else dictionary.Add(file[i], 1);
            }
            Console.Write("complete!");
            Console.WriteLine("Creating a Huffman tree...");
            List<HuffmanTreeNode> nodes = new List<HuffmanTreeNode>();
            foreach(KeyValuePair<char, int> p in dictionary)
            {
                HuffmanTreeNode node = new HuffmanTreeNode();
                node.Symbol = p.Key;
                node.Weight = p.Value;
                nodes.Add(node);
            }
            while (nodes.Count > 1)
            {
                nodes = nodes.OrderByDescending(n => n.Weight).ToList();
                HuffmanTreeNode parentNode = new HuffmanTreeNode();
                parentNode.LeftChild = nodes[0];
                parentNode.LeftChild.Rotation = '0';
                parentNode.RightChild = nodes[1];
                parentNode.RightChild.Rotation = '1';
                parentNode.Weight = parentNode.LeftChild.Weight + parentNode.RightChild.Weight;
                nodes.RemoveAt(0);
                nodes.RemoveAt(0);
                nodes.Add(parentNode);
            }
            Console.Write("complete!");
            Console.WriteLine("Creating output file...");

            Console.ReadKey();
        }
    }
}
/*
            List<BiBranch> branches = new List<BiBranch>();
            foreach (char symbol in probability.Keys)
            {
                BiLeaf leaf = new BiLeaf();
                leaf.symbol = symbol;
                leaf.nodeLevel = 0;
                branches.Add(leaf);
            }
            int iterator = 0;
            short levelID = 1, branchID = 1;
            bool binValue = true;
            BiBranch newBranch = new BiBranch();
            foreach (var branch in branches)
            {
                if (levelID == 1)
                {
                    if (branch.GetType() == typeof(BiLeaf))
                    {
                        if (branchID < 3)
                        {
                            branch.lastBranch = newBranch;
                            branchID++;
                        }
                        else
                        {
                            //branches.Add(newBranch);
                            newBranch = new BiBranch();
                            branchID = 1;
                        }
                        iterator++;
                    }
                    else
                    {

                    }
                }
                else
                {

                }

            }
*/