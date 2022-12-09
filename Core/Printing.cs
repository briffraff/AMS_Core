using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AMS2_core.Core
{
    public class Printing
    {
        public void PrintingResults(int skuCounter, int skuCounterNonGeometry)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Total assigned skus: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{skuCounter}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Total UNassigned skus: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{skuCounterNonGeometry}");

            Console.WriteLine();
            Console.WriteLine("*Press ENTER to EXIT");
            Console.ReadLine();
            Console.WriteLine();
        }

        public void PrintingScanResults(Stopwatch watch, HashSet<string> allFiles)
        {
            Console.WriteLine($"files count : {allFiles.Count}");
            Console.WriteLine($"Execution Time: {watch.Elapsed.TotalSeconds} sec");
        }

        public void ReachOutTheLimits(int maxSkus, string sku, Dictionary<string, string> dict)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"You've reach out of capacity: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{maxSkus}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" - ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{dict[sku]}");
        }

        public void ToolHeader(string name, string description, string version)
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine($"--*** {name} ***--");
            Console.WriteLine($"---*** {description} ***---");
            Console.WriteLine($"--------------{version}------------");
        }

        public void NewLine()
        {
            Console.WriteLine();
        }
    }
}
