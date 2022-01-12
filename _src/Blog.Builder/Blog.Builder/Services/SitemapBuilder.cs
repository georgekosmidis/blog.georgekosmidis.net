using Blog.Builder.Models;
using Microsoft.Extensions.Options;
using RazorEngine.Templating;
using WebMarkupMin.Core;

namespace Blog.Builder.Services;

internal class SitemapBuilder : ISitemapBuilder
{
    private readonly IRazorEngineService _templateEngine;
    private readonly AppSettings _appSettings;
    private readonly ITemplateProvider _templateProvider;
    private readonly Sitemap sitemap = new Sitemap();
    private readonly IMarkupMinifier _markupMinifier;

    public SitemapBuilder(
            IOptions<AppSettings> options, 
            IRazorEngineService templateService, 
            ITemplateProvider templateProvider,
            IMarkupMinifier markupMinifier
            )
    {
        _templateEngine = templateService;
        _appSettings = options.Value;
        _templateProvider = templateProvider;
        _markupMinifier = markupMinifier;
    }

    public void Build(Sitemap siteMap)
    {
        var sitemapPageHtml = _templateEngine.RunCompile(_templateProvider.SitemapTemplate, nameof(SitemapBuilder), typeof(Sitemap), siteMap);

        var result = _markupMinifier.Minify(sitemapPageHtml);

        File.WriteAllText(Path.Combine(_appSettings.OutputFolder, "sitemap.xml"), result.MinifiedContent);
    }

    public void Add(string relaticeUrl, DateTime dateModified)
    {
        sitemap.Add(relaticeUrl, dateModified);
    }


}
