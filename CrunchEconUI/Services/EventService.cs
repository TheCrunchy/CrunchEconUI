using CrunchEconModels.Models.Events;
using CrunchEconUI.Helpers;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
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
            var path = @$"{this.Environment.WebRootPath}\Textures.Json";
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                Textures = JsonConvert.DeserializeObject<Dictionary<string, TextureEvent>>(text);
            }
        }
        public async Task SaveToJson()
        {
            var path = @$"{this.Environment.WebRootPath}\Textures.Json";
            var textures = JsonConvert.SerializeObject(Textures, Formatting.Indented);
            await File.WriteAllTextAsync(path, textures);
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

        public async Task AddEvent(ulong playerId, Event playerEvent)
        {
            if (DBService.Context.ArchivedEvents.Any(x => x.Id == playerEvent.Id))
            {
                var playersEvent = DBService.Context.ArchivedEvents.FirstOrDefault(x => x.Id == playerEvent.Id);
                playersEvent.OriginatingPlayerId = (long)playerId;
                playersEvent.Waiting = true;
                playersEvent.Source = CrunchEconModels.Models.EventSource.Web;
            }
            else
            {
                playerEvent.OriginatingPlayerId = (long)playerId;
                playerEvent.Waiting = true;
                playerEvent.Source = CrunchEconModels.Models.EventSource.Web;
                DBService.Context.ArchivedEvents.Add(playerEvent);

            }
          

           await DBService.Context.SaveChangesAsync();
        }

        public async Task RemoveEvent(ulong playerId, Guid eventId)
        {

            if (DBService.Context.ArchivedEvents.Any(x => x.Id == eventId))
            {
                var playersEvent = DBService.Context.ArchivedEvents.FirstOrDefault(x => x.Id == eventId);
                playersEvent.Processed = true;
                playersEvent.Source = CrunchEconModels.Models.EventSource.Archived;
                playersEvent.Waiting = false;
                await DBService.Context.SaveChangesAsync();
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
                texture.Base64Texture = null;
                Textures.Add(definition, texture);
            }
            catch (Exception)
            {
            }

        }

        public List<String> GetAllIds()
        {
            return Textures.Values.Select(x => x.DefinitionId.Replace("MyObjectBuilder_", "")).ToList();
        }

        public string GetTexture(string definition)
        {

            if (definition == null)
            {
                return V;
            }
            if (!definition.StartsWith("MyObjectBuilder_"))
            {
                definition = "MyObjectBuilder_" + definition;
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
