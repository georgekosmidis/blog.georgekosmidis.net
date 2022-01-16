using Blog.Builder.Exceptions;
using System.Text;

namespace Blog.Builder.Models.Templates;

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

    //todo: too many conditions, find me some time to make it smarter or cover it with unit tests
    private static string SpanCalculation(TimeSpan span)
    {
        var result = new StringBuilder();

        var years = Math.Round(span.Days / 365d);
        var months = Math.Max(0, Math.Round((span.Days - years * 365) / 30));
        var weeks = Math.Max(0, Math.Round((span.Days - years * 365 - months * 30) / 7));
        var days = Math.Max(0, span.Days - weeks * 7 - months * 30 - years * 365);

        if (years > 0)
        {
            result.Append(years);
            result.Append(years == 1 ? " year" : " years");
        }
        if (months > 0)
        {
            if (years > 0)
            {
                result.Append(" and ");
            }
            result.Append(months);
            result.Append(months == 1 ? " month" : " months");
        }
        //don't show days for more than a year old articles
        if (years == 0)
        {
            if (weeks > 0)
            {
                if (months > 0)
                {
                    result.Append(" and ");
                }
                result.Append(weeks);
                result.Append(weeks == 1 ? " week" : " weeks");
            }
            //don't show days if we have weeks or months
            if (months == 0 && weeks == 0 && days > 0)
            {
                if (months > 0)
                {
                    result.Append(" and ");
                }
                result.Append(days);
                result.Append(days == 1 ? " day" : " days");
            }
        }

        //if no years, months or days, then check smaller time parts
        if (result.Length == 0)
        {
            if (span.Hours > 0)
            {
                result.Append(span.Hours);
                result.Append(span.Hours == 1 ? " hour" : " hours");
            }
            if (span.Hours == 0 && span.Minutes > 0)
            {
                result.Append(span.Minutes);
                result.Append(span.Minutes == 1 ? " minute" : " minutes");
            }
        }

        if (result.Length == 0)
        {
            return "a few seconds ago";
        }
        else
        {
            return result.Append(" ago").ToString();
        }
    }

    public new void Validate()
    {
        base.Validate();

        ExceptionHelpers.ThrowIfNullOrWhiteSpace(RelativeImageUrlSmall);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(DateModifiedText);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(DatePublishedText);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(DatePublishedAndModificationText);

        ExceptionHelpers.ThrowIfNullOrWhiteSpace(DatePublishedOrModificationText);

        if (TemplateDataModel != nameof(LayoutArticleModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(LayoutArticleModel)} for the type {nameof(LayoutArticleModel)}.");
        }
    }
}
