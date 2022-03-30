namespace TestCntrl.Data;

// JSON
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public class VendorFEService
{
    private const string Path = "vendorfes.json";
    private static JsonArray vendorfeArray = new JsonArray();

    public Task<VendorFEObject[]> GetVendorFE()
    {
        string jsonDoc = File.ReadAllText(Path);
        List<VendorFEObject> VendorFEList = new List<VendorFEObject>();

        if (jsonDoc is not null) {
            //TODO fix null warning...

            //TODO Wrap in Try/Catch?
            JsonNode VendorFEDoc = JsonNode.Parse(jsonDoc); 

            // Get a JSON array from a JsonNode.
            if( VendorFEDoc is not null) {
                vendorfeArray = VendorFEDoc["vendorfe"].AsArray();


                foreach (JsonNode VendorFE in vendorfeArray) {
                    //Console.WriteLine($"Object={VendorFE.ToJsonString()}");
                    string positionIdVar = VendorFE["positionId"].ToString();
                    string vendorIdVar = VendorFE["vendorId"].ToString();
                    string noteVar = VendorFE["note"].ToString();


                    VendorFEList.Add(new VendorFEObject {
                        positionId = positionIdVar,
                        vendorId = vendorIdVar,
                        note = noteVar
                    });
                }
            }   
        }
 
        return Task.FromResult( VendorFEList.ToArray());
    }
}