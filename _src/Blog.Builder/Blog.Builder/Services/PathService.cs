using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces;
using Blog.Builder.Models;
using Microsoft.Extensions.Options;

namespace Blog.Builder.Services;

/// <inheritdoc/>
internal class PathService : IPathService
{
    /// <inheritdoc/>
    public string WorkingFolder { get; init; }
    /// <inheritdoc/>
    public string OutputFolder { get; init; }
    /// <inheritdoc/>
    public string OutputMediaFolder { get; init; }

    /// <inheritdoc/>
    public string WorkingTemplatesFolder { get; init; }
    /// <inheritdoc/>
    public string WorkingJustCopyFolder { get; init; }
    /// <inheritdoc/>
    public string WorkingArticlesFolder { get; init; }
    /// <inheritdoc/>
    public string WorkingStandalonesFolder { get; init; }
    /// <inheritdoc/>
    public string WorkingCardsFolder { get; init; }
    /// <inheritdoc/>
    public string WorkingEventsFolder { get; init; }

    /// <inheritdoc/>
    public string TemplateMainFile { get; init; }
    /// <inheritdoc/>
    public string TemplateArticleFile { get; init; }

    /// <inheritdoc/>
    public string TemplateSitemapFile { get; init; }

    /// <inheritdoc/>
    public string TemplateStandaloneFile { get; init; }

    /// <inheritdoc/>
    public string TemplateCardArticleFile { get; init; }
    /// <inheritdoc/>
    public string TemplateCardImageFile { get; init; }
    /// <inheritdoc/>
    public string TemplateCardSearchFile { get; init; }
    /// <inheritdoc/>
    public string TemplateCardCalendarEventsFile { get; init; }
    /// <inheritdoc/>
    public string OutputSitemapFile { get; init; }


    /// <summary>
    /// Besides DI, it creates and tests the application paths
    /// </summary>
    /// <param name="options">The AppSettings</param>
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

        WorkingEventsFolder = Path.Combine(WorkingFolder, appsettings.WorkingEventsFolderName);
        ExceptionHelpers.ThrowIfPathNotExists(WorkingEventsFolder);

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

        TemplateCardCalendarEventsFile = Path.Combine(WorkingTemplatesFolder, appsettings.TemplateCardCalendarEventsFilename);
        ExceptionHelpers.ThrowIfPathNotExists(TemplateCardCalendarEventsFile);



        OutputSitemapFile = Path.Combine(OutputFolder, "sitemap.xml");

    }

}
