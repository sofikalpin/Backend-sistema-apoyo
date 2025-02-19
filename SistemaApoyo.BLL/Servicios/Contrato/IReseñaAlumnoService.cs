using SistemaApoyo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.BLL.Servicios.Contrato
{
    public interface IReseñaAlumnoService
    {
        Task<List<ReseñaAlumnoDTO>> ObtenerTodasReseñas();
        Task<ReseñaAlumnoDTO> ObtenerPorId(int id);
        Task<bool> CrearReseña(ReseñaAlumnoDTO reseñaDTO);

    }
}
