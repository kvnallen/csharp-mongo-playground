using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace MongoTest
{
    public class MongoContext
    {
        public const string ConnectionString = "mongodb://localhost:27017";
        private IMongoDatabase _database;

        public MongoContext()
        {
            
            var mongoConnectionUrl = new MongoUrl(ConnectionString);
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
            mongoClientSettings.ClusterConfigurator = builder =>
            {
                builder.Subscribe<CommandStartedEvent>(e =>
                {
                    Console.WriteLine($"{e.CommandName} - {e.Command.ToJson(new JsonWriterSettings { Indent = true })}");
                });
            };
            var connection = new MongoClient(mongoClientSettings);
            _database = connection.GetDatabase("Users");
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("orders");
    }
}