using System.Linq.Expressions;

namespace QForms.Utils;

public static class QueryableExtensions
{
    public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int skipCount, int maxResultCount)
    {
        Check.NotNull(query, nameof(query));

        return query.Skip(skipCount).Take(maxResultCount);
    }

    public static TQueryable PageBy<T, TQueryable>(this TQueryable query, int skipCount, int maxResultCount)
        where TQueryable : IQueryable<T>
    {
        Check.NotNull(query, nameof(query));

        return (TQueryable)query.Skip(skipCount).Take(maxResultCount);
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
    {
        Check.NotNull(query, nameof(query));

        return condition
            ? query.Where(predicate)
            : query;
    }

    public static TQueryable WhereIf<T, TQueryable>(this TQueryable query, bool condition, Expression<Func<T, bool>> predicate)
        where TQueryable : IQueryable<T>
    {
        Check.NotNull(query, nameof(query));

        return condition
            ? (TQueryable)query.Where(predicate)
            : query;
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
    {
        Check.NotNull(query, nameof(query));

        return condition
            ? query.Where(predicate)
            : query;
    }

    public static TQueryable WhereIf<T, TQueryable>(this TQueryable query, bool condition, Expression<Func<T, int, bool>> predicate)
        where TQueryable : IQueryable<T>
    {
        Check.NotNull(query, nameof(query));

        return condition
            ? (TQueryable)query.Where(predicate)
            : query;
    }

    public static TQueryable OrderByIf<T, TQueryable>(this TQueryable query, bool condition, string sorting)
        where TQueryable : IQueryable<T>
    {
        Check.NotNull(query, nameof(query));

        return condition
            ? (TQueryable)System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(query, sorting)
            : query;
    }
}