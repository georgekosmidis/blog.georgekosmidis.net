using System.Text.RegularExpressions;

internal record class ItemData
{

    public string? Type { get; init; }

    public string? Url { get; init; }

    public string? Title { get; init; }

    public string? Description { get; init; }

    public string PlainTextDescription
    {
        get
        {
            
            return Regex.Replace(Description ?? throw new ArgumentNullException(nameof(Description)), "<.*?>", String.Empty);
        }
    }


    public DateTime? DatePublished { get; init; }

    public DateTime? DateModified { get; init; }

    public string? Image { get; init; }

    public List<string>? Tags { get; init; }

}
