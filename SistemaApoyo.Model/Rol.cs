using System;
using System.Collections.Generic;

namespace SistemaApoyo.Model;

public partial class Rol
{
    public int Idrol { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
