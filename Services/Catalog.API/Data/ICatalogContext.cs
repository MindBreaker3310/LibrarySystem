using System;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public interface ICatalogContext
    {
        IMongoCollection<DocumentItem> DocumentItems { get; }
    }
}
