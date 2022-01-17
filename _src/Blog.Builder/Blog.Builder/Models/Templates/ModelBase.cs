using Blog.Builder.Exceptions;

namespace Blog.Builder.Models.Templates;

/// <summary>
/// The base model of all tempalte models.
/// </summary>
public abstract record class ModelBase
{
    /// <summary>
    /// Property that holds the current template model name.
    /// </summary>
    public string TemplateDataModel { get; set; } = nameof(ModelBase);


    private static Guid nonce = Guid.NewGuid();

    /// <summary>
    /// A static Guid as the cryptographic nonce attribute.
    /// Since the build is static a new nonce will not produced in every visit but in every build.
    /// todo: find a way to produce a new nonce for every page load, which means it cannot be part of the build,
    ///       but maybe part of the host?
    /// </summary>
    public Guid Nonce
    {
        get
        {
            //todo: lock or a better way for a unique nonce?
            return nonce;
        }
    }

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
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
