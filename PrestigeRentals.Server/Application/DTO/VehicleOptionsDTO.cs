using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigeRentals.Domain.Entities;

namespace PrestigeRentals.Application.DTO
{
    public class VehicleOptionsDTO
    {
        public bool Navigation { get; set; }
        public bool HeadsUpDisplay { get; set; }
        public bool HillAssist { get; set; }
        public bool CruiseControl { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
    }
}
