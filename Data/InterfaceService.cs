namespace TestCntrl.Data;

// JSON
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public class InterfaceService
{
    private const string Path = "interfaces.json";
    private static JsonArray interfaceArray = new JsonArray();

    public Task<InterfaceObject[]> GetInterfaces()
    {
        //Pull in our configuration
        var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

        string jsonDoc = File.ReadAllText(config["DataFolder"] + "\\" + Path);
        List<InterfaceObject> InterfaceList = new List<InterfaceObject>();

        if (jsonDoc is not null) {
            //TODO fix null warning...

            //TODO Wrap in Try/Catch?
            JsonNode InterfaceDoc = JsonNode.Parse(jsonDoc); 

            // Get a JSON array from a JsonNode.
            if( InterfaceDoc is not null) {
                interfaceArray = InterfaceDoc["interfaces"].AsArray();


                foreach (JsonNode Interface in interfaceArray) {
                    //Console.WriteLine($"Object={Interface.ToJsonString()}");
                    string interfaceIdVar = Interface["interfaceId"].ToString();
                    string descriptionVar = Interface["description"].ToString();


                    InterfaceList.Add(new InterfaceObject {
                        interfaceId = interfaceIdVar,
                        description = descriptionVar
                    });
                }
            }   
        }
 
        return Task.FromResult( InterfaceList.ToArray());
    }
}