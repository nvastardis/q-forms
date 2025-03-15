using Microsoft.AspNetCore.Identity;

namespace QForms.Identity;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser<Guid>, IEntity<Guid>
{
    public string? Name { get; set; }
    public string? Surname { get; set; }

    public ApplicationUser() : base() {}

    public ApplicationUser(
        string username,
        string name,
        string surname) : base(username)
    {
        Name = name;
        Surname = surname;
    }

    /// <inheritdoc/>
    public object?[] GetKeys()
    {
        return [Id];
    }
}