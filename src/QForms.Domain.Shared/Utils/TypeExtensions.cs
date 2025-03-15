namespace QForms.Utils;

public static class TypeExtensions
{
    public static string GetFullNameWithAssemblyName(this Type type)
    {
        return type.FullName + ", " + type.Assembly.GetName().Name;
    }

    public static bool IsAssignableTo<TTarget>(this Type type)
    {
        Check.NotNull(type, nameof(type));

        return type.IsAssignableTo(typeof(TTarget));
    }
}