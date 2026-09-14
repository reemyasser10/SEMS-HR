using Microsoft.Extensions.Configuration;
using Utilities.Extensions;

namespace ApiHub.Services
{
    public class ServiceConfig
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }
    public class TenantConfig
    {
        public required string Code { get; set; }
        public required string Id { get; set; }
    }
    public class ApiHubService
    {
        private readonly Dictionary<string, ServiceConfig> _serviceConfigs;
        private readonly TenantConfig _tenantConfigs;

        public ApiHubService(IConfiguration configuration)
        {
            _serviceConfigs = configuration.GetSection("ApiSettings:Services").Get<Dictionary<string, ServiceConfig>>()
                ?? throw new Exception("Services configuration is missing in appsettings.json");

            _tenantConfigs = configuration.GetSection("ApiSettings:Tenant").Get<TenantConfig>()
               ?? throw new Exception("Tenant configuration is missing in appsettings.json");
        }

        public ServiceConfig GetServiceConfig(string serviceName)
        {
            return _serviceConfigs.TryGetValue(serviceName, out ServiceConfig? config)
                ? config
                : throw new Exception($"Configuration for service '{serviceName}' not found.");
        }

        public TenantConfig GetTenantConfig()
        {
            return _tenantConfigs ?? throw new Exception($"Configuration for Tenant not found.");
        }
    }

}
