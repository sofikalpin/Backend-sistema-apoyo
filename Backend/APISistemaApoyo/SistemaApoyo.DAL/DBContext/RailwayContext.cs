using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SistemaApoyo.Model;

namespace SistemaApoyo.DAL.DBContext {

    public partial class RailwayContext : DbContext
    {
        public RailwayContext()
        {
        }

        public RailwayContext(DbContextOptions<RailwayContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actividad> Actividads { get; set; }

        public virtual DbSet<Articulo> Articulos { get; set; }

        public virtual DbSet<Bolsatrabajo> Bolsatrabajos { get; set; }

        public virtual DbSet<Chat> Chats { get; set; }

        public virtual DbSet<Consulta> Consulta { get; set; }

        public virtual DbSet<Examen> Examen { get; set; }

        public virtual DbSet<Foro> Foros { get; set; }

        public virtual DbSet<Mensaje> Mensajes { get; set; }

        public virtual DbSet<Nivel> Nivels { get; set; }

        public virtual DbSet<Reseña> Reseñas { get; set; }

        public virtual DbSet<Reseñapagina> Reseñapaginas { get; set; }

        public virtual DbSet<Respuesta> Respuesta { get; set; }

        public virtual DbSet<Rol> Rols { get; set; }

        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseNpgsql("Host=ballast.proxy.rlwy.net;Port=56631;Database=railway;Username=postgres;Password=WxHWPhYvFBBwzcdBHLJiWPvwUxPqPKjY;SslMode=Require;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actividad>(entity =>
            {
                entity.HasKey(e => e.Idactividad).HasName("pk_idActividad");

                entity.ToTable("actividad");

                entity.Property(e => e.Idactividad)
                    .HasDefaultValueSql("nextval('\"Actividad_id_seq\"'::regclass)")
                    .HasColumnName("idactividad");
                entity.Property(e => e.Descripcion)
                    .HasColumnType("character varying")
                    .HasColumnName("descripcion");
                entity.Property(e => e.FechaCreacion).HasColumnName("fechaCreacion");
                entity.Property(e => e.Idnivel).HasColumnName("idnivel");
                entity.Property(e => e.Idusuario).HasColumnName("idusuario");
                entity.Property(e => e.Nombre)
                    .HasColumnType("character varying")
                    .HasColumnName("nombre");
                entity.Property(e => e.Url)
                    .HasColumnType("character varying")
                    .HasColumnName("url");

                entity.HasOne(d => d.IdnivelNavigation).WithMany(p => p.Actividads)
                    .HasForeignKey(d => d.Idnivel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idnivel");

                entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Actividads)
                    .HasForeignKey(d => d.Idusuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idusuario");
            });

            modelBuilder.Entity<Articulo>(entity =>
            {
                entity.HasKey(e => e.Idarticulo).HasName("pk_idArticulo");

                entity.ToTable("articulo");

                entity.Property(e => e.Idarticulo)
                    .HasDefaultValueSql("nextval('\"Articulo_id_seq\"'::regclass)")
                    .HasColumnName("idarticulo");
                entity.Property(e => e.Descripcion)
                    .HasColumnType("character varying")
                    .HasColumnName("descripcion");
                entity.Property(e => e.FechaCreacion).HasColumnName("fechaCreacion");
                entity.Property(e => e.Idnivel).HasColumnName("idnivel");
                entity.Property(e => e.Idusuario).HasColumnName("idusuario");
                entity.Property(e => e.Titulo)
                    .HasColumnType("character varying")
                    .HasColumnName("titulo");
                entity.Property(e => e.Url)
                    .HasColumnType("character varying")
                    .HasColumnName("url");

                entity.HasOne(d => d.IdnivelNavigation).WithMany(p => p.Articulos)
                    .HasForeignKey(d => d.Idnivel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idnivel");

                entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Articulos)
                    .HasForeignKey(d => d.Idusuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idusuario");
            });

            modelBuilder.Entity<Bolsatrabajo>(entity =>
            {
                entity.HasKey(e => e.Idbolsa).HasName("pk_idbolsa");

                entity.ToTable("bolsatrabajo");

                entity.Property(e => e.Idbolsa).HasColumnName("idbolsa");
                entity.Property(e => e.Correo)
                    .HasColumnType("character varying")
                    .HasColumnName("correo");
                entity.Property(e => e.CvUrl)
                    .HasColumnType("character varying")
                    .HasColumnName("cvUrl");
                entity.Property(e => e.Especildad)
                    .HasColumnType("character varying")
                    .HasColumnName("especildad");
                entity.Property(e => e.Idusuario).HasColumnName("idusuario");
                entity.Property(e => e.Nivel)
                    .HasColumnType("character varying")
                    .HasColumnName("nivel");
                entity.Property(e => e.NombreCompleto)
                    .HasColumnType("character varying")
                    .HasColumnName("nombreCompleto");
                entity.Property(e => e.Profesion)
                    .HasColumnType("character varying")
                    .HasColumnName("profesion");

                entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Bolsatrabajos)
                    .HasForeignKey(d => d.Idusuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idusuario");
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasKey(e => e.Idchat).HasName("pk_idChat");

                entity.ToTable("chat");

                entity.Property(e => e.Idchat)
                    .HasDefaultValueSql("nextval('\"Chat_id_seq\"'::regclass)")
                    .HasColumnName("idchat");
                entity.Property(e => e.FechahoraInicio).HasColumnName("fechahora_inicio");
                entity.Property(e => e.Idusuario1).HasColumnName("idusuario1");
                entity.Property(e => e.Idusuario2).HasColumnName("idusuario2");

                entity.HasOne(d => d.Idusuario1Navigation).WithMany(p => p.ChatIdusuario1Navigations)
                    .HasForeignKey(d => d.Idusuario1)
                    .HasConstraintName("idusuario1");

                entity.HasOne(d => d.Idusuario2Navigation).WithMany(p => p.ChatIdusuario2Navigations)
                    .HasForeignKey(d => d.Idusuario2)
                    .HasConstraintName("idusuario2");
            });

            modelBuilder.Entity<Consulta>(entity =>
            {
                entity.HasKey(e => e.Idconsulta).HasName("pk_idConsulta");

                entity.ToTable("consulta");

                entity.Property(e => e.Idconsulta)
                    .HasDefaultValueSql("nextval('\"Consulta_id_seq\"'::regclass)")
                    .HasColumnName("idconsulta");
                entity.Property(e => e.Contenido)
                    .HasColumnType("character varying")
                    .HasColumnName("contenido");
                entity.Property(e => e.Fechahora).HasColumnName("fechahora");
                entity.Property(e => e.Idforo).HasColumnName("idforo");
                entity.Property(e => e.Idusuario).HasColumnName("idusuario");
                entity.Property(e => e.Titulo)
                    .HasColumnType("character varying")
                    .HasColumnName("titulo");

                entity.HasOne(d => d.IdforoNavigation).WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.Idforo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idforo");

                entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.Idusuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idusuario");
            });

            modelBuilder.Entity<Examen>(entity =>
            {
                entity.HasKey(e => e.Idexamen).HasName("pk_idExamen");

                entity.ToTable("examen");

                entity.Property(e => e.Idexamen)
                    .HasDefaultValueSql("nextval('\"Examen_id_seq\"'::regclass)")
                    .HasColumnName("idexamen");
                entity.Property(e => e.Calificacion)
                    .HasColumnType("character varying")
                    .HasColumnName("calificacion");
                entity.Property(e => e.FechaCreacion).HasColumnName("fechaCreacion");
                entity.Property(e => e.Idnivel).HasColumnName("idnivel");
                entity.Property(e => e.Idusuario).HasColumnName("idusuario");
                entity.Property(e => e.Titulo)
                    .HasColumnType("character varying")
                    .HasColumnName("titulo");
                entity.Property(e => e.Url)
                    .HasColumnType("character varying")
                    .HasColumnName("url");

                entity.HasOne(d => d.IdnivelNavigation).WithMany(p => p.Examen)
                    .HasForeignKey(d => d.Idnivel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idnivel");

                entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Examen)
                    .HasForeignKey(d => d.Idusuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idusuario");
            });

            modelBuilder.Entity<Foro>(entity =>
            {
                entity.HasKey(e => e.Idforo).HasName("pk_idForo");

                entity.ToTable("foro");

                entity.Property(e => e.Idforo)
                    .HasDefaultValueSql("nextval('\"Foro_id_seq\"'::regclass)")
                    .HasColumnName("idforo");
                entity.Property(e => e.Descripcion)
                    .HasColumnType("character varying")
                    .HasColumnName("descripcion");
                entity.Property(e => e.Fecha).HasColumnName("fecha");
                entity.Property(e => e.Idnivel).HasColumnName("idnivel");
                entity.Property(e => e.Idusuario).HasColumnName("idusuario");
                entity.Property(e => e.Nombre)
                    .HasColumnType("character varying")
                    .HasColumnName("nombre");

                entity.HasOne(d => d.IdnivelNavigation).WithMany(p => p.Foros)
                    .HasForeignKey(d => d.Idnivel)
                    .HasConstraintName("idnivel");

                entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Foros)
                    .HasForeignKey(d => d.Idusuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idusuario");
            });

            modelBuilder.Entity<Mensaje>(entity =>
            {
                entity.HasKey(e => e.Idmensaje).HasName("pk_mensaje");

                entity.ToTable("mensaje");

                entity.Property(e => e.Idmensaje)
                    .HasDefaultValueSql("nextval('autoincrementoidmensaje'::regclass)")
                    .HasColumnName("idmensaje");
                entity.Property(e => e.Contenido)
                    .HasColumnType("character varying")
                    .HasColumnName("contenido");
                entity.Property(e => e.FechaEnvio).HasColumnName("fecha_envio");
                entity.Property(e => e.Idchat).HasColumnName("idchat");
                entity.Property(e => e.Idusuario).HasColumnName("idusuario");

                entity.HasOne(d => d.IdchatNavigation).WithMany(p => p.Mensajes)
                    .HasForeignKey(d => d.Idchat)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idchat");

                entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Mensajes)
                    .HasForeignKey(d => d.Idusuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idusuario");
            });

            modelBuilder.Entity<Nivel>(entity =>
            {
                entity.HasKey(e => e.Idnivel).HasName("pk_idNivel");

                entity.ToTable("nivel");

                entity.Property(e => e.Idnivel)
                    .HasDefaultValueSql("nextval('\"Nivel_id_seq\"'::regclass)")
                    .HasColumnName("idnivel");
                entity.Property(e => e.Descripcion)
                    .HasColumnType("character varying")
                    .HasColumnName("descripcion");
            });

            modelBuilder.Entity<Reseña>(entity =>
            {
                entity.HasKey(e => e.IdReseña).HasName("pk_idReseña");

                entity.ToTable("reseña");

                entity.Property(e => e.IdReseña)
                    .ValueGeneratedNever()
                    .HasColumnName("idReseña");
                entity.Property(e => e.Comentario)
                    .HasColumnType("character varying")
                    .HasColumnName("comentario");
                entity.Property(e => e.IdProfesor).HasColumnName("idProfesor");
                entity.Property(e => e.Idusuario).HasColumnName("idusuario");
                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.HasOne(d => d.IdProfesorNavigation).WithMany(p => p.ReseñaIdProfesorNavigations)
                    .HasForeignKey(d => d.IdProfesor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idprofesor");

                entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.ReseñaIdusuarioNavigations)
                    .HasForeignKey(d => d.Idusuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idusuario");
            });

            modelBuilder.Entity<Reseñapagina>(entity =>
            {
                entity.HasKey(e => e.IdReseñaP).HasName("pk_idReseñaP");

                entity.ToTable("reseñapagina");

                entity.Property(e => e.IdReseñaP)
                    .ValueGeneratedNever()
                    .HasColumnName("idReseñaP");
                entity.Property(e => e.Comentario)
                    .HasColumnType("character varying")
                    .HasColumnName("comentario");
                entity.Property(e => e.Idusuaro).HasColumnName("idusuaro");
                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.HasOne(d => d.IdusuaroNavigation).WithMany(p => p.Reseñapaginas)
                    .HasForeignKey(d => d.Idusuaro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idsuaro");
            });

            modelBuilder.Entity<Respuesta>(entity =>
            {
                entity.HasKey(e => e.Idrespuesta).HasName("pk_idRespuesta");

                entity.ToTable("respuesta");

                entity.Property(e => e.Idrespuesta)
                    .HasDefaultValueSql("nextval('\"Respuesta_id_seq\"'::regclass)")
                    .HasColumnName("idrespuesta");
                entity.Property(e => e.Contenido)
                    .HasColumnType("character varying")
                    .HasColumnName("contenido");
                entity.Property(e => e.Fechahora).HasColumnName("fechahora");
                entity.Property(e => e.Idconsulta).HasColumnName("idconsulta");
                entity.Property(e => e.Idusuario).HasColumnName("idusuario");

                entity.HasOne(d => d.IdconsultaNavigation).WithMany(p => p.Respuesta)
                    .HasForeignKey(d => d.Idconsulta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idconsulta");

                entity.HasOne(d => d.IdusuarioNavigation).WithMany(p => p.Respuesta)
                    .HasForeignKey(d => d.Idusuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idusuario");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.Idrol).HasName("Rol_pkey");

                entity.ToTable("rol");

                entity.Property(e => e.Idrol)
                    .ValueGeneratedNever()
                    .HasColumnName("idrol");
                entity.Property(e => e.Nombre)
                    .HasColumnType("character varying")
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Idusuario).HasName("Usuario_pkey");

                entity.ToTable("usuario");

                entity.Property(e => e.Idusuario)
                    .ValueGeneratedNever()
                    .HasColumnName("idusuario");
                entity.Property(e => e.AutProf).HasColumnName("autProf");
                entity.Property(e => e.ContraseñaHash)
                    .HasColumnType("character varying")
                    .HasColumnName("contraseña_hash");
                entity.Property(e => e.Correo)
                    .HasColumnType("character varying")
                    .HasColumnName("correo");
                entity.Property(e => e.CvRuta)
                    .HasColumnType("character varying")
                    .HasColumnName("cvRuta");
                entity.Property(e => e.Fecharegistro).HasColumnName("fecharegistro");
                entity.Property(e => e.FotoRuta)
                    .HasColumnType("character varying")
                    .HasColumnName("fotoRuta");
                entity.Property(e => e.Idnivel).HasColumnName("idnivel");
                entity.Property(e => e.Idrol).HasColumnName("idrol");
                entity.Property(e => e.Nombrecompleto)
                    .HasColumnType("character varying")
                    .HasColumnName("nombrecompleto");
                entity.Property(e => e.TokenRecuperacion).HasColumnType("character varying");

                entity.HasOne(d => d.IdnivelNavigation).WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.Idnivel)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idnivel");

                entity.HasOne(d => d.IdrolNavigation).WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.Idrol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("idrol");
            });
            modelBuilder.HasSequence("autoincrementoidmensaje").StartsAt(15L);
            modelBuilder.HasSequence("bolsatrabajo_idbolsa_seq").StartsAt(9L);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}