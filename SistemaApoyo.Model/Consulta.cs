using System;
using System.Collections.Generic;

namespace SistemaApoyo.Model;

public partial class Consulta
{
    public int Idconsulta { get; set; }

    public string Contenido { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    public int Idusuario { get; set; }

    public DateTime? Fechahora { get; set; }

    public int Idforo { get; set; }

    public virtual Foro IdforoNavigation { get; set; } = null!;

    public virtual Usuario IdusuarioNavigation { get; set; } = null!;

    public virtual ICollection<Respuesta> Respuesta { get; set; } = new List<Respuesta>();
}
