using Blog.Builder.Exceptions;
using Blog.Builder.Models.Templates;
using Blog.Builder.Services.Interfaces;
using Blog.Builder.Services.Interfaces.Builders;
using RazorEngine.Templating;
using WebMarkupMin.Core;

namespace Blog.Builder.Services.Builders;

/// <inheritdoc/>
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

    /// <inheritdoc/>
    public void Build()
    {
        ExceptionHelpers.ThrowIfNullOrEmpty(sitemap.Urls);

        var sitemapPageHtml = _templateEngine.RunCompile(_templateProvider.Get<LayoutSitemapModel>(),
                                                            nameof(SitemapBuilder),
                                                            typeof(LayoutSitemapModel),
                                                            sitemap);

        var result = _markupMinifier.Minify(sitemapPageHtml);
        if (result.Errors.Count() > 0)
        {
            throw new Exception($"Minification failed with at least one error:" +
                $"{Environment.NewLine}{result.Errors.First().Message}" +
                $"{Environment.NewLine}{result.Errors.First().SourceFragment}");
        }
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(result.MinifiedContent);

        File.WriteAllText(_pathService.OutputSitemapFile, result.MinifiedContent);
    }

    /// <inheritdoc/>
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
