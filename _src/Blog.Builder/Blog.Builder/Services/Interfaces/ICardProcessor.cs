using Blog.Builder.Models.Templates;

namespace Blog.Builder.Services.Interfaces;

internal interface ICardProcessor
{
    void ProcessCard(string directory);
    void ProcessArticleCard(CardArticleModel cardData, DateTime dateCreated);
    int GetCardsNumber();
    string GetHtml(int pageIndex, int perPage);
}