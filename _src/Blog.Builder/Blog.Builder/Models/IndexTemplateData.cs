using Blog.Builder.Exceptions;
using System.Text.RegularExpressions;

namespace Blog.Builder.Models;

public record class IndexTemplateData : ModelBase
{
    public List<ArticleModel> Articles { get; } = new List<ArticleModel>();

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
