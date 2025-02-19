using System;
using System.Collections.Generic;

namespace SistemaApoyo.Model;

public partial class Nivel
{
    public int Idnivel { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Actividad> Actividads { get; set; } = new List<Actividad>();

    public virtual ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();

    public virtual ICollection<Examen> Examen { get; set; } = new List<Examen>();

    public virtual ICollection<Foro> Foros { get; set; } = new List<Foro>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
