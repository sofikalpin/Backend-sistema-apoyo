using SistemaApoyo.DTO;
using SistemaApoyo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SistemaApoyo.BLL.Servicios.Contrato
{
    public interface IReseñaService
    {
        Task<ReseñaDTO> ObtenerReseñaPorId(int id);
        Task<List<ReseñaDTO>> ObtenerTodasLasReseñas();
        Task<List<ReseñaDTO>> ObtenerReseñasDeUsuario(int idUsuario);
        Task<bool> CrearReseña(ReseñaDTO reseñaDTO);

    }
}
