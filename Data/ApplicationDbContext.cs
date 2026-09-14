using Entities.Constants;
using Entities.DBModels.Common;
using Entities.DBModels.Permission;
using Entities.DBModels.Tenants;
using Entities.DBModels.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;

        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
         


     
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

            if (!optionsBuilder.IsConfigured)
            {
                _ = optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        #region Tenants
        public DbSet<Tenant> Tenant { get; set; }
        public DbSet<TenantSetting> TenantSetting { get; set; }
        #endregion


        #region Common
        public DbSet<Attachment> Attachment { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<EmailTemplate> EmailTemplate { get; set; }
        public DbSet<Lookup> Lookup { get; set; }
        public DbSet<LookupLang> LookupLang { get; set; }
        public DbSet<Translation> Translation { get; set; }
        public DbSet<ApplicationLanguage> ApplicationLanguage { get; set; }
       
        #endregion


        #region Users
        public DbSet<User> User { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<ExternalLogin> ExternalLogin { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Verification> Verification { get; set; }
        #endregion

        #region Permission
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Functionality> Functionalities { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        #endregion

     

    }
}



