namespace Blog.Builder.Models.Builders;

/// <summary>
/// The result of the <see cref="Services.Builders.CardBuilder.AddCard(string)"/> method.
/// </summary>
internal record class OtherCardBuilderResult
{
    /// <summary>
    /// The HTML of the card.
    /// </summary>
    public string CardHtml { get; init; } = default!;

    //The position of the card in the grid.
    public int Position { get; init; }

    /// <summary>
    /// The stickiness of the card (exists in the same <see cref="Position"/> in every page).
    /// </summary>
    public bool IsSticky { get; init; }
}