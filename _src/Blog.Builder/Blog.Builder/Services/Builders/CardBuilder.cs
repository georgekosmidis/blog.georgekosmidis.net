using Blog.Builder.Models;
using RazorEngine.Templating;
using WebMarkupMin.Core;

namespace Blog.Builder.Services;

internal class CardBuilder 
{
    private readonly IRazorEngineService _templateEngine;
    private readonly IPathService _pathService;
    private readonly ITemplateProvider _templateProvider;
    private static readonly Sitemap sitemap = new Sitemap();
    private readonly IMarkupMinifier _markupMinifier;

    private readonly object __lockObj = new object();

    public CardBuilder(
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
        //ArgumentNullException.ThrowIfNull(relativeUrl);
        //ArgumentNullException.ThrowIfNull(dateModified);

        var cardHtml = _templateEngine.RunCompile(cardArticleTemplate, "card:" + directory, typeof(ArticleTemplateData), articleData);

        var sitemapPageHtml = _templateEngine.RunCompile(_templateProvider.SitemapTemplate, nameof(SitemapBuilder), typeof(Sitemap), sitemap);

        var result = _markupMinifier.Minify(sitemapPageHtml);

        File.WriteAllText(_pathService.FileOutputSitemap, result.MinifiedContent);
    }

   


}
