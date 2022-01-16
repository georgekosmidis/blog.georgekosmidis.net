namespace Blog.Builder.Models;

internal record class AppSettings
{
    public string WorkingFolderPath { get; init; } = string.Empty;
    public string OutputFolderPath { get; init; } = string.Empty;
    public string OutputFolderMediaName { get; init; } = string.Empty;
    public string WorkingTemplatesFolderName { get; init; } = string.Empty;
    public string WorkingJustCopyFolderName { get; init; } = string.Empty;
    public string WorkingArticlesFolderName { get; init; } = string.Empty;
    public string WorkingStandalonesFolderName { get; init; } = string.Empty;
    public string WorkingCardsFolderName { get; init; } = string.Empty;

    public string TemplateMainFilename { get; init; } = string.Empty;
    public string TemplateArticleFilename { get; init; } = string.Empty;
    public string TemplateSitemapFilename { get; init; } = string.Empty;
    public string TemplateStandaloneFilename { get; init; } = string.Empty;
    public string TemplateCardArticleFilename { get; init; } = string.Empty;
    public string TemplateCardImageFilename { get; init; } = string.Empty;
    public string TemplateCardSearchFilename { get; init; } = string.Empty;

    public int CardsPerPage { get; init; } = 9;

}

