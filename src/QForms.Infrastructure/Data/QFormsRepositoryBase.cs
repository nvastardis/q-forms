using System.Linq.Dynamic.Core;
using QForms.EntityFrameworkCore;
using QForms.Utils;

namespace QForms.Data;


public class QFormsRepositoryBase<TEntity> : EfCoreRepository<QFormsDbContext, TEntity>
    where TEntity: class, IEntity
{
    protected string DefaultSorting { get; } = string.Empty;

    public QFormsRepositoryBase(
        QFormsDbContext dbContext)
        : base(dbContext)
    {
    }

    protected virtual IQueryable<TEntity> ApplyPagination(
        IQueryable<TEntity> query,
        string? sorting,
        int skipCount = QFormsConsts.SkipCountDefaultValue,
        int maxResultCount = QFormsConsts.MaxResultCountDefaultValue)
    {
        var sortingQuery = sorting ?? DefaultSorting;

        query = query.OrderBy(sortingQuery);

        return query.Page(skipCount, maxResultCount);
    }
}


public class QFormsRepositoryBase<TEntity, TKey> : EfCoreRepository<QFormsDbContext, TEntity, TKey>
    where TEntity: class, IEntity<TKey>
{
    protected string DefaultSorting { get; init; } = string.Empty;

    public QFormsRepositoryBase(
        QFormsDbContext dbContext)
        : base(dbContext)
    {
    }

    protected virtual IQueryable<TEntity> ApplyPagination(
        IQueryable<TEntity> query,
        string? sorting,
        int skipCount = QFormsConsts.SkipCountDefaultValue,
        int maxResultCount = QFormsConsts.MaxResultCountDefaultValue)
    {
        var sortingQuery = sorting ?? DefaultSorting;

        query = query.OrderBy(sortingQuery);

        return query.PageBy(skipCount, maxResultCount);
    }
}