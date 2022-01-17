using Blog.Builder.Models.Templates;

namespace Blog.Builder.Services.Interfaces.Builders;

/// <summary>
/// The service that builds cards.
/// </summary>
internal interface ICardBuilder
{
    /// <summary>
    /// Adds an article card to the collection of cards
    /// </summary>
    /// <param name="cardData">The <see cref="CardArticleModel"/> that conains all information.</param>
    /// <param name="datePublished">The date this artice was published (for ordering)</param>
    void AddArticleCard(CardArticleModel cardData, DateTime datePublished);

    /// <summary>
    /// Adds a card to the collection of cards. 
    /// The card must have a physical directory describing it, and thus it cannot be an article.
    /// The method tries to load data from a "card.json" file which will be deserialized to a <see cref="CardModelBase"/>.
    /// </summary>
    /// <param name="directory">The directory that contains all necessary information for the card.</param>
    void AddCard(string directory);

    /// <summary>
    /// Builds the HTML of the requested index page.
    /// </summary>
    /// <param name="pageIndex">The index of the page (e.g. 2 for index-page-3.html)</param>
    /// <param name="cardsPerPage">The number of cards per page.</param>
    /// <returns>Returns the HTML for the requested page.</returns>
    string GetHtml(int pageIndex, int cardsPerPage);

    /// <summary>
    /// Gets the total number of cards in the collection.
    /// The number of cards is calculated using the number of articles, the number of non-sticky cards
    /// and the number of sticky cards multiplied by the number of pages, since the sticky ones will 
    /// exist in every page.
    /// </summary>
    /// <param name="cardsPerPage">The number of cards per page. It will be used for the sticky cards calculation.</param>
    /// <returns>The total number of cards registered.</returns>
    int GetCardsNumber(int cardsPerPage);
}
