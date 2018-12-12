using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageProcessor mp = new MessageProcessor();
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "q") break;

                Console.WriteLine(mp.ProcessMessage(input));
            }
        }
    }
}
