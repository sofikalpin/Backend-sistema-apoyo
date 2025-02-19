using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.DTO
{
    public class CrearReseñaAlumnoDTO
    {
        public string Comentario { get; set; } = null!;

        public int Rating { get; set; }

        public int IdProfesor { get; set; }

        public int Idusuario { get; set; }
    }
}
