using Microsoft.AspNetCore.Mvc;
using SistemaApoyo.API.Utilidad;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using SistemaApoyo.DAL.DBContext;
using SistemaApoyo.BLL.Servicios;
using Microsoft.EntityFrameworkCore;
using SistemaApoyo.Model;

namespace WebApiApoyo.Controllers.Administrador
{
    [Route("API/[controller]")]
    [ApiController]

    public class AdministradorAlumnoController : ControllerBase
    {
        private readonly IAdministrador _administrador;
        private readonly ILogger<AdministradorAlumnoController> _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly RailwayContext _context;

        public AdministradorAlumnoController(IAdministrador administrador, ILogger<AdministradorAlumnoController> logger, IUsuarioService usuarioService, RailwayContext context)
        {
            _administrador = administrador;
            _logger = logger;
            _usuarioService = usuarioService;
            _context = context;
        }

        [HttpGet("ListaAlumnos")]
        public async Task<IActionResult> ListaAlumnos()
        {
            var rsp = new Response<List<UsuarioDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _administrador.ListaRol(2);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener la lista de alumnos.");
            }

            return Ok(rsp);
        }

        [HttpPut]
        [Route("BusquedaAlumnoNombre")]
        public async Task<IActionResult> ListaAlumnoPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return BadRequest("El nombre no es  válido.");
            }

            var rsp = new Response<List<UsuarioDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _administrador.ConsultaNombre(2, nombre);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el nombre del alumno.");
            }
            return Ok(rsp);
        }

        [HttpGet]
        [Route("AlumnoID")]
        public async Task<IActionResult> BusquedaAlumnoID(int id)
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
                var usuario = await _administrador.ObtenerUsuarioId(id);

                if (usuario == null)
                {
                    return NotFound("El usuario no existe.");
                }

                if (usuario.Idrol != 2)
                {
                    return BadRequest("El usuario no tiene el rol de alumno.");
                }

                rsp.value = usuario;
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al obtener el id del alumno");
                return StatusCode(500, "Ocurrió un error interno del servidor.");
            }

            return Ok(rsp);
        }

        [HttpPost]
        [Route("CrearAlumno")]
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

                if (string.IsNullOrEmpty(usuario.ContraseñaHash))
                {
                    rsp.status = false;
                    rsp.msg = "Error al hashear la contraseña.";
                    return BadRequest(rsp);
                }

                rsp.status = true;
                var resultado = await _administrador.CrearUsuario(usuario);
                usuario.Idrol = 2;
                rsp.value = "Alumno creado con éxito";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = "Ocurrió un error inesperado al intentar guardar el usuario.";
                _logger.LogError(ex, "Error al guardar el alumno.");
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
                return BadRequest("El ID del alumno no coincide con el ID proporcionado.");
            }

            var rsp = new Response<string>();
            try
            {
                var resultado = await _administrador.ActualizarUsuario(usuario);
                if (resultado)
                {
                    return Ok(new
                    {
                        status = true,
                        message = "Alumno actualizado con éxito."
                    });
                }

                return BadRequest(new
                {
                    status = false,
                    message = "No se pudo actualizar el alumno. Verifique los datos enviados."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el alumno.");
                return StatusCode(500, new
                {
                    status = false,
                    message = "Ocurrió un error al procesar la solicitud. Intente nuevamente más tarde."
                });
            }
        }

        [HttpDelete]
        [Route("EliminarAlumno")]
        public async Task<IActionResult> EliminarAlumno(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID proporcionado no es válido.");
            }

            var rsp = new Response<string>();

            try
            {
                var eli = await _administrador.EliminarUsuario(id);
                if (eli)
                {
                    rsp.status = true;
                    rsp.value = "Se elimino el alumno con exito.";
                }
                else
                {
                    rsp.status = false;
                    rsp.msg = "No se pudo eliminar el alumno.";
                }
                   
            }
            catch (Exception ex)
            {
                rsp.status = false;
                _logger.LogError(ex, "Error al eliminar el alumno");
            }
            return Ok(rsp);
        }
    }
}
