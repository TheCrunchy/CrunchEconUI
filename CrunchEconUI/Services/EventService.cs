using CrunchEconModels.Models.Events;
using Microsoft.AspNetCore.Http.Features;
using System.Linq;

namespace CrunchEconUI.Services
{
    public class EventService
    {
        private Dictionary<ulong, List<Event>> playersEvents = new();
        
        public Dictionary<ulong, List<Event>> GetPlayersEvents(List<ulong> players)
        {
            return playersEvents.Where(x => players.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }

        public void AddEvent(ulong playerId, Event playerEvent)
        {
            if (playersEvents.ContainsKey(playerId))
            {
                var items = playersEvents[playerId];
                items.Add(playerEvent);
                playersEvents[playerId] = items;
            }
            else
            {
                playersEvents.Add(playerId, new List<Event>() { playerEvent} );
            }
        }

        public void RemoveEvent(ulong playerId, Guid eventId)
        {
            if (playersEvents.ContainsKey(playerId))
            {
                var events = playersEvents[playerId];
                var removing = events.FirstOrDefault(x => x.EventId == eventId);
                if (removing != null)
                {
                    events.Remove(removing);
                }
            }
        }
    }
}
