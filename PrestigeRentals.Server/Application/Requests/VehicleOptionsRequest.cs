using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Requests
{
    public class VehicleOptionsRequest
    {
        public bool Navigation { get; set; }
        public bool HeadsUpDisplay { get; set; }
        public bool HillAssist { get; set; }
        public bool CruiseControl { get; set; }
    }
}
