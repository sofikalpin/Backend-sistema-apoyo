using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SistemaApoyo.DTO
{
    public class ChatDTO
    {
        public int Idchat { get; set; }

        public int? Idusuario1 { get; set; }

        public int? Idusuario2 { get; set; }

        public DateTime? FechahoraInicio { get; set; }

        public List<MensajeDTO> Mensajes { get; set; } = new List<MensajeDTO>();
    }
}
