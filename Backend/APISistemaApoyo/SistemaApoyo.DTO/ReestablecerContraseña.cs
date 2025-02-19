using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.DTO
{
    public class ReestablecerContraseñaDTO
    {
        public string Token { get; set; }

        public string NuevaContraseña { get; set; }
    }
}
