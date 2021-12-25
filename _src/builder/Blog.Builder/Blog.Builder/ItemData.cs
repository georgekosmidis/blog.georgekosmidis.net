internal record class ItemData
{
    //public ItemData() { }

    //public ItemData(string type, string url, string slang, string title, string description, DateTime datePublished, DateTime dateModified, string? image, List<string> tags)
    //{
    //    Type = type;
    //    Url = url;
    //    Slang = slang;
    //    Title = title;
    //    Description = description;  
    //    DatePublished = datePublished;  
    //    DateModified = dateModified;
    //    Image = image;
    //    Tags = tags;
    //}


    public string Type { get; init; } = "website";

    public string Url { get; init; } = "https://blog.georgekosmidis.net";

    public string Slang { get; init; } = "george-kosmidis";

    public string Title { get; init; } = "George Kosmidis";

    public string Description { get; init; } = "Microsoft MVP | Cloud Solutions Architect | .NET Software Engineer | Organizer of Munich .NET Meetup | Speaker";

    public DateTime DatePublished { get; init; } = default;

    public DateTime DateModified { get; init; } = default;

    public string? Image { get; init; } 

    public List<string> Tags { get; init; } = new List<string>{ "C#", "CSharp", "Microsoft", "dotnet" };

}
