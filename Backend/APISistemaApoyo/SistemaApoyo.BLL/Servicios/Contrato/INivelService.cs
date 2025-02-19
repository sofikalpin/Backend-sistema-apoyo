using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaApoyo.DTO;


namespace SistemaApoyo.BLL.Servicios.Contrato
{
    public interface INivelService
    {
        Task<List<NivelDTO>> ConsultarNivel();

        Task<List<NivelDTO>> ConsultarPorDescripcion(string descripcion);
        Task<NivelDTO> ObtenerNivelPorID(int id);

    }
}