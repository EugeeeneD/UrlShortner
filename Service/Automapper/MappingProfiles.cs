using AutoMapper;
using Domain.Dto.Request;
using Domain.Dto.Response;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Automapper
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            CreateMap<UserRegistrationDto, User>();
            CreateMap<ShortenedUrl, UrlShortenedDto>();
            CreateMap<User, UserDto>();
        }
    }
}
