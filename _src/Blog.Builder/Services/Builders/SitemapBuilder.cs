using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Interfaces.RazorEngineServices;
using Blog.Builder.Models.Templates;

namespace Blog.Builder.Services.Builders;

/// <inheritdoc/>
internal class SitemapBuilder : ISitemapBuilder
{
    private readonly IRazorEngineWrapperService _templateEngine;
    private static readonly LayoutSitemapModel sitemapModel = new();

    public SitemapBuilder(IRazorEngineWrapperService templateService)
    {
        ArgumentNullException.ThrowIfNull(templateService);

        _templateEngine = templateService;
    }

    /// <inheritdoc/>
    public void Build()
    {
        ExceptionHelpers.ThrowIfNullOrEmpty(sitemapModel.Urls);

        var sitemapPageXml = _templateEngine.RunCompile(sitemapModel);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(sitemapPageXml);

        var sitemap = Path.Combine(Consts.OutputFolderPath, Consts.GoogleSitemap);
        File.WriteAllText(sitemap, sitemapPageXml);
    }

    /// <inheritdoc/>
    public void Add(string relativeUrl, DateTime dateModified)
    {
        ArgumentNullException.ThrowIfNull(relativeUrl);
        ArgumentNullException.ThrowIfNull(dateModified);

        sitemapModel.Add(relativeUrl, dateModified);
    }

    public void Dispose()
    {
        _templateEngine.Dispose();
    }
}
