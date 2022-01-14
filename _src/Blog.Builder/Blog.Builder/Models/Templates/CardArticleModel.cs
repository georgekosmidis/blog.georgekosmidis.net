using Blog.Builder.Exceptions;

namespace Blog.Builder.Models.Templates;

public class CardArticleModel : CardModelBase
{

    public new void Validate()
    {
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.Title);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.Description);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.Footer);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.Link);
    }
}
