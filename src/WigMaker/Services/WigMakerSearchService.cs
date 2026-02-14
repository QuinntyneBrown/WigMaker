using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using WigMaker.Configuration;
using WigMaker.Models;

namespace WigMaker.Services;

public sealed partial class WigMakerSearchService : IWigMakerSearchService
{
    private readonly IBrowserService _browserService;
    private readonly SearchSettings _settings;
    private readonly ILogger<WigMakerSearchService> _logger;

    public WigMakerSearchService(
        IBrowserService browserService,
        SearchSettings settings,
        ILogger<WigMakerSearchService> logger)
    {
        _browserService = browserService;
        _settings = settings;
        _logger = logger;
    }

    public async Task<IReadOnlyList<WigMakerListing>> SearchAsync(string city, CancellationToken ct = default)
    {
        var page = await _browserService.NewPageAsync();
        page.SetDefaultTimeout(_settings.TimeoutMs);

        var query = Uri.EscapeDataString($"wig makers in {city}");
        var url = $"https://www.google.com/maps/search/{query}";

        _logger.LogInformation("Navigating to {Url}", url);
        await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });

        // Handle consent dialog if present
        await DismissConsentAsync(page);

        // Wait for the results feed
        var feed = await page.WaitForSelectorAsync("div[role='feed']", new PageWaitForSelectorOptions
        {
            Timeout = _settings.TimeoutMs
        });

        if (feed is null)
        {
            _logger.LogWarning("No results feed found for {City}", city);
            return [];
        }

        // Scroll to load more results
        await ScrollFeedAsync(page, feed);

        // Collect all listing links
        var cards = await feed.QuerySelectorAllAsync("a.hfpxzc");
        _logger.LogInformation("Found {Count} listing cards in {City}", cards.Count, city);

        var listings = new List<WigMakerListing>();

        foreach (var card in cards)
        {
            if (ct.IsCancellationRequested) break;

            try
            {
                var listing = await ExtractListingAsync(page, card, city);
                if (listing is not null)
                {
                    listings.Add(listing);
                    _logger.LogDebug("Extracted: {Name}", listing.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to extract a listing card in {City}", city);
            }
        }

        await page.CloseAsync();
        return listings;
    }

    private async Task DismissConsentAsync(IPage page)
    {
        try
        {
            var acceptButton = await page.QuerySelectorAsync("button:has-text('Accept all')");
            if (acceptButton is not null)
            {
                await acceptButton.ClickAsync();
                _logger.LogInformation("Dismissed consent dialog");
                await page.WaitForTimeoutAsync(1000);
            }
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "No consent dialog to dismiss");
        }
    }

    private async Task ScrollFeedAsync(IPage page, IElementHandle feed)
    {
        for (var i = 0; i < _settings.ScrollCount; i++)
        {
            _logger.LogDebug("Scrolling feed ({Iteration}/{Total})", i + 1, _settings.ScrollCount);
            await feed.EvaluateAsync("el => el.scrollTo(0, el.scrollHeight)");
            await page.WaitForTimeoutAsync(_settings.ScrollPauseMs);

            // Check for end-of-list marker
            var endMarker = await feed.QuerySelectorAsync("span.HlvSq");
            if (endMarker is not null)
            {
                _logger.LogInformation("Reached end of results after {Scrolls} scrolls", i + 1);
                break;
            }
        }
    }

    private async Task<WigMakerListing?> ExtractListingAsync(IPage page, IElementHandle card, string city)
    {
        await card.ClickAsync();
        await page.WaitForTimeoutAsync(1500);

        var name = await page.EvalOnSelectorAsync<string?>("h1.DUwDvf", "el => el?.textContent?.trim()");
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var address = await SafeGetAriaTextAsync(page, "Address");
        var phone = await SafeGetAriaTextAsync(page, "Phone");
        var (rating, reviewCount) = await ExtractRatingAsync(page);

        return new WigMakerListing
        {
            Name = name,
            Address = address,
            Phone = phone,
            Rating = rating,
            ReviewCount = reviewCount,
            City = city
        };
    }

    private static async Task<string?> SafeGetAriaTextAsync(IPage page, string labelPrefix)
    {
        try
        {
            var el = await page.QuerySelectorAsync($"*[aria-label^='{labelPrefix}:']");
            if (el is null) return null;
            var ariaLabel = await el.GetAttributeAsync("aria-label");
            // aria-label is like "Address: 123 Main St" — strip the prefix
            return ariaLabel?.Split(':', 2).ElementAtOrDefault(1)?.Trim();
        }
        catch
        {
            return null;
        }
    }

    private async Task<(double? rating, int? reviewCount)> ExtractRatingAsync(IPage page)
    {
        try
        {
            var ratingEl = await page.QuerySelectorAsync("div.F7nice span[aria-hidden='true']");
            var ratingText = ratingEl is not null
                ? await ratingEl.InnerTextAsync()
                : null;

            var reviewEl = await page.QuerySelectorAsync("div.F7nice span[aria-label*='reviews']");
            var reviewLabel = reviewEl is not null
                ? await reviewEl.GetAttributeAsync("aria-label")
                : null;

            double? rating = double.TryParse(ratingText, out var r) ? r : null;
            int? reviewCount = null;

            if (reviewLabel is not null)
            {
                var match = ReviewCountRegex().Match(reviewLabel);
                if (match.Success && int.TryParse(match.Groups[1].Value.Replace(",", ""), out var rc))
                    reviewCount = rc;
            }

            return (rating, reviewCount);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Could not extract rating");
            return (null, null);
        }
    }

    [GeneratedRegex(@"([\d,]+)\s+reviews?")]
    private static partial Regex ReviewCountRegex();
}
