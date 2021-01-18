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
            #region Paises... Gzuis Ô.Ô

            if (_context.CatalogPaises.FirstOrDefault() == null)
            {
                var paises = new List<Pais>
                {
                    new Pais {Nome = "Açores"},
                    new Pais {Nome = "Acrotiri e Deceleia"},
                    new Pais {Nome = "Afeganistão"},
                    new Pais {Nome = "África do Sul"},
                    new Pais {Nome = "Albânia"},
                    new Pais {Nome = "Alemanha"},
                    new Pais {Nome = "Andorra"},
                    new Pais {Nome = "Angola"},
                    new Pais {Nome = "Anguila"},
                    new Pais {Nome = "Antártida"},
                    new Pais {Nome = "Antígua e Barbuda"},
                    new Pais {Nome = "Antilhas Holandesas"},
                    new Pais {Nome = "Arábia Saudita"},
                    new Pais {Nome = "Argélia"},
                    new Pais {Nome = "Argentina"},
                    new Pais {Nome = "Armênia"},
                    new Pais {Nome = "Aruba"},
                    new Pais {Nome = "Austrália"},
                    new Pais {Nome = "Áustria"},
                    new Pais {Nome = "Azerbaijão"},
                    new Pais {Nome = "Bahamas"},
                    new Pais {Nome = "Bangladesh"},
                    new Pais {Nome = "Barbados"},
                    new Pais {Nome = "Barém"},
                    new Pais {Nome = "Bélgica"},
                    new Pais {Nome = "Belize"},
                    new Pais {Nome = "Benim"},
                    new Pais {Nome = "Bermudas"},
                    new Pais {Nome = "Bielorrússia"},
                    new Pais {Nome = "Bolívia"},
                    new Pais {Nome = "Bósnia e Herzegovina"},
                    new Pais {Nome = "Botsuana"},
                    new Pais {Nome = "Brasil"},
                    new Pais {Nome = "Brunei"},
                    new Pais {Nome = "Bulgária"},
                    new Pais {Nome = "Burquina Faso"},
                    new Pais {Nome = "Burundi"},
                    new Pais {Nome = "Butão"},
                    new Pais {Nome = "Cabo Verde"},
                    new Pais {Nome = "Camarões"},
                    new Pais {Nome = "Camboja"},
                    new Pais {Nome = "Canadá"},
                    new Pais {Nome = "Canárias"},
                    new Pais {Nome = "Catar"},
                    new Pais {Nome = "Cazaquistão"},
                    new Pais {Nome = "Chade"},
                    new Pais {Nome = "Chile"},
                    new Pais {Nome = "China"},
                    new Pais {Nome = "Chipre"},
                    new Pais {Nome = "Colômbia"},
                    new Pais {Nome = "Comores"},
                    new Pais {Nome = "Coreia do Norte"},
                    new Pais {Nome = "Coreia do Sul"},
                    new Pais {Nome = "Costa do Marfim"},
                    new Pais {Nome = "Costa Rica"},
                    new Pais {Nome = "Croácia"},
                    new Pais {Nome = "Cuba"},
                    new Pais {Nome = "Curaçao"},
                    new Pais {Nome = "Dinamarca"},
                    new Pais {Nome = "Djibuti"},
                    new Pais {Nome = "Domínica"},
                    new Pais {Nome = "Egito"},
                    new Pais {Nome = "El Salvador"},
                    new Pais {Nome = "Emirados Árabes Unidos"},
                    new Pais {Nome = "Equador"},
                    new Pais {Nome = "Eritreia"},
                    new Pais {Nome = "Escócia"},
                    new Pais {Nome = "Eslováquia"},
                    new Pais {Nome = "Eslovênia"},
                    new Pais {Nome = "Espanha"},
                    new Pais {Nome = "Estados Unidos"},
                    new Pais {Nome = "Estônia"},
                    new Pais {Nome = "Etiópia"},
                    new Pais {Nome = "Faroé"},
                    new Pais {Nome = "Fiji"},
                    new Pais {Nome = "Filipinas"},
                    new Pais {Nome = "Finlândia"},
                    new Pais {Nome = "França"},
                    new Pais {Nome = "Gabão"},
                    new Pais {Nome = "Gâmbia"},
                    new Pais {Nome = "Gana"},
                    new Pais {Nome = "Geórgia"},
                    new Pais {Nome = "Geórgia do Sul e Sandwich do Sul"},
                    new Pais {Nome = "Gibraltar"},
                    new Pais {Nome = "Granada"},
                    new Pais {Nome = "Grécia"},
                    new Pais {Nome = "Groenlândia"},
                    new Pais {Nome = "Guadalupe"},
                    new Pais {Nome = "Guam"},
                    new Pais {Nome = "Guatemala"},
                    new Pais {Nome = "Guernsey"},
                    new Pais {Nome = "Guiana"},
                    new Pais {Nome = "Guiana Francesa"},
                    new Pais {Nome = "Guiné"},
                    new Pais {Nome = "Guiné Bissau"},
                    new Pais {Nome = "Guiné Equatorial"},
                    new Pais {Nome = "Haiti"},
                    new Pais {Nome = "Holanda (Países baixos)"},
                    new Pais {Nome = "Honduras"},
                    new Pais {Nome = "Hong Kong"},
                    new Pais {Nome = "Hungria"},
                    new Pais {Nome = "Iêmen"},
                    new Pais {Nome = "Ilha Bouvet"},
                    new Pais {Nome = "Ilha da Madeira"},
                    new Pais {Nome = "Ilha de Clipperton"},
                    new Pais {Nome = "Ilha de Man"},
                    new Pais {Nome = "Ilha de Navassa"},
                    new Pais {Nome = "Ilha do Natal"},
                    new Pais {Nome = "Ilha Jan Mayen"},
                    new Pais {Nome = "Ilha Norfolk"},
                    new Pais {Nome = "Ilha Wake"},
                    new Pais {Nome = "Ilhas Ashmore e Cartier"},
                    new Pais {Nome = "Ilhas Caimão"},
                    new Pais {Nome = "Ilhas Cocos"},
                    new Pais {Nome = "Ilhas Cook"},
                    new Pais {Nome = "Ilhas do mar de coral"},
                    new Pais {Nome = "Ilhas Falkland"},
                    new Pais {Nome = "Ilhas Heard e McDonald"},
                    new Pais {Nome = "Ilhas Marshall"},
                    new Pais {Nome = "Ilhas Paracel"},
                    new Pais {Nome = "Ilhas Pitcairn"},
                    new Pais {Nome = "Ilhas Salomão"},
                    new Pais {Nome = "Ilhas Spratly"},
                    new Pais {Nome = "Ilhas Turcas e Caicos"},
                    new Pais {Nome = "Ilhas Virgens Americanas"},
                    new Pais {Nome = "Ilhas Virgens Britânicas"},
                    new Pais {Nome = "Índia"},
                    new Pais {Nome = "Indonésia"},
                    new Pais {Nome = "Inglaterra"},
                    new Pais {Nome = "Irã"},
                    new Pais {Nome = "Iraque"},
                    new Pais {Nome = "Irlanda"},
                    new Pais {Nome = "Irlanda do norte"},
                    new Pais {Nome = "Islândia"},
                    new Pais {Nome = "Israel"},
                    new Pais {Nome = "Itália"},
                    new Pais {Nome = "Jamaica"},
                    new Pais {Nome = "Japão"},
                    new Pais {Nome = "Jersey"},
                    new Pais {Nome = "Jordânia"},
                    new Pais {Nome = "Kosovo"},
                    new Pais {Nome = "Kuwait"},
                    new Pais {Nome = "Laos"},
                    new Pais {Nome = "Lesoto"},
                    new Pais {Nome = "Letônia"},
                    new Pais {Nome = "Líbano"},
                    new Pais {Nome = "Libéria"},
                    new Pais {Nome = "Líbia"},
                    new Pais {Nome = "Liechtenstein"},
                    new Pais {Nome = "Lituânia"},
                    new Pais {Nome = "Luxemburgo"},
                    new Pais {Nome = "Macau"},
                    new Pais {Nome = "Macedônia"},
                    new Pais {Nome = "Madagascar"},
                    new Pais {Nome = "Malásia"},
                    new Pais {Nome = "Malawi"},
                    new Pais {Nome = "Maldivas"},
                    new Pais {Nome = "Mali"},
                    new Pais {Nome = "Malta"},
                    new Pais {Nome = "Marianas do Norte"},
                    new Pais {Nome = "Marrocos"},
                    new Pais {Nome = "Martinica"},
                    new Pais {Nome = "Maurício"},
                    new Pais {Nome = "Mauritânia"},
                    new Pais {Nome = "Mayotte"},
                    new Pais {Nome = "México"},
                    new Pais {Nome = "Micronésia"},
                    new Pais {Nome = "Moçambique"},
                    new Pais {Nome = "Moldávia"},
                    new Pais {Nome = "Mônaco"},
                    new Pais {Nome = "Mongólia"},
                    new Pais {Nome = "Monserrate"},
                    new Pais {Nome = "Montenegro"},
                    new Pais {Nome = "Myanmar"},
                    new Pais {Nome = "Namíbia"},
                    new Pais {Nome = "Nauru"},
                    new Pais {Nome = "Nepal"},
                    new Pais {Nome = "Nicarágua"},
                    new Pais {Nome = "Níger"},
                    new Pais {Nome = "Nigéria"},
                    new Pais {Nome = "Niue"},
                    new Pais {Nome = "Noruega"},
                    new Pais {Nome = "Nova Caledônia"},
                    new Pais {Nome = "Nova Zelândia"},
                    new Pais {Nome = "Omã"},
                    new Pais {Nome = "País de Gales"},
                    new Pais {Nome = "Palau"},
                    new Pais {Nome = "Palestina"},
                    new Pais {Nome = "Panamá"},
                    new Pais {Nome = "Papua-Nova Guiné"},
                    new Pais {Nome = "Paquistão"},
                    new Pais {Nome = "Paraguai"},
                    new Pais {Nome = "Peru"},
                    new Pais {Nome = "Polinésia Francesa"},
                    new Pais {Nome = "Polônia"},
                    new Pais {Nome = "Porto Rico"},
                    new Pais {Nome = "Portugal"},
                    new Pais {Nome = "Quênia"},
                    new Pais {Nome = "Quirguizistão"},
                    new Pais {Nome = "Quiribati"},
                    new Pais {Nome = "Reino Unido"},
                    new Pais {Nome = "República Centro-Africana"},
                    new Pais {Nome = "República Checa"},
                    new Pais {Nome = "República Democrática do Congo"},
                    new Pais {Nome = "República do Congo"},
                    new Pais {Nome = "República Dominicana"},
                    new Pais {Nome = "Romênia"},
                    new Pais {Nome = "Ruanda"},
                    new Pais {Nome = "Rússia"},
                    new Pais {Nome = "Saara Ocidental"},
                    new Pais {Nome = "Samoa"},
                    new Pais {Nome = "Samoa Americana"},
                    new Pais {Nome = "Santa Helena"},
                    new Pais {Nome = "Santa Lúcia"},
                    new Pais {Nome = "São Cristóvão e Neves"},
                    new Pais {Nome = "São Marinho"},
                    new Pais {Nome = "São Pedro e Miquelon"},
                    new Pais {Nome = "São Tomé e Príncipe"},
                    new Pais {Nome = "São Vicente e Granadinas"},
                    new Pais {Nome = "Seicheles"},
                    new Pais {Nome = "Senegal"},
                    new Pais {Nome = "Serra Leoa"},
                    new Pais {Nome = "Sérvia"},
                    new Pais {Nome = "Singapura"},
                    new Pais {Nome = "Síria"},
                    new Pais {Nome = "Somália"},
                    new Pais {Nome = "Sri Lanka"},
                    new Pais {Nome = "Suazilândia"},
                    new Pais {Nome = "Sudão"},
                    new Pais {Nome = "Sudão do Sul"},
                    new Pais {Nome = "Suécia"},
                    new Pais {Nome = "Suíça"},
                    new Pais {Nome = "Suriname"},
                    new Pais {Nome = "Tailândia"},
                    new Pais {Nome = "Taiwan"},
                    new Pais {Nome = "Tajiquistão"},
                    new Pais {Nome = "Tanzânia"},
                    new Pais {Nome = "Território Britânico do Oceano Índico"},
                    new Pais {Nome = "Territórios Austrais Franceses"},
                    new Pais {Nome = "Timor Leste"},
                    new Pais {Nome = "Togo"},
                    new Pais {Nome = "Tokelau"},
                    new Pais {Nome = "Tonga"},
                    new Pais {Nome = "Trindade e Tobago"},
                    new Pais {Nome = "Tunísia"},
                    new Pais {Nome = "Turquemenistão"},
                    new Pais {Nome = "Turquia"},
                    new Pais {Nome = "Tuvalu"},
                    new Pais {Nome = "Ucrânia"},
                    new Pais {Nome = "Uganda"},
                    new Pais {Nome = "Uruguai"},
                    new Pais {Nome = "Uzbequistão"},
                    new Pais {Nome = "Vanuatu"},
                    new Pais {Nome = "Vaticano"},
                    new Pais {Nome = "Venezuela"},
                    new Pais {Nome = "Vietnã"},
                    new Pais {Nome = "Wallis e Futuna"},
                    new Pais {Nome = "Zâmbia"},
                    new Pais {Nome = "Zimbabué"}
                };
                paises.ForEach(t => _context.CatalogPaises.Add(t));
                _context.SaveChanges();
            }

            #endregion

            if (_context.Temas.FirstOrDefault() == null)
            {
                var temas = new List<Tema>
                {
                    new Tema
                    {
                        Valor = "FA",
                        Nome = "Fontes alternativas de geração de energia elétrica",
                        SubTemas = new List<Tema>
                        {
                            new Tema
                            {
                                Valor = "FA01",
                                Nome =
                                    "Alternativas energéticas sustentáveis de atendimento a pequenos sistemas isolados."
                            },
                            new Tema
                                {Valor = "FA02", Nome = "Geração de energia a partir de resíduos sólidos urbanos."},
                            new Tema
                            {
                                Valor = "FA03",
                                Nome = "Novos materiais e equipamentos para geração de energia por fontes alternativas."
                            },
                            new Tema
                            {
                                Valor = "FA04",
                                Nome = "Tecnologias para aproveitamento de novos combustíveis em plantas geradoras."
                            },
                            new Tema {Valor = "FA0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new Tema
                    {
                        Valor = "GT",
                        Nome = "Geração Termelétrica",
                        SubTemas = new List<Tema>
                        {
                            new Tema
                            {
                                Valor = "GT01",
                                Nome =
                                    "Avaliação de riscos e incertezas do fornecimento contínuo de gás natural para geração termelétrica."
                            },
                            new Tema
                            {
                                Valor = "GT02",
                                Nome =
                                    "Novas técnicas para eficientização e diminuição da emissão de poluentes de usinas termelétricas a combustível derivado de petróleo."
                            },
                            new Tema
                            {
                                Valor = "GT03",
                                Nome =
                                    "Otimização da geração de energia elétrica em plantas industriais: aumento de eficiência na cogeração."
                            },
                            new Tema {Valor = "GT04", Nome = "Micro-sistemas de cogeração residenciais."},
                            new Tema
                            {
                                Valor = "GT05", Nome = "Técnicas para captura e seqüestro de carbono de termelétricas."
                            },
                            new Tema {Valor = "GT0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new Tema
                    {
                        Valor = "GB",
                        Nome = "Gestão de Bacias e Reservatórios",
                        SubTemas = new List<Tema>
                        {
                            new Tema
                            {
                                Valor = "GB01",
                                Nome =
                                    "Emissões de gases de efeito estufa (GEE) em reservatórios de usinas hidrelétricas."
                            },
                            new Tema
                            {
                                Valor = "GB02",
                                Nome =
                                    "Efeitos de mudanças climáticas globais no regime hidrológico de bacias hidrográficas."
                            },
                            new Tema
                            {
                                Valor = "GB03",
                                Nome = "Integração e otimização do uso múltiplo de reservatórios hidrelétricos."
                            },
                            new Tema
                            {
                                Valor = "GB04",
                                Nome = "Gestão sócio-patrimonial de reservatórios de usinas hidrelétricas."
                            },
                            new Tema
                                {Valor = "GB05", Nome = "Gestão da segurança de barragens de usinas hidrelétricas."},
                            new Tema
                            {
                                Valor = "GB06",
                                Nome = "Assoreamento de reservatórios formados por barragens de usinas hidrelétricas."
                            },
                            new Tema {Valor = "GB0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new Tema
                    {
                        Valor = "MA",
                        Nome = "Meio Ambiente",
                        SubTemas = new List<Tema>
                        {
                            new Tema
                            {
                                Valor = "MA01",
                                Nome = "Impactos e restrições socioambientais de sistemas de energia elétrica."
                            },
                            new Tema
                            {
                                Valor = "MA02",
                                Nome =
                                    "Metodologias para mensuração econômico-financeira de externalidades em sistemas de energia elétrica."
                            },
                            new Tema
                            {
                                Valor = "MA03",
                                Nome =
                                    "Estudos de toxicidade relacionados à deterioração da qualidade da água em reservatórios. "
                            },
                            new Tema {Valor = "MA0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new Tema
                    {
                        Valor = "SE",
                        Nome = "Segurança",
                        SubTemas = new List<Tema>
                        {
                            new Tema
                            {
                                Valor = "SE01",
                                Nome =
                                    "Identificação e mitigação dos impactos de campos eletromagnéticos em organismos vivos."
                            },
                            new Tema
                                {Valor = "SE02", Nome = "Análise e mitigação de riscos de acidentes elétricos."},
                            new Tema
                                {Valor = "SE03", Nome = "Novas tecnologias para equipamentos de proteção individual."},
                            new Tema
                            {
                                Valor = "SE04",
                                Nome = "Novas tecnologias para inspeção e manutenção de sistemas elétricos."
                            },
                            new Tema {Valor = "SE0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new Tema
                    {
                        Valor = "EE",
                        Nome = "Eficiência Energética",
                        SubTemas = new List<Tema>
                        {
                            new Tema
                                {Valor = "EE01", Nome = "Novas tecnologias para melhoria da eficiência energética."},
                            new Tema {Valor = "EE02", Nome = "Gerenciamento de carga pelo lado da demanda."},
                            new Tema
                                {Valor = "EE03", Nome = "Definição de indicadores de eficiência energética."},
                            new Tema
                            {
                                Valor = "EE04",
                                Nome = "Metodologias para avaliação de resultados de projetos de eficiência energética."
                            },
                            new Tema {Valor = "EE0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new Tema
                    {
                        Valor = "PL",
                        Nome = "Planejamento de Sistemas de Energia Elétrica",
                        SubTemas = new List<Tema>
                        {
                            new Tema
                                {Valor = "PL01", Nome = "Planejamento integrado da expansão de sistemas elétricos."},
                            new Tema {Valor = "PL02", Nome = "Integração de centrais eólicas ao SIN."},
                            new Tema
                                {Valor = "PL03", Nome = "Integração de geração distribuída a redes elétricas."},
                            new Tema
                            {
                                Valor = "PL04",
                                Nome =
                                    "Metodologia de previsão de mercado para diferentes níveis temporais e estratégias de contratação."
                            },
                            new Tema
                            {
                                Valor = "PL05",
                                Nome = "Modelos hidrodinâmicos aplicados em reservatórios de usinas hidrelétricas."
                            },
                            new Tema
                            {
                                Valor = "PL06", Nome = "Materiais supercondutores para transmissão de energia elétrica."
                            },
                            new Tema
                            {
                                Valor = "PL07",
                                Nome = "Tecnologias e sistemas de transmissão de energia em longas distâncias."
                            },
                            new Tema {Valor = "PL0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new Tema
                    {
                        Valor = "OP",
                        Nome = "Operação de Sistemas de Energia Elétrica",
                        SubTemas = new List<Tema>
                        {
                            new Tema
                            {
                                Valor = "OP01",
                                Nome =
                                    "Ferramentas de apoio à operação de sistemas elétricos de potência em tempo real."
                            },
                            new Tema
                            {
                                Valor = "OP02",
                                Nome = "Critérios de gerenciamento de carga para diferentes níveis de hierarquia."
                            },
                            new Tema
                            {
                                Valor = "OP03",
                                Nome = "Estruturas, funções e regras de operação dos mercados de serviços ancilares."
                            },
                            new Tema
                            {
                                Valor = "OP04",
                                Nome = "Otimização estrutural e paramétrica da capacidade dos sistemas de distribuição."
                            },
                            new Tema
                            {
                                Valor = "OP05",
                                Nome = "Alocação de fontes de potência reativa em sistemas de distribuição."
                            },
                            new Tema
                            {
                                Valor = "OP06",
                                Nome = "Estudo, simulação e análise do desempenho de sistemas elétricos de potência."
                            },
                            new Tema
                            {
                                Valor = "OP07",
                                Nome =
                                    "Análise das grandes perturbações e impactos no planejamento, operação e controle."
                            },
                            new Tema
                            {
                                Valor = "OP08",
                                Nome = "Desenvolvimento de modelos para a otimização de despacho hidrotérmico."
                            },
                            new Tema
                            {
                                Valor = "OP09",
                                Nome =
                                    "Desenvolvimento e/ou aprimoramento dos modelos de previsão de chuva versus vazão."
                            },
                            new Tema
                            {
                                Valor = "OP10",
                                Nome = "Sistemas de monitoramento da operação de usinas não-despachadas pelo ONS."
                            },
                            new Tema {Valor = "OP0X", Nome = "Outros."}
                        }
                    },
                    new Tema
                    {
                        Valor = "SC",
                        Nome = "Supervisão, Controle e Proteção de Sistemas de Energia Elétrica",
                        SubTemas = new List<Tema>
                        {
                            new Tema
                            {
                                Valor = "SC01",
                                Nome = "Implementação de sistemas de controle (robusto, adaptativo e inteligente)."
                            },
                            new Tema {Valor = "SC02", Nome = "Análise dinâmica de sistemas em tempo real."},
                            new Tema
                            {
                                Valor = "SC03",
                                Nome = "Técnicas eficientes de restauração rápida de grandes centros de carga."
                            },
                            new Tema
                            {
                                Valor = "SC04",
                                Nome = "Desenvolvimento de técnicas para recomposição de sistemas elétricos."
                            },
                            new Tema
                            {
                                Valor = "SC05",
                                Nome =
                                    "Técnicas de inteligência artificial aplicadas ao controle, operação e proteção de sistemas elétricos."
                            },
                            new Tema
                            {
                                Valor = "SC06",
                                Nome = "Novas tecnologias para supervisão do fornecimento de energia elétrica."
                            },
                            new Tema
                                {Valor = "SC07", Nome = "Desenvolvimento e aplicação de sistemas de medição fasorial."},
                            new Tema {Valor = "SC08", Nome = "Análise de falhas em sistemas elétricos."},
                            new Tema
                                {Valor = "SC09", Nome = "Compatibilidade eletromagnética em sistemas elétricos."},
                            new Tema {Valor = "SC10", Nome = "Sistemas de aterramento."},
                            new Tema {Valor = "SC0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new Tema
                    {
                        Valor = "QC",
                        Nome = "Qualidade e Confiabilidade dos Serviços de Energia Elétrica",
                        SubTemas = new List<Tema>
                        {
                            new Tema
                            {
                                Valor = "QC01",
                                Nome =
                                    "Sistemas e técnicas de monitoração e gerenciamento da qualidade da energia elétrica."
                            },
                            new Tema
                            {
                                Valor = "QC02",
                                Nome = "Modelagem e análise dos distúrbios associados à qualidade da energia elétrica."
                            },
                            new Tema
                            {
                                Valor = "QC03",
                                Nome =
                                    "Requisitos para conexão de cargas potencialmente perturbadoras no sistema elétrico."
                            },
                            new Tema
                            {
                                Valor = "QC04", Nome = "Curvas de sensibilidade e de suportabilidade de equipamentos."
                            },
                            new Tema
                            {
                                Valor = "QC05",
                                Nome = "Impactos econômicos e aspectos contratuais da qualidade da energia elétrica."
                            },
                            new Tema
                            {
                                Valor = "QC06",
                                Nome = "Compensação financeira por violação de indicadores de qualidade."
                            },
                            new Tema {Valor = "QC0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new Tema
                    {
                        Valor = "MF",
                        Nome = "Medição, faturamento e combate a perdas comerciais",
                        SubTemas = new List<Tema>
                        {
                            new Tema
                            {
                                Valor = "MF01", Nome = "Avaliação econômica para definição da perda mínima atingível."
                            },
                            new Tema
                            {
                                Valor = "MF02",
                                Nome = "Estimação, análise e redução de perdas técnicas em sistemas elétricos."
                            },
                            new Tema
                            {
                                Valor = "MF03",
                                Nome =
                                    "Desenvolvimento de tecnologias para combate à fraude e ao furto de energia elétrica."
                            },
                            new Tema
                            {
                                Valor = "MF04",
                                Nome =
                                    "Diagnóstico, prospecção e redução da vulnerabilidade de sistemas elétricos ao furto e à fraude."
                            },
                            new Tema
                            {
                                Valor = "MF05",
                                Nome = "Energia economizada e agregada ao mercado após regularização de fraudes."
                            },
                            new Tema
                            {
                                Valor = "MF06",
                                Nome = "Uso de indicadores socioeconômicos, dados fiscais e gastos com outros insumos."
                            },
                            new Tema
                            {
                                Valor = "MF07",
                                Nome = "Gerenciamento dos equipamentos de medição (qualidade e redução de falhas)."
                            },
                            new Tema
                            {
                                Valor = "MF08",
                                Nome = "Impacto dos projetos de eficiência energética na redução de perdas comerciais."
                            },
                            new Tema
                            {
                                Valor = "MF09",
                                Nome =
                                    "Sistemas centralizados de medição, controle e gerenciamento de energia em consumidores finais."
                            },
                            new Tema
                                {Valor = "MF10", Nome = "Sistemas de tarifação e novas estruturas tarifárias."},
                            new Tema {Valor = "MF0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new Tema
                    {
                        Valor = "OU",
                        Nome = "Outros",
                        SubTemas = new List<Tema>
                        {
                            new Tema {Valor = "OU  ", Nome = "Outros"}
                        },
                        Order = 1
                    }
                };

                _context.AddRange(temas);

                _context.SaveChanges();
            }


            if (_context.CatalogCategoriaContabilGestao.FirstOrDefault() == null)
            {
                var temas = new List<CategoriaContabil>
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

                temas.ForEach(t => _context.CatalogCategoriaContabilGestao.Add(t));
                _context.SaveChanges();
            }

            if (_context.CatalogProdutoFaseCadeia.FirstOrDefault() == null)
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

                fases.ForEach(t => _context.CatalogProdutoFaseCadeia.Add(t));
                _context.SaveChanges();
            }
        }
    }
}