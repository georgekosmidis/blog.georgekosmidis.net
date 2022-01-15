using Blog.Builder.Models.Templates;

namespace Blog.Builder.Services.Interfaces;

internal interface ICardPreparation
{
    void RegisterCard(string directory);
    void RegisterArticleCard(CardArticleModel cardData, DateTime dateCreated);
}