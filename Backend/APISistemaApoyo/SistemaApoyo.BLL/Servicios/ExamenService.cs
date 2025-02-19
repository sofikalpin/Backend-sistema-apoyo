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
    public class ExamenService : IExamenService
    {

        private readonly IGenericRepository<Examen> _examenRepositorio;
        private readonly IMapper _mapper;

        public ExamenService(IGenericRepository<Examen> examenRepositorio, IMapper mapper)
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
                throw new Exception("Error al obtener el examen", ex);
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

        public async Task<ExamenDTO> ObteneExamenrPorId(int id)
        {
            try
            {
                var examen = await _examenRepositorio.Obtener(a => a.Idexamen == id);
                if (examen == null)
                {
                    throw new InvalidOperationException("Examen no encontrado.");
                }
                return _mapper.Map<ExamenDTO>(examen);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el examen por id.", ex);
            }
        }

        public async Task<List<ExamenDTO>> ObtenerPorNivel(int idNivel)
        {
            try
            {
                var examenes = await _examenRepositorio.ObtenerTodos(a => a.Idnivel == idNivel);
                if (examenes == null)
                {
                    throw new InvalidOperationException("Examen no encontrada.");
                }
                return _mapper.Map<List<ExamenDTO>>(examenes);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de examenes por nivel.", ex);
            }
        }
    }
}