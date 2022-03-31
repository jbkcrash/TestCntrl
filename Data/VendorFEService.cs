namespace TestCntrl.Data;

// JSON
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public class VendorFEService
{
    private const string Path = "vendorfes.json";
    private static JsonArray vendorfeArray = new JsonArray();

    public Task<VendorFEObject[]> GetVendorFE(string strPosition)
    {
        //Pull in our configuration
        var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

        string jsonDoc = File.ReadAllText(config["DataFolder"] + "\\" + Path);
        List<VendorFEObject> VendorFEList = new List<VendorFEObject>();

        if (jsonDoc is not null) {
            //TODO fix null warning...

            //TODO Wrap in Try/Catch?
            JsonNode VendorFEDoc = JsonNode.Parse(jsonDoc); 

            // Get a JSON array from a JsonNode.
            if( VendorFEDoc is not null) {

                //Iterate of each position in the bench, extract the arrays and create our list(s).
                JsonObject positionsObject = VendorFEDoc["vendorfes"].AsObject();
                JsonArray targetPosition = positionsObject[strPosition].AsArray();

                foreach(JsonNode vendorObject in targetPosition) {
                    Console.WriteLine($"Targets={vendorObject["vendorId"].ToString()}");

                    string vendorIdVar = vendorObject["vendorId"].ToString();
                    string noteVar = vendorObject["note"].ToString();

                    VendorFEList.Add(new VendorFEObject {
                        vendorId = vendorIdVar,
                        note = noteVar
                    });
                }
            }   
        }
 
        return Task.FromResult( VendorFEList.ToArray());
    }
}