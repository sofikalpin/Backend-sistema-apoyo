using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.DTO
{
    public class ReseñaDTO
    {
        public int IdReseñaP { get; set; }

        public int Idusuaro { get; set; }

        public string NombreUsuario { get; set; }

        public int Rating { get; set; }

        public string Comentario { get; set; } = null!;
    }
}
