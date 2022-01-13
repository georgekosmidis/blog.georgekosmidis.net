using Blog.Builder.Exceptions;
using System.Text.RegularExpressions;

namespace Blog.Builder.Models;

public record class MainTemplateData
{

    public PageTypes Type { get; set; } = default!;

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

    private static Guid nonce = Guid.NewGuid();
    public Guid Nonce
    {
        get
        {
            return nonce;
        }
    }

    public void Validate()
    {
        ExceptionHelpers.ThrowIfNull(this.Type);
        ExceptionHelpers.ThrowIfNull(this.DateModified);
        ExceptionHelpers.ThrowIfNull(this.DatePublished);
        ExceptionHelpers.ThrowIfNull(this.DateExpires);
        ExceptionHelpers.ThrowIfNullOrEmpty(this.Tags);
        ExceptionHelpers.ThrowIfNull(this.Section);
        ExceptionHelpers.ThrowIfNull(this.ExtraHeaders);

        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.Description);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.PlainTextDescription);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.Title);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.RelativeUrl);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.Body);

        if (this.Type == PageTypes.Unknown)
        {
            throw new Exception($"{nameof(this.Type)} cannot be {nameof(PageTypes.Unknown)}.");
        }
    }

}
