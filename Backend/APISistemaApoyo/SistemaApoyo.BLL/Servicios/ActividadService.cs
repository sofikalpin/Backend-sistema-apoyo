using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DAL.Repositorios.Contrato;
using SistemaApoyo.DTO;
using SistemaApoyo.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace SistemaApoyo.BLL.Servicios
{
    public class ActividadService : IActividadService
    {
        private readonly IGenericRepository<Actividad> _actividadRepositorio;
        private readonly IMapper _mapper;

        public ActividadService(IGenericRepository<Actividad> actividadRepositorio, IMapper mapper)
        {
            _actividadRepositorio = actividadRepositorio;
            _mapper = mapper;
        }

        public async Task<List<ActividadDTO>> ConsultarActividad()
        {
            try
            {
                var consulta = await _actividadRepositorio.Consultar();
                var listaActividad = await consulta.ToListAsync();
                return _mapper.Map<List<ActividadDTO>>(listaActividad);
            }
            catch(Exception ex)
            {
                throw new Exception("Error al obtener la lista de actividad.", ex);
            }
        }

        public async Task<List<ActividadDTO>> ConsultarporNombre(string nombre)
        {
            try
            {
                var Actividadquery = await _actividadRepositorio.Consultar();
                if (!string.IsNullOrEmpty(nombre))
                {
                    Actividadquery = Actividadquery.Where(v => v.Nombre == nombre);
                }


                var listaResultado = await Actividadquery.ToListAsync();
                return _mapper.Map<List<ActividadDTO>>(listaResultado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la actividad por nombre.", ex);
            }
        }


        public async Task<ActividadDTO> ObtenerPorId(int id)
        {
            try
            {
                var actividad = await _actividadRepositorio.Obtener(a => a.Idactividad == id);
                if (actividad == null)
                {
                    throw new InvalidOperationException("Consulta no encontrada.");
                }
                return _mapper.Map<ActividadDTO>(actividad);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la consulta.", ex);
            }
        }

       public async Task<List<ActividadDTO>> ObtenerPorNivel(int idNivel)
        {
            try
            {
                var actividades = await _actividadRepositorio.ObtenerTodos(a => a.Idnivel == idNivel);
                if (actividades == null)
                {
                    throw new InvalidOperationException("Consulta no encontrada.");
                }
                return _mapper.Map<List<ActividadDTO>>(actividades);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de actividades por nivel.", ex);
            }
        }
    }
}
