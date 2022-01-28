using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces;
using Blog.Builder.Models;
using Blog.Builder.Models.Templates;
using Microsoft.Extensions.Options;

namespace Blog.Builder.Services;

/// <inheritdoc/>
internal class TemplateProvider : ITemplateProvider
{
    /// <summary>
    /// A dictionary with key the name of a tempalte model (e.g. <see cref="LayoutArticleModel"/>)
    /// and value the html of proper template. 
    /// </summary>
    private readonly Dictionary<string, string> Templates = new();

    /// <summary>
    /// Besides DI, it registers all template htmls in a dictionary with key being the name of the model for that dictionary.
    /// </summary>
    /// <param name="options">The options that hold the app settings.</param>
    public TemplateProvider(IOptions<AppSettings> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var appSettings = options.Value;
        var workingTemplatesFolder = Path.Combine(Consts.WorkingFolderPath, Consts.WorkingTemplatesFolderName);

        Templates = new Dictionary<string, string>()
        {
            { nameof(LayoutIndexModel), Path.Combine(workingTemplatesFolder, Consts.TemplateIndexFilename) },
            { nameof(LayoutModelBase), Path.Combine(workingTemplatesFolder, Consts.TemplateMainFilename) },
            { nameof(LayoutArticleModel), Path.Combine(workingTemplatesFolder, Consts.TemplateArticleFilename) },
            { nameof(LayoutStandaloneModel), Path.Combine(workingTemplatesFolder, Consts.TemplateStandaloneFilename) },
            { nameof(LayoutSitemapModel), Path.Combine(workingTemplatesFolder, Consts.TemplateSitemapFilename) },
            { nameof(CardArticleModel), Path.Combine(workingTemplatesFolder, Consts.TemplateCardArticleFilename) },
            { nameof(CardImageModel), Path.Combine(workingTemplatesFolder, Consts.TemplateCardImageFilename) },
            { nameof(CardSearchModel),Path.Combine(workingTemplatesFolder, Consts.TemplateCardSearchFilename) },
            { nameof(CardCalendarEventsModel), Path.Combine(workingTemplatesFolder, Consts.TemplateCardCalendarEventsFilename) },
        };

        foreach (var (model, path) in Templates)
        {
            ExceptionHelpers.ThrowIfPathNotExists(path, model);
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
        return path.Value;
    }

    /// <inheritdoc/>
    public string GetPath(string nameOfType)
    {
        var path = Templates.First(x => x.Key == nameOfType);
        return path.Value;
    }
}
