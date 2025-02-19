using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaApoyo.DTO;
using SistemaApoyo.Model;

namespace SistemaApoyo.BLL.Servicios.Contrato
{
    public interface IRespuestaService
    {
        Task<List<RespuestaDTO>> ConsultarRespuesta();
        Task<RespuestaDTO> ObteneRespuestarPorId(int id);
        Task<bool> CrearRespuesta(RespuestaDTO respuestas);
    }
}
