using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SistemaApoyo.DTO
{
    public class ActividadDTO
    {
        public int Idactividad { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int? Idusuario { get; set; }

        public int? Idnivel { get; set; }

        public DateOnly? FechaCreacion { get; set; }

        public string? Url { get; set; }
    }
}
