using Newtonsoft.Json;

namespace BusBoard;

public class APIHandler
{
    static readonly HttpClient client = new HttpClient();
    
        public static async Task<Dictionary<string,string>> Post2LatLong(string postCode)
        {
            // Make API call
            string uri = $"http://api.postcodes.io/postcodes/{postCode}";
            string responseBody = await MakeApiReq(uri, "Access to postcode API");
           
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
        
        public static async Task<string> LatLongToAtCode(string lat, string lon)
        {
            
            // Make API call
            string uri =
                $"https://transportapi.com/v3/uk/places.json?app_id={Credentials.appId}&app_key={Credentials.appKey}&lat={lat}&lon={lon}&type=bus_stop";
            string responseBody = await MakeApiReq(uri, "Access to places API");
            
            // Deserialize JSON
            PlacesResponse placesResponse = JsonConvert.DeserializeObject<PlacesResponse>(responseBody);
            return placesResponse.member[0]["atcocode"];
            
        }

        private static async Task<string> MakeApiReq(string uri, string successMsg)
        {
            string responseBody = "";
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(successMsg + ": " + response.StatusCode);
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }

            return responseBody;
        }

        
        public static async Task<object> AtcocodeToBuses(string atcocode)
        {
            string uri =
                $"https://transportapi.com/v3/uk/bus/stop/{atcocode}/live.json?app_id={Credentials.appId}&app_key={Credentials.appKey}&group=no&limit=5&nextbuses=yes";
            string responseBody = await MakeApiReq(uri, "Access to buses");
            
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
        
}