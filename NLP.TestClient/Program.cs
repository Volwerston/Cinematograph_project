using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new DialogFlowService();

            var resp = service.Ask("What is Harry Potter rate");
        }
    }
}
