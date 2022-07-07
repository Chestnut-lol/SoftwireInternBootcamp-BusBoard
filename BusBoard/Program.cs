using System.ComponentModel;
using System.Net.Mail;
using System.Runtime.InteropServices;
using BusBoard.JSON_Classes;
using Newtonsoft.Json;

namespace BusBoard
{

    class Program
    {
        static async Task Main()
        {
            string postCode =  GetPostCode();
            var dic = await APIHandler.Post2LatLong(postCode);
            Console.WriteLine(dic["long"]);
            var atcocode = await APIHandler.LatLongToAtCode(dic["lat"], dic["long"]);
            var dep = await APIHandler.AtcocodeToBusDepart(atcocode);
            PrintDepartures(dep);
            atcocode = await APIHandler.LatLongToAtCode(dic["lat"], dic["long"], 1);
            dep = await APIHandler.AtcocodeToBusDepart(atcocode);
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
                    Console.Write(bus.lineName);
                    Console.WriteLine(bus.direction);
                }
            }
        }
    }
}
