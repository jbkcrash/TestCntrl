namespace TestCntrl.Data;

// JSON
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public class VendorService
{
    private const string Path = "vendors.json";
    private static JsonArray vendorArray = new JsonArray();

    public Task<VendorObject[]> GetVendor()
    {
        //Pull in our configuration
        var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();
                 
        string jsonDoc = File.ReadAllText(config["DataFolder"] + "\\" + Path);
        List<VendorObject> VendorList = new List<VendorObject>();

        if (jsonDoc is not null) {
            //TODO fix null warning...

            //TODO Wrap in Try/Catch?
            JsonNode VendorDoc = JsonNode.Parse(jsonDoc); 

            // Get a JSON array from a JsonNode.
            if( VendorDoc is not null) {
                vendorArray = VendorDoc["vendors"].AsArray();


                foreach (JsonNode Vendor in vendorArray) {
                    //Console.WriteLine($"Object={Vendor.ToJsonString()}");
                    string vendorIdVar = Vendor["vendorId"].ToString();
                    string vendorNameVar = Vendor["vendorName"].ToString();

                    VendorList.Add(new VendorObject {
                        vendorId = vendorIdVar,
                        vendorName = vendorNameVar,
                    });
                }
            }   
        }
 
        return Task.FromResult( VendorList.ToArray());
    }
}