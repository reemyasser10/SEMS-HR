using Data;
using Entities.DBModels.Common;
using Entities.DBModels.Tenants;
using Entities.DBModels.Users;
using Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Common;
using Repositories.Repositories.Modules.Permission;
using Repositories.Repositories.Modules.Users;
using Utilities.RequestHandler;

namespace Repositories
{
    public class UnitOfWork : GenericUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        protected readonly CryptoService _crypto;

        public UnitOfWork(ApplicationDbContext context, CryptoService crypto) : base(context)
        {
            _context = context;
            _crypto = crypto;
        }

        // Generic repository resolver (generic method)
        public GenericRepository<T> ResolveRepository<T>() where T : BaseEntity
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                var repo = new GenericRepository<T>(_context);
                _repositories[type] = repo;
            }
            return (GenericRepository<T>)_repositories[type];
        }

        // Wrapper for resolving a generic repository by runtime type
        private object ResolveRepository(Type type)
        {
            var method = typeof(UnitOfWork).GetMethod(nameof(ResolveRepository))!
                .MakeGenericMethod(type);

            return method.Invoke(this, null)!;
        }
        #region Generic repository
        #region Common
        private TranslationRepository _translationRepository;
        private GenericRepository<Lookup> _lookupRepository;
        private GenericRepository<Configuration> _configurationRepository;
        private GenericRepository<Attachment> _attachmentRepository;
        private GenericRepository<ApplicationLanguage> _applicationLanguageRepository;



        #endregion

        #region User
        private GenericRepository<Device> _deviceRepository;
        private GenericRepository<ExternalLogin> _externalLoginRepository;
        private GenericRepository<RefreshToken> _refreshTokenRepository;
        private GenericRepository<Verification> _verificationRepository;
        #endregion

        #region Tenants
        private GenericRepository<Tenant> _tenantRepository;
        private GenericRepository<TenantSetting> _tenantSettingRepository;
        #endregion
        #endregion


        #region Custom repositories
        private UserRepository _userRepository;

        #region Permission
        private UserRoleRepository _userRoleRepository;
        private RolePermissionRepository _rolePermissionRepository;
        private PermissionRepository _permissionRepository;

        #endregion


        #endregion


        #region Generic repository access
        #region Common
        public GenericRepository<ApplicationLanguage> ApplicationLanguageRepository
        {
            get
            {
                _applicationLanguageRepository ??= GetRepository<ApplicationLanguage>();
                return _applicationLanguageRepository;
            }
        }
        public TranslationRepository TranslationRepository
        {
            get
            {
                _translationRepository ??= new TranslationRepository(_context, _crypto);
                return _translationRepository;
            }
        }

        public GenericRepository<Lookup> LookUpRepository
        {
            get
            {
                _lookupRepository ??= GetRepository<Lookup>();
                return _lookupRepository;
            }
        }
       
        public GenericRepository<Configuration> ConfigurationRepository
        {
            get
            {
                _configurationRepository ??= GetRepository<Configuration>();
                return _configurationRepository;
            }
        }
        public GenericRepository<Attachment> AttachmentRepository
        {
            get
            {
                _attachmentRepository ??= GetRepository<Attachment>();
                return _attachmentRepository;
            }
        }


        #endregion

        #region User
        public GenericRepository<Device> DeviceRepository
        {
            get
            {
                _deviceRepository ??= GetRepository<Device>();
                return _deviceRepository;
            }
        }
        public GenericRepository<ExternalLogin> ExternalLoginRepository
        {
            get
            {
                _externalLoginRepository ??= GetRepository<ExternalLogin>();
                return _externalLoginRepository;
            }
        }
        public GenericRepository<RefreshToken> RefreshTokenRepository
        {
            get
            {
                _refreshTokenRepository ??= GetRepository<RefreshToken>();
                return _refreshTokenRepository;
            }
        }

        public GenericRepository<Verification> VerificationRepository
        {
            get
            {
                _verificationRepository ??= GetRepository<Verification>();
                return _verificationRepository;
            }
        }
        #endregion

        #region Tenants
        public GenericRepository<Tenant> TenantRepository
        {
            get
            {
                _tenantRepository ??= GetRepository<Tenant>();
                return _tenantRepository;
            }
        }
        public GenericRepository<TenantSetting> TenantSettingRepository
        {
            get
            {
                _tenantSettingRepository ??= GetRepository<TenantSetting>();
                return _tenantSettingRepository;
            }
        }
        #endregion

        #endregion


        #region Custom repository access
        public UserRepository UserRepository
        {
            get
            {
                _userRepository ??= new UserRepository(_context);
                return _userRepository;
            }
        }

        #region Permission
        public PermissionRepository PermissionRepository
        {
            get
            {
                _permissionRepository ??= new PermissionRepository(_context, _crypto);
                return _permissionRepository;
            }
        }

        public UserRoleRepository UserRoleRepository
        {
            get
            {
                _userRoleRepository ??= new UserRoleRepository(_context);
                return _userRoleRepository;
            }
        }
        public RolePermissionRepository RolePermissionRepository
        {
            get
            {
                _rolePermissionRepository ??= new RolePermissionRepository(_context);
                return _rolePermissionRepository;
            }
        }
        #endregion
        #endregion

    }
}
