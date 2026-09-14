using Data.Configurations;
using Entities.DBModels.Common;
using Entities.DBModels.Permission;
using Entities.DBModels.Tenants;
using Entities.DBModels.Users;
using Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data
{
    public class MigrationDbContext : ApplicationDbContext
    {
        public MigrationDbContext(string connectionString) : base(connectionString)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Index
         
            #region Users

            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.Fk_Tenant, u.UserName })
                .IsUnique()
                .HasFilter("[SoftDelete] = 0");

            _ = modelBuilder.Entity<ExternalLogin>()
                        .HasIndex(a => new { a.Fk_Tenant, a.ProviderKey })
                        .IsUnique();

            _ = modelBuilder.Entity<RefreshToken>()
                        .HasIndex(a => new { a.Fk_Tenant, a.Token })
                        .IsUnique();

            _ = modelBuilder.Entity<Device>()
                        .HasIndex(a => new { a.Fk_User, a.FirebaseToken })
                        .IsUnique();

            #endregion

            #region Tenant

            _ = modelBuilder.Entity<TenantSetting>()
                        .HasIndex(a => new { a.Fk_Tenant, a.Key })
                        .IsUnique();

            #endregion

            #region Permission

            _ = modelBuilder.Entity<Role>()
                        .HasIndex(a => new { a.Fk_Tenant, a.Name })
                        .IsUnique();


            _ = modelBuilder.Entity<UserRole>()
                        .HasIndex(a => new { a.Fk_User, a.Fk_Role })
                        .IsUnique();

            #endregion

            #region Common

            _ = modelBuilder.Entity<Configuration>()
                        .HasIndex(a => new { a.Fk_Tenant, a.Module, a.Key })
                        .IsUnique();

            _ = modelBuilder.Entity<EmailTemplate>()
                        .HasIndex(a => new { a.Fk_Tenant, a.Name })
                        .IsUnique();

            _ = modelBuilder.Entity<Lookup>()
                        .HasIndex(a => new { a.Fk_Tenant, a.EntityType, a.Name })
                        .IsUnique();

            _ = modelBuilder.Entity<Lookup>()
                        .HasOne(a => a.LookupLang)
                        .WithOne(a => a.Source)
                        .HasForeignKey<LookupLang>(a => a.Fk_Source)
                        .OnDelete(DeleteBehavior.Cascade);

            _ = modelBuilder.Entity<LookupLang>()
                        .HasIndex(a => a.Fk_Source)
                        .IsUnique();

            _ = modelBuilder.Entity<Translation>()
                        .HasIndex(a => new { a.Fk_Tenant, a.LanguageCode, a.ApplicationEnum, a.RowText })
                        .IsUnique();

            #endregion

            #endregion

            #region Settings

            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (IMutableForeignKey foreignKey in entityType.GetForeignKeys())
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.Restrict; // Apply globally
                }
            }
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes()
              .Where(t => t.ClrType.IsSubclassOf(typeof(BaseEntity))))
            {
                _ = modelBuilder.Entity(
                    entityType.Name,
                    x =>
                    {
                        _ = x.Property("CreatedAt")
                            .HasDefaultValueSql("getutcdate()");
                    });
            }
            #endregion
         
     



            #region Seed Data
            _ = modelBuilder.ApplyConfiguration(new LookupConfiguration());
            #endregion
        }
    }
}
