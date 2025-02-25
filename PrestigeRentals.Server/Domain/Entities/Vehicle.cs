using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Domain.Entities
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Make {  get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public decimal EngineSize { get; set; }
        [Required]
        public string FuelType {  get; set; }
        [Required]
        public string Transmission {  get; set; }
    }
}
