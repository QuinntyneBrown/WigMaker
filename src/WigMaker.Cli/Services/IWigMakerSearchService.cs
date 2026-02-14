using WigMaker.Cli.Models;

namespace WigMaker.Cli.Services;

public interface IWigMakerSearchService
{
    Task<IReadOnlyList<WigMakerListing>> SearchAsync(string city, CancellationToken ct = default);
}
