using Blog.Builder.Exceptions;
using Blog.Builder.Models;
using Blog.Builder.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Blog.Builder.Services;

internal class PathService : IPathService
{
    //folders
    public string WorkingFolder { get; init; }
    public string OutputFolder { get; init; }
    public string OutputMediaFolder { get; init; }

    public string WorkingTemplatesFolder { get; init; }
    public string WorkingJustCopyFolder { get; init; }
    public string WorkingArticlesFolder { get; init; }
    public string WorkingStandalonesFolder { get; init; }
    public string WorkingCardsFolder { get; init; }

    //files
    public string TemplateMainFile { get; init; }
    public string TemplateArticleFile { get; init; }
    public string TemplateSitemapFile { get; init; }
    public string TemplateStandaloneFile { get; init; }

    public string TemplateCardArticleFile { get; init; }
    public string TemplateCardImageFile { get; init; }
    public string TemplateCardSearchFile { get; init; }

    public string OutputSitemapFile { get; init; }

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
