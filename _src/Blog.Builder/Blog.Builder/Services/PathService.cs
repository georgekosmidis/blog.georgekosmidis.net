using Blog.Builder.Exceptions;
using Blog.Builder.Models;
using Blog.Builder.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Blog.Builder.Services;

//todo: pass all of this to appsettings, no reason to be hardocoded
internal class PathService : IPathService
{
    public string WorkingFolder { get; init; }

    public string TemplatesFolder { get; init; }
    public string JustCopyFolder { get; init; }
    public string ArticlesFolder { get; init; }
    public string StandalonesFolder { get; init; }
    public string CardsFolder { get; init; }


    public string TemplateMain { get; init; }
    public string TemplateArticle { get; init; }
    public string TemplateSitemap { get; init; }
    public string TemplateStandalone { get; init; }

    public string TemplateCardArticle { get; init; }
    public string TemplateCardImage { get; init; }
    public string TemplateCardSearch { get; init; }


    public string OutputFolder { get; init; }
    public string OutputMediaFolder { get; init; }
    public string OutputSitemap { get; init; }

    public PathService(IOptions<AppSettings> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(options.Value.OutputFolder);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(options.Value.RawFolder);

        WorkingFolder = options.Value.RawFolder;

        OutputFolder = options.Value.OutputFolder;
        OutputMediaFolder = Path.Combine(OutputFolder, "media");

        OutputSitemap = Path.Combine(OutputFolder, "sitemap.xml");

        JustCopyFolder = Path.Combine(WorkingFolder, "justcopyme");
        ArticlesFolder = Path.Combine(WorkingFolder, "articles");
        StandalonesFolder = Path.Combine(WorkingFolder, "standalones");
        CardsFolder = Path.Combine(WorkingFolder, "cards");

        TemplatesFolder = Path.Combine(WorkingFolder, "templates");

        TemplateMain = Path.Combine(TemplatesFolder, "template-layout.cshtml");
        TemplateArticle = Path.Combine(TemplatesFolder, "template-article.cshtml");
        TemplateStandalone = Path.Combine(TemplatesFolder, "template-standalone.cshtml");
        TemplateSitemap = Path.Combine(TemplatesFolder, "teamplate-sitemap.cshtml");

        TemplateCardArticle = Path.Combine(TemplatesFolder, "template-card-article.cshtml");
        TemplateCardImage = Path.Combine(TemplatesFolder, "template-card-image.cshtml");
        TemplateCardSearch = Path.Combine(TemplatesFolder, "template-card-search.cshtml");

    }

}
