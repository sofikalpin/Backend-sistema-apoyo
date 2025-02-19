using Microsoft.AspNetCore.Mvc;
using SistemaApoyo.API.Utilidad;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using Microsoft.AspNetCore.Http;
using SistemaApoyo.BLL.Servicios;
using SistemaApoyo.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using SistemaApoyo.Model;

namespace WebApiApoyo.Controllers.Administrador
{
    [Route("API/[controller]")]
    [ApiController]

    public class AdministradorProfesorController : ControllerBase
    {
        private readonly IAdministrador _administradorService;
        private readonly ILogger<AdministradorProfesorController> _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly RailwayContext _context;

        public AdministradorProfesorController(IAdministrador administradorService, IUsuarioService usuarioService, ILogger<AdministradorProfesorController> logger, RailwayContext context)
        {
            _administradorService = administradorService;
            _usuarioService = usuarioService;
            _logger = logger;
            _context = context;
        }

        [HttpGet("ListaProfesores")]
        public async Task<IActionResult> ListaProfesores()
        {
            var general = await _administradorService.ListaTotal();

            if (general == null || !general.Any())
            {
                return Ok(new { status = false, msg = "No se encontraron profesores.", value = (object)null });
            }

            var profesores = general.Where(u => u.Idrol == 1).ToList();
            if (!profesores.Any())
            {
                return Ok(new { status = false, msg = "No se encontraron profesores.", value = (object)null });
            }

            return Ok(new { status = true, msg = "Profesores encontrados.", value = profesores });
        }

        [HttpGet("ListaProfesoresAutorizados")]
        public async Task<IActionResult> ListaAutorizados()
        {
            var rsp = new Response<List<UsuarioDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _administradorService.ListaAutorizacion(true);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener la lista de profesores autorizados.");
            }

            return Ok(rsp);
        }

        [HttpGet("ListaProfesoresNOAutorizados")]
        public async Task<IActionResult> ListaNoAutorizados()
        {
            var rsp = new Response<List<UsuarioDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _administradorService.ListaAutorizacion(false);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener la lista de profesores autorizados.");
            }

            return Ok(rsp);
        }

        [HttpPut]
        [Route("AutorizarProfesor")]
        public async Task<IActionResult> AutorizarProfesor(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var rsp = new Response<string>();

            try
            {
                var eli = await _administradorService.AutorizarProfesor(id);
                rsp.status = true;
                rsp.value = "Se autorizo el profesor con exito.";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al autorizar el profesor");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("BusquedaProfesorNombre")]
        public async Task<IActionResult> ListaProfesorPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest("El nombre no es  válido.");
            }

            var rsp = new Response<List<UsuarioDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _administradorService.ConsultaNombre(1, nombre);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el nombre del profesor.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("ProfesorID")]
        public async Task<IActionResult> BusquedaProfesorID(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var rsp = new Response<UsuarioDTO>();
            try
            {
                rsp.status = true;

                // Obtener el usuario por ID
                var usuario = await _administradorService.ObtenerUsuarioId(id);

                if (usuario == null)
                {
                    return NotFound("El usuario no existe.");
                }

                if (usuario.Idrol != 1)
                {
                    return BadRequest("El usuario no tiene el rol de profesor.");
                }

                rsp.value = usuario;
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el id del profesor");
                return StatusCode(500, "Ocurrió un error interno del servidor.");
            }

            return Ok(rsp);
        }


        [HttpPost]
        [Route("CrearProfesor")]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioDTO usuario)
        {
            var idMaximo = await _context.Usuarios.MaxAsync(u => u.Idusuario);
            usuario.Idusuario = idMaximo + 1;

            var rsp = new Response<string>();
            try
            {

                _logger.LogInformation("Hashing password: {PasswordHash}", usuario.ContraseñaHash);

                if (string.IsNullOrEmpty(usuario.ContraseñaHash))
                {
                    rsp.status = false;
                    rsp.msg = "La contraseña no puede estar vacía.";
                    return BadRequest(rsp);
                }

                // Hashear la contraseña antes de guardar
                usuario.ContraseñaHash = _usuarioService.CubreContrasena(usuario.ContraseñaHash);
                
                // Todo profesor que se cree tanto por registro como administrador, no tendra autorizacion hasta que lo permita el administrador

                if (string.IsNullOrEmpty(usuario.ContraseñaHash))
                {
                    rsp.status = false;
                    rsp.msg = "Error al hashear la contraseña.";
                    return BadRequest(rsp);
                }

                rsp.status = true;
                usuario.AutProf = false;
                usuario.Idrol = 1;
                var resultado = await _administradorService.CrearUsuario(usuario);


                if (resultado)
                {
                    rsp.status = true;
                    rsp.value = "Profesor creado con éxito";
                    return Ok(rsp);
                }
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = "Ocurrió un error inesperado al intentar guardar el usuario.";
                _logger.LogError(ex, "Error al guardar el profesor.");
            }
            return Ok(rsp);
        }

        [HttpPut]
        [Route("EditarporID")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody] UsuarioDTO usuario)
        {
            _logger.LogInformation("ID de la URL: {Id}", id);
            _logger.LogInformation("ID del usuario en el cuerpo: {IdUsuario}", usuario?.Idusuario);

            if (id != usuario.Idusuario)
            {
                _logger.LogWarning("El ID de la URL no coincide con el ID del cuerpo.");
                return BadRequest("El ID del profesor no coincide con el ID proporcionado.");
            }

            var rsp = new Response<string>();
            try
            {
                usuario.AutProf = true;

                var resultado = await _administradorService.ActualizarUsuario(usuario);
                if (resultado)
                {
                    return Ok(new
                    {
                        status = true,
                        message = "Profesor actualizado con éxito."
                    });
                }

                return BadRequest(new
                {
                    status = false,
                    message = "No se pudo actualizar el profesor. Verifique los datos enviados."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el profesor.");
                return StatusCode(500, new
                {
                    status = false,
                    message = "Ocurrió un error al procesar la solicitud. Intente nuevamente más tarde."
                });
            }
        }

        [HttpDelete]
        [Route("EliminarProfesor")]
        public async Task<IActionResult> EliminarProfesor(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }
            var rsp = new Response<string>();

            try
            {
                var eli = await _administradorService.EliminarUsuario(id);
                rsp.status = true;
                rsp.value = "Se elimino el profesor con exito.";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al eliminar el profesor");
            }
            return Ok(rsp);
        }

    }
}
