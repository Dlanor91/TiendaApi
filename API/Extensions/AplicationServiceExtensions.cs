using AspNetCoreRateLimit;
using CORE.Interfaces;
using Infraestructura.Repositories;
using Infraestructura.UnitOfWork;

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
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<IProductoRepository, ProductoRepository>();
            //services.AddScoped<IMarcaRepository, MarcaRepository>();
            //services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();//como UnitOfWork los implementa por separado, los de arriba no los preciso
        }

        //para rate limit
        public static void ConfigureRateLimitiong(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IRateLimitConfiguration,RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();

            services.Configure<IpRateLimitOptions>( options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-IP";
                options.GeneralRules = new List<RateLimitRule> { 
                    new RateLimitRule 
                    { 
                        Endpoint = "*",
                        Period = "10s",
                        Limit = 2
                    } 
                };
            });
        }
    }
}
