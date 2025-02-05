using RecetasAPI.Repositories;
using RecetasAPI.Service;
using RecetasAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión desde el archivo de configuración
var connectionString = builder.Configuration.GetConnectionString("RestauranteDB");

// Registrar los repositorios con la cadena de conexión
builder.Services.AddScoped<IBebidaRepository>(provider =>
    new BebidaRepository(connectionString));

builder.Services.AddScoped<IPlatoPrincipalRepository>(provider =>
    new PlatoPrincipalRepository(connectionString));

builder.Services.AddScoped<IPostreRepository>(provider =>
    new PostreRepository(connectionString));


// Registrar los servicios
builder.Services.AddScoped<IBebidaService, BebidaService>();
builder.Services.AddScoped<IPostreService, PostreService>();
builder.Services.AddScoped<IPlatoPrincipalService, PlatoPrincipalService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
