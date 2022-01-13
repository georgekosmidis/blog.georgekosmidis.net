using Blog.Builder.Models;

namespace Blog.Builder.Services;

internal interface IPathService
{
    string FileWorkingTemplateArticle { get; init; }
    string FileWorkingTemplateBase { get; init; }
    string FileWorkingTemplateCardArticle { get; init; }
    string FileWorkingTemplateCardImage { get; init; }
    string FileWorkingTemplateCardSearch { get; init; }
    string FileWorkingTemplateSitemap { get; init; }
    string FolderOutput { get; init; }
    string FolderOutputMedia { get; init; }
    string FileOutputSitemap { get; init; }
    string FolderWorking { get; init; }
    string FolderWorkingJustCopy { get; init; }
    string FolderWorkingPosts { get; init; }
    string FolderWorkingStandalones { get; init; }
    string FolderWorkingTemplates { get; init; }

    string GetWorkingPath(PageTypes pageType);
}