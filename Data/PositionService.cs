namespace TestCntrl.Data;

// JSON
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

public class PositionService
{
    private static JsonArray positionArray = new JsonArray();

    public Task<PositionListObject> GetPositionList(String strNature) {
        
        //TODO de-duplicate getting this for the entire class.
        //Pull in our configuration
        var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

        Console.WriteLine($"strNature={strNature}");

        //This is our return object
        string jsonDoc = File.ReadAllText(config["DataFolder"] + "\\" + strNature);
        PositionListObject positionListObject = new PositionListObject();
        
        if (jsonDoc is not null) {
            //TODO fix null warning...

            //TODO Wrap in Try/Catch?
            JsonNode PositionDoc = JsonNode.Parse(jsonDoc);
            JsonArray PositionArray = PositionDoc["positionList"].AsArray();
            positionListObject.positionList = new List<string>();
             //List<string> positionList = new List<string>;
            foreach(string PositionString in PositionArray) {
                positionListObject.positionList.Add(PositionString);
                Console.WriteLine($"Position={PositionString}");
            }
        }




        return Task.FromResult( positionListObject );
    }

    public Task<PositionObject[]> GetPositions(String strNature)
    {
        //Pull in our configuration
        var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();


        Console.WriteLine($"strNature={strNature}");

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