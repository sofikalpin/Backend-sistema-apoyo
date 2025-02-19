using System;
using System.Collections.Generic;

namespace SistemaApoyo.Model;

public partial class Chat
{
    public int Idchat { get; set; }

    public int? Idusuario1 { get; set; }

    public int? Idusuario2 { get; set; }

    public DateTime? FechahoraInicio { get; set; }

    public virtual Usuario? Idusuario1Navigation { get; set; }

    public virtual Usuario? Idusuario2Navigation { get; set; }

    public virtual ICollection<Mensaje> Mensajes { get; set; } = new List<Mensaje>();
}
