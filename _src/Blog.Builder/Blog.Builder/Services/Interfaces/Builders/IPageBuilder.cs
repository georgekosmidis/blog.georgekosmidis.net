using Blog.Builder.Models.Templates;
using Blog.Builder.Services.Builders;

namespace Blog.Builder.Services.Interfaces.Builders;

internal interface IPageBuilder
{
    PageBuilderResult Build<T>(string directory) where T : LayoutModelBase;
    PageBuilderResult Build<T>(T pageData) where T : LayoutModelBase;
}