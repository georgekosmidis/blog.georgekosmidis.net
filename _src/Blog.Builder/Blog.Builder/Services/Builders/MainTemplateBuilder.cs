using Blog.Builder.Exceptions;
using Blog.Builder.Models;
using Newtonsoft.Json;
using RazorEngine.Templating;

namespace Blog.Builder.Services;

internal record class MainTemplateBuilderResult
{
    public string FinalHtml { get; init; } = String.Empty;
    public string RelativeUrl { get; init; } = String.Empty;
    public DateTime DateModified { get; init; } = DateTime.MaxValue;

}

internal class MainTemplateBuilder : IMainTemplateBuilder
{
    private readonly IRazorEngineService _templateEngine;
    private readonly ITemplateProvider _templateProvider;
    private readonly IPathService _pathService;

    public MainTemplateBuilder(IRazorEngineService templateService,
                                ITemplateProvider templateProvider,
                                IPathService pathService)
    {
        ArgumentNullException.ThrowIfNull(templateService);
        ArgumentNullException.ThrowIfNull(templateProvider);
        ArgumentNullException.ThrowIfNull(pathService);

        _templateEngine = templateService;
        _templateProvider = templateProvider;
        _pathService = pathService;
    }

    public MainTemplateBuilderResult BuildManiTemplate(MainTemplateData pageData)
    {
        ArgumentNullException.ThrowIfNull(pageData);
        pageData.Validate();

        var finalHtml = _templateEngine.RunCompile(
                                                _templateProvider.MainTemplate,
                                                Guid.NewGuid().ToString(),
                                                typeof(MainTemplateData),
                                                pageData);

        return new MainTemplateBuilderResult
        {
            FinalHtml = finalHtml,
            DateModified = pageData.DateModified,
            RelativeUrl = pageData.RelativeUrl
        };
    }

    public MainTemplateBuilderResult Build(string path, PageTypes pageType)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(pageType);

        var jsonFileContent = File.ReadAllText(
                                            Path.Combine(_pathService.GetWorkingPath(pageType),
                                            Path.GetFileNameWithoutExtension(path) + ".json"));

        var pageData = JsonConvert.DeserializeObject<MainTemplateData>(jsonFileContent);
        ExceptionHelpers.ThrowIfNull(pageData);
        pageData.Body = File.ReadAllText(path);

        return BuildManiTemplate(pageData);
    }

}
