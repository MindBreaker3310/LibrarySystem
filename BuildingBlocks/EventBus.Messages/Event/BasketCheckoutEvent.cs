using System;
using System.Collections.Generic;

namespace EventBus.Messages.Event
{
    public class BasketCheckoutEvent : IntegrationBaseEvent
    {
        public string UserName { get; set; }
        public List<BorrowItem> BorrowItems { get; set; } = new List<BorrowItem>();
    }

    public class BorrowItem
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public DateTime? ReturnTime { get; set; }
    }
}
