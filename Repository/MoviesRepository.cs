using Domain.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class PaginationRequest
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;

        public int StartIndex => (Page - 1) * Size;
    }

    public class PaginatedResponse<T>
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public long TotalSize { get; set; }

        public List<T> Data { get; set; }
    }

    public interface IMoviesRepository
    {
        List<Movie> SearchByTitle(string title);
    }

    public class MoviesRepository : BaseRepository, IMoviesRepository
    {
        static MoviesRepository()
        {
            BsonClassMap.RegisterClassMap<Movie>();
        }

        private IMongoCollection<Movie> _moviesCollection => MoviesDatabse.GetCollection<Movie>("movies");

        private FilterDefinitionBuilder<Movie> _filterBuilder => Builders<Movie>.Filter;

        public List<Movie> SearchByTitle(string title)
        {
            var filter = _filterBuilder.Regex("title", new BsonRegularExpression(title, "i"));

            return _moviesCollection.Find(filter).ToList();
        }

        public List<Movie> FindAll()
        {
            return _moviesCollection.Find(_filterBuilder.Empty).ToList();
        }

        public PaginatedResponse<Movie> SearchByGenre(List<string> genres, PaginationRequest paginationRequest)
        {
            var filter = new BsonDocument(new BsonElement("genres", new BsonDocument(
                new BsonElement("$elemMatch", 
                new BsonDocument(
                    new BsonElement("$in", 
                    new BsonArray(MatchToPattern(genres))))))));

            var query = _moviesCollection.Find(filter).SortBy(m => m.MetacriticRate).SortBy(m => m.Id);

            var total = query.CountDocuments();
            var result = query.Skip(paginationRequest.StartIndex).Limit(paginationRequest.Size).ToList();

            return new PaginatedResponse<Movie>
            {
                Page = paginationRequest.Page,
                Size = result.Count,
                TotalSize = total,
                Data = result
            };
        }

        private List<BsonRegularExpression> MatchToPattern(List<string> elements)
        {
            return elements.Select(x => new BsonRegularExpression(x, "i")).ToList();
        }
    }
}
