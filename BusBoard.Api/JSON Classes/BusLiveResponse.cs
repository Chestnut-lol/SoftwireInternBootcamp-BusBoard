namespace BusBoard.JSON_Classes;

public class BusLiveResponse
{
    public string atcocode { get; set; }
    public string smscode { get; set; }
    public string requestTime { get; set; }
    public string name { get; set; }
    public string bearing { get; set; }
    public string indicator { get; set; }
    public string locality { get; set; }
    public object location { get; set; }
    public Dictionary<string, List<Bus>> departures { get; set; }
}