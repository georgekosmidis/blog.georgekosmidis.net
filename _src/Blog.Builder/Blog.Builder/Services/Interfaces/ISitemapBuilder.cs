using Blog.Builder.Models;

namespace Blog.Builder.Services;

internal interface ISitemapBuilder
{
    void Add(string relativeUrl, DateTime dateModified);
    void Build();
}