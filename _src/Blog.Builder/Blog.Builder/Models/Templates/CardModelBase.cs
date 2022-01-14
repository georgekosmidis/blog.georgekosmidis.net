namespace Blog.Builder.Models.Templates;

public class CardModelBase
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Footer { get; set; }
    public string? Link { get; set; }
    public string? Image { get; set; }

    public void Validate()
    {

    }
}
