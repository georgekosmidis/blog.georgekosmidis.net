using Blog.Builder.Models.Templates;

namespace Blog.Builder.Services.Interfaces.Builders;

internal interface ICardBuilder
{
    ICardBuilder AddArticleCard(CardArticleModel cardData, DateTime dateCreated);
    ICardBuilder AddCard(string directory);
    string GetHtml(int page, int perPage);
}
