using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Builder;

/// <summary>
/// Constants used through out the system.
/// </summary>
public static class Consts
{
    /// <summary>
    /// The name of the media folder in the output directory.
    /// </summary>
    public const string MediaFolderName = "media";

    /// <summary>
    /// The name of the folder that holds all tempaltes.
    /// </summary>
    public const string WorkingTemplatesFolderName = "templates";

    /// <summary>
    /// The name of the folder that holds all items to be copied directly without any process.
    /// </summary>
    public const string WorkingJustCopyFolderName = "justcopyme";

    /// <summary>
    /// The name of the folder where all the articles live.
    /// </summary>
    public const string WorkingArticlesFolderName = "articles";

    /// <summary>
    /// The name of the folder for the standalones (like privacy.html).
    /// </summary>
    public const string WorkingStandalonesFolderName = "standalones";

    /// <summary>
    /// The name of the folder that holds all additional cards (besides the article cards).
    /// </summary>
    public const string WorkingCardsFolderName = "cards";

    /// <summary>
    /// The name of the folder that holds information about the events, along with its card.json.
    /// It's a static and not constant because <see cref="Path.DirectorySeparatorChar"/> is a static and not const!
    /// </summary>
    public static string WorkingEventsFolderName = $"{WorkingCardsFolderName}{Path.DirectorySeparatorChar}events";

    /// <summary>
    /// The filename of the main template, the layout.
    /// </summary>
    public const string TemplateMainFilename = "template-layout.cshtml";

    /// <summary>
    /// The filename of the index page.
    /// </summary>
    public const string TemplateIndexFilename = "template-index.cshtml";

    /// <summary>
    /// The filename of the article template
    /// </summary>
    public const string TemplateArticleFilename = "template-article.cshtml";

    /// <summary>
    /// The filename of the sitemap template.
    /// </summary>
    public const string TemplateSitemapFilename = "template-sitemap.cshtml";

    /// <summary>
    /// The filename of the template for the standalones.
    /// </summary>
    public const string TemplateStandaloneFilename = "template-standalone.cshtml";

    /// <summary>
    /// The filename of the template for the article cards.
    /// </summary>
    public const string TemplateCardArticleFilename = "template-card-article.cshtml";

    /// <summary>
    /// The filename of the template from the image cards.
    /// </summary>
    public const string TemplateCardImageFilename = "template-card-image.cshtml";

    /// <summary>
    /// The filename of the template for the search.
    /// </summary>
    public const string TemplateCardSearchFilename = "template-card-search.cshtml";

    /// <summary>
    /// The filename of the template for the calendar events.
    /// </summary>
    public const string TemplateCardCalendarEventsFilename = "template-card-calendar-events.cshtml";

}
