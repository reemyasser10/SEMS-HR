using Utilities.Middleware;

namespace API.Admin.Extensions
{
    public static class PipelineExtensions
    {
        public static void ConfigureStaticFiles(this WebApplication app)
        {
            _ = app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context
                       .Response
                       .Headers
                       .Append("Access-Control-Allow-Origin", "*");

                    ctx.Context
                       .Response
                       .Headers
                       .Append("Access-Control-Allow-Headers", "Origin, x-Requested-With, Content-Type, Accept");
                }
            });
        }

        public static void ConfigureSwagger(this WebApplication app)
        {
            _ = app.UseMiddleware<SwaggerAuthMiddleware>();
            _ = app.UseSwagger();

            _ = app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/Authentication/swagger.json", "Authentication");
                c.SwaggerEndpoint("/swagger/Tenant/swagger.json", "Tenant");
                c.SwaggerEndpoint("/swagger/Common/swagger.json", "Common");
               

                c.RoutePrefix = "docs";
            });
        }
    }
}
