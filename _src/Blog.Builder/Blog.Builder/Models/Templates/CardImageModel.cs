using Blog.Builder.Exceptions;

namespace Blog.Builder.Models.Templates;

/// <summary>
/// A card template model that displays an image.
/// </summary>
public record class CardImageModel : CardModelBase
{
    /// <summary>
    /// Converts a base <see cref="CardModelBase"/> to a <see cref="CardImageModel"/> applying any custom logic
    /// for this unusual conversion.
    /// </summary>
    /// <param name="parent">The base <see cref="CardModelBase"/> to be used.</param>
    /// <returns>A new instance of a <see cref="CardImageModel"/>.</returns>
    public static CardImageModel FromBase(CardModelBase parent)
    {
        return new CardImageModel
        {
            TemplateDataModel = parent.TemplateDataModel,
            ImageUrl = parent.ImageUrl,
            Title = parent.Title,
            Link = parent.Link,
            Position = parent.Position,
            IsSticky = parent.IsSticky
        };
    }

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();

        if (TemplateDataModel != nameof(CardImageModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(CardImageModel)} for the type {nameof(CardImageModel)}.");
        }

        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.Title);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.Link);
        ExceptionHelpers.ThrowIfPathNotExists(this.ImageUrl);
    }

}