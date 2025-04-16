using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigeRentals.Application.DTO
{
    public class LogEntryDTO
    {
        public string Path {  get; set; }
        public string Method { get; set; }
        public int StatusCode { get; set; }
        public long DurationMs { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
