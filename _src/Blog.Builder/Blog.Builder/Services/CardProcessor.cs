using Blog.Builder.Exceptions;
using Blog.Builder.Models.Templates;
using Blog.Builder.Services.Interfaces;
using Blog.Builder.Services.Interfaces.Builders;
using SixLabors.ImageSharp;

namespace Blog.Builder.Services;

/// <inheritdoc/>
internal class CardProcessor : ICardProcessor
{
    private readonly IPathService _pathService;
    private readonly ICardBuilder _cardBuilder;

    public CardProcessor(IPathService pathService,
                        ICardBuilder cardBuilder)
    {
        ArgumentNullException.ThrowIfNull(pathService);
        ArgumentNullException.ThrowIfNull(cardBuilder);

        _pathService = pathService;
        _cardBuilder = cardBuilder;
    }

    /// <inheritdoc/>
    public void ProcessCard(string directory)
    {
        ExceptionHelpers.ThrowIfPathNotExists(directory);

        //build and add the card to the card collection
        _cardBuilder.AddCard(directory);

        //copy all media associated with this card
        if (Directory.Exists(Path.Combine(directory, "media")))
        {
            Helpers.Copy(Path.Combine(directory, "media"), _pathService.OutputMediaFolder);

            //create smaller versions of the media
            foreach (var file in Directory.GetFiles(Path.Combine(directory, "media")))
            {
                var ext = Path.GetExtension(file);
                var name = Path.GetFileNameWithoutExtension(file);
                Helpers.ResizeImage(file, Path.Combine(_pathService.OutputMediaFolder, name + "-small" + ext), new Size(300, 10000));//stop at 300 width, who cares about height
            }
        }
    }

    /// <inheritdoc/>
    public void ProcessArticleCard(CardArticleModel cardData, DateTime datePublished)
    {
        ArgumentNullException.ThrowIfNull(cardData);

        _cardBuilder.AddArticleCard(cardData, datePublished);
    }

    /// <inheritdoc/>
    public string GetHtml(int pageIndex, int cardsPerPage)
    {
        return _cardBuilder.GetHtml(pageIndex, cardsPerPage);
    }

    /// <inheritdoc/>
    public int GetCardsNumber(int cardsPerPage)
    {
        return _cardBuilder.GetCardsNumber(cardsPerPage);
    }

}
