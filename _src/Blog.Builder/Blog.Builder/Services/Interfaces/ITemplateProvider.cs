namespace Blog.Builder.Services;

internal interface ITemplateProvider
{
    string ArticleTemplate { get; }
    string CardArticleTemplate { get; }
    string CardImageTemplate { get; }
    string CardSearchTemplate { get; }
    string MainTemplate { get; }
    string SitemapTemplate { get; }
}