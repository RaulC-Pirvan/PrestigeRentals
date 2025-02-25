using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PrestigeRentals.Application.DTO
{
    public class CreateVehicleDTO
    {
        public string Make {get; set;}
        public string Model { get; set;}
        public decimal EngineSize { get; set;}
        public string FuelType { get; set;}
        public string Transmission {  get; set;}

    }
}
