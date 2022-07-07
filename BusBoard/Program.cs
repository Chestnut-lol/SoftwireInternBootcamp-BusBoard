using System.ComponentModel;
using System.Net.Mail;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace BusBoard
{
    class Interface
    {
        public static string EnterAtcocode()
        {
            Console.WriteLine("Please enter atcocode: ");
            return CheckAtcocode(Console.ReadLine());
        }

        private static string CheckAtcocode(string atcocode)
        {
            return atcocode;
        }
    }
    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            string atcocode = Interface.EnterAtcocode();
            int n = 5;
            GetNextNBuses(atcocode,5);

        }
        
        static async void GetNextNBuses(string atcocode, int n)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try	
            {
                HttpResponseMessage response = await client.GetAsync($"https://transportapi.com/v3/uk/bus/stop/{atcocode}/live.json?app_id={Credentials.appId}&app_key={Credentials.appKey}&group=no&limit={n.ToString()}&nextbuses=yes");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine(response.StatusCode);

                BusLiveResponse liveResponse = JsonConvert.DeserializeObject<BusLiveResponse>(responseBody);
                var dep = liveResponse.departures["all"];
                foreach (var bus in dep)
                {
                    string time = bus.bestDepEst;
                    DateTime.TryParse(time, out DateTime bestDepEst);
                    Console.WriteLine(bestDepEst.TimeOfDay);
                }



            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }
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
    


}
