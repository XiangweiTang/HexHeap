using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HexHeap
{
    class Program
    {
        static void Main(string[] args)
        {
            Expend S = new Expend();

            var list=S.Print(20, 1);
            File.WriteAllLines("1.txt", list);
        }
    }
}
