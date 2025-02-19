using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SistemaApoyo.BLL.Servicios;
using SistemaApoyo.API.Utilidad;

namespace SistemaApoyo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BolsatrabajoController : Controller
    {
        private readonly IBolsatrabajoService _bolsaTrabajoService;
        private readonly ILogger<BolsatrabajoController> _logger;

        public BolsatrabajoController(IBolsatrabajoService bolsaTrabajoService, ILogger<BolsatrabajoController> logger)
        {
            _bolsaTrabajoService = bolsaTrabajoService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BolsatrabajoDTO dto)
        {
            _logger.LogInformation("Solicitud de creación de bolsa de trabajo recibida: {@DTO}", dto);

            if (dto == null)
            {
                _logger.LogWarning("Datos de la bolsa de trabajo no válidos.");
                return BadRequest("Los datos proporcionados son incorrectos.");
            }

            // Agrega esta validación
            if (dto.Idbolsa != 0)
            {
                _logger.LogWarning("Intento de crear bolsa de trabajo con ID predefinido.");
                return BadRequest("No se debe proporcionar un ID al crear una nueva bolsa de trabajo.");
            }

            var createdBolsaTrabajo = await _bolsaTrabajoService.Create(dto);
            if (createdBolsaTrabajo == null)
            {
                _logger.LogError("Error al crear la bolsa de trabajo.");
                return StatusCode(500, "Ocurrió un error al crear la bolsa de trabajo.");
            }

            _logger.LogInformation("Bolsa de trabajo creada con éxito: {@CreatedBolsaTrabajo}", createdBolsaTrabajo);
            return CreatedAtAction(nameof(GetById), new { id = createdBolsaTrabajo.Idbolsa }, createdBolsaTrabajo);
        }

        // GET: api/Bolsatrabajo
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Solicitud para obtener todas las bolsas de trabajo.");

            var bolsaTrabajoList = await _bolsaTrabajoService.GetAll();

            if (bolsaTrabajoList == null || !bolsaTrabajoList.Any())
            {
                _logger.LogWarning("No se encontraron bolsas de trabajo.");
                return NoContent();
            }

            _logger.LogInformation("Se encontraron {Count} bolsas de trabajo.", bolsaTrabajoList.Count());
            return Ok(bolsaTrabajoList);
        }

        // GET: api/Bolsatrabajo/ingles
        [HttpGet("ingles")]
        public async Task<IActionResult> GetProfesoresIngles()
        {
            _logger.LogInformation("Solicitud para obtener profesores de inglés.");

            var bolsaTrabajoList = await _bolsaTrabajoService.GetProfesoresIngles();

            if (bolsaTrabajoList == null || !bolsaTrabajoList.Any())
            {
                _logger.LogWarning("No se encontraron profesores de inglés.");
                return NoContent();
            }

            _logger.LogInformation("Se encontraron {Count} profesores de inglés.", bolsaTrabajoList.Count());
            return Ok(bolsaTrabajoList);
        }







        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Solicitud para obtener bolsa de trabajo con ID: {Id}", id);

            var bolsaTrabajo = await _bolsaTrabajoService.GetById(id);

            if (bolsaTrabajo == null)
            {
                _logger.LogWarning("No se encontró la bolsa de trabajo con ID: {Id}", id);
                return NotFound("No se encontró la bolsa de trabajo.");
            }

            _logger.LogInformation("Bolsa de trabajo encontrada: {@BolsaTrabajo}", bolsaTrabajo);
            return Ok(bolsaTrabajo);
        }

        [HttpDelete]
        [Route("EliminarBolsa")]
        public async Task<IActionResult> EliminarActividad(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var rsp = new Response<string>();

            try
            {
                var eli = await _bolsaTrabajoService.Eliminar(id);
                rsp.status = true;
                rsp.value = "Se elimino JellyJobs con exito.";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al eliminar JellyJobs");
            }
            return Ok(rsp);
        }


    }
}
