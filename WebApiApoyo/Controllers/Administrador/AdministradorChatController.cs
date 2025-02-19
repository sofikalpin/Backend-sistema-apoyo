using Microsoft.AspNetCore.Mvc;
using SistemaApoyo.API.Utilidad;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using Microsoft.AspNetCore.Http;
using SistemaApoyo.BLL.Servicios;
using SistemaApoyo.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using SistemaApoyo.BLL.Hubs;
using Npgsql;
using SistemaApoyo.Model;

namespace WebApiApoyo.Controllers.Administrador
{

    [Route("API/[controller]")]
    [ApiController]

    public class AdministradorChatController : ControllerBase
    {
        private readonly IAdministrador _administradorService;
        private readonly IChatService _chatService;
        private readonly IMensajeService _mensajeService;
        private readonly IHubContext<ChatHub> _chatHubContext;
        private readonly ILogger<AdministradorChatController> _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly RailwayContext _context;
            
        public AdministradorChatController(IAdministrador administradorServie, ILogger<AdministradorChatController> logger, IUsuarioService usuarioService, IMensajeService mensajeService, IChatService chatService, IHubContext<ChatHub> chatHubContext, RailwayContext context)
        {
            _administradorService = administradorServie;
            _context = context;
            _logger = logger;
            _usuarioService = usuarioService;
            _chatService = chatService;
            _mensajeService = mensajeService;
            _chatHubContext = chatHubContext;
        }

        [HttpGet]
        [Route("Contactos")]
        public async Task<IActionResult> ListaContacto()
        {
            var general = await _administradorService.ListaTotal();

            if (general == null || !general.Any())
            {
                return Ok(new { status = false, msg = "No se encontraron usuarios.", value = (object)null });
            }

            var contactos = general.Where(u => u.AutProf != false).ToList();
            if (!contactos.Any())
            {
                return Ok(new { status = false, msg = "No se encontraron usuarios.", value = (object)null });
            }

            return Ok(new { status = true, msg = "Usuarios encontrados.", value = contactos });
        }

        [HttpGet]
        [Route("ListaChats")]
        public async Task<IActionResult> ListaChats(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return BadRequest(new { status = false, msg = "El id del usuario debe ser un número positivo." });
            }

            var rsp = new Response<IEnumerable<ChatDTO>>();

            try
            {
                var listachats = await _chatService.ObtenerChatsPorUsuarioId(idUsuario);
                rsp.status = true;
                rsp.value = listachats;
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = "Ocurrió un error al obtener los chats.";
                _logger.LogError(ex, rsp.msg);
                return StatusCode(500, rsp);

            }
        }

        [HttpPost]
        [Route("CrearChat")]
        public async Task<IActionResult> CrearChat([FromBody] ChatDTO chatDTO)
        {

            if (chatDTO == null || chatDTO.Idusuario1 <= 0 || chatDTO.Idusuario2 <= 0)
            {
                return BadRequest(new { status = false, msg = "Los datos del chat son inválidos." });
            }

            var usuario1 = await _usuarioService.ObtenerUsuarioPorID(chatDTO.Idusuario1.Value);
            if (usuario1 == null)
            {
                return NotFound(new { status = false, msg = "Usuario 1 no encontrado." });
            }
            string nombre1 = usuario1.Nombrecompleto;

            var usuario2 = await _usuarioService.ObtenerUsuarioPorID(chatDTO.Idusuario2.Value);
            if (usuario2 == null)
            {
                return NotFound(new { status = false, msg = "Usuario 2 no encontrado." });
            }
            string nombre2 = usuario2.Nombrecompleto;

            _logger.LogInformation("Datos recibidos: {@ChatDTO}", chatDTO);
            var rsp = new Response<bool>();

            try
            {
                var idMaximo = (await _context.Chats.MaxAsync(c => (int?)c.Idchat) ?? 0) + 1;


                var nuevochat = new ChatDTO
                {
                    Idchat = idMaximo,
                    Idusuario1 = chatDTO.Idusuario1,
                    Idusuario2 = chatDTO.Idusuario2,
                    FechahoraInicio = DateTime.UtcNow,
                    Mensajes = new List<MensajeDTO>()
                    {
                        new MensajeDTO
                        {
                            Contenido = $"Chat iniciado entre los usuarios {nombre1} y {nombre2}",
                            Idusuario = chatDTO.Idusuario1.GetValueOrDefault(),
                            Idchat = idMaximo,
                        }
                    }
                };

                var chatCreado = await _chatService.CrearChat(nuevochat);
                rsp.status = true;
                rsp.value = chatCreado;
                return Ok(rsp);
            }
            catch (DbUpdateException dbEx) when (dbEx.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
            {
                rsp.status = false;
                rsp.msg = "Ya existe un chat con este ID o con datos duplicados.";
                _logger.LogError(dbEx, rsp.msg);
                return Conflict(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = "Ocurrió un error al crear el chat.";
                _logger.LogError(ex, rsp.msg);
                return StatusCode(500, rsp);
            }
        }

        [HttpGet]
        [Route("MensajesPorChat")]
        public async Task<IActionResult> MensajesPorChat(int IdChat, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest(new Response<string>
                {
                    status = false,
                    msg = "Los parámetros de paginación deben ser mayores a cero."
                });
            }

            var rsp = new Response<IEnumerable<MensajeDTO>>();

            try
            {
                var mensajes = await _mensajeService.ObtenerMensajesPorChatId(IdChat, pageNumber, pageSize);
                rsp.status = true;
                rsp.value = mensajes;
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = "Ocurrió un error al obtener los mensajes.";
                _logger.LogError(ex, rsp.msg);
                return StatusCode(500, rsp);
            }
        }

        [HttpPost]
        [Route("EnviarMensaje")]
        public async Task<IActionResult> EnviarMensaje([FromBody] MensajeDTO mensajeDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new Response<string>
                {
                    status = false,
                    msg = "Datos inválidos"
                });
            }

            if (mensajeDTO.Idchat <= 0 || mensajeDTO.Idusuario <= 0 || string.IsNullOrEmpty(mensajeDTO.Contenido))
            {
                return BadRequest(new { status = false, msg = "Los datos del mensaje son inválidos." });
            }


            var rsp = new Response<MensajeDTO>();

            try
            {
                mensajeDTO.Idmensaje = 0;
                rsp.status = true;
                var mensajeEnviado = await _mensajeService.EnviarMensaje(mensajeDTO);
                Console.WriteLine("Mensaje recibido:", mensajeDTO);
                rsp.value = mensajeEnviado;

                await _chatHubContext.Clients.Group(mensajeDTO.Idchat.ToString()).SendAsync("RecibirMensaje", mensajeEnviado);

                return Ok(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = "Ocurrió un error al enviar el mensaje.";
                _logger.LogError(ex, rsp.msg);
                return StatusCode(500, rsp);
            }
        }

    }
}
