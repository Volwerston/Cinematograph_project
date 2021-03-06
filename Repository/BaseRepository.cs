﻿using MongoDB.Driver;

namespace Repository
{
    public abstract class BaseRepository
    {
        private const string CONNECTION_STRING = "mongodb://192.168.99.100:32768";

        private readonly static IMongoClient _client = new MongoClient(CONNECTION_STRING);

        protected IMongoDatabase MoviesDatabse => _client.GetDatabase("moviesdb");
    }
}
