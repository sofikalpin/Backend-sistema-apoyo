using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DAL.Repositorios.Contrato;
using SistemaApoyo.DTO;
using SistemaApoyo.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace SistemaApoyo.BLL.Servicios
{
    public class ConsultaService : IConsultaService
    {
        private readonly IGenericRepository<Consulta> _consultaRepositorio;
        private readonly IGenericRepository<Respuesta> _respuestaRepositorio;
        private readonly IMapper _mapper;

        public ConsultaService(IGenericRepository<Consulta> consultaRepositorio, IGenericRepository<Respuesta> respuestaRepositorio,IMapper mapper)
        {
            _consultaRepositorio = consultaRepositorio;
            _respuestaRepositorio = respuestaRepositorio;
            _mapper = mapper;
        }

        public async Task<List<ConsultaDTO>> ConsultarConsultas()
        {
            try
            {
                var consulta = await _consultaRepositorio.Consultar();
                var listaConsulta = await consulta.ToListAsync();
                return _mapper.Map<List<ConsultaDTO>>(listaConsulta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de consultas.", ex);
            }
        }

        public async Task<List<ConsultaDTO>> ConsultarPorTitulo(string titulo)
        {
            try
            {
                var Actividadquery = await _consultaRepositorio.Consultar();
                if (!string.IsNullOrEmpty(titulo))
                {
                    Actividadquery = Actividadquery.Where(v => v.Titulo == titulo);
                }

                var listaResultado = await Actividadquery.ToListAsync();
                return _mapper.Map<List<ConsultaDTO>>(listaResultado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la consulta por título.", ex);
            }
        }

        public async Task<ConsultaDTO> ObtenerConsultaPorId(int idconsulta)
        {
            try
            {
                var consulta = await _consultaRepositorio.Obtener(c => c.Idconsulta == idconsulta);
                if (consulta == null)
                {
                    throw new InvalidOperationException("Consulta no encontrada.");
                }

                return _mapper.Map<ConsultaDTO>(consulta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la consulta.", ex);
            }
        }

        public async Task<bool> CrearConsulta(ConsultaDTO consultaDto)
        {
            try
            {
                var consulta = _mapper.Map<Consulta>(consultaDto);
                await _consultaRepositorio.Crear(consulta);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la consulta.", ex);
            }
        }

        public async Task<List<RespuestaDTO>> RespuestasDeConsultaID(int idConsulta)
        {
            try
            {
                var consulta = await _consultaRepositorio.Obtener(c => c.Idconsulta == idConsulta);
                if (consulta == null)
                {
                    throw new InvalidOperationException("La consulta no exite");
                }

                var respuestasConsulta = await _respuestaRepositorio.Consultar(r => r.Idconsulta == idConsulta);

                return _mapper.Map<List<RespuestaDTO>>(respuestasConsulta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las respuestas de la consulta.", ex);
            }
        }
    }
}
