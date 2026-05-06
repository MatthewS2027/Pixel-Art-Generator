using System;
using System.Threading.Tasks;
using PixelArtGenerator.Infrastructure;
using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        var client = new ApiClient();

        Console.WriteLine("Enter pixel art prompt:");
        //string input = Console.ReadLine();

        Console.WriteLine("Generating...");

        //string result = await client.GenerateImage(input, 128, 128);

        //Console.WriteLine(result);

        // TEMP: just print result first to inspect structure
    }
}