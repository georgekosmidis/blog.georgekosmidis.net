using Blog.Builder.Exceptions;
using System.Diagnostics.CodeAnalysis;

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


    private static readonly Guid nonce = Guid.NewGuid();


    /// <summary>
    /// A static Guid as the cryptographic nonce attribute.
    /// Since the build is static a new nonce will not produced in every visit but in every build.
    /// </summary>
    /// <remarks>
    /// todo: find a way to produce a new nonce for every page load, which means it cannot be part of the build,
    ///       but maybe part of the host?
    /// </remarks>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "RazorEngine wants it to be an instance member.")]
    public Guid Nonce => nonce;

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public void Validate()
    {
        ExceptionHelpers.ThrowIfNull(Nonce);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(TemplateDataModel);
    }
}
