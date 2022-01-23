using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Models.Builders;
using Blog.Builder.Models.Templates;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using WebMarkupMin.Core;

namespace Blog.Builder.Services;

/// <inheritdoc/>
internal class PageProcessor : IPageProcessor
{
    private readonly IPathService _pathService;
    private readonly ISitemapBuilder _sitemapBuilder;
    private readonly IMarkupMinifier _markupMinifier;
    private readonly IPageBuilder _pageBuilder;
    private readonly ICardProcessor _cardProcessor;


    public PageProcessor(IPathService pathService,
                        ISitemapBuilder sitemapBuilder,
                        IMarkupMinifier markupMinifier,
                        IPageBuilder pageBuilder,
                        ICardProcessor cardProcessor)
    {
        ArgumentNullException.ThrowIfNull(pathService);
        ArgumentNullException.ThrowIfNull(sitemapBuilder);
        ArgumentNullException.ThrowIfNull(markupMinifier);
        ArgumentNullException.ThrowIfNull(pageBuilder);
        ArgumentNullException.ThrowIfNull(cardProcessor);

        _pathService = pathService;
        _sitemapBuilder = sitemapBuilder;
        _markupMinifier = markupMinifier;
        _pageBuilder = pageBuilder;
        _cardProcessor = cardProcessor;
    }

    //todo: introduce caching
    private T GetPageModelData<T>(string jsonPath) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfPathNotExists(jsonPath);

        var json = File.ReadAllText(jsonPath);
        var data = JsonConvert.DeserializeObject<T>(json);
        ExceptionHelpers.ThrowIfNull(data);

        return data;
    }

    private PageBuilderResult GetBuilderResult<T>(string directory) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfPathNotExists(directory);

        //read json and html from the folder
        var jsonFileContent = Path.Combine(directory, "content.json");

        var pageData = this.GetPageModelData<T>(jsonFileContent);
        ExceptionHelpers.ThrowIfNull(pageData);

        var pageHtml = File.ReadAllText(Path.Combine(directory, "content.html"));
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(pageHtml);

        //get the right column cards, if any
        var rightColumnCards = _cardProcessor.GetRightColumnCardsHtml();

        //get the inner layout build
        var internalHtml = _pageBuilder.BuildInternalLayout(pageData, pageHtml, rightColumnCards);

        //get the outer layout build
        var builderResult = _pageBuilder.BuildMainLayout(pageData, internalHtml);

        //done!
        return builderResult;
    }

    private PageBuilderResult GetBuilderResult<T>(T pageData, IEnumerable<string> pageCards) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfNull(pageData);
        ExceptionHelpers.ThrowIfNullOrEmpty(pageCards);

        //get the right column cards, if any
        var rightColumnCards = _cardProcessor.GetRightColumnCardsHtml();

        //get the inner layout build
        var internalHtml = _pageBuilder.BuildInternalLayout(pageData, pageCards, rightColumnCards);

        //get the outer layout build
        var builderResult = _pageBuilder.BuildMainLayout(pageData, internalHtml);

        //done!
        return builderResult;
    }

    /// <inheritdoc/>
    public void ProcessPage<T>(string directory) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfPathNotExists(directory);

        //get the builder result
        var builderResult = GetBuilderResult<T>(directory);

        //minify and save page
        var minifier = _markupMinifier.Minify(builderResult.FinalHtml);
        if (minifier.Errors.Count() > 0)
        {
            //todo: add all errors
            throw new Exception($"Minification failed with at least one error: " +
                $"{Environment.NewLine}{minifier.Errors.First().Message}" +
                $"{Environment.NewLine}{minifier.Errors.First().SourceFragment}");
        }
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(minifier.MinifiedContent);
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

        //add the page to the sitemap builder
        _sitemapBuilder.Add(builderResult.RelativeUrl, builderResult.DateModified);
    }

    /// <inheritdoc/>
    public void ProcessIndex(LayoutIndexModel pageData, int cardsPerPage)
    {
        ArgumentNullException.ThrowIfNull(pageData);

        //Get all the cards html for the main page
        var cards = _cardProcessor.GetBodyCardsHtml(pageData.Paging.CurrentPageIndex, cardsPerPage);

        //build the page
        var builderResult = this.GetBuilderResult(pageData, cards);

        var minifier = _markupMinifier.Minify(builderResult.FinalHtml);
        if (minifier.Errors.Count() > 0)
        {
            throw new Exception($"Minification failed with at least one error:" +
                $"{Environment.NewLine}{minifier.Errors.First().Message}" +
                $"{Environment.NewLine}{minifier.Errors.First().SourceFragment}");
        }
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(minifier.MinifiedContent);

        //save it
        var indexPageNumber = pageData.Paging.CurrentPageIndex == 0 ? string.Empty : "-page-" + (pageData.Paging.CurrentPageIndex + 1);
        var savingPath = Path.Combine(_pathService.OutputFolder, $"index{indexPageNumber}.html");
        File.WriteAllText(savingPath, minifier.MinifiedContent);

        //add it to sitemap.xml
        _sitemapBuilder.Add(savingPath, builderResult.DateModified);
    }
}
