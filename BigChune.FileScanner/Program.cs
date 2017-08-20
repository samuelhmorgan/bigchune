using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigChune.Scanner.Scan;

namespace BigChune.Scanner
{
    class Program
    {
        static void Main(string[] args)
        {
            MediaScanner scanner = new MediaScanner();
            scanner.TreeScan( "E:\\Music");
            Console.ReadLine();
        }
    }
}
