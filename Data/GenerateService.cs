namespace TestCntrl.Data;

// JSON
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public class GenerateService
{
    private const string VendorFEPath = "vendorfes.json";
    private const string ScenarioPath = "scenarios.json";

    private static JsonArray vendorfeArray = new JsonArray(); // Our resultant array of vendor to positions.

    public Task<TestCaseObject[]> GetTests(String strScenario, PositionListObject positionListObject)
    {
        //Pull in our configuration
        var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

        

        //This is our list of lists to permutate
        var listOfPositions = new List<List<string>>();

        //List of tests to return
        List<TestCaseObject> TestCaseObjectList = new List<TestCaseObject>();
        
        //TODO load scenario data and search for the scenario's bench file to load. That is change VendorFEPath...
        string JsonDocScenarios = File.ReadAllText(config["DataFolder"] + "\\" + ScenarioPath);
        JsonNode ScenariosDoc = JsonNode.Parse(JsonDocScenarios);
        JsonArray ScenariosArray = ScenariosDoc["scenarios"].AsArray();
        string strBench = new string("");
        foreach(JsonNode Scenario in ScenariosArray){
            if(Scenario["id"].ToString() == strScenario) {
                strBench = Scenario["bench"].ToString(); //Set our bench file string to our temp variable
                //Console.WriteLine("Scenario Data Object: " + Scenario.ToString());
                break;
            }

        }
        //TODO, load bench "strBench"

        //Read in our Vendor FE file
        string jsonDocVendorFEs = File.ReadAllText(config["DataFolder"] + "\\" + VendorFEPath);

        //TODO this needs base on the strScenario that is passed to this method.
        //The scenario will define which "bench" is in use, hence the positions and then calculate and return.
        //In the razor page we will get an object that contains the structure of the bench and the vendors in those
        //posisions.

        //Create our array of positions to calculate, this is replaced by bench definition for the scenario selected
        
        //TODO flop out this array for this list... positionListObject
        string[] strPositionsArray = positionListObject.positionList.ToArray();
        Console.WriteLine("{ \"" + string.Join("\", \"", strPositionsArray) + "\" }");


        //string[] strPositionsArray = new string[] {
        //    "osp","ibcf","esrp","ecrf","ebcf","che","logger"
        //};

        if( jsonDocVendorFEs is not null) {
        
            JsonNode VendorFEDoc = JsonNode.Parse(jsonDocVendorFEs); //Parse our document
            if( VendorFEDoc is not null ) {
                //Iterate of each position in the bench, extract the arrays and create our list(s).
                JsonObject positionsObject = VendorFEDoc["vendorfes"].AsObject(); //Get all the FEs
                foreach(string strPosition in strPositionsArray){
                    List<string> vendorsList = new List<string>();
                    //Console.WriteLine("{ \"" + strPosition + "\" }");
                    //Console.WriteLine("{ \"" + positionsObject[strPosition].ToString() + "\" }");
                    JsonArray targetPosition = positionsObject[strPosition].AsArray(); //Get the current position as array
                    foreach(JsonNode vendorObject in targetPosition) {
                        vendorsList.Add(vendorObject["vendorId"].ToString()); //Make this a dictionary...
                    }
                    listOfPositions.Add(vendorsList);
                } //Done processing the list(s)

                var permuter2 = new ListOfListsPermuter<string>(listOfPositions);
                foreach (IEnumerable<string> item in permuter2) {
                    TestCaseObject tcObject = new TestCaseObject();
                    int iIndex = 0;
                    //This is a hack, but it works as long as the order of teh elements doesn't change compared to the packed list and the data input
                    //into the permuter
                    foreach(string strPosition in strPositionsArray){
                        switch(strPosition) {
                            case "osp":
                                tcObject.osp = item.ElementAt(iIndex);
                                break;
                            case "lis":
                                tcObject.lis = item.ElementAt(iIndex);
                                break;
                            case "pif":
                                tcObject.pif = item.ElementAt(iIndex);
                                break;
                            case "plis":
                                tcObject.plis = item.ElementAt(iIndex);
                                break;
                            case "adr":
                                tcObject.adr = item.ElementAt(iIndex);
                                break;
                            case "padr":
                                tcObject.padr = item.ElementAt(iIndex);
                                break;
                            case "obcf":
                                tcObject.obcf = item.ElementAt(iIndex);
                                break;
                            case "ibcf":
                                tcObject.ibcf = item.ElementAt(iIndex);
                                break;
                            case "esrp":
                                tcObject.esrp = item.ElementAt(iIndex);
                                break;
                            case "ecrf":
                                tcObject.ecrf = item.ElementAt(iIndex);
                                break;
                            case "policy":
                                tcObject.policy = item.ElementAt(iIndex);
                                break;
                            case "ebcf":
                                tcObject.ebcf = item.ElementAt(iIndex);
                                break;
                            case "che":
                                tcObject.che = item.ElementAt(iIndex);
                                break;
                            case "logger":
                                tcObject.logger = item.ElementAt(iIndex);
                                break;
                            default:
                                break;
                        }
                        iIndex++;
                    }
                    TestCaseObjectList.Add(tcObject);
                    //Console.WriteLine("{ \"" + string.Join("\", \"", item) + "\" }");

                    /* TestCaseObjectList.Add(new TestCaseObject {
                        osp = item.ElementAt(0),
                        ibcf = item.ElementAt(1),
                        esrp = item.ElementAt(2),
                        ecrf = item.ElementAt(3),
                        ebcf = item.ElementAt(4),
                        che = item.ElementAt(5),
                        logger = item.ElementAt(6)
                    }); */

                }
            }
        }
        return Task.FromResult( TestCaseObjectList.ToArray() );
    }
}