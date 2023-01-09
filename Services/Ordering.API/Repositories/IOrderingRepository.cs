using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.API.Entities;

namespace Ordering.API.Repositories
{
    public interface IOrderingRepository
    {
        Task<bool> CreateOrderRecord(OrderRecord orderRecord);

        Task<List<OrderRecord>> GetOrderRecord(string Borrower);

        Task<bool> UpdateOrderRecord();

        Task<bool> DeleteOrderRecord();
    }
}
