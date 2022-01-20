using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Interfaces.Crawlers;
using Blog.Builder.Models;
using Blog.Builder.Models.Templates;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Blog.Builder.Services;

/// <inheritdoc/>
internal class WebsitePreparation : IWebsiteProcessor
{
    private readonly IPathService _pathService;
    private readonly IPageProcessor _pagePreparation;
    private readonly ICardProcessor _cardPreparation;
    private readonly ISitemapBuilder _sitemapBuilder;
    private readonly IMeetupEventCrawler _meetupEventCrawler;
    private readonly AppSettings _options;
    private readonly LayoutIndexModel layoutIndexModel;

    /// <summary>
    /// Besides DI, it creates and hold the basic information for the index pages. 
    /// See <seealso cref="LayoutIndexModel"/>.
    /// </summary>
    /// <param name="pathService">The service that build and checks all the paths.</param>
    /// <param name="pageProcessor">The service that processes full pages (like index.html and privacy.html).</param>
    /// <param name="cardProcessor">The service that processes all cards.</param>
    /// <param name="sitemapBuilder">The service that builds the sitemap.xml.</param>
    /// <param name="options">The AppSettings</param>
    public WebsitePreparation(IPathService pathService,
                            IPageProcessor pageProcessor,
                            ICardProcessor cardProcessor,
                            ISitemapBuilder sitemapBuilder,
                            IMeetupEventCrawler meetupEventCrawler,
                            IOptions<AppSettings> options)
    {
        _pathService = pathService;
        _pagePreparation = pageProcessor;
        _cardPreparation = cardProcessor;
        _sitemapBuilder = sitemapBuilder;
        _meetupEventCrawler = meetupEventCrawler;
        _options = options.Value;

        //todo: use appsettings for this values
        layoutIndexModel = new LayoutIndexModel
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
                CardsPerPage = _options.CardsPerPage,
                TotalCardsCount = 0,
                CurrentPageIndex = 0
            }
        };
    }

    /// <summary>
    /// Prepares the output folder located at <see cref="AppSettings.OutputFolderPath"/> by deleting it first 
    /// and then creating all the necessary folders again.
    /// It will also copy all the <see cref="AppSettings.WorkingJustCopyFolderName"/> directly to <see cref="AppSettings.OutputFolderPath"/>.
    /// </summary>
    private void PrepareOutputFolders()
    {
        Directory.Delete(_pathService.OutputFolder, true);
        Directory.CreateDirectory(_pathService.OutputFolder);
        Directory.CreateDirectory(_pathService.OutputMediaFolder);
        Helpers.Copy(_pathService.WorkingJustCopyFolder, _pathService.OutputFolder);
    }

    /// <summary>
    /// Prepares all the standalone pages (like privacy.html).
    /// Standalones are scanned from <see cref="AppSettings.WorkingStandalonesFolderName"/>.
    /// </summary>
    private void PrepareStandalones()
    {
        var standalonesDirectory = Directory.GetDirectories(_pathService.WorkingStandalonesFolder);
        foreach (var directory in standalonesDirectory)
        {
            _pagePreparation.ProcessPage<LayoutStandaloneModel>(directory);
        }
    }

    /// <summary>
    /// Prepares all the article pages and the article cards for the index pages.
    /// Articles are scanned from <see cref="AppSettings.WorkingArticlesFolderName"/>.
    /// </summary>
    private void PrepareArticles()
    {
        var articleDirectories = Directory.GetDirectories(_pathService.WorkingArticlesFolder);
        foreach (var directory in articleDirectories)
        {
            _pagePreparation.ProcessPage<LayoutArticleModel>(directory);
        }
    }

    /// <summary>
    /// Prepares all the additional cards for the index pages.
    /// Additional cards are scanned from <see cref="AppSettings.WorkingCardsFolderName"/>.
    /// </summary>
    private void PrepareAdditionalCards()
    {
        var cardsDirectory = Directory.GetDirectories(_pathService.WorkingCardsFolder);
        foreach (var directory in cardsDirectory)
        {
            _cardPreparation.ProcessCard(directory);
        }
    }

    private async Task PrepareCalendarEventsAsync()
    {
        var jsonFileContent = File.ReadAllText(Path.Combine(_pathService.WorkingEventsFolder, "card.json"));
        var cardDataBase = JsonConvert.DeserializeObject<CardCalendarEventsModel>(jsonFileContent);
        ExceptionHelpers.ThrowIfNull(cardDataBase);
        cardDataBase.Validate();

        //todo: use appsettings for this values
        cardDataBase.CalendarEvents = await _meetupEventCrawler.GetAsync("Munich .NET Meetup",
                        new Uri("https://www.meetup.com/munich-dotnet-meetup/"),
                        new Uri("https://www.meetup.com/munich-dotnet-meetup/events/ical/"));

        //only add the card if there are events to show
        if(cardDataBase.CalendarEvents.Count() > 0)
        {
            _cardPreparation.ProcessCalendarEventCard(cardDataBase);
        }
    }

    /// <summary>
    /// Prepares all the index pages (like index.html, index-page-2.html etc) 
    /// and copies it to <see cref="AppSettings.OutputFolderPath"/>.
    /// </summary>
    private void PrepareIndex()
    {
        var pageIndex = 0;
        var cardsNumber = _cardPreparation.GetCardsNumber(_options.CardsPerPage);
        layoutIndexModel.Paging.TotalCardsCount = cardsNumber;

        for (var i = _options.CardsPerPage; i < cardsNumber; i++)
        {
            if (i % _options.CardsPerPage == 0 || i == cardsNumber - 1)
            {
                layoutIndexModel.Paging.CurrentPageIndex = pageIndex++;
                _pagePreparation.ProcessIndex(layoutIndexModel, _options.CardsPerPage);
            }
        }
    }

    /// <summary>
    /// Prepares the google site map and copies it to <see cref="AppSettings.OutputFolderPath"/>.
    /// </summary>
    private void PrepareSitemap()
    {
        _sitemapBuilder.Build();
    }

    /// <inheritdoc/>
    public async Task PrepareAsync()
    {
        this.PrepareOutputFolders();
        this.PrepareStandalones();
        this.PrepareArticles();
        this.PrepareAdditionalCards();
        await this.PrepareCalendarEventsAsync();
        this.PrepareIndex();//todo: must be called after PrepareArticles and PrepareAdditionalCards, check that
        this.PrepareSitemap();
    }
}
