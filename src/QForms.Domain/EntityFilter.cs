namespace QForms;

public abstract class EntityFilter<TEntity>
    where TEntity: class, IEntity
{
    public string? Filter { get; set; }
    
    public virtual IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> query)
        => throw new NotImplementedException();
}

public abstract class EntityFilter<TEntity, TKey> : EntityFilter<TEntity>
    where TEntity: class, IEntity<TKey>
{
    public TKey? Id { get; set; }
}