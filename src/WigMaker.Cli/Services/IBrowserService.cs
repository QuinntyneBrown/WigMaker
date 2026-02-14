using Microsoft.Playwright;

namespace WigMaker.Cli.Services;

public interface IBrowserService : IAsyncDisposable
{
    Task InitializeAsync();
    Task<IPage> NewPageAsync();
}
