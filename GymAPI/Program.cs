using GymAPI.Repositories;
using GymAPI.Services;


var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión desde el archivo de configuración
var connectionString = builder.Configuration.GetConnectionString("GymappDB");

// Registrar los repositorios con la cadena de conexión
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

// Registrar los servicios
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IEntrenamientoService, EntrenamientoService>();

builder.Services.AddScoped<IEjercicioService, EjercicioService>();

builder.Services.AddScoped<IEntrenamientoEjercicioService, EntrenamientoEjercicioService>();

builder.Services.AddScoped<IComentarioService, ComentarioService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Configurar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
