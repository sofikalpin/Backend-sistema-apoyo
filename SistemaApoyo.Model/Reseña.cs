using System;
using System.Collections.Generic;

namespace SistemaApoyo.Model;

public partial class Reseña
{
    public int IdReseña { get; set; }

    public int Idusuario { get; set; }

    public int IdProfesor { get; set; }

    public int Rating { get; set; }

    public string Comentario { get; set; } = null!;

    public virtual Usuario IdProfesorNavigation { get; set; } = null!;

    public virtual Usuario IdusuarioNavigation { get; set; } = null!;
}
