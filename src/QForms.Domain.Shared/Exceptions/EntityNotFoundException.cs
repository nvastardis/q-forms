namespace QForms.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException()
    {

    }

    public EntityNotFoundException(Type entityType)
        : this(entityType, null, null)
    {

    }

    public EntityNotFoundException(Type entityType, object? id)
        : this(entityType, id, null)
    {

    }

    public EntityNotFoundException(Type entityType, object? id, Exception? innerException)
        : base(
            id == null
                ? $"There is no such entity with the given id. Entity type: {entityType.FullName}, id: {id}"
                : $"There is no such entity. Entity type: {entityType.FullName}",
            innerException)
    {
    }

    public EntityNotFoundException(string message)
        : base(message)
    {

    }

    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {

    }
}