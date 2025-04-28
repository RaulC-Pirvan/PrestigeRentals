using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Exceptions
{
    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException(long orderId) : base($"The order with Id {orderId} was not found.") { }
    }

    public class OrderAlreadyCancelledException : Exception
    {
        public OrderAlreadyCancelledException(long orderId) : base($"The order with Id {orderId} is already cancelled.") { }
    }
}
