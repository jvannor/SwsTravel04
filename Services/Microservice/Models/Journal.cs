using System;
using System.Collections.Generic;

namespace Microservice.Models
{
    public partial class Journal
    {
        public int JournalId { get; set; }
        public string? JournalEntry { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
