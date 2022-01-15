namespace Blog.Builder.Models.Templates;

public record class CardSearchModel : CardModelBase
{
    public static CardSearchModel FromBase(CardModelBase parent)
    {
        return new CardSearchModel
        {
            TemplateDataModel = parent.TemplateDataModel
        };
    }

    public new void Validate()
    {
        base.Validate();

        if (TemplateDataModel != nameof(CardSearchModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(CardSearchModel)} for the type {nameof(CardSearchModel)}.");
        }
    }
}
