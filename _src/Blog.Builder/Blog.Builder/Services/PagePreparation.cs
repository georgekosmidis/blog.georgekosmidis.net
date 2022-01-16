using Blog.Builder.Models.Templates;
using Blog.Builder.Services.Interfaces;
using Blog.Builder.Services.Interfaces.Builders;
using SixLabors.ImageSharp;
using WebMarkupMin.Core;

namespace Blog.Builder.Services;

internal class PagePreparation : IPagePreparation
{
    private readonly IPathService _pathService;
    private readonly ISitemapBuilder _sitemapBuilder;
    private readonly IMarkupMinifier _markupMinifier;
    private readonly IPageBuilder _pageBuilder;
    private readonly ICardPreparation _cardPreparation;

    public PagePreparation(IPathService pathService,
                        ISitemapBuilder sitemapBuilder,
                        IMarkupMinifier markupMinifier,
                        IPageBuilder pageBuilder,
                        ICardPreparation cardPreparation)
    {
        ArgumentNullException.ThrowIfNull(pathService);
        ArgumentNullException.ThrowIfNull(sitemapBuilder);
        ArgumentNullException.ThrowIfNull(markupMinifier);
        ArgumentNullException.ThrowIfNull(pageBuilder);
        ArgumentNullException.ThrowIfNull(cardPreparation);

        _pathService = pathService;
        _sitemapBuilder = sitemapBuilder;
        _markupMinifier = markupMinifier;
        _pageBuilder = pageBuilder;
        _cardPreparation = cardPreparation;
    }

    public void PreparePage<T>(string directory) where T : LayoutModelBase
    {
        ArgumentNullException.ThrowIfNull(directory);

        //Get the result from the builder
        var builderResult = _pageBuilder.Build<T>(directory);

        //minify and save page
        var minifier = _markupMinifier.Minify(builderResult.FinalHtml);
        var savingPath = Path.Combine(_pathService.OutputFolder, Path.GetFileName(builderResult.RelativeUrl));
        File.WriteAllText(savingPath, minifier.MinifiedContent);

        //copy all media associated with this page
        if (Directory.Exists(Path.Combine(directory, "media")))
        {
            Helpers.Copy(Path.Combine(directory, "media"), _pathService.OutputMediaFolder);
            foreach (var file in Directory.GetFiles(Path.Combine(directory, "media")))
            {
                var ext = Path.GetExtension(file);
                var name = Path.GetFileNameWithoutExtension(file);
                Helpers.ResizeImage(file, Path.Combine(_pathService.OutputMediaFolder, name + "-small" + ext), new Size(500, 10000));//stop at 500 width, who cares about height
            }
        }

        _sitemapBuilder.Add(builderResult.RelativeUrl, builderResult.DateModified);
    }

    public void PrepareIndex(LayoutIndexModel pageData, int cardsPerPage)
    {
        ArgumentNullException.ThrowIfNull(pageData);

        pageData.Body = _cardPreparation.GetHtml(pageData.Paging.CurrentPageIndex, cardsPerPage);
        pageData.Validate();

        var builderResult = _pageBuilder.Build(pageData);
        var minifier = _markupMinifier.Minify(builderResult.FinalHtml);
        var indexPageNumber = pageData.Paging.CurrentPageIndex == 0 ? string.Empty : "-page-" + (pageData.Paging.CurrentPageIndex + 1);
        var savingPath = Path.Combine(_pathService.OutputFolder, $"index{indexPageNumber}.html");

        // cardPreparation.GetHtml(pageIndex++, 9)

        File.WriteAllText(savingPath, minifier.MinifiedContent);
        _sitemapBuilder.Add(savingPath, builderResult.DateModified);

    }
}
