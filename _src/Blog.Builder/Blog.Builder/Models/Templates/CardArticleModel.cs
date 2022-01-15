using Blog.Builder.Exceptions;

namespace Blog.Builder.Models.Templates;

public record class CardArticleModel : CardModelBase
{
    public static CardArticleModel FromBase(CardModelBase parent)
    {
        return new CardArticleModel
        {
            TemplateDataModel = parent.TemplateDataModel,
            ImageUrl = parent.ImageUrl,
            Title = parent.Title,
            Description = parent.Description,
            Link = parent.Link,
            Footer = parent.Footer
        };
    }

    public new void Validate()
    {
        base.Validate();

        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.Title);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.Description);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.Footer);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.Link);

        if (TemplateDataModel != nameof(CardArticleModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(CardArticleModel)} for the type {nameof(CardArticleModel)}.");
        }
    }
}
