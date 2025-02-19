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
    public class ArticuloService : IArticuloService
    {
        private readonly IGenericRepository<Articulo> _articuloRepository;
        private readonly IMapper _mapper;

        public ArticuloService(IGenericRepository<Articulo> articuloRepository, IMapper mapper)
        {
            _articuloRepository = articuloRepository;
            _mapper = mapper;
        }

        public async Task<List<ArticuloDTO>> ConsultarArticulo()
        {
            try
            {
                var consulta = await _articuloRepository.Consultar();
                var listaArticulo= await consulta.ToListAsync();
                return _mapper.Map<List<ArticuloDTO>>(listaArticulo);

            }
            catch
            {
                throw;

            }
        }

        public async Task<List<ArticuloDTO>> ConsultarPorTitulo(string titulo)
        {
            try
            {
                var Articuloquery = await _articuloRepository.Consultar();
                if (!string.IsNullOrEmpty(titulo))
                {
                    Articuloquery = Articuloquery.Where(v => v.Titulo == titulo);
                }


                var listaResultado = await Articuloquery.ToListAsync();
                return _mapper.Map<List<ArticuloDTO>>(listaResultado);
            }
            catch
            {
                throw;
            }
        }


        public async Task<ArticuloDTO> ObtenerPorId(int id)
        {
            try
            {
                var articulo = await _articuloRepository.Obtener(a => a.Idarticulo == id);
                return _mapper.Map<ArticuloDTO>(articulo);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ArticuloDTO>> ObtenerPorNivel(int idNivel)
        {
            try
            {
                var articulos = await _articuloRepository.ObtenerTodos(a => a.Idnivel == idNivel);
                if (articulos == null)
                {
                    throw new InvalidOperationException("Consulta no encontrada.");
                }
                return _mapper.Map<List<ArticuloDTO>>(articulos);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de articulos por nivel.", ex);
            }
        }

    }
}