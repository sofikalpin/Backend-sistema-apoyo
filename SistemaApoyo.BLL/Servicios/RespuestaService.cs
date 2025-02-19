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
using SistemaApoyo.DAL.DBContext;

namespace SistemaApoyo.BLL.servicios
{
    public class RespuestaService : IRespuestaService
    {
        private readonly IGenericRepository<Respuesta> _respuestaRepositorio;
        private readonly IGenericRepository<Consulta> _consultaRepositorio;
        RailwayContext _context;
        private readonly IMapper _mapper;

        public RespuestaService(IGenericRepository<Respuesta> respuestaRepositorio, IGenericRepository<Consulta> consultaRepositorio,IMapper mapper, RailwayContext context)
        {
            _respuestaRepositorio = respuestaRepositorio;
            _consultaRepositorio = consultaRepositorio;
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<RespuestaDTO>> ConsultarRespuesta()
        {
            try
            {
                var consulta = await _respuestaRepositorio.Consultar();
                var listaRespuesta = await consulta.ToListAsync();
                return _mapper.Map<List<RespuestaDTO>>(listaRespuesta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la respuesta.", ex);
            }
        }

        public async Task<RespuestaDTO> ObteneRespuestarPorId(int id)
        {
            try
            {
                var respuesta = await _respuestaRepositorio.Obtener(a => a.Idrespuesta == id);
                if (respuesta == null)
                {
                    throw new InvalidOperationException("Respuesta no encontrada.");
                }
                return _mapper.Map<RespuestaDTO>(respuesta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la respuesta por id.", ex);
            }
        }

        public async Task<bool> CrearRespuesta(RespuestaDTO respuestas)
        {
            try
            {
                var respuesta = _mapper.Map<Respuesta>(respuestas);
                await _respuestaRepositorio.Crear(respuesta);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la respuesta.", ex);
            }
        }
    }
}
