using Blog.Builder.Models;
using Blog.Builder.Models.Templates;
using Blog.Builder.Services;
using Blog.Builder.Services.Builders;
using Blog.Builder.Services.Interfaces;
using Blog.Builder.Services.Interfaces.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System.Text;
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
          .AddSingleton<IPagePreparation, PagePreparation>()
          .AddSingleton<ICardBuilder, CardBuilder>()
          .AddSingleton<ICardPreparation, CardPreparation>()
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


//prepare output
Directory.Delete(pathService.OutputFolder, true);
Directory.CreateDirectory(pathService.OutputFolder);
Directory.CreateDirectory(pathService.OutputMediaFolder);
Helpers.Copy(pathService.JustCopyFolder, pathService.OutputFolder);


var pagePreparation = serviceProvider.GetService<IPagePreparation>() ?? throw new NullReferenceException(nameof(IPageBuilder));

//standalone pages and their sitemap registration
//----------------------------------------------------------------------------------
var standalonesDirectory = Directory.GetDirectories(pathService.StandalonesFolder);
foreach (var directory in standalonesDirectory)
{
    pagePreparation.PreparePage<LayoutStandaloneModel>(directory);
}

//articles, article cards and their sitemap registration
//----------------------------------------------------------------------------------
var articleDirectories = Directory.GetDirectories(pathService.ArticlesFolder);
foreach (var directory in articleDirectories)
{
    pagePreparation.PreparePage<LayoutArticleModel>(directory);
}

//cards
//----------------------------------------------------------------------------------
var cardPreparation = serviceProvider.GetService<ICardPreparation>() ?? throw new NullReferenceException(nameof(ICardPreparation));
var cardsDirectory = Directory.GetDirectories(pathService.CardsFolder);
foreach (var directory in cardsDirectory)
{
    cardPreparation.RegisterCard(directory);
}


//index page and paging
//----------------------------------------------------------------------------------
var indexPageData = new LayoutIndexModel
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
        CardsCount = cardPreparation.GetCardsNumber(),
        CurrentPageIndex = 0
    }
};

//first page
var pageIndex = 0;
pagePreparation.PrepareIndex(indexPageData, 9);


for (var i = 8; i < cardPreparation.GetCardsNumber(); i++)
{
    if(i % 9 == 0 || i == cardPreparation.GetCardsNumber() - 1)
    {
        indexPageData.Paging.CurrentPageIndex = pageIndex++;
        pagePreparation.PrepareIndex(indexPageData, 9);
    }
}

//Google Sitemaps
//----------------------------------------------------------------------------------
var sitemapBuilder = serviceProvider.GetService<ISitemapBuilder>() ?? throw new NullReferenceException(nameof(ISitemapBuilder));
sitemapBuilder.Build();

//Meetup events
//    var httpClientService = HttpClientServiceFactory.Instance.CreateHttpClientService();
//var iCalMeetup = await httpClientService.GetAsync("https://www.meetup.com/munich-dotnet-meetup/events/ical/");
//var iCalSessionize = await httpClientService.GetAsync("https://sessionize.com/calendar/3a2660e0e9bd49cf853a35956e110a80");
