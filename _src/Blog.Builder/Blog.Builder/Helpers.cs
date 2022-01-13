using System.Text;
using System.Text.RegularExpressions;
using Blog.Builder.Models;
using Newtonsoft.Json;
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

    public static void ValidateItemData(MainTemplateData itemData)
    {
        if (itemData.Type == PageTypes.Unknown)
        {
            throw new NullReferenceException(nameof(itemData.Type));
        }
        //if (itemData.DateModified is null)
        //{
        //    throw new NullReferenceException(nameof(itemData.DateModified));
        //}
        //if (itemData.DatePublished is null)
        //{
        //    throw new NullReferenceException(nameof(itemData.DatePublished));
        //}
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
