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

        //XmlDocument doc = new XmlDocument();
        //doc.PreserveWhitespace = true;
        //try { doc.Load("scenarios.xml"); }
        //catch (System.IO.FileNotFoundException);



        if (jsonDoc is not null) {
            JsonNode scenarioDoc = JsonNode.Parse(jsonDoc); //TODO fix null warning...

            // Get a JSON array from a JsonNode.
            //JsonNode scenarios = scenarioDoc!["scenarios"]!;
            if( scenarioDoc is not null) {
                scenarioArray = scenarioDoc["scenarios"].AsArray();

               
            }   
        }
      



        //Console.WriteLine($"First={scenarios.ToJsonString()}");
        //Console.WriteLine($"JSON={datesAvailable.ToJsonString()}");

        


        return Task.FromResult(Enumerable.Range(1, 5).Select(index => new ScenarioObject
        {
            id = "sc_1",
            osp = "Wireless/IMS",
            description = "Simple i3 Voice Call by_reference and value",
            exercised = "SIP, HELD, LOST, ADR"
        }).ToArray());
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