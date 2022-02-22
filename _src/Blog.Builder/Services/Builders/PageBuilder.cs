using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Interfaces.RazorEngineServices;
using Blog.Builder.Models.Builders;
using Blog.Builder.Models.Templates;

namespace Blog.Builder.Services.Builders;

/// <inheritdoc/>
internal class PageBuilder : IPageBuilder
{
    private readonly IRazorEngineWrapperService _templateEngine;
    private readonly ICardProcessor _cardPreparation;
    private readonly ILogger _logger;

    public PageBuilder(IRazorEngineWrapperService templateService, ICardProcessor cardPreparation, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(templateService);
        ArgumentNullException.ThrowIfNull(cardPreparation);

        _templateEngine = templateService;
        _cardPreparation = cardPreparation;
        _logger = logger;
    }

    /// <inheritdoc/>
    public string BuildInternalLayout<T>(T pageData, string bodyHtml, IEnumerable<string> rightColumnCards) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfNull(pageData);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(bodyHtml);

        pageData.RightColumnCards = rightColumnCards;
        pageData.Body = bodyHtml;
        pageData.TemplateDataModel = typeof(T).Name;
        var innerPartResult = Build(pageData);

        ExceptionHelpers.ThrowIfNull(pageData);

        _logger.Log($"Processing of {pageData.RelativeUrl} done!");
        return innerPartResult.FinalHtml;
    }

    /// <inheritdoc/>
    public string BuildInternalLayout<T>(T pageData, IEnumerable<string> bodyCards, IEnumerable<string> rightColumnCards) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfNull(pageData);
        ExceptionHelpers.ThrowIfNullOrEmpty(bodyCards);

        //First prepare the body of the page
        // (Right column is in the inner layouts, not the main layout)
        pageData.RightColumnCards = rightColumnCards;
        pageData.CardsHtml = bodyCards;
        pageData.TemplateDataModel = typeof(T).Name;
        var innerPartResult = Build(pageData);

        ExceptionHelpers.ThrowIfNull(pageData);

        _logger.Log($"Processing of {pageData.RelativeUrl}, page {pageData.Paging.CurrentPageIndex + 1} done!");
        return innerPartResult.FinalHtml;
    }

    /// <inheritdoc/>
    private PageBuilderResult Build<T>(T pageData) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfNull(pageData);
        pageData.Validate();

        var finalHtml = _templateEngine.RunCompile(pageData);

        return new PageBuilderResult
        {
            FinalHtml = finalHtml,
            DateModified = pageData.DateModified,
            RelativeUrl = pageData.RelativeUrl
        };
    }

    /// <inheritdoc/>
    public PageBuilderResult BuildMainLayout<T>(T pageData, string bodyHtml) where T : LayoutModelBase
    {
        pageData.Body = bodyHtml;
        var mainBuilderResult = Build(pageData as LayoutModelBase);

        //At the end, add a card for this article
        //todo: should that be here?
        if (pageData.TemplateDataModel == nameof(LayoutArticleModel))
        {
            var articleData = pageData as LayoutArticleModel;
            ExceptionHelpers.ThrowIfNull(articleData);

            var footer = articleData.DateModifiedText == articleData.DatePublishedText ? $"Published {articleData.DatePublishedText}" : $"Modified {articleData.DateModifiedText}";

            _cardPreparation.ProcessArticleCard(new CardArticleModel
            {
                TemplateDataModel = nameof(CardArticleModel),
                Title = articleData.Title,
                Description = articleData.Description,
                ImageUrl = articleData.RelativeImageUrl,
                Link = pageData.RelativeUrl,
                LinkTarget = "_top",
                IsSticky = false,
                Position = -1,
                Footer = footer,
            }, pageData.DatePublished);
        }

        return mainBuilderResult;
    }

    public void Dispose() => _templateEngine.Dispose();
}