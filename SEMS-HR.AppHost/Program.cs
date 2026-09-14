IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.API_Admin>("api-admin");

builder.AddProject<Projects.WebApp_Auth>("webapp-auth");



builder.Build().Run();
