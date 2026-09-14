using ApiHub.Services.Common;
using ApiHub.Services.HR.Authentication;
using ApiHub.Services.HR.Common;
using AutoMapper;


namespace ApiHub.Services.HR
{
    public class HRServices
    {
        private readonly ApiBase _apiHub;
        private readonly string _serviceName = "HR";
        private readonly string _accessToken;
        public IMapper _mapper { get; set; }

        public HRServices(ApiBase apiHub, string accessToken)
        {
            _apiHub = apiHub;
            _accessToken = accessToken;
        }

        public HRServices(ApiBase apiHub, string accessToken, IMapper mapper)
        {
            _apiHub = apiHub;
            _accessToken = accessToken;
            _mapper = mapper;
        }


        public HRServices(ApiBase apiHub)
        {
            _apiHub = apiHub;
        }

        #region Private

        #region Authentication

        private ForgetPasswordServices _forgetPasswordServices;
        private LoginServices _loginServices;
        private VerificationServices _verificationServices;
        private RegisterServices _registerServices;
        private UserServices _userServices;

        #endregion

        #region Common

        private AttachmentServices _attachmentServices;
        private LookupServices _lookupServices;
        private ApplicationLanguageServices _applicationLanguageServices;
        private TranslationServices _translationServices;

        #endregion

        #region Permission
        private ApiHub.Services.HR.Permission.FunctionalityServices _functionalityServices;
        private ApiHub.Services.HR.Permission.ResourceServices _resourceServices;
        private ApiHub.Services.HR.Permission.RoleServices _roleServices;
        private ApiHub.Services.HR.Permission.AdministratorServices _administratorServices;

        public ApiHub.Services.HR.Permission.FunctionalityServices FunctionalityServices { get => _functionalityServices ??= new ApiHub.Services.HR.Permission.FunctionalityServices(_apiHub, _serviceName, _accessToken); }
        public ApiHub.Services.HR.Permission.ResourceServices ResourceServices { get => _resourceServices ??= new ApiHub.Services.HR.Permission.ResourceServices(_apiHub, _serviceName, _accessToken); }
        public ApiHub.Services.HR.Permission.RoleServices RoleServices { get => _roleServices ??= new ApiHub.Services.HR.Permission.RoleServices(_apiHub, _serviceName, _accessToken); }
        public ApiHub.Services.HR.Permission.AdministratorServices AdministratorServices { get => _administratorServices ??= new ApiHub.Services.HR.Permission.AdministratorServices(_apiHub, _serviceName, _accessToken); }
        #endregion


        #endregion


        #region Access

        #region Authentication

        public ForgetPasswordServices ForgetPasswordServices
        {
            get
            {
                _forgetPasswordServices ??= new ForgetPasswordServices(_apiHub, _serviceName, _accessToken);
                return _forgetPasswordServices;
            }
        }

        public LoginServices LoginServices
        {
            get
            {
                _loginServices ??= new LoginServices(_apiHub, _serviceName);
                return _loginServices;
            }
        }

        public VerificationServices VerificationServices
        {
            get
            {
                _verificationServices ??= new VerificationServices(_apiHub, _serviceName);
                return _verificationServices;
            }
        }

        public RegisterServices RegisterServices
        {
            get
            {
                _registerServices ??= new RegisterServices(_apiHub, _serviceName);
                return _registerServices;
            }
        }

        public UserServices UserServices
        {
            get
            {
                _userServices ??= new UserServices(_apiHub, _serviceName, _accessToken);
                return _userServices;
            }
        }

        #endregion

        #region Common

     
        public AttachmentServices AttachmentServices
        {
            get
            {
                _attachmentServices ??= new AttachmentServices(_apiHub, _serviceName, _accessToken);
                return _attachmentServices;
            }
        }
        public ApplicationLanguageServices ApplicationLanguageServices
        {
            get
            {
                _applicationLanguageServices ??= new ApplicationLanguageServices(_apiHub, _serviceName);
                return _applicationLanguageServices;
            }
        }
        public LookupServices LookupServices
        {
            get
            {
                _lookupServices ??= new LookupServices(_apiHub, _serviceName, _accessToken);
                return _lookupServices;
            }
        }
     
        public TranslationServices TranslationServices
        {
            get
            {
                _translationServices ??= new TranslationServices(_apiHub, _serviceName,_accessToken);
                return _translationServices;
            }
        }

        #endregion

     

        #endregion
    }
}




