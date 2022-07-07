using Newtonsoft.Json;

namespace BusBoard.JSON_Classes;

public class Bus
{
    public string mode { get; set; }
    public string line { get; set; }
    public string lineName { get; set; }
    public string direction { get; set; }
    [JsonProperty("operator")]
    public string op { get; set; }
    public DateTime date { get; set; }
    [JsonProperty("expected_departure_date")]
    public string expectedDepDate { get; set; }
    [JsonProperty("aimed_departure_time")]
    public string aimedDepTime { get; set; }
    [JsonProperty("expecteded_departure_time")]
    public string expectedDepTime { get; set; }
    public object aimed { get; set; }
    public object expected { get; set; }
    [JsonProperty("best_departure_estimate")]
    public string bestDepEst { get; set; }
    public object status { get; set; }
    public string source { get; set; }
    public string dir { get; set; }
    [JsonProperty("operator_name")]
    public string opName { get; set; }
    public string id { get; set; }
}