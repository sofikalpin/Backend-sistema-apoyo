using System;
using System.Collections.Generic;

namespace SistemaApoyo.Model;

public partial class Usuario
{
    public int Idusuario { get; set; }

    public string Nombrecompleto { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string ContraseñaHash { get; set; } = null!;

    public DateOnly Fecharegistro { get; set; }

    public int Idnivel { get; set; }

    public int Idrol { get; set; }

    public bool? AutProf { get; set; }

    public string? TokenRecuperacion { get; set; }

    public DateTime? TokenExpiracion { get; set; }

    public string? CvRuta { get; set; }

    public string? FotoRuta { get; set; }

    public virtual ICollection<Actividad> Actividads { get; set; } = new List<Actividad>();

    public virtual ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();

    public virtual ICollection<Bolsatrabajo> Bolsatrabajos { get; set; } = new List<Bolsatrabajo>();

    public virtual ICollection<Chat> ChatIdusuario1Navigations { get; set; } = new List<Chat>();

    public virtual ICollection<Chat> ChatIdusuario2Navigations { get; set; } = new List<Chat>();

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    public virtual ICollection<Examen> Examen { get; set; } = new List<Examen>();

    public virtual ICollection<Foro> Foros { get; set; } = new List<Foro>();

    public virtual Nivel IdnivelNavigation { get; set; } = null!;

    public virtual Rol IdrolNavigation { get; set; } = null!;

    public virtual ICollection<Mensaje> Mensajes { get; set; } = new List<Mensaje>();

    public virtual ICollection<Reseña> ReseñaIdProfesorNavigations { get; set; } = new List<Reseña>();

    public virtual ICollection<Reseña> ReseñaIdusuarioNavigations { get; set; } = new List<Reseña>();

    public virtual ICollection<Reseñapagina> Reseñapaginas { get; set; } = new List<Reseñapagina>();

    public virtual ICollection<Respuesta> Respuesta { get; set; } = new List<Respuesta>();
}
