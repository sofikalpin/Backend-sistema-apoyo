using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.DTO
{
    public class RespuestaDTO
    {
        public int Idrespuesta { get; set; }

        public string Contenido { get; set; } = null!;

        public int Idconsulta { get; set; }

        public int Idusuario { get; set; }

        public DateTime? Fechahora { get; set; }

    }
}