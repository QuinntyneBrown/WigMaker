namespace WigMaker.Models;

public sealed class WigMakerListing
{
    public required string Name { get; init; }
    public string? Address { get; init; }
    public string? Phone { get; init; }
    public double? Rating { get; init; }
    public int? ReviewCount { get; init; }
    public required string City { get; init; }

    public override string ToString()
    {
        var parts = new List<string> { Name };
        if (Address is not null) parts.Add($"  Address: {Address}");
        if (Phone is not null) parts.Add($"  Phone:   {Phone}");
        if (Rating is not null) parts.Add($"  Rating:  {Rating:F1} ({ReviewCount ?? 0} reviews)");
        parts.Add($"  City:    {City}");
        return string.Join(Environment.NewLine, parts);
    }
}
