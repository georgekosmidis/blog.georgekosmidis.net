using Blog.Builder.Exceptions;
using Blog.Builder.Models;
using Newtonsoft.Json;
using RazorEngine.Templating;

namespace Blog.Builder.Services;

internal record class PageBuilderResult
{
    public string FinalHtml { get; init; } = String.Empty;
    public string RelativeUrl { get; init; } = String.Empty;
    public DateTime DateModified { get; init; } = DateTime.MaxValue;

}

internal class PageBuilder : IPageBuilder
{
    private readonly IRazorEngineService _templateEngine;
    private readonly ITemplateProvider _templateProvider;

    public PageBuilder(IRazorEngineService templateService,
                                ITemplateProvider templateProvider)
    {
        ArgumentNullException.ThrowIfNull(templateService);
        ArgumentNullException.ThrowIfNull(templateProvider);

        _templateEngine = templateService;
        _templateProvider = templateProvider;
    }

    private PageBuilderResult Build<T>(T pageData) where T : LayoutModelBase
    {
        ArgumentNullException.ThrowIfNull(pageData);
        pageData.Validate();

        var finalHtml = _templateEngine.RunCompile(
                                                _templateProvider.Get<T>(),
                                                Guid.NewGuid().ToString(),
                                                typeof(T),
                                                pageData);

        return new PageBuilderResult
        {
            FinalHtml = finalHtml,
            DateModified = pageData.DateModified,
            RelativeUrl = pageData.RelativeUrl
        };
    }

    public PageBuilderResult Build<T>(string directory) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfPathNotExists(directory);

        var jsonFileContent = File.ReadAllText(Path.Combine(directory, "content.json"));
        var pageData = JsonConvert.DeserializeObject<T>(jsonFileContent);
        ExceptionHelpers.ThrowIfNull(pageData);

        //first prepare the body of the page
        pageData.Body = File.ReadAllText(Path.Combine(directory, "content.html"));
        pageData.TemplateDataModel = typeof(T).Name;
        var innerPartHtml = Build(pageData);

        //then prepare the entire page
        pageData.Body = innerPartHtml.FinalHtml;
        var completePageHtml = Build(pageData as LayoutModelBase);

        return completePageHtml;
    }

}
