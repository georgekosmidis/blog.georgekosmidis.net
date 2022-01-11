using System.Text;
using Blog.Builder.Models;
using Geko.HttpClientService;
using Geko.HttpClientService.Extensions;
using Newtonsoft.Json;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using SixLabors.ImageSharp;


var configuration = new TemplateServiceConfiguration();
configuration.EncodedStringFactory = new RawStringFactory();
configuration.AllowMissingPropertiesOnDynamic = true;
var templateService = RazorEngineService.Create(configuration);

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
var articleDirectories = Directory.GetDirectories(Path.Combine(ROOT, "posts")).ToList().OrderByDescending(x => x);

//prepare output
Directory.Delete(OUTPUT, true);
Directory.CreateDirectory(OUTPUT);
Directory.CreateDirectory(Path.Combine(OUTPUT, "media"));
Helpers.Copy(Path.Combine(ROOT, "justcopyme"), OUTPUT);

//Meetup events
var httpClientService = HttpClientServiceFactory.Instance.CreateHttpClientService();
var iCalMeetup = await httpClientService.GetAsync("https://www.meetup.com/munich-dotnet-meetup/events/ical/");
var iCalSessionize = await httpClientService.GetAsync("https://sessionize.com/calendar/3a2660e0e9bd49cf853a35956e110a80");

//Sitemap preparation
var sitemap = new SiteMap();

//string build that will hold first page cards html
var bodyForIndexPage = new StringBuilder();

//data for index
//----------------------------------------------------------------------------------
var indexPageData = new MainTemplateData
{
    DatePublished = DateTime.Now,
    DateModified = DateTime.Now,
    Description = "Microsoft MVP | Cloud Solutions Architect | .NET Software Engineer | Organizer of Munich .NET Meetup | Speaker",
    Tags = new List<string> { "C#", "CSharp", "dotnet", "ML.NET", "Q#", "Microsoft MVP" },
    Type = PageTypes.MainPage,
    Title = "George Kosmidis",
    RelativeUrl = "/",
    RelativeImageUrl = "/media/me.jpg",
    Paging = new Paging
    {
        ArticlesCount = articleDirectories.Count(),
        CurrentPageIndex = 0
    }
};

//standalones
//----------------------------------------------------------------------------------
var standalones = Directory.GetFiles(Path.Combine(ROOT, "standalones"), "*.html").ToList();
foreach (var standalonePath in standalones)
{
    var filename = Path.GetFileName(standalonePath);

    var standaloneJsonRaw = File.ReadAllText(Path.Combine(ROOT, "standalones", Path.GetFileNameWithoutExtension(standalonePath) + ".json"));
    var standaloneJsonData = JsonConvert.DeserializeObject<MainTemplateData>(standaloneJsonRaw) ?? throw new NullReferenceException("Can't find JSON for " + standalonePath);

    standaloneJsonData.Body = File.ReadAllText(standalonePath);

    var standalonePageHtml = templateService.RunCompile(mainTemplate, standalonePath, typeof(MainTemplateData), standaloneJsonData);

    Helpers.SaveCompressedHtml(Path.Combine(OUTPUT, filename), standalonePageHtml);

    sitemap.Add(filename, standaloneJsonData.DateModified);
}

//Search
//----------------------------------------------------------------------------------
bodyForIndexPage.Append(
    templateService.RunCompile(cardSearchTemplate, "search", typeof(MainTemplateData), indexPageData)
);

//index & article
//----------------------------------------------------------------------------------
var articleNumberForPagination = 0;
var pageNumber = 0;

foreach (var directory in articleDirectories)
{
    articleNumberForPagination++;

    var articleDataRaw = File.ReadAllText(Path.Combine(directory, "content.json"));
    var articleData = JsonConvert.DeserializeObject<ArticleTemplateData>(articleDataRaw) ?? throw new NullReferenceException("Card Data: " + directory);
    var articlePageData = JsonConvert.DeserializeObject<MainTemplateData>(articleDataRaw) ?? throw new NullReferenceException("Card Data: " + directory);

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
    if (articleData.Type == PageTypes.Article)
    {
        //sumup all cards for the first page
        bodyForIndexPage.Append(
            templateService.RunCompile(cardArticleTemplate, "card:" + directory, typeof(ArticleTemplateData), articleData)
        );

        //for each article page
        articleData.Body = File.ReadAllText(Path.Combine(directory, "content.html"));
        articlePageData.Body = templateService.RunCompile(articleTemplate, "body:" + directory, typeof(ArticleTemplateData), articleData);
        var articlePageHtml = templateService.RunCompile(mainTemplate, "page:" + directory, typeof(MainTemplateData), articlePageData);

        var articleFilename = articleData.RelativeUrl?.Trim('/').Split('/').Last();
        Helpers.SaveCompressedHtml(Path.Combine(OUTPUT, $"{articleFilename}"), articlePageHtml);

        sitemap.Add(articleFilename, articleData.DateModified);
    }


    //image
    //----------------------------------------------------------------------------------
    if (articleData.Type == PageTypes.Image)
    {
        bodyForIndexPage.Append(
            templateService.RunCompile(cardImageTemplate, "card:" + directory, typeof(ArticleTemplateData), articleData)
        );
    }

    //pagination
    //----------------------------------------------------------------------------------
    if (articleNumberForPagination % indexPageData.Paging.ArticlesPerPage == 0 || articleNumberForPagination == articleDirectories.Count())
    {
        indexPageData.Paging.CurrentPageIndex = pageNumber;

        //index
        //----------------------------------------------------------------------------------
        indexPageData.Body = bodyForIndexPage.ToString();
        var indexPage = templateService.RunCompile(mainTemplate, "index", typeof(MainTemplateData), indexPageData);
        var indexFileName = "index.html";
        if (pageNumber > 0)
        {
            indexFileName = $"index-page-{pageNumber + 1}.html";
        }
        Helpers.SaveCompressedHtml(Path.Combine(OUTPUT, indexFileName), indexPage);

        sitemap.Add(indexFileName, indexPageData.DateModified);

        bodyForIndexPage.Clear();
        pageNumber++;

    }
}


//google sitemap
//----------------------------------------------------------------------------------
var sitemapTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "teamplate-sitemap.cshtml"));
var sitemapPage = templateService.RunCompile(sitemapTemplate, "sitemap", typeof(SiteMap), sitemap);
Helpers.SaveCompressedHtml(Path.Combine(OUTPUT, "sitemap.xml"), sitemapPage);