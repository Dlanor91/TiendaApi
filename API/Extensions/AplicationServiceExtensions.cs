using CORE.Interfaces;

namespace API.Extensions
{
    public static class AplicationServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()    //WithOrigins("https://dominio.com") Esto es como queda en desarrollo
                .AllowAnyMethod()           //WithMethods("Get",Post)
                .AllowAnyHeader());         //WithHeaders("accept", "content-type)
            });

        //agrego todos los repositorios a usar
        public static void AddAplicacionServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IProductoRepository, ProductoRepository>();
            services.AddScoped<IMarcaRepository, MarcaRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        }
    }
}
