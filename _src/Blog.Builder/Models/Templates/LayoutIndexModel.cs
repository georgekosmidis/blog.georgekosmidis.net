using Blog.Builder.Exceptions;

namespace Blog.Builder.Models.Templates;

/// <summary>
/// Used for the index pages.
/// Inherits all members of <see cref="LayoutModelBase"/>.
/// </summary>
public record class LayoutIndexModel : LayoutModelBase
{
    /// <summary>
    /// Constructor that sets the <see cref="ModelBase.TemplateDataModel"/> to nameof(<seealso cref="LayoutIndexModel"/>).
    /// This is a very specific tempalte model, so setting the <see cref="ModelBase.TemplateDataModel"/> from within 
    /// enforces its usage.
    /// </summary>
    public LayoutIndexModel() => TemplateDataModel = nameof(LayoutIndexModel);//its a very specific model

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();

        ExceptionHelpers.ThrowIfNull(Paging);
        ExceptionHelpers.ThrowIfNull(Paging.CurrentPageIndex);
        ExceptionHelpers.ThrowIfNull(Paging.TotalCardsCount);
        ExceptionHelpers.ThrowIfNullOrEmpty(CardsHtml);

        if (TemplateDataModel != nameof(LayoutIndexModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(LayoutIndexModel)} for the type {nameof(LayoutIndexModel)}.");
        }
    }
}