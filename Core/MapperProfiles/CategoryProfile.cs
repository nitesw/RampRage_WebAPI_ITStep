using AutoMapper;
using Core.Dtos.Category;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryEntity, CategoryDto>().ReverseMap();
            CreateMap<CategoryCreateDto, CategoryEntity>()
                .ForMember(opt => opt.ImageUrl, x => x.Ignore());
            CreateMap<CategoryEditDto, CategoryEntity>()
                .ForMember(opt => opt.ImageUrl, x => x.Ignore());
        }
    }
}
