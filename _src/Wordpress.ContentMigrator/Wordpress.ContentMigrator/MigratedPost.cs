internal record class MigratedPost
{

    public MigratedPost(string url, string title, string description, DateTime datePublished, DateTime dateModified, string? image, List<string> tags)
    {
        Url = url;
        Title = title;
        Description = description;
        DatePublished = datePublished;
        DateModified = dateModified;
        Image = image;
        Tags = tags;
    }

    public string Type { get; init; } = "article";

    public string Url { get; init; }

    public string Title { get; init; }

    public string Description { get; init; }

    public DateTime DatePublished { get; init; }

    public DateTime DateModified { get; init; }

    public string? Image { get; init; }

    public List<string> Tags { get; init; }

}
