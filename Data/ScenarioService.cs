namespace TestCntrl.Data;

// JSON
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public class ScenarioService
{
    private const string Path = "scenarios.json";
    private static JsonArray scenarioArray = new JsonArray();

    public Task<ScenarioObject[]> GetScenarios()
    {
        string jsonDoc = File.ReadAllText(Path);
        List<ScenarioObject> ScenarioList = new List<ScenarioObject>();

        if (jsonDoc is not null) {
            //TODO fix null warning...

            //TODO Wrap in Try/Catch?
            JsonNode scenarioDoc = JsonNode.Parse(jsonDoc); 

            // Get a JSON array from a JsonNode.
            if( scenarioDoc is not null) {
                scenarioArray = scenarioDoc["scenarios"].AsArray();


                foreach (JsonNode scenario in scenarioArray) {
                    //Console.WriteLine($"Object={scenario.ToJsonString()}");
                    string idVar = scenario["id"].ToString();
                    string ospVar = scenario["osp"].ToString();
                    string benchVar = scenario["bench"].ToString();
                    string descriptionVar = scenario["description"].ToString();
                    string exercisedVar = scenario["exercised"].ToString();

                    ScenarioList.Add(new ScenarioObject {
                        id = idVar,
                        osp = ospVar,
                        bench = benchVar,
                        description = descriptionVar,
                        exercised = exercisedVar
                    });
                }
               
            }   
        }
 
        return Task.FromResult( ScenarioList.ToArray());
    }




    /* string jsonString =
@"{
  ""Date"": ""2019-08-01T00:00:00"",
  ""Temperature"": 25,
  ""Summary"": ""Hot"",
  ""DatesAvailable"": [
    ""2019-08-01T00:00:00"",
    ""2019-08-02T00:00:00""
  ],
  ""TemperatureRanges"": {
      ""Cold"": {
          ""High"": 20,
          ""Low"": -10
      },
      ""Hot"": {
          ""High"": 60,
          ""Low"": 20
      }
  }
}
";
    // Create a JsonNode DOM from a JSON string.
    JsonNode forecastNode = JsonNode.Parse(jsonString);

    // Write JSON from a JsonNode
    var options = new JsonSerializerOptions { WriteIndented = true };
    Console.WriteLine(forecastNode.ToJsonString(options));

    //XmlDocument doc = new XmlDocument();
    //doc.PreserveWhitespace = true;
    //try { doc.Load("scenarios.xml"); }
  */   //catch (System.IO.FileNotFoundException)
}