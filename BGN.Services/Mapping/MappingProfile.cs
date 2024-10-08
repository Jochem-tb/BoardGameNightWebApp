using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGN.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BGN.Domain.Entities.Person, BGN.Shared.PersonDto>().ReverseMap();
            CreateMap<BGN.Domain.Entities.GameNight, BGN.Shared.GameNightDto>().ReverseMap();
            CreateMap<BGN.Domain.Entities.Category, BGN.Shared.CategoryDto>().ReverseMap();
            CreateMap<BGN.Domain.Entities.Genre, BGN.Shared.GenreDto>().ReverseMap();
            CreateMap<BGN.Domain.Entities.Game, BGN.Shared.GameDto>().ReverseMap();
            CreateMap < BGN.Domain.Entities.Gender, BGN.Shared.GenderDto>().ReverseMap();
        }
    }
}
