using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Api.Orders.Models
{
    public class Order
    {
        public int CustomerId { get; set; }
        public string TourName { get; set; }
        public string Email { get; set; }
        public bool IsNeedsTransport { get; set; }
    }
}
