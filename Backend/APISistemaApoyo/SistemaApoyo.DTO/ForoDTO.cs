using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.DTO
{
    public class ForoDTO
    {
        public int Idforo { get; set; }

        public string Nombre { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public DateOnly? Fecha { get; set; }

        public int Idusuario { get; set; }

        public int? Idnivel { get; set; }

    }
}