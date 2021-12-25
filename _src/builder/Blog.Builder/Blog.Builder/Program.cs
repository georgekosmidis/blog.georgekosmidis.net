using System.Text;
using Newtonsoft.Json;

var ROOT = "..\\..\\..\\..\\..\\..\\..\\";
Console.WriteLine("Hello, World!");

var indexTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "template", "index.html"));
var cardPostTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "template", "card-post.html"));
var cardImageTemplate = await File.ReadAllTextAsync(Path.Combine(ROOT, "template", "card-image.html"));

//prepare output
Directory.Delete(Path.Combine(ROOT, "_output"), true);
Directory.CreateDirectory(Path.Combine(ROOT, "_output"));
Directory.CreateDirectory(Path.Combine(ROOT, "_output", "media"));

var directories = Directory.GetDirectories(Path.Combine(ROOT, "posts")).ToList().OrderByDescending(x => x);
var body = new StringBuilder();
foreach (var directory in directories)
{
    var post = File.ReadAllText(Path.Combine(directory, "content.html"));
    var rawData = File.ReadAllText(Path.Combine(directory, "content.json"));
    var cardData = JsonConvert.DeserializeObject<CardData>(rawData) ?? throw new NullReferenceException("Card Data");

    if (cardData.Type == "post")
    {
        body.Append(
            cardPostTemplate.Replace("{post-photo}", cardData.Image)
                            .Replace("{post-title}", cardData.Title)
                            .Replace("{post-description}", cardData.Description)
                            .Replace("{post-date}", cardData.DateModified.ToString())
        );
    }

    List<string> media = new List<string>();
    if (Directory.Exists(Path.Combine(directory, "media")))
    {
        foreach (var image in Directory.GetFiles(Path.Combine(directory, "media")).ToList())
        {
            File.Copy(image, Path.Combine(ROOT, "_output", "media", Path.GetFileName(image)));
        }
    }
}

var index = indexTemplate.Replace("{body}", body.ToString());
File.WriteAllText(Path.Combine(ROOT, "_output", "index.html"), index);

