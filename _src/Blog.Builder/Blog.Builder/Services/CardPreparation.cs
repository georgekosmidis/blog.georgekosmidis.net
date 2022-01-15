using Blog.Builder.Exceptions;
using Blog.Builder.Models.Templates;
using SixLabors.ImageSharp;

namespace Blog.Builder.Services;

internal class CardPreparation : ICardPreparation
{
    private readonly IPathService _pathService;
    private readonly ICardBuilder _cardBuilder;

    public CardPreparation(IPathService pathService,
                        ICardBuilder cardBuilder)
    {
        ArgumentNullException.ThrowIfNull(pathService);
        ArgumentNullException.ThrowIfNull(cardBuilder);

        _pathService = pathService;
        _cardBuilder = cardBuilder;
    }

    public void RegisterCard(string directory)
    {
        ExceptionHelpers.ThrowIfPathNotExists(directory);

        _cardBuilder.AddCard(directory);

        //copy all media associated with this card
        if (Directory.Exists(Path.Combine(directory, "media")))
        {
            Helpers.Copy(Path.Combine(directory, "media"), _pathService.OutputMediaFolder);
            foreach (var file in Directory.GetFiles(Path.Combine(directory, "media")))
            {
                var ext = Path.GetExtension(file);
                var name = Path.GetFileNameWithoutExtension(file);
                Helpers.ResizeImage(file, Path.Combine(_pathService.OutputMediaFolder, name + "-small" + ext), new Size(300, 10000));//stop at 300 width, who cares about height
            }
        }
    }

    public void RegisterArticleCard(CardArticleModel cardData, DateTime dateCreated)
    {
        ArgumentNullException.ThrowIfNull(cardData);
        _cardBuilder.AddArticleCard(cardData, dateCreated);
    }

    public string GetHtml(int page, int perPage)
    {
        return _cardBuilder.GetHtml(page, perPage);
    }
}
