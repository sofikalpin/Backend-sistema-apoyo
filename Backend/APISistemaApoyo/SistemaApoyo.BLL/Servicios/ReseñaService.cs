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
    public class ReseñaService : IReseñaService
    {
        private readonly IGenericRepository<Reseñapagina> _reseñaRepositorio;
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        private readonly IMapper _mapper;

        public ReseñaService(
            IGenericRepository<Reseñapagina> reseñaRepositorio,
            IGenericRepository<Usuario> usuarioRepository, IMapper mapper)
        {
            _reseñaRepositorio = reseñaRepositorio;
            _usuarioRepositorio = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<ReseñaDTO> ObtenerReseñaPorId(int id)
        {
            try
            {
                var resena = await _reseñaRepositorio.Obtener(r => r.IdReseñaP == id);
                if (resena == null)
                {
                    throw new Exception("Reseña no encontrada");
                }
                return _mapper.Map<ReseñaDTO>(resena);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la reseña", ex);
            }
        }

        public async Task<List<ReseñaDTO>> ObtenerTodasLasReseñas()
        {
            try
            {
                var reseñas = await _reseñaRepositorio.Consultar();
                var listaReseña = await reseñas.ToListAsync();
                return _mapper.Map<List<ReseñaDTO>>(listaReseña);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las reseñas", ex);
            }
        }

        public async Task<List<ReseñaDTO>> ObtenerReseñasDeUsuario(int idUsuario)
        {
            try
            {
                var usuario = await _usuarioRepositorio.Obtener(u => u.Idusuario == idUsuario);
                if (usuario == null)
                {
                    throw new InvalidOperationException("El usuario no se encontro");
                }

                var reseñas = await _reseñaRepositorio.Consultar(r => r.Idusuaro == idUsuario);
                var listaReseñas = await reseñas.ToListAsync();
                return _mapper.Map<List<ReseñaDTO>>(listaReseñas);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las reseñas del usuario", ex);
            }
        }

        public async Task<bool> CrearReseña(ReseñaDTO reseñaDTO)
        {
            try
            {
                if (reseñaDTO.Rating < 1 || reseñaDTO.Rating > 5)
                {
                    throw new Exception("El rating debe estar entre 1 y 5");
                }

                var reseña = _mapper.Map<Reseñapagina>(reseñaDTO);
                var reseñaCreada = await _reseñaRepositorio.Crear(reseña);
                if (reseñaCreada == null)
                {
                    throw new Exception("Error al crear la reseña");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la reseña", ex);
            }
        }




    }
}