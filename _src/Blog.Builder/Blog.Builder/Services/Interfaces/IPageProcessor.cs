using Blog.Builder.Models.Templates;

namespace Blog.Builder.Services.Interfaces;

internal interface IPageProcessor
{
    void ProcessPage<T>(string directory) where T : LayoutModelBase;
    void ProcessIndex(LayoutIndexModel pageData, int cardsPerPage);
}