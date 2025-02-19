using System;
using System.Collections.Generic;

namespace SistemaApoyo.Model;

public partial class Reseñapagina
{
    public int IdReseñaP { get; set; }

    public int Idusuaro { get; set; }

    public int Rating { get; set; }

    public string Comentario { get; set; } = null!;

    public virtual Usuario IdusuaroNavigation { get; set; } = null!;
}
