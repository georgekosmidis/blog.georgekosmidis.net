using System.Text;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

using WebMarkupMin.Core;

internal static class Helpers
{
    private static readonly Guid Nonce = Guid.NewGuid();

    public static void Copy(string sourceDir, string targetDir)
    {
        Directory.CreateDirectory(targetDir);

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));
        }

        foreach (var directory in Directory.GetDirectories(sourceDir))
        {
            Copy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }
    }

    public static void ValidateItemData(ItemData itemData)
    {
        if (string.IsNullOrWhiteSpace(itemData.Type))
        {
            throw new NullReferenceException(nameof(itemData.Type));
        }
        if (itemData.DateModified is null)
        {
            throw new NullReferenceException(nameof(itemData.DateModified));
        }
        if (itemData.DatePublished is null)
        {
            throw new NullReferenceException(nameof(itemData.DatePublished));
        }
        if (itemData.Tags is null || !itemData.Tags.Any())
        {
            throw new NullReferenceException(nameof(itemData.Tags));
        }
        if (string.IsNullOrWhiteSpace(itemData.Description))
        {
            throw new NullReferenceException(nameof(itemData.Description));
        }
        if (string.IsNullOrWhiteSpace(itemData.Title))
        {
            throw new NullReferenceException(nameof(itemData.Title));
        }
        if (string.IsNullOrWhiteSpace(itemData.RelativeUrl))
        {
            throw new NullReferenceException(nameof(itemData.RelativeUrl));
        }
    }

    public static string BuildHtml(string indexTemplate, string body, string paging, ItemData itemData)
    {
        ValidateItemData(itemData);
        var template = indexTemplate.Replace("{itemdata-Type}", itemData.Type, StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-RelativeUrl}", itemData.RelativeUrl, StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-Title}", itemData.Title, StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-Description}", itemData.Description, StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-PlainTextDescription}", itemData.PlainTextDescription, StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-DateModifiedText}", itemData.DateModifiedText, StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-DatePublishedText}", itemData.DatePublishedText, StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-DatePublishedAndModificationText}", itemData.DatePublishedAndModificationText, StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-DatePublishedOrModificationText}", itemData.DatePublishedOrModificationText, StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-DateModified}", itemData.DateModified?.ToString("o"), StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-DatePublished}", itemData.DatePublished?.ToString("o"), StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-dateexpires}", itemData.DateModified?.AddYears(100).ToString("o"), StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-RelativeImageUrl}", itemData.RelativeImageUrl, StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-RelativeImageUrl-small}", itemData.RelativeImageUrlSmall, StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-Tags}", string.Join(", ", itemData.Tags ?? throw new NullReferenceException(nameof(itemData.Tags))), StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-ExtraHeaders}", string.Join(' ', itemData.ExtraHeaders), StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{page-navigation}", paging)
                            .Replace("{body}", body, StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{script-nonce}", Nonce.ToString(), StringComparison.InvariantCultureIgnoreCase)
                           ;
        if (string.IsNullOrWhiteSpace(itemData.RelativeImageUrl))
        {
            template = Regex.Replace(template, "{start-imageexists}(.*?){end-imageexists}", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
        }

        if (itemData.Type?.Trim().ToLower() != "article")
        {
            template = Regex.Replace(template, "{start-article}(.*?){end-article}", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
        }
        if (itemData.Type?.Trim().ToLower() != "mainpage")
        {
            template = Regex.Replace(template, "{start-mainpage}(.*?){end-mainpage}", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
        }
        if (itemData.Type?.Trim().ToLower() != "standalone")
        {
            template = Regex.Replace(template, "{start-standalone}(.*?){end-standalone}", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
        }

        template = template.Replace("{start-imageexists}", string.Empty)
                           .Replace("{end-imageexists}", string.Empty)
                           .Replace("{start-article}", string.Empty)
                           .Replace("{end-article}", string.Empty)
                           .Replace("{start-mainpage}", string.Empty)
                           .Replace("{end-mainpage}", string.Empty)
                           .Replace("{start-standalone}", string.Empty)
                           .Replace("{end-standalone}", string.Empty);
        return template;
    }

    public static string BuildSiteMapXML(List<ItemData> items)
    {
        if (items is null || !items.Any())
        {
            throw new NullReferenceException(nameof(items));
        }
        var sb = new StringBuilder();
        sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><urlset xmlns = \"http://www.sitemaps.org/schemas/sitemap/0.9\">");

        foreach (var item in items)
        {
            ValidateItemData(item);

            sb.Append($"<url><loc>https://blog.georgekosmidis.net{item.RelativeUrl}</loc><lastmod>{item.DateModified?.ToString("yyyy-MM-ddTHH:mm:sszzz")}</lastmod></url>");

        }
        sb.Append("</urlset>");

        return sb.ToString();
    }

    //TODO: revert all to razor engine. life will be simpler
    public static string BuildPaging(int currentPageIndex, int pageTotal)
    {
        var sb = new StringBuilder();

        var disabled = "";
        var previousLink = $"/index-page-{currentPageIndex}.html";
        if (currentPageIndex == 0)
        {
            disabled = "disabled";
        }
        if (currentPageIndex == 1)
        {
            previousLink = $"/index.html";
        }
        sb.Append($"<li class=\"page-item {disabled}\">");
        sb.Append($"<a class=\"page-link\" href=\"{previousLink}\">Previous</a>");
        sb.Append("</li>");

        for (var i = 0; i < pageTotal; i++)
        {
            var active = "";
            var currentLink = $"/index-page-{i + 1}.html";
            if (i == 0)
            {
                currentLink = $"/index.html";
            }
            if (i == currentPageIndex)
            {
                active = "active";
            }
            sb.Append($"<li class=\"page-item {active}\"><a class=\"page-link\" href=\"{currentLink}\">{i + 1}</a></li>");
        }

        disabled = "";
        var nextLink = $"/index-page-{currentPageIndex + 2}.html";
        if (currentPageIndex == pageTotal - 1)
        {
            nextLink = $"/index-page-{currentPageIndex + 1}.html";
            disabled = "disabled";
        }
        sb.Append($"<li class=\"page-item {disabled}\">");
        sb.Append($"<a class=\"page-link\" href=\"{nextLink}\">Next</a>");
        sb.Append("</li>");


        return sb.ToString().Trim();
    }

    public static void ResizeImage(string inputPath, string outputPath, Size size)
    {
        using var image = Image.Load(inputPath);
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = size
        }));
        image.Save(outputPath);
    }

    public static void SaveCompressedHtml(string filename, string html)
    {
        Console.WriteLine("Saving: " + filename);

        var settings = new HtmlMinificationSettings()
        {
            AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.Html5,
            CollapseBooleanAttributes = true,
            MinifyEmbeddedCssCode = true,
            MinifyEmbeddedJsCode = true,
            RemoveHtmlComments = true,
            RemoveOptionalEndTags = false,
            WhitespaceMinificationMode = WhitespaceMinificationMode.Safe
        };
        var htmlMinifier = new HtmlMinifier(settings);

        var result = htmlMinifier.Minify(html, generateStatistics: true);

        if (result.Errors.Count > 0)
        {
            //todo: fix minification errors
            File.WriteAllText(filename, html);
            Console.WriteLine("ERROR, NOT COMPRESSED: " + filename);
            return;
        }


        var statistics = result.Statistics;
        if (statistics != null)
        {
            Console.WriteLine("Saved: {0:N2}%", statistics.SavedInPercent);
        }
        File.WriteAllText(filename, result.MinifiedContent);

    }

}
