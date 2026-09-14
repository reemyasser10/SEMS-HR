using API.Admin.Controllers;
using AutoMapper;
using Data;
using Entities.Enums;
using Identity;
using Identity.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Utilities.Constants;
using Utilities.EmailServices;
using Utilities.Extensions;
using Utilities.RequestHandler;
using Utilities.Settings;
namespace API.Admin.Areas.AuthenticationArea.Controllers
{
    public class AuthController : ExtendControllerBase
    {
        protected readonly JwtUtil _jwtUtil;
        public IWebHostEnvironment _webHostEnvironment { get; set; }

        public AuthController(TenantConnectionResolver tenantService, JwtUtil jwtUtil,
                                         LinkGenerator linkGenerator,
                                         IMapper mapper, IOptions<AppSettings> appSettings, CryptoService crypto, IWebHostEnvironment webHostEnvironment) : base(tenantService, jwtUtil, linkGenerator, mapper, appSettings, crypto)
        {
            _jwtUtil = jwtUtil;
            _webHostEnvironment = webHostEnvironment;
        }


        protected async Task SendVerification(string code, int? fk_User = null)
        {
           

        }


        protected void SetToken(TokenResponse token)
        {
            Response.Headers.Append(key: HeadersConstants.Expires, value: token.Expires.CommaEncode());
            Response.Headers.Append(key: HeadersConstants.Authorization, value: token.JwtToken.CommaEncode());
        }
        protected void SetRefresh(TokenResponse token)
        {
            Response.Headers.Append(key: HeadersConstants.SetRefresh, value: token.ToString());
        }
       
        protected string GetPortalUrl(string portal)
        {
            
            int tenantId = TenantId;

            if (_webHostEnvironment.IsDevelopment())
            {
                portal = portal + "_Dev";
            }
          
            Entities.DBModels.Common.Configuration? configDB = UnitOfWork.ConfigurationRepository
                                                                         .Find(a => (a.Fk_Tenant == tenantId || a.Fk_Tenant == null) &&
                                                                                     a.Module == ModuleEnum.PortalsUrl &&
                                                                                     a.Key == portal)
                                                                         .OrderByDescending(a => a.Fk_Tenant != null)
                                                                         .FirstOrDefault();

            return configDB?.Value ?? "";
          
        }
    }
}
