using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaApoyo.DAL.Repositorios.Contrato;
using SistemaApoyo.DAL.Repositorios;
using SistemaApoyo.Utility;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.BLL.Servicios;
using SistemaApoyo.DAL.DBContext;
using SistemaApoyo.BLL.servicios;
using SistemaApoyo.Model;

namespace SistemaApoyo.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddDbContext<RailwayContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("cadenaPostGreSQL");
                options.UseNpgsql(connectionString);
            });

            services.AddAutoMapper(typeof(AutoMapperProfile));

           
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IExamenService, ExamenService>();
            services.AddScoped<IConsultaService, ConsultaService>();
            services.AddScoped<IForoService, ForoService>();
            services.AddScoped<IRespuestaService,RespuestaService>();
            services.AddScoped<IActividadService, ActividadService>();
            services.AddScoped<IArticuloService, ArticuloService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<INivelService, NivelService>();
            services.AddScoped<IProfesorActividad, ProfesorActividadService>();
            services.AddScoped<IProfesorExamen, ProfesorExamenService>();
            services.AddScoped<IProfesorArticulo, ProfesorArticuloService>();
            services.AddScoped<IMensajeService, MensajeService>();
            services.AddScoped<IAdministrador, AdministradorService>();
            services.AddScoped<IReseñaAlumnoService, ReseñaAlumnoService>();
            services.AddScoped<IReseñaService, ReseñaService>();
            services.AddScoped<IBolsatrabajoService, BolsatrabajoService>();
            services.AddSignalR();
          
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}
