using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Domain.Entities
{
    public class VehicleOptions
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("VehicleId")]
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        [Required]
        public bool Navigation { get; set; } = false;
        [Required]
        public bool HeadsUpDisplay { get; set; } = false;
        [Required]
        public bool HillAssist { get; set; } = false;
        [Required]
        public bool CruiseControl { get; set; } = false;
        [Required]
        public bool Active { get; set; } = true;
        [Required]
        public bool Deleted { get; set; } = false;

    }
}
