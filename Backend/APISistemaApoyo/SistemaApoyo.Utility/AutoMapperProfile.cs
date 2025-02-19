using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SistemaApoyo.DTO;
using SistemaApoyo.Model;

namespace SistemaApoyo.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Actividad
            CreateMap<Actividad, ActividadDTO>().ReverseMap();
            #endregion Actividad

            #region Articulo
            CreateMap<Articulo, ArticuloDTO>().ReverseMap();
            #endregion Articulo

            #region Chat
            CreateMap<Chat, ChatDTO>()
                .ForMember(dest => dest.Mensajes,
                           opt => opt.MapFrom(src => src.Mensajes))
                .ReverseMap();
            #endregion Chat

            #region Mensaje
            CreateMap<Mensaje, MensajeDTO>().ReverseMap();
            #endregion Mensaje

            #region Consulta
            CreateMap<Consulta, ConsultaDTO>().ReverseMap();
                
            #endregion Consulta

            #region Examen
            CreateMap<Examen, ExamenDTO>().ReverseMap();
            #endregion Examen

            #region Foro
            CreateMap<Foro, ForoDTO>().ReverseMap();
            #endregion Foro

            #region Menu
            CreateMap<Menu, MenuDTO>().ReverseMap();
            #endregion Menu

            #region Nivel
            CreateMap<Nivel, NivelDTO>().ReverseMap();
            #endregion Nivel

            #region Respuesta
            CreateMap<Respuesta, RespuestaDTO>().ReverseMap();
            #endregion Respuesta

            #region Rol
            CreateMap<Rol, RolDTO>().ReverseMap();
            #endregion Rol

            #region Usuario
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.Idusuario, opt => opt.MapFrom(src => src.Idusuario))
                .ForMember(dest => dest.Nombrecompleto, opt => opt.MapFrom(src => src.Nombrecompleto))
                .ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Correo))
                .ReverseMap();
            CreateMap<Usuario, SesionDTO>()
                .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => src.Idusuario))
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.Nombrecompleto))
                .ForMember(dest => dest.Correo, opt => opt.MapFrom(src => src.Correo))
                .ForMember(destino => destino.Rol,
                           opt => opt.MapFrom(origen => origen.IdrolNavigation.Nombre)); 
            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(destino => destino.IdrolNavigation,
                           opt => opt.Ignore());
            #endregion Usuario

            #region Reseña
            CreateMap<Reseña, ReseñaAlumnoDTO>()
    .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.IdusuarioNavigation.Nombrecompleto))
    .ForMember(dest => dest.NombreProfesor, opt => opt.MapFrom(src => src.IdProfesorNavigation.Nombrecompleto))
    .ReverseMap();

            CreateMap<ReseñaAlumnoDTO, Reseña>();

            CreateMap<Reseñapagina, ReseñaDTO>()
                .ReverseMap();
            #endregion Reseña

        }
    }
}
