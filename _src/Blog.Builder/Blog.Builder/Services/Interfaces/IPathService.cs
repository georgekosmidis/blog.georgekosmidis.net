namespace Blog.Builder.Services.Interfaces;

internal interface IPathService
{
    string TemplateArticleFile { get; init; }
    string TemplateMainFile { get; init; }
    string TemplateStandaloneFile { get; init; }
    string TemplateCardArticleFile { get; init; }
    string TemplateCardImageFile { get; init; }
    string TemplateCardSearchFile { get; init; }
    string TemplateSitemapFile { get; init; }
    string OutputFolder { get; init; }
    string OutputMediaFolder { get; init; }
    string OutputSitemapFile { get; init; }
    string WorkingFolder { get; init; }
    string WorkingJustCopyFolder { get; init; }
    string WorkingArticlesFolder { get; init; }
    string WorkingStandalonesFolder { get; init; }
    string WorkingTemplatesFolder { get; init; }
    string WorkingCardsFolder { get; init; }

}