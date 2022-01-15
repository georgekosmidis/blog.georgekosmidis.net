using Blog.Builder.Exceptions;

namespace Blog.Builder.Models.Templates;

public record class CardImageModel : CardModelBase
{
    public static CardImageModel FromBase(CardModelBase parent)
    {
        return new CardImageModel
        {
            TemplateDataModel = parent.TemplateDataModel,
            ImageUrl = parent.ImageUrl,
            Title = parent.Title,
            Link = parent.Link
        };
    }


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