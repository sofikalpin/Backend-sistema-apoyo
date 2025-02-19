using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.DTO
{
    public class ReseñaAlumnoDTO
    {
        public int IdReseña { get; set; }

        public int Idusuario { get; set; }

        public string NombreUsuario { get; set; }

        public int IdProfesor { get; set; }

        public string NombreProfesor { get; set; }

        public int Rating { get; set; }

        public string Comentario { get; set; } = null!;

    }
}