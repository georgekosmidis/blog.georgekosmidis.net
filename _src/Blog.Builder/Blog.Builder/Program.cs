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
          .AddSingleton<IMainTemplateBuilder, PageBuilder>()
          .AddSingleton<IPathService, PathService>()
          .AddSingleton<IPagePreparation, PagePreparation>()
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

//todo: include meetup and sessionize events
//todo: postprocessing of articles (fix html, add highlightjs markers)
//todo: bigger images on tap, is it possible?

var pathService = serviceProvider.GetService<IPathService>() ?? throw new NullReferenceException(nameof(IPathService));


////prepare output
Directory.Delete(pathService.OutputFolder, true);
Directory.CreateDirectory(pathService.OutputFolder);
Directory.CreateDirectory(pathService.OutputMediaFolder);
Helpers.Copy(pathService.JustCopyFolder, pathService.OutputFolder);

var articleDirectories = Directory.GetDirectories(pathService.PostsFolder).ToList().OrderByDescending(x => x);

//data for index
//----------------------------------------------------------------------------------
var indexPageData = new IndexTemplateData
{
    DatePublished = DateTime.Now,
    DateModified = DateTime.Now,
    Description = "Microsoft MVP | Cloud Solutions Architect | .NET Software Engineer | Organizer of Munich .NET Meetup | Speaker",
    Tags = new List<string> { "C#", "CSharp", "dotnet", "ML.NET", "Q#", "Microsoft MVP" },
    Title = "George Kosmidis",
    RelativeUrl = "/",
    RelativeImageUrl = "/media/me.jpg",
    Paging = new Paging
    {
        ArticlesCount = articleDirectories.Count(),
        CurrentPageIndex = 0
    }
};

var pagePreparation = serviceProvider.GetService<IPagePreparation>() ?? throw new NullReferenceException(nameof(IMainTemplateBuilder));

//standalones
//----------------------------------------------------------------------------------
var standaloneDirectories = Directory.GetDirectories(pathService.StandalonesFolder).ToList();
foreach (var directory in standaloneDirectories)
{
    pagePreparation.PreparePage<StandaloneModel>(directory);
}

foreach (var directory in articleDirectories)
{
    //prepare an article page
    pagePreparation.PreparePage<ArticleModel>(directory);

    //var articleLayoutData = JsonConvert.DeserializeObject<TemplateLayoutModelBase>(articleData) ?? throw new NullReferenceException("Card Data: " + directory);

}

var sitemapBuilder = serviceProvider.GetService<ISitemapBuilder>() ?? throw new NullReferenceException(nameof(ISitemapBuilder));
sitemapBuilder.Build();

//Meetup events
//    var httpClientService = HttpClientServiceFactory.Instance.CreateHttpClientService();
//var iCalMeetup = await httpClientService.GetAsync("https://www.meetup.com/munich-dotnet-meetup/events/ical/");
//var iCalSessionize = await httpClientService.GetAsync("https://sessionize.com/calendar/3a2660e0e9bd49cf853a35956e110a80");


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
