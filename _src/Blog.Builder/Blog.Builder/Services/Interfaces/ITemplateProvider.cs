namespace Blog.Builder.Services;

internal interface ITemplateProvider
{
    public string Get<T>();
}