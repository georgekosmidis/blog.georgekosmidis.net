using Blog.Builder.Models.Templates;

namespace Blog.Builder.Services.Interfaces.Builders;

internal interface ICardBuilder
{
    void AddArticleCard(CardArticleModel cardData, DateTime dateCreated);
    void AddCard(string directory);
    string GetHtml(int pageIndex, int cardsPerPage);

    int GetCardsNumber(int cardsPerPage);
}
