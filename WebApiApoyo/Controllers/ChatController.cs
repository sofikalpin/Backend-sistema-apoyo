using Microsoft.AspNetCore.Mvc;
using SistemaApoyo.API.Utilidad;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using Microsoft.AspNetCore.SignalR;
using SistemaApoyo.BLL.Hubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SistemaApoyo.BLL.Servicios;
using SistemaApoyo.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SistemaApoyo.Model;

namespace WebApiApoyo.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IUsuarioService _usuarioService;
        private readonly IHubContext<ChatHub> _chatHubContext;
        private readonly ILogger<ChatController> _logger;
        private readonly RailwayContext _context;

        public ChatController(IChatService chatService, IUsuarioService usuarioService, IHubContext<ChatHub> chatHubContext, ILogger<ChatController> logger, RailwayContext context)
        {
            _chatService = chatService;
            _usuarioService = usuarioService;
            _chatHubContext = chatHubContext;
            _logger = logger;
            _context = context;

        }

        [HttpGet]
        [Route("ListaContactos")]
        public async Task<IActionResult> ListaContactos() 
        {
            var general = await _chatService.ListaContactos();

            if (general == null || !general.Any())
            {
                return Ok(new { status = false, msg = "No se encontraron usuarios.", value = (object)null });
            }

            var contactos = general.Where(u => u.AutProf != false && u.Idrol != 3).ToList();
            if (!contactos.Any())
            {
                return Ok(new { status = false, msg = "No se encontraron usuarios.", value = (object)null });
            }

            return Ok(new { status = true, msg = "Usuarios encontrados.", value = contactos });
        }

        [HttpGet]
        [Route("ChatporUsuarioID")]
        public async Task<IActionResult> ObtenerChatsPorUsuarioId(int userId)
        {
            if (userId < 0)
            {
                return BadRequest(new { status = false, msg = "El UserId debe ser un número positivo." });
            }

            var rsp = new Response<IEnumerable<ChatDTO>>();

            try
            {
                var chats = await _chatService.ObtenerChatsPorUsuarioId(userId);
                rsp.status = true;
                rsp.value = chats;
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

        [HttpGet]
        [Route("ChatporID")]
        public async Task<IActionResult> ObtenerChatPorId(int chatId)
        {
            if (chatId <= 0)
            {
                return BadRequest(new { status = false, msg = "El ChatId debe ser un número positivo." });
            }

            var rsp = new Response<ChatDTO>();

            try
            {
                var chat = await _chatService.ObtenerChatPorId(chatId);
                rsp.status = true;
                rsp.value = chat;
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = "Ocurrió un error al obtener el chat.";
                _logger.LogError(ex, rsp.msg);
                return StatusCode(500, rsp);
            }
            return Ok(rsp);
        }


        [HttpPost]
        [Route("CrearChat")]
        public async Task<IActionResult> CrearChat([FromBody] ChatDTO chatDTO)
        {
            if (chatDTO == null || chatDTO.Idusuario1 <= 0 || chatDTO.Idusuario2 <= 0)
            {
                return BadRequest(new { status = false, msg = "El objeto ChatDTO no puede ser nulo." });
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
    }
}
