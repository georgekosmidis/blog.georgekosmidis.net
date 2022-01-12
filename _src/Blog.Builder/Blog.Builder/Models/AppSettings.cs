namespace Blog.Builder.Models;

internal record class AppSettings
{
    public string RawFolder { get; init; } = string.Empty;
    public string OutputFolder { get; init; } = string.Empty;
    public string TemplatesFolder { get; init; } = string.Empty;
}

