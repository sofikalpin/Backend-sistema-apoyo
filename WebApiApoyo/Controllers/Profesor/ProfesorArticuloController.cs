using Microsoft.AspNetCore.Mvc;
using SistemaApoyo.API.Utilidad;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using Microsoft.AspNetCore.Http;
using SistemaApoyo.Model;
using Microsoft.EntityFrameworkCore;
using SistemaApoyo.BLL.Servicios;
using SistemaApoyo.DAL.DBContext;

namespace WebApiApoyo.Controllers.Profesor
{
    [Route("API/[controller]")]
    [ApiController]

    public class ProfesorArticuloController : ControllerBase
    {
        private readonly IProfesorArticulo _profesorArticuloService;
        private readonly ILogger<ProfesorArticuloController> _logger;
        private readonly RailwayContext _context;

        public ProfesorArticuloController(IProfesorArticulo profesorArticuloService, ILogger<ProfesorArticuloController> logger, RailwayContext context)
        {
            _profesorArticuloService = profesorArticuloService;
            _context = context;
            _logger = logger;
        }

        [HttpGet("ListaArticulo")]
        public async Task<IActionResult> ListaActividades()
        {
            var rsp = new Response<List<ArticuloDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _profesorArticuloService.ConsultarArticulo();
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
        public async Task<IActionResult> ListaActividadPorNombre(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                return BadRequest("El nombre no es  válido.");
            }

            var rsp = new Response<List<ArticuloDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _profesorArticuloService.ConsultarporTitulo(titulo);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el titulo del articulo.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("ArticuloIDProfesor")]
        public async Task<IActionResult> ListaArticuloPorId(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var rsp = new Response<List<ArticuloDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _profesorArticuloService.ObteneArticulorPorIdProfesor(id);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el id de la actividad");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("ArticuloProProfeyNivel")]
        public async Task<IActionResult> ListaArticuloPorProfesorYNivel(int idUsuario, int idNivel)
        {
            if (idUsuario <= 0 || idNivel <= 0)
            {
                return BadRequest("El ID del nivel o usuario proporcionado no es válido.");
            }

            var rsp = new Response<List<ArticuloDTO>>();

            var listaArticuloProfe = await _profesorArticuloService.ObteneArticulorPorIdProfesor(idUsuario);

            if (listaArticuloProfe == null || !listaArticuloProfe.Any())
            {
                return NotFound("No se encontraron articulos para el profesor indicado.");
            }

            var listaFinal = listaArticuloProfe.Where(a => a.Idnivel == idNivel).ToList();

            try
            {
                rsp.status = true;
                rsp.value = listaFinal;
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el id de la articulo");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }

        [HttpPost]
        [Route("CrearArticulo")]
        public async Task<IActionResult> CrearArticulo([FromBody] ArticuloDTO articulo)
        {

            var idMaximo = await _context.Articulos.MaxAsync(a => a.Idarticulo) + 1;

            var rsp = new Response<string>();
            try
            {
                if (articulo.Idarticulo == 0)
                {
                    articulo.Idarticulo = idMaximo;
                    rsp.status = true;
                    var resultado = await _profesorArticuloService.CrearArticulo(articulo);
                    rsp.value = "Articulo creado con éxito";
                }
                else
                {
                    rsp.status = false;
                    rsp.value = "El valor de Idarticulo debe ser cero";
                }
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al guardar el articulo.");
            }
            return Ok(rsp);
        }

        [HttpPut]
        [Route("EditarporID")]
        public async Task<IActionResult> EditarArticulo(int id, [FromBody] ArticuloDTO articulo)
        {
            if (id != articulo.Idarticulo)
            {
                return BadRequest("El ID de la actividad no coincide con el ID proporcionado.");
            }

            var rsp = new Response<string>();
            try
            {
                var resultado = await _profesorArticuloService.ActualizarArticulo(articulo);
                rsp.status = true;
                rsp.value = "Articulo actualizado con éxito.";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al actualizar el articulo.");
            }
            return Ok(rsp);
        }

        [HttpDelete]
        [Route("EliminarArticulo")]
        public async Task<IActionResult> Eliminar(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var rsp = new Response<string>();

            try
            {
                var eli = await _profesorArticuloService.EliminarArticulo(id);
                rsp.status = true;
                rsp.value = "Articulo Eliminado con éxito.";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al eliminar el articulo");
            }
            return Ok(rsp);
        }
    }
}