using SistemaApoyo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.BLL.Servicios.Contrato
{
    public interface IMensajeService
    {
        Task<IEnumerable<MensajeDTO>> ObtenerMensajesPorChatId(int chatId, int pageNumber, int pageSize);
        Task<IEnumerable<MensajeDTO>> ObtenerHistorialChat(int chatId, int usuarioId);
        Task<MensajeDTO> EnviarMensaje(MensajeDTO mensajeDto);
        Task<MensajeDTO> EditarMensaje(int mensajeId, MensajeDTO mensajeDto);

    }
}
