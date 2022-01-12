using System.Text.RegularExpressions;

namespace Blog.Builder.Models;

public record class MainTemplateData : BasicData
{
    public Paging Paging { get; set; } = new Paging();

    public string PlainTextDescription
    {
        get
        {
            var result = Regex.Replace(Description, "<.*?>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            return result.Trim();
        }
    }

    public List<ArticleTemplateData> Articles { get; } = new List<ArticleTemplateData>();

    public new void ValidateMainPage()
    {
        base.Validate();

        ArgumentNullException.ThrowIfNull(this.Paging);
        ArgumentNullException.ThrowIfNull(this.Paging.CurrentPageIndex);
        ArgumentNullException.ThrowIfNull(this.Paging.ArticlesCount);
        ArgumentNullException.ThrowIfNull(this.Paging.ArticlesPerPage);

        ArgumentNullException.ThrowIfNull(this.PlainTextDescription);

        ArgumentNullException.ThrowIfNull(this.Articles);
    }

    public void ValidateIndex()
    {
        this.ValidateMainPage();

        if (this.Articles.Count == 0)
        {
            throw new ArgumentException("No articles found", nameof(this.Articles));
        }
    }

}
