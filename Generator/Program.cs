using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PixelArtGenerator.Core;
using PixelArtGenerator.Infrastructure;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        string apiKey = config["PixelLab:ApiKey"]!;
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("API key is missing from appsettings.json under PixelLab:ApiKey.");
        }

        var client = new ApiClient(apiKey);

        Console.WriteLine("Enter pixel art subject or scene:");
        string? subject = Console.ReadLine();

        Console.WriteLine("Enter a style (or leave blank for default):");
        string? style = Console.ReadLine();

        // Each method fills in one field and returns 'this', allowing for chaining
        var prompt = PromptBuilder.Create()
            .WithSubject(subject ?? string.Empty)
            .WithStyle(string.IsNullOrWhiteSpace(style)
                ? "pixel art, flat colors, clean lines, cartoonish character design"
                : style)
            .WithDetail("monotone color palette, simple shapes, glowing elements")
            .WithMood("ominous, mysterious, eerie")
            .Build();

        Console.WriteLine($"Built prompt: {prompt}");
        Console.WriteLine("Generating...");

        string result = await client.GenerateImage(prompt, 128, 128);

        Console.WriteLine(result);

        Console.WriteLine("Enter output folder path (leave blank for default Output folder):");
        string? outputFolder = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(outputFolder))
        {
            outputFolder = Path.Combine(AppContext.BaseDirectory, "Output");
        }
        else
        {
            outputFolder = outputFolder.Trim();
            if (outputFolder.StartsWith("\"") && outputFolder.EndsWith("\""))
            {
                outputFolder = outputFolder[1..^1];
            }
            outputFolder = Path.GetFullPath(outputFolder);
        }

        string outputName = $"pixel-art-{DateTime.UtcNow:yyyyMMdd_HHmmss}";
        var saveResult = FileStorage.SaveResponse(result, outputFolder, outputName);

        Console.WriteLine($"Saved JSON to: {saveResult.JsonPath}");
        if (saveResult.PngPath is not null)
        {
            Console.WriteLine($"Saved PNG to: {saveResult.PngPath}");
        }
        else
        {
            Console.WriteLine("No PNG image was found in the API response. Raw JSON was saved.");
        }
    }
}

// Current Workflow

// Program.cs creates client ApiClient() object.

// ApiClient() constructor creates an instance of HttpClient class
// --- HttpClient class allows for sending/receiving HTTP requests/responses from a resource identified by URL

/*
Program.cs                          PromptBuilder.cs
──────────────────────────────      ──────────────────────────────
Collect user input
                    ──Create()────► Instantiate blank builder
                    ◄─────────────  Return builder instance
                    
                    ─WithSubject()► Store & sanitize _subject
                    ◄─────────────  return this
                    
                    ─WithStyle()──► Store & sanitize _style
                    ◄─────────────  return this
                    
                    ─WithDetail()─► Store & sanitize _detail
                    ◄─────────────  return this
                    
                    ─WithMood()───► Store & sanitize _mood
                    ◄─────────────  return this
                    
                    ──Build()─────► Filter empty fields
                                    Join with ", "
                    ◄─────────────  Return final prompt string

Use prompt string
(pass to ApiClient)
*/


// ApiClient() method GenerateImage():
// First assigns target URL and Api token
// Next, it creates and serializes the payload that will be used as the request body
// HTTP request is sent: The JSON string is sent as the request body, encoded in UTF8, and labeled as 'application/json'
// HTTP response is received and returned in form of string

// Program.cs stores the resulting JSON in a string named result and then outputs to console