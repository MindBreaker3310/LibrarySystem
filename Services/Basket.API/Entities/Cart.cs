using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Basket.API.Entities
{
    /// <summary>
    /// 購物車
    /// </summary>
    public class Cart
    {
        [BsonId]
        public string UserId { get; set; }

        [BsonElement("Items")]
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public Cart(string userId)
        {
            this.UserId = userId;
        }
    }
}
