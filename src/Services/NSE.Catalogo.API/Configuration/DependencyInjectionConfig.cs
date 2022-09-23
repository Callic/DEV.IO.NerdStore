using Microsoft.Extensions.DependencyInjection;
using NSE.Catalogo.API.Data;
using NSE.Catalogo.API.Data.Implementations;
using NSE.Catalogo.API.Data.Interfaces;

namespace NSE.Catalogo.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void ResolveDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<CatalogoContext>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
        }
    }
}
