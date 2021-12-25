using System.Text;
using Newtonsoft.Json;

//set paths
var ROOT = "..\\..\\..\\..\\..\\..\\raw\\";
var OUTPUT = "..\\..\\..\\..\\..\\..\\_output\\";

//load templates
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
Helpers.Copy(Path.Combine(ROOT, "justcopyme"), OUTPUT);

var pages = new List<ItemData>();

//standalones
//----------------------------------------------------------------------------------
var standalones = Directory.GetFiles(Path.Combine(ROOT, "standalones"), "*.html").ToList();
foreach (var standalonePath in standalones)
{
    var standaloneBody = File.ReadAllText(standalonePath);

    var standaloneJsonRaw = File.ReadAllText(Path.Combine(ROOT, "standalones", Path.GetFileNameWithoutExtension(standalonePath) + ".json"));
    var standaloneJsonTyped = JsonConvert.DeserializeObject<ItemData>(standaloneJsonRaw) ?? throw new NullReferenceException("Can't find JSON for " + standalonePath);
    Helpers.ValidateItemData(standaloneJsonTyped);
    pages.Add(standaloneJsonTyped);

    var standalonePageHtml = Helpers.BuildPage(indexTemplate, standaloneBody, standaloneJsonTyped);
    File.WriteAllText(Path.Combine(OUTPUT, Path.GetFileName(standalonePath)), standalonePageHtml);
}

//index & posts
//----------------------------------------------------------------------------------
var articleDirectories = Directory.GetDirectories(Path.Combine(ROOT, "posts")).ToList().OrderByDescending(x => x);
var bodyForIndexPage = new StringBuilder();
foreach (var directory in articleDirectories)
{
    var articleBody = File.ReadAllText(Path.Combine(directory, "content.html"));
    var articleJsonRaw = File.ReadAllText(Path.Combine(directory, "content.json"));
    var articleJsonTyped = JsonConvert.DeserializeObject<ItemData>(articleJsonRaw) ?? throw new NullReferenceException("Card Data: "+ directory);
    Helpers.ValidateItemData(articleJsonTyped);
    pages.Add(articleJsonTyped);

    if (Directory.Exists(Path.Combine(directory, "media")))
    {
        //TODO: create a smaller version of the feature image for the index page
        Helpers.Copy(Path.Combine(directory, "media"), Path.Combine(OUTPUT, "media"));
    }

    //index
    //----------------------------------------------------------------------------------
    if (articleJsonTyped.Type == "article")
    {
        bodyForIndexPage.Append(
            Helpers.BuildCard(cardPostTemplate, articleJsonTyped)
        );
    }
}

//index
//----------------------------------------------------------------------------------
var indexData = new ItemData
{
    DatePublished = DateTime.Now,
    DateModified = DateTime.Now,
    Description = "Microsoft MVP | Cloud Solutions Architect | .NET Software Engineer | Organizer of Munich .NET Meetup | Speaker",
    Tags = new List<string> { "C#", "CSharp", "dotnet", "ML.NET", "Q#", "Microsoft MVP" },
    Type = "website",
    Title = "George Kosmidis",
    Url = "/",
    Image = "/media/me.jpg"
};
pages.Add(indexData);

var indexPage = Helpers.BuildPage(indexTemplate, bodyForIndexPage.ToString(), indexData);
File.WriteAllText(Path.Combine(OUTPUT, "index.html"), indexPage);

//TODO: use ´pages´ variable to create a sitemap.xml, an rss, and a json rss with the last 5(?)