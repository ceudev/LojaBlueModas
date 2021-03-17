using System;
using System.Collections.Generic;
using System.Text;

namespace BlueModas.Domain.Models
{
    public class StockOnHold
    {
        public int Id { get; set; }

        public string SessionId { get; set; }

        public int StockId { get; set; }
        public Stock Stock { get; set; }

        public int Quantity { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
