using Blog.Builder.Interfaces;
using Blog.Builder.Interfaces.Builders;
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
    private bool additionalCardsPrepared = false;
    private bool articlesPrepared = false;

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

        layoutIndexModel = new LayoutIndexModel
        {
            DatePublished = DateTime.Now,
            DateModified = DateTime.Now,
            Description = appSettings.BlogDescription,
            Tags = appSettings.BlogTags,
            Title = appSettings.BlogTitle,
            RelativeUrl = "/",
            RelativeImageUrl = appSettings.BlogImage,
            BlogUrl = appSettings.BlogUrl,
            Paging = new Paging
            {
                CardsPerPage = appSettings.CardsPerPage,
                CurrentPageIndex = 0,
                TotalCardsCount = 0
            }
        };
    }

    /// <summary>
    /// Prepares the output folder located at <see cref="AppSettings.OutputFolderPath"/> by deleting it first 
    /// and then creating all the necessary folders again.
    /// It will also copy all the <see cref="Consts.WorkingJustCopyFolderName"/> directly to <see cref="AppSettings.OutputFolderPath"/>.
    /// </summary>
    private void PrepareOutputFolders()
    {
        Directory.Delete(appSettings.OutputFolderPath, true);
        Directory.CreateDirectory(appSettings.OutputFolderPath);
        Directory.CreateDirectory(
            Path.Combine(appSettings.OutputFolderPath, Consts.MediaFolderName)
        );
        Helpers.Copy(
            Path.Combine(appSettings.WorkingFolderPath, Consts.WorkingJustCopyFolderName),
            appSettings.OutputFolderPath
        );
    }

    /// <summary>
    /// Prepares all the standalone pages (like privacy.html).
    /// Standalones are scanned from <see cref="Consts.WorkingStandalonesFolderName"/>.
    /// </summary>
    private void PrepareStandalonePages()
    {

        var standalonesDirectory = Directory.GetDirectories(
            Path.Combine(appSettings.WorkingFolderPath, Consts.WorkingStandalonesFolderName)
        );
        foreach (var directory in standalonesDirectory)
        {
            _pageProcessor.ProcessPage<LayoutStandaloneModel>(directory);
        }
    }

    /// <summary>
    /// Prepares all the article pages and the article cards for the index pages.
    /// Articles are scanned from <see cref="Consts.WorkingArticlesFolderName"/>.
    /// </summary>
    private void PrepareArticlePages()
    {
        if (!additionalCardsPrepared)
        {
            throw new Exception($"Method {nameof(PrepareArticlePages)} must be called after method {nameof(PrepareAdditionalCardsAsync)}.");
        }
        var articleDirectories = Directory.GetDirectories(
            Path.Combine(appSettings.WorkingFolderPath, Consts.WorkingArticlesFolderName)
        );
        foreach (var directory in articleDirectories)
        {
            _pageProcessor.ProcessPage<LayoutArticleModel>(directory);
        }

        articlesPrepared = true;
    }

    /// <summary>
    /// Prepares all the additional cards for the index pages.
    /// Additional cards are scanned from <see cref="Consts.WorkingCardsFolderName"/>.
    /// </summary>
    private async Task PrepareAdditionalCardsAsync()
    {
        var cardsDirectory = Directory.GetDirectories(
            Path.Combine(appSettings.WorkingFolderPath, Consts.WorkingCardsFolderName)
        );
        foreach (var directory in cardsDirectory)
        {
            await _cardProcessor.ProcessCardAsync(directory);
        }
        additionalCardsPrepared = true;
    }

    /// <summary>
    /// Prepares all the index pages (like index.html, index-page-2.html etc) 
    /// and copies it to <see cref="AppSettings.OutputFolderPath"/>.
    /// </summary>
    private void PrepareIndexPages()
    {
        if (!additionalCardsPrepared || !articlesPrepared)
        {
            throw new Exception($"Method {nameof(PrepareIndexPages)} must be called after methods {nameof(PrepareAdditionalCardsAsync)} and {nameof(PrepareArticlePages)}.");
        }
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
        await this.PrepareAdditionalCardsAsync();
        this.PrepareStandalonePages();
        this.PrepareArticlePages();
        this.PrepareIndexPages();
        this.PrepareSitemap();
    }

    public void Dispose()
    {
        _pageProcessor.Dispose();
        _sitemapBuilder.Dispose();
        _cardProcessor.Dispose();
    }
}
