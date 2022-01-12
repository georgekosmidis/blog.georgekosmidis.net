using Blog.Builder.Models;

namespace Blog.Builder.Services;

internal interface ISitemapBuilder
{
    void Add(string relaticeUrl, DateTime dateModified);
    void Build(Sitemap siteMap);
}