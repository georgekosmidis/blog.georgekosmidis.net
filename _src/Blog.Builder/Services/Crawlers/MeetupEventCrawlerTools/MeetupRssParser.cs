using Blog.Builder.Models.Crawlers;
using System.Xml.Linq;

namespace Blog.Builder.Services.Crawlers.MeetupEventCrawlerTools;
internal static class MeetupRssParser
{
    public static List<CalendarEvent> Parse(string rssContent)
    {
        var xDoc = XDocument.Parse(rssContent);
        var ns = "";

        var events = new List<CalendarEvent>();

        foreach (var item in xDoc.Descendants(ns + "item"))
        {
            var title = item.Element(ns + "title")?.Value!;
            var organizer = title.Split(':')[0].Trim();
            var eventTitle = title.Split(':')[1].Trim();
            var url = item.Element(ns + "guid")?.Value;
            var dateTime = ParseMeetupDateTime(item.Element(ns + "pubDate")?.Value!);

            events.Add(new CalendarEvent
            {
                Organizer = organizer,
                OrganizerUrl = url!,
                Title = eventTitle,
                DateTime = dateTime,
                Place = "", // RSS feed doesn't contain event location; you'll need to fetch it from the event URL or use another data source
                Url = url!,
            });
        }

        return events;
    }

    public static DateTime ParseMeetupDateTime(string input)
    {
        if (input.Contains("EDT"))
        {
            input = input.Replace("EDT", "-04:00"); // Replace EDT with UTC-4 offset
        }
        return DateTime.Parse(input);
    }

}
