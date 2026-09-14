using Microsoft.Extensions.Configuration;

namespace Data
{
    public class TenantConnectionResolver
    {
        private readonly DbContextFactory _dbContextFactory;
        private readonly IConfiguration _configuration;

        public TenantConnectionResolver(DbContextFactory dbContextFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContextFactory = dbContextFactory;
        }

        private string GetConnectionStringForTenant(int tenantId)
        {
            // Fetch connection string from configuration based on tenant ID
            return _configuration.GetConnectionString($"TenantDb{tenantId}");
        }

        public ApplicationDbContext CreateDbContextForTenant(int tenantId)
        {
            string connectionString = GetConnectionStringForTenant(tenantId);
            return _dbContextFactory.CreateDbContext(connectionString);
        }
    }
}
