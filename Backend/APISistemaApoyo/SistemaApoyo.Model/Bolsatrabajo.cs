using System;
using System.Collections.Generic;

namespace SistemaApoyo.Model;

public partial class Bolsatrabajo
{
    public int Idbolsa { get; set; }

    public int Idusuario { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string? Correo { get; set; }

    public string Profesion { get; set; } = null!;

    public string Especildad { get; set; } = null!;

    public string Nivel { get; set; } = null!;

    public string? CvUrl { get; set; }

    public virtual Usuario IdusuarioNavigation { get; set; } = null!;
}
