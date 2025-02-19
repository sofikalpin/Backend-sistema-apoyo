using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DAL.Repositorios.Contrato;
using SistemaApoyo.DTO;
using SistemaApoyo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaApoyo.BLL.Servicios
{
    public class ReseñaAlumnoService : IReseñaAlumnoService
    {
        private readonly IGenericRepository<Reseña> _resenaRepositorio;
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        private readonly IMapper _mapper;

        public ReseñaAlumnoService( IGenericRepository<Reseña> resenaRepositorio, IGenericRepository<Usuario> usuarioRepositorio, IMapper mapper)
        {
            _resenaRepositorio = resenaRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
        }

        public async Task<List<ReseñaAlumnoDTO>> ObtenerTodasReseñas()
        {
            try
            {
                var res = await _resenaRepositorio.Consultar();
                var listaReseñas = await res.ToListAsync();
                return _mapper.Map<List<ReseñaAlumnoDTO>>(listaReseñas);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las reseñas", ex);
            }
        }

        public async Task<ReseñaAlumnoDTO> ObtenerPorId(int id)
        {
            try
            {
                var resena = await _resenaRepositorio.Obtener(r => r.IdReseña == id);
                if (resena == null)
                    throw new TaskCanceledException("Reseña no encontrada");

                return _mapper.Map<ReseñaAlumnoDTO>(resena);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la reseña", ex);
            }
        }

        public async Task<bool> CrearReseña(ReseñaAlumnoDTO reseñaDTO)
        {
            try
            {
                if (reseñaDTO.Rating < 1 || reseñaDTO.Rating > 5)
                {
                    throw new Exception("El rating debe estar entre 1 y 5");
                }

                var reseña = _mapper.Map<Reseña>(reseñaDTO);
                await _resenaRepositorio.Crear(reseña);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la reseña", ex);
            }
        }
    }
}
