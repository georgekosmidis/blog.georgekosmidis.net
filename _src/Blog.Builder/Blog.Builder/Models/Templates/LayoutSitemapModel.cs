namespace Blog.Builder.Models.Templates;

/// <summary>
/// Used for the sitemap.xml (template-sitemap.cshtml)
/// </summary>
public class LayoutSitemapModel
{
    /// <summary>
    /// A list of URLs which the sitemap.xml will include.
    /// </summary>
    public List<Url> Urls { get; set; } = new List<Url>();

    /// <summary>
    /// A helper method that adss a URL to the <see cref="Urls"/> list.
    /// </summary>
    /// <param name="relativeUrl">The relative URL of the page.</param>
    /// <param name="dateModified"> The last modified date.</param>
    public void Add(string relativeUrl, DateTime dateModified)
    {
        Urls.Add(new Url { RelativeUrl = relativeUrl, DateModified = dateModified });
    }
}

/// <summary>
/// An object that holds all urls for the sitemap.xml
/// </summary>
public record class Url
{
    /// <summary>
    /// The relative URL of the page.
    /// </summary>
    public string RelativeUrl { get; set; } = string.Empty;

    /// <summary>
    /// The last modified date.
    /// </summary>
    public DateTime DateModified { get; set; } = DateTime.MaxValue;
}