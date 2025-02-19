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
    public class NivelService : INivelService
    {
        private readonly IGenericRepository<Nivel> _nivelRepository;
        private readonly IMapper _mapper;

        public NivelService(IGenericRepository<Nivel> nivelRepository, IMapper mapper)
        {
            _nivelRepository = nivelRepository;
            _mapper = mapper;
        }

        public async Task<List<NivelDTO>> ConsultarNivel()
        {
            try
            {
                var consulta = await _nivelRepository.Consultar();
                var listaNivel = await consulta.ToListAsync();
                return _mapper.Map<List<NivelDTO>>(listaNivel);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de foros.", ex);
            }
        }

        public async Task<List<NivelDTO>> ConsultarPorDescripcion(string descripcion)
        {
            try
            {
                var Actividadquery = await _nivelRepository.Consultar();
                if (!string.IsNullOrEmpty(descripcion))
                {
                    Actividadquery = Actividadquery.Where(v => v.Descripcion == descripcion);
                }


                var listaResultado = await Actividadquery.ToListAsync();
                return _mapper.Map<List<NivelDTO>>(listaResultado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de foros.", ex);
            }
        }

        public async Task<NivelDTO> ObtenerNivelPorID(int id)
            {
            try
            {
                var actividad = await _nivelRepository.Obtener(a => a.Idnivel == id);
                if (actividad == null)
                {
                    throw new InvalidOperationException("Foro no encontrado.");
                }
                return _mapper.Map<NivelDTO>(actividad);
            }
            catch (Exception ex) 
            { 
                throw new Exception("Error al obtener la lista de foros.", ex);
            }

        } 
        
    }
}