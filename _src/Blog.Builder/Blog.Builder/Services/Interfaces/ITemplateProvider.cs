namespace Blog.Builder.Services.Interfaces;

internal interface ITemplateProvider
{
    public string Get<T>();
}