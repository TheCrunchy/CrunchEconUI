using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrunchEconModels.Models.Events
{
    [Table("ArchivedEvents")]
    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public EventType EventType { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public EventSource Source { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string JsonEvent { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = Guid.NewGuid();
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime ExpiredAt { get; set; } = DateTime.Now.AddSeconds(30);
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Boolean Waiting { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Boolean Processed { get; set; } = false;
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long OriginatingPlayerId { get; set; }

    }
}
