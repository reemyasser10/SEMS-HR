using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Utilities.Constants;
using Utilities.PaginationHelper;
using Utilities.ResponseHandler;

namespace Utilities.ActionFilters
{
    public class SwaggerOperations : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= [];

            ControllerActionDescriptor? ActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (ActionDescriptor != null)
            {
                bool allowAnonymous = ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();

#if !DEBUG
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = HeadersConstants.AppKey,
                    In = ParameterLocation.Header,
                    Required = !allowAnonymous,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                });
#endif

                if (!allowAnonymous)
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = HeadersConstants.AuthorizationToken,
                        In = ParameterLocation.Header,
                        Required = true,
                        Schema = new OpenApiSchema
                        {
                            Type = "string"
                        }
                    });
                }

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = HeadersConstants.TenantId,
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Format = "int32"
                    },
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = HeadersConstants.Culture,
                    In = ParameterLocation.Header,
                    Description = "en | ar | ****",
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                    },
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = HeadersConstants.DeviceId,
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema
                    {
                        Type = "integer",
                        Format = "int32"
                    },
                });
            }

            OpenApiSchema statusSchema = new()
            {
                Type = "string",
                Example = new OpenApiString(new ResponseStatus().ToString())
            };

            foreach (KeyValuePair<string, OpenApiResponse> response in operation.Responses)
            {
                response.Value.Headers.Add(HeadersConstants.Status, new OpenApiHeader
                {
                    Schema = statusSchema
                });
            }

            OpenApiSchema pagingSchema = new()
            {
                Type = "string",
                Example = new OpenApiString(MetaData.PaginationMetaData())
            };

            foreach (KeyValuePair<string, OpenApiResponse> response in operation.Responses)
            {
                response.Value.Headers.Add(HeadersConstants.Pagination, new OpenApiHeader
                {
                    Schema = pagingSchema
                });
            }
        }
    }
}
