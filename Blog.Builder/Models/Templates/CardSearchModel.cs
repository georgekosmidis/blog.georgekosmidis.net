namespace Blog.Builder.Models.Templates;

/// <summary>
/// Used for the (only one exists) search card.
/// Inherits all members of <see cref="CardModelBase"/>.
/// </summary>
public record class CardSearchModel : CardModelBase
{
    /// <summary>
    /// Converts a base <see cref="CardModelBase"/> to a <see cref="CardSearchModel"/> applying any custom logic
    /// for this unusual conversion.
    /// </summary>
    /// <param name="parent">The base <see cref="CardModelBase"/> to be used.</param>
    /// <returns>A new instance of a <see cref="CardSearchModel"/>.</returns>
    public static CardSearchModel FromBase(CardModelBase parent)
    {
        return new CardSearchModel
        {
            TemplateDataModel = parent.TemplateDataModel,
            Position = parent.Position,
            IsSticky = parent.IsSticky,
            RightColumnPosition = parent.RightColumnPosition,
        };
    }

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();

        if (TemplateDataModel != nameof(CardSearchModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(CardSearchModel)} for the type {nameof(CardSearchModel)}.");
        }
    }
}
