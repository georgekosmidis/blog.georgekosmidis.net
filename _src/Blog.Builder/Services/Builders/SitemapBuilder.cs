using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Interfaces.RazorEngineServices;
using Blog.Builder.Models;
using Blog.Builder.Models.Templates;
using Microsoft.Extensions.Options;
using WebMarkupMin.Core;

namespace Blog.Builder.Services.Builders;

/// <inheritdoc/>
internal class SitemapBuilder : ISitemapBuilder
{
    private readonly IRazorEngineWrapperService _templateEngine;
    private static readonly LayoutSitemapModel sitemapModel = new();
    private readonly IMarkupMinifier _markupMinifier;
    private readonly AppSettings appSettings;

    public SitemapBuilder(
            IRazorEngineWrapperService templateService,
            IMarkupMinifier markupMinifier,
            IOptions<AppSettings> options
            )
    {
        ArgumentNullException.ThrowIfNull(templateService);
        ArgumentNullException.ThrowIfNull(markupMinifier);
        ArgumentNullException.ThrowIfNull(options);

        _templateEngine = templateService;
        _markupMinifier = markupMinifier;
        appSettings = options.Value;
    }

    /// <inheritdoc/>
    public void Build()
    {
        ExceptionHelpers.ThrowIfNullOrEmpty(sitemapModel.Urls);

        var sitemapPageXml = _templateEngine.RunCompile(sitemapModel);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(sitemapPageXml);

        //minification as it is, removes quotes and renders the sitemap invalid.
        //var result = _markupMinifier.Minify(sitemapPageHtml);
        //if (result.Errors.Count > 0)
        //{
        //    throw new Exception($"Minification failed with at least one error:" +
        //        $"{Environment.NewLine}{result.Errors.First().Message}" +
        //        $"{Environment.NewLine}{result.Errors.First().SourceFragment}");
        //}
        //ExceptionHelpers.ThrowIfNullOrWhiteSpace(result.MinifiedContent);

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
