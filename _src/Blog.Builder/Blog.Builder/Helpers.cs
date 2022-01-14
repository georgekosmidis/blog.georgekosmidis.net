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
}
