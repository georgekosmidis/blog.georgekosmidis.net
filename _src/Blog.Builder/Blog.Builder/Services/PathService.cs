using Blog.Builder.Exceptions;
using Blog.Builder.Models;
using Microsoft.Extensions.Options;

namespace Blog.Builder.Services;

internal class PathService : IPathService
{
    public string FolderWorking { get; init; }

    public string FolderWorkingTemplates { get; init; }
    public string FolderWorkingJustCopy { get; init; }
    public string FolderWorkingPosts { get; init; }
    public string FolderWorkingStandalones { get; init; }


    public string FileWorkingTemplateBase { get; init; }
    public string FileWorkingTemplateArticle { get; init; }
    public string FileWorkingTemplateCardArticle { get; init; }
    public string FileWorkingTemplateCardImage { get; init; }
    public string FileWorkingTemplateCardSearch { get; init; }
    public string FileWorkingTemplateSitemap { get; init; }



    public string FolderOutput { get; init; }
    public string FolderOutputMedia { get; init; }
    public string FileOutputSitemap { get; init; }


    public PathService(IOptions<AppSettings> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(options.Value.OutputFolder);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(options.Value.RawFolder);

        //todo: check options if they are empty
        FolderWorking = options.Value.RawFolder;
        FolderOutput = options.Value.OutputFolder;

        FolderOutputMedia = Path.Combine(FolderOutput, "media");

        FileOutputSitemap = Path.Combine(FolderOutput, "sitemap.xml");

        FolderWorkingJustCopy = Path.Combine(FolderWorking, "justcopyme");
        FolderWorkingPosts = Path.Combine(FolderWorking, "posts");
        FolderWorkingStandalones = Path.Combine(FolderWorking, "standalones");

        FolderWorkingTemplates = Path.Combine(FolderWorking, "templates");

        FileWorkingTemplateBase = Path.Combine(FolderWorkingTemplates, "template.cshtml");
        FileWorkingTemplateArticle = Path.Combine(FolderWorkingTemplates, "template-article.cshtml");
        FileWorkingTemplateCardArticle = Path.Combine(FolderWorkingTemplates, "template-card-article.cshtml");
        FileWorkingTemplateCardImage = Path.Combine(FolderWorkingTemplates, "template-card-article.cshtml");
        FileWorkingTemplateCardSearch = Path.Combine(FolderWorkingTemplates, "template-card-search.cshtml");
        FileWorkingTemplateSitemap = Path.Combine(FolderWorkingTemplates, "teamplate-sitemap.cshtml");

    }

    public string GetWorkingPath(PageTypes pageType)
    {
        ArgumentNullException.ThrowIfNull(pageType);

        switch (pageType)
        {
            case PageTypes.MainPage:
                return FileWorkingTemplateBase;
            case PageTypes.Article:
                return FileWorkingTemplateArticle;
            case PageTypes.Standalone:
                return FolderWorkingStandalones;
            default:
                throw new Exception(pageType.ToString() + " not supported!");
        }
    }
}
