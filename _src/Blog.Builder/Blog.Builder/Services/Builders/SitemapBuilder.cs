using Blog.Builder.Models;
using RazorEngine.Templating;
using WebMarkupMin.Core;

namespace Blog.Builder.Services;

internal class SitemapBuilder : ISitemapBuilder
{
    private readonly IRazorEngineService _templateEngine;
    private readonly IPathService _pathService;
    private readonly ITemplateProvider _templateProvider;
    private static readonly LayoutSitemapModel sitemap = new LayoutSitemapModel();
    private readonly IMarkupMinifier _markupMinifier;

    private readonly object __lockObj = new object();

    public SitemapBuilder(
            IPathService pathService,
            IRazorEngineService templateService,
            ITemplateProvider templateProvider,
            IMarkupMinifier markupMinifier
            )
    {
        ArgumentNullException.ThrowIfNull(pathService);
        ArgumentNullException.ThrowIfNull(templateService);
        ArgumentNullException.ThrowIfNull(templateProvider);
        ArgumentNullException.ThrowIfNull(markupMinifier);

        _templateEngine = templateService;
        _pathService = pathService;
        _templateProvider = templateProvider;
        _markupMinifier = markupMinifier;
    }

    public void Build()
    {
        if(sitemap.Urls.Count() == 0)
        {
            throw new ArgumentException("Sitemap is empty", nameof(sitemap));
        }

        var sitemapPageHtml = _templateEngine.RunCompile(_templateProvider.Get<LayoutSitemapModel>(), 
                                                            nameof(SitemapBuilder), 
                                                            typeof(LayoutSitemapModel), 
                                                            sitemap);

        var result = _markupMinifier.Minify(sitemapPageHtml);

        File.WriteAllText(_pathService.OutputSitemap, result.MinifiedContent);
    }

    public void Add(string relativeUrl, DateTime dateModified)
    {
        ArgumentNullException.ThrowIfNull(relativeUrl);
        ArgumentNullException.ThrowIfNull(dateModified);

        lock (__lockObj)
        {
            sitemap.Add(relativeUrl, dateModified);
        }
    }


}
