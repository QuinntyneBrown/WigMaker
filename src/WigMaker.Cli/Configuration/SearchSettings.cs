namespace WigMaker.Cli.Configuration;

public sealed class SearchSettings
{
    public string[] Cities { get; set; } = ["Toronto", "Mississauga"];
    public int ScrollCount { get; set; } = 5;
    public int ScrollPauseMs { get; set; } = 2000;
    public bool Headless { get; set; } = true;
    public int TimeoutMs { get; set; } = 30000;
}
