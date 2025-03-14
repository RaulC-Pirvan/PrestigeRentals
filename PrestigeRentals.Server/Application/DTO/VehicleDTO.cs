using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PrestigeRentals.Application.DTO
{
    public class VehicleDTO
    {
        public string Make {get; set;}
        public string Model { get; set;}
        public decimal EngineSize { get; set;}
        public string FuelType { get; set;}
        public string Transmission {  get; set;}
        public bool Navigation { get; set; }
        public bool HeadsUpDisplay { get; set; }
        public bool HillAssist { get; set; }
        public bool CruiseControl { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }

    }
}
