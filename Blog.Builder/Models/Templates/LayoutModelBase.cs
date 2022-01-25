using Blog.Builder.Exceptions;
using System.Text.RegularExpressions;

namespace Blog.Builder.Models.Templates;

/// <summary>
/// Used for the main template, the layout (template-layout.cshtml).
/// Inherits all members of <see cref="ModelBase"/>.
/// </summary>
public record class LayoutModelBase : ModelBase
{
    /// <summary>
    /// The github repo URL of this article.
    /// </summary>
    public string GithubUrl { get; set; } = default!;


    /// <summary>
    /// The URL of the blog
    /// </summary>
    public string BlogUrl { get; set; } = default!;

    /// <summary>
    /// The HTML for the right column.
    /// Default is empty.
    /// </summary>
    public IEnumerable<string> RightColumnCards { get; set; } = new List<string>();

    /// <summary>
    /// The HTML of the cards to be parsed
    /// </summary>
    public IEnumerable<string> CardsHtml { get; set; } = new List<string>();

    /// <summary>
    /// The paging information for this layout. 
    /// Currently it is used only by index pages.
    /// </summary>
    public Paging Paging { get; set; } = new Paging();

    /// <summary>
    /// The relative URL of the current page.
    /// </summary>
    public string RelativeUrl { get; set; } = default!;

    /// <summary>
    /// The title of the current page.
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    /// The tags of the current page.
    /// </summary>
    public IEnumerable<string> Tags { get; set; } = new List<string>();

    /// <summary>
    /// A text representation for the <see cref="Tags"/> list.
    /// </summary>
    public string TagsText => string.Join(", ", Tags);

    /// <summary>
    /// The section list of the current page.
    /// </summary>
    public IEnumerable<string> Sections { get; set; } = new List<string>();

    /// <summary>
    /// A text representation for the <see cref="Sections"/> list.
    /// </summary>
    public string SectionsText => string.Join(", ", Sections);

    /// <summary>
    /// Any list of extra headers to be included in the current page.
    /// </summary>
    public IEnumerable<string> ExtraHeaders { get; } = new List<string>();

    /// <summary>
    /// A text representation for the <see cref="ExtraHeaders"/> list.
    /// </summary>
    public string ExtraHeadersText => string.Join(", ", ExtraHeaders);

    /// <summary>
    /// An HTML description of the current page.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// The date this page was first published.
    /// </summary>
    public DateTime DatePublished { get; set; } = default!;

    /// <summary>
    /// The date this page was last modified.
    /// </summary>
    public DateTime DateModified { get; set; } = default!;

    /// <summary>
    /// The date the information on this page expires.
    /// Default is <see cref="DateTime.MaxValue"/>.
    /// </summary>
    public DateTime DateExpires { get; } = DateTime.MaxValue;

    /// <summary>
    /// The path to the main image of this page.
    /// </summary>
    public string? RelativeImageUrl { get; set; }

    /// <summary>
    /// The HTML body of this page.
    /// </summary>
    public string Body { get; set; } = default!;

    /// <summary>
    /// A calculated description of this page in plain text.
    /// Uses regular expressions to remove all tags from the <see cref="Description"/>.
    /// </summary>
    public string PlainTextDescription
    {
        get
        {
            var result = Regex.Replace(Description, "<.*?>", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            return result.Trim();
        }
    }

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();

        ExceptionHelpers.ThrowIfNull(DateModified);
        ExceptionHelpers.ThrowIfNull(DatePublished);
        ExceptionHelpers.ThrowIfNull(DateExpires);
        ExceptionHelpers.ThrowIfNullOrEmpty(Tags);
        ExceptionHelpers.ThrowIfNull(Sections);
        ExceptionHelpers.ThrowIfNull(ExtraHeaders);

        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Description);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(PlainTextDescription);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Title);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(RelativeUrl);

    }

}
