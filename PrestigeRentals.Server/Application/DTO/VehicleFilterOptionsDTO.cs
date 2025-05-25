using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.DTO
{
    public class VehicleFilterOptionsDto
    {
        public List<string> Makes { get; set; } = new();
        public List<string> Models { get; set; } = new();
        public List<string> FuelTypes { get; set; } = new();
        public List<string> Transmissions { get; set; } = new();
        public List<string> Chassis { get; set; } = new();
    }
}
