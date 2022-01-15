namespace Blog.Builder.Models;

/// <summary>
/// Used for the standalones, e.g. privacy.html (template-standalone.cshtml)
/// </summary>
public record class LayoutStandaloneModel : LayoutModelBase
{
    public new void Validate()
    {
        base.Validate();

        if (TemplateDataModel != nameof(LayoutStandaloneModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(LayoutStandaloneModel)} for the type {nameof(LayoutStandaloneModel)}.");
        }
    }
}
