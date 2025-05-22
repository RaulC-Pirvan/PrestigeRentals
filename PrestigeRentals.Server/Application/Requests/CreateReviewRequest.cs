using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.Requests
{
    public class CreateReviewRequest
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public long VehicleId { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
