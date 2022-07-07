using System.ComponentModel;
using System.Net.Mail;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace BusBoard
{

    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static string GetPostCode()
        {
            Console.WriteLine("Please enter your postcode: ");
            return Console.ReadLine().Replace(" ","");
        }

        static async Task<Dictionary<string,string>> Post2LatLong(string postCode)
        {
            try
            {
                // Make API call
                HttpResponseMessage response = await client.GetAsync($"http://api.postcodes.io/postcodes/{postCode}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Access to postcode API: " + response.StatusCode.ToString());
                
                // Deserialize JSON
                PostCodeResponse postCodeResponse = JsonConvert.DeserializeObject<PostCodeResponse>(responseBody);
                string lat = postCodeResponse.res["latitude"].ToString();
                string lon = postCodeResponse.res["longitude"].ToString();
                return new Dictionary<string, string>()
                {
                    { "long", lon },
                    { "lat", lat },
                };

            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }

            return null;
        }
        
        static async Task<string> LatLongToAtCode(string lat, string lon)
        {
            try
            {
                // Make API call
                HttpResponseMessage response = await client.GetAsync($"https://transportapi.com/v3/uk/places.json?app_id={Credentials.appId}&app_key={Credentials.appKey}&lat={lat}&lon={lon}&type=bus_stop");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Access to places API: " + response.StatusCode.ToString());
                
                // Deserialize JSON
                PlacesResponse placesResponse = JsonConvert.DeserializeObject<PlacesResponse>(responseBody);
                return placesResponse.member[0]["atcocode"];
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }

            return null;
        }

        static async Task<object> AtcocodeToBuses(string atcocode)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://transportapi.com/v3/uk/bus/stop/{atcocode}/live.json?app_id={Credentials.appId}&app_key={Credentials.appKey}&group=no&limit=5&nextbuses=yes");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine(response.StatusCode);

                BusLiveResponse liveResponse = JsonConvert.DeserializeObject<BusLiveResponse>(responseBody);
                if (!liveResponse.departures.ContainsKey("all"))
                {
                    Console.WriteLine("No departure... :(");
                    return new object();
                }
                var dep = liveResponse.departures["all"];
                foreach (var bus in dep)
                {
                    string time = bus.bestDepEst;
                    DateTime.TryParse(time, out DateTime bestDepEst);
                    Console.WriteLine(bestDepEst.TimeOfDay);
                }

                return new object();
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }

            return new object();
        }
        
        static async Task Main()
        {
            string postCode =  GetPostCode();
            var dic = await Post2LatLong(postCode);
            Console.WriteLine(dic["long"]);
            var atcocode = await LatLongToAtCode(dic["lat"], dic["long"]);
            var _ = await AtcocodeToBuses(atcocode);
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
