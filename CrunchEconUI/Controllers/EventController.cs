using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using CrunchEconUI.Models;
using CrunchEconUI.Interfaces;
using System.Security.Claims;
using Newtonsoft.Json;
using CrunchEconModels.Models;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using CrunchEconUI.Services;
using CrunchEconModels.Models.Events;

namespace CrunchEconUI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EventController : ControllerBase
    {
        private EventService eventService { get; set; }
        private IListingsService listingService { get; set; }
        private PlayerBalanceService balanceService { get; set; }
        public EventController(EventService events, PlayerBalanceService balances, IListingsService listservice)
        {
            this.eventService = events;
            this.balanceService = balances;
            this.listingService = listservice;
        }

        [HttpPost]
        public IActionResult GetEventsForPlayers([FromBody] object jsonMessage)
        {
            var message = JsonConvert.DeserializeObject<APIMessage>(jsonMessage.ToString());

            if (message.APIKEY != Program.APIKEY)
            {
                return Unauthorized("API KEY IS NOT VALID");
            }
            var events = eventService.GetPlayersEvents(JsonConvert.DeserializeObject<List<ulong>>(message.JsonMessage));
            return Ok(JsonConvert.SerializeObject(events));
        }

        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] object jsonMessage)
        {
            var message = JsonConvert.DeserializeObject<APIMessage>(jsonMessage.ToString());

            if (message.APIKEY != Program.APIKEY)
            {
                return Unauthorized("API KEY IS NOT VALID");
            }
            var eventMessage = JsonConvert.DeserializeObject<Event>(message.JsonMessage.ToString());
            await ProcessEvent(eventMessage);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PostMultipleEvents([FromBody] object jsonMessage)
        {
            var message = JsonConvert.DeserializeObject<APIMessage>(jsonMessage.ToString());

            if (message.APIKEY != Program.APIKEY)
            {
                return Unauthorized("API KEY IS NOT VALID");
            }

            var eventMessages = JsonConvert.DeserializeObject<List<Event>>(message.JsonMessage.ToString());
            foreach (var eventMessage in eventMessages)
            {
                await ProcessEvent(eventMessage);
            }
           
            return Ok();
        }
        public async Task ProcessEvent(Event eventMessage){
            switch (eventMessage.EventType)
            {
                case EventType.BalanceUpdate:
                    var balances = JsonConvert.DeserializeObject<List<BalanceUpdateEvent>>(eventMessage.JsonEvent);
                    foreach (var balance in balances)
                    {
                        balanceService.SetBalance(balance.OriginatingPlayerSteamId, balance.Balance);
                    }
                    break;
                case EventType.BuyItemResult:
                    var result = JsonConvert.DeserializeObject<BuyItemEvent>(eventMessage.JsonEvent);
                    if (result.Result == EventResult.Success)
                    {
                        eventService.RemoveEvent(result.OriginatingPlayerSteamId, eventMessage.EventId);
                    }
                    else
                    {
                        var item = await listingService.GetUpdatedItem(result.ListedItemId);
                        await listingService.ModifySuspended(item, false);
                    }
                    break;
                case EventType.SellItemResult:
                    break;
                case EventType.ListItemResult:
                    break;
                default:
                    break;
            }
        }
    }
}
