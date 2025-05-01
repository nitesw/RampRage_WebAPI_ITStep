using AutoMapper;
using Core.Dtos.User;
using Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegisterDto, UserEntity>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.UserName));
        }
    }
}
