using System;
using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<DocumentItem> DocumentItems { get; }

        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
            var database = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);
            DocumentItems = database.GetCollection<DocumentItem>(configuration["DatabaseSettings:CollectionName"]);
            CatalogContextSeed.SeedData(DocumentItems);
        }

    }
}
