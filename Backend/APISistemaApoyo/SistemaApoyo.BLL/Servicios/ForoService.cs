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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemaApoyo.BLL.Servicios
{
    public class ForoService : IForoService
    {
        private readonly IGenericRepository<Foro> _foroRepositorio;
        private readonly IGenericRepository<Consulta> _consultaRepositorio;
        private readonly IMapper _mapper;

        public ForoService(IGenericRepository<Foro> foroRepositorio, IGenericRepository<Consulta> consultaRepositorio, IMapper mapper)
        {
            _foroRepositorio = foroRepositorio;
            _consultaRepositorio = consultaRepositorio;
            _mapper = mapper;
        }

        public async Task<List<ForoDTO>> ConsultarForos()
        {
            try
            {
                var foro = await _foroRepositorio.Consultar();
                var listaforo = await foro.ToListAsync();
                return _mapper.Map<List<ForoDTO>>(listaforo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de foros.", ex);
            }
        }

        public async Task<List<ForoDTO>> ConsultarForoNivel(int idNivel)
        {
            try
            {
                var Foroquery = await _foroRepositorio.Consultar();
                if (idNivel != null && idNivel > 0)
                {
                    Foroquery = Foroquery.Where(f => f.Idnivel == idNivel);
                }
                var listaResultado = await Foroquery.ToListAsync();
                return _mapper.Map<List<ForoDTO>>(listaResultado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el foro por nombre.", ex);
            }
        }

        public async Task<List<ForoDTO>> ConsultarPorNombre(string nombre)
        {
            try
            {
                var Actividadquery = await _foroRepositorio.Consultar();
                if (!string.IsNullOrEmpty(nombre))
                {
                    Actividadquery = Actividadquery.Where(v => v.Nombre == nombre);
                }
                var listaResultado = await Actividadquery.ToListAsync();
                return _mapper.Map<List<ForoDTO>>(listaResultado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el foro por nombre.", ex);
            }
        }

        public async Task<ForoDTO> ObtenerForoPorId(int id)
        {
            try
            {
                var foro = await _foroRepositorio.Obtener(c => c.Idforo == id);
                if (foro == null)
                {
                    throw new InvalidOperationException("Foro no encontrado.");
                }
                return _mapper.Map<ForoDTO>(foro);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el foro por id.", ex);
            }
        }

        public async Task<List<ConsultaDTO>> ObtenerConsultasPorForo(int idForo)
        {
            try
            {
                var consultas = await _consultaRepositorio.Consultar(c => c.Idforo == idForo);

                if (consultas == null)
                {
                    throw new InvalidOperationException("El foro ingresado no posee consultas.");
                }

                var listaConsultas = await consultas.ToListAsync();
                return _mapper.Map<List<ConsultaDTO>>(listaConsultas);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las consultas del foro.", ex);
            }
        }

        public async Task<bool> CrearForo(ForoDTO foros)
        {
            try
            {
                var foro = _mapper.Map<Foro>(foros);
                await _foroRepositorio.Crear(foro);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el foro.", ex);
            }
        }
    }
}