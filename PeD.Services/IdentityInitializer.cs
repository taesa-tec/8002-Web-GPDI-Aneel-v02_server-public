using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PeD.Core.Models;
using PeD.Core.Models.Catalogos;
using PeD.Data;
using CategoriaContabil = PeD.Core.Models.Catalogos.CategoriaContabil;

namespace PeD.Services
{
    public class IdentityInitializer : IStartupFilter
    {
        private readonly GestorDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private UserService _userService;
        private MailService _mailService;
        private ILogger<IdentityInitializer> logger;

        public IConfiguration Configuration { get; }

        public IdentityInitializer(IServiceProvider services, IConfiguration Configuration,
            ILogger<IdentityInitializer> logger
        )
        {
            var scope = services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<GestorDbContext>();
            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _mailService = scope.ServiceProvider.GetRequiredService<MailService>();
            _userService = new UserService(_context, _userManager, _roleManager, _mailService,
                scope.ServiceProvider.GetService<AccessManager>());
            this.Configuration = Configuration;
            this.logger = logger;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            Initialize();
            return next;
        }

        public void Initialize()
        {
            if (!_context.Database.EnsureCreated())
            {
                CreateRoles().Wait();
                CreateAdminUser();
                CreateCatalog();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Skip Database seed");
                Console.WriteLine();
            }
        }

        protected async Task CreateRoles()
        {
            foreach (var role in Roles.AllRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(role));
                    if (result.Succeeded)
                    {
                        Console.WriteLine($"{role} Criado");
                    }
                    else
                        result.Errors.ToList().ForEach(error => { logger.LogError(error.Description); });
                }
            }
        }

        protected void CreateAdminUser()
        {
            // Verifica se já termos um administrador cadastrado
            if (_context.Users.Any(User => User.Role == Roles.AdminGestor))
                return;

            var adminSection = Configuration.GetSection("AdminUser");
            var adminEmail = adminSection.GetValue<string>("Email");
            var adminPass = adminSection.GetValue<string>("Password");

            if (adminEmail != null && adminPass != null)
            {
                _userService.CreateUser(
                    new ApplicationUser()
                    {
                        Email = adminEmail,
                        EmailConfirmed = true,
                        Status = true,
                        Role = "Admin-PeD"
                    }, adminPass, Roles.AdminGestor);
            }
        }

        protected void CreateCatalog()
        {
            if (_context.CategoriasContabeis.FirstOrDefault() == null)
            {
                var categorias = new List<CategoriaContabil>
                {
                    new CategoriaContabil
                    {
                        Valor = "RH",
                        Nome = "Recursos Humanos",
                        Atividades = new List<Atividade>
                        {
                            new Atividade
                            {
                                Valor = "HH",
                                Nome =
                                    "Dedicação horária dos membros da equipe de gestão do Programa de P&D da Empresa, quadro efetivo."
                            }
                        }
                    },
                    new CategoriaContabil
                    {
                        Valor = "ST",
                        Nome = "Serviços de Terceiros",
                        Atividades = new List<Atividade>
                        {
                            new Atividade
                            {
                                Valor = "FG",
                                Nome =
                                    "Desenvolvimento de ferramenta para gestão do Programa de P&D da Empresa, excluindose aquisição de equipamentos."
                            },
                            new Atividade
                            {
                                Valor = "PP",
                                Nome =
                                    "Prospecção tecnológica e demais atividades necessárias ao planejamento e à elaboração do plano estratégico de investimento em P&D."
                            }
                        }
                    },
                    new CategoriaContabil
                    {
                        Valor = "MC",
                        Nome = "Materiais de Consumo",
                        Atividades = new List<Atividade>
                        {
                            new Atividade
                            {
                                Valor = "RP",
                                Nome = "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução."
                            }
                        }
                    },
                    new CategoriaContabil
                    {
                        Valor = "VD",
                        Nome = "Viagens e Diárias",
                        Atividades = new List<Atividade>
                        {
                            new Atividade
                            {
                                Valor = "EC",
                                Nome =
                                    "Participação dos membros da equipe de gestão em eventos sobre pesquisa, desenvolvimento e inovação relacionados ao setor elétrico e/ou em cursos de gestão tecnológica e da informação."
                            },
                            new Atividade
                            {
                                Valor = "PP",
                                Nome =
                                    "Prospecção tecnológica e demais atividades necessárias ao planejamento e à elaboração do plano estratégico de investimento em P&D."
                            },
                            new Atividade
                            {
                                Valor = "RP",
                                Nome = "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução."
                            },
                            new Atividade
                            {
                                Valor = "AP",
                                Nome =
                                    "Participação dos responsáveis técnicos pelos projetos de P&D nas avaliações presenciais convocadas pela ANEEL."
                            }
                        }
                    },
                    new CategoriaContabil
                    {
                        Valor = "OU",
                        Nome = "Outros",
                        Atividades = new List<Atividade>
                        {
                            new Atividade
                            {
                                Valor = "EC",
                                Nome =
                                    "Participação dos membros da equipe de gestão em eventos sobre pesquisa, desenvolvimento e inovação relacionados ao setor elétrico e/ou em cursos de gestão tecnológica e da informação."
                            },
                            new Atividade
                            {
                                Valor = "RP",
                                Nome = "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução."
                            },
                            new Atividade
                            {
                                Valor = "BA",
                                Nome = "Buscas de anterioridade no Instituto Nacional da Propriedade Industrial (INPI)."
                            }
                        }
                    },
                    new CategoriaContabil
                    {
                        Valor = "CT",
                        Nome = "CITENEL",
                        Atividades = new List<Atividade>
                        {
                            new Atividade {Valor = "AC", Nome = "Apoio à realização do CITENEL."}
                        }
                    },
                    new CategoriaContabil
                    {
                        Valor = "AC",
                        Nome = "Auditoria Contábil e Financeira",
                        Atividades = new List<Atividade>
                        {
                            new Atividade
                            {
                                Valor = "CA",
                                Nome = "Contratação de auditoria contábil e financeira para os projetos concluídos."
                            }
                        }
                    }
                };

                categorias.ForEach(t => _context.CategoriasContabeis.Add(t));
                _context.SaveChanges();
            }

            if (_context.ProdutoFasesCadeia.FirstOrDefault() == null)
            {
                var fases = new List<ProdutoFaseCadeia>
                {
                    new ProdutoFaseCadeia
                    {
                        Valor = "PB",
                        Nome = "Pesquisa Básica Dirigida",
                        TiposDetalhados = new List<CatalogProdutoTipoDetalhado>
                        {
                            new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Novo material"},
                            new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Nova estrutura"},
                            new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Modelo"},
                            new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Algoritmo"}
                        }
                    },
                    new ProdutoFaseCadeia
                    {
                        Valor = "PA",
                        Nome = "Pesquisa Aplicada",
                        TiposDetalhados = new List<CatalogProdutoTipoDetalhado>
                        {
                            new CatalogProdutoTipoDetalhado {Valor = "", Nome = "metodologia ou técnica"},
                            new CatalogProdutoTipoDetalhado
                                {Valor = "", Nome = "Projeto demonstrativo de novos equipamentos"},
                            new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Modelos digitais"},
                            new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Modelos de funções ou de processos"}
                        }
                    },
                    new ProdutoFaseCadeia
                    {
                        Valor = "DE",
                        Nome = "Desenvolvimento Experimental",
                        TiposDetalhados = new List<CatalogProdutoTipoDetalhado>
                        {
                            new CatalogProdutoTipoDetalhado
                                {Valor = "", Nome = "Protótipo de material para demonstração e testes"},
                            new CatalogProdutoTipoDetalhado
                                {Valor = "", Nome = "Protótipo de dispositivo para demonstração e testes"},
                            new CatalogProdutoTipoDetalhado
                                {Valor = "", Nome = "Protótipo de equipamento para demonstração e testes"},
                            new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Implantação de projeto piloto"},
                            new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Serviços (novos ou aperfeiçoados)"},
                            new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Software baseado em pesquisa aplicada"}
                        }
                    },
                    new ProdutoFaseCadeia
                    {
                        Valor = "CS",
                        Nome = "Cabeça de série",
                        TiposDetalhados = new List<CatalogProdutoTipoDetalhado>
                        {
                            new CatalogProdutoTipoDetalhado
                                {Valor = "", Nome = "Aperfeiçoamento de protótipo obtido em projeto anterior"}
                        }
                    },
                    new ProdutoFaseCadeia
                    {
                        Valor = "LP",
                        Nome = "Lote Pioneiro",
                        TiposDetalhados = new List<CatalogProdutoTipoDetalhado>
                        {
                            new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Primeira fabricação de produto"},
                            new CatalogProdutoTipoDetalhado
                                {Valor = "", Nome = "Reprodução de licenças para ensaios de validação"},
                            new CatalogProdutoTipoDetalhado
                            {
                                Valor = "",
                                Nome =
                                    "Análise de custos e refino do projeto, com vistas à produção industrial e/ou à comercialização"
                            }
                        }
                    },
                    new ProdutoFaseCadeia
                    {
                        Valor = "IM",
                        Nome = "Inserção no Mercado",
                        TiposDetalhados = new List<CatalogProdutoTipoDetalhado>
                        {
                            new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Estudos mercadológicos"},
                            new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Material de divulgação"},
                            new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Registro de patentes"},
                            new CatalogProdutoTipoDetalhado
                            {
                                Valor = "",
                                Nome = "Contratação de empresa de transferência de tecnologia e serviços jurídicos"
                            },
                            new CatalogProdutoTipoDetalhado
                                {Valor = "", Nome = "Aprimoramentos e melhorias incrementais nos produtos"},
                            new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Software ou serviços"}
                        }
                    }
                };

                fases.ForEach(t => _context.ProdutoFasesCadeia.Add(t));
                _context.SaveChanges();
            }
        }
    }
}