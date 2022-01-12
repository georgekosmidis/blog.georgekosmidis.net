using Blog.Builder.Models;
using Microsoft.Extensions.Options;

namespace Blog.Builder.Services;

internal class TemplateProvider : ITemplateProvider
{
    public string MainTemplate { get; private set; } = string.Empty;
    public string ArticleTemplate { get; private set; } = string.Empty;
    public string CardArticleTemplate { get; private set; } = string.Empty;
    public string CardImageTemplate { get; private set; } = string.Empty;
    public string CardSearchTemplate { get; private set; } = string.Empty;
    public string SitemapTemplate { get; private set; } = string.Empty;

    public TemplateProvider(IPathService pathService)
    {
        Task.Run(async () =>
        {
            MainTemplate = await File.ReadAllTextAsync(pathService.FileWorkingTemplateBase);
            ArticleTemplate = await File.ReadAllTextAsync(pathService.FileWorkingTemplateArticle);
            CardArticleTemplate = await File.ReadAllTextAsync(pathService.FileWorkingTemplateCardArticle);
            CardImageTemplate = await File.ReadAllTextAsync(pathService.FileWorkingTemplateCardImage);
            CardSearchTemplate = await File.ReadAllTextAsync(pathService.FileWorkingTemplateCardSearch);
            SitemapTemplate = await File.ReadAllTextAsync(pathService.FileWorkingTemplateSitemap);

        }).Wait();
    }
}
