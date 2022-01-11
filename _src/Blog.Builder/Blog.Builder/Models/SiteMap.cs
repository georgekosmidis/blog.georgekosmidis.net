namespace Blog.Builder.Models;

public class SiteMap
{
    public List<Url> Urls { get; set; } = new List<Url>();

    public void Add(string? relaticeUrl, DateTime? dateModified)
    {
        ArgumentNullException.ThrowIfNull(relaticeUrl);
        ArgumentNullException.ThrowIfNull(dateModified);

        Urls.Add(new Url { RelativeUrl = relaticeUrl, DateModified = dateModified.Value });
    }

    public string? RelativeUrl { get; set; }
    public DateTime DateModified { get; set; }
}

public record class Url
{
    public string? RelativeUrl { get; set; }
    public DateTime DateModified { get; set; }
}