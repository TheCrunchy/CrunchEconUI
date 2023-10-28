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
        private IListingService listingService { get; set; }
        private ILogger<EventController> logger { get; set; }
        private PlayerBalanceAndNotifyService balanceService { get; set; }
        public EventController(EventService events, PlayerBalanceAndNotifyService balances, IListingService listservice, ILogger<EventController> logger)
        {
            this.eventService = events;
            this.balanceService = balances;
            this.listingService = listservice;
            this.logger = logger;
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
            logger.Log(LogLevel.Information, $"Processing event : {eventMessage.JsonEvent}");
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
                logger.Log(LogLevel.Information, $"Processing event : {eventMessage.JsonEvent}");
                await ProcessEvent(eventMessage);
            }

            return Ok();
        }
        public async Task ProcessEvent(Event eventMessage)
        {
            switch (eventMessage.EventType)
            {
                case EventType.SaveTexturesJson:
                    {
                        await eventService.SaveToJson();
                        break;
                    }

                case EventType.PrefabEvent:
                    {
                        var result = JsonConvert.DeserializeObject<PrefabEvent>(eventMessage.JsonEvent);
                        listingService.SetPrefabs(result.Prefabs);
                        break;
                    }
                case EventType.BuyShipResult:
                    {
                        var result = JsonConvert.DeserializeObject<BuyShipEvent>(eventMessage.JsonEvent);
                        if (result.Result == EventResult.Success)
                        {
                           
                            await eventService.RemoveEvent(result.OriginatingPlayerSteamId, eventMessage.Id);
                            balanceService.SendNotification(result.OriginatingPlayerSteamId, $"Successfully bought {result.ShipListing.ShipName}");
                        }
                        else
                        {
                           await eventService.RemoveEvent(result.OriginatingPlayerSteamId, eventMessage.Id);
                            balanceService.SendNotification(result.OriginatingPlayerSteamId, $"Failed to buy {result.ShipListing.ShipName}, reason <p class=\"PriceRed\">{result.Result}</p>");
                        }
                        await listingService.ArchiveEvent(eventMessage);
                        break;
                    }

                case EventType.ListItemResult:
                    {
   
                        var result = JsonConvert.DeserializeObject<CreateListingEvent>(eventMessage.JsonEvent);
                        if (result.Result == EventResult.Success)
                        {
                            await eventService.RemoveEvent(result.OriginatingPlayerSteamId, eventMessage.Id);
                            await listingService.StoreItem(result.Listing);
                            await listingService.ModifySuspended(result.Listing, false);
                            balanceService.SendNotification(result.OriginatingPlayerSteamId, $"Successfully created listing for {result.Listing.ItemId.Replace("MyObjectBuilder_", "")}");
                        }
                        else
                        {
                            balanceService.SendNotification(result.OriginatingPlayerSteamId, $"Failed to create Listing, reason <p class=\"PriceRed\">{result.Result}</p>");
                            var item = result.Listing;
                            item.Suspended = false;
                            item.Deleted = false;
                            await listingService.ModifySuspended(item, false);
                        }
                        await listingService.ArchiveEvent(eventMessage);
                        break;
                    }
                case EventType.DeleteListing:
                    {
                        var result = JsonConvert.DeserializeObject<DeleteListingEvent>(eventMessage.JsonEvent);
                        if (result.Result == EventResult.Success)
                        {
                            await eventService.RemoveEvent(result.OriginatingPlayerSteamId, eventMessage.Id);
                            await listingService.DeleteListing(result.Listing);
                            balanceService.SendNotification(result.OriginatingPlayerSteamId, $"Successfully deleted {result.Listing.Amount} of {result.Listing.ItemId.Replace("MyObjectBuilder_", "")}");
                        }
                        else
                        {
                            balanceService.SendNotification(result.OriginatingPlayerSteamId, $"Failed to delete Listing, reason <p class=\"PriceRed\">{result.Result}</p>");
                            var item = result.Listing;
                            item.Suspended = false;
                            item.Deleted = false;
                            await listingService.ModifySuspended(item, false);
                        }
                        await listingService.ArchiveEvent(eventMessage);
                        break;
                    }
                case EventType.TextureEvent:
                    {
                        var text = JsonConvert.DeserializeObject<TextureEvent>(eventMessage.JsonEvent);
                        eventService.AddTexture(text.DefinitionId, text);
                        break;
                    }
                case EventType.BalanceUpdate:
                    {
                        var balances = JsonConvert.DeserializeObject<List<BalanceUpdateEvent>>(eventMessage.JsonEvent);
                        foreach (var balance in balances)
                        {
                            balanceService.SetBalance(balance.OriginatingPlayerSteamId, balance.Balance);
                        }
                        break;
                    }
                case EventType.BuyItemResult:
                    {
                        var result = JsonConvert.DeserializeObject<BuyItemEvent>(eventMessage.JsonEvent);
                        if (result.Result == EventResult.Success)
                        {
                            var item = await listingService.GetUpdatedItem(result.ListedItemId);
                            item.Amount -= result.Amount;
                            item.Suspended = false;
                            await listingService.StoreItem(item);
                            if (item.Amount <= 0)
                            {
                                if (item.IsBuying)
                                {
                                    if (item.MaxAmountToBuy <= 0)
                                    {
                                        await listingService.DeleteListing(item);
                                    }
                                }
                                else
                                {
                                    await listingService.DeleteListing(item);
                                }
                            }
                            await eventService.RemoveEvent(result.OriginatingPlayerSteamId, eventMessage.Id);
                            balanceService.SendNotification(result.OriginatingPlayerSteamId, $"Successfully bought {result.Amount} of {result.DefinitionIdString.Replace("MyObjectBuilder_", "")}");
                            await listingService.ModifySuspended(item, false);
                        }
                        else
                        {
                            await eventService.RemoveEvent(result.OriginatingPlayerSteamId, eventMessage.Id);
                            balanceService.SendNotification(result.OriginatingPlayerSteamId, $"Failed to buy {result.Amount} of {result.DefinitionIdString.Replace("MyObjectBuilder_", "")}, reason <p class=\"PriceRed\">{result.Result}</p>");
                            var item = await listingService.GetUpdatedItem(result.ListedItemId);
                            await listingService.ModifySuspended(item, false);
                        }
                        await listingService.ArchiveEvent(eventMessage);
                        break;
                    }

                case EventType.SellItemResult:
                    {
                        var result = JsonConvert.DeserializeObject<BuyItemEvent>(eventMessage.JsonEvent);
                        if (result.Result == EventResult.Success)
                        {
                            var item = await listingService.GetUpdatedItem(result.ListedItemId);
                            item.Amount += result.Amount;
                            item.MaxAmountToBuy -= result.Amount;
                            await listingService.StoreItem(item);
                           await eventService.RemoveEvent(result.OriginatingPlayerSteamId, eventMessage.Id);
                            await listingService.ModifySuspended(item, false);
                            balanceService.SendNotification(result.OriginatingPlayerSteamId, $"Successfully sold {result.Amount} of {result.DefinitionIdString.Replace("MyObjectBuilder_", "")}");
                        }
                        else
                        {
                           await eventService.RemoveEvent(result.OriginatingPlayerSteamId, eventMessage.Id);
                            balanceService.SendNotification(result.OriginatingPlayerSteamId, $"Failed to sell {result.Amount} of {result.DefinitionIdString.Replace("MyObjectBuilder_", "")}, reason <p class=\"PriceRed\">{result.Result}<p>");
                            var item = await listingService.GetUpdatedItem(result.ListedItemId);
                            await listingService.ModifySuspended(item, false);
                        }
                        await listingService.ArchiveEvent(eventMessage);
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
