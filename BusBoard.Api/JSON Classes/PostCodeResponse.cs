using Newtonsoft.Json;

namespace BusBoard.Api.JSON_Classes;

public class PostCodeResponse
{
    [JsonProperty("result")]
    public Dictionary<string,object> res { get; set; }
}