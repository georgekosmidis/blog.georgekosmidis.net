namespace Blog.Builder.Models;

public class Sitemap
{
    public List<Url> Urls { get; set; } = new List<Url>();

    public void Add(string relaticeUrl, DateTime dateModified)
    {
        Urls.Add(new Url { RelativeUrl = relaticeUrl, DateModified = dateModified });
    }
}

public record class Url
{
    public string RelativeUrl { get; set; } = string.Empty;
    public DateTime DateModified { get; set; } = DateTime.MaxValue;
}