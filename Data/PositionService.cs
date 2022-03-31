namespace TestCntrl.Data;

// JSON
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public class PositionService
{
    private static JsonArray positionArray = new JsonArray();

    public Task<PositionObject[]> GetPositions(String strNature)
    {
        //Pull in our configuration
        var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

        string jsonDoc = File.ReadAllText(config["DataFolder"] + "\\" + strNature);
        List<PositionObject> PositionList = new List<PositionObject>();

        if (jsonDoc is not null) {
            //TODO fix null warning...

            //TODO Wrap in Try/Catch?
            JsonNode PositionDoc = JsonNode.Parse(jsonDoc); 

            // Get a JSON array from a JsonNode.
            if( PositionDoc is not null) {
                positionArray = PositionDoc["positions"].AsArray();


                foreach (JsonNode Position in positionArray) {
                    //Console.WriteLine($"Object={Position.ToJsonString()}");
                    string positionIdVar = Position["positionId"].ToString();
                    string xBenchPosVar = Position["xBenchPos"].ToString();
                    string yBenchPosVar = Position["yBenchPos"].ToString();
                    string northObjectsVar = Position["northObjects"].ToString();
                    string southObjectsVar = Position["southObjects"].ToString();
                    string westObjectsVar = Position["westObjects"].ToString();
                    string eastObjectsVar = Position["eastObjects"].ToString();

                    PositionList.Add(new PositionObject {
                        positionId = positionIdVar,
                        xBenchPos = xBenchPosVar,
                        yBenchPos = yBenchPosVar,
                        northObjects = northObjectsVar,
                        southObjects = southObjectsVar,
                        westObjects = westObjectsVar,
                        eastObjects = eastObjectsVar
                    });
                }
            }   
        }
 
        return Task.FromResult( PositionList.ToArray());
    }
}