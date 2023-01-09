using System;
using Basket.API.Entities;
using MongoDB.Driver;

namespace Basket.API.Data
{
    public interface IBasketContext
    {
        IMongoCollection<Cart> Carts { get; }
    }
}
