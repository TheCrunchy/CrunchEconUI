using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchEconModels.Models.Events
{
    public class Event
    {
        public EventType EventType { get; set; }
        public string JsonEvent { get; set; }
        public Guid EventId = Guid.NewGuid();
    }
}
