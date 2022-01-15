using Blog.Builder.Models;

namespace Blog.Builder.Services;

internal interface IPageBuilder
{
    PageBuilderResult Build<T>(string directory) where T : LayoutModelBase;
}