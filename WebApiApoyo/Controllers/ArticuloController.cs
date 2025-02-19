using Microsoft.AspNetCore.Mvc;
using SistemaApoyo.API.Utilidad;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using Microsoft.AspNetCore.Http;
using SistemaApoyo.BLL.Servicios;

namespace SistemaApoyo.API.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class ArticuloController : ControllerBase
    {
        private readonly IArticuloService _articuloService;
        private readonly ILogger<ArticuloController> _logger;

        public ArticuloController(IArticuloService articuloService, ILogger<ArticuloController> logger)
        {
            _articuloService = articuloService;
            _logger = logger;
        }

        [HttpGet]
        [Route("ListarArticulo")]
        public async Task<IActionResult> ListaArticulo()
        {
            var rsp = new Response<List<ArticuloDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _articuloService.ConsultarArticulo();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener la lista de articulos.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("TituloArticulo")]
        public async Task<IActionResult> ListaArticuloPorNombre(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                return BadRequest("El titulo no es valido.");
            }

            var rsp = new Response<List<ArticuloDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _articuloService.ConsultarPorTitulo(titulo);  
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el titulo del articulo.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("ArticuloID")]
        public async Task<IActionResult> ListaArticuloId(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var rsp = new Response<ArticuloDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _articuloService.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el id del articulo");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("ArticulosPorNivel")]
        public async Task<IActionResult> ListaArticuloporNivel(int idNivel)
        {
            if (idNivel <= 0 || idNivel > 6)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var rsp = new Response<List<ArticuloDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _articuloService.ObtenerPorNivel(idNivel);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener la lista de articulos por nivel");
            }
            return Ok(rsp);
        }
    }
}
