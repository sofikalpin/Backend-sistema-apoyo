using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaApoyo.API.Utilidad;
using SistemaApoyo.BLL.Servicios;
using System.Text.Json.Nodes;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using SistemaApoyo.DAL.DBContext;
using SistemaApoyo.Model;

[Route("API/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ILogger<UsuarioController> _logger;
    private readonly IConfiguration _configuration;
    private readonly RailwayContext _context;

    public UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger, IConfiguration configuration, RailwayContext context)
    {
        _usuarioService = usuarioService;
        _logger = logger;
        _configuration = configuration;
        _context = context;
    }

    [HttpGet("ListaUsuarios")]
    public async Task<IActionResult> Lista()
    {
        var rsp = new Response<List<UsuarioDTO>>();
        try
        {
            rsp.status = true;
            rsp.value = await _usuarioService.Lista();
        }
        catch (Exception ex)
        {
            rsp.status = false;
            rsp.msg = "Error al obtener la lista de usuarios.";
            _logger.LogError(ex, "Error al obtener la lista de usuarios.");
            return StatusCode(500, rsp);
        }
        return Ok(rsp);
    }

    [HttpPost("IniciarSesion")]
    public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO login)
    {
        var rsp = new Response<UsuarioDTO>();
        try
        {
            if (login == null || string.IsNullOrEmpty(login.Correo) || string.IsNullOrEmpty(login.ContrasenaHash))
            {
                rsp.status = false;
                rsp.msg = "El correo y la contraseña son obligatorios.";
                return BadRequest(rsp);
            }

            var sesion = await _usuarioService.ValidarCredencialesAsync(login.Correo, login.ContrasenaHash);

            if (sesion == null)
            {
                rsp.status = false;
                rsp.msg = "Correo o contraseña incorrectos.";
                return Unauthorized(rsp);
            }

            // Generar un token
            var token = GenerarToken(sesion.Correo);

            rsp.status = true;
            rsp.value = sesion;
            rsp.token = token;
            rsp.msg = "Inicio de sesión exitoso.";
            return Ok(rsp);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al iniciar sesión para correo: {Correo}", login?.Correo);
            return StatusCode(500, new Response<UsuarioDTO>
            {
                status = false,
                msg = "Error interno del servidor. Intente nuevamente más tarde."
            });
        }
    }

    [HttpGet]
    [Route("BuscarUsuario")]
    public async Task<IActionResult> ObtenerUsuario(int idUsuario)
    {
        var rsp = new Response<UsuarioDTO>();
        try
        {
            if (idUsuario == null || idUsuario < 0)
            {
                rsp.status = false;
                rsp.msg = "El dato de idUsuario son obligatorios.";
                return BadRequest(rsp);
            }

            var usuario = await _usuarioService.ObtenerUsuarioPorID(idUsuario);

            if (usuario == null)
            {
                rsp.status = false;
                rsp.msg = "Id ingresado incorrecto.";
                return Unauthorized(rsp);
            }

            rsp.status = true;
            rsp.value = usuario;
            return Ok(rsp);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener datos");
            return StatusCode(500, new Response<UsuarioDTO>
            {
                status = false,
                msg = "Error interno del servidor"
            });
        }
    }

    [HttpPost]
    [Route("InformacionUsuario")]
    public async Task<IActionResult> InformacionUsuario([FromBody] LoginDTO login) 
    {
        var rsp = new Response<UsuarioDTO>();
        try
        {
            if (login == null || string.IsNullOrEmpty(login.Correo) || string.IsNullOrEmpty(login.ContrasenaHash))
            {
                rsp.status = false;
                rsp.msg = "El correo y la contraseña son obligatorios.";
                return BadRequest(rsp);
            }

            var usuario = await _usuarioService.ObtenerUsuarioPorCorreo(login.Correo);

            if (usuario == null)
            {
                rsp.status = false;
                rsp.msg = "Correo o contraseña incorrectos.";
                return Unauthorized(rsp);
            }

            rsp.status = true;
            rsp.value = usuario;
            return Ok(rsp);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al iniciar sesión para correo: {Correo}", login?.Correo);
            return StatusCode(500, new Response<SesionDTO>
            {
                status = false,
                msg = "Error interno del servidor"
            });
        }
    }
    private string GenerarToken(string correo)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]);

        // Asegúrate de que la clave tenga al menos 128 bits (16 bytes)
        if (key.Length < 16)
        {
            throw new ArgumentException("La clave secreta para JWT debe tener al menos 128 bits (16 bytes).");
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Email, correo) }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    [HttpPost("GuardarUsuario")]
    public async Task<IActionResult> Guardar([FromBody] UsuarioDTO usuario)
    {
        var rsp = new Response<UsuarioDTO>();
        try
        {
            var idMaximo = (await _context.Usuarios.MaxAsync(c => (int?)c.Idusuario) ?? 0) + 1;

            if (string.IsNullOrEmpty(usuario.ContraseñaHash))
            {
                rsp.status = false;
                rsp.msg = "La contraseña no puede estar vacía.";
                return BadRequest(rsp);
            }

            var listausuario = await _usuarioService.Lista();

            if (listausuario.Any(u => u.Correo == usuario.Correo))
            {
                rsp.status = false;
                rsp.msg = "El usuario debe tener un mail diferente.";
                return BadRequest(rsp);
            }

            if (usuario.Idrol == 1)
            {
                usuario.AutProf = false;
            }
            else
            {
                usuario.AutProf = null;
            } 
            
            usuario.Idusuario = idMaximo;
            rsp.status = true;
            rsp.value = await _usuarioService.Crear(usuario);
        }
        catch (Exception ex)
        {
            rsp.status = false;
            rsp.msg = "Ocurrió un error inesperado al intentar guardar el usuario.";
            _logger.LogError(ex, "Error al guardar usuario con correo: {Correo}", usuario.Correo);
            return StatusCode(500, rsp);
        }
        return Ok(rsp);
    }

    [HttpPut("EditarUsuario")]
    public async Task<IActionResult> Editar(int id, [FromBody] UsuarioDTO usuario)
    {
        if (id != usuario.Idusuario)
        {
            _logger.LogWarning("El ID de la URL no coincide con el ID del cuerpo.");
            return BadRequest("El ID del alumno no coincide con el ID proporcionado.");
        }

        var rsp = new Response<string>();
        try
        {
            var resultado = await _usuarioService.Editar(usuario);
            rsp.status = resultado;
            if (!resultado)
            {
                rsp.msg = "No se pudo editar el usuario.";
                return BadRequest(rsp);
            }
        }
        catch (Exception ex)
        {
            rsp.status = false;
            rsp.msg = "Error al editar el usuario.";
            _logger.LogError(ex, "Error al editar el usuario.");
            return StatusCode(500, rsp);
        }
        return Ok(rsp);
    }

    [HttpPost("SolicitudToken")]
    public async Task<IActionResult> SolicitarRecuperacionContraseña([FromBody] string correo)
    {
        var rsp = new Response<bool>();
        try
        {
            if (string.IsNullOrEmpty(correo))
            {
                rsp.status = false;
                rsp.msg = "El correo es requerido.";
                return BadRequest(rsp);
            }

            bool resultado = await _usuarioService.GenerarTokenRecuperacion(correo);

            if (resultado)
            {
                rsp.status = true;
                rsp.msg = "Se ha enviado un correo con las instrucciones para recuperar tu contraseña.";
                return Ok(rsp);
            }

            rsp.status = false;
            rsp.msg = "No se pudo procesar la solicitud de recuperación.";
            return BadRequest(rsp);
        }
        catch (InvalidOperationException ex)
        {
            rsp.status = false;
            rsp.msg = ex.Message;
            return NotFound(rsp);
        }
        catch (Exception ex)
        {
            rsp.status = false;
            rsp.msg = "Error interno del servidor.";
            _logger.LogError(ex, "Error en SolicitarRecuperacionContraseña: {Message}", ex.Message);
            return StatusCode(500, rsp);
        }
    }

    [HttpPost("ReestablecerContrasena")]
    public async Task<IActionResult> ReestablecerContraseña([FromBody] ReestablecerContraseñaDTO modelo)
    {
        var rsp = new Response<bool>();
        try
        {
            if (string.IsNullOrEmpty(modelo.Token) || string.IsNullOrEmpty(modelo.NuevaContraseña))
            {
                rsp.status = false;
                rsp.msg = "El token y la nueva contraseña son requeridos.";
                return BadRequest(rsp);
            }

            bool resultado = await _usuarioService.ReestablecerContraseña(modelo.Token, modelo.NuevaContraseña);

            if (resultado)
            {
                rsp.status = true;
                rsp.msg = "Contraseña reestablecida exitosamente.";
                return Ok(rsp);
            }

            rsp.status = false;
            rsp.msg = "No se pudo reestablecer la contraseña.";
            return BadRequest(rsp);
        }
        catch (InvalidOperationException ex)
        {
            rsp.status = false;
            rsp.msg = ex.Message;
            return BadRequest(rsp);
        }
        catch (Exception ex)
        {
            rsp.status = false;
            rsp.msg = "Error interno del servidor.";
            _logger.LogError(ex, "Error en ReestablecerContraseña: {Message}", ex.Message);
            return StatusCode(500, rsp);
        }
    }

    [HttpPost("SubirCV")]
    public async Task<IActionResult> SubirCV(int idUsuario, IFormFile archivo)
    {
        var rsp = new Response<string>();

        if (archivo == null || archivo.Length == 0)
        {
            rsp.status = false;
            rsp.msg = "Debe seleccionar un archivo.";
            return BadRequest(rsp);
        }

        
        var allowedExtensions = new[] { ".pdf" };
        var fileExtension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(fileExtension))
        {
            rsp.status = false;
            rsp.msg = "Solo se permiten archivos PDF.";
            return BadRequest(rsp);
        }

        try
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorID(idUsuario);
            if (usuario == null || usuario.Idrol != 1)
            {
                rsp.status = false;
                rsp.msg = "Usuario no encontrado o el usuario no corresponde a un profesor.";
                return NotFound(rsp);
            }

            // Definir la carpeta donde se guardarán los CVs (dentro de wwwroot/uploads)
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Crear el nombre del archivo único (cv_{id}.pdf)
            string nombreArchivo = $"cv_{idUsuario}{fileExtension}";
            string rutaCompleta = Path.Combine(uploadsFolder, nombreArchivo);

            // Guardar el archivo en el servidor
            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            // Actualizar solo la ruta del CV en el usuario
            string rutaRelativa = $"/uploads/{nombreArchivo}";

            // Llamar al servicio para actualizar solo el campo CvRuta
            var actualizado = await _usuarioService.ActualizarCvRuta(usuario.Idusuario, rutaRelativa);
            if (!actualizado)
            {
                if (System.IO.File.Exists(rutaCompleta))
                {
                    System.IO.File.Delete(rutaCompleta);
                }

                rsp.status = false;
                rsp.msg = "Error al actualizar la base de datos.";
                return StatusCode(500, rsp);
            }

            rsp.status = true;
            rsp.msg = "CV subido exitosamente.";
            rsp.value = rutaRelativa;
            return Ok(rsp);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al subir el CV para usuario {idUsuario}", idUsuario);
            rsp.status = false;
            rsp.msg = "Error interno al subir el CV.";
            return StatusCode(500, rsp);
        }
    }

    [HttpGet("ObtenerCV")]
    public async Task<IActionResult> ObtenerCV(int idUsuario)
    {
        var rsp = new Response<string>();

        try
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Idusuario == idUsuario);
            if (usuario == null || string.IsNullOrEmpty(usuario.CvRuta))
            {
                rsp.status = false;
                rsp.msg = "CV no encontrado.";
                return NotFound(rsp);
            }

            string rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", usuario.CvRuta.TrimStart('/'));
            _logger.LogError(rutaArchivo);
            if (!System.IO.File.Exists(rutaArchivo))
            {
                return NotFound(new Response<string> { status = false, msg = "El archivo no existe en el servidor." });
            }

            rsp.status = true;

            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(rutaArchivo);
            return File(fileBytes, "application/pdf", Path.GetFileName(usuario.CvRuta));

        }
        catch (Exception ex)
        {
            rsp.status = false;
            rsp.msg = "Error al obtener el CV.";
            _logger.LogError(ex, "Error al obtener el CV del usuario {IdUsuario}", idUsuario);
            return StatusCode(500, rsp);
        }
    }

    [HttpPost("ActualizarFoto")]
    public async Task<IActionResult> ActualizarFoto([FromBody] ActualizarFotoDTO modelo)
    {
        var rsp = new Response<string>();
        try
        {
            if (string.IsNullOrEmpty(modelo.Correo) || string.IsNullOrEmpty(modelo.Foto))
            {
                rsp.status = false;
                rsp.msg = "El correo y la foto son requeridos.";
                return BadRequest(rsp);
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == modelo.Correo);
            if (usuario == null)
            {
                rsp.status = false;
                rsp.msg = "Usuario no encontrado.";
                return NotFound(rsp);
            }

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fotos");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string nombreArchivo = $"foto_{usuario.Idusuario}_{DateTime.Now.Ticks}.jpg";
            string rutaCompleta = Path.Combine(uploadsFolder, nombreArchivo);

            var fotoBytes = Convert.FromBase64String(modelo.Foto.Split(',')[1]);
            await System.IO.File.WriteAllBytesAsync(rutaCompleta, fotoBytes);

            usuario.FotoRuta = Path.Combine("fotos", nombreArchivo);
            await _context.SaveChangesAsync();

            rsp.status = true;
            rsp.msg = "Foto actualizada exitosamente.";
            rsp.value = usuario.FotoRuta;
            return Ok(rsp);
        }
        catch (Exception ex)
        {
            rsp.status = false;
            rsp.msg = "Error al actualizar la foto.";
            _logger.LogError(ex, "Error al actualizar foto para usuario {Correo}", modelo.Correo);
            return StatusCode(500, rsp);
        }
    }

    [HttpGet("ObtenerFoto/{correo}")]
    public async Task<IActionResult> ObtenerFoto(string correo)
    {
        try
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
            if (usuario == null || string.IsNullOrEmpty(usuario.FotoRuta))
            {
                return NotFound(new Response<string> { status = false, msg = "Foto no encontrada." });
            }

            string rutaCompleta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", usuario.FotoRuta);
            if (!System.IO.File.Exists(rutaCompleta))
            {
                return NotFound(new Response<string> { status = false, msg = "Archivo no encontrado." });
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(rutaCompleta);
            return File(bytes, "image/jpeg");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener foto para usuario {Correo}", correo);
            return StatusCode(500, new Response<string> { status = false, msg = "Error al obtener la foto." });
        }
    }
}


