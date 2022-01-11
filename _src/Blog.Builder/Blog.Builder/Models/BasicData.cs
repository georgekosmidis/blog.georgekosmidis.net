using System.Text.RegularExpressions;

namespace Blog.Builder.Models;

public record class BasicData
{

    public PageTypes Type { get; set; } = PageTypes.Unknown;

    public string? RelativeUrl { get; set; }

    public string? Title { get; set; }

    public List<string>? Tags { get; set; }

    public List<string>? Section { get; set; }

    public List<string> ExtraHeaders { get; } = new List<string>();

    public string? Description { get; set; }

    public DateTime? DatePublished { get; set; }

    public DateTime? DateModified { get; set; }

    public DateTime DateExpires { get; } = DateTime.MaxValue;

    public string? Name { get; set; }

    public string? RelativeImageUrl { get; set; }

    private static Guid nonce = Guid.NewGuid();
    public Guid Nonce
    {
        get
        {
            return nonce;
        }
    }

    public string? Body { get; set; }

}
