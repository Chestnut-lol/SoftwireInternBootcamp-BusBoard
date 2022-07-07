using System.ComponentModel;
using System.Net.Mail;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace BusBoard
{

    class Program
    {
        static string GetPostCode()
        {
            Console.WriteLine("Please enter your postcode: ");
            return Console.ReadLine().Replace(" ","");
        }

        static async Task Main()
        {
            string postCode =  GetPostCode();
            var dic = await APIHandler.Post2LatLong(postCode);
            Console.WriteLine(dic["long"]);
            var atcocode = await APIHandler.LatLongToAtCode(dic["lat"], dic["long"]);
            var _ = await APIHandler.AtcocodeToBuses(atcocode);
        }
    }

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

    public class PostCodeResponse
    {
        [JsonProperty("result")]
        public Dictionary<string,object> res { get; set; }
    }
    
    public class PlacesResponse
    {
        public List<Dictionary<string, string>> member { get; set; }
    }
    

}
