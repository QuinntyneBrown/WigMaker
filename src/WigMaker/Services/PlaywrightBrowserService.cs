using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using WigMaker.Configuration;

namespace WigMaker.Services;

public sealed class PlaywrightBrowserService : IBrowserService
{
    private readonly SearchSettings _settings;
    private readonly ILogger<PlaywrightBrowserService> _logger;
    private IPlaywright? _playwright;
    private IBrowser? _browser;

    public PlaywrightBrowserService(SearchSettings settings, ILogger<PlaywrightBrowserService> logger)
    {
        _settings = settings;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        _logger.LogInformation("Initializing Playwright...");
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = _settings.Headless
        });
        _logger.LogInformation("Browser launched (headless={Headless})", _settings.Headless);
    }

    public async Task<IPage> NewPageAsync()
    {
        if (_browser is null)
            throw new InvalidOperationException("Browser not initialized. Call InitializeAsync first.");

        return await _browser.NewPageAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser is not null)
        {
            await _browser.CloseAsync();
            _browser = null;
        }
        _playwright?.Dispose();
        _playwright = null;
    }
}
