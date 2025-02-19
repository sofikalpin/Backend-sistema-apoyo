using System;
using System.Collections.Generic;

namespace SistemaApoyo.Model;

public partial class Menu
{
    public int Idmenu { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Icono { get; set; }

    public string? Url { get; set; }

    public virtual ICollection<Menurol> Menurols { get; set; } = new List<Menurol>();
}
