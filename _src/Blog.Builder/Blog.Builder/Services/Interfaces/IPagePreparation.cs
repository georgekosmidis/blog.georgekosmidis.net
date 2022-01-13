using Blog.Builder.Models;

namespace Blog.Builder.Services;

internal interface IPagePreparation
{
    void Prepare(string path, PageTypes pageType);
}