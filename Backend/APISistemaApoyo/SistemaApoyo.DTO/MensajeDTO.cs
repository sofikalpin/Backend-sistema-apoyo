using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.DTO
{
    public class MensajeDTO
    {
      public int Idmensaje { get; set; }

      public string? Contenido { get; set; }

      public DateOnly? FechaEnvio { get; set; }

      public int Idchat { get; set; }

      public int Idusuario { get; set; }  

    }
}

