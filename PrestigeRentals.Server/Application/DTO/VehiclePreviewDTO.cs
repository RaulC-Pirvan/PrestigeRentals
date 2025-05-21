using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.DTO
{
    public class VehiclePreviewDTO
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public byte[] PhotoBase64 { get; set; }
    }
}
