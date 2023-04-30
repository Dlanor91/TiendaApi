using AspNetCoreRateLimit;
using CORE.Interfaces;
using Infraestructura.Repositories;
using Infraestructura.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;

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
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();

            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-IP";
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint ="*",
                        Period = "5s",
                        Limit = 2 
                        
                    }
                };
            });
        }

        //Versionado
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true; //version por defecto
                //query string version con param ver
                //options.ApiVersionReader = new QueryStringApiVersionReader("ver");
                //encabezados, aqui X-Version es el parametro del header
                //options.ApiVersionReader = new HeaderApiVersionReader("X-Version");
                //tecnicas combinadas pero solo puedo usar 1
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("ver"),
                    new HeaderApiVersionReader("X-Version"));
                //te dice las versiones de encabezado
                options.ReportApiVersions = true;
            });
        }
    }
}
