using AutoMapper;
using Shared.DTOs.CommonModels;
using Shared.VMs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ApiHub.Services.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TranslationFilter, TranslationParameters>();
        }
    }
}
