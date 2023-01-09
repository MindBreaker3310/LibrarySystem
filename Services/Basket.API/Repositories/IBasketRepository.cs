using System;
using System.Threading.Tasks;
using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<Cart> CreateNewBasket(string userId);
        Task<Cart> GetBasket(string userId);
        Task<bool> UpdateBasket(Cart basket);
        Task<bool> DeleteBasket(string userId);
    }
}
