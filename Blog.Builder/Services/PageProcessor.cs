using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Models;
using Blog.Builder.Models.Builders;
using Blog.Builder.Models.Templates;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using WebMarkupMin.Core;

namespace Blog.Builder.Services;

/// <inheritdoc/>
internal class PageProcessor : IPageProcessor
{
    private readonly ISitemapBuilder _sitemapBuilder;
    private readonly IMarkupMinifier _markupMinifier;
    private readonly IPageBuilder _pageBuilder;
    private readonly ICardProcessor _cardProcessor;
    private readonly AppSettings appSettings;

    public PageProcessor(ISitemapBuilder sitemapBuilder,
                        IMarkupMinifier markupMinifier,
                        IPageBuilder pageBuilder,
                        ICardProcessor cardProcessor,
                        IOptions<AppSettings> options)
    {
        ArgumentNullException.ThrowIfNull(sitemapBuilder);
        ArgumentNullException.ThrowIfNull(markupMinifier);
        ArgumentNullException.ThrowIfNull(pageBuilder);
        ArgumentNullException.ThrowIfNull(cardProcessor);
        ArgumentNullException.ThrowIfNull(options);

        _sitemapBuilder = sitemapBuilder;
        _markupMinifier = markupMinifier;
        _pageBuilder = pageBuilder;
        _cardProcessor = cardProcessor;
        appSettings = options.Value;
    }

    //todo: introduce caching
    private T GetPageModelData<T>(string jsonPath) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfPathNotExists(jsonPath);

        var json = File.ReadAllText(jsonPath);
        var data = JsonConvert.DeserializeObject<T>(json);
        ExceptionHelpers.ThrowIfNull(data);

        //enrich with standard properites
        data.BlogUrl = appSettings.BlogUrl;
        data.Paging = new Paging
        {
            CardsPerPage = appSettings.CardsPerPage
        };

        return data;
    }

    private PageBuilderResult GetBuilderResult<T>(string directory) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfPathNotExists(directory);

        //read json and html from the folder
        var jsonFileContent = Path.Combine(directory, Consts.ContentJsonFilename);

        var pageData = GetPageModelData<T>(jsonFileContent);
        ExceptionHelpers.ThrowIfNull(pageData);

        var pageHtml = File.ReadAllText(Path.Combine(directory, Consts.ContentHtmlFilename));
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(pageHtml);

        //add the GitHub repo
        var articleFolderName = Path.GetFileName(directory.Trim(Path.DirectorySeparatorChar));
        pageData.GithubUrl = $"{appSettings.GithubWorkablesFolderUrl}/{Consts.WorkingArticlesFolderName}/{articleFolderName}/{Consts.ContentHtmlFilename}";

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
        if (minifier.Errors.Count > 0)
        {
            //todo: add all errors
            throw new Exception($"Minification failed with at least one error: " +
                $"{Environment.NewLine}{minifier.Errors.First().Message}" +
                $"{Environment.NewLine}{minifier.Errors.First().SourceFragment}");
        }
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(minifier.MinifiedContent);
        var savingPath = Path.Combine(appSettings.OutputFolderPath, Path.GetFileName(builderResult.RelativeUrl));
        File.WriteAllText(savingPath, minifier.MinifiedContent);

        //copy all media associated with this page
        if (Directory.Exists(Path.Combine(directory, Consts.MediaFolderName)))
        {
            //copy original
            Helpers.Copy(
                    Path.Combine(directory, Consts.MediaFolderName),
                    Path.Combine(appSettings.OutputFolderPath, Consts.MediaFolderName)
            );

            //create and copy a smaller version
            foreach (var file in Directory.GetFiles(Path.Combine(directory, Consts.MediaFolderName)))
            {
                var ext = Path.GetExtension(file);
                var name = Path.GetFileNameWithoutExtension(file);

                Helpers.ResizeImage(file,
                    Path.Combine(appSettings.OutputFolderPath, Consts.MediaFolderName, name + "-small" + ext),
                    new Size(500, 10000)
                );//stop at 500 width, who cares about height :)
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
        if (minifier.Errors.Count > 0)
        {
            throw new Exception($"Minification failed with at least one error:" +
                $"{Environment.NewLine}{minifier.Errors.First().Message}" +
                $"{Environment.NewLine}{minifier.Errors.First().SourceFragment}");
        }
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(minifier.MinifiedContent);

        //save it
        var indexPageNumber = pageData.Paging.CurrentPageIndex == 0 ? string.Empty : "-page-" + (pageData.Paging.CurrentPageIndex + 1);
        var savingPath = Path.Combine(appSettings.OutputFolderPath, $"index{indexPageNumber}.html");
        File.WriteAllText(savingPath, minifier.MinifiedContent);

        //add it to sitemap.xml
        _sitemapBuilder.Add(savingPath, builderResult.DateModified);
    }

    public void Dispose()
    {
        _sitemapBuilder.Dispose();
        _pageBuilder.Dispose();
        _cardProcessor.Dispose();
    }
}
