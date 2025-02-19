using System;
using System.Collections.Generic;

namespace SistemaApoyo.Model;

public partial class Menurol
{
    public int Idmenurol { get; set; }

    public int Idmenu { get; set; }

    public int Idrol { get; set; }

    public virtual Menu IdmenuNavigation { get; set; } = null!;

    public virtual Rol IdrolNavigation { get; set; } = null!;
}
