using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Requests
{
    public class CreatePaymentRequest
    {
        public long OrderId { get; set; }
        public decimal TotalCost { get; set; }
        public long UserId { get; set; }
        public long VehicleId { get; set; }

    }
}
