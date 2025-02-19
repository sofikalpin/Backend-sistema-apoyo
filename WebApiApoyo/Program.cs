using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation;
using FluentValidation.AspNetCore;
using SistemaApoyo.BLL.Validaciones;
using SistemaApoyo.BLL.Hubs;
using SistemaApoyo.IOC;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Kestrel para escuchar en todos los IPs
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5228);
});

// Deshabilitar HTTPS Redirection
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = null;
});

// Inyección de dependencias
builder.Services.InyectarDependencias(builder.Configuration);
builder.Services.AddControllers();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.WithOrigins("https://edumatch-three.vercel.app/")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Configuración de Fluent Validation
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

// Configuración de CORS
app.UseCors("AllowAllOrigins");

// Remover HTTPS redirection
//app.UseHttpsRedirection();  // Comentado para evitar redirección HTTPS

app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.MapHub<ChatHub>("/chatHub");

app.Run();