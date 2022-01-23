namespace Blog.Builder.Models;

/// <summary>
/// The AppSettings of this app.
/// </summary>
internal record class AppSettings
{
    /// <summary>
    /// The working folder, the folder that contains standalones, articles etc.
    /// </summary>
    public string WorkingFolderPath { get; init; } = default!;

    /// <summary>
    /// The folder that everything will be copied at the end of the process.
    /// </summary>
    public string OutputFolderPath { get; init; } = default!;

    /// <summary>
    /// The name of the media folder in the output directory.
    /// </summary>
    public string OutputFolderMediaName { get; init; } = default!;

    /// <summary>
    /// The name of the folder that holds all tempaltes.
    /// </summary>
    public string WorkingTemplatesFolderName { get; init; } = default!;

    /// <summary>
    /// The name of the folder that holds all items to be copied directly without any process.
    /// </summary>
    public string WorkingJustCopyFolderName { get; init; } = default!;

    /// <summary>
    /// The name of the folder where all the articles live.
    /// </summary>
    public string WorkingArticlesFolderName { get; init; } = default!;

    /// <summary>
    /// The name of the folder for the standalones (like privacy.html).
    /// </summary>
    public string WorkingStandalonesFolderName { get; init; } = default!;

    /// <summary>
    /// The name of the folder that holds all additional cards (besides the article cards).
    /// </summary>
    public string WorkingCardsFolderName { get; init; } = default!;

    /// <summary>
    /// The name of the folder that holds information about the events, along with its card.json.
    /// </summary>
    public string WorkingEventsFolderName { get; init; } = default!;

    /// <summary>
    /// The filename of the main template, the layout.
    /// </summary>
    public string TemplateMainFilename { get; init; } = default!;

    /// <summary>
    /// The filename of the index page.
    /// </summary>
    public string TemplateIndexFilename { get; init; } = default!;

    /// <summary>
    /// The filename of the article template
    /// </summary>
    public string TemplateArticleFilename { get; init; } = default!;

    /// <summary>
    /// The filename of the sitemap template.
    /// </summary>
    public string TemplateSitemapFilename { get; init; } = default!;

    /// <summary>
    /// The filename of the template for the standalones.
    /// </summary>
    public string TemplateStandaloneFilename { get; init; } = default!;

    /// <summary>
    /// The filename of the template for the article cards.
    /// </summary>
    public string TemplateCardArticleFilename { get; init; } = default!;

    /// <summary>
    /// The filename of the template from the image cards.
    /// </summary>
    public string TemplateCardImageFilename { get; init; } = default!;

    /// <summary>
    /// The filename of the template for the search.
    /// </summary>
    public string TemplateCardSearchFilename { get; init; } = default!;

    /// <summary>
    /// The filename of the template for the calendar events.
    /// </summary>
    public string TemplateCardCalendarEventsFilename { get; init; } = default!;

    /// <summary>
    /// The number of cards per page (defauls is 9).
    /// </summary>
    public int CardsPerPage { get; init; } = 9;

}

