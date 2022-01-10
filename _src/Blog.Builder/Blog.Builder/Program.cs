using System.Text;
using Geko.HttpClientService;
using Geko.HttpClientService.Extensions;
using Newtonsoft.Json;
using RazorEngine;
using RazorEngine.Templating;
using SixLabors.ImageSharp;

//todo: include meetup and sessionize events
//todo: postprocessing of articles (fix html, add highlightjs markers)
//todo: bigger images on tap, is it possible?

//set paths
var ROOT = "..\\..\\..\\..\\..\\..\\raw\\";
var OUTPUT = "..\\..\\..\\..\\..\\..\\_output\\";

//load templates
var mainTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template.cshtml"));
var articleTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template-article.cshtml"));
var cardArticleTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template-card-article.cshtml"));
var cardImageTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template-card-image.cshtml"));
var cardSearchTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template-card-search.cshtml"));

//prepare output
Directory.Delete(OUTPUT, true);
Directory.CreateDirectory(OUTPUT);
Directory.CreateDirectory(Path.Combine(OUTPUT, "media"));
Helpers.Copy(Path.Combine(ROOT, "justcopyme"), OUTPUT);

var pages = new List<ItemData>();

//Meetup events
var httpClientService = HttpClientServiceFactory.Instance.CreateHttpClientService();
var iCalMeetup = await httpClientService.GetAsync("https://www.meetup.com/munich-dotnet-meetup/events/ical/");
var iCalSessionize = await httpClientService.GetAsync("https://sessionize.com/calendar/3a2660e0e9bd49cf853a35956e110a80");

//data for index
//----------------------------------------------------------------------------------
var indexData = new ItemData
{
    DatePublished = DateTime.Now,
    DateModified = DateTime.Now,
    Description = "Microsoft MVP | Cloud Solutions Architect | .NET Software Engineer | Organizer of Munich .NET Meetup | Speaker",
    Tags = new List<string> { "C#", "CSharp", "dotnet", "ML.NET", "Q#", "Microsoft MVP" },
    Type = "mainpage",
    Title = "George Kosmidis",
    RelativeUrl = "/",
    RelativeImageUrl = "/media/me.jpg"
};
pages.Add(indexData);

var result = Engine.Razor.RunCompile(mainTemplate, "templateKey", typeof(ItemData), indexData);

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
    Helpers.SaveCompressedHtml(Path.Combine(OUTPUT, Path.GetFileName(standalonePath)), standalonePageHtml);
}

//todo: add other cards, like meetup and talks
var bodyForIndexPage = new StringBuilder();

//Search
//----------------------------------------------------------------------------------
bodyForIndexPage.Append(
    Helpers.BuildHtml(cardSearchTemplate,
                        string.Empty,
                        string.Empty,
                        indexData)
);

//index & article
//----------------------------------------------------------------------------------
var articleDirectories = Directory.GetDirectories(Path.Combine(ROOT, "posts")).ToList().OrderByDescending(x => x);
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
        Helpers.Copy(Path.Combine(directory, "media"), Path.Combine(OUTPUT, "media"));
        foreach (var file in Directory.GetFiles(Path.Combine(directory, "media")))
        {
            var ext = Path.GetExtension(file);
            var name = Path.GetFileNameWithoutExtension(file);
            Helpers.ResizeImage(file, Path.Combine(OUTPUT, "media", name + "-small" + ext), new Size(500, 10000));//arbitrary big number
        }
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
        Helpers.SaveCompressedHtml(Path.Combine(OUTPUT, $"{articleFilename}"), articlePage);

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
        Helpers.SaveCompressedHtml(Path.Combine(OUTPUT, indexFileName), indexPage);

        bodyForIndexPage.Clear();
        pageNumber++;

    }
}



//google sitemap
//----------------------------------------------------------------------------------
var sitemap = Helpers.BuildSiteMapXML(pages);
Helpers.SaveCompressedHtml(Path.Combine(OUTPUT, "sitemap.xml"), sitemap);