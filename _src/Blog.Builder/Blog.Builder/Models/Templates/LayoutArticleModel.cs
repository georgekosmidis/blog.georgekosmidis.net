using Blog.Builder.Exceptions;

namespace Blog.Builder.Models;

/// <summary>
/// Used for an article page (template-article.cshtml)
/// </summary>
public record class LayoutArticleModel : LayoutModelBase
{

    public string? RelativeImageUrlSmall => RelativeImageUrl is null
                ? null
                : (Path.GetDirectoryName(RelativeImageUrl) ?? string.Empty).Replace("\\", "/")
                    + "/"
                    + Path.GetFileNameWithoutExtension(RelativeImageUrl) + "-small" + Path.GetExtension(RelativeImageUrl);

    public string DateModifiedText
    {
        get
        {
            var span = DateTime.Now - DateModified;

            return SpanCalculation(span);
        }
    }

    public string DatePublishedText
    {
        get
        {
            var span = DateTime.Now - DatePublished;

            return SpanCalculation(span);
        }
    }

    public string DatePublishedAndModificationText => DateModifiedText == DatePublishedText
                ? $"Published {DatePublishedText}"
                : $"Published {DatePublishedText}, modified {DateModifiedText}";

    public string DatePublishedOrModificationText => DateModifiedText == DatePublishedText ? $"Published {DatePublishedText}" : $"Modified {DateModifiedText}";


    private static string SpanCalculation(TimeSpan span)
    {
        if (span.Days > 365)
        {
            return Math.Round(span.Days / 365d, 1) + " years ago";
        }
        if (span.Days > 0)
        {
            return span.Days > 30 ? Math.Round(span.Days / 30d) + $" month{(Math.Round(span.Days / 30d) > 1 ? "s" : "")} ago" : span.Days + $" day{(span.Days > 1 ? "s" : "")} ago";
        }
        if (span.Hours > 0)
        {
            return span.Hours + $" hour{(span.Hours > 1 ? "s" : "")} ago";
        }
        if (span.Minutes > 0)
        {
            return span.Minutes + $" minute{(span.Minutes > 1 ? "s" : "")} ago";
        }
        return " a few seconds ago";
    }

    public new void Validate()
    {
        base.Validate();

        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.RelativeImageUrlSmall);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.DateModifiedText);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.DatePublishedText);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.DatePublishedAndModificationText);

        ExceptionHelpers.ThrowIfNullOrWhiteSpace(this.DatePublishedOrModificationText);

        if (TemplateDataModel != nameof(LayoutArticleModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(LayoutArticleModel)} for the type {nameof(LayoutArticleModel)}.");
        }
    }
}
