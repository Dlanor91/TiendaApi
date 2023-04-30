using API.Dtos;
using AutoMapper;
using CORE.Entities;

namespace API.Profiles
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            //llamo cadaclase con su dto
            CreateMap<Producto, ProductoDto>()
                .ReverseMap();
            CreateMap<Categoria, CategoriaDto>()
                .ReverseMap();
            CreateMap<Marca, MarcaDto>()
                .ReverseMap();

            //Mapeo Complejo de PorductoList
            CreateMap<Producto, ProductoListDto>()
            .ForMember(dest => dest.Marca, origen => origen.MapFrom(origen => origen.Marca.Nombre))
            .ForMember(dest => dest.Categoria, origen => origen.MapFrom(origen => origen.Categoria.Nombre))
            .ReverseMap()
            .ForMember(origen => origen.Categoria, dest => dest.Ignore())
            .ForMember(origen => origen.Marca, dest => dest.Ignore());

            CreateMap<Producto, ProductoAddUpdateDto>()
                .ReverseMap()
                .ForMember(origen => origen.Categoria, dest => dest.Ignore())
                .ForMember(origen => origen.Marca, dest => dest.Ignore());
        }
    }
}
