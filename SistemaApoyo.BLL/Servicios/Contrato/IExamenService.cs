using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaApoyo.DTO;


namespace SistemaApoyo.BLL.Servicios.Contrato
{
    public interface IExamenService
    {
        Task<List<ExamenDTO>> ConsultarExamen();
        Task<List<ExamenDTO>> ObtenerPorNivel(int idNivel);
        Task<List<ExamenDTO>> ConsultarPorTitulo(string titulo);
        Task<ExamenDTO> ObteneExamenrPorId(int id);
        
    }        
}
