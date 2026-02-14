using System.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WigMaker.Cli.Commands;
using WigMaker.Cli.Configuration;
using WigMaker.Cli.Services;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

var searchSettings = new SearchSettings();
configuration.GetSection("Search").Bind(searchSettings);

var services = new ServiceCollection()
    .AddLogging(builder =>
    {
        builder.AddConfiguration(configuration.GetSection("Logging"));
        builder.AddConsole();
    })
    .AddSingleton(searchSettings)
    .AddSingleton<IBrowserService, PlaywrightBrowserService>()
    .AddSingleton<IWigMakerSearchService, WigMakerSearchService>()
    .BuildServiceProvider();

var rootCommand = new RootCommand("WigMaker - Find wig makers in Toronto & Mississauga");
rootCommand.AddCommand(SearchCommand.Create(services));

return await rootCommand.InvokeAsync(args);
