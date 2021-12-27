using System.Text;
using System.Text.RegularExpressions;

internal static class Helpers
{
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

    public static string BuildHtml(string indexTemplate, string body, ItemData itemData)
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
                            .Replace("{itemdata-Tags}", string.Join(", ", itemData.Tags ?? throw new NullReferenceException(nameof(itemData.Tags))), StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{itemdata-ExtraHeaders}", string.Join(' ', itemData.ExtraHeaders), StringComparison.InvariantCultureIgnoreCase)
                            .Replace("{body}", body, StringComparison.InvariantCultureIgnoreCase)
                           ;
        if (string.IsNullOrWhiteSpace(itemData.RelativeImageUrl))
        {
            template = Regex.Replace(template, "{start-imageexists}(.*?){end-imageexists}", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
        }

        if (itemData.Type != "article")
        {
            template = Regex.Replace(template, "{start-extraHeadersExists}(.*?){end-extraHeadersExists}", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
        }

        template = template.Replace("{start-imageexists}", string.Empty)
                           .Replace("{end-imageexists}", string.Empty)
                           .Replace("{start-extraHeadersExists}", string.Empty)
                           .Replace("{end-extraHeadersExists}", string.Empty);
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

    //TODO: compress output

}
