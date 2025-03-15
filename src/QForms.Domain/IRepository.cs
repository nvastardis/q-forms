using System.Linq.Expressions;

namespace QForms;

public interface IRepository<TEntity>
    where TEntity: class, IEntity
{
    Task<TEntity> InsertAsync(
        TEntity entity,
        bool autoSave = true,
        CancellationToken cancellationToken = default);

    Task InsertManyAsync(
        IEnumerable<TEntity> entities,
        bool autoSave = true,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<IEnumerable<TEntity>?> FindManyAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    Task<List<TEntity>> GetListAsync(
        EntityFilter<TEntity>? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListAsync(
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        EntityFilter<TEntity>? filter = null,
        CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetPagedListAsync(
        int skipCount,
        int maxResultCount,
        string? sorting,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync(
        TEntity entity,
        bool autoSave = true,
        CancellationToken cancellationToken = default);

    Task UpdateManyAsync(
        IEnumerable<TEntity> entities,
        bool autoSave = true,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        TEntity entity,
        bool autoSave = true,
        CancellationToken cancellationToken = default
    );

    Task DeleteManyAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool autoSave = true,
        CancellationToken cancellationToken = default
    );

    Task<IQueryable<TEntity>> WithDetailsAsync(
        CancellationToken cancellationToken = default);

    Task<IQueryable<TEntity>> WithDetailsAsync(
        Expression<Func<TEntity, object>>[] propertySelectors,
        CancellationToken cancellationToken = default);

    Task<IQueryable<TEntity>> GetQueryableAsync(
        CancellationToken cancellationToken = default);
}

public interface IRepository<TEntity, in TKey> : IRepository<TEntity>
    where TEntity : class, IEntity<TKey>
{
    Task<TEntity> GetAsync(
        TKey id,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FindAsync(
        TKey id,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        TKey id,
        bool autoSave = true,
        CancellationToken cancellationToken = default);

    Task DeleteManyAsync(
        IEnumerable<TKey> ids,
        bool autoSave = true,
        CancellationToken cancellationToken = default);
}
