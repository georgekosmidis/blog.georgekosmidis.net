namespace Blog.Builder.Services.Interfaces;

/// <summary>
/// A service that provides the html for the templates.
/// </summary>
internal interface ITemplateProvider
{
    /// <summary>
    /// Returns the proper template HTML based on the model <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The template model to get the equivalent HTML.</typeparam>
    /// <returns>The HTML of the proper template</returns>
    public string Get<T>();
}