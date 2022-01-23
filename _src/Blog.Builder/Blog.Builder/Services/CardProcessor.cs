using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Interfaces.Crawlers;
using Blog.Builder.Models;
using Blog.Builder.Models.Crawlers;
using Blog.Builder.Models.Templates;
using Newtonsoft.Json;
using SixLabors.ImageSharp;

namespace Blog.Builder.Services;

/// <inheritdoc/>
internal class CardProcessor : ICardProcessor
{
    private readonly IPathService _pathService;
    private readonly ICardBuilder _cardBuilder;
    private readonly IMeetupEventCrawler _meetupEventCrawler;
    private readonly IFileEventCrawler _fileEventCrawler;

    public CardProcessor(IPathService pathService,
                        ICardBuilder cardBuilder,
                        IMeetupEventCrawler meetupEventCrawler,
                        IFileEventCrawler fileEventCrawler)
    {
        ArgumentNullException.ThrowIfNull(pathService);
        ArgumentNullException.ThrowIfNull(cardBuilder);
        ArgumentNullException.ThrowIfNull(meetupEventCrawler);
        ArgumentNullException.ThrowIfNull(fileEventCrawler);

        _pathService = pathService;
        _cardBuilder = cardBuilder;
        _meetupEventCrawler = meetupEventCrawler;
        _fileEventCrawler = fileEventCrawler;
    }

    /// <summary>
    /// Reads the JSON from <paramref name="jsonPath"/> and retuns an object of type <see cref="CardModelBase"/>.
    /// </summary>
    /// <param name="jsonPath">The path to a valid JSON.</param>
    /// <returns>A <see cref="CardModelBase"/> with the data from the JSON file.</returns>
    private CardModelBase GetCardModelData(string jsonPath)
    {
        ExceptionHelpers.ThrowIfPathNotExists(jsonPath);

        var json = File.ReadAllText(jsonPath);
        var data = JsonConvert.DeserializeObject<CardModelBase>(json);
        ExceptionHelpers.ThrowIfNull(data);

        data.Validate();

        return data;
    }

    /// <inheritdoc/>
    public async Task ProcessCardAsync(string directory)
    {
        ExceptionHelpers.ThrowIfPathNotExists(directory);

        var jsonFileContent = Path.Combine(directory, "card.json");
        var cardDataBase = this.GetCardModelData(jsonFileContent);

        //Find the correct model for this card.
        switch (cardDataBase.TemplateDataModel)
        {
            case nameof(CardSearchModel):
                _cardBuilder.AddCard(CardSearchModel.FromBase(cardDataBase));
                break;
            case nameof(CardImageModel):
                _cardBuilder.AddCard(CardImageModel.FromBase(cardDataBase));
                break;
            case nameof(CardCalendarEventsModel):
                var calendarCard = CardCalendarEventsModel.FromBase(cardDataBase);
                calendarCard.CalendarEvents = await this.GetCalendarEvents();
                _cardBuilder.AddCard(calendarCard);
                break;
            case nameof(CardArticleModel):
                throw new Exception($"Method {nameof(ProcessCardAsync)} cannot be used with {nameof(CardArticleModel)}, use {nameof(ProcessArticleCard)} instead.");
            default:
                throw new Exception($"{cardDataBase.TemplateDataModel} switch is missing.");
        };

        //copy all media associated with this card
        if (Directory.Exists(Path.Combine(directory, "media")))
        {
            Helpers.Copy(Path.Combine(directory, "media"), _pathService.OutputMediaFolder);

            //create smaller versions of the media
            foreach (var file in Directory.GetFiles(Path.Combine(directory, "media")))
            {
                var ext = Path.GetExtension(file);
                var name = Path.GetFileNameWithoutExtension(file);
                Helpers.ResizeImage(file, Path.Combine(_pathService.OutputMediaFolder, name + "-small" + ext), new Size(300, 10000));//stop at 300 width, who cares about height
            }
        }
    }

    /// <summary>
    /// Retrieves all calendar events from meetup.com but also from a file repo located at <see cref="AppSettings.WorkingEventsFolderName"/>.
    /// </summary>
    /// <returns>A list of <see cref="CalendarEvent"/>.</returns>
    private async Task<IList<CalendarEvent>> GetCalendarEvents()
    {
        //todo: use appsettings for this values
        var calendarEvents = (await _meetupEventCrawler.GetAsync("Munich .NET Meetup",
                        new Uri("https://www.meetup.com/munich-dotnet-meetup/"),
                        new Uri("https://www.meetup.com/munich-dotnet-meetup/events/ical/"))
        ).ToList();

        calendarEvents.AddRange(
            _fileEventCrawler.Get(_pathService.WorkingEventsFolder)
        );

        return calendarEvents;
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetRightColumnCardsHtml()
    {
        return _cardBuilder.GetRightColumnCardsHtml();
    }

    /// <inheritdoc/>
    public void ProcessArticleCard(CardArticleModel cardData, DateTime datePublished)
    {
        ArgumentNullException.ThrowIfNull(cardData);
        _cardBuilder.AddArticleCard(cardData, datePublished);
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetBodyCardsHtml(int pageIndex, int cardsPerPage)
    {
        return _cardBuilder.GetBodyCardsHtml(pageIndex, cardsPerPage);
    }

    /// <inheritdoc/>
    public int GetCardsNumber(int cardsPerPage)
    {
        return _cardBuilder.GetCardsNumber(cardsPerPage);
    }

}
