namespace QForms.Entities;

[Serializable]
public abstract class Entity
{
    protected Entity(){}
    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Keys = {string.Join(',', GetKeys())}";
    }

    public abstract object?[] GetKeys();

    public bool EntityEquals(IEntity other)
    {
        return GetKeys().SequenceEqual(other.GetKeys());
    }
}

[Serializable]
public abstract class Entity<TKey> : Entity, IEntity<TKey>
{
    public TKey Id { get; init; }

    protected Entity(TKey id)
    {
        Id = id;
    }

    public override object?[] GetKeys()
    {
        return [Id];
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"[ENTITY: {GetType().Name}] Id = {Id}";
    }
}