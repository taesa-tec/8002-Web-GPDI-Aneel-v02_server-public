using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PeD.Services.Captacoes;

namespace PeD.BackgroundServices
{
    public class PropostasServices : BackgroundService
    {
        private ILogger<PropostasServices> _logger;
        private IServiceProvider Services;

        public PropostasServices(ILogger<PropostasServices> logger, IServiceProvider services)
        {
            _logger = logger;
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await CheckPropostas(stoppingToken);
        }

        private async Task CheckPropostas(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = Services.CreateScope())
                    {
                        var servicePropostas = scope.ServiceProvider.GetRequiredService<PropostaService>();
                        var serviceCaptacao = scope.ServiceProvider.GetRequiredService<CaptacaoService>();
                        await servicePropostas.FinalizarPropostasExpiradas(stoppingToken);
                        serviceCaptacao.EncerrarCaptacoesExpiradas();
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError("{Error}", e.Message);
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}