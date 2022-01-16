using Blog.Builder.Exceptions;
using Blog.Builder.Models;
using Blog.Builder.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Blog.Builder.Services;

/// <summary>
/// A service that returns all useful paths for the builder.
/// </summary>
internal class PathService : IPathService
{
    /// <summary>
    /// The working folder, the folder that contains standalones, articles etc.
    /// </summary>
    public string WorkingFolder { get; init; }
    /// <summary>
    /// The folder that everything will be copied.
    /// </summary>
    public string OutputFolder { get; init; }
    /// <summary>
    /// The output folder for the media.
    /// </summary>
    public string OutputMediaFolder { get; init; }

    /// <summary>
    /// The folder that contains the templates.
    /// </summary>
    public string WorkingTemplatesFolder { get; init; }
    /// <summary>
    /// The folder that contains files to be copied without any process.
    /// </summary>
    public string WorkingJustCopyFolder { get; init; }
    /// <summary>
    /// The folder that contains the articles.
    /// </summary>
    public string WorkingArticlesFolder { get; init; }
    /// <summary>
    /// The folder that contains the standalones, like privacy.html.
    /// </summary>
    public string WorkingStandalonesFolder { get; init; }
    /// <summary>
    /// The folder that contains additional cards, sticky or not.
    /// </summary>
    public string WorkingCardsFolder { get; init; }

    /// <summary>
    /// The main template file, used as a layout.
    /// </summary>
    public string TemplateMainFile { get; init; }
    /// <summary>
    /// The template for the articles.
    /// </summary>
    public string TemplateArticleFile { get; init; }
    /// <summary>
    /// The tempalte for the sitemap.xml.
    /// </summary>
    public string TemplateSitemapFile { get; init; }
    /// <summary>
    /// The template for the standalones.
    /// </summary>
    public string TemplateStandaloneFile { get; init; }

    /// <summary>
    /// The tempalte for the article cards.
    /// </summary>
    public string TemplateCardArticleFile { get; init; }
    /// <summary>
    /// The template for the image cards.
    /// </summary>
    public string TemplateCardImageFile { get; init; }
    /// <summary>
    /// The template for the search cards.
    /// </summary>
    public string TemplateCardSearchFile { get; init; }

    public string OutputSitemapFile { get; init; }

    /// <summary>
    /// Besides DI, it creates and tests the application paths
    /// </summary>
    /// <param name="options"></param>
    public PathService(IOptions<AppSettings> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        var appsettings = options.Value;
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(appsettings.OutputFolderPath);         //it is created in runtime
        ExceptionHelpers.ThrowIfPathNotExists(appsettings.WorkingFolderPath);           //must exists

        WorkingFolder = options.Value.WorkingFolderPath;
        OutputFolder = options.Value.OutputFolderPath;
        OutputMediaFolder = Path.Combine(OutputFolder, appsettings.OutputFolderMediaName);

        WorkingJustCopyFolder = Path.Combine(WorkingFolder, appsettings.WorkingJustCopyFolderName);
        ExceptionHelpers.ThrowIfPathNotExists(WorkingJustCopyFolder);

        WorkingArticlesFolder = Path.Combine(WorkingFolder, appsettings.WorkingArticlesFolderName);
        ExceptionHelpers.ThrowIfPathNotExists(WorkingArticlesFolder);

        WorkingStandalonesFolder = Path.Combine(WorkingFolder, appsettings.WorkingStandalonesFolderName);
        ExceptionHelpers.ThrowIfPathNotExists(WorkingStandalonesFolder);

        WorkingCardsFolder = Path.Combine(WorkingFolder, appsettings.WorkingCardsFolderName);
        ExceptionHelpers.ThrowIfPathNotExists(WorkingCardsFolder);

        WorkingTemplatesFolder = Path.Combine(WorkingFolder, appsettings.WorkingTemplatesFolderName);
        ExceptionHelpers.ThrowIfPathNotExists(WorkingTemplatesFolder);


        TemplateMainFile = Path.Combine(WorkingTemplatesFolder, appsettings.TemplateMainFilename);
        ExceptionHelpers.ThrowIfPathNotExists(TemplateMainFile);

        TemplateArticleFile = Path.Combine(WorkingTemplatesFolder, appsettings.TemplateArticleFilename);
        ExceptionHelpers.ThrowIfPathNotExists(TemplateArticleFile);

        TemplateStandaloneFile = Path.Combine(WorkingTemplatesFolder, appsettings.TemplateStandaloneFilename);
        ExceptionHelpers.ThrowIfPathNotExists(TemplateStandaloneFile);

        TemplateSitemapFile = Path.Combine(WorkingTemplatesFolder, appsettings.TemplateSitemapFilename);
        ExceptionHelpers.ThrowIfPathNotExists(TemplateSitemapFile);


        TemplateCardArticleFile = Path.Combine(WorkingTemplatesFolder, appsettings.TemplateCardArticleFilename);
        ExceptionHelpers.ThrowIfPathNotExists(TemplateCardArticleFile);

        TemplateCardImageFile = Path.Combine(WorkingTemplatesFolder, appsettings.TemplateCardImageFilename);
        ExceptionHelpers.ThrowIfPathNotExists(TemplateCardImageFile);

        TemplateCardSearchFile = Path.Combine(WorkingTemplatesFolder, appsettings.TemplateCardSearchFilename);
        ExceptionHelpers.ThrowIfPathNotExists(TemplateCardSearchFile);


        OutputSitemapFile = Path.Combine(OutputFolder, "sitemap.xml");

    }

}
