namespace Blog.Builder.Services.Interfaces.Builders;

internal interface ISitemapBuilder
{
    void Add(string relativeUrl, DateTime dateModified);
    void Build();
}