using System;
using System.Collections.Generic;

namespace SistemaApoyo.Model;

public partial class Mensaje
{
    public int Idmensaje { get; set; }

    public string? Contenido { get; set; }

    public DateOnly? FechaEnvio { get; set; }

    public int Idchat { get; set; }

    public int Idusuario { get; set; }

    public virtual Chat IdchatNavigation { get; set; } = null!;

    public virtual Usuario IdusuarioNavigation { get; set; } = null!;
}
