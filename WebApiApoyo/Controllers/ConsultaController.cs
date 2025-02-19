using Microsoft.AspNetCore.Mvc;
using SistemaApoyo.API.Utilidad;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using Microsoft.AspNetCore.Http;
using SistemaApoyo.BLL.servicios;
using SistemaApoyo.Model;

namespace WebApiApoyo.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class ConsultaController : ControllerBase
    {
        private readonly IConsultaService _consultaService;
        private readonly ILogger<ConsultaController> _logger;
        private readonly IRespuestaService _respuestaService;

        public ConsultaController(IConsultaService consultaService, IRespuestaService respuestaService,ILogger<ConsultaController> logger)
        {
            _consultaService = consultaService;
            _respuestaService = respuestaService;
            _logger = logger;
        }

        [HttpGet]
        [Route("ListarConsultas")]
        public async Task<IActionResult> ListaConsultas()
        {
            var rsp = new Response<List<ConsultaDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _consultaService.ConsultarConsultas();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener la actividad.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("TituloConsulta")]
        public async Task<IActionResult> ListaConsultaPorTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                return BadRequest("El titulo no es válido.");
            }

            var rsp = new Response<List<ConsultaDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _consultaService.ConsultarPorTitulo(titulo);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el nombre de la consulta.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("ConsultaID")]
        public async Task<IActionResult> ListaConsultaPorId(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var rsp = new Response<ConsultaDTO>();
            try
            {
                rsp.value = await _consultaService.ObtenerConsultaPorId(id);
                rsp.status = true;
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el id de la consulta.");
            }
            return Ok(rsp);
        }

        [HttpPost]
        [Route("CrearConsulta")]
        public async Task<IActionResult> CrearConsulta([FromBody] ConsultaDTO consultaDto)
        {
            if (consultaDto == null)
            {
                return BadRequest("Los datos de la consulta son inválidos.");
            }

            var rsp = new Response<string>();
            try
            {
                var resultado = await _consultaService.CrearConsulta(consultaDto);
                rsp.status = true;
                rsp.value = "Consulta creada con éxito.";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al crear la consulta.");
            }
            return Ok(rsp);
        }

        [HttpGet("ListaRespuestasDeConsulta")]
        public async Task<IActionResult> ListaRespuestasDeConsulta(int consultaId)
        {
            var rsp = new Response<List<RespuestaDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _consultaService.RespuestasDeConsultaID(consultaId);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener la lista de respuestas.");
            }
            return Ok(rsp);
        }
    }
}
