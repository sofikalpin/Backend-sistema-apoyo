using System;
using System.Collections.Generic;

namespace SistemaApoyo.Model;

public partial class Actividad
{
    public int Idactividad { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int Idusuario { get; set; }

    public int Idnivel { get; set; }

    public DateOnly? FechaCreacion { get; set; }

    public string? Url { get; set; }

    public virtual Nivel IdnivelNavigation { get; set; } = null!;

    public virtual Usuario IdusuarioNavigation { get; set; } = null!;
}
