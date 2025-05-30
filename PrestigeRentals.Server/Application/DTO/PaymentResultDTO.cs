using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.DTO
{
    public class PaymentResultDTO
    {
        public bool Success { get; set; }
        public string BookingReference { get; set; }
        public string QrCodeData { get; set; }
        public string ErrorMessage { get; set; }
    }
}
