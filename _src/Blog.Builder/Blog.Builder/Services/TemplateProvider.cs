using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces;
using Blog.Builder.Models.Templates;

namespace Blog.Builder.Services;

/// <inheritdoc/>
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
            { nameof(LayoutIndexModel), pathService.TempalteIndexFile },
            { nameof(LayoutModelBase), pathService.TemplateMainFile },
            { nameof(LayoutArticleModel), pathService.TemplateArticleFile },
            { nameof(LayoutStandaloneModel), pathService.TemplateStandaloneFile },
            { nameof(LayoutSitemapModel), pathService.TemplateSitemapFile },
            { nameof(CardArticleModel), pathService.TemplateCardArticleFile },
            { nameof(CardImageModel), pathService.TemplateCardImageFile },
            { nameof(CardSearchModel), pathService.TemplateCardSearchFile },
            { nameof(CardCalendarEventsModel), pathService.TemplateCardCalendarEventsFile },
        };

        foreach (var (model, template) in Templates)
        {
            ExceptionHelpers.ThrowIfNullOrWhiteSpace(template, model);
        }
    }

    /// <inheritdoc/>
    public string GetHtml<T>()
    {
        return File.ReadAllText(this.GetPath<T>());
    }

    /// <inheritdoc/>
    public string GetHtml(string nameOfType)
    {
        return File.ReadAllText(this.GetPath(nameOfType));
    }

    /// <inheritdoc/>
    public string GetPath<T>()
    {
        var path = Templates.First(x => x.Key == typeof(T).Name);
        ExceptionHelpers.ThrowIfPathNotExists(path.Value);
        return path.Value;
    }

    /// <inheritdoc/>
    public string GetPath(string nameOfType)
    {
        var path = Templates.First(x => x.Key == nameOfType);
        ExceptionHelpers.ThrowIfPathNotExists(path.Value);
        return path.Value;
    }
}
