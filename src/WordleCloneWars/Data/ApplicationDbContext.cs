﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WordleCloneWars.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<Round> Rounds { get; set; } = null!;
    public DbSet<GameInfo> GameInfos { get; set; } = null!;
    
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
        builder.Entity<User>().HasIndex(_ => _.DisplayName).IsUnique();
        builder.Entity<Round>()
            .HasIndex(_ => new { _.Type, _.UserId, _.GameRound })
            .IsUnique();
        builder.Entity<Round>()
            .Property(_ => _.CompletedDateTime)
            .HasConversion(new DateTimeOffsetToBinaryConverter());
    } 
}
