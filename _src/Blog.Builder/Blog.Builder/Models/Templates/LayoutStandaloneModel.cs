﻿namespace Blog.Builder.Models.Templates;

/// <summary>
/// Used for the standalones, e.g. privacy.html (template-standalone.cshtml).
/// Inherits all members of <see cref="LayoutModelBase"/>.
/// </summary>
public record class LayoutStandaloneModel : LayoutModelBase
{
    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();

        if (TemplateDataModel != nameof(LayoutStandaloneModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(LayoutStandaloneModel)} for the type {nameof(LayoutStandaloneModel)}.");
        }
    }
}
