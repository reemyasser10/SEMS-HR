using Data;
using Entities.DBModels.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.DTOs.CommonModels;

namespace Repositories.Repositories.Modules.Users
{
    public class UserRepository : GenericRepository<User>
    {
        private readonly GenericRepository<Device> _deviceRepository;
        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _deviceRepository = new GenericRepository<Device>(context);
        }
        private IQueryable<User> GetFilteredQuery(UserParameters parameters)
        {
            IQueryable<User> query = GetAll();

          //  query = query.Where(u => u.Teacher != null);

            if (!string.IsNullOrWhiteSpace(parameters.Name))
            {
                query = query.Where(u => u.FullName.Contains(parameters.Name));
            }

            if (parameters.IsApproved.HasValue)
            {
                query = query.Where(u => u.IsApproved == parameters.IsApproved.Value);
            }

            if (parameters.IsLockedOut.HasValue)
            {
                query = query.Where(u => u.IsLockedOut == parameters.IsLockedOut.Value);
            }

            return query.OrderBy(u => u.FullName);
        }
        public IQueryable<User> GetUsers(UserParameters parameters)
        {
            IQueryable<User> query = GetAll();

            if (parameters.Id > 0)
            {
                query = query.Where(a => a.Id == parameters.Id);
            }
            return query;
        }


        public IQueryable<CustomLookUpDto> GetAssignableUsersDto(UserParameters parameters)
        {
            return GetFilteredQuery(parameters)
                .Select(u => new CustomLookUpDto
                {
                    Id = u.Id,
                    Name = u.FullName
                });
        }
    }
}
