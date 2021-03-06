namespace TestCntrl.Data;

// JSON
using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;



public class EventService
{
    private const string Path = "event.json";
    private static JsonArray eventArray = new JsonArray();

    public Task<EventObject[]> GetEvent()
    {
        //Pull in our configuration
        var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

        string jsonDoc = File.ReadAllText(config["DataFolder"] + "\\" + Path);
        List<EventObject> EventList = new List<EventObject>();

        if (jsonDoc is not null) {
            //TODO fix null warning...

            //TODO Wrap in Try/Catch?
            JsonNode EventDoc = JsonNode.Parse(jsonDoc); 

            // Get a JSON array from a JsonNode.
            if( EventDoc is not null) {
                eventArray = EventDoc["event"].AsArray();


                foreach (JsonNode Event in eventArray) {
                    //Console.WriteLine($"Object={Event.ToJsonString()}");
                    string eventIdVar = Event["eventId"].ToString();
                    string descriptionVar = Event["description"].ToString();
                    string dateVar = Event["date"].ToString();


                    EventList.Add(new EventObject {
                        eventId = eventIdVar,
                        description = descriptionVar,
                        date = dateVar
                    });
                }
            }   
        }
 
        return Task.FromResult( EventList.ToArray());
    }
}