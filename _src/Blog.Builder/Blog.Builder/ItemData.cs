using System.Text.RegularExpressions;

internal record class ItemData
{

    public string? Type { get; init; }

    public string? RelativeUrl { get; init; }

    public string? Title { get; init; }

    public string? Description { get; init; }

    public string PlainTextDescription
    {
        get
        {
            return Regex.Replace(Description ?? throw new ArgumentNullException(nameof(Description)), "<.*?>", String.Empty);
        }
    }


    public string LastModifiedText
    {
        get
        {
            var span = DateTime.Now - (DateModified ?? throw new ArgumentNullException(nameof(DateModified)));

            return SpanCalculation(span);
        }
    }

    public string PublishedText
    {
        get
        {
            var span = DateTime.Now - (DatePublished ?? throw new ArgumentNullException(nameof(DatePublished)));

            return SpanCalculation(span);
        }
    }

    public DateTime? DatePublished { get; init; }

    public DateTime? DateModified { get; init; }

    public string? RelativeImageUrl { get; init; }

    public List<string>? Tags { get; init; }

    public List<string> ExtraHeaders { get; init; } = new List<string>();


    private string SpanCalculation(TimeSpan span)
    {

        if (span.Days > 0)
        {
            return span.Days > 30 ? Math.Round(span.Days / 30d) + $" month{(Math.Round(span.Days / 30d) > 1 ? "s" : "")} ago" : span.Days + $" day{(span.Days > 1 ? "s" : "")} ago";
        }
        if (span.Hours > 0)
        {
            return span.Hours + $" hour{(span.Hours > 1 ? "s" : "")} ago";
        }
        if (span.Minutes > 0)
        {
            return span.Minutes + $" minute{(span.Minutes > 1 ? "s" : "")} ago";
        }
        return " a few seconds ago";
    }
}
