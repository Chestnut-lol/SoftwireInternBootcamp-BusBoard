using BusBoard.Api.JSON_Classes;

namespace BusBoard.Web.Models;

public class Stop
{
    public string Name { get; set; }
    public string Distance { get; set; }
    public string Atcocode { get; set; }
    
    public List<Bus> BusList { get; set; }
    public Stop(string name, string distance, string atcocode, List<Bus> busList)
    {
        Name = name;
        Distance = distance;
        Atcocode = atcocode;
        BusList = busList;
    }
}