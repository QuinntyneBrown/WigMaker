using WigMaker.Models;

namespace WigMaker.Services;

public interface IWigMakerSearchService
{
    Task<IReadOnlyList<WigMakerListing>> SearchAsync(string city, CancellationToken ct = default);
}
