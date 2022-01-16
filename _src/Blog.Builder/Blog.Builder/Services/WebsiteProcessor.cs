using Blog.Builder.Models;
using Blog.Builder.Models.Templates;
using Blog.Builder.Services.Interfaces;
using Blog.Builder.Services.Interfaces.Builders;
using Microsoft.Extensions.Options;

namespace Blog.Builder.Services;

internal class WebsitePreparation : IWebsitePreparation
{
    private readonly IPathService _pathService;
    private readonly IPageProcessor _pagePreparation;
    private readonly ICardProcessor _cardPreparation;
    private readonly ISitemapBuilder _sitemapBuilder;
    private readonly AppSettings _options;
    private readonly LayoutIndexModel layoutIndexModel;

    public WebsitePreparation(IPathService pathService,
                            IPageProcessor pageProcessor,
                            ICardProcessor cardProcessor,
                            ISitemapBuilder sitemapBuilder,
                            IOptions<AppSettings> options)
    {
        _pathService = pathService;
        _pagePreparation = pageProcessor;
        _cardPreparation = cardProcessor;
        _sitemapBuilder = sitemapBuilder;
        _options = options.Value;

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
                CardsCount = 0,
                CurrentPageIndex = 0
            }
        };
    }

    private void PrepareOutputFolders()
    {
        Directory.Delete(_pathService.OutputFolder, true);
        Directory.CreateDirectory(_pathService.OutputFolder);
        Directory.CreateDirectory(_pathService.OutputMediaFolder);
        Helpers.Copy(_pathService.WorkingJustCopyFolder, _pathService.OutputFolder);
    }

    private void PrepareStandalones()
    {
        var standalonesDirectory = Directory.GetDirectories(_pathService.WorkingStandalonesFolder);
        foreach (var directory in standalonesDirectory)
        {
            _pagePreparation.ProcessPage<LayoutStandaloneModel>(directory);
        }
    }

    private void PrepareArticles()
    {
        var articleDirectories = Directory.GetDirectories(_pathService.WorkingArticlesFolder);
        foreach (var directory in articleDirectories)
        {
            _pagePreparation.ProcessPage<LayoutArticleModel>(directory);
        }
    }

    private void PrepareAdditionalCards()
    {
        var cardsDirectory = Directory.GetDirectories(_pathService.WorkingCardsFolder);
        foreach (var directory in cardsDirectory)
        {
            _cardPreparation.ProcessCard(directory);
        }
    }

    private void PrepareIndex()
    {
        var pageIndex = 0;
        var cardsNumber = _cardPreparation.GetCardsNumber(_options.CardsPerPage);
        layoutIndexModel.Paging.CardsCount = cardsNumber;

        for (var i = _options.CardsPerPage; i < cardsNumber; i++)
        {
            if (i % _options.CardsPerPage == 0 || i == cardsNumber - 1)
            {
                layoutIndexModel.Paging.CurrentPageIndex = pageIndex++;
                _pagePreparation.ProcessIndex(layoutIndexModel, _options.CardsPerPage);
            }
        }
    }

    private void PrepareSitemap()
    {
        _sitemapBuilder.Build();
    }

    public void Prepare()
    {
        this.PrepareOutputFolders();
        this.PrepareStandalones();
        this.PrepareArticles();
        this.PrepareAdditionalCards();
        this.PrepareIndex();//todo: must be called after PrepareArticles and PrepareAdditionalCards, check that
        this.PrepareSitemap();
    }
}
