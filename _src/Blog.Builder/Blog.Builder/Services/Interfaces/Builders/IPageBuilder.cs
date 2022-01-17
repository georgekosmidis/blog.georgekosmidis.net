using Blog.Builder.Models.Builders;
using Blog.Builder.Models.Templates;

namespace Blog.Builder.Services.Interfaces.Builders;

/// <summary>
/// The service that builds the pages.
/// </summary>
internal interface IPageBuilder
{
    /// <summary>
    /// Builds the final HTML of a page, based on a directory that holds all information.
    /// </summary>
    /// <typeparam name="T">A type that inherits from <see cref="LayoutModelBase"/>.</typeparam>
    /// <param name="directory">The directory where all files describing this page resign.</param>
    /// <returns>Returns a <see cref="PageBuilderResult"/> with the final HTML, the relative URL and the date it was modified.</returns>
    PageBuilderResult Build<T>(string directory) where T : LayoutModelBase;

    /// <summary>
    /// Builds the final HTML of a page, based only on <see cref="LayoutIndexModel"/> for now.
    /// It cannot be used directly for pages that have physical parameterization, e.g. standalones or articles.
    /// In this case method <see cref="Build{T}(string)"/> must be used.
    /// </summary>
    /// <typeparam name="T">A type that inherits from <see cref="LayoutModelBase"/>.</typeparam>
    /// <param name="pageData">The information for the build, which indlude the body (<see cref="LayoutModelBase.Body"/>).</param>
    /// <returns>Returns a <see cref="PageBuilderResult"/> with the final HTML, the relative URL and the date it was modified.</returns>
    PageBuilderResult Build<T>(T pageData) where T : LayoutModelBase;
}