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
    public class ProfesorExamenService : IProfesorExamen
    {

        private readonly IGenericRepository<Examen> _examenRepositorio;
        private readonly IMapper _mapper;

        public ProfesorExamenService(IGenericRepository<Examen> examenRepositorio, IMapper mapper)
        {
            _examenRepositorio = examenRepositorio;
            _mapper = mapper;
        }

        public async Task<List<ExamenDTO>> ConsultarExamen()
        {
            try
            {
                var consulta = await _examenRepositorio.Consultar();
                var listaExamen = await consulta.ToListAsync();
                return _mapper.Map<List<ExamenDTO>>(listaExamen);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la actividad.", ex);
            }
        }

        public async Task<List<ExamenDTO>> ConsultarPorTitulo(string titulo)
        {
            try
            {
                var Actividadquery = await _examenRepositorio.Consultar();
                if (!string.IsNullOrEmpty(titulo))
                {
                    Actividadquery = Actividadquery.Where(v => v.Titulo == titulo);
                }


                var listaResultado = await Actividadquery.ToListAsync();
                return _mapper.Map<List<ExamenDTO>>(listaResultado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la actividad por nombre.", ex);
            }
        }

        public async Task<List<ExamenDTO>> ObteneExamenrPorIdProfesor(int id)
        {
            try
            {
                var examen = await _examenRepositorio.ObtenerTodos(a => a.Idusuario == id);
                if (examen == null)
                {
                    throw new InvalidOperationException("Examen no encontrado.");
                }
                return _mapper.Map<List<ExamenDTO>>(examen);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el examen por id.", ex);
            }
        }

        public async Task<bool> CrearExamen(ExamenDTO examenes)
        {
            try
            {
                var examen = _mapper.Map<Examen>(examenes);
                await _examenRepositorio.Crear(examen);
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el examen.", ex);
            }
        }

        public async Task<bool> ActualizarExamen(ExamenDTO examen)
        {
            try
            {
                var examenModelo = _mapper.Map<Examen>(examen);
                var examenEncontrado = await _examenRepositorio.Obtener(a => a.Idexamen == examenModelo.Idexamen);
                if (examenEncontrado == null)
                {
                    throw new TaskCanceledException("La examen no existe");
                }
                _mapper.Map(examen, examenEncontrado);
                await _examenRepositorio.Editar(examenEncontrado);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la examen.", ex);
            }
        }

        public async Task<bool> EliminarExamen(int id)
        {
            try
            {
                var examenEncontrado = await _examenRepositorio.Obtener(a => a.Idexamen == id);
                if (examenEncontrado == null)
                    throw new TaskCanceledException("Examen no encontrado.");

                bool respuesta = await _examenRepositorio.Eliminar(examenEncontrado);
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