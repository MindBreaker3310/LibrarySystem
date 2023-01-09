using System;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Ordering.API.Entities;

namespace Ordering.API.Data
{
    public class OrderingContext : IOrderingContext
    {
        public IMongoCollection<OrderRecord> OrderRecords { get; }

        public OrderingContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
            var database = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);
            OrderRecords = database.GetCollection<OrderRecord>(configuration["DatabaseSettings:CollectionName"]);
        }

    }
}
