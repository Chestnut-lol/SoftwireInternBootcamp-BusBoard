using BusBoard.Api.JSON_Classes;
using BusBoard.Web.Models;
using Microsoft.VisualBasic.CompilerServices;

namespace BusBoard.Web.ViewModels
{
    public class BusInfo
    {
        public BusInfo(string postcode, List<Stop> stops)
        {
            Postcode = postcode;
            Stops = stops;
        }
        public string Postcode { get; set; }
        public List<Stop> Stops { get; set; }


    }
}
