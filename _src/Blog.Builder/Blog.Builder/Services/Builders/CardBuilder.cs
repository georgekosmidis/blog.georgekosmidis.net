using Blog.Builder.Exceptions;
using Blog.Builder.Models.Templates;
using Blog.Builder.Services.Interfaces;
using Blog.Builder.Services.Interfaces.Builders;
using Newtonsoft.Json;
using RazorEngine.Templating;
using WebMarkupMin.Core;

namespace Blog.Builder.Services.Builders;

internal record class ArticleCardBuilderResult
{
    public string CardHtml { get; init; } = string.Empty;
    public DateTime DateCreated { get; init; }
}
internal record class OtherCardBuilderResult
{
    public string CardHtml { get; init; } = string.Empty;
    public int Position { get; init; }
    public bool IsSticky { get; init; }
}

internal class CardBuilder : ICardBuilder
{
    private readonly IRazorEngineService _templateEngine;
    private readonly ITemplateProvider _templateProvider;
    private static readonly List<ArticleCardBuilderResult> ArticleCards = new List<ArticleCardBuilderResult>();
    private static readonly List<OtherCardBuilderResult> OtherCards = new List<OtherCardBuilderResult>();

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


    private string CreateCardHtml<T>(T cardData) where T : CardModelBase
    {
        ArgumentNullException.ThrowIfNull(cardData);
        cardData.Validate();

        return _templateEngine.RunCompile(
                                        _templateProvider.Get<T>(),
                                        Guid.NewGuid().ToString(),
                                        typeof(T),
                                        cardData);

    }
    public ICardBuilder AddCard(string directory)
    {
        ExceptionHelpers.ThrowIfPathNotExists(directory);

        var jsonFileContent = File.ReadAllText(Path.Combine(directory, "card.json"));
        var cardDataBase = JsonConvert.DeserializeObject<CardModelBase>(jsonFileContent);
        ExceptionHelpers.ThrowIfNull(cardDataBase);
        cardDataBase.Validate();

        //is there a way to load the type directly like: 
        // var cardData = cardDataBase as >>cardDataBase.CardType.Name<<;
        string cardHtml = cardDataBase.TemplateDataModel switch
        {
            nameof(CardSearchModel) => CreateCardHtml(CardSearchModel.FromBase(cardDataBase)),
            nameof(CardImageModel) => CreateCardHtml(CardImageModel.FromBase(cardDataBase)),
            nameof(CardArticleModel) => throw new Exception($"{AddCard} cannot be used with {nameof(CardArticleModel)}, use {AddArticleCard} instead."),
            _ => throw new Exception($"{cardDataBase.TemplateDataModel} switch is missing."),
        };
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(cardHtml);

        lock (__lockObj)
        {
            OtherCards.Add(new OtherCardBuilderResult
            {
                CardHtml = cardHtml,
                Position = cardDataBase.Position,
                IsSticky = cardDataBase.IsSticky
            });
        }

        return this;

    }
    public ICardBuilder AddArticleCard(CardArticleModel cardData, DateTime dateCreated)
    {
        var cardHtml = CreateCardHtml(cardData);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(cardHtml);

        lock (__lockObj)
        {
            ArticleCards.Add(new ArticleCardBuilderResult
            {
                CardHtml = cardHtml,
                DateCreated = dateCreated
            });
        }

        return this;
    }


    public string GetHtml(int page, int perPage)
    {
        var cards = ArticleCards.OrderByDescending(x => x.DateCreated)
                                .Select(x => x.CardHtml)
                                .ToList();

        foreach (var card in OtherCards.Select(x => (x.IsSticky ? x.Position + page : x.Position, x.CardHtml)))
        {
            cards.Insert(card.Item1, card.CardHtml);
        }

        return string.Join(string.Empty, cards.Skip(page * perPage).Take(perPage).ToArray());
    }
}
