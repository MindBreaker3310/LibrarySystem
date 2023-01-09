using System;
using MongoDB.Driver;
using Ordering.API.Entities;

namespace Ordering.API.Data
{
    public interface IOrderingContext
    {
        IMongoCollection<OrderRecord> OrderRecords { get; }
    }
}
