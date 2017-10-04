using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace Zippo
{
    class Program
    {
        public static Dictionary<char, string> coding;
        public static StreamWriter output;
        static void Main(string[] args)
        {
            begin:
            Console.WriteLine("Enter the file path (preferably .txt format and with russians symbols): ");

            string filePath = Console.ReadLine();
            string pattern = @"^[\w]:\/([\w]+\/)?[\w]+\.[\w]{2,5}$";
            Regex r = new Regex(pattern);
            if (!r.IsMatch(filePath))
            {
                Console.WriteLine("You should write a correct path!");
                goto begin;
            }

            Console.Write("Reading...");

            string file = "";
            try
            {
                StreamReader stream = new StreamReader(filePath, Encoding.UTF8);
                while (!stream.EndOfStream) file += stream.ReadLine();
                stream.Close();
                Console.Write("complete!");
                Console.WriteLine();
            }
            catch (FileNotFoundException fe)
            {
                Console.WriteLine(fe.Message);
                goto begin;
            }
            catch
            {
                Console.WriteLine("Reading finished with errors!");
                goto begin;
            }

            string outputPath = "";
            for (int i = filePath.Count() - 1; i >= 0; i--)
            {
                if (filePath[i].CompareTo('.') == 0)
                {
                    outputPath = filePath.Substring(0, i);
                    break;
                }
            }

            if (file.Substring(0, 8).CompareTo("IVT-142|") == 0)
            {
                Console.Write("This file was zipping later. Reading a Huffman codes...");
                file = file.Substring(8, file.Length - 8);
                int j, count = 0;
                string codeBuf = "";
                char symBuf = 'a';
                coding = new Dictionary<char, string>();
                foreach (char symbol in file)
                {
                    count++;
                    if (int.TryParse(symbol.ToString(), out j) && (j == 0 || j == 1))
                    {
                        codeBuf += symbol;
                    }
                    else
                    { 
                        if (codeBuf.Length > 0)
                        {
                            coding.Add(symBuf, codeBuf);
                            codeBuf = "";
                        }
                        if (symbol.Equals('<')) break;
                        symBuf = symbol;
                    }
                }
                file = file.Substring(count, file.Length - count);
                coding = coding.OrderBy(o => o.Value.Length).ToDictionary(p => p.Key, t => t.Value);

                Console.Write("complete!");
                Console.WriteLine();
                Console.Write("Decoding a bits...");

                int l = 0;
                while (l < file.Length)
                {
                    foreach (KeyValuePair<char, string> p in coding)
                    {
                        try
                        {
                            if (file.Substring(l, p.Value.Length).CompareTo(p.Value) == 0)
                            {
                                file = file.Substring(0, l) + p.Key + file.Substring(l + p.Value.Length);
                                l++;
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            continue;
                        }
                    }
                }

                output = new StreamWriter(outputPath + "_zippo_decoding.txt", false, Encoding.UTF8);
                output.Write(file);
                output.Close();
                goto exit;
            }

            Console.Write("Counting a symbols...");

            Dictionary<char, int> dictionary = new Dictionary<char, int>();
            for (int i = 0; i < file.Length; ++i)
            {
                if (dictionary.ContainsKey(file[i])) dictionary[file[i]]++;
                else dictionary.Add(file[i], 1);
            }

            Console.Write("complete!");
            Console.WriteLine();
            Console.Write("Creating a Huffman tree...");

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
                nodes = nodes.OrderBy(n => n.Weight).ToList();
                HuffmanTreeNode parentNode = new HuffmanTreeNode();
                parentNode.LeftChild = nodes[0];
                parentNode.RightChild = nodes[1];
                parentNode.Weight = parentNode.LeftChild.Weight + parentNode.RightChild.Weight;
                nodes.RemoveAt(0);
                nodes.RemoveAt(0);
                nodes.Add(parentNode);
            }

            Console.Write("complete!");
            Console.WriteLine();
            Console.Write("Creating output file...");

            coding = new Dictionary<char, string>();
            output = new StreamWriter(outputPath+"_zippo_encoding.txt", false, Encoding.UTF8);
            output.WriteLine("IVT-142|");
            printHuffmanTree(nodes[0], output);
            output.WriteLine("<");
            foreach (char symbol in file) output.Write(coding[symbol]);
            output.Close();

            exit:
            Console.Write("complete!");
            Console.WriteLine();
            Console.ReadKey();
        }

        static void printHuffmanTree(HuffmanTreeNode node, StreamWriter output, string code = "")
        {
            if (node.LeftChild == null)
            {
                string print = node.Symbol /*+ ":" + code.Length + "|"*/ + code;
                coding.Add(node.Symbol, code);
                output.WriteLine(print);
            }
            if (node.LeftChild != null) printHuffmanTree(node.LeftChild, output, code+"0");
            if (node.RightChild != null) printHuffmanTree(node.RightChild, output, code+"1");
        }
    }
}