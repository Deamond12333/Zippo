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
            FileStream stream = new FileStream(filePath, FileMode.Open);
            byte[] array = new byte[(int)stream.Length];
            if (stream.Read(array, 0, (int)stream.Length) == 0) Console.WriteLine("Reading successfull complete!");
            else
            {
                Console.WriteLine("Reading complete with errors...");
                return;
            }
            stream.Close();
            string file = Encoding.UTF8.GetString(array);
            Console.WriteLine("Counting a symbols...");
            Dictionary<char, int> dictionary = new Dictionary<char, int>();
            int size = 0;
            for (int i = 0; i < file.Length; ++i)
            {
                int symbValue;
                if (int.TryParse(file[i].ToString(), out symbValue)) size += 1;
                else size += 2;
                for (int j = 0; j < dictionary.Count; ++j)
                {
                    int value = 1;
                    if (dictionary.TryGetValue(file[i], out value))
                    {
                        value++;
                        dictionary.Remove(file[i]);
                    }
                    dictionary.Add(file[i], value);
                }
            }
            Console.WriteLine("Counting symbols successfull complete! Approximate file size: "+size+" bytes");
            Console.WriteLine("Counting probabilities...");
            Dictionary<char, double> probability = new Dictionary<char, double>();
            foreach(var symbol in dictionary.OrderBy(symbol => symbol.Value)) probability.Add(symbol.Key, symbol.Value / dictionary.Count);

            Console.ReadKey();
        }
    }
}
