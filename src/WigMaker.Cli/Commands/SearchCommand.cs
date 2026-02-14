using System.CommandLine;
using Microsoft.Extensions.Logging;
using WigMaker.Cli.Configuration;
using WigMaker.Cli.Services;

namespace WigMaker.Cli.Commands;

public static class SearchCommand
{
    public static Command Create(IServiceProvider services)
    {
        var cityOption = new Option<string?>(
            aliases: ["--city", "-c"],
            description: "City to search in (default: all configured cities)");

        var command = new Command("search", "Search for wig makers on Google Maps")
        {
            cityOption
        };

        command.SetHandler(async (string? city) =>
        {
            var searchService = (IWigMakerSearchService)services.GetService(typeof(IWigMakerSearchService))!;
            var browserService = (IBrowserService)services.GetService(typeof(IBrowserService))!;
            var settings = (SearchSettings)services.GetService(typeof(SearchSettings))!;
            var logger = (ILogger<Program>)services.GetService(typeof(ILogger<Program>))!;

            await browserService.InitializeAsync();

            var cities = city is not null ? [city] : settings.Cities;
            var allListings = new List<Models.WigMakerListing>();

            foreach (var c in cities)
            {
                logger.LogInformation("Searching for wig makers in {City}...", c);
                var listings = await searchService.SearchAsync(c);
                allListings.AddRange(listings);
                logger.LogInformation("Found {Count} listings in {City}", listings.Count, c);
            }

            Console.WriteLine();
            Console.WriteLine($"=== Found {allListings.Count} wig maker(s) total ===");
            Console.WriteLine();

            for (var i = 0; i < allListings.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {allListings[i]}");
                Console.WriteLine();
            }

            await browserService.DisposeAsync();
        }, cityOption);

        return command;
    }
}
