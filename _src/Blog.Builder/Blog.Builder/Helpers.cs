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
        return indexTemplate.Replace("{itemdata-type}", itemData.Type)
                            .Replace("{itemdata-relativeurl}", itemData.RelativeUrl)
                            .Replace("{itemdata-title}", itemData.Title)
                            .Replace("{itemdata-description}", itemData.Description)
                            .Replace("{itemdata-plaintextdescription}", itemData.PlainTextDescription)
                            .Replace("{itemdata-lastmodifiedtext}", itemData.LastModifiedText)
                            .Replace("{itemdata-publishedtext}", itemData.PublishedText)
                            .Replace("{itemdata-datepublished}", itemData.DatePublished?.ToLongDateString())
                            .Replace("{itemdata-datemodified}", itemData.DateModified?.ToLongDateString())
                            .Replace("{itemdata-relativeimageurl}", itemData.RelativeImageUrl)
                            .Replace("{itemdata-tags}", string.Join(", ", itemData.Tags ?? throw new NullReferenceException(nameof(itemData.Tags))))
                            .Replace("{itemdata-extraheaders}", string.Join(' ', itemData.ExtraHeaders))
                            .Replace("{body}", body)
                           ;
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
