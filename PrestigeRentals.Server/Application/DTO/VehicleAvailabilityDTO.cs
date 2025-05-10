using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.DTO
{
    public class VehicleAvailabilityDTO
    {
        public long VehicleID { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? AvailableAt { get; set; }
    }
}
