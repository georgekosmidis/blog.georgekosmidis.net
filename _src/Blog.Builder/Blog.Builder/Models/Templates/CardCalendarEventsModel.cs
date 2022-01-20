using Blog.Builder.Models.Crawlers;

namespace Blog.Builder.Models.Templates;

/// <summary>
/// A card template model that displays a list of events .
/// </summary>
public record class CardCalendarEventsModel : CardModelBase
{

    /// <summary>
    /// The calendar events (meetups or file events) to be parted in the template.
    /// </summary>
    public IList<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();

    /// <summary>
    /// Converts a base <see cref="CardModelBase"/> to a <see cref="CardCalendarEventsModel"/> applying any custom logic
    /// for this unusual conversion.
    /// </summary>
    /// <param name="parent">The base <see cref="CardModelBase"/> to be used.</param>
    /// <returns>A new instance of a <see cref="CardCalendarEventsModel"/>.</returns>
    public static CardCalendarEventsModel FromBase(CardModelBase parent)
    {
        return new CardCalendarEventsModel
        {
            TemplateDataModel = parent.TemplateDataModel,
            ImageUrl = parent.ImageUrl,
            Title = parent.Title,
            Link = parent.Link,
            Position = parent.Position,
            IsSticky = parent.IsSticky
        };
    }

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();
    }

}