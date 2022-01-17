using Blog.Builder.Models;
using Blog.Builder.Services;
using Blog.Builder.Services.Builders;
using Blog.Builder.Services.Interfaces;
using Blog.Builder.Services.Interfaces.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
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
          .AddSingleton<IPageProcessor, PageProcessor>()
          .AddSingleton<ICardBuilder, CardBuilder>()
          .AddSingleton<ICardProcessor, CardProcessor>()
          .AddSingleton<IWebsiteProcessor, WebsitePreparation>()
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

//todo: lists, like Tags, are not working correctly
//todo: unique images of additional cards (do not overwrite media)
//todo: last modified on cards and in the article page
//todo: include meetup and sessionize events
//todo: postprocessing of articles (fix html, add highlightjs markers)
//todo: bigger images on tap, is it possible?
//todo: cards should define the "target" property for <a> tags
//todo: add commenting system
//todo: postprocessing: process media and check dimensions (e.g. 100210-actionresult_derives.png)

var websitePreparation = serviceProvider.GetService<IWebsiteProcessor>() ?? throw new NullReferenceException(nameof(IWebsiteProcessor));
websitePreparation.Prepare();

//Meetup events
//    var httpClientService = HttpClientServiceFactory.Instance.CreateHttpClientService();
//var iCalMeetup = await httpClientService.GetAsync("https://www.meetup.com/munich-dotnet-meetup/events/ical/");
//var iCalSessionize = await httpClientService.GetAsync("https://sessionize.com/calendar/3a2660e0e9bd49cf853a35956e110a80");
