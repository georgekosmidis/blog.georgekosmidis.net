using Blog.Builder.Exceptions;

namespace Blog.Builder.Models.Templates;

public record class LayoutIndexModel : LayoutModelBase
{
    public LayoutIndexModel()
    {
        this.TemplateDataModel = nameof(LayoutIndexModel);//its a very specific model
    }

    public new void Validate()
    {
        base.Validate();

        ExceptionHelpers.ThrowIfNull(Paging);
        ExceptionHelpers.ThrowIfNull(Paging.CurrentPageIndex);
        ExceptionHelpers.ThrowIfNull(Paging.CardsCount);

        if (TemplateDataModel != nameof(LayoutIndexModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(LayoutIndexModel)} for the type {nameof(LayoutIndexModel)}.");
        }

    }
}
