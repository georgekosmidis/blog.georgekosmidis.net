using Blog.Builder.Interfaces;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Interfaces.Crawlers;
using Blog.Builder.Interfaces.RazorEngineServices;
using Blog.Builder.Models;
using Blog.Builder.Services;
using Blog.Builder.Services.Builders;
using Blog.Builder.Services.Crawlers;
using Blog.Builder.Services.RazorEngineServices;
using Geko.HttpClientService.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RazorEngine.Templating;
using WebMarkupMin.Core;

var serviceProvider = new ServiceCollection()
          .AddLogging()
          .AddHttpClientService()
          .AddSingleton<IRazorEngineWrapperService, RazorEngineWrapperService>()
          .AddSingleton<ITemplateManager, TemplateManager>()
          .AddSingleton<ITemplateProvider, TemplateProvider>()
          .AddSingleton<ISitemapBuilder, SitemapBuilder>()
          .AddSingleton<IPageBuilder, PageBuilder>()
          .AddSingleton<IPageProcessor, PageProcessor>()
          .AddSingleton<ICardBuilder, CardBuilder>()
          .AddSingleton<ICardProcessor, CardProcessor>()
          .AddSingleton<IWebsiteProcessor, WebsitePreparation>()
          .AddSingleton<IMeetupEventCrawler, MeetupEventCrawler>()
          .AddSingleton<IFileEventCrawler, FileEventCrawler>()
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


//var httpService = serviceProvider.GetService<IHttpClientServiceFactory>() ?? throw new NullReferenceException(nameof(IWebsiteProcessor));
//new MeetupEventCrawler(httpService)
//        .GetAsync("Munich .NET Meetup", 
//                new Uri("https://www.meetup.com/munich-dotnet-meetup/"), 
//                new Uri("https://www.meetup.com/munich-dotnet-meetup/events/ical/"))
//        .GetAwaiter()
//        .GetResult();

//todo: clean template models, it seems its way too complicated now
//todo: bigger images on tap, is it possible?
//todo: add commenting system
//todo: fix style of some articles, example: http://blog/net-6-a-guide-for-the-high-impact-breaking-changes.html

var websitePreparation = serviceProvider.GetService<IWebsiteProcessor>() ?? throw new NullReferenceException(nameof(IWebsiteProcessor));
await websitePreparation.PrepareAsync();

//Meetup events
//    var httpClientService = HttpClientServiceFactory.Instance.CreateHttpClientService();
//var iCalMeetup = await httpClientService.GetAsync("https://www.meetup.com/munich-dotnet-meetup/events/ical/");
//var iCalSessionize = await httpClientService.GetAsync("https://sessionize.com/calendar/3a2660e0e9bd49cf853a35956e110a80");
