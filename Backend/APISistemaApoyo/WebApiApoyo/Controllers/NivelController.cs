using Microsoft.AspNetCore.Mvc;
using SistemaApoyo.API.Utilidad;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using Microsoft.AspNetCore.Http;


namespace WebApiApoyo.Controllers
{

    [Route("API/[controller]")]
    [ApiController]

    public class NivelController : ControllerBase
    {
        private readonly INivelService _nivelService;
        private readonly ILogger<NivelController> _logger;

        public NivelController(INivelService nivelService, ILogger<NivelController> logger)
        {
            _nivelService = nivelService;
            _logger = logger;
        }

        [HttpGet]
        [Route ("Listar Niveles")]
        public async Task<IActionResult> GetNiveles()
        {
            var rsp = new Response<List<NivelDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _nivelService.ConsultarNivel();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener los niveles.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("Nivel Descripcion")]
        public async Task<IActionResult> ListarNivelPorDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                return BadRequest("La descripcion no es  válida.");
            }

            var rsp = new Response<List<NivelDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _nivelService.ConsultarPorDescripcion(descripcion);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener la descripcion del nivel.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("Nivel ID")]
        public async Task<IActionResult> ListarNivelID(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var rsp = new Response<NivelDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _nivelService.ObtenerNivelPorID(id);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el id del nivel");
            }
            return Ok(rsp);
        }

    }
}

