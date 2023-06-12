using Core.DAL.Models;
using Core.DALEF.ContextFactory.Seeds;
using Core.DALEF.ContextFactory.Seeds.Dummy;
using IPSTemplate.Dal.Models;
using IPSTemplate.Dal.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.DALEF.ContextFactory;

internal static class ModelBuilderExtensions
{
    public static void ConfigureIdentity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TEIdentityUser>().ToTable("User", "Identity");
        modelBuilder.Entity<TEIdentityRole>().ToTable("Role", "Identity");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRole", "Identity");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaim", "Identity");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogin", "Identity");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaim", "Identity");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserToken", "Identity");
    }
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SYSetting>().HasData(new SYSettingSeed());
        modelBuilder.Entity<TEEntity>().HasData(new TEEntitySeed());
        modelBuilder.Entity<TEIdentityUser>().HasData(new TEIdentityUserSeed());
        modelBuilder.Entity<TEIdentityRole>().HasData(new TEIdentityRoleSeed());
        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new TEIdentityUserRoleSeed());

        if (EnvironmentHelper.IsDevelopment())
        {
            modelBuilder.SeedDummyData();
        }
    }

    private static void SeedDummyData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TEEntity>().HasData(new TEEntityDummy());
    }
}
