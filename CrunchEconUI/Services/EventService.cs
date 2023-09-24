using CrunchEconModels.Models.Events;
using CrunchEconUI.Helpers;
using Microsoft.AspNetCore.Http.Features;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace CrunchEconUI.Services
{
    public class EventService
    {
        private const string V = "https://w7.pngwing.com/pngs/327/920/png-transparent-space-engineers-medieval-engineers-engineering-outer-space-space-survival-simulator-3d-others-text-logo-engineering.png";
        private Dictionary<ulong, List<Event>> playersEvents = new();
        private Dictionary<string, TextureEvent> Textures = new();
        private IWebHostEnvironment Environment;

        public EventService(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public Dictionary<ulong, List<Event>> GetPlayersEvents(List<ulong> players)
        {
            var events = playersEvents.Where(x => players.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            foreach (var ev in events)
            {
                playersEvents.Remove(ev.Key);
            }
            return events;
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
                playersEvents.Add(playerId, new List<Event>() { playerEvent });
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

        public void AddTexture(string definition, TextureEvent texture)
        {
            Textures.Remove(definition);

            try
            {
                //{AppDomain.CurrentDomain.BaseDirectory}

                var path = @$"{this.Environment.WebRootPath}\{texture.TexturePath}";
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllBytes($"{path}", texture.Base64Texture);
                if (path.Contains(".dds"))
                {
                    DDSImageReader.ConvertFile($"{path}");
                    texture.TexturePath = texture.TexturePath.Replace(".dds", ".png");
                    File.Delete(path);
                }

                Textures.Add(definition, texture);
            }
            catch (Exception)
            {
            }

        }

        public string GetTexture(string definition)
        {
            if (definition == null)
            {
                return V;
            }
            if (!Textures.ContainsKey(definition))
            {
                return V;

            }
            var path = @$"{Textures[definition].TexturePath}";
            return path;
        }

        public string GetAlt(string definition)
        {
            if (!Textures.ContainsKey(definition))
            {
                return "Texture not found";
            }
            return Textures[definition].TexturePath;
        }
    }
}
