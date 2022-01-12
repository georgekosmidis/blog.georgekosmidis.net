using Blog.Builder.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RazorEngine.Templating;
using WebMarkupMin.Core;

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
    private readonly IPathService _pathService;

    public PageBuilder(IRazorEngineService templateService,
                        ITemplateProvider templateProvider,
                        IPathService pathService)
    {
        _templateEngine = templateService;
        _templateProvider = templateProvider;
        _pathService = pathService;
    }

    public PageBuilderResult Build(string path, PageTypes pageType)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(pageType);

        var jsonFileContent = File.ReadAllText(Path.Combine(_pathService.GetWorkingPath(pageType), Path.GetFileNameWithoutExtension(path) + ".json"));
       
        var pageData = JsonConvert.DeserializeObject<MainTemplateData>(jsonFileContent) ?? throw new NullReferenceException("Can't find JSON for " + path);
        pageData.Body = File.ReadAllText(path);

        if(pageType == PageTypes.MainPage)
        {
            pageData.ValidateMainPage();
        }
        else
        {
            pageData.Validate();
        }

        var finalHtml = _templateEngine.RunCompile(_templateProvider.MainTemplate, Guid.NewGuid().ToString(), typeof(MainTemplateData), pageData);

        return new PageBuilderResult
        {
           FinalHtml  = finalHtml,
           DateModified = pageData.DateModified,
           RelativeUrl = pageData.RelativeUrl
        };
    }

}
