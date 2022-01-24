using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces;
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
    private static readonly LayoutSitemapModel sitemapModel = new LayoutSitemapModel();
    private readonly IMarkupMinifier _markupMinifier;
    private readonly AppSettings appSettings;

    private readonly object __lockObj = new object();

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

        var sitemapPageHtml = _templateEngine.RunCompile(sitemapModel);

        var result = _markupMinifier.Minify(sitemapPageHtml);
        if (result.Errors.Count() > 0)
        {
            throw new Exception($"Minification failed with at least one error:" +
                $"{Environment.NewLine}{result.Errors.First().Message}" +
                $"{Environment.NewLine}{result.Errors.First().SourceFragment}");
        }
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(result.MinifiedContent);

        var sitemap = Path.Combine(appSettings.OutputFolderPath, "sitemap.xml");
        File.WriteAllText(sitemap, result.MinifiedContent);
    }

    /// <inheritdoc/>
    public void Add(string relativeUrl, DateTime dateModified)
    {
        ArgumentNullException.ThrowIfNull(relativeUrl);
        ArgumentNullException.ThrowIfNull(dateModified);

        lock (__lockObj)
        {
            sitemapModel.Add(relativeUrl, dateModified);
        }
    }
}
