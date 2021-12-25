using System.Text;
using Newtonsoft.Json;

var ROOT = "..\\..\\..\\..\\..\\..\\..\\raw\\";
var OUTPUT = "..\\..\\..\\..\\..\\..\\..\\_output\\";

var indexTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template.html"));
var postTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template-post.html"));
var cardPostTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template-card-post.html"));
var cardImageTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template-card-image.html"));

//prepare output
Directory.Delete(OUTPUT, true);
Directory.CreateDirectory(OUTPUT);
Directory.CreateDirectory(Path.Combine(OUTPUT, "media"));
Directory.CreateDirectory(Path.Combine(OUTPUT, "themes"));
Helpers.Copy(Path.Combine(ROOT, "themes"), Path.Combine(OUTPUT, "themes"));

var pages = new List<ItemData>();

//standalones
var standalones = Directory.GetFiles(Path.Combine(ROOT, "templates", "pages")).ToList();
foreach (var standalonePath in standalones)
{
    var standaloneHtml = File.ReadAllText(standalonePath);
    var html = indexTemplate.Replace("{body}", standaloneHtml);
    File.WriteAllText(Path.Combine(OUTPUT, Path.GetFileName(standalonePath)), html);
    //TODO: Has to be generic with a JSON on disk
    pages.Add(new ItemData
    {
        DatePublished = DateTime.Now,
        DateModified= DateTime.Now, 
        Description = "Is lawyer the most boring job in the world? Read this Privacy Policy to find out.",
        Title = "Privacy Policy",
        Image = String.Empty,
        Slang = "privacy",
        Tags = new List<string> { "privacy"},
        Type = "website",
        Url = "/privacy.html"        
    });
}

//posts
var directories = Directory.GetDirectories(Path.Combine(ROOT, "posts")).ToList().OrderByDescending(x => x);
var body = new StringBuilder();
foreach (var directory in directories)
{
    var post = File.ReadAllText(Path.Combine(directory, "content.html"));
    var rawData = File.ReadAllText(Path.Combine(directory, "content.json"));
    var cardData = JsonConvert.DeserializeObject<ItemData>(rawData) ?? throw new NullReferenceException("Card Data");
    pages.Add(cardData);

    if (cardData.Type == "post")
    {
        body.Append(
            cardPostTemplate.Replace("{post-photo}", cardData.Image)
                            .Replace("{post-title}", cardData.Title)
                            .Replace("{post-description}", cardData.Description)
                            .Replace("{post-date}", cardData.DateModified.ToString())
        );
    }

    List<string> media = new List<string>();
    if (Directory.Exists(Path.Combine(directory, "media")))
    {
        Helpers.Copy(Path.Combine(directory, "media"), Path.Combine(OUTPUT, "media"));
    }
}

var index = indexTemplate.Replace("{body}", body.ToString());
File.WriteAllText(Path.Combine(OUTPUT, "index.html"), index);
pages.Add(new ItemData
{
    Image = "/themes/default/img/me.jpg"
});