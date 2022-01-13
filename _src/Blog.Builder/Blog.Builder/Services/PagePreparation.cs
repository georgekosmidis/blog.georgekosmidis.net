using Blog.Builder.Models;
using WebMarkupMin.Core;

namespace Blog.Builder.Services;

internal class PagePreparation : IPagePreparation
{
    private readonly IPathService _pathService;
    private readonly ISitemapBuilder _sitemapBuilder;
    private readonly IMarkupMinifier _markupMinifier;
    private readonly IMainTemplateBuilder _pageBuilder;

    public PagePreparation(IPathService pathService,
                        ISitemapBuilder sitemapBuilder,
                        IMarkupMinifier markupMinifier,
                        IMainTemplateBuilder pageBuilder)
    {
        ArgumentNullException.ThrowIfNull(pathService);
        ArgumentNullException.ThrowIfNull(sitemapBuilder);
        ArgumentNullException.ThrowIfNull(markupMinifier);
        ArgumentNullException.ThrowIfNull(pageBuilder);

        _pathService = pathService;
        _sitemapBuilder = sitemapBuilder;
        _markupMinifier = markupMinifier;
        _pageBuilder = pageBuilder;
    }

    public void Prepare(string path, PageTypes pageType)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(pageType);

        var builderResult = _pageBuilder.Build(path, pageType);
        var minifier = _markupMinifier.Minify(builderResult.FinalHtml);
        var savingPath = Path.Combine(_pathService.FolderOutput, Path.GetFileName(path));
        File.WriteAllText(savingPath, minifier.MinifiedContent);

        _sitemapBuilder.Add(builderResult.RelativeUrl, builderResult.DateModified);
    }
}
