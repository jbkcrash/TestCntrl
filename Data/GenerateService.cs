namespace TestCntrl.Data;

// JSON
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public class GenerateService
{

    // Permutate and Combination
    // TODO move to its own file and class when ready
    /* public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> source, TSource item)
    {
        if (source == null)
            throw new ArgumentNullException("source");

        yield return item;

        foreach (var element in source)
            yield return element;
    }

    public static IEnumerable<IEnumerable<TSource>> Permutate<TSource>(this IEnumerable<TSource> source)
    {
        if (source == null)
            throw new ArgumentNullException("source");

        var list = source.ToList();

        if (list.Count > 1)
            return from s in list
                    from p in Permutate(list.Take(list.IndexOf(s)).Concat(list.Skip(list.IndexOf(s) + 1)))
                    select p.Prepend(s);

        return new[] { list };
    }

    public static IEnumerable<IEnumerable<TSource>> Combinate<TSource>(this IEnumerable<TSource> source, int k)
    {
        if (source == null)
            throw new ArgumentNullException("source");

        var list = source.ToList();
        if (k > list.Count)
            throw new ArgumentOutOfRangeException("k");

        if (k == 0)
            yield return Enumerable.Empty<TSource>();

        foreach (var l in list)
            foreach (var c in Combinate(list.Skip(list.Count - k - 2), k - 1))
                yield return c.Prepend(l);
    } */


    private const string ScenarioPath = "scenarios.json";
    private const string VendorsPath = "vendors.json";
    private const string VendorFEPath = "vendorfes.json";

    private static JsonArray vendorfeArray = new JsonArray(); // Our resultant array of vendor to positions.

    public Task<VendorFEObject[]> GetTests(String strScenario)
    {
        //Read in our Vendor FE file
        string jsonDocVendorFEs = File.ReadAllText(VendorFEPath);

        var listOfPositions = new List<List<string>>();

        //Start with Scenarios, for each scenario(strScenario) we must do one run of tests.
        //string jsonDocScenario = File.ReadAllText(ScenarioPath);

        //string jsonDocVendors = File.ReadAllText(VendorsPath);
        //JsonNode VendorsDoc = JsonNode.Parse(jsonDocVendors);
        //JsonArray VendorsArray = VendorsDoc["vendors"].AsArray();
        
        //Create our array of positions to calculate
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
                        //Console.WriteLine($"Targets={vendorObject["vendorId"].ToString()}");
                        //TODO generate list(s) and list of lists and permutate
                    }
                    listOfPositions.Add(vendorsList);
                } //Done processing the list(s)

                var permuter2 = new ListOfListsPermuter<string>(listOfPositions);
                foreach (IEnumerable<string> item in permuter2) {
                    Console.WriteLine("{ \"" + string.Join("\", \"", item) + "\" }");
                }
            }
        }
        //JsonArray VendorFEsArray = VendorFEsDoc["vendorfes"].AsArray();

        List<VendorFEObject> VendorFEList = new List<VendorFEObject>();
        
        /* //Dictionary work        
        private Dictionary<string,List<VendorFEObject>> vendorFesDict = new Dictionary<string,List<VendorFEObject>>();

        //Add data to dictionary
        Var ospFes = new List<VendorFEObject>() {
            new VendorFEObject(){
                vendorId = "osp_vendor1",
                note = "note here"
            }
        }

        vendorFesDict["osp"] = ospFes;
        //Dictonary */

        // var listOfLists = new List<List<string>>()
        //     {
            
        //         { new List<string>() { "osp_vendor1", "osp_vendor2", "osp_vendor3" } }, //OSP
        //         { new List<string>() { "bcf_vendor1", "bcf_vendor2" } }, //iBCF
        //         { new List<string>() { "ecrf_vendor1", "ecrf_vendor2", "ecrf_vendor3" } }, //ECRF
        //         { new List<string>() { "esrp_vendor1", "esrp_vendor2" } }, //ESRP
        //         { new List<string>() { "bcf_vendor1", "bcf_vendor2" } }, //eBCF
        //         { new List<string>() { "che_vendor1" } }//CHe
        //     };

        // var permuter = new ListOfListsPermuter<string>(listOfLists);
        // foreach (IEnumerable<string> item in permuter)
        //     {
        //         Console.WriteLine("{ \"" + string.Join("\", \"", item) + "\" }");
        //     }
        /* if (jsonDocScenario is not null) {
            JsonNode ScenariosDoc = JsonNode.Parse(jsonDocScenario);

            if( ScenariosDoc is not null) {
                
                JsonArray ScenarioArray = ScenariosDoc["scenarios"].AsArray();

                foreach (JsonNode Scenario in ScenarioArray) {
                    //We now need to build test cases for this scenario.
                    String scenarioId = Scenario["id"].ToString();

                    String strPosition = ""; // String to store the calculated vendor.
                    String strVendor = "";
                    
                    //Parse the document intended for this scenario
                    string jsonDocPositions = File.ReadAllText(Scenario["bench"].ToString());
                    JsonNode BenchDoc = JsonNode.Parse(jsonDocPositions);
                    JsonArray BenchArray = BenchDoc["positions"].AsArray();

                    bool bFirst = true;

                    foreach (JsonNode Position in BenchArray) { // Each position will be in here once.
                        foreach (JsonNode VendorFE in VendorFEsArray) {
                            //We have our first VendorFE, if its positionId matches, then process it for next position.
                            if(bFirst && VendorFE["positionId"].ToString() == Position["positionId"].ToString()) {
                                strVendor = VendorFE["vendorId"].ToString(); // Set the starting point.
                                bFirst = false;
                            } else { //Find
                                //Need to iterate through every other iteration of next positions

                            }



                            VendorFEList.Add(new VendorFEObject {
                                positionId = strPosition,
                                vendorId = strVendor
                            });
                        }



                        
                    }
                    //Find 
                   

                }
                
            }
        } */
        return Task.FromResult( VendorFEList.ToArray());
    }
}