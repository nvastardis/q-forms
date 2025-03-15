using System.Diagnostics.CodeAnalysis;

namespace QForms.Utils;

public static class EnumerableExtensions
{
    /// <summary>
    /// Determines whether the collection is null or contains no elements.
    /// </summary>
    /// <typeparam name="T">The IEnumerable type.</typeparam>
    /// <param name="enumerable">The enumerable, which may be null or empty.</param>
    /// <returns>
    ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)]this IEnumerable<T>? enumerable)
    {
        return enumerable switch
        {
            null => true,
            ICollection<T> collection => collection.Count == 0,
            _ => !enumerable.Any()
        };
    }
}