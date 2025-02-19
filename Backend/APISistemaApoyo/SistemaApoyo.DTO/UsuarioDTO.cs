using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.DTO
{
    public class UsuarioDTO
    {
        public int Idusuario { get; set; }

        public string Nombrecompleto { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string ContraseñaHash { get; set; } = null!;

        public DateOnly Fecharegistro { get; set; }

        public int Idnivel { get; set; }

        public int Idrol { get; set; }

        public bool? AutProf { get; set; }

        public string? TokenRecuperacion { get; set; }

        public DateTime? TokenExpiracion { get; set; }

        public string? CvRuta { get; set; }

        public string? FotoRuta { get; set; }
    }
}
