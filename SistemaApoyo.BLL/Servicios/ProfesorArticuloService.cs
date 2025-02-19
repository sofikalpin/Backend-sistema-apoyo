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
    public class ProfesorArticuloService : IProfesorArticulo
    {
        public readonly IGenericRepository<Articulo> _articuloRepositorio;
        private readonly IMapper _mapper;

        public ProfesorArticuloService(IGenericRepository<Articulo> articuloRepositorio, IMapper mapper)
        {
            _articuloRepositorio = articuloRepositorio;
            _mapper = mapper;
        }

        public async Task<List<ArticuloDTO>> ConsultarArticulo()
        {
            try
            {
                var consulta = await _articuloRepositorio.Consultar();
                var listaArticulo = await consulta.ToListAsync();
                return _mapper.Map<List<ArticuloDTO>>(listaArticulo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el articulo.", ex);
            }
        }

        public async Task<List<ArticuloDTO>> ConsultarporTitulo(string titulo)
        {
            try
            {
                var Articuloquery = await _articuloRepositorio.Consultar();
                if (!string.IsNullOrEmpty(titulo))
                {
                    Articuloquery = Articuloquery.Where(v => v.Titulo == titulo);
                }


                var listaResultado = await Articuloquery.ToListAsync();
                return _mapper.Map<List<ArticuloDTO>>(listaResultado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el articulo por titulo.", ex);
            }
        }

        public async Task<List<ArticuloDTO>> ObteneArticulorPorIdProfesor(int id)
        {
            try
            {
                var articulo = await _articuloRepositorio.ObtenerTodos(a => a.Idusuario == id);
                if (articulo == null)
                {
                    throw new InvalidOperationException("Articulo no encontrado.");
                }
                return _mapper.Map<List<ArticuloDTO>>(articulo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el articulo por id.", ex);
            }

        }

        public async Task<bool> CrearArticulo(ArticuloDTO articulos)
        {
            try
            {
                var articulo = _mapper.Map<Articulo>(articulos);
                await _articuloRepositorio.Crear(articulo);
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el articulo.", ex);
            }
        }

        public async Task<bool> ActualizarArticulo(ArticuloDTO articulo)
        {
            try
            {
                var articuloModelo = _mapper.Map<Articulo>(articulo);
                var articuloEncontrado = await _articuloRepositorio.Obtener(a => a.Idarticulo == articuloModelo.Idarticulo);
                if (articuloEncontrado == null)
                {
                    throw new TaskCanceledException("El articulo no existe");
                }
                _mapper.Map(articulo, articuloEncontrado);
                await _articuloRepositorio.Editar(articuloEncontrado);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el articulo.", ex);
            }
        }

        public async Task<bool> EliminarArticulo(int id)
        {
            try
            {
                var articuloEncontrado = await _articuloRepositorio.Obtener(a => a.Idarticulo == id);
                if (articuloEncontrado == null)
                    throw new TaskCanceledException("Articulo no encontrado.");

                bool respuesta = await _articuloRepositorio.Eliminar(articuloEncontrado);
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