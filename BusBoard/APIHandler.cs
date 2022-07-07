namespace BusBoard;

public class APIHandler
{
    static readonly HttpClient client = new HttpClient();

    /*static async Task Main()
    {
        // Call asynchronous network methods in a try/catch block to handle exceptions.
        try	
        {
            HttpResponseMessage response = await client.GetAsync($"https://transportapi.com/v3/uk/bus/stop/0500CCITY436/live.json?app_id={Credentials.appId}&app_key={Credentials.appKey}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);

            Console.WriteLine(responseBody);
        }
        catch(HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");	
            Console.WriteLine("Message :{0} ",e.Message);
        }
    }*/
}