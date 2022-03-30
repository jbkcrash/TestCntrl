namespace TestCntrl.Data;

// JSON
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public class GenerateService
{
    private const string ScenarioPath = "scenarios.json";
    private const string VendorsPath = "vendors.json";
    private const string VendorFEsPath = "vendorfes.json";

    private static JsonArray vendorfeArray = new JsonArray(); // Our resultant array of vendor to positions.

    public Task<VendorFEObject[]> GetTests(String strScenario)
    {

        //Start with Scenarios, for each scenario(strScenario) we must do one run of tests.


        string jsonDocScenario = File.ReadAllText(ScenarioPath);

        string jsonDocVendors = File.ReadAllText(VendorsPath);
        JsonNode VendorsDoc = JsonNode.Parse(jsonDocVendors);
        JsonArray VendorsArray = VendorsDoc["vendors"].AsArray();

        string jsonDocVendorFEs = File.ReadAllText(VendorFEsPath);
        JsonNode VendorFEsDoc = JsonNode.Parse(jsonDocVendorFEs);
        JsonArray VendorFEsArray = VendorFEsDoc["vendorfe"].AsArray();

        List<VendorFEObject> VendorFEList = new List<VendorFEObject>();

        if (jsonDocScenario is not null) {
            JsonNode ScenariosDoc = JsonNode.Parse(jsonDocScenario);

            if( ScenariosDoc is not null) {
                
                JsonArray ScenarioArray = ScenariosDoc["scenarios"].AsArray();

                foreach (JsonNode Scenario in ScenarioArray) {
                    //We now need to build test cases for this scenario.
                    String scenarioId = Scenario["id"].ToString();

                    //Parse the document intended for this scenario
                    string jsonDocPositions = File.ReadAllText(Scenario["bench"].ToString());
                    JsonNode BenchDoc = JsonNode.Parse(jsonDocPositions);
                    JsonArray BenchArray = BenchDoc["positions"].AsArray();

                    foreach (JsonNode Position in BenchArray) {
                        // Find the 
                        VendorFEList.Add(new VendorFEObject {
                            positionId = Position["positionId"].ToString(),
                            vendorId = ""
                        });
                    }
                    //Find 
                   

                }
                
            }
        }
        return Task.FromResult( VendorFEList.ToArray());
    }
}