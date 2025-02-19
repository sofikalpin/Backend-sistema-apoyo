using System;
using System.Collections.Generic;

namespace SistemaApoyo.Model;

public partial class Foro
{
    public int Idforo { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public int Idusuario { get; set; }

    public int? Idnivel { get; set; }

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    public virtual Nivel? IdnivelNavigation { get; set; }

    public virtual Usuario IdusuarioNavigation { get; set; } = null!;
}
