using System.Text;

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
        return indexTemplate.Replace("{extra-headers}", String.Join(' ', itemData.ExtraHeaders))
                            .Replace("{page-plaintext-description}", itemData.PlainTextDescription)
                            .Replace("{og-type}", itemData.Type)
                            .Replace("{page-relative-image-url}", itemData.RelativeImageUrl)
                            .Replace("{page-relative-url}", itemData.RelativeUrl)
                            .Replace("{page-title}", itemData.Title)

                            .Replace("{article-title}", itemData.Title)
                            .Replace("{page-body}", body)
                            .Replace("{article-content}", body)

                            .Replace("{ad-relative-image-url}", itemData.RelativeImageUrl)
                            .Replace("{article-relative-image-url}", itemData.RelativeImageUrl)
                            .Replace("{article-relative-thumb-url}", itemData.RelativeImageUrl)
                            .Replace("{article-description}", itemData.Description)
                            .Replace("{article-date}", itemData.DateModified?.ToLongDateString());
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

    
}
