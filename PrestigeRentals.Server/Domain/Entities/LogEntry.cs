using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Domain.Entities
{
    public class LogEntry
    {
        public int Id { get; set; }
        public string? Path { get; set; }
        public string? Method { get; set; }
        public int? StatusCode { get; set; }
        public string? RequestBody { get; set; }
        public string? ResponseBody { get; set; }
        public string? UserId { get; set; }
        public long DurationMs { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
