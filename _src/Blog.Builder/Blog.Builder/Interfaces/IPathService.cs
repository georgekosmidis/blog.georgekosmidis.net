namespace Blog.Builder.Interfaces;

/// <summary>
/// A service that returns all useful paths for the builder.
/// </summary>
internal interface IPathService
{
    /// <summary>
    /// The template for the articles.
    /// </summary>
    string TemplateArticleFile { get; init; }

    /// <summary>
    /// The main template file, used as a layout.
    /// </summary>
    string TemplateMainFile { get; init; }

    /// <summary>
    /// The template for the index pages.
    /// </summary>
    string TempalteIndexFile { get; init; }

    /// <summary>
    /// The template for the standalones.
    /// </summary>
    string TemplateStandaloneFile { get; init; }

    /// <summary>
    /// The tempalte for the article cards.
    /// </summary>
    string TemplateCardArticleFile { get; init; }

    /// <summary>
    /// The template for the calendar cards
    /// </summary>
    string TemplateCardCalendarEventsFile { get; init; }

    /// <summary>
    /// The template for the image cards.
    /// </summary>
    string TemplateCardImageFile { get; init; }

    /// <summary>
    /// The template for the search cards.
    /// </summary>
    string TemplateCardSearchFile { get; init; }

    /// <summary>
    /// The tempalte for the sitemap.xml.
    /// </summary>
    string TemplateSitemapFile { get; init; }

    /// <summary>
    /// The folder that everything will be copied at the end of the process.
    /// </summary>
    string OutputFolder { get; init; }

    /// <summary>
    /// The output folder for the media.
    /// </summary>
    string OutputMediaFolder { get; init; }

    /// <summary>
    /// The output file for the sitemap.xml
    /// </summary>
    string OutputSitemapFile { get; init; }

    /// <summary>
    /// The working folder, the folder that contains standalones, articles etc.
    /// </summary>
    string WorkingFolder { get; init; }

    /// <summary>
    /// The folder that contains files to be copied without any process.
    /// </summary>
    string WorkingJustCopyFolder { get; init; }

    /// <summary>
    /// The folder that contains the articles.
    /// </summary>
    string WorkingArticlesFolder { get; init; }

    /// <summary>
    /// The folder that contains information about the events.
    /// </summary>
    string WorkingEventsFolder { get; init; }

    /// <summary>
    /// The folder that contains the standalones, like privacy.html.
    /// </summary>
    string WorkingStandalonesFolder { get; init; }

    /// <summary>
    /// The folder that contains the templates.
    /// </summary>
    string WorkingTemplatesFolder { get; init; }

    /// <summary>
    /// The folder that contains additional cards, sticky or not.
    /// </summary>
    string WorkingCardsFolder { get; init; }

}