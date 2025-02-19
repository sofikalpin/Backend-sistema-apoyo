using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.DTO
{
    public class BolsatrabajoDTO
    {
        public int Idbolsa { get; set; }

        public int Idusuario { get; set; }

        public string NombreCompleto { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string Profesion { get; set; } = null!;

        public string Especildad { get; set; } = null!;

        public string? Nivel { get; set; }

        public string CvUrl { get; set; } = null!;
    }
}