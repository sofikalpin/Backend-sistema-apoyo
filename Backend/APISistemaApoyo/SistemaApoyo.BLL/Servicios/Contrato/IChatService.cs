using SistemaApoyo.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaApoyo.BLL.Servicios.Contrato
{
    public interface IChatService
    {
        Task<List<UsuarioDTO>> ListaContactos();
        Task<IEnumerable<ChatDTO>> ObtenerChatsPorUsuarioId(int userId);
        Task<bool> CrearChat(ChatDTO chat);
        Task<ChatDTO> ObtenerChatPorId(int chatId);
    }
}