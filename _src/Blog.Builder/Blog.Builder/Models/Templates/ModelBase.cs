using Blog.Builder.Exceptions;

namespace Blog.Builder.Models.Templates;

public abstract record class ModelBase
{
    public string TemplateDataModel { get; set; } = nameof(ModelBase);

    private static Guid nonce = Guid.NewGuid();
    public Guid Nonce
    {
        get
        {
            //todo: lock or a better way for a unique nonce?
            return nonce;
        }
    }

    public void Validate()
    {
        ExceptionHelpers.ThrowIfNull(this.Nonce);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.TemplateDataModel);

        if (TemplateDataModel == nameof(ModelBase))
        {
            throw new Exception($"{nameof(TemplateDataModel)} cannot be of base type {nameof(ModelBase)}.");
        }
    }
}
