using Microsoft.Extensions.DependencyInjection;

namespace PeD.Fornecedor.Services
{
    public static class FornecedorServices
    {
        public static void AddFornecedorServices(this IServiceCollection services)
        {
            services.AddScoped<PropostaService>();
        }
    }
}