using Microsoft.EntityFrameworkCore;
using YellowHouseStudio.LifeOrbit.Domain.Family;

namespace YellowHouseStudio.LifeOrbit.Application.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<FamilyMember> FamilyMembers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FamilyMember>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Age).IsRequired();

            entity.OwnsMany(e => e.Allergies, nav =>
            {
                nav.WithOwner().HasForeignKey("FamilyMemberId");
                nav.Property<Guid>("Id");
                nav.HasKey("Id");
            });

            entity.OwnsMany(e => e.SafeFoods, nav =>
            {
                nav.WithOwner().HasForeignKey("FamilyMemberId");
                nav.Property<Guid>("Id");
                nav.HasKey("Id");
            });

            entity.OwnsMany(e => e.FoodPreferences, nav =>
            {
                nav.WithOwner().HasForeignKey("FamilyMemberId");
                nav.Property<Guid>("Id");
                nav.HasKey("Id");
            });
        });
    }
}