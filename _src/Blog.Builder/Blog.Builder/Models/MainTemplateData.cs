using System.Text.RegularExpressions;

namespace Blog.Builder.Models;

public record class MainTemplateData : BasicData
{
    public Paging Paging { get; set; } = new Paging();

    public string PlainTextDescription
    {
        get
        {
            var result = Regex.Replace(Description ?? throw new ArgumentNullException(nameof(Description)), "<.*?>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            return result.Trim();
        }
    }

    public List<ArticleTemplateData> Articles { get; } = new List<ArticleTemplateData>();
}
