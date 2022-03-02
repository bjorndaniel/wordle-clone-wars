namespace WordleCloneWars.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<Round> Rounds { get; set; } = null!;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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
        builder.Entity<Round>()
            .HasIndex(_ => new { _.Type, _.UserId, _.GameRound })
            .IsUnique();
    } 
}
