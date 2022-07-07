namespace BusBoard
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to BusBoard!");
            Console.WriteLine("My name is Alice.");
        }
    }
}
