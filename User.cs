using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoTest
{
    public record User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }

        public string Name { get; init; }
        public ObjectId MainProduct { get; set; }
        public IEnumerable<ObjectId> Products { get; set; }
    }

    public record Order
    {
        public Order()
        {
            Id = ObjectId.GenerateNewId();
        }
        
        [BsonId]
        public ObjectId Id { get; set; }
        public string Product { get; init; }
    }
}