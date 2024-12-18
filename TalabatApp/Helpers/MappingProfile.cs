using AutoMapper;
using TalabatApp.Core.Entities;
using TalabatApp.Dtos;

namespace TalabatApp.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(P => P.Brand, o => o.MapFrom(s => s.Brand.Name))
                .ForMember(P => P.Category, o => o.MapFrom(s => s.Category.Name));


        }
    }
}
