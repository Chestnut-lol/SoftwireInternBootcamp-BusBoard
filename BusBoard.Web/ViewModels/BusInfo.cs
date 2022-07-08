namespace BusBoard.Web.ViewModels
{
    public class BusInfo
    {
        public BusInfo(string postCode, Dictionary<string, string> latlong)
        {
            PostCode = postCode;
            LatLong = latlong;
        }
        public string Stop { get; set; } 
        public Dictionary<string, string> LatLong { get; set; }
        public string PostCode { get; set; }

    }
}
