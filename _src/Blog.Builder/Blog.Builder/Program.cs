using System.Text;
using Newtonsoft.Json;

//set paths
var ROOT = "..\\..\\..\\..\\..\\..\\raw\\";
var OUTPUT = "..\\..\\..\\..\\..\\..\\_output\\";

//load templates
var mainTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template.html"));
var articleTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template-article.html"));
var cardArticleTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template-card-article.html"));
var cardImageTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template-card-image.html"));

//prepare output
Directory.Delete(OUTPUT, true);
Directory.CreateDirectory(OUTPUT);
Directory.CreateDirectory(Path.Combine(OUTPUT, "media"));
Directory.CreateDirectory(Path.Combine(OUTPUT, "themes"));
//Helpers.Copy(Path.Combine(ROOT, "themes"), Path.Combine(OUTPUT, "themes"));
Helpers.Copy(Path.Combine(ROOT, "justcopyme"), OUTPUT);

var pages = new List<ItemData>();

//data for index
//----------------------------------------------------------------------------------
var indexData = new ItemData
{
    DatePublished = DateTime.Now,
    DateModified = DateTime.Now,
    Description = "Microsoft MVP | Cloud Solutions Architect | .NET Software Engineer | Organizer of Munich .NET Meetup | Speaker",
    Tags = new List<string> { "C#", "CSharp", "dotnet", "ML.NET", "Q#", "Microsoft MVP" },
    Type = "website",
    Title = "George Kosmidis",
    RelativeUrl = "/",
    RelativeImageUrl = "/media/me.jpg"
};
pages.Add(indexData);


//standalones
//----------------------------------------------------------------------------------
var standalones = Directory.GetFiles(Path.Combine(ROOT, "standalones"), "*.html").ToList();
foreach (var standalonePath in standalones)
{
    var standaloneBody = File.ReadAllText(standalonePath);

    var standaloneJsonRaw = File.ReadAllText(Path.Combine(ROOT, "standalones", Path.GetFileNameWithoutExtension(standalonePath) + ".json"));
    var standaloneJsonTyped = JsonConvert.DeserializeObject<ItemData>(standaloneJsonRaw) ?? throw new NullReferenceException("Can't find JSON for " + standalonePath);
    pages.Add(standaloneJsonTyped);

    var standalonePageHtml = Helpers.BuildHtml(mainTemplate, standaloneBody, string.Empty, standaloneJsonTyped);
    File.WriteAllText(Path.Combine(OUTPUT, Path.GetFileName(standalonePath)), standalonePageHtml);
}

//todo: add images and possibly other cards

//index & article
//----------------------------------------------------------------------------------
var articleDirectories = Directory.GetDirectories(Path.Combine(ROOT, "posts")).ToList().OrderByDescending(x => x);
var bodyForIndexPage = new StringBuilder();
var articleNumberForPagination = 0;
var pageNumber = 0;

foreach (var directory in articleDirectories)
{
    articleNumberForPagination++;
    var articleBody = File.ReadAllText(Path.Combine(directory, "content.html"));
    var articleJsonRaw = File.ReadAllText(Path.Combine(directory, "content.json"));
    var articleJsonTyped = JsonConvert.DeserializeObject<ItemData>(articleJsonRaw) ?? throw new NullReferenceException("Card Data: " + directory);
    pages.Add(articleJsonTyped);

    if (Directory.Exists(Path.Combine(directory, "media")))
    {
        //TODO: create a smaller version of the feature image for the index page
        Helpers.Copy(Path.Combine(directory, "media"), Path.Combine(OUTPUT, "media"));
    }

    //article
    //----------------------------------------------------------------------------------
    if (articleJsonTyped.Type == "article")
    {
        bodyForIndexPage.Append(
            Helpers.BuildHtml(cardArticleTemplate,
                                articleJsonTyped.Description ?? throw new NullReferenceException(nameof(articleJsonTyped.Description)),
                                string.Empty,
                                articleJsonTyped)
        );

        var articleBodyFinal = Helpers.BuildHtml(articleTemplate, articleBody, string.Empty, articleJsonTyped);
        var articleFilename = articleJsonTyped.RelativeUrl?.Trim('/').Split('/').Last();
        var articlePage = Helpers.BuildHtml(mainTemplate, articleBodyFinal, string.Empty, articleJsonTyped);
        File.WriteAllText(Path.Combine(OUTPUT, $"{articleFilename}"), articlePage);

    }

    //image
    //----------------------------------------------------------------------------------
    if (articleJsonTyped.Type == "image")
    {
        bodyForIndexPage.Append(
            Helpers.BuildHtml(cardImageTemplate,
                                articleJsonTyped.Description ?? throw new NullReferenceException(nameof(articleJsonTyped.Description)),
                                string.Empty,
                                articleJsonTyped)
        );
    }

    //pagination
    //----------------------------------------------------------------------------------
    if (articleNumberForPagination % 9 == 0 || articleNumberForPagination == articleDirectories.Count())
    {
        var paging = Helpers.BuildPaging(pageNumber, (int)Math.Ceiling(articleDirectories.Count() / 9m));

        //index
        //----------------------------------------------------------------------------------
        var indexPage = Helpers.BuildHtml(mainTemplate, bodyForIndexPage.ToString(), paging, indexData);
        var indexFileName = "index.html";
        if (pageNumber > 0)
        {
            indexFileName = $"index-page-{pageNumber + 1}.html";
        }
        File.WriteAllText(Path.Combine(OUTPUT, indexFileName), indexPage);

        bodyForIndexPage.Clear();
        pageNumber++;

    }
}


//google sitemap
//----------------------------------------------------------------------------------
var sitemap = Helpers.BuildSiteMapXML(pages);
File.WriteAllText(Path.Combine(OUTPUT, "sitemap.xml"), sitemap);