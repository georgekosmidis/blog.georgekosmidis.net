namespace Blog.Builder.Models.Templates;

public record class CardModelBase : ModelBase
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Footer { get; set; }
    public string? Link { get; set; }
    public string? ImageUrl { get; set; }
    public int Position { get; set; }
    public bool IsSticky { get; set; }

    public new void Validate()
    {
        base.Validate();

        if (TemplateDataModel == nameof(CardModelBase))
        {
            throw new Exception($"{nameof(TemplateDataModel)} cannot be of base type {nameof(CardModelBase)}.");
        }
    }
}
