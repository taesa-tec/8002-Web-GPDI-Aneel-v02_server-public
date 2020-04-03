using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGestor.Data;
using APIGestor.Models;
using APIGestor.Models.Catalogs;
using APIGestor.Models.Projetos;
using APIGestor.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APIGestor.Services
{
    public class IdentityInitializer : IStartupFilter
    {
        private readonly GestorDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private UserService _userService;
        private MailService _mailService;
        private CatalogService _catalogService;

        public IConfiguration Configuration { get; }

        public IdentityInitializer(IServiceProvider services, IConfiguration Configuration
        )
        {
            var scope = services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<GestorDbContext>();
            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _mailService = scope.ServiceProvider.GetRequiredService<MailService>();
            _userService = new UserService(_context, _userManager, _roleManager, _mailService,
                scope.ServiceProvider.GetService<AccessManager>());
            _catalogService = new CatalogService(_context);
            this.Configuration = Configuration;
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
            Console.WriteLine("Tentando criar as funções de usuário (Roles)");

            foreach (var role in Roles.AllRoles)
            {
                if (!await _roleManager.RoleExistsAsync(Roles.Fornecedor))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(Roles.Fornecedor));
                    if (!result.Succeeded)
                    {
                        result.Errors.ToList().ForEach(error => { Console.WriteLine(error.Description); });
                    }
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
                        Role = "Admin-APIGestor"
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
                var estados = new List<CatalogEstado>
                {
                    new CatalogEstado
                    {
                        Nome = "ACRE", Valor = "AC"
                    },
                    new CatalogEstado
                    {
                        Nome = "ALAGOAS", Valor = "AL"
                    },
                    new CatalogEstado
                    {
                        Nome = "AMAPÁ", Valor = "AP"
                    },
                    new CatalogEstado
                    {
                        Nome = "AMAZONAS", Valor = "AM"
                    },
                    new CatalogEstado
                    {
                        Nome = "BAHIA", Valor = "BA"
                    },
                    new CatalogEstado
                    {
                        Nome = "CEARÁ", Valor = "CE"
                    },
                    new CatalogEstado
                    {
                        Nome = "DISTRITO FEDERAL", Valor = "DF"
                    },
                    new CatalogEstado
                    {
                        Nome = "ESPÍRITO SANTO", Valor = "ES"
                    },
                    new CatalogEstado
                    {
                        Nome = "GOIÁS", Valor = "GO"
                    },
                    new CatalogEstado
                    {
                        Nome = "MARANHÃO", Valor = "MA"
                    },
                    new CatalogEstado
                    {
                        Nome = "MATO GROSSO", Valor = "MT"
                    },
                    new CatalogEstado
                    {
                        Nome = "MATO GROSSO DO SUL", Valor = "MS"
                    },
                    new CatalogEstado
                    {
                        Nome = "MINAS GERAIS", Valor = "MG"
                    },
                    new CatalogEstado
                    {
                        Nome = "PARÁ", Valor = "PA"
                    },
                    new CatalogEstado
                    {
                        Nome = "PARAÍBA", Valor = "PB"
                    },
                    new CatalogEstado
                    {
                        Nome = "PARANÁ", Valor = "PR"
                    },
                    new CatalogEstado
                    {
                        Nome = "PERNAMBUCO", Valor = "PE"
                    },
                    new CatalogEstado
                    {
                        Nome = "PIAUÍ", Valor = "PI"
                    },
                    new CatalogEstado
                    {
                        Nome = "RIO DE JANEIRO", Valor = "RJ"
                    },
                    new CatalogEstado
                    {
                        Nome = "RIO GRANDE DO NORTE", Valor = "RN"
                    },
                    new CatalogEstado
                    {
                        Nome = "RIO GRANDE DO SUL", Valor = "RS"
                    },
                    new CatalogEstado
                    {
                        Nome = "RONDONIA", Valor = "RO"
                    },
                    new CatalogEstado
                    {
                        Nome = "RORAIMA", Valor = "RR"
                    },
                    new CatalogEstado
                    {
                        Nome = "SANTA CATARINA", Valor = "SC"
                    },
                    new CatalogEstado
                    {
                        Nome = "SÃO PAULO", Valor = "SP"
                    },
                    new CatalogEstado
                    {
                        Nome = "SERGIPE", Valor = "SE"
                    },
                    new CatalogEstado
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
                var paises = new List<CatalogPais>
                {
                    new CatalogPais
                    {
                        Nome = "Açores"
                    },
                    new CatalogPais
                    {
                        Nome = "Acrotiri e Deceleia"
                    },
                    new CatalogPais
                    {
                        Nome = "Afeganistão"
                    },
                    new CatalogPais
                    {
                        Nome = "África do Sul"
                    },
                    new CatalogPais
                    {
                        Nome = "Albânia"
                    },
                    new CatalogPais
                    {
                        Nome = "Alemanha"
                    },
                    new CatalogPais
                    {
                        Nome = "Andorra"
                    },
                    new CatalogPais
                    {
                        Nome = "Angola"
                    },
                    new CatalogPais
                    {
                        Nome = "Anguila"
                    },
                    new CatalogPais
                    {
                        Nome = "Antártida"
                    },
                    new CatalogPais
                    {
                        Nome = "Antígua e Barbuda"
                    },
                    new CatalogPais
                    {
                        Nome = "Antilhas Holandesas"
                    },
                    new CatalogPais
                    {
                        Nome = "Arábia Saudita"
                    },
                    new CatalogPais
                    {
                        Nome = "Argélia"
                    },
                    new CatalogPais
                    {
                        Nome = "Argentina"
                    },
                    new CatalogPais
                    {
                        Nome = "Armênia"
                    },
                    new CatalogPais
                    {
                        Nome = "Aruba"
                    },
                    new CatalogPais
                    {
                        Nome = "Austrália"
                    },
                    new CatalogPais
                    {
                        Nome = "Áustria"
                    },
                    new CatalogPais
                    {
                        Nome = "Azerbaijão"
                    },
                    new CatalogPais
                    {
                        Nome = "Bahamas"
                    },
                    new CatalogPais
                    {
                        Nome = "Bangladesh"
                    },
                    new CatalogPais
                    {
                        Nome = "Barbados"
                    },
                    new CatalogPais
                    {
                        Nome = "Barém"
                    },
                    new CatalogPais
                    {
                        Nome = "Bélgica"
                    },
                    new CatalogPais
                    {
                        Nome = "Belize"
                    },
                    new CatalogPais
                    {
                        Nome = "Benim"
                    },
                    new CatalogPais
                    {
                        Nome = "Bermudas"
                    },
                    new CatalogPais
                    {
                        Nome = "Bielorrússia"
                    },
                    new CatalogPais
                    {
                        Nome = "Bolívia"
                    },
                    new CatalogPais
                    {
                        Nome = "Bósnia e Herzegovina"
                    },
                    new CatalogPais
                    {
                        Nome = "Botsuana"
                    },
                    new CatalogPais
                    {
                        Nome = "Brasil"
                    },
                    new CatalogPais
                    {
                        Nome = "Brunei"
                    },
                    new CatalogPais
                    {
                        Nome = "Bulgária"
                    },
                    new CatalogPais
                    {
                        Nome = "Burquina Faso"
                    },
                    new CatalogPais
                    {
                        Nome = "Burundi"
                    },
                    new CatalogPais
                    {
                        Nome = "Butão"
                    },
                    new CatalogPais
                    {
                        Nome = "Cabo Verde"
                    },
                    new CatalogPais
                    {
                        Nome = "Camarões"
                    },
                    new CatalogPais
                    {
                        Nome = "Camboja"
                    },
                    new CatalogPais
                    {
                        Nome = "Canadá"
                    },
                    new CatalogPais
                    {
                        Nome = "Canárias"
                    },
                    new CatalogPais
                    {
                        Nome = "Catar"
                    },
                    new CatalogPais
                    {
                        Nome = "Cazaquistão"
                    },
                    new CatalogPais
                    {
                        Nome = "Chade"
                    },
                    new CatalogPais
                    {
                        Nome = "Chile"
                    },
                    new CatalogPais
                    {
                        Nome = "China"
                    },
                    new CatalogPais
                    {
                        Nome = "Chipre"
                    },
                    new CatalogPais
                    {
                        Nome = "Colômbia"
                    },
                    new CatalogPais
                    {
                        Nome = "Comores"
                    },
                    new CatalogPais
                    {
                        Nome = "Coreia do Norte"
                    },
                    new CatalogPais
                    {
                        Nome = "Coreia do Sul"
                    },
                    new CatalogPais
                    {
                        Nome = "Costa do Marfim"
                    },
                    new CatalogPais
                    {
                        Nome = "Costa Rica"
                    },
                    new CatalogPais
                    {
                        Nome = "Croácia"
                    },
                    new CatalogPais
                    {
                        Nome = "Cuba"
                    },
                    new CatalogPais
                    {
                        Nome = "Curaçao"
                    },
                    new CatalogPais
                    {
                        Nome = "Dinamarca"
                    },
                    new CatalogPais
                    {
                        Nome = "Djibuti"
                    },
                    new CatalogPais
                    {
                        Nome = "Domínica"
                    },
                    new CatalogPais
                    {
                        Nome = "Egito"
                    },
                    new CatalogPais
                    {
                        Nome = "El Salvador"
                    },
                    new CatalogPais
                    {
                        Nome = "Emirados Árabes Unidos"
                    },
                    new CatalogPais
                    {
                        Nome = "Equador"
                    },
                    new CatalogPais
                    {
                        Nome = "Eritreia"
                    },
                    new CatalogPais
                    {
                        Nome = "Escócia"
                    },
                    new CatalogPais
                    {
                        Nome = "Eslováquia"
                    },
                    new CatalogPais
                    {
                        Nome = "Eslovênia"
                    },
                    new CatalogPais
                    {
                        Nome = "Espanha"
                    },
                    new CatalogPais
                    {
                        Nome = "Estados Unidos"
                    },
                    new CatalogPais
                    {
                        Nome = "Estônia"
                    },
                    new CatalogPais
                    {
                        Nome = "Etiópia"
                    },
                    new CatalogPais
                    {
                        Nome = "Faroé"
                    },
                    new CatalogPais
                    {
                        Nome = "Fiji"
                    },
                    new CatalogPais
                    {
                        Nome = "Filipinas"
                    },
                    new CatalogPais
                    {
                        Nome = "Finlândia"
                    },
                    new CatalogPais
                    {
                        Nome = "França"
                    },
                    new CatalogPais
                    {
                        Nome = "Gabão"
                    },
                    new CatalogPais
                    {
                        Nome = "Gâmbia"
                    },
                    new CatalogPais
                    {
                        Nome = "Gana"
                    },
                    new CatalogPais
                    {
                        Nome = "Geórgia"
                    },
                    new CatalogPais
                    {
                        Nome = "Geórgia do Sul e Sandwich do Sul"
                    },
                    new CatalogPais
                    {
                        Nome = "Gibraltar"
                    },
                    new CatalogPais
                    {
                        Nome = "Granada"
                    },
                    new CatalogPais
                    {
                        Nome = "Grécia"
                    },
                    new CatalogPais
                    {
                        Nome = "Groenlândia"
                    },
                    new CatalogPais
                    {
                        Nome = "Guadalupe"
                    },
                    new CatalogPais
                    {
                        Nome = "Guam"
                    },
                    new CatalogPais
                    {
                        Nome = "Guatemala"
                    },
                    new CatalogPais
                    {
                        Nome = "Guernsey"
                    },
                    new CatalogPais
                    {
                        Nome = "Guiana"
                    },
                    new CatalogPais
                    {
                        Nome = "Guiana Francesa"
                    },
                    new CatalogPais
                    {
                        Nome = "Guiné"
                    },
                    new CatalogPais
                    {
                        Nome = "Guiné Bissau"
                    },
                    new CatalogPais
                    {
                        Nome = "Guiné Equatorial"
                    },
                    new CatalogPais
                    {
                        Nome = "Haiti"
                    },
                    new CatalogPais
                    {
                        Nome = "Holanda (Países baixos)"
                    },
                    new CatalogPais
                    {
                        Nome = "Honduras"
                    },
                    new CatalogPais
                    {
                        Nome = "Hong Kong"
                    },
                    new CatalogPais
                    {
                        Nome = "Hungria"
                    },
                    new CatalogPais
                    {
                        Nome = "Iêmen"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilha Bouvet"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilha da Madeira"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilha de Clipperton"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilha de Man"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilha de Navassa"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilha do Natal"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilha Jan Mayen"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilha Norfolk"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilha Wake"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilhas Ashmore e Cartier"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilhas Caimão"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilhas Cocos"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilhas Cook"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilhas do mar de coral"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilhas Falkland"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilhas Heard e McDonald"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilhas Marshall"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilhas Paracel"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilhas Pitcairn"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilhas Salomão"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilhas Spratly"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilhas Turcas e Caicos"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilhas Virgens Americanas"
                    },
                    new CatalogPais
                    {
                        Nome = "Ilhas Virgens Britânicas"
                    },
                    new CatalogPais
                    {
                        Nome = "Índia"
                    },
                    new CatalogPais
                    {
                        Nome = "Indonésia"
                    },
                    new CatalogPais
                    {
                        Nome = "Inglaterra"
                    },
                    new CatalogPais
                    {
                        Nome = "Irã"
                    },
                    new CatalogPais
                    {
                        Nome = "Iraque"
                    },
                    new CatalogPais
                    {
                        Nome = "Irlanda"
                    },
                    new CatalogPais
                    {
                        Nome = "Irlanda do norte"
                    },
                    new CatalogPais
                    {
                        Nome = "Islândia"
                    },
                    new CatalogPais
                    {
                        Nome = "Israel"
                    },
                    new CatalogPais
                    {
                        Nome = "Itália"
                    },
                    new CatalogPais
                    {
                        Nome = "Jamaica"
                    },
                    new CatalogPais
                    {
                        Nome = "Japão"
                    },
                    new CatalogPais
                    {
                        Nome = "Jersey"
                    },
                    new CatalogPais
                    {
                        Nome = "Jordânia"
                    },
                    new CatalogPais
                    {
                        Nome = "Kosovo"
                    },
                    new CatalogPais
                    {
                        Nome = "Kuwait"
                    },
                    new CatalogPais
                    {
                        Nome = "Laos"
                    },
                    new CatalogPais
                    {
                        Nome = "Lesoto"
                    },
                    new CatalogPais
                    {
                        Nome = "Letônia"
                    },
                    new CatalogPais
                    {
                        Nome = "Líbano"
                    },
                    new CatalogPais
                    {
                        Nome = "Libéria"
                    },
                    new CatalogPais
                    {
                        Nome = "Líbia"
                    },
                    new CatalogPais
                    {
                        Nome = "Liechtenstein"
                    },
                    new CatalogPais
                    {
                        Nome = "Lituânia"
                    },
                    new CatalogPais
                    {
                        Nome = "Luxemburgo"
                    },
                    new CatalogPais
                    {
                        Nome = "Macau"
                    },
                    new CatalogPais
                    {
                        Nome = "Macedônia"
                    },
                    new CatalogPais
                    {
                        Nome = "Madagascar"
                    },
                    new CatalogPais
                    {
                        Nome = "Malásia"
                    },
                    new CatalogPais
                    {
                        Nome = "Malawi"
                    },
                    new CatalogPais
                    {
                        Nome = "Maldivas"
                    },
                    new CatalogPais
                    {
                        Nome = "Mali"
                    },
                    new CatalogPais
                    {
                        Nome = "Malta"
                    },
                    new CatalogPais
                    {
                        Nome = "Marianas do Norte"
                    },
                    new CatalogPais
                    {
                        Nome = "Marrocos"
                    },
                    new CatalogPais
                    {
                        Nome = "Martinica"
                    },
                    new CatalogPais
                    {
                        Nome = "Maurício"
                    },
                    new CatalogPais
                    {
                        Nome = "Mauritânia"
                    },
                    new CatalogPais
                    {
                        Nome = "Mayotte"
                    },
                    new CatalogPais
                    {
                        Nome = "México"
                    },
                    new CatalogPais
                    {
                        Nome = "Micronésia"
                    },
                    new CatalogPais
                    {
                        Nome = "Moçambique"
                    },
                    new CatalogPais
                    {
                        Nome = "Moldávia"
                    },
                    new CatalogPais
                    {
                        Nome = "Mônaco"
                    },
                    new CatalogPais
                    {
                        Nome = "Mongólia"
                    },
                    new CatalogPais
                    {
                        Nome = "Monserrate"
                    },
                    new CatalogPais
                    {
                        Nome = "Montenegro"
                    },
                    new CatalogPais
                    {
                        Nome = "Myanmar"
                    },
                    new CatalogPais
                    {
                        Nome = "Namíbia"
                    },
                    new CatalogPais
                    {
                        Nome = "Nauru"
                    },
                    new CatalogPais
                    {
                        Nome = "Nepal"
                    },
                    new CatalogPais
                    {
                        Nome = "Nicarágua"
                    },
                    new CatalogPais
                    {
                        Nome = "Níger"
                    },
                    new CatalogPais
                    {
                        Nome = "Nigéria"
                    },
                    new CatalogPais
                    {
                        Nome = "Niue"
                    },
                    new CatalogPais
                    {
                        Nome = "Noruega"
                    },
                    new CatalogPais
                    {
                        Nome = "Nova Caledônia"
                    },
                    new CatalogPais
                    {
                        Nome = "Nova Zelândia"
                    },
                    new CatalogPais
                    {
                        Nome = "Omã"
                    },
                    new CatalogPais
                    {
                        Nome = "País de Gales"
                    },
                    new CatalogPais
                    {
                        Nome = "Palau"
                    },
                    new CatalogPais
                    {
                        Nome = "Palestina"
                    },
                    new CatalogPais
                    {
                        Nome = "Panamá"
                    },
                    new CatalogPais
                    {
                        Nome = "Papua-Nova Guiné"
                    },
                    new CatalogPais
                    {
                        Nome = "Paquistão"
                    },
                    new CatalogPais
                    {
                        Nome = "Paraguai"
                    },
                    new CatalogPais
                    {
                        Nome = "Peru"
                    },
                    new CatalogPais
                    {
                        Nome = "Polinésia Francesa"
                    },
                    new CatalogPais
                    {
                        Nome = "Polônia"
                    },
                    new CatalogPais
                    {
                        Nome = "Porto Rico"
                    },
                    new CatalogPais
                    {
                        Nome = "Portugal"
                    },
                    new CatalogPais
                    {
                        Nome = "Quênia"
                    },
                    new CatalogPais
                    {
                        Nome = "Quirguizistão"
                    },
                    new CatalogPais
                    {
                        Nome = "Quiribati"
                    },
                    new CatalogPais
                    {
                        Nome = "Reino Unido"
                    },
                    new CatalogPais
                    {
                        Nome = "República Centro-Africana"
                    },
                    new CatalogPais
                    {
                        Nome = "República Checa"
                    },
                    new CatalogPais
                    {
                        Nome = "República Democrática do Congo"
                    },
                    new CatalogPais
                    {
                        Nome = "República do Congo"
                    },
                    new CatalogPais
                    {
                        Nome = "República Dominicana"
                    },
                    new CatalogPais
                    {
                        Nome = "Romênia"
                    },
                    new CatalogPais
                    {
                        Nome = "Ruanda"
                    },
                    new CatalogPais
                    {
                        Nome = "Rússia"
                    },
                    new CatalogPais
                    {
                        Nome = "Saara Ocidental"
                    },
                    new CatalogPais
                    {
                        Nome = "Samoa"
                    },
                    new CatalogPais
                    {
                        Nome = "Samoa Americana"
                    },
                    new CatalogPais
                    {
                        Nome = "Santa Helena"
                    },
                    new CatalogPais
                    {
                        Nome = "Santa Lúcia"
                    },
                    new CatalogPais
                    {
                        Nome = "São Cristóvão e Neves"
                    },
                    new CatalogPais
                    {
                        Nome = "São Marinho"
                    },
                    new CatalogPais
                    {
                        Nome = "São Pedro e Miquelon"
                    },
                    new CatalogPais
                    {
                        Nome = "São Tomé e Príncipe"
                    },
                    new CatalogPais
                    {
                        Nome = "São Vicente e Granadinas"
                    },
                    new CatalogPais
                    {
                        Nome = "Seicheles"
                    },
                    new CatalogPais
                    {
                        Nome = "Senegal"
                    },
                    new CatalogPais
                    {
                        Nome = "Serra Leoa"
                    },
                    new CatalogPais
                    {
                        Nome = "Sérvia"
                    },
                    new CatalogPais
                    {
                        Nome = "Singapura"
                    },
                    new CatalogPais
                    {
                        Nome = "Síria"
                    },
                    new CatalogPais
                    {
                        Nome = "Somália"
                    },
                    new CatalogPais
                    {
                        Nome = "Sri Lanka"
                    },
                    new CatalogPais
                    {
                        Nome = "Suazilândia"
                    },
                    new CatalogPais
                    {
                        Nome = "Sudão"
                    },
                    new CatalogPais
                    {
                        Nome = "Sudão do Sul"
                    },
                    new CatalogPais
                    {
                        Nome = "Suécia"
                    },
                    new CatalogPais
                    {
                        Nome = "Suíça"
                    },
                    new CatalogPais
                    {
                        Nome = "Suriname"
                    },
                    new CatalogPais
                    {
                        Nome = "Tailândia"
                    },
                    new CatalogPais
                    {
                        Nome = "Taiwan"
                    },
                    new CatalogPais
                    {
                        Nome = "Tajiquistão"
                    },
                    new CatalogPais
                    {
                        Nome = "Tanzânia"
                    },
                    new CatalogPais
                    {
                        Nome = "Território Britânico do Oceano Índico"
                    },
                    new CatalogPais
                    {
                        Nome = "Territórios Austrais Franceses"
                    },
                    new CatalogPais
                    {
                        Nome = "Timor Leste"
                    },
                    new CatalogPais
                    {
                        Nome = "Togo"
                    },
                    new CatalogPais
                    {
                        Nome = "Tokelau"
                    },
                    new CatalogPais
                    {
                        Nome = "Tonga"
                    },
                    new CatalogPais
                    {
                        Nome = "Trindade e Tobago"
                    },
                    new CatalogPais
                    {
                        Nome = "Tunísia"
                    },
                    new CatalogPais
                    {
                        Nome = "Turquemenistão"
                    },
                    new CatalogPais
                    {
                        Nome = "Turquia"
                    },
                    new CatalogPais
                    {
                        Nome = "Tuvalu"
                    },
                    new CatalogPais
                    {
                        Nome = "Ucrânia"
                    },
                    new CatalogPais
                    {
                        Nome = "Uganda"
                    },
                    new CatalogPais
                    {
                        Nome = "Uruguai"
                    },
                    new CatalogPais
                    {
                        Nome = "Uzbequistão"
                    },
                    new CatalogPais
                    {
                        Nome = "Vanuatu"
                    },
                    new CatalogPais
                    {
                        Nome = "Vaticano"
                    },
                    new CatalogPais
                    {
                        Nome = "Venezuela"
                    },
                    new CatalogPais
                    {
                        Nome = "Vietnã"
                    },
                    new CatalogPais
                    {
                        Nome = "Wallis e Futuna"
                    },
                    new CatalogPais
                    {
                        Nome = "Zâmbia"
                    },
                    new CatalogPais
                    {
                        Nome = "Zimbabué"
                    }
                };
                paises.ForEach(t => _context.CatalogPaises.Add(t));
                _context.SaveChanges();
            }

            #endregion

            if (_context.CatalogTema.FirstOrDefault() == null)
            {
                var temas = new List<CatalogTema>
                {
                    new CatalogTema
                    {
                        Valor = "FA",
                        Nome = "Fontes alternativas de geração de energia elétrica",
                        SubTemas = new List<CatalogSubTema>
                        {
                            new CatalogSubTema
                            {
                                Valor = "FA01",
                                Nome =
                                    "Alternativas energéticas sustentáveis de atendimento a pequenos sistemas isolados."
                            },
                            new CatalogSubTema
                                {Valor = "FA02", Nome = "Geração de energia a partir de resíduos sólidos urbanos."},
                            new CatalogSubTema
                            {
                                Valor = "FA03",
                                Nome = "Novos materiais e equipamentos para geração de energia por fontes alternativas."
                            },
                            new CatalogSubTema
                            {
                                Valor = "FA04",
                                Nome = "Tecnologias para aproveitamento de novos combustíveis em plantas geradoras."
                            },
                            new CatalogSubTema {Valor = "FA0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new CatalogTema
                    {
                        Valor = "GT",
                        Nome = "Geração Termelétrica",
                        SubTemas = new List<CatalogSubTema>
                        {
                            new CatalogSubTema
                            {
                                Valor = "GT01",
                                Nome =
                                    "Avaliação de riscos e incertezas do fornecimento contínuo de gás natural para geração termelétrica."
                            },
                            new CatalogSubTema
                            {
                                Valor = "GT02",
                                Nome =
                                    "Novas técnicas para eficientização e diminuição da emissão de poluentes de usinas termelétricas a combustível derivado de petróleo."
                            },
                            new CatalogSubTema
                            {
                                Valor = "GT03",
                                Nome =
                                    "Otimização da geração de energia elétrica em plantas industriais: aumento de eficiência na cogeração."
                            },
                            new CatalogSubTema {Valor = "GT04", Nome = "Micro-sistemas de cogeração residenciais."},
                            new CatalogSubTema
                            {
                                Valor = "GT05", Nome = "Técnicas para captura e seqüestro de carbono de termelétricas."
                            },
                            new CatalogSubTema {Valor = "GT0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new CatalogTema
                    {
                        Valor = "GB",
                        Nome = "Gestão de Bacias e Reservatórios",
                        SubTemas = new List<CatalogSubTema>
                        {
                            new CatalogSubTema
                            {
                                Valor = "GB01",
                                Nome =
                                    "Emissões de gases de efeito estufa (GEE) em reservatórios de usinas hidrelétricas."
                            },
                            new CatalogSubTema
                            {
                                Valor = "GB02",
                                Nome =
                                    "Efeitos de mudanças climáticas globais no regime hidrológico de bacias hidrográficas."
                            },
                            new CatalogSubTema
                            {
                                Valor = "GB03",
                                Nome = "Integração e otimização do uso múltiplo de reservatórios hidrelétricos."
                            },
                            new CatalogSubTema
                            {
                                Valor = "GB04",
                                Nome = "Gestão sócio-patrimonial de reservatórios de usinas hidrelétricas."
                            },
                            new CatalogSubTema
                                {Valor = "GB05", Nome = "Gestão da segurança de barragens de usinas hidrelétricas."},
                            new CatalogSubTema
                            {
                                Valor = "GB06",
                                Nome = "Assoreamento de reservatórios formados por barragens de usinas hidrelétricas."
                            },
                            new CatalogSubTema {Valor = "GB0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new CatalogTema
                    {
                        Valor = "MA",
                        Nome = "Meio Ambiente",
                        SubTemas = new List<CatalogSubTema>
                        {
                            new CatalogSubTema
                            {
                                Valor = "MA01",
                                Nome = "Impactos e restrições socioambientais de sistemas de energia elétrica."
                            },
                            new CatalogSubTema
                            {
                                Valor = "MA02",
                                Nome =
                                    "Metodologias para mensuração econômico-financeira de externalidades em sistemas de energia elétrica."
                            },
                            new CatalogSubTema
                            {
                                Valor = "MA03",
                                Nome =
                                    "Estudos de toxicidade relacionados à deterioração da qualidade da água em reservatórios. "
                            },
                            new CatalogSubTema {Valor = "MA0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new CatalogTema
                    {
                        Valor = "SE",
                        Nome = "Segurança",
                        SubTemas = new List<CatalogSubTema>
                        {
                            new CatalogSubTema
                            {
                                Valor = "SE01",
                                Nome =
                                    "Identificação e mitigação dos impactos de campos eletromagnéticos em organismos vivos."
                            },
                            new CatalogSubTema
                                {Valor = "SE02", Nome = "Análise e mitigação de riscos de acidentes elétricos."},
                            new CatalogSubTema
                                {Valor = "SE03", Nome = "Novas tecnologias para equipamentos de proteção individual."},
                            new CatalogSubTema
                            {
                                Valor = "SE04",
                                Nome = "Novas tecnologias para inspeção e manutenção de sistemas elétricos."
                            },
                            new CatalogSubTema {Valor = "SE0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new CatalogTema
                    {
                        Valor = "EE",
                        Nome = "Eficiência Energética",
                        SubTemas = new List<CatalogSubTema>
                        {
                            new CatalogSubTema
                                {Valor = "EE01", Nome = "Novas tecnologias para melhoria da eficiência energética."},
                            new CatalogSubTema {Valor = "EE02", Nome = "Gerenciamento de carga pelo lado da demanda."},
                            new CatalogSubTema
                                {Valor = "EE03", Nome = "Definição de indicadores de eficiência energética."},
                            new CatalogSubTema
                            {
                                Valor = "EE04",
                                Nome = "Metodologias para avaliação de resultados de projetos de eficiência energética."
                            },
                            new CatalogSubTema {Valor = "EE0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new CatalogTema
                    {
                        Valor = "PL",
                        Nome = "Planejamento de Sistemas de Energia Elétrica",
                        SubTemas = new List<CatalogSubTema>
                        {
                            new CatalogSubTema
                                {Valor = "PL01", Nome = "Planejamento integrado da expansão de sistemas elétricos."},
                            new CatalogSubTema {Valor = "PL02", Nome = "Integração de centrais eólicas ao SIN."},
                            new CatalogSubTema
                                {Valor = "PL03", Nome = "Integração de geração distribuída a redes elétricas."},
                            new CatalogSubTema
                            {
                                Valor = "PL04",
                                Nome =
                                    "Metodologia de previsão de mercado para diferentes níveis temporais e estratégias de contratação."
                            },
                            new CatalogSubTema
                            {
                                Valor = "PL05",
                                Nome = "Modelos hidrodinâmicos aplicados em reservatórios de usinas hidrelétricas."
                            },
                            new CatalogSubTema
                            {
                                Valor = "PL06", Nome = "Materiais supercondutores para transmissão de energia elétrica."
                            },
                            new CatalogSubTema
                            {
                                Valor = "PL07",
                                Nome = "Tecnologias e sistemas de transmissão de energia em longas distâncias."
                            },
                            new CatalogSubTema {Valor = "PL0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new CatalogTema
                    {
                        Valor = "OP",
                        Nome = "Operação de Sistemas de Energia Elétrica",
                        SubTemas = new List<CatalogSubTema>
                        {
                            new CatalogSubTema
                            {
                                Valor = "OP01",
                                Nome =
                                    "Ferramentas de apoio à operação de sistemas elétricos de potência em tempo real."
                            },
                            new CatalogSubTema
                            {
                                Valor = "OP02",
                                Nome = "Critérios de gerenciamento de carga para diferentes níveis de hierarquia."
                            },
                            new CatalogSubTema
                            {
                                Valor = "OP03",
                                Nome = "Estruturas, funções e regras de operação dos mercados de serviços ancilares."
                            },
                            new CatalogSubTema
                            {
                                Valor = "OP04",
                                Nome = "Otimização estrutural e paramétrica da capacidade dos sistemas de distribuição."
                            },
                            new CatalogSubTema
                            {
                                Valor = "OP05",
                                Nome = "Alocação de fontes de potência reativa em sistemas de distribuição."
                            },
                            new CatalogSubTema
                            {
                                Valor = "OP06",
                                Nome = "Estudo, simulação e análise do desempenho de sistemas elétricos de potência."
                            },
                            new CatalogSubTema
                            {
                                Valor = "OP07",
                                Nome =
                                    "Análise das grandes perturbações e impactos no planejamento, operação e controle."
                            },
                            new CatalogSubTema
                            {
                                Valor = "OP08",
                                Nome = "Desenvolvimento de modelos para a otimização de despacho hidrotérmico."
                            },
                            new CatalogSubTema
                            {
                                Valor = "OP09",
                                Nome =
                                    "Desenvolvimento e/ou aprimoramento dos modelos de previsão de chuva versus vazão."
                            },
                            new CatalogSubTema
                            {
                                Valor = "OP10",
                                Nome = "Sistemas de monitoramento da operação de usinas não-despachadas pelo ONS."
                            },
                            new CatalogSubTema {Valor = "OP0X", Nome = "Outros."}
                        }
                    },
                    new CatalogTema
                    {
                        Valor = "SC",
                        Nome = "Supervisão, Controle e Proteção de Sistemas de Energia Elétrica",
                        SubTemas = new List<CatalogSubTema>
                        {
                            new CatalogSubTema
                            {
                                Valor = "SC01",
                                Nome = "Implementação de sistemas de controle (robusto, adaptativo e inteligente)."
                            },
                            new CatalogSubTema {Valor = "SC02", Nome = "Análise dinâmica de sistemas em tempo real."},
                            new CatalogSubTema
                            {
                                Valor = "SC03",
                                Nome = "Técnicas eficientes de restauração rápida de grandes centros de carga."
                            },
                            new CatalogSubTema
                            {
                                Valor = "SC04",
                                Nome = "Desenvolvimento de técnicas para recomposição de sistemas elétricos."
                            },
                            new CatalogSubTema
                            {
                                Valor = "SC05",
                                Nome =
                                    "Técnicas de inteligência artificial aplicadas ao controle, operação e proteção de sistemas elétricos."
                            },
                            new CatalogSubTema
                            {
                                Valor = "SC06",
                                Nome = "Novas tecnologias para supervisão do fornecimento de energia elétrica."
                            },
                            new CatalogSubTema
                                {Valor = "SC07", Nome = "Desenvolvimento e aplicação de sistemas de medição fasorial."},
                            new CatalogSubTema {Valor = "SC08", Nome = "Análise de falhas em sistemas elétricos."},
                            new CatalogSubTema
                                {Valor = "SC09", Nome = "Compatibilidade eletromagnética em sistemas elétricos."},
                            new CatalogSubTema {Valor = "SC10", Nome = "Sistemas de aterramento."},
                            new CatalogSubTema {Valor = "SC0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new CatalogTema
                    {
                        Valor = "QC",
                        Nome = "Qualidade e Confiabilidade dos Serviços de Energia Elétrica",
                        SubTemas = new List<CatalogSubTema>
                        {
                            new CatalogSubTema
                            {
                                Valor = "QC01",
                                Nome =
                                    "Sistemas e técnicas de monitoração e gerenciamento da qualidade da energia elétrica."
                            },
                            new CatalogSubTema
                            {
                                Valor = "QC02",
                                Nome = "Modelagem e análise dos distúrbios associados à qualidade da energia elétrica."
                            },
                            new CatalogSubTema
                            {
                                Valor = "QC03",
                                Nome =
                                    "Requisitos para conexão de cargas potencialmente perturbadoras no sistema elétrico."
                            },
                            new CatalogSubTema
                            {
                                Valor = "QC04", Nome = "Curvas de sensibilidade e de suportabilidade de equipamentos."
                            },
                            new CatalogSubTema
                            {
                                Valor = "QC05",
                                Nome = "Impactos econômicos e aspectos contratuais da qualidade da energia elétrica."
                            },
                            new CatalogSubTema
                            {
                                Valor = "QC06",
                                Nome = "Compensação financeira por violação de indicadores de qualidade."
                            },
                            new CatalogSubTema {Valor = "QC0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new CatalogTema
                    {
                        Valor = "MF",
                        Nome = "Medição, faturamento e combate a perdas comerciais",
                        SubTemas = new List<CatalogSubTema>
                        {
                            new CatalogSubTema
                            {
                                Valor = "MF01", Nome = "Avaliação econômica para definição da perda mínima atingível."
                            },
                            new CatalogSubTema
                            {
                                Valor = "MF02",
                                Nome = "Estimação, análise e redução de perdas técnicas em sistemas elétricos."
                            },
                            new CatalogSubTema
                            {
                                Valor = "MF03",
                                Nome =
                                    "Desenvolvimento de tecnologias para combate à fraude e ao furto de energia elétrica."
                            },
                            new CatalogSubTema
                            {
                                Valor = "MF04",
                                Nome =
                                    "Diagnóstico, prospecção e redução da vulnerabilidade de sistemas elétricos ao furto e à fraude."
                            },
                            new CatalogSubTema
                            {
                                Valor = "MF05",
                                Nome = "Energia economizada e agregada ao mercado após regularização de fraudes."
                            },
                            new CatalogSubTema
                            {
                                Valor = "MF06",
                                Nome = "Uso de indicadores socioeconômicos, dados fiscais e gastos com outros insumos."
                            },
                            new CatalogSubTema
                            {
                                Valor = "MF07",
                                Nome = "Gerenciamento dos equipamentos de medição (qualidade e redução de falhas)."
                            },
                            new CatalogSubTema
                            {
                                Valor = "MF08",
                                Nome = "Impacto dos projetos de eficiência energética na redução de perdas comerciais."
                            },
                            new CatalogSubTema
                            {
                                Valor = "MF09",
                                Nome =
                                    "Sistemas centralizados de medição, controle e gerenciamento de energia em consumidores finais."
                            },
                            new CatalogSubTema
                                {Valor = "MF10", Nome = "Sistemas de tarifação e novas estruturas tarifárias."},
                            new CatalogSubTema {Valor = "MF0X", Nome = "Outro.", Order = 1}
                        }
                    },
                    new CatalogTema
                    {
                        Valor = "OU",
                        Nome = "Outros",
                        SubTemas = new List<CatalogSubTema>
                        {
                            new CatalogSubTema {Valor = "OU  ", Nome = "Outros"}
                        },
                        Order = 1
                    }
                };

                temas.ForEach(t => _context.CatalogTema.Add(t));
                _context.SaveChanges();
            }

            if (_context.CatalogUserPermissoes.FirstOrDefault() == null)
            {
                var permissoes = new List<CatalogUserPermissao>
                {
                    new CatalogUserPermissao
                    {
                        Valor = "leitura", Nome = "Leitura"
                    },
                    new CatalogUserPermissao
                    {
                        Valor = "leituraEscrita", Nome = "Leitura e Escrita"
                    },
                    new CatalogUserPermissao
                    {
                        Valor = "aprovador", Nome = "Aprovador"
                    },
                    new CatalogUserPermissao
                    {
                        Valor = "admin", Nome = "Administrador"
                    }
                };
                permissoes.ForEach(s => _context.CatalogUserPermissoes.Add(s));
                _context.SaveChanges();
            }

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