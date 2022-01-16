using Blog.Builder.Models.Templates;

namespace Blog.Builder.Services.Interfaces;

internal interface IPagePreparation
{
    void PreparePage<T>(string directory) where T : LayoutModelBase;
    void PrepareIndex(LayoutIndexModel pageData, int cardsPerPage);
}