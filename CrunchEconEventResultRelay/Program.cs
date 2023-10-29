using System.Reflection;
using System.IO;
using Microsoft.Extensions.Configuration;
using System;
using CrunchEconModels.Models;
using CrunchEconModels;
using CrunchEconModels.Models.EntityFramework;
using RestSharp;
using Newtonsoft.Json;

internal class Program
{
    public static string APIKEY = "";
    public static string UIURL = "";
    public static string DBString;
    private static void Main(string[] args)
    {
        var path = $"{Directory.GetCurrentDirectory()}/appsettings.json";
        if (File.Exists(path))
        {
            var builder = new ConfigurationBuilder().AddJsonFile(path, true, true);
            var config = builder.Build();
            DBString = config.GetSection("DBString").Value;
            APIKEY = config.GetSection("ApiKey").Value;
            UIURL = config.GetSection("UIURL").Value;
            DBService.Setup();
            Task.Run(async () =>
            {
                while (true)
                {
                    await OnTimedEventA();
                    await Task.Delay(1000);
                }
            });

            while (true)
            {
                Console.ReadLine();
            }
        }
   
    }

    private static string MyDirectory()
    {
        return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }

    public static class DBService
    {
        public static EconContext Context { get; set; }
        public static void Setup()
        {
           Context = new EconContext(Program.DBString);
        }
    }
    private static List<Guid> Processed = new List<Guid>();
    private static async Task OnTimedEventA()
    {
        try
        {
            Console.WriteLine($"{DateTime.Now} Reading events");
            var events = DBService.Context.ArchivedEvents.Where(x => x.Processed && x.Source == EventSource.Torch).ToList();
            var client = new RestClient($"{UIURL}api/Event/GetEventsForPlayers");
            var request = new RestRequest();
            var message = new APIMessage();
            events = events.Where(x => !Processed.Contains(x.Id)).ToList();
            if (events.Any())
            {
       
                Console.WriteLine($"{DateTime.Now} {events.Count} to post");
                client = new RestClient($"{UIURL}api/Event/PostMultipleEvents");
                request = new RestRequest();

                message = new APIMessage();
                message.APIKEY = APIKEY;

                message.JsonMessage = JsonConvert.SerializeObject(events);
                request.AddStringBody(JsonConvert.SerializeObject(message), DataFormat.Json);

                var result = await client.PostAsync(request);
                Console.WriteLine($"{DateTime.Now} Posted Events");
            }
            Processed.AddRange(events.Select(x => x.Id));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }
}