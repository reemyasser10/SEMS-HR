using AutoMapper;
using Entities.DBModels.Common;
using Entities.DBModels.Users;
using Entities.Shared;
using Identity.Entities;
using Shared.DTOs.CommonModels;
using Shared.DTOs.SharedModels;


namespace API.Admin.MappingProfileCls
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
         

            #region User Models

            #region User

            _ = CreateMap<UserRegistrationDto, User>();
            _ = CreateMap<CreateUserDto, User>();

            #endregion

            #endregion

            #region Common

            _ = CreateMap<CreateLookupDto, Lookup>().ReverseMap();
            #endregion

       
        }
    }
}


