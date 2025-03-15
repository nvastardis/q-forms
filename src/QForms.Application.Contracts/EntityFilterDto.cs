namespace QForms;

public abstract class EntityFilterDto
{
    public string? Filter { get; set; }
}

public abstract class EntityFilterDto<TKey> : EntityFilterDto
{
    public TKey? Id { get; set; }
}