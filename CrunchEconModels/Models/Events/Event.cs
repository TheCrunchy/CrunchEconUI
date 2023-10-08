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
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime ExpiredAt { get; set; } = DateTime.Now.AddSeconds(30);
    }
}
