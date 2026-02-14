using Microsoft.Playwright;

namespace WigMaker.Services;

public interface IBrowserService : IAsyncDisposable
{
    Task InitializeAsync();
    Task<IPage> NewPageAsync();
}
