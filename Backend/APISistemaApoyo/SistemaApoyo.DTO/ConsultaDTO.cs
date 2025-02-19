using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.DTO
{
    public class ConsultaDTO
    {
        public int Idconsulta { get; set; }

        public string Contenido { get; set; } = null!;

        public string Titulo { get; set; } = null!;

        public int Idusuario { get; set; }

        public DateTime? Fechahora { get; set; }

        public int IdForo { get; set; }
    }
}