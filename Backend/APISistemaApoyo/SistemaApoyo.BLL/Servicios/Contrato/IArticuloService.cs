using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaApoyo.DTO;


namespace SistemaApoyo.BLL.Servicios.Contrato
{
    public interface IArticuloService
    {
        Task<List<ArticuloDTO>> ConsultarPorTitulo(string titulo);
        Task<ArticuloDTO> ObtenerPorId(int id);
        Task<List<ArticuloDTO>> ConsultarArticulo();
        Task<List<ArticuloDTO>> ObtenerPorNivel(int idNivel);

    }
}
