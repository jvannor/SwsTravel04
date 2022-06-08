using System;
using System.Collections.Generic;

namespace ChannelAdapter.Models
{
    public partial class Event
    {
        public int EventId { get; set; }
        public string EventCode { get; set; } = null!;
        public DateTime Scheduled { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime? Completed { get; set; }
    }
}
