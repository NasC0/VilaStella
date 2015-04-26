using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VilaStella.ConsoleTesting
{
    class ConsoleTesting
    {
        static void Main()
        {
            var biggerDate = new DateTime(2020, 8, 15);
            var smallerDate = new DateTime(2014, 8, 15);

            var difference = biggerDate - smallerDate;
            Console.WriteLine(difference.Days);
        }
    }
}
