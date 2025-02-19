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
    public class ProfesorActividadService : IProfesorActividad
    {
        private readonly IGenericRepository<Actividad> _actividadRepositorio;
        private readonly IMapper _mapper;

        public ProfesorActividadService(IGenericRepository<Actividad> actividadRepositorio, IMapper mapper)
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
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la actividad.", ex);
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

        public async Task<List<ActividadDTO>> ObteneActividadrPorIdProfesor(int id)
        {
            try
            {
                var actividad = await _actividadRepositorio.ObtenerTodos(a => a.Idusuario == id);
                if (actividad == null)
                {
                    throw new InvalidOperationException("El usuario no desarrollo ninguna actividad.");
                }
                return _mapper.Map<List<ActividadDTO>>(actividad);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la actividad por id del usuario.", ex);
            }

        }


        public async Task<bool> CrearActividad(ActividadDTO actividads)
        {
            try
            {
                var actividad = _mapper.Map<Actividad>(actividads);
                await _actividadRepositorio.Crear(actividad);
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la actividad.", ex);
            }
        }

        public async Task<bool> ActualizarActivdad(ActividadDTO actividad)
        {
            try
            {
                var actividadModelo = _mapper.Map<Actividad>(actividad);
                var actividadEncontrada = await _actividadRepositorio.Obtener(a => a.Idactividad == actividadModelo.Idactividad);
                if (actividadEncontrada == null)
                {
                    throw new TaskCanceledException("La actividad no existe");
                }
                _mapper.Map(actividad, actividadEncontrada);
                await _actividadRepositorio.Editar(actividadEncontrada);
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception("Error al actualizar la actividad.", ex);
            }
        }

        public async Task<bool> EliminarActividad(int id)
        {
            try
            {
                var actividadEncontrada = await _actividadRepositorio.Obtener(a => a.Idactividad == id);
                if (actividadEncontrada == null)
                    throw new TaskCanceledException("La actividad no encontrada.");

                bool respuesta = await _actividadRepositorio.Eliminar(actividadEncontrada);
                if (!respuesta)
                    throw new TaskCanceledException("No se pudo eliminar");
                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}