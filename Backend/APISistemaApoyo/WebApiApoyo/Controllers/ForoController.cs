using Microsoft.AspNetCore.Mvc;
using SistemaApoyo.API.Utilidad;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using SistemaApoyo.BLL.Servicios;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;

namespace WebApiApoyo.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class ForoController : ControllerBase
    {
        private readonly IForoService _foroService;
        private readonly INivelService _nivelService;
        private readonly ILogger<ForoController> _logger;

        public ForoController(IForoService foroService, ILogger<ForoController> logger, INivelService nivelService)
        {
            _foroService = foroService;
            _nivelService = nivelService;
            _logger = logger;
        }

        [HttpGet]
        [Route("ListarForos")]
        public async Task<IActionResult> ListaForos()
        {
            var rsp = new Response<List<ForoDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _foroService.ConsultarForos();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el foro.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("ListaForosPorNivel")]
        public async Task<IActionResult> ListaForoNivel(int idNivel)
        {
            var existeNivel = await _nivelService.ObtenerNivelPorID(idNivel);
            if (existeNivel == null)
            {
                return BadRequest("El ID proporcionado no corresponde a un nivel.");
            }

            var rsp = new Response<List<ForoDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _foroService.ConsultarForoNivel(idNivel);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener lso foros correspondientes al nivel");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("NombreForo")]
        public async Task<IActionResult> ListaConsultaPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest("El titulo no es válido.");
            }

            var rsp = new Response<List<ForoDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _foroService.ConsultarPorNombre(nombre);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el nombre de la consulta");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("ForoID")]
        public async Task<IActionResult> BuscarForoPorId(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var rsp = new Response<ForoDTO>();
            try
            {
                rsp.value = await _foroService.ObtenerForoPorId(id);
                rsp.status = true;
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el id del foro.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("ConsultasForo")]
        public async Task<IActionResult> ListaConsultasPorForoId(int idForo)
        {
            if (idForo <= 0)
            {
                return BadRequest("El ID del foro no es válido.");
            }

            var rsp = new Response<List<ConsultaDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _foroService.ObtenerConsultasPorForo(idForo);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener las consultas del foro.");
            }
            return Ok(rsp);
        }

        [HttpPost]
        [Route("CrearForo")]
        public async Task<IActionResult> CrearForo([FromBody] ForoDTO foro)
        {
            if (foro == null)
            {
                return BadRequest("Los datos del foro son inválidos.");
            }

            var rsp = new Response<string>();
            try
            {
                var resultado = await _foroService.CrearForo(foro);
                rsp.status = true;
                rsp.value = "Foro creado con éxito.";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al crear el foro.");
            }
            return Ok(rsp);
        }
    }
}
