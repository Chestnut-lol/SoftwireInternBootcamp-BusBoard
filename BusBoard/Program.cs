using System.ComponentModel;
using System.Net.Mail;
using System.Runtime.InteropServices;
using BusBoard.Api.JSON_Classes;
using BusBoard.Api;

namespace BusBoard
{

    class Program
    {
        static async Task Main()
        {
            string postCode =  GetPostCode();
            var dic = await APIHandler.Post2LatLong(postCode);
            Console.WriteLine(dic["long"]);
            Dictionary<string,string> stops;
            for (int i = 0; i < 2; i++)
            {
                stops = (await APIHandler.LatLongToStop(dic["lat"], dic["long"], i));
                await StopToPrintBoard(stops);
            }
        }

        private static async Task StopToPrintBoard(Dictionary<string, string> stop)
        {
            var atcocode = stop["atcocode"];
            Console.WriteLine(stop["name"]);
            var dep = await APIHandler.AtcocodeToBusDepart(atcocode);
            PrintDepartures(dep);
        }


        static string GetPostCode()
        {
            Console.WriteLine("Please enter your postcode: ");
            return Console.ReadLine().Replace(" ","");
        }


        private static void PrintDepartures(Dictionary<string, List<Bus>> departures)
        {
            if (!departures.ContainsKey("all"))
            {
                Console.WriteLine("No departure... :(");
            }
            else
            {
                var dep = departures["all"];
                foreach (var bus in dep)
                {
                    //string time = bus.bestDepEst;
                    //DateTime.TryParse(time, out DateTime bestDepEst);
                    Console.Write(bus.bestDepEst+" ");
                    Console.Write(bus.lineName + " ");
                    Console.WriteLine(bus.direction);
                }
            }
        }
    }
}
