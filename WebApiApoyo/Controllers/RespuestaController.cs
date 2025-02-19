using Microsoft.AspNetCore.Mvc;
using SistemaApoyo.API.Utilidad;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using Microsoft.AspNetCore.Http;

namespace WebApiApoyo.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class RespuestaController : ControllerBase
    {
        private readonly IRespuestaService _respuestaService;
        private readonly ILogger<RespuestaController> _logger;

        public RespuestaController(IRespuestaService respuestaService, ILogger<RespuestaController> logger)
        {
            _respuestaService = respuestaService;
            _logger = logger;
        }

        [HttpGet]
        [Route("ListaRespuestas")]
        public async Task<IActionResult> ListarRespuestas()
        {
            var rsp = new Response<List<RespuestaDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _respuestaService.ConsultarRespuesta();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener las respuestas.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("RespuestaID")]
        public async Task<IActionResult> ListaRespuestaPorId(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es valido.");
            }

            var rsp = new Response<RespuestaDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _respuestaService.ObteneRespuestarPorId(id);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el id de la respuesta");
            }
            return Ok(rsp);
        }

        [HttpPost]
        [Route("CrearRespuesta")]
        public async Task<IActionResult> CrearRespuesta([FromBody] RespuestaDTO respuesta)
        {
            var rsp = new Response<string>();
            try
            {
                rsp.status = true;
                var resultado = await _respuestaService.CrearRespuesta(respuesta);
                rsp.value = "Respuesta creada con éxito";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al guardar la respuesta.");
            }
            return Ok(rsp);
        }
    }
}
