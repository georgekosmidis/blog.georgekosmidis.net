using Blog.Builder.Models;
using Newtonsoft.Json;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMarkupMin.Core;

namespace Blog.Builder.Services;

internal class PageOrganiser
{
    private readonly IRazorEngineService _templateEngine;
    private readonly IPathService _pathService;
    private readonly ITemplateProvider _templateProvider;
    private readonly ISitemapBuilder _sitemapBuilder;
    private readonly IMarkupMinifier _markupMinifier;
    private readonly IPageBuilder _pageBuilder;

    public PageOrganiser(IPathService pathService,
                        IRazorEngineService templateService,
                        ITemplateProvider templateProvider,
                        ISitemapBuilder sitemapBuilder,
                        IMarkupMinifier markupMinifier,
                        IPageBuilder pageBuilder)
    {
        _templateEngine = templateService;
        _pathService = pathService;
        _templateProvider = templateProvider;
        _sitemapBuilder = sitemapBuilder;
        _markupMinifier = markupMinifier;
        _pageBuilder = pageBuilder;
    }

    public void Organise(string path, PageTypes pageType)
    {

        var builderResult = _pageBuilder.Build(path, pageType);

        var minifier = _markupMinifier.Minify(builderResult.FinalHtml);

        var savingPath = Path.Combine(_pathService.FolderOutput, Path.GetFileName(path));

        File.WriteAllText(savingPath, minifier.MinifiedContent);

        _sitemapBuilder.Add(builderResult.RelativeUrl, builderResult.DateModified);
    }
}
