using System;
using System.Collections.Generic;
using EventBus.Messages.Event;
using MongoDB.Bson.Serialization.Attributes;

namespace Ordering.API.Entities
{
    public class OrderRecord
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("Borrower")]
        public string Borrower { get; set; }
        [BsonElement("BorrowItems")]
        public List<BorrowItem> BorrowItems { get; set; } = new List<BorrowItem>();
        [BsonElement("BorrowTime")]
        public DateTime BorrowTime { get; set; }
        [BsonElement("Completed")]
        public bool Completed { get; set; }
        [BsonElement("CompletedTime")]
        public DateTime? CompletedTime { get; set; }
    }
}
