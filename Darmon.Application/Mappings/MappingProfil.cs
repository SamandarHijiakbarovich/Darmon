using AutoMapper;
using Darmon.Application.DTOs;
using Darmon.Application.DTOs.User;
using Darmon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darmon.Application.Mappings;

public class MappingProfil:Profile
{
    public MappingProfil()
    {
        CreateMap<UserRequestDto, User>();
        CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

      
    }
}
