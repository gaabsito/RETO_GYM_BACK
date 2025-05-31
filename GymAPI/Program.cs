using GymAPI.Repositories;
using GymAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using GymAPI.Settings;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configuración JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()
    ?? throw new InvalidOperationException("JwtSettings no está configurado");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

// 🔹 Registrar JwtSettings como servicio
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// 🔹 Configurar EmailSettings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

// 🔹 Configurar Cloudinary Settings
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<IImageService, CloudinaryImageService>();

// 🔹 Configurar Google Auth Settings
builder.Services.Configure<GoogleAuthSettings>(
    builder.Configuration.GetSection("Authentication:Google"));

// 🔹 Obtener la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("GymappDB")
    ?? throw new InvalidOperationException("Connection string 'GymappDB' not found.");

// 🔹 Registrar los repositorios con la cadena de conexión
builder.Services.AddScoped<IUsuarioRepository>(provider =>
{
    var logger = provider.GetService<ILogger<UsuarioRepository>>();
    return new UsuarioRepository(connectionString, logger);
});

builder.Services.AddScoped<IEntrenamientoRepository>(provider =>
    new EntrenamientoRepository(connectionString));

builder.Services.AddScoped<IEjercicioRepository>(provider =>
    new EjercicioRepository(connectionString));

builder.Services.AddScoped<IEntrenamientoEjercicioRepository>(provider =>
    new EntrenamientoEjercicioRepository(connectionString));

builder.Services.AddScoped<IComentarioRepository>(provider =>
    new ComentarioRepository(connectionString));

builder.Services.AddScoped<IMedicionRepository>(provider =>
    new MedicionRepository(connectionString));

builder.Services.AddScoped<IRutinaCompletadaRepository>(provider =>
    new RutinaCompletadaRepository(connectionString));

builder.Services.AddScoped<ILogroRepository>(provider =>
    new LogroRepository(connectionString));

builder.Services.AddScoped<IUsuarioLogroRepository>(provider =>
    new UsuarioLogroRepository(connectionString));

// 🔹 Registrar los servicios
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IEntrenamientoService, EntrenamientoService>();
builder.Services.AddScoped<IEjercicioService, EjercicioService>();
builder.Services.AddScoped<IEntrenamientoEjercicioService, EntrenamientoEjercicioService>();
builder.Services.AddScoped<IComentarioService, ComentarioService>();
builder.Services.AddScoped<IMedicionService, MedicionService>();
builder.Services.AddScoped<IRutinaCompletadaService, RutinaCompletadaService>();
builder.Services.AddScoped<ILogroService, LogroService>();
builder.Services.AddScoped<IRolService, RolService>();

// 🔹 Configurar controladores con JSON options mejoradas
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // 🚨 CRÍTICO: Configurar JSON para que maneje correctamente camelCase del frontend
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.WriteIndented = true;
        
        // Permitir campos adicionales sin errores
        options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
        options.JsonSerializerOptions.AllowTrailingCommas = true;
    });

builder.Services.AddEndpointsApiExplorer();

// 🔹 Configurar Swagger para autenticación - SIEMPRE habilitado
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GymAPI",
        Version = "v1",
        Description = "API para aplicación de entrenamiento personal"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token en el formato: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// 🔹 Configurar CORS para permitir Vue
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// 🔹 Middleware
app.UseStaticFiles();
app.UseCors("AllowVueApp");

// 🔹 Habilitar Swagger en todos los entornos
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GymAPI v1");
    // Hacer que Swagger UI sea la página de inicio
    c.RoutePrefix = string.Empty;
});

// Solo redirigir a HTTPS en entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// 🔹 Middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();