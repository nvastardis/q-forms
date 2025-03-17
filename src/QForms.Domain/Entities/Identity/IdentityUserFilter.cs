using QForms.Utils;

namespace QForms.Identity;

public class IdentityUserFilter: EntityFilter<ApplicationUser, Guid>
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }

    public override IQueryable<ApplicationUser> ApplyFilter(IQueryable<ApplicationUser> query)
    {
        query = query
                .WhereIf(!Username.IsNullOrEmpty(), e => e.UserName == Username)
                .WhereIf(!Email.IsNullOrEmpty(), e => e.Email == Email)
                .WhereIf(!Name.IsNullOrEmpty(), e => e.Name == Name)
                .WhereIf(!Surname.IsNullOrEmpty(), e => e.Surname == Surname);

        return query;
    }
}