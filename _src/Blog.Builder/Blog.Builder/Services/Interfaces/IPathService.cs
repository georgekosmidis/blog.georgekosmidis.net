namespace Blog.Builder.Services;

internal interface IPathService
{
    string TemplateArticle { get; init; }
    string TemplateMain { get; init; }
    string TemplateStandalone { get; init; }
    string TemplateCardArticle { get; init; }
    string TemplateCardImage { get; init; }
    string TemplateCardSearch { get; init; }
    string TemplateSitemap { get; init; }
    string OutputFolder { get; init; }
    string OutputMediaFolder { get; init; }
    string OutputSitemap { get; init; }
    string WorkingFolder { get; init; }
    string JustCopyFolder { get; init; }
    string ArticlesFolder { get; init; }
    string StandalonesFolder { get; init; }
    string TemplatesFolder { get; init; }
    string CardsFolder { get; init; }

}