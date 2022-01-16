using Blog.Builder.Exceptions;
using Blog.Builder.Models.Templates;
using Blog.Builder.Services.Interfaces;

namespace Blog.Builder.Services;

internal class TemplateProvider : ITemplateProvider
{
    private Dictionary<string, string> Templates = new Dictionary<string, string>();

    public TemplateProvider(IPathService pathService)
    {
        ArgumentNullException.ThrowIfNull(pathService);

        Templates = new Dictionary<string, string>()
        {
            { nameof(LayoutIndexModel), File.ReadAllText(pathService.TemplateMain) },
            { nameof(LayoutModelBase), File.ReadAllText(pathService.TemplateMain) },
            { nameof(LayoutArticleModel), File.ReadAllText(pathService.TemplateArticle) },
            { nameof(LayoutStandaloneModel), File.ReadAllText(pathService.TemplateStandalone) },
            { nameof(LayoutSitemapModel), File.ReadAllText(pathService.TemplateSitemap) },
            { nameof(CardArticleModel), File.ReadAllText(pathService.TemplateCardArticle) },
            { nameof(CardImageModel), File.ReadAllText(pathService.TemplateCardImage) },
            { nameof(CardSearchModel), File.ReadAllText(pathService.TemplateCardSearch) },
        };

        foreach (var (model, template) in Templates)
        {
            ExceptionHelpers.ThrowIfNullOrWhiteSpace(template, model);
        }
    }

    public string Get<T>()
    {
        var template = Templates.First(x => x.Key == typeof(T).Name);
        return template.Value;
    }
}
