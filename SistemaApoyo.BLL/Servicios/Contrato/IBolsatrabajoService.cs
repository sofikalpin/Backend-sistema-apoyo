using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaApoyo.DTO;

namespace SistemaApoyo.BLL.Servicios.Contrato
{
    public interface IBolsatrabajoService
    {
        Task<BolsatrabajoDTO?> Create(BolsatrabajoDTO dto);
        Task<IEnumerable<BolsatrabajoDTO>> GetAll();
        Task<BolsatrabajoDTO?> GetById(int id);
        Task<IEnumerable<BolsatrabajoDTO>> GetProfesoresIngles();
        Task<bool> Eliminar(int id);

    }
}
