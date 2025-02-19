using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaApoyo.DTO;


namespace SistemaApoyo.BLL.Servicios.Contrato
{
    public interface IProfesorActividad
    {
        Task<List<ActividadDTO>> ConsultarActividad();
        Task<List<ActividadDTO>> ConsultarporNombre(string nombre);
        Task<List<ActividadDTO>> ObteneActividadrPorIdProfesor(int id);
        Task<bool> CrearActividad(ActividadDTO actividads);
        Task<bool> ActualizarActivdad(ActividadDTO actividad);
        Task<bool> EliminarActividad(int id);

    }
}
