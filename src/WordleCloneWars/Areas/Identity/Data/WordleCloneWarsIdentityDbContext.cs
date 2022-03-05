using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WordleCloneWars.Areas.Identity.Data;

public class WordleCloneWarsIdentityDbContext : IdentityDbContext<IdentityUser>
{
    public WordleCloneWarsIdentityDbContext(DbContextOptions<WordleCloneWarsIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>()
            .HasMany(_ => _.Rounds)
            .WithOne(_ => _.User)
            .HasForeignKey(_ => _.UserId);
        builder.Entity<User>().HasIndex(_ => _.DisplayName).IsUnique();
        builder.Entity<Round>()
            .HasIndex(_ => new { _.Type, _.UserId, _.GameRound })
            .IsUnique();
    }
}
