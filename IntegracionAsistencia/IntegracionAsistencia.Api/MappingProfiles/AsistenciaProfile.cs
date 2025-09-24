using AutoMapper;
using IntegracionAsistencia.Application.Dtos;
using IntegracionAsistencia.Domain.Entities;

namespace IntegracionAsistencia.Api.MappingProfiles
{
    public class AsistenciaProfile : Profile
    {
        public AsistenciaProfile()
        {
            // DTO -> Entidad
            CreateMap<AsistenciaRequestDto, Asistencia>();

            // Entidad -> DTO
            CreateMap<Asistencia, AsistenciaResponseDto>()
                .ForMember(dest => dest.NombreTipoJornada, opt => opt.MapFrom(src => src.TipoJornada.Nombre))
                .ForMember(dest => dest.NombreEstadoAsistencia, opt => opt.MapFrom(src => src.EstadoAsistencia.Nombre))
                .ForMember(dest => dest.NombreTipoOrigenDato, opt => opt.MapFrom(src => src.TipoOrigenDato.Nombre));
        }
    }
}
