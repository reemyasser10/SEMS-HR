using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Utilities.Constants;

namespace Utilities.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireAccessTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
            {
                return;
            }

            ClaimsPrincipal user = context.HttpContext.User;
            string? accessToken = user.FindFirst(PortalConstants.AccessToken)?.Value;

            if (string.IsNullOrEmpty(accessToken))
            {
                string path = context.HttpContext.Request.Path.ToUriComponent();
                string query = context.HttpContext.Request.QueryString.ToUriComponent();
                string returnUrl = Uri.EscapeDataString(path + query);

                context.Result = new RedirectResult($"/auth/Refresh?returnUrl={returnUrl}");
            }

            base.OnActionExecuting(context);
        }
    }
}
