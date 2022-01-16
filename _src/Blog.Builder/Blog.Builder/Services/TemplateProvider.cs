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
            { nameof(LayoutIndexModel), File.ReadAllText(pathService.TemplateMainFile) },
            { nameof(LayoutModelBase), File.ReadAllText(pathService.TemplateMainFile) },
            { nameof(LayoutArticleModel), File.ReadAllText(pathService.TemplateArticleFile) },
            { nameof(LayoutStandaloneModel), File.ReadAllText(pathService.TemplateStandaloneFile) },
            { nameof(LayoutSitemapModel), File.ReadAllText(pathService.TemplateSitemapFile) },
            { nameof(CardArticleModel), File.ReadAllText(pathService.TemplateCardArticleFile) },
            { nameof(CardImageModel), File.ReadAllText(pathService.TemplateCardImageFile) },
            { nameof(CardSearchModel), File.ReadAllText(pathService.TemplateCardSearchFile) },
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
