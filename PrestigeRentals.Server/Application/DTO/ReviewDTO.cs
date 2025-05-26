using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.DTO
{
    public class ReviewDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long VehicleId { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
