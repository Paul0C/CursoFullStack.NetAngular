using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CursoWebApi.Application.Dtos;
using CursoWebApi.Domain;
using CursoWebApi.Domain.Identity;

namespace CursoWebApi.Application.Helpers
{
    public class CursoWebApiProfile : Profile
    {
        public CursoWebApiProfile(){
            CreateMap<Evento, EventoDto>().ReverseMap();
            CreateMap<Lote, LoteDto>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
            CreateMap<Palestrante, PalestranteDto>().ReverseMap();
            
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
        }   
    }
}