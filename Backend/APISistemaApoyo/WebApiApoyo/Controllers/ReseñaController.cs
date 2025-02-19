using Microsoft.AspNetCore.Mvc;
using SistemaApoyo.API.Utilidad;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using Microsoft.AspNetCore.Http;
using SistemaApoyo.BLL.Servicios;
using SistemaApoyo.Model;
using Microsoft.EntityFrameworkCore;
using SistemaApoyo.DAL.DBContext;

namespace SistemaApoyo.API.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class ReseñaController : ControllerBase
    {
        private readonly IReseñaService _reseñaService;
        private readonly ILogger<ReseñaController> _logger;
        RailwayContext _context;

        public ReseñaController(IReseñaService reseñaService, ILogger<ReseñaController> logger, RailwayContext context)
        {
            _reseñaService = reseñaService;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("ListaReseñas")]
        public async Task<IActionResult> ListaReseñas()
        {
            var rsp = new Response<List<ReseñaDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _reseñaService.ObtenerTodasLasReseñas();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener las reseñas.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("ReseñaID")]
        public async Task<IActionResult> ListaReseñaporId(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var rsp = new Response<ReseñaDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _reseñaService.ObtenerReseñaPorId(id);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener la reseña por ID");
            }
            return Ok(rsp);
        }

        [HttpPost]
        [Route("CrearReseña")]
        public async Task<IActionResult> CrearReseña([FromBody] ReseñaDTO reseñaDTO)
        {
            var idMaximo = await _context.Reseñapaginas.MaxAsync(r => r.IdReseñaP) + 1;
            var rsp = new Response<string>();
            try
            {
                if (reseñaDTO.IdReseñaP == 0)
                {
                    reseñaDTO.IdReseñaP = idMaximo;
                    rsp.status = true;
                    var resultado = await _reseñaService.CrearReseña(reseñaDTO);
                    rsp.value = "Reseña creada con exito";
                }
                else
                {
                    rsp.status = false;
                    rsp.value = "Error al guardar reseña";
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