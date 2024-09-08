using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces.Crawlers;
using Blog.Builder.Models.Crawlers;
using Blog.Builder.Services.Crawlers.MeetupEventCrawlerTools;
using Geko.HttpClientService;
using Geko.HttpClientService.Extensions;

namespace Blog.Builder.Services.Crawlers;

/// <inheritdoc/>
internal class MeetupEventCrawler : IMeetupEventCrawler
{
    /// <summary>
    /// The Geko.HttpClientService.
    /// </summary>
    private readonly HttpClientService _httpClientService;

    public MeetupEventCrawler(IHttpClientServiceFactory requestServiceFactory)
    {
        _httpClientService = requestServiceFactory.CreateHttpClientService();
    }

    /// <inheritdoc/>
    public async Task<IList<CalendarEvent>> GetAsync(string ogranizer, Uri organizerUrl, Uri iCalendarUrl)
    {
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(ogranizer);
        ArgumentNullException.ThrowIfNull(organizerUrl);
        ArgumentNullException.ThrowIfNull(iCalendarUrl);

        var rssResult = await _httpClientService.GetAsync(iCalendarUrl.ToString());
        if (rssResult.HasError)
        {
            throw new Exception($"An error occured while loading the calendar: {rssResult.Error}.");
        }
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(rssResult.BodyAsString);

        //throws an exception if the format is wrong
        var calendarEvents = MeetupRssParser.Parse(rssResult.BodyAsString);

        var eventList = new List<CalendarEvent>();

        foreach (var evnt in calendarEvents)
        {
            eventList.Add(new CalendarEvent
            {
                Organizer = ogranizer,
                OrganizerUrl = organizerUrl.ToString(),
                Title = evnt.Title,
                DateTime = evnt.DateTime.ToUniversalTime(),
                Place = evnt.Place,
                Url = evnt.Url.ToString(),

            });
        }

        return eventList;
    }
}
