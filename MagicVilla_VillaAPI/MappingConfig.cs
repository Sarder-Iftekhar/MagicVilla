using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>().ReverseMap();  //REVERSE MAP MEANS CreateMap<VillaDTO, Villa>() also, reverse map also perform
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();

            //mapping villa naumber 
            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();  //REVERSE MAP MEANS CreateMap<VillaDTO, Villa>() also, reverse map also perform
            CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
        }

    }
}
