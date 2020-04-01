using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using APIGestor.Data;
using APIGestor.Models;
using APIGestor.Security;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using APIGestor.Authorizations;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Exceptions.Demandas;
using APIGestor.Services;
using APIGestor.Services.Demandas;
using APIGestor.Services.Relatorios;
using APIGestor.Services.Resultados;
using APIGestor.Services.Sistema;
using APIGestor.Services.XmlProjeto;
using AutoMapper;
using GlobalExceptionHandler.WebApi;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerUI;
using TaesaCore.Data;
using TaesaCore.Interfaces;
using TaesaCore.Services;

namespace APIGestor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IStartupFilter, IdentityInitializer>();
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

            services.AddScoped<IViewRenderService, ViewRenderService>();
            // Configurando o acesso a dados de projetos
            if (System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Stage")
            {
                var connectionString = System.Environment.GetEnvironmentVariable("ConnectionString");
                services.AddDbContext<GestorDbContext>(options =>
                    options.UseSqlServer(connectionString));
            }
            else
            {
                var connectionString = Configuration.GetConnectionString("BaseGestor");
                services.AddDbContext<GestorDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("BaseGestor")));
            }

            services.AddScoped<DbContext, GestorDbContext>();

            services.AddCors();
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2",
                    new OpenApiInfo()
                    {
                        Title = "Taesa - Gestor P&D",
                        Version = "2.2",
                        Description = "API REST criada com o ASP.NET Core 3.1 para comunição com o Gestor P&D",
                    });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();
            });

            #region Serviços

            services.AddScoped<CatalogService>();
            services.AddScoped<MailService>();
            services.AddScoped<UserService>();
            services.AddScoped<ProjetoService>();
            services.AddScoped<UserProjetoService>();
            services.AddScoped<TemaService>();
            services.AddScoped<EmpresaService>();
            services.AddScoped<RecursoHumanoService>();
            services.AddScoped<AlocacaoRhService>();
            services.AddScoped<RecursoMaterialService>();
            services.AddScoped<AlocacaoRmService>();
            services.AddScoped<ProdutoService>();
            services.AddScoped<EtapaService>();
            services.AddScoped<LogService>();
            services.AddScoped<UploadService>();
            services.AddScoped<RelatorioEmpresaService>();
            services.AddScoped<RelatorioEtapaService>();
            services.AddScoped<RelatorioAtividadeService>();

            // Gerador Xml Services
            services.AddScoped<GeradorXmlService>();
            services.AddScoped<XmlProjetoPedService>();
            services.AddScoped<XmlInteressePedService>();
            services.AddScoped<XmlInicioExecService>();
            services.AddScoped<XmlProrrogacaoService>();
            services.AddScoped<XmlRelatorioFinalService>();
            services.AddScoped<XmlRelatorioAuditoriaService>();
            services.AddScoped<XmlProjetoGestaoService>();
            services.AddScoped<XmlRelatorioFinalGestaoService>();
            services.AddScoped<XmlRelatorioAuditoriaGestaoService>();
            ////////////////////////

            services.AddScoped<RegistroFinanceiroService>();

            services.AddScoped<RelatorioFinalService>();
            services.AddScoped<ResultadoCapacitacaoService>();
            services.AddScoped<ResultadoProducaoService>();
            services.AddScoped<ResultadoInfraService>();
            services.AddScoped<ResultadoIntelectualService>();
            services.AddScoped<ResultadoSocioAmbientalService>();
            services.AddScoped<ResultadoEconomicoService>();

            // Projeto Gestão
            services.AddScoped<AtividadeGestaoService>();

            services.AddScoped<DemandaService>();
            services.AddScoped<DemandaLogService>();
            services.AddScoped<SistemaService>();
            services.AddScoped<MailerService>();

            #endregion

            // Ativando a utilização do ASP.NET Identity, a fim de
            // permitir a recuperação de seus objetos via injeção de
            // dependências
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/";
                })
                .AddEntityFrameworkStores<GestorDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<PortugueseIdentityErrorDescriber>();

            // Configurando a dependência para a classe de validação
            // de credenciais e geração de tokens
            services.AddScoped<AccessManager>();

            #region Genéricos

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IService<>), typeof(BaseService<>));

            #endregion


            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                    Configuration.GetSection("TokenConfigurations"))
                .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            services.AddSingleton<IAuthorizationHandler, ProjectAuthorizationHandler>();
            services.AddSingleton<IdentityInitializer>();

            // Aciona a extensão que irá configurar o uso de
            // autenticação e autorização via tokens
            services.AddJwtSecurity(
                signingConfigurations, tokenConfigurations);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            #region GlobalExceptionHandler

            //*
            app.UseGlobalExceptionHandler(configuration =>
            {
                configuration.ContentType = "application/json";
                configuration.ResponseBody((exception, httpContext) =>
                {
                    httpContext.Response.Headers["Access-Control-Allow-Origin"] = "*";
                    return JsonConvert.SerializeObject(new
                    {
                        exception.Message,
                        exception.Source,
                    });
                });
                configuration.Map<DemandaException>()
                    .ToStatusCode(HttpStatusCode.UnprocessableEntity);
            });
            // */

            #endregion


            // Define Cultura Padrão
            var cultureInfo = new CultureInfo("pt-BR");
            cultureInfo.NumberFormat.CurrencySymbol = "R$";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            // Criação de estruturas, usuários e permissões
            // na base do ASP.NET Identity Core (caso ainda não
            // existam)


            // Ativando middlewares para uso do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "API V2");
                c.DocExpansion(DocExpansion.None);
                c.ShowExtensions();
                c.EnableFilter();
                c.EnableDeepLinking();
                c.EnableValidator();
            });

            app.UseRouting();
            app.UseAuthorization();

            app.UseCors(builder =>
                    builder.AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                //.AllowCredentials()
            );
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseHttpsRedirection();
        }
    }
}