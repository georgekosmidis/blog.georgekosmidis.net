using Blog.Builder.Exceptions;
using System.Text.RegularExpressions;

namespace Blog.Builder.Models;

public record class IndexTemplateData : MainTemplateData
{
    public Paging Paging { get; set; } = new Paging();


    public List<ArticleTemplateData> Articles { get; } = new List<ArticleTemplateData>();

    public new void Validate()
    {
        base.Validate();

        ExceptionHelpers.ThrowIfNull(this.Paging);
        ExceptionHelpers.ThrowIfNull(this.Paging.CurrentPageIndex);
        ExceptionHelpers.ThrowIfNull(this.Paging.ArticlesCount);
        ExceptionHelpers.ThrowIfNull(this.Paging.ArticlesPerPage);
        ExceptionHelpers.ThrowIfNullOrEmpty(this.Articles);


    }


}
