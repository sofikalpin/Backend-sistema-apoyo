using Microsoft.AspNetCore.Mvc;
using SistemaApoyo.API.Utilidad;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using Microsoft.AspNetCore.Http;
using SistemaApoyo.BLL.Servicios;
using SistemaApoyo.Model;
using Microsoft.EntityFrameworkCore;
using SistemaApoyo.DAL.DBContext;

namespace WebApiApoyo.Controllers.Profesor
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfeExamenController : ControllerBase
    {
        private readonly IProfesorExamen _profesorExamen;
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<ProfeExamenController> _logger;
        private readonly RailwayContext _context;

        public ProfeExamenController(IProfesorExamen profesorExamen, ILogger<ProfeExamenController> logger, RailwayContext context, IUsuarioService usuarioService)
        {
            _context = context;
            _profesorExamen = profesorExamen;
            _logger = logger;
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Route("ListaExamenes")]
        public async Task<IActionResult> ListarExamenes()
        {
            var rsp = new Response<List<ExamenDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _profesorExamen.ConsultarExamen();
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener los examenes.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("TituloExamen")]
        public async Task<IActionResult> ListaExamenPorTitulo(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                return BadRequest("El titulo no es valido");
            }

            var rsp = new Response<List<ExamenDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _profesorExamen.ConsultarPorTitulo(titulo);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el titulo del examen.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("ExamenIDProfesor")]
        public async Task<IActionResult> ListaExamenPorId(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es valido.");
            }

            var rsp = new Response<List<ExamenDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _profesorExamen.ObteneExamenrPorIdProfesor(id);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el id del examen");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("ExamenPorProfeyNivel")]
        public async Task<IActionResult> ListaExamenPorProfesorYNivel(int idUsuario, int idNivel)
        {
            if (idUsuario <= 0 || idNivel <= 0)
            {
                return BadRequest("El ID del nivel o usuario proporcionado no es válido.");
            }

            var rsp = new Response<List<ExamenDTO>>();

            var listaExamenProfe = await _profesorExamen.ObteneExamenrPorIdProfesor(idUsuario);

            if (listaExamenProfe == null || !listaExamenProfe.Any())
            {
                return NotFound("No se encontraron actividades para el profesor indicado.");
            }

            var listaFinal = listaExamenProfe.Where(a => a.Idnivel == idNivel).ToList();

            try
            {
                rsp.status = true;
                rsp.value = listaFinal;
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el id del examen");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }

        [HttpPost]
        [Route("CrearExamen")]
        public async Task<IActionResult> CrearExamen([FromBody] ExamenDTO examen)
        {
            var idMaximo = await _context.Examen.MaxAsync(e => e.Idexamen) + 1;
            
            var rsp = new Response<string>();
            try
            {
                if ( examen.Idexamen == 0) 
                { 
                    examen.Idexamen = idMaximo;
                    rsp.status = true;
                    var resultado = await _profesorExamen.CrearExamen(examen);
                    rsp.value = "Examen creado con éxito";
                }
                else
                {
                    rsp.status = false;
                    rsp.value = "El valor de IdExamen debe ser cero";
                }
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al guardar el examen.");
            }
            return Ok(rsp);
        }

        [HttpPut]
        [Route("EditarporID")]
        public async Task<IActionResult> EditarActividad(int id, [FromBody] ExamenDTO examen)
        {
            if (id != examen.Idexamen)
            {
                return BadRequest("El ID del examen no coincide con el ID proporcionado.");
            }

            var rsp = new Response<string>();
            try
            {
                var resultado = await _profesorExamen.ActualizarExamen(examen);
                rsp.status = true;
                rsp.value = "Examen actualizado con éxito.";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al actualizar el examen.");
            }
            return Ok(rsp);
        }

        [HttpDelete]
        [Route("EliminarExamen")]
        public async Task<IActionResult> EliminarExamen(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var rsp = new Response<string>();

            try
            {
                var eli = await _profesorExamen.EliminarExamen(id);
                rsp.status = true;
                rsp.value = "Se elimino el examen con exito.";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al eliminar el examen");
            }
            return Ok(rsp);
        }

    }
}