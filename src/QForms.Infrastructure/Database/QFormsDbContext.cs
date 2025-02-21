using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QForms.Identity;

namespace QForms.Database;

public class QFormsDbContext: IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public QFormsDbContext(DbContextOptions<QFormsDbContext> options) :
        base(options)
    { }
}