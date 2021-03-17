using BlueModas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueModas.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderRef { get; set; }
        public string StripeReference { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }

        public OrderStatus Status { get; set; }

        public ICollection<OrderStock> OrderStocks { get; set; }
    }
}
