using Svg;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

internal class Program
{
    private static List<int> Sizes = new() { 32, 64, 256, 512, 2048 };
    private static List<int> VisibleSizes = new() { 64, 256, 512, 2048 };

    private static Uri AzureBaseUri = new("https://georgekosmidis.azureedge.net/azure-architecture-icons/");
    private static string SvgDirectoryPath = "..\\..\\..\\Azure Architecture Icons\\";
    private static string CustomPagePath = "..\\..\\..\\..\\..\\workables\\standalones\\azure-architecture-icons\\";

    private static string EditedSvgFlag = "-geko";

    private static bool PreparePngs = true;
    private static int DisplayPngSize = 32;

    private static void Main(string[] args)
    {

        if (PreparePngs)
        {
            Console.WriteLine(new string('*', 50));
            Console.WriteLine("Deleting old versions.");
            Console.WriteLine(new string('*', 50));

            //Delete old PNGs
            var pngFiles = Directory.GetFiles(SvgDirectoryPath, "*.png", SearchOption.AllDirectories);
            foreach (var pngFile in pngFiles)
            {
                File.Delete(pngFile);
                Console.WriteLine($"{pngFile} deleted.");
            }

            //Delete old SVGs
            var svgFiles = Directory.GetFiles(SvgDirectoryPath, "*.svg", SearchOption.AllDirectories).Where(x => x.Contains(EditedSvgFlag));
            foreach (var svgFile in svgFiles)
            {
                File.Delete(svgFile);
                Console.WriteLine($"{svgFile} deleted.");
            }
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(new string('*', 50));
        Console.WriteLine(PreparePngs ? "Creating new PNGs and HTML" : "Creating the HTML");
        Console.WriteLine(new string('*', 50));

        var html = new StringBuilder();
        var tags = new List<string>();
        tags.Add("Azure Architecture Icons");

        var categoryDirectories = Directory.GetDirectories(SvgDirectoryPath).OrderBy(x => x);
        foreach (var carDir in categoryDirectories)
        {
            var rootFolder = new DirectoryInfo(carDir).Name;
            var category = MakeTextLookNice(rootFolder);
            html.AppendLine(
                GetCategoryHTML(
                    category
                )
            );
            tags.Add(category);

            var svgFiles = Directory.GetFiles(carDir, "*.svg", SearchOption.AllDirectories).Where(x => !x.Contains(EditedSvgFlag)).OrderBy(x => x);
            foreach (var svgFile in svgFiles)
            {
                // Create the HTML
                var baseName = Path.GetFileNameWithoutExtension(svgFile);
                var dir = Path.GetDirectoryName(svgFile) ?? throw new Exception("That should never happen.");
                tags.Add(MakeTextLookNice(baseName));

                // Remove original size restrictions
                var svgText = File.ReadAllText(svgFile);
                svgText = FixSvg(svgText);
                File.WriteAllText(Path.Combine(dir, $"{baseName}{EditedSvgFlag}.svg"), svgText);
                //File.WriteAllText(Path.Combine("C:\\111", $"{MakeTextLookNice(rootFolder)} - {MakeTextLookNice(baseName)}.svg"), svgText);

                var svgDocument = SvgDocument.Open(svgFile);
                svgDocument.ShapeRendering = SvgShapeRendering.GeometricPrecision;

                // Create HTML
                html.AppendLine(
                    GetCardHTML(rootFolder, baseName)
                );

                // Create PNGs from SVG
                if (PreparePngs)
                {
                    foreach (var size in Sizes)
                    {
                        var bmp = svgDocument.Draw(size, size);
                        var name = GetImageName(baseName, size);

                        bmp.Save(Path.Combine(dir, name), ImageFormat.Png);
                    }
                }

                Console.WriteLine($"{svgFile} done.");
            }
        }


        // Save the page
        File.Delete(CustomPagePath + "content.html");
        File.WriteAllText(CustomPagePath + "content.html", AddWrappers(html.ToString()));

        var json = PrepareJson(tags);
        File.Delete(CustomPagePath + "content.json");
        File.WriteAllText(CustomPagePath + "content.json", json);


    }

    private static string FixSvg(string svgText)
    {
        //svgText = Regex.Replace(svgText, "width=['\"][0-9.]+['\"]", "", RegexOptions.IgnoreCase);
        //svgText = Regex.Replace(svgText, "height=['\"][0-9.]+['\"]", "", RegexOptions.IgnoreCase);

        var regex = new Regex("width=['\"][0-9.]+['\"]\\sheight=['\"][0-9.]+['\"]\\sviewBox=\"[0-9.]+\\s[0-9.]+\\s[0-9.]+\\s[0-9.]+\"", RegexOptions.IgnoreCase);
        var matches = regex.Matches(svgText);

        if (matches.Count() == 1)
        {
            svgText = svgText.Replace(matches[0].Value, "viewBox=\"0 0 18 18\"");
        }
        else if(matches.Count() > 1)
        {
            throw new Exception("It has to be one and only one");
        }
        return svgText;
    }
    private static string PrepareJson(List<string> tags)
    {
        return $@"
                    {{
                      ""RelativeUrl"": ""/azure-architecture-icons.html"",
                      ""Title"": ""Azure Architecture Icons"",
                      ""Description"": ""On this page, you will find a helpful resource for accessing every Azure Icon in SVG and PNG format in various sizes. This is an invaluable tool for creating Azure Architectural diagrams using online services like draw.io and miro.com, as well as desktop applications like Visio. To use the webpage, simply visit the page, use CONTROL+F to search for the desired icon by name, or scroll through the options to find the desired icon. This is a must-have resource for anyone working with Azure Architectural diagrams."",
                      ""DatePublished"": ""2023-01-04T18:48:24+02:00"",
                      ""DateModified"": ""{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzzz")}"",
                      ""RelativeImageUrl"": """",
                      ""Tags"": [""{string.Join("\",\"", tags)}""]
                    }}
        ";
    }

    private static string AddWrappers(string html)
    {
        return $@"
            <div class=""container"">
                <div class=""row p-1 mb-3 m2-3"">
                    <div class=""col"">

                        <p>
                            On this page, you will find a helpful resource for accessing every Azure Icon in SVG and PNG format in various sizes. 
                            This is an invaluable tool for creating Azure Architectural diagrams using online services like draw.io and miro.com, 
                            as well as desktop applications like Visio. To use the webpage, simply visit the page, use <code>CTRL+F</code> to search for the desired icon by name, 
                            or scroll through the options to find the desired icon. This is a must-have resource for anyone working with Azure Architectural Diagrams.
                        </p>

                        <p class=""d-flex justify-content-center"">
                            <a href=""https://app.diagrams.net/?splash=0&clibs=U{Uri.EscapeDataString($"{AzureBaseUri}Azure Architecture Icons.xml")}"" class=""btn btn-dark"" target=""_blank"" rel=""noopener"">
                               Click to add the icons as a <i>diagrams.net</i> library (former draw.io)
                            </a>
                        </p>                        

                        <blockquote>
                            <strong>Icon terms!</strong><br />
                            Microsoft permits the use of these icons in architectural diagrams, training materials, or documentation. You may copy, distribute, 
                            and display the icons only for the permitted use unless granted explicit permission by Microsoft. Microsoft reserves all other rights.
                        </blockquote>

                        <p>
                            You can always find the original distribution of SVG Icons here: 
                            <a href=""https://learn.microsoft.com/en-us/azure/architecture/icons/"">Azure Architecture Icons</a>
                        </p>

                    </div>
                </div>
                {html}
            </div>";
    }

    private static string GetCategoryHTML(string category)
    {
        return $@"
            <div class=""row p-1 mb-3 m2-3"">
                <div class=""col"">
                   <h3> {category} </h3>
                </div>
            </div>
        ";
    }

    private static string GetCardHTML(string rootFolder, string baseName)
    {
        var defaultSrc = GetAzureImageUrl(rootFolder, GetImageName(baseName, DisplayPngSize));
        var svgSrc = GetAzureImageUrl(rootFolder, $"{baseName}{EditedSvgFlag}.svg");

        //var srcset = new StringBuilder();
        //foreach (var size in Sizes)
        //{
        //    var src = GetAzureImageUrl(rootFolder, GetImageName(baseName, size));
        //    srcset.Append(src);
        //    srcset.AppendLine($" {size}w,");
        //}
        ////scrset removed (why have one): srcset=""{srcset.ToString().TrimEnd(',')}""

        var title = CreateDescr(rootFolder, baseName, true);
        var descr = CreateDescr(rootFolder, baseName, false);

        var pnglinks = new StringBuilder("| ");
        foreach (var size in VisibleSizes)
        {
            var src = GetAzureImageUrl(rootFolder, GetImageName(baseName, size));
            pnglinks.Append($"<a href=\"{src}\" rel=\"noopener\" target=\"_blank\" title=\"A {size}x{size} icon for '{title}'\">{size}</a> | ");
        }
        var svglink = $"<a href=\"{svgSrc}\" rel=\"noopener\" target=\"_blank\" title=\"An SVG icon for '{title}'\">SVG</a> ";

        return $@"
            <div class=""row border rounded bg-light p-1 mb-3"">
                <div class=""col-sm-1 col-2 img-thumbnail d-flex flex-wrap align-items-center"">
                    <img src=""{defaultSrc}"" alt=""{title}"" title=""{title}"" width=""{DisplayPngSize}"" height=""{DisplayPngSize}""/>
                </div>
                <div class=""col"">
                   {descr}
                   <small>Download the {svglink} or choose one of the following PNGs sizes: {pnglinks}</small>
                </div>
            </div>
        ";
    }

    private static string GetImageName(string baseName, int size)
    {
        return $"{baseName}-{size}x{size}.png";
    }

    private static string GetAzureImageUrl(string rootFolder, string imageName)
    {
        var a = new Uri(AzureBaseUri, rootFolder.TrimEnd('/') + "/");
        var b = new Uri(a, imageName);
        return b.ToString().Replace(" ", "%20");
    }

    private static string CreateDescr(string rootFolder, string baseName, bool forImageTitle)
    {
        var categoryPretty = MakeTextLookNice(rootFolder);
        var iconNamePretty = MakeTextLookNice(baseName);
        return forImageTitle
            ? $"Azure Architecture Icons / {categoryPretty} / {iconNamePretty}"
            : $@"<div class=""fw-bold"">{iconNamePretty}</div>";
    }

    private static string MakeTextLookNice(string text)
    {
        var textInfo = new CultureInfo("en-US", false).TextInfo;
        text = textInfo.ToTitleCase(text);
        text = text.Replace("+", "&");
        text = Regex.Replace(text, "((^|\\s)ai($|\\s))", " AI ", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, "((^|\\s)ml($|\\s))", " ML ", RegexOptions.IgnoreCase);
        text = Regex.Replace(text, "((^|\\s)iot($|\\s))", " IoT ", RegexOptions.IgnoreCase);

        //filename specifics
        text = text.Replace("-", " ");
        text = text.Replace(".svg", "");
        text = Regex.Replace(text, "^[0-9]+\\sicon\\sservice\\s", "", RegexOptions.IgnoreCase);

        text = Regex.Replace(text, "\\s+", " ");
        return text;

    }
}