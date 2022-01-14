using Blog.Builder.Exceptions;

namespace Blog.Builder.Models.Templates;

public class CardImageModel : CardModelBase
{

    public new void Validate()
    {
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.Title);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.Link);
        ExceptionHelpers.ThrowIfPathNotExists(this.Image);

    }
}
