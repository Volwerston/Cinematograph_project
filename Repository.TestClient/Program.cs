using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var moviesRepo = new MoviesRepository();
            var titleSearch = moviesRepo.FindAll();
            var lines = titleSearch.Select(m => $"\"{m.Title.Replace("\"", "\\\"").Replace("(", "[").Replace(")", "]")}\"," +
            $"\"{m.Title.Replace("\"", "\\\"").Replace("(", "[").Replace(")", "]")}\"\n").ToArray();
            System.IO.File.WriteAllLines(@"D:\data.csv", lines);
        }
    }
}
