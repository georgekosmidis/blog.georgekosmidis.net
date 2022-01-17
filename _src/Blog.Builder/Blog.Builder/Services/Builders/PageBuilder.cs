using Blog.Builder.Exceptions;
using Blog.Builder.Models.Builders;
using Blog.Builder.Models.Templates;
using Blog.Builder.Services.Interfaces;
using Blog.Builder.Services.Interfaces.Builders;
using Newtonsoft.Json;
using RazorEngine.Templating;

namespace Blog.Builder.Services.Builders;

/// <inheritdoc/>
internal class PageBuilder : IPageBuilder
{
    private readonly IRazorEngineService _templateEngine;
    private readonly ITemplateProvider _templateProvider;
    private readonly ICardProcessor _cardPreparation;

    public PageBuilder(IRazorEngineService templateService, ITemplateProvider templateProvider, ICardProcessor cardPreparation)
    {
        ArgumentNullException.ThrowIfNull(templateService);
        ArgumentNullException.ThrowIfNull(templateProvider);
        ArgumentNullException.ThrowIfNull(cardPreparation);

        _templateEngine = templateService;
        _templateProvider = templateProvider;
        _cardPreparation = cardPreparation;

    }

    /// <inheritdoc/>
    public PageBuilderResult Build<T>(T pageData) where T : LayoutModelBase
    {
        ArgumentNullException.ThrowIfNull(pageData);
        pageData.Validate();

        var finalHtml = _templateEngine.RunCompile(
                                                _templateProvider.Get<T>(),
                                                Guid.NewGuid().ToString(),
                                                typeof(T),
                                                pageData);

        return new PageBuilderResult
        {
            FinalHtml = finalHtml,
            DateModified = pageData.DateModified,
            RelativeUrl = pageData.RelativeUrl
        };
    }

    /// <inheritdoc/>
    public PageBuilderResult Build<T>(string directory) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfPathNotExists(directory);

        var jsonFileContent = File.ReadAllText(Path.Combine(directory, "content.json"));
        var pageData = JsonConvert.DeserializeObject<T>(jsonFileContent);
        ExceptionHelpers.ThrowIfNull(pageData);

        //First prepare the body of the page
        pageData.Body = File.ReadAllText(Path.Combine(directory, "content.html"));
        pageData.TemplateDataModel = typeof(T).Name;
        var innerPartHtml = Build(pageData);

        //Then prepare the entire page
        pageData.Body = innerPartHtml.FinalHtml;
        var completePageHtml = Build(pageData as LayoutModelBase);

        //At the end, add a card for this article
        if (pageData.TemplateDataModel == nameof(LayoutArticleModel))
        {
            _cardPreparation.ProcessArticleCard(new CardArticleModel
            {
                TemplateDataModel = nameof(CardArticleModel),
                Title = pageData.Title,
                Description = pageData.Description,
                ImageUrl = pageData.RelativeImageUrl,
                Link = pageData.RelativeUrl,
                LinkTarget = "_top",
                IsSticky = false,
                Position = -1,
                Footer = (pageData as LayoutArticleModel)?.DatePublishedOrModificationText ?? string.Empty,
            }, pageData.DatePublished);
        }

        return completePageHtml;
    }

}