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
        string input = Console.ReadLine();

        Console.WriteLine("Generating...");

        string result = await client.GenerateImage(input, 128, 128);

        Console.WriteLine(result);

        // TEMP: just print result first to inspect structure
    }
}

// Current Workflow

// Program.cs creates client ApiClient() object.

// ApiClient() constructor creates an instance of HttpClient class
// --- HttpClient class allows for sending/receiving HTTP requests/responses from a resource identified by URL

// Program.cs takes user input
// Program.cs uses client.GenerateImage method and passes the user input string, and the length and width in pixels

// ApiClient() method GenerateImage():
// First assigns target URL and Api token
// Next, it creates and serializes the payload that will be used as the request body
// HTTP request is sent: The JSON string is sent as the request body, encoded in UTF8, and labeled as 'application/json'
// HTTP response is received and returned in form of string

// Program.cs stores the resulting JSON in a string named result and then outputs to console