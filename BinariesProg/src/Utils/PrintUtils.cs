using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaries.src.Utils
{
    internal class PrintUtils
    {
        public static void PrintDepth(string msg, int depth)
        {
            for(int i = 0; i < depth; i++)
            {
                Console.Write('\t');
            }
            Console.WriteLine(msg);
        }
    }
}
