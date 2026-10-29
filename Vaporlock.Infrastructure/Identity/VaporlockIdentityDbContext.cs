
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Vaporlock.Infrastructure.Identity;

public class VaporlockIdentityDbContext
    : IdentityDbContext<ApplicationUser>
{
    public VaporlockIdentityDbContext(
        DbContextOptions<VaporlockIdentityDbContext> options)
        : base(options)
    {
    }
}
