using Blog.Builder.Models;
using Blog.Builder.Models.Templates;
using RazorEngine.Templating;
using WebMarkupMin.Core;

namespace Blog.Builder.Services;

internal record class CardBuilderResult
{
    public string FinalHtml { get; init; } = String.Empty;
    public int Position { get; init; }
    public bool IsSticky { get; init; }

}
internal class CardBuilder
{
    private readonly IRazorEngineService _templateEngine;
    private readonly ITemplateProvider _templateProvider;
    private static readonly List<CardBuilderResult> Cards = new List<CardBuilderResult>();

    private readonly object __lockObj = new object();

    public CardBuilder(
            IRazorEngineService templateEngine,
            ITemplateProvider templateProvider,
            IMarkupMinifier markupMinifier
            )
    {
        ArgumentNullException.ThrowIfNull(templateEngine);
        ArgumentNullException.ThrowIfNull(templateProvider);
        ArgumentNullException.ThrowIfNull(markupMinifier);

        _templateEngine = templateEngine;
        _templateProvider = templateProvider;
    }

    private CardBuilder Add<T>(T cardData, int position, bool isSticky) where T : CardModelBase
    {
        ArgumentNullException.ThrowIfNull(cardData);
        cardData.Validate();

        var finalHtml = _templateEngine.RunCompile(
                                                _templateProvider.Get<T>(),
                                                Guid.NewGuid().ToString(),
                                                typeof(T),
                                                cardData);
        lock (__lockObj)
        {
            Cards.Add(new CardBuilderResult
            {
                FinalHtml = finalHtml,
                Position = position,
                IsSticky = isSticky
            });
        }

        return this;
    }


    public string GetHtml(int page, int perPage)
    {
        var result = Cards.OrderBy(x => x.IsSticky ? x.Position + page : x.Position).Take(perPage);
        return string.Join(string.Empty, result.Select(x => x.FinalHtml).ToArray());
    }
}
