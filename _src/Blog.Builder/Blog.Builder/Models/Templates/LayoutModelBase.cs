using Blog.Builder.Exceptions;
using System.Text.RegularExpressions;

namespace Blog.Builder.Models.Templates;

/// <summary>
/// Used for the main template, the layout (template-layout.cshtml)
/// </summary>
public record class LayoutModelBase : ModelBase
{
    public Paging Paging { get; set; } = new Paging();

    public string RelativeUrl { get; set; } = default!;

    public string Title { get; set; } = default!;

    public List<string> Tags { get; set; } = new List<string>();

    public List<string> Section { get; set; } = new List<string>();

    public List<string> ExtraHeaders { get; } = new List<string>();

    public string Description { get; set; } = default!;

    public DateTime DatePublished { get; set; } = default!;

    public DateTime DateModified { get; set; } = default!;

    public DateTime DateExpires { get; } = default!;

    public string Name { get; set; } = default!;

    public string? RelativeImageUrl { get; set; }

    public string Body { get; set; } = default!;

    public string PlainTextDescription
    {
        get
        {
            var result = Regex.Replace(Description, "<.*?>", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            return result.Trim();
        }
    }


    public new void Validate()
    {
        base.Validate();

        ExceptionHelpers.ThrowIfNull(DateModified);
        ExceptionHelpers.ThrowIfNull(DatePublished);
        ExceptionHelpers.ThrowIfNull(DateExpires);
        ExceptionHelpers.ThrowIfNullOrEmpty(Tags);
        ExceptionHelpers.ThrowIfNull(Section);
        ExceptionHelpers.ThrowIfNull(ExtraHeaders);

        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Description);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(PlainTextDescription);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Title);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(RelativeUrl);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Body);

    }

}
