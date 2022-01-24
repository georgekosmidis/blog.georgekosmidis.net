using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces;
using Blog.Builder.Interfaces.RazorEngineServices;
using Blog.Builder.Models.Templates;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System.Text;

namespace Blog.Builder.Services.RazorEngineServices;

/// <inheritdoc/>
internal class RazorEngineWrapperService : IRazorEngineWrapperService, IDisposable
{
    private readonly IRazorEngineService razorEngineService;
    private readonly ITemplateProvider _templateProvider;

    /// <summary>
    /// Creates an instance of a <see cref="RazorEngineService"/> and keeps in a private field.
    /// </summary>
    /// <param name="templateManager">The <see cref="ITemplateManager"/> that will be used with this instance of <see cref="RazorEngineService"/></param>
    /// <param name="templateProvider">The template provider that hosts paths and html of all templates. See <see cref="TemplateProvider"/>.</param>
    public RazorEngineWrapperService(ITemplateManager templateManager, ITemplateProvider templateProvider)
    {
        ArgumentNullException.ThrowIfNull(templateManager);
        ArgumentNullException.ThrowIfNull(templateProvider);

        _templateProvider = templateProvider;

        var config = new TemplateServiceConfiguration();
        config.EncodedStringFactory = new RawStringFactory();
        config.TemplateManager = templateManager;
#if DEBUG
        config.Debug = true;
#endif
        razorEngineService = RazorEngine.Templating.RazorEngineService.Create(config);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        razorEngineService.Dispose();
    }

    /// <inheritdoc/>
    public string RunCompile<T>(T data) where T : ModelBase
    {
        ExceptionHelpers.ThrowIfNull(data);

        var key = new RazorEngine.Templating.NameOnlyTemplateKey(
                typeof(T).Name,
                RazorEngine.Templating.ResolveType.Global,
                null);

        var sb = new StringBuilder();
        var sw = new StringWriter(sb);
        razorEngineService.RunCompile(key, sw, typeof(T), data);

        return sb.ToString();
    }


}
