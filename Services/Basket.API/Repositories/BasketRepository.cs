using System;
using System.Threading.Tasks;
using Basket.API.Data;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _context;

        public BasketRepository(IBasketContext context)
        {
            _context = context;
        }

        public async Task<Cart> CreateNewBasket(string userId)
        {
            bool hasData = _context.Carts.Find(cart => cart.UserId == userId).Any();
            if (!hasData)
            {
                await _context.Carts.InsertOneAsync(new Cart(userId));
            }
            return await _context.Carts.Find(cart => cart.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<Cart> GetBasket(string userId)
        {
            return await _context.Carts.Find(cart => cart.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateBasket(Cart basket)
        {
            var updateResult = await _context.Carts.ReplaceOneAsync(filter: cart => cart.UserId == basket.UserId, replacement: basket);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount >= 1;
        }

        public async Task<bool> DeleteBasket(string userId)
        {
            var deleteResult = await _context.Carts.DeleteOneAsync(filter: cart => cart.UserId == userId);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount >= 1;
        }


    }
}
