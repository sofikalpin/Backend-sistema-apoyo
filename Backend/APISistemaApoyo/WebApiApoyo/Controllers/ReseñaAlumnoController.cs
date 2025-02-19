using Microsoft.AspNetCore.Mvc;
using SistemaApoyo.API.Utilidad;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using Microsoft.AspNetCore.Http;
using SistemaApoyo.BLL.Servicios;
using SistemaApoyo.Model;
using Microsoft.EntityFrameworkCore;
using SistemaApoyo.DAL.DBContext;

namespace WebApiApoyo.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class ReseñaController : ControllerBase
    {
        private readonly IReseñaAlumnoService _reseñaService;
        private readonly ILogger<ReseñaController> _logger;
        private readonly RailwayContext _context;

        public ReseñaController(IReseñaAlumnoService reseñaService, ILogger<ReseñaController> logger, RailwayContext context)
        {
            _reseñaService = reseñaService;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("ListaReseñasAlumno")]
        public async Task<IActionResult> ListaReseñas()
        {
            var rsp = new Response<List<ReseñaAlumnoDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _reseñaService.ObtenerTodasReseñas();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener las reseñas.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("ReseñaIDAlumno")]
        public async Task<IActionResult> ListaReseñaporId(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El id proporcionado no es valido");

            }

            var rsp = new Response<ReseñaAlumnoDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _reseñaService.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                rsp.status = false;

                _logger.LogWarning(ex, "Reseña no encontrada");

            }
            return Ok(rsp);
        }

        [HttpPost]
        [Route("CrearReseñaAlumno")]
        public async Task<IActionResult> CrearReseña([FromBody] ReseñaAlumnoDTO reseñaAluDTO)
        {
            var idMaximo = await _context.Reseñas.MaxAsync(r => r.IdReseña) + 1;
            var rsp = new Response<string>();
            try
            {
                if (reseñaAluDTO.IdReseña == 0)
                {
                    reseñaAluDTO.IdReseña = idMaximo;
                    rsp.status = true;
                    var resultado = await _reseñaService.CrearReseña(reseñaAluDTO);
                    rsp.value = "Reseña creada con exito";
                }
                else
                {
                    rsp.status = false;
                    rsp.value = "El valor de idreseña debe ser 0";
                }
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al crear la reseña");
            }
            return Ok(rsp);
        }

    }
}