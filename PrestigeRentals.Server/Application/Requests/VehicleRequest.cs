using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PrestigeRentals.Application.Requests
{
    public class VehicleRequest
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public decimal EngineSize { get; set; }
        public string FuelType { get; set; }
        public string Transmission { get; set; }

    }
}
