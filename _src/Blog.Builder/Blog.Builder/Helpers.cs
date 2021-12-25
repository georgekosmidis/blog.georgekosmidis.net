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
        if (string.IsNullOrWhiteSpace(itemData.Url))
        {
            throw new NullReferenceException(nameof(itemData.Url));
        }

    }

    public static string BuildPage(string indexTemplate, string body, ItemData itemData )
    {
        return indexTemplate.Replace("{page-body}", body.ToString())
                            .Replace("{page-raw-description}", itemData.PlainTextDescription)
                            .Replace("{page-title}", itemData.Title)
                            .Replace("{og-type}", itemData.Type)
                            .Replace("{page-filename}", itemData.Url)
                            .Replace("{page-image}", itemData.Image);
    }
    public static string BuildCard(string cardTemplate, ItemData itemData)
    {
        return cardTemplate.Replace("{card-date}", itemData.DateModified?.ToLongDateString())
                            .Replace("{card-description}", itemData.Description)
                            .Replace("{card-title}", itemData.Title)
                            .Replace("{card-photo}", itemData.Image);
    }

    //public static string SitemapXML()
    //{
    //    var sb = new StringBuilder();
    //    sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><urlset xmlns = \"http://www.sitemaps.org/schemas/sitemap/0.9\">");

    //    sb.Append($"<url><loc>http://www.example.com/foo.html</loc><lastmod></lastmod></url>");
    //    sb.Append("</urlset>");
    //}
}
