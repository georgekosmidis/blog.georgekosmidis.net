using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Blog.Builder.Exceptions;

internal static class ExceptionHelpers
{

    public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            Throw($"{paramName} is null or filled with whitespaces only.");
        }

    }

    public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is null)
        {
            Throw($"{paramName} is null.");
        }

    }

    public static void ThrowIfNullOrEmpty<T>([NotNull] ICollection<T>? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        ThrowIfNull(argument, paramName);

        if (!argument.Any())
        {
            Throw($"{paramName} doesn't has any items.");
        }

    }

    [DoesNotReturn]
    private static void Throw(string? message) =>
        throw new Exception(message);
}
