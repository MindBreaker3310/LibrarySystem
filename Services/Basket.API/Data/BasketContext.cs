using System;
using Basket.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Basket.API.Data
{
    public class BasketContext:IBasketContext
    {
        public IMongoCollection<Cart> Carts { get; }

        public BasketContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
            var database = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);
            Carts = database.GetCollection<Cart>(configuration["DatabaseSettings:CollectionName"]);
        }
    }
}
