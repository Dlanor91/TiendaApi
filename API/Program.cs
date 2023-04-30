using API.Extensions;
using AspNetCoreRateLimit;
using Infraestructura.Data;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
//Auto maoper
builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

//Rate Limit
builder.Services.ConfigureRateLimitiong();

//Versionado
builder.Services.ConfigureApiVersioning();

// Add services to the container.
builder.Services.ConfigureCors(); //aqui lo annado al proyecto
builder.Services.AddAplicacionServices();//servicio que tiene los repositorios 

builder.Services.AddControllers();

//agrego el context
builder.Services.AddDbContext<TiendaContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//añado elservicio por app
app.UseIpRateLimiting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//codigo que revisa automaticamente la bd luego de IsDevelopment
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<TiendaContext>();
        await context.Database.MigrateAsync();
        //llamo los datos automatizados
        await TiendaContextSeed.SeedAsync(context, loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Ocurrio un error durante la migracion.");
    }
}

//
app.UseCors("CorsPolicy"); //ademas lo uso aqui donde lo llamo a aplicar

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
