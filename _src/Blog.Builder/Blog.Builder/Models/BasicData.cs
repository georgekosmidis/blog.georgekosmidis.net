using System.Text.RegularExpressions;

namespace Blog.Builder.Models;

public record class BasicData
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
        ArgumentNullException.ThrowIfNull(this.Type);
        ArgumentNullException.ThrowIfNull(this.DateModified);
        ArgumentNullException.ThrowIfNull(this.DatePublished);
        ArgumentNullException.ThrowIfNull(this.DateExpires);
        ArgumentNullException.ThrowIfNull(this.Tags);
        ArgumentNullException.ThrowIfNull(this.Section);
        ArgumentNullException.ThrowIfNull(this.ExtraHeaders);

        if (this.Type == PageTypes.Unknown)
        {
            throw new ArgumentException($"{nameof(this.Type)} cannot be {nameof(PageTypes.Unknown)}!", nameof(this.Type));
        }

        if (!this.Tags.Any())
        {
            throw new ArgumentException($"{nameof(this.Tags)} cannot be empty!", nameof(this.Tags)) ;
        }

        if (string.IsNullOrWhiteSpace(this.Description))
        {
            throw new ArgumentException($"{nameof(this.Description)} cannot be empty!", nameof(this.Description));
        }

        if (string.IsNullOrWhiteSpace(this.Title))
        {
            throw new ArgumentException($"{nameof(this.Title)} cannot be empty!", nameof(this.Title));
        }

        if (string.IsNullOrWhiteSpace(this.RelativeUrl))
        {
            throw new ArgumentException($"{nameof(this.RelativeUrl)} cannot be empty!", nameof(this.RelativeUrl));
        }

        if (string.IsNullOrWhiteSpace(this.Body))
        {
            throw new ArgumentException($"{nameof(this.Body)} cannot be empty!", nameof(this.Body));
        }
    }

}
