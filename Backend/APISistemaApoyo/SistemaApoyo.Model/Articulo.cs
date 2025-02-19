using System;
using System.Collections.Generic;

namespace SistemaApoyo.Model;

public partial class Articulo
{
    public int Idarticulo { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string? Url { get; set; }

    public int Idusuario { get; set; }

    public int Idnivel { get; set; }

    public DateOnly? FechaCreacion { get; set; }

    public virtual Nivel IdnivelNavigation { get; set; } = null!;

    public virtual Usuario IdusuarioNavigation { get; set; } = null!;
}
