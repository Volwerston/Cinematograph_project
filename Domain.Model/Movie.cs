using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Domain.Model
{
    [BsonIgnoreExtraElements]
    public class Movie
    {
        [BsonIgnoreExtraElements]
        public class ImdbData
        {
            [BsonElement("id")]
            public String Id { get; set; }

            [BsonElement("rating")]
            public double? Rating { get; set; }

            [BsonElement("votes")]
            public int? Votes { get; set; }
        }

        [BsonIgnoreExtraElements]
        public class TomatoData
        {
            [BsonElement("meter")]
            public int? Meter { get; set; }

            [BsonElement("rating")]
            public double? Rating { get; set; }

            [BsonElement("reviews")]
            public int? Reviews { get; set; }

            [BsonElement("consensus")]
            public String Consensus { get; set; }
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public String Id { get; set; }

        [BsonElement("title")]
        public String Title { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("genres")]
        public List<String> Genres { get; set; }

        [BsonElement("director")]
        public String Director { get; set; }

        [BsonElement("writers")]
        public List<String> Writers { get; set; }

        [BsonElement("actors")]
        public List<String> Actors { get; set; }

        [BsonElement("imdb")]
        public ImdbData ImdbRate { get; set; }

        [BsonElement("tomato")]
        public TomatoData TomatoRate { get; set; }

        [BsonElement("metacritic")]
        public int MetacriticRate { get; set; }

        [BsonElement("plot")]
        public String Plot { get; set; }
    }
}
