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
                c.SwaggerEndpoint("/swagger/AcademicData/swagger.json", "AcademicData");
                c.SwaggerEndpoint("/swagger/Enterprise/swagger.json", "Enterprise");
                c.SwaggerEndpoint("/swagger/Children/swagger.json", "Children");
                c.SwaggerEndpoint("/swagger/Application/swagger.json", "Application");
                c.SwaggerEndpoint("/swagger/Payment/swagger.json", "Payment");
                c.SwaggerEndpoint("/swagger/Attendance/swagger.json", "Attendance");
                c.SwaggerEndpoint("/swagger/Teacher/swagger.json", "Teacher");
                c.SwaggerEndpoint("/swagger/DynamicForm/swagger.json", "DynamicForm");
                c.SwaggerEndpoint("/swagger/Communication/swagger.json", "Communication");
                c.SwaggerEndpoint("/swagger/Event/swagger.json", "Event");
                c.SwaggerEndpoint("/swagger/Incident/swagger.json", "Incident");
                c.SwaggerEndpoint("/swagger/Birthday/swagger.json", "Birthday");
                c.SwaggerEndpoint("/swagger/Curriculum/swagger.json", "Curriculum");
                c.SwaggerEndpoint("/swagger/Admission/swagger.json", "Admission");
                c.SwaggerEndpoint("/swagger/Activity/swagger.json", "Activity");
                c.SwaggerEndpoint("/swagger/Assessment/swagger.json", "Assessment");
                c.SwaggerEndpoint("/swagger/NewsFeed/swagger.json", "NewsFeed");

                c.RoutePrefix = "docs";
            });
        }
    }
}
