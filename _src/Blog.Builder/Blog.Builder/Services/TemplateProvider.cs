using Blog.Builder.Exceptions;
using Blog.Builder.Models;
using Microsoft.Extensions.Options;

namespace Blog.Builder.Services;

internal class TemplateProvider : ITemplateProvider
{
    private Dictionary<string, string> Templates = new Dictionary<string, string>();

    public TemplateProvider(IPathService pathService)
    {
        ArgumentNullException.ThrowIfNull(pathService);

        Templates = new Dictionary<string, string>()
        {
            { nameof(ModelBase), File.ReadAllText(pathService.TemplateMain) },
            { nameof(ArticleModel), File.ReadAllText(pathService.TemplateArticle) },
            { nameof(StandaloneModel), File.ReadAllText(pathService.TemplateStandalone) },
            { nameof(SitemapModel), File.ReadAllText(pathService.TemplateSitemap) },
            { nameof(ModelBase), File.ReadAllText(pathService.TemplateCardArticle) },
            { nameof(ModelBase), File.ReadAllText(pathService.TemplateCardImage) },
            { nameof(ModelBase), File.ReadAllText(pathService.TemplateCardSearch) },
        };

        foreach (var (model, template) in Templates)
        {
            ExceptionHelpers.ThrowIfNullOrWhiteSpace(template, model);
        }
    }

    public string Get<T>()
    {
        var template = Templates.First( x => x.Key == typeof(T).Name);
        return template.Value;
    }
}
