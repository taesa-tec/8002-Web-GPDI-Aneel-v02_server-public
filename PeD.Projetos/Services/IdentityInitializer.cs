using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PeD.Core.Models;
using PeD.Core.Models.Catalogs;
using PeD.Data;
using CatalogAtividade = PeD.Projetos.Models.Catalogs.CatalogAtividade;
using CatalogCategoriaContabilGestao = PeD.Projetos.Models.Catalogs.CatalogCategoriaContabilGestao;
using CatalogEmpresa = PeD.Projetos.Models.Catalogs.CatalogEmpresa;
using CatalogProdutoFaseCadeia = PeD.Projetos.Models.Catalogs.CatalogProdutoFaseCadeia;
using CatalogProdutoTipoDetalhado = PeD.Projetos.Models.Catalogs.CatalogProdutoTipoDetalhado;
using CatalogSegmento = PeD.Projetos.Models.Catalogs.CatalogSegmento;
using CatalogStatus = PeD.Projetos.Models.Catalogs.CatalogStatus;
using Estado = PeD.Projetos.Models.Catalogs.Estado;
using Pais = PeD.Projetos.Models.Catalogs.Pais;

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
            _context = scope.ServiceProvider.GetRequiredService<DbContext>();
            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //_mailService = scope.ServiceProvider.GetRequiredService<MailService>();
            //_userService = new UserService(_context, _userManager, _roleManager, _mailService,scope.ServiceProvider.GetService<AccessManager>());
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
                        Status = UserStatus.Ativo,
                        Role = "Admin-PeD"
                    }, adminPass, Roles.AdminGestor);
            }
        }

        protected void CreateCatalog()
        {
            if (_context.CatalogStatus.FirstOrDefault() == null)
            {
                var status = new List<CatalogStatus>
                {
                    new CatalogStatus
                    {
                        Status = "Proposta"
                    },
                    new CatalogStatus
                    {
                        Status = "Iniciado"
                    },
                    new CatalogStatus
                    {
                        Status = "Encerrado"
                    }
                };
                status.ForEach(t => _context.CatalogStatus.Add(t));
                _context.SaveChanges();
            }

            if (_context.CatalogSegmentos.FirstOrDefault() == null)
            {
                var segmento = new List<CatalogSegmento>
                {
                    new CatalogSegmento
                    {
                        Nome = "Geração", Valor = "G"
                    },
                    new CatalogSegmento
                    {
                        Nome = "Transmissão", Valor = "T"
                    },
                    new CatalogSegmento
                    {
                        Nome = "Distribuição", Valor = "D"
                    },
                    new CatalogSegmento
                    {
                        Nome = "Comercialização", Valor = "C"
                    }
                };
                segmento.ForEach(t => _context.CatalogSegmentos.Add(t));
                _context.SaveChanges();
            }

            if (_context.CatalogEmpresas.FirstOrDefault() == null)
            {
                var empresa = new List<CatalogEmpresa>
                {
                    new CatalogEmpresa
                    {
                        Nome = "TAESA",
                        Valor = "07130",
                        Cnpj = "07.859.971/0001-30"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "ATE",
                        Valor = "04906",
                        Cnpj = "07.859.971/0001-30"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "ATE II",
                        Valor = "05012",
                        Cnpj = "07.859.971/0001-30"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "ATE III",
                        Valor = "05455",
                        Cnpj = "07.002.685/0001-54"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "ETEO",
                        Valor = "	0414",
                        Cnpj = "07.859.971/0001-30"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "GTESA",
                        Valor = "03624",
                        Cnpj = "07.859.971/0001-30"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "MARIANA",
                        Valor = "08837",
                        Cnpj = "19.486.977/0001-99"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "MUNIRAH",
                        Valor = "04757",
                        Cnpj = "07.859.971/0001-30"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "NOVATRANS",
                        Valor = "02609",
                        Cnpj = "07.859.971/0001-30"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "NTE",
                        Valor = "03619",
                        Cnpj = "07.859.971/0001-30"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "PATESA",
                        Valor = "03943",
                        Cnpj = "07.859.971/0001-30"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "São Gotardo",
                        Valor = "08193",
                        Cnpj = "15.867.360/0001-62"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "STE",
                        Valor = "03944",
                        Cnpj = "07.859.971/0001-30"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "TSN",
                        Valor = "02607",
                        Cnpj = "07.859.971/0001-30"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "ETAU",
                        Valor = "03942",
                        Cnpj = "05.063.249/0001-60"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "BRASNORTE",
                        Valor = "06625",
                        Cnpj = "09.274.998/0001-97"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "MIRACEMA",
                        Valor = "10731",
                        Cnpj = "24.944.194/0001-41"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "JANAÚBA",
                        Valor = "11114",
                        Cnpj = "26.617.923/0001-80"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "AIMORÉS",
                        Valor = "11105",
                        Cnpj = "26.707.830/0001-47"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "PARAGUAÇÚ",
                        Valor = "11104",
                        Cnpj = "26.712.591/0001-13"
                    },
                    new CatalogEmpresa
                    {
                        Nome = "ERB 1",
                        Valor = "00000",
                        Cnpj = "28.052.123/0001-95"
                    }
                };
                empresa.ForEach(t => _context.CatalogEmpresas.Add(t));
                _context.SaveChanges();
            }

            if (_context.CatalogEstados.FirstOrDefault() == null)
            {
                var estados = new List<Estado>
                {
                    new Estado
                    {
                        Nome = "ACRE", Valor = "AC"
                    },
                    new Estado
                    {
                        Nome = "ALAGOAS", Valor = "AL"
                    },
                    new Estado
                    {
                        Nome = "AMAPÁ", Valor = "AP"
                    },
                    new Estado
                    {
                        Nome = "AMAZONAS", Valor = "AM"
                    },
                    new Estado
                    {
                        Nome = "BAHIA", Valor = "BA"
                    },
                    new Estado
                    {
                        Nome = "CEARÁ", Valor = "CE"
                    },
                    new Estado
                    {
                        Nome = "DISTRITO FEDERAL", Valor = "DF"
                    },
                    new Estado
                    {
                        Nome = "ESPÍRITO SANTO", Valor = "ES"
                    },
                    new Estado
                    {
                        Nome = "GOIÁS", Valor = "GO"
                    },
                    new Estado
                    {
                        Nome = "MARANHÃO", Valor = "MA"
                    },
                    new Estado
                    {
                        Nome = "MATO GROSSO", Valor = "MT"
                    },
                    new Estado
                    {
                        Nome = "MATO GROSSO DO SUL", Valor = "MS"
                    },
                    new Estado
                    {
                        Nome = "MINAS GERAIS", Valor = "MG"
                    },
                    new Estado
                    {
                        Nome = "PARÁ", Valor = "PA"
                    },
                    new Estado
                    {
                        Nome = "PARAÍBA", Valor = "PB"
                    },
                    new Estado
                    {
                        Nome = "PARANÁ", Valor = "PR"
                    },
                    new Estado
                    {
                        Nome = "PERNAMBUCO", Valor = "PE"
                    },
                    new Estado
                    {
                        Nome = "PIAUÍ", Valor = "PI"
                    },
                    new Estado
                    {
                        Nome = "RIO DE JANEIRO", Valor = "RJ"
                    },
                    new Estado
                    {
                        Nome = "RIO GRANDE DO NORTE", Valor = "RN"
                    },
                    new Estado
                    {
                        Nome = "RIO GRANDE DO SUL", Valor = "RS"
                    },
                    new Estado
                    {
                        Nome = "RONDONIA", Valor = "RO"
                    },
                    new Estado
                    {
                        Nome = "RORAIMA", Valor = "RR"
                    },
                    new Estado
                    {
                        Nome = "SANTA CATARINA", Valor = "SC"
                    },
                    new Estado
                    {
                        Nome = "SÃO PAULO", Valor = "SP"
                    },
                    new Estado
                    {
                        Nome = "SERGIPE", Valor = "SE"
                    },
                    new Estado
                    {
                        Nome = "TOCANTINS", Valor = "TO"
                    }
                };
                estados.ForEach(t => _context.CatalogEstados.Add(t));
                _context.SaveChanges();
            }

            #region Paises... Gzuis Ô.Ô

            if (_context.CatalogPaises.FirstOrDefault() == null)
            {
                var paises = new List<Pais>
                {
                    new Pais
                    {
                        Nome = "Açores"
                    },
                    new Pais
                    {
                        Nome = "Acrotiri e Deceleia"
                    },
                    new Pais
                    {
                        Nome = "Afeganistão"
                    },
                    new Pais
                    {
                        Nome = "África do Sul"
                    },
                    new Pais
                    {
                        Nome = "Albânia"
                    },
                    new Pais
                    {
                        Nome = "Alemanha"
                    },
                    new Pais
                    {
                        Nome = "Andorra"
                    },
                    new Pais
                    {
                        Nome = "Angola"
                    },
                    new Pais
                    {
                        Nome = "Anguila"
                    },
                    new Pais
                    {
                        Nome = "Antártida"
                    },
                    new Pais
                    {
                        Nome = "Antígua e Barbuda"
                    },
                    new Pais
                    {
                        Nome = "Antilhas Holandesas"
                    },
                    new Pais
                    {
                        Nome = "Arábia Saudita"
                    },
                    new Pais
                    {
                        Nome = "Argélia"
                    },
                    new Pais
                    {
                        Nome = "Argentina"
                    },
                    new Pais
                    {
                        Nome = "Armênia"
                    },
                    new Pais
                    {
                        Nome = "Aruba"
                    },
                    new Pais
                    {
                        Nome = "Austrália"
                    },
                    new Pais
                    {
                        Nome = "Áustria"
                    },
                    new Pais
                    {
                        Nome = "Azerbaijão"
                    },
                    new Pais
                    {
                        Nome = "Bahamas"
                    },
                    new Pais
                    {
                        Nome = "Bangladesh"
                    },
                    new Pais
                    {
                        Nome = "Barbados"
                    },
                    new Pais
                    {
                        Nome = "Barém"
                    },
                    new Pais
                    {
                        Nome = "Bélgica"
                    },
                    new Pais
                    {
                        Nome = "Belize"
                    },
                    new Pais
                    {
                        Nome = "Benim"
                    },
                    new Pais
                    {
                        Nome = "Bermudas"
                    },
                    new Pais
                    {
                        Nome = "Bielorrússia"
                    },
                    new Pais
                    {
                        Nome = "Bolívia"
                    },
                    new Pais
                    {
                        Nome = "Bósnia e Herzegovina"
                    },
                    new Pais
                    {
                        Nome = "Botsuana"
                    },
                    new Pais
                    {
                        Nome = "Brasil"
                    },
                    new Pais
                    {
                        Nome = "Brunei"
                    },
                    new Pais
                    {
                        Nome = "Bulgária"
                    },
                    new Pais
                    {
                        Nome = "Burquina Faso"
                    },
                    new Pais
                    {
                        Nome = "Burundi"
                    },
                    new Pais
                    {
                        Nome = "Butão"
                    },
                    new Pais
                    {
                        Nome = "Cabo Verde"
                    },
                    new Pais
                    {
                        Nome = "Camarões"
                    },
                    new Pais
                    {
                        Nome = "Camboja"
                    },
                    new Pais
                    {
                        Nome = "Canadá"
                    },
                    new Pais
                    {
                        Nome = "Canárias"
                    },
                    new Pais
                    {
                        Nome = "Catar"
                    },
                    new Pais
                    {
                        Nome = "Cazaquistão"
                    },
                    new Pais
                    {
                        Nome = "Chade"
                    },
                    new Pais
                    {
                        Nome = "Chile"
                    },
                    new Pais
                    {
                        Nome = "China"
                    },
                    new Pais
                    {
                        Nome = "Chipre"
                    },
                    new Pais
                    {
                        Nome = "Colômbia"
                    },
                    new Pais
                    {
                        Nome = "Comores"
                    },
                    new Pais
                    {
                        Nome = "Coreia do Norte"
                    },
                    new Pais
                    {
                        Nome = "Coreia do Sul"
                    },
                    new Pais
                    {
                        Nome = "Costa do Marfim"
                    },
                    new Pais
                    {
                        Nome = "Costa Rica"
                    },
                    new Pais
                    {
                        Nome = "Croácia"
                    },
                    new Pais
                    {
                        Nome = "Cuba"
                    },
                    new Pais
                    {
                        Nome = "Curaçao"
                    },
                    new Pais
                    {
                        Nome = "Dinamarca"
                    },
                    new Pais
                    {
                        Nome = "Djibuti"
                    },
                    new Pais
                    {
                        Nome = "Domínica"
                    },
                    new Pais
                    {
                        Nome = "Egito"
                    },
                    new Pais
                    {
                        Nome = "El Salvador"
                    },
                    new Pais
                    {
                        Nome = "Emirados Árabes Unidos"
                    },
                    new Pais
                    {
                        Nome = "Equador"
                    },
                    new Pais
                    {
                        Nome = "Eritreia"
                    },
                    new Pais
                    {
                        Nome = "Escócia"
                    },
                    new Pais
                    {
                        Nome = "Eslováquia"
                    },
                    new Pais
                    {
                        Nome = "Eslovênia"
                    },
                    new Pais
                    {
                        Nome = "Espanha"
                    },
                    new Pais
                    {
                        Nome = "Estados Unidos"
                    },
                    new Pais
                    {
                        Nome = "Estônia"
                    },
                    new Pais
                    {
                        Nome = "Etiópia"
                    },
                    new Pais
                    {
                        Nome = "Faroé"
                    },
                    new Pais
                    {
                        Nome = "Fiji"
                    },
                    new Pais
                    {
                        Nome = "Filipinas"
                    },
                    new Pais
                    {
                        Nome = "Finlândia"
                    },
                    new Pais
                    {
                        Nome = "França"
                    },
                    new Pais
                    {
                        Nome = "Gabão"
                    },
                    new Pais
                    {
                        Nome = "Gâmbia"
                    },
                    new Pais
                    {
                        Nome = "Gana"
                    },
                    new Pais
                    {
                        Nome = "Geórgia"
                    },
                    new Pais
                    {
                        Nome = "Geórgia do Sul e Sandwich do Sul"
                    },
                    new Pais
                    {
                        Nome = "Gibraltar"
                    },
                    new Pais
                    {
                        Nome = "Granada"
                    },
                    new Pais
                    {
                        Nome = "Grécia"
                    },
                    new Pais
                    {
                        Nome = "Groenlândia"
                    },
                    new Pais
                    {
                        Nome = "Guadalupe"
                    },
                    new Pais
                    {
                        Nome = "Guam"
                    },
                    new Pais
                    {
                        Nome = "Guatemala"
                    },
                    new Pais
                    {
                        Nome = "Guernsey"
                    },
                    new Pais
                    {
                        Nome = "Guiana"
                    },
                    new Pais
                    {
                        Nome = "Guiana Francesa"
                    },
                    new Pais
                    {
                        Nome = "Guiné"
                    },
                    new Pais
                    {
                        Nome = "Guiné Bissau"
                    },
                    new Pais
                    {
                        Nome = "Guiné Equatorial"
                    },
                    new Pais
                    {
                        Nome = "Haiti"
                    },
                    new Pais
                    {
                        Nome = "Holanda (Países baixos)"
                    },
                    new Pais
                    {
                        Nome = "Honduras"
                    },
                    new Pais
                    {
                        Nome = "Hong Kong"
                    },
                    new Pais
                    {
                        Nome = "Hungria"
                    },
                    new Pais
                    {
                        Nome = "Iêmen"
                    },
                    new Pais
                    {
                        Nome = "Ilha Bouvet"
                    },
                    new Pais
                    {
                        Nome = "Ilha da Madeira"
                    },
                    new Pais
                    {
                        Nome = "Ilha de Clipperton"
                    },
                    new Pais
                    {
                        Nome = "Ilha de Man"
                    },
                    new Pais
                    {
                        Nome = "Ilha de Navassa"
                    },
                    new Pais
                    {
                        Nome = "Ilha do Natal"
                    },
                    new Pais
                    {
                        Nome = "Ilha Jan Mayen"
                    },
                    new Pais
                    {
                        Nome = "Ilha Norfolk"
                    },
                    new Pais
                    {
                        Nome = "Ilha Wake"
                    },
                    new Pais
                    {
                        Nome = "Ilhas Ashmore e Cartier"
                    },
                    new Pais
                    {
                        Nome = "Ilhas Caimão"
                    },
                    new Pais
                    {
                        Nome = "Ilhas Cocos"
                    },
                    new Pais
                    {
                        Nome = "Ilhas Cook"
                    },
                    new Pais
                    {
                        Nome = "Ilhas do mar de coral"
                    },
                    new Pais
                    {
                        Nome = "Ilhas Falkland"
                    },
                    new Pais
                    {
                        Nome = "Ilhas Heard e McDonald"
                    },
                    new Pais
                    {
                        Nome = "Ilhas Marshall"
                    },
                    new Pais
                    {
                        Nome = "Ilhas Paracel"
                    },
                    new Pais
                    {
                        Nome = "Ilhas Pitcairn"
                    },
                    new Pais
                    {
                        Nome = "Ilhas Salomão"
                    },
                    new Pais
                    {
                        Nome = "Ilhas Spratly"
                    },
                    new Pais
                    {
                        Nome = "Ilhas Turcas e Caicos"
                    },
                    new Pais
                    {
                        Nome = "Ilhas Virgens Americanas"
                    },
                    new Pais
                    {
                        Nome = "Ilhas Virgens Britânicas"
                    },
                    new Pais
                    {
                        Nome = "Índia"
                    },
                    new Pais
                    {
                        Nome = "Indonésia"
                    },
                    new Pais
                    {
                        Nome = "Inglaterra"
                    },
                    new Pais
                    {
                        Nome = "Irã"
                    },
                    new Pais
                    {
                        Nome = "Iraque"
                    },
                    new Pais
                    {
                        Nome = "Irlanda"
                    },
                    new Pais
                    {
                        Nome = "Irlanda do norte"
                    },
                    new Pais
                    {
                        Nome = "Islândia"
                    },
                    new Pais
                    {
                        Nome = "Israel"
                    },
                    new Pais
                    {
                        Nome = "Itália"
                    },
                    new Pais
                    {
                        Nome = "Jamaica"
                    },
                    new Pais
                    {
                        Nome = "Japão"
                    },
                    new Pais
                    {
                        Nome = "Jersey"
                    },
                    new Pais
                    {
                        Nome = "Jordânia"
                    },
                    new Pais
                    {
                        Nome = "Kosovo"
                    },
                    new Pais
                    {
                        Nome = "Kuwait"
                    },
                    new Pais
                    {
                        Nome = "Laos"
                    },
                    new Pais
                    {
                        Nome = "Lesoto"
                    },
                    new Pais
                    {
                        Nome = "Letônia"
                    },
                    new Pais
                    {
                        Nome = "Líbano"
                    },
                    new Pais
                    {
                        Nome = "Libéria"
                    },
                    new Pais
                    {
                        Nome = "Líbia"
                    },
                    new Pais
                    {
                        Nome = "Liechtenstein"
                    },
                    new Pais
                    {
                        Nome = "Lituânia"
                    },
                    new Pais
                    {
                        Nome = "Luxemburgo"
                    },
                    new Pais
                    {
                        Nome = "Macau"
                    },
                    new Pais
                    {
                        Nome = "Macedônia"
                    },
                    new Pais
                    {
                        Nome = "Madagascar"
                    },
                    new Pais
                    {
                        Nome = "Malásia"
                    },
                    new Pais
                    {
                        Nome = "Malawi"
                    },
                    new Pais
                    {
                        Nome = "Maldivas"
                    },
                    new Pais
                    {
                        Nome = "Mali"
                    },
                    new Pais
                    {
                        Nome = "Malta"
                    },
                    new Pais
                    {
                        Nome = "Marianas do Norte"
                    },
                    new Pais
                    {
                        Nome = "Marrocos"
                    },
                    new Pais
                    {
                        Nome = "Martinica"
                    },
                    new Pais
                    {
                        Nome = "Maurício"
                    },
                    new Pais
                    {
                        Nome = "Mauritânia"
                    },
                    new Pais
                    {
                        Nome = "Mayotte"
                    },
                    new Pais
                    {
                        Nome = "México"
                    },
                    new Pais
                    {
                        Nome = "Micronésia"
                    },
                    new Pais
                    {
                        Nome = "Moçambique"
                    },
                    new Pais
                    {
                        Nome = "Moldávia"
                    },
                    new Pais
                    {
                        Nome = "Mônaco"
                    },
                    new Pais
                    {
                        Nome = "Mongólia"
                    },
                    new Pais
                    {
                        Nome = "Monserrate"
                    },
                    new Pais
                    {
                        Nome = "Montenegro"
                    },
                    new Pais
                    {
                        Nome = "Myanmar"
                    },
                    new Pais
                    {
                        Nome = "Namíbia"
                    },
                    new Pais
                    {
                        Nome = "Nauru"
                    },
                    new Pais
                    {
                        Nome = "Nepal"
                    },
                    new Pais
                    {
                        Nome = "Nicarágua"
                    },
                    new Pais
                    {
                        Nome = "Níger"
                    },
                    new Pais
                    {
                        Nome = "Nigéria"
                    },
                    new Pais
                    {
                        Nome = "Niue"
                    },
                    new Pais
                    {
                        Nome = "Noruega"
                    },
                    new Pais
                    {
                        Nome = "Nova Caledônia"
                    },
                    new Pais
                    {
                        Nome = "Nova Zelândia"
                    },
                    new Pais
                    {
                        Nome = "Omã"
                    },
                    new Pais
                    {
                        Nome = "País de Gales"
                    },
                    new Pais
                    {
                        Nome = "Palau"
                    },
                    new Pais
                    {
                        Nome = "Palestina"
                    },
                    new Pais
                    {
                        Nome = "Panamá"
                    },
                    new Pais
                    {
                        Nome = "Papua-Nova Guiné"
                    },
                    new Pais
                    {
                        Nome = "Paquistão"
                    },
                    new Pais
                    {
                        Nome = "Paraguai"
                    },
                    new Pais
                    {
                        Nome = "Peru"
                    },
                    new Pais
                    {
                        Nome = "Polinésia Francesa"
                    },
                    new Pais
                    {
                        Nome = "Polônia"
                    },
                    new Pais
                    {
                        Nome = "Porto Rico"
                    },
                    new Pais
                    {
                        Nome = "Portugal"
                    },
                    new Pais
                    {
                        Nome = "Quênia"
                    },
                    new Pais
                    {
                        Nome = "Quirguizistão"
                    },
                    new Pais
                    {
                        Nome = "Quiribati"
                    },
                    new Pais
                    {
                        Nome = "Reino Unido"
                    },
                    new Pais
                    {
                        Nome = "República Centro-Africana"
                    },
                    new Pais
                    {
                        Nome = "República Checa"
                    },
                    new Pais
                    {
                        Nome = "República Democrática do Congo"
                    },
                    new Pais
                    {
                        Nome = "República do Congo"
                    },
                    new Pais
                    {
                        Nome = "República Dominicana"
                    },
                    new Pais
                    {
                        Nome = "Romênia"
                    },
                    new Pais
                    {
                        Nome = "Ruanda"
                    },
                    new Pais
                    {
                        Nome = "Rússia"
                    },
                    new Pais
                    {
                        Nome = "Saara Ocidental"
                    },
                    new Pais
                    {
                        Nome = "Samoa"
                    },
                    new Pais
                    {
                        Nome = "Samoa Americana"
                    },
                    new Pais
                    {
                        Nome = "Santa Helena"
                    },
                    new Pais
                    {
                        Nome = "Santa Lúcia"
                    },
                    new Pais
                    {
                        Nome = "São Cristóvão e Neves"
                    },
                    new Pais
                    {
                        Nome = "São Marinho"
                    },
                    new Pais
                    {
                        Nome = "São Pedro e Miquelon"
                    },
                    new Pais
                    {
                        Nome = "São Tomé e Príncipe"
                    },
                    new Pais
                    {
                        Nome = "São Vicente e Granadinas"
                    },
                    new Pais
                    {
                        Nome = "Seicheles"
                    },
                    new Pais
                    {
                        Nome = "Senegal"
                    },
                    new Pais
                    {
                        Nome = "Serra Leoa"
                    },
                    new Pais
                    {
                        Nome = "Sérvia"
                    },
                    new Pais
                    {
                        Nome = "Singapura"
                    },
                    new Pais
                    {
                        Nome = "Síria"
                    },
                    new Pais
                    {
                        Nome = "Somália"
                    },
                    new Pais
                    {
                        Nome = "Sri Lanka"
                    },
                    new Pais
                    {
                        Nome = "Suazilândia"
                    },
                    new Pais
                    {
                        Nome = "Sudão"
                    },
                    new Pais
                    {
                        Nome = "Sudão do Sul"
                    },
                    new Pais
                    {
                        Nome = "Suécia"
                    },
                    new Pais
                    {
                        Nome = "Suíça"
                    },
                    new Pais
                    {
                        Nome = "Suriname"
                    },
                    new Pais
                    {
                        Nome = "Tailândia"
                    },
                    new Pais
                    {
                        Nome = "Taiwan"
                    },
                    new Pais
                    {
                        Nome = "Tajiquistão"
                    },
                    new Pais
                    {
                        Nome = "Tanzânia"
                    },
                    new Pais
                    {
                        Nome = "Território Britânico do Oceano Índico"
                    },
                    new Pais
                    {
                        Nome = "Territórios Austrais Franceses"
                    },
                    new Pais
                    {
                        Nome = "Timor Leste"
                    },
                    new Pais
                    {
                        Nome = "Togo"
                    },
                    new Pais
                    {
                        Nome = "Tokelau"
                    },
                    new Pais
                    {
                        Nome = "Tonga"
                    },
                    new Pais
                    {
                        Nome = "Trindade e Tobago"
                    },
                    new Pais
                    {
                        Nome = "Tunísia"
                    },
                    new Pais
                    {
                        Nome = "Turquemenistão"
                    },
                    new Pais
                    {
                        Nome = "Turquia"
                    },
                    new Pais
                    {
                        Nome = "Tuvalu"
                    },
                    new Pais
                    {
                        Nome = "Ucrânia"
                    },
                    new Pais
                    {
                        Nome = "Uganda"
                    },
                    new Pais
                    {
                        Nome = "Uruguai"
                    },
                    new Pais
                    {
                        Nome = "Uzbequistão"
                    },
                    new Pais
                    {
                        Nome = "Vanuatu"
                    },
                    new Pais
                    {
                        Nome = "Vaticano"
                    },
                    new Pais
                    {
                        Nome = "Venezuela"
                    },
                    new Pais
                    {
                        Nome = "Vietnã"
                    },
                    new Pais
                    {
                        Nome = "Wallis e Futuna"
                    },
                    new Pais
                    {
                        Nome = "Zâmbia"
                    },
                    new Pais
                    {
                        Nome = "Zimbabué"
                    }
                };
                paises.ForEach(t => _context.CatalogPaises.Add(t));
                _context.SaveChanges();
            }

            #endregion

            if (_context.CatalogCategoriaContabilGestao.FirstOrDefault() == null)
            {
                var temas = new List<CatalogCategoriaContabilGestao>
                {
                    new CatalogCategoriaContabilGestao
                    {
                        Valor = "RH",
                        Nome = "Recursos Humanos",
                        Atividades = new List<CatalogAtividade>
                        {
                            new CatalogAtividade
                            {
                                Valor = "HH",
                                Nome =
                                    "Dedicação horária dos membros da equipe de gestão do Programa de P&D da Empresa, quadro efetivo."
                            }
                        }
                    },
                    new CatalogCategoriaContabilGestao
                    {
                        Valor = "ST",
                        Nome = "Serviços de Terceiros",
                        Atividades = new List<CatalogAtividade>
                        {
                            new CatalogAtividade
                            {
                                Valor = "FG",
                                Nome =
                                    "Desenvolvimento de ferramenta para gestão do Programa de P&D da Empresa, excluindose aquisição de equipamentos."
                            },
                            new CatalogAtividade
                            {
                                Valor = "PP",
                                Nome =
                                    "Prospecção tecnológica e demais atividades necessárias ao planejamento e à elaboração do plano estratégico de investimento em P&D."
                            }
                        }
                    },
                    new CatalogCategoriaContabilGestao
                    {
                        Valor = "MC",
                        Nome = "Materiais de Consumo",
                        Atividades = new List<CatalogAtividade>
                        {
                            new CatalogAtividade
                            {
                                Valor = "RP",
                                Nome = "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução."
                            }
                        }
                    },
                    new CatalogCategoriaContabilGestao
                    {
                        Valor = "VD",
                        Nome = "Viagens e Diárias",
                        Atividades = new List<CatalogAtividade>
                        {
                            new CatalogAtividade
                            {
                                Valor = "EC",
                                Nome =
                                    "Participação dos membros da equipe de gestão em eventos sobre pesquisa, desenvolvimento e inovação relacionados ao setor elétrico e/ou em cursos de gestão tecnológica e da informação."
                            },
                            new CatalogAtividade
                            {
                                Valor = "PP",
                                Nome =
                                    "Prospecção tecnológica e demais atividades necessárias ao planejamento e à elaboração do plano estratégico de investimento em P&D."
                            },
                            new CatalogAtividade
                            {
                                Valor = "RP",
                                Nome = "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução."
                            },
                            new CatalogAtividade
                            {
                                Valor = "AP",
                                Nome =
                                    "Participação dos responsáveis técnicos pelos projetos de P&D nas avaliações presenciais convocadas pela ANEEL."
                            }
                        }
                    },
                    new CatalogCategoriaContabilGestao
                    {
                        Valor = "OU",
                        Nome = "Outros",
                        Atividades = new List<CatalogAtividade>
                        {
                            new CatalogAtividade
                            {
                                Valor = "EC",
                                Nome =
                                    "Participação dos membros da equipe de gestão em eventos sobre pesquisa, desenvolvimento e inovação relacionados ao setor elétrico e/ou em cursos de gestão tecnológica e da informação."
                            },
                            new CatalogAtividade
                            {
                                Valor = "RP",
                                Nome = "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução."
                            },
                            new CatalogAtividade
                            {
                                Valor = "BA",
                                Nome = "Buscas de anterioridade no Instituto Nacional da Propriedade Industrial (INPI)."
                            }
                        }
                    },
                    new CatalogCategoriaContabilGestao
                    {
                        Valor = "CT",
                        Nome = "CITENEL",
                        Atividades = new List<CatalogAtividade>
                        {
                            new CatalogAtividade {Valor = "AC", Nome = "Apoio à realização do CITENEL."}
                        }
                    },
                    new CatalogCategoriaContabilGestao
                    {
                        Valor = "AC",
                        Nome = "Auditoria Contábil e Financeira",
                        Atividades = new List<CatalogAtividade>
                        {
                            new CatalogAtividade
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
                var fases = new List<CatalogProdutoFaseCadeia>
                {
                    new CatalogProdutoFaseCadeia
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
                    new CatalogProdutoFaseCadeia
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
                    new CatalogProdutoFaseCadeia
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
                    new CatalogProdutoFaseCadeia
                    {
                        Valor = "CS",
                        Nome = "Cabeça de série",
                        TiposDetalhados = new List<CatalogProdutoTipoDetalhado>
                        {
                            new CatalogProdutoTipoDetalhado
                                {Valor = "", Nome = "Aperfeiçoamento de protótipo obtido em projeto anterior"}
                        }
                    },
                    new CatalogProdutoFaseCadeia
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
                    new CatalogProdutoFaseCadeia
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