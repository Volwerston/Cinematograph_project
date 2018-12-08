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
            var titleSearch = moviesRepo.SearchByTitle("King");
            var genresSearch = moviesRepo.SearchByGenre(new List<string> { "sci-fi" }, new PaginationRequest { Size = 25, Page = 1 });
        }
    }
}
