using Blog.Builder.Exceptions;

namespace Blog.Builder.Models.Templates;

/// <summary>
/// A card template model that displays an article.
/// </summary>
public record class CardArticleModel : CardModelBase
{
    /// <summary>
    /// Converts a base <see cref="CardModelBase"/> to a <see cref="CardArticleModel"/> applying any custom logic
    /// for this unusual conversion.
    /// </summary>
    /// <param name="parent">The base <see cref="CardModelBase"/> to be used.</param>
    /// <returns>A new instance of a <see cref="CardArticleModel"/>.</returns>
    public static CardArticleModel FromBase(CardModelBase parent) => new()
    {
        TemplateDataModel = parent.TemplateDataModel,
        ImageUrl = parent.ImageUrl,
        Title = parent.Title,
        Description = parent.Description,
        Link = parent.Link,
        Footer = parent.Footer
    };

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();

        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Title);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Description);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Footer);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Link);

        if (TemplateDataModel != nameof(CardArticleModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(CardArticleModel)} for the type {nameof(CardArticleModel)}.");
        }
    }
}
