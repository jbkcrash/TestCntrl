namespace TestCntrl.Data;

// JSON
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public class GenerateService
{
    private const string VendorFEPath = "vendorfes.json";

    private static JsonArray vendorfeArray = new JsonArray(); // Our resultant array of vendor to positions.

    public Task<TestCaseObject[]> GetTests(String strScenario)
    {
        //Read in our Vendor FE file
        string jsonDocVendorFEs = File.ReadAllText(VendorFEPath);

        //This is our list of lists to permutate
        var listOfPositions = new List<List<string>>();

        //List of tests to return
        List<TestCaseObject> TestCaseObjectList = new List<TestCaseObject>();
        
        //TODO this needs base on the strScenario that is passed to this method.
        //The scenario will define which "bench" is in use, hence the positions and then calculate and return.
        //In the razor page we will get an object that contains the structure of the bench and the vendors in those
        //posisions.

        //Create our array of positions to calculate, this is replaced by bench definition for the scenario selected
        string[] strPositionsArray = new string[] {
            "osp","ibcf","esrp","ecrf","ebcf","che","logger"
        };

        if( jsonDocVendorFEs is not null) {
        
            JsonNode VendorFEDoc = JsonNode.Parse(jsonDocVendorFEs); //Parse our document
            if( VendorFEDoc is not null ) {
                //Iterate of each position in the bench, extract the arrays and create our list(s).
                JsonObject positionsObject = VendorFEDoc["vendorfes"].AsObject(); //Get all the FEs
                foreach(string strPosition in strPositionsArray){
                    List<string> vendorsList = new List<string>();

                    JsonArray targetPosition = positionsObject[strPosition].AsArray(); //Get the current position as array
                    foreach(JsonNode vendorObject in targetPosition) {
                        vendorsList.Add(vendorObject["vendorId"].ToString());
                    }
                    listOfPositions.Add(vendorsList);
                } //Done processing the list(s)

                var permuter2 = new ListOfListsPermuter<string>(listOfPositions);
                foreach (IEnumerable<string> item in permuter2) {
                    //Console.WriteLine("{ \"" + string.Join("\", \"", item) + "\" }");

                    TestCaseObjectList.Add(new TestCaseObject {
                        osp = item.ElementAt(0),
                        ibcf = item.ElementAt(1),
                        esrp = item.ElementAt(2),
                        ecrf = item.ElementAt(3),
                        ebcf = item.ElementAt(4),
                        che = item.ElementAt(5),
                        logger = item.ElementAt(6)
                    });

                }
            }
        }
        return Task.FromResult( TestCaseObjectList.ToArray() );
    }
}