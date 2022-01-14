using Blog.Builder.Models;

namespace Blog.Builder.Services;

internal interface IMainTemplateBuilder
{
    PageBuilderResult Build<T>(string directory) where T : ModelBase;
}