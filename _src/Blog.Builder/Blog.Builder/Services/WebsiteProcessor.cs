using Blog.Builder.Interfaces;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Interfaces.Crawlers;
using Blog.Builder.Models;
using Blog.Builder.Models.Templates;
using Microsoft.Extensions.Options;

namespace Blog.Builder.Services;

/// <inheritdoc/>
internal class WebsitePreparation : IWebsiteProcessor
{
    private readonly IPageProcessor _pageProcessor;
    private readonly ICardProcessor _cardProcessor;
    private readonly ISitemapBuilder _sitemapBuilder;
    private readonly AppSettings appSettings;
    private readonly LayoutIndexModel layoutIndexModel;

    /// <summary>
    /// Besides DI, it creates and hold the basic information for the index pages. 
    /// See <seealso cref="LayoutIndexModel"/>.
    /// </summary>
    /// <param name="pageProcessor">The service that processes full pages (like index.html and privacy.html).</param>
    /// <param name="cardProcessor">The service that processes all cards.</param>
    /// <param name="sitemapBuilder">The service that builds the sitemap.xml.</param>
    /// <param name="options">The AppSettings</param>
    public WebsitePreparation(
                            IPageProcessor pageProcessor,
                            ICardProcessor cardProcessor,
                            ISitemapBuilder sitemapBuilder,
                            IOptions<AppSettings> options)
    {
        _pageProcessor = pageProcessor;
        _cardProcessor = cardProcessor;
        _sitemapBuilder = sitemapBuilder;
        appSettings = options.Value;

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
                CardsPerPage = appSettings.CardsPerPage,
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
        Directory.Delete(appSettings.OutputFolderPath, true);
        Directory.CreateDirectory(appSettings.OutputFolderPath);
        Directory.CreateDirectory(
            Path.Combine(appSettings.OutputFolderPath, appSettings.MediaFolderName)
        );
        Helpers.Copy(
            Path.Combine(appSettings.WorkingFolderPath, appSettings.WorkingJustCopyFolderName), 
            appSettings.OutputFolderPath
        );
    }

    /// <summary>
    /// Prepares all the standalone pages (like privacy.html).
    /// Standalones are scanned from <see cref="AppSettings.WorkingStandalonesFolderName"/>.
    /// </summary>
    private void PrepareStandalones()
    {

        var standalonesDirectory = Directory.GetDirectories(
            Path.Combine(appSettings.WorkingFolderPath, appSettings.WorkingStandalonesFolderName)
        );
        foreach (var directory in standalonesDirectory)
        {
            _pageProcessor.ProcessPage<LayoutStandaloneModel>(directory);
        }
    }

    /// <summary>
    /// Prepares all the article pages and the article cards for the index pages.
    /// Articles are scanned from <see cref="AppSettings.WorkingArticlesFolderName"/>.
    /// </summary>
    private void PrepareArticles()
    {
        var articleDirectories = Directory.GetDirectories(
            Path.Combine(appSettings.WorkingFolderPath, appSettings.WorkingArticlesFolderName)
        );
        foreach (var directory in articleDirectories)
        {
            _pageProcessor.ProcessPage<LayoutArticleModel>(directory);
        }
    }

    /// <summary>
    /// Prepares all the additional cards for the index pages.
    /// Additional cards are scanned from <see cref="AppSettings.WorkingCardsFolderName"/>.
    /// </summary>
    private async Task PrepareAdditionalCardsAsync()
    {
        var cardsDirectory = Directory.GetDirectories(
            Path.Combine(appSettings.WorkingFolderPath, appSettings.WorkingCardsFolderName)
        );
        foreach (var directory in cardsDirectory)
        {
            await _cardProcessor.ProcessCardAsync(directory);
        }
    }

    /// <summary>
    /// Prepares all the index pages (like index.html, index-page-2.html etc) 
    /// and copies it to <see cref="AppSettings.OutputFolderPath"/>.
    /// </summary>
    private void PrepareIndex()
    {
        var pageIndex = 0;
        var cardsNumber = _cardProcessor.GetCardsNumber(appSettings.CardsPerPage);
        layoutIndexModel.Paging.TotalCardsCount = cardsNumber;

        for (var i = appSettings.CardsPerPage; i < cardsNumber; i++)
        {
            if (i % appSettings.CardsPerPage == 0 || i == cardsNumber - 1)
            {
                layoutIndexModel.Paging.CurrentPageIndex = pageIndex++;
                _pageProcessor.ProcessIndex(layoutIndexModel, appSettings.CardsPerPage);
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
        await this.PrepareAdditionalCardsAsync(); //todo: enforce PrepareAdditionalCards before PrepareArticles
        this.PrepareStandalones();
        this.PrepareArticles();
        this.PrepareIndex();//todo: enforce PrepareIndex before PrepareArticles and PrepareAdditionalCards
        this.PrepareSitemap();
    }
}
