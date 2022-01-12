using System.Text;
using Blog.Builder.Models;
using Blog.Builder.Services;
using Geko.HttpClientService;
using Geko.HttpClientService.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using SixLabors.ImageSharp;
using WebMarkupMin.Core;

var serviceProvider = new ServiceCollection()
          .AddLogging()
          .AddSingleton<IRazorEngineService>(provider =>
          {
              var configuration = new TemplateServiceConfiguration();
              configuration.EncodedStringFactory = new RawStringFactory();
              configuration.AllowMissingPropertiesOnDynamic = true;
              return RazorEngineService.Create(configuration);

          })
          .AddSingleton<ITemplateProvider, TemplateProvider>()
          .AddSingleton<ISitemapBuilder, SitemapBuilder>()
          .AddSingleton<IPageBuilder, PageBuilder>()
          .AddSingleton<IPathService, PathService>()
          .AddSingleton<IMarkupMinifier>(provider =>
          {
              var settings = new HtmlMinificationSettings()
              {
                  AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.Html5,
                  CollapseBooleanAttributes = true,
                  MinifyEmbeddedCssCode = true,
                  MinifyEmbeddedJsCode = true,
                  RemoveHtmlComments = true,
                  RemoveOptionalEndTags = false,
                  WhitespaceMinificationMode = WhitespaceMinificationMode.Safe
              };
              return new HtmlMinifier(settings);
          })
          .AddOptions()
          .Configure<AppSettings>( 
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables()
                    .Build()
          )
          .BuildServiceProvider();


//var configuration = new TemplateServiceConfiguration();
//configuration.EncodedStringFactory = new RawStringFactory();
//configuration.AllowMissingPropertiesOnDynamic = true;
//var templateService = RazorEngineService.Create(configuration);

//todo: include meetup and sessionize events
//todo: postprocessing of articles (fix html, add highlightjs markers)
//todo: bigger images on tap, is it possible?

//set paths
//var ROOT = "..\\..\\..\\..\\..\\..\\raw\\";
//var OUTPUT = "..\\..\\..\\..\\..\\..\\_output\\";

////load templates
//var mainTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template.cshtml"));
//var articleTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template-article.cshtml"));
//var cardArticleTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template-card-article.cshtml"));
//var cardImageTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template-card-image.cshtml"));
//var cardSearchTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "template-card-search.cshtml"));
//var articleDirectories = Directory.GetDirectories(Path.Combine(ROOT, "posts")).ToList().OrderByDescending(x => x);

var pathService = serviceProvider.GetService<IPathService>() ?? throw new NullReferenceException(nameof(IPathService));


////prepare output
Directory.Delete(pathService.FolderOutput, true);
Directory.CreateDirectory(pathService.FolderOutput);
Directory.CreateDirectory(pathService.FolderOutputMedia);
Helpers.Copy(pathService.FolderWorkingJustCopy, pathService.FolderOutput);

var articleDirectories = Directory.GetDirectories(pathService.FolderWorkingPosts).ToList().OrderByDescending(x => x);

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

var pageOrganiser = serviceProvider.GetService<IPageOrganiser>() ?? throw new NullReferenceException(nameof(IPageBuilder));

//standalones
//----------------------------------------------------------------------------------
var standalones = Directory.GetFiles(pathService.FolderWorkingStandalones, "*.html").ToList();
foreach (var standalonePath in standalones)
{
    pageBuilder.Build(standalonePath, PageTypes.Standalone);
}

//Meetup events
var httpClientService = HttpClientServiceFactory.Instance.CreateHttpClientService();
var iCalMeetup = await httpClientService.GetAsync("https://www.meetup.com/munich-dotnet-meetup/events/ical/");
var iCalSessionize = await httpClientService.GetAsync("https://sessionize.com/calendar/3a2660e0e9bd49cf853a35956e110a80");

//Sitemap preparation
var sitemap = new Sitemap();

//string build that will hold first page cards html
var bodyForIndexPage = new StringBuilder();

////Search
////----------------------------------------------------------------------------------
//bodyForIndexPage.Append(
//    templateService.RunCompile(cardSearchTemplate, "search", typeof(MainTemplateData), indexPageData)
//);

//index & article
//----------------------------------------------------------------------------------
var articleNumberForPagination = 0;
var pageNumber = 0;

//foreach (var directory in articleDirectories)
//{
//    articleNumberForPagination++;

//    var articleDataRaw = File.ReadAllText(Path.Combine(directory, "content.json"));
//    var articleData = JsonConvert.DeserializeObject<ArticleTemplateData>(articleDataRaw) ?? throw new NullReferenceException("Card Data: " + directory);
//    var articlePageData = JsonConvert.DeserializeObject<MainTemplateData>(articleDataRaw) ?? throw new NullReferenceException("Card Data: " + directory);

//    if (Directory.Exists(Path.Combine(directory, "media")))
//    {
//        Helpers.Copy(Path.Combine(directory, "media"), Path.Combine(OUTPUT, "media"));
//        foreach (var file in Directory.GetFiles(Path.Combine(directory, "media")))
//        {
//            var ext = Path.GetExtension(file);
//            var name = Path.GetFileNameWithoutExtension(file);
//            Helpers.ResizeImage(file, Path.Combine(OUTPUT, "media", name + "-small" + ext), new Size(500, 10000));//arbitrary big number
//        }
//    }

//    //article
//    //----------------------------------------------------------------------------------
//    if (articleData.Type == PageTypes.Article)
//    {
//        //sumup all cards for the first page
//        bodyForIndexPage.Append(
//            templateService.RunCompile(cardArticleTemplate, "card:" + directory, typeof(ArticleTemplateData), articleData)
//        );

//        //for each article page
//        articleData.Body = File.ReadAllText(Path.Combine(directory, "content.html"));
//        articlePageData.Body = templateService.RunCompile(articleTemplate, "body:" + directory, typeof(ArticleTemplateData), articleData);
//        var articlePageHtml = templateService.RunCompile(mainTemplate, "page:" + directory, typeof(MainTemplateData), articlePageData);

//        var articleFilename = articleData.RelativeUrl?.Trim('/').Split('/').Last();
//        Helpers.SaveCompressedHtml(Path.Combine(OUTPUT, $"{articleFilename}"), articlePageHtml);

//        sitemap.Add(articleFilename, articleData.DateModified);
//    }


//    //image
//    //----------------------------------------------------------------------------------
//    if (articleData.Type == PageTypes.Image)
//    {
//        bodyForIndexPage.Append(
//            templateService.RunCompile(cardImageTemplate, "card:" + directory, typeof(ArticleTemplateData), articleData)
//        );
//    }

//    //pagination
//    //----------------------------------------------------------------------------------
//    if (articleNumberForPagination % indexPageData.Paging.ArticlesPerPage == 0 || articleNumberForPagination == articleDirectories.Count())
//    {
//        indexPageData.Paging.CurrentPageIndex = pageNumber;

//        //index
//        //----------------------------------------------------------------------------------
//        indexPageData.Body = bodyForIndexPage.ToString();
//        var indexPage = templateService.RunCompile(mainTemplate, "index", typeof(MainTemplateData), indexPageData);
//        var indexFileName = "index.html";
//        if (pageNumber > 0)
//        {
//            indexFileName = $"index-page-{pageNumber + 1}.html";
//        }
//        Helpers.SaveCompressedHtml(Path.Combine(OUTPUT, indexFileName), indexPage);

//        sitemap.Add(indexFileName, indexPageData.DateModified);

//        bodyForIndexPage.Clear();
//        pageNumber++;

//    }
//}


////google sitemap
////----------------------------------------------------------------------------------
//var sitemapTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "templates", "teamplate-sitemap.cshtml"));
//var sitemapPage = templateService.RunCompile(sitemapTemplate, "sitemap", typeof(Sitemap), sitemap);
//Helpers.SaveCompressedHtml(Path.Combine(OUTPUT, "sitemap.xml"), sitemapPage);