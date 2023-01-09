using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Ordering.API.Data;
using Ordering.API.Entities;
using System.Collections.Generic;

namespace Ordering.API.Repositories
{
    public class OrderingRepository : IOrderingRepository
    {
        private readonly IOrderingContext _context;

        public OrderingRepository(IOrderingContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateOrderRecord(OrderRecord orderRecord)
        {
            try
            {
                await _context.OrderRecords.InsertOneAsync(orderRecord);
                return true;
            }
            catch (Exception ex)
            {
                //寫入失敗
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        Task<bool> IOrderingRepository.DeleteOrderRecord()
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderRecord>> GetOrderRecord(string Borrower)
        {
            var list = await _context.OrderRecords.Find(order => order.Id == Borrower && order.Completed == false).ToListAsync();
            return list;
        }

        Task<bool> IOrderingRepository.UpdateOrderRecord()
        {
            throw new NotImplementedException();
        }
    }
}
