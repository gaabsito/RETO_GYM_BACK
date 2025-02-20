using GymAPI.Repositories;
using GymAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
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

// 🔹 Obtener la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("GymappDB");

// 🔹 Registrar los repositorios con la cadena de conexión
builder.Services.AddScoped<IUsuarioRepository>(provider =>
    new UsuarioRepository(connectionString));

builder.Services.AddScoped<IEntrenamientoRepository>(provider =>
    new EntrenamientoRepository(connectionString));

builder.Services.AddScoped<IEjercicioRepository>(provider =>
    new EjercicioRepository(connectionString));

builder.Services.AddScoped<IEntrenamientoEjercicioRepository>(provider =>
    new EntrenamientoEjercicioRepository(connectionString));

builder.Services.AddScoped<IComentarioRepository>(provider =>
    new ComentarioRepository(connectionString));

builder.Services.Configure<GoogleAuthSettings>(
    builder.Configuration.GetSection("Authentication:Google"));

// 🔹 Registrar los servicios
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IEntrenamientoService, EntrenamientoService>();
builder.Services.AddScoped<IEjercicioService, EjercicioService>();
builder.Services.AddScoped<IEntrenamientoEjercicioService, EntrenamientoEjercicioService>();
builder.Services.AddScoped<IComentarioService, ComentarioService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 🔹 Configurar Swagger para autenticación
builder.Services.AddSwaggerGen(c =>
{
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
    options.AddPolicy("AllowVueApp",
        builder =>
        {
            builder
                .WithOrigins("http://localhost:5173")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

var app = builder.Build();

app.UseCors("AllowVueApp");

// 🔹 Configurar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔹 Añadir middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
