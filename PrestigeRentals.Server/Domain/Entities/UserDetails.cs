using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PrestigeRentals.Domain.Entities
{
    public class UserDetails
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("UserID")]
        public int UserID { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public byte[] ImageData { get; set; }
        [Required]
        public bool Active { get; set; } = true;
        [Required]
        public bool Deleted { get; set; } = false;
    }
}
