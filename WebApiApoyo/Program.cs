using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation;
using FluentValidation.AspNetCore;
using SistemaApoyo.BLL.Validaciones;
using SistemaApoyo.BLL.Hubs;
using SistemaApoyo.IOC;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de Kestrel para escuchar en todos los IPs
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5228);
});

// Deshabilitar HTTPS Redirection (esto se mantuvo como lo ten�as)
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = null;
});

// Inyecci�n de dependencias
builder.Services.InyectarDependencias(builder.Configuration);
builder.Services.AddControllers();

// Configuraci�n de CORS para permitir cualquier origen (en desarrollo)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        // Permitir todos los or�genes en desarrollo
        policy.AllowAnyOrigin()  // Para producci�n, cambia a WithOrigins("https://edumatch-three.vercel.app")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Configuraci�n de Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ActividadValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ArticuloValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ChatValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ConsultaValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ExamenValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ForoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MensajeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RespuestaValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SesionValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UsuarioValidator>();

// Agregar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar Swagger para todos los ambientes
app.UseSwagger();
app.UseSwaggerUI();

// Configuraci�n de CORS
app.UseCors("AllowAllOrigins");

// Remover HTTPS redirection (comentado para evitar redirecci�n HTTPS)
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();

// Mapear SignalR Hub
app.MapHub<ChatHub>("/chatHub");

app.Run();
