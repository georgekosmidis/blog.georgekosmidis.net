using Blog.Builder.Models;

namespace Blog.Builder.Services;

internal interface IMainTemplateBuilder
{
    MainTemplateBuilderResult Build(string html, PageTypes pageType);
}