using Blog.Builder.Models;

namespace Blog.Builder.Services;

internal interface IPageBuilder
{
    PageBuilderResult Build(string html, PageTypes pageType);
}