using Blog.Builder.Models.Crawlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var dateTime = DateTime.Parse(item.Element(ns + "pubDate")?.Value!);

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
}
