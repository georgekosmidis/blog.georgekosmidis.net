using Blog.Builder.Exceptions;
using Blog.Builder.Models.Templates;
using Blog.Builder.Services.Interfaces;

namespace Blog.Builder.Services;

/// <summary>
/// A service that provides the html for the templates.
/// </summary>
internal class TemplateProvider : ITemplateProvider
{
    /// <summary>
    /// A dictionary with key the name of a tempalte model (e.g. <see cref="LayoutArticleModel"/>)
    /// and value the html of proper template. 
    /// </summary>
    private Dictionary<string, string> Templates = new Dictionary<string, string>();

    /// <summary>
    /// Besides DI, it registers all template htmls in a dictionary with key being the name of the model for that dictionary.
    /// </summary>
    /// <param name="pathService">The service that build and checks all the paths.</param>
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

    /// <summary>
    /// Returns the proper template HTML based on the model <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The template model to get the equivalent HTML.</typeparam>
    /// <returns>The HTML of the proper template</returns>
    public string Get<T>()
    {
        var template = Templates.First(x => x.Key == typeof(T).Name);
        return template.Value;
    }
}
