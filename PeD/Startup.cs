﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Reflection;
using AutoMapper;
using FluentValidation.AspNetCore;
using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using PeD.Auth;
using PeD.Core;
using PeD.Core.Exceptions.Demandas;
using PeD.Core.Models;
using PeD.Data;
using PeD.Services;
using PeD.Services.Captacoes;
using PeD.Services.Demandas;
using PeD.Services.Sistema;
using Swashbuckle.AspNetCore.SwaggerUI;
using TaesaCore.Data;
using TaesaCore.Interfaces;
using TaesaCore.Services;
using CaptacaoService = PeD.Services.Captacoes.CaptacaoService;
using Path = System.IO.Path;


namespace PeD
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
            var spaPath = Configuration.GetValue<string>("SpaPath");
            services.AddTransient<IStartupFilter, IdentityInitializer>();
            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddControllers().AddNewtonsoftJson();

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
                services.AddDbContext<GestorDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("BaseGestor")));
            }

            services.AddScoped<DbContext, GestorDbContext>();

            services.AddCors();
            services.AddMvc()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining(typeof(Startup));
                    fv.RegisterValidatorsFromAssemblyContaining(typeof(ApplicationUser));
                });

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2",
                    new OpenApiInfo()
                    {
                        Title = "Taesa - Gestor P&D",
                        Version = "2.2",
                        Description = "API REST criada com o ASP.NET Core 3.1 para comunição com o Gestor P&D",
                    });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });


                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.EnableAnnotations();
            });

            #endregion

            #region Serviços

            services.AddScoped<SendGridService>();
            services.AddScoped<ArquivoService>();
            services.AddScoped<MailService>();
            services.AddScoped<UserService>();
            services.AddScoped<DemandaService>();
            services.AddScoped<DemandaLogService>();
            services.AddScoped<SistemaService>();
            services.AddScoped<MailerService>();

            services.AddScoped<CaptacaoService>();
            services.AddScoped<PropostaService>();
            services.AddScoped<CoExecutorService>();

            #endregion

            // Ativando a utilização do ASP.NET Identity, a fim de
            // permitir a recuperação de seus objetos via injeção de
            // dependências
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/";
                    options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Jti;
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


            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                    Configuration.GetSection("TokenConfigurations"))
                .Configure(tokenConfigurations);
            var signingConfigurations = new SigningConfigurations(tokenConfigurations.BaseHash);
            services.AddSingleton(signingConfigurations);
            services.AddSingleton(tokenConfigurations);
            // Aciona a extensão que irá configurar o uso de
            // autenticação e autorização via tokens
            services.AddJwtSecurity(signingConfigurations, tokenConfigurations);
            var sendgrid = Configuration.GetSection("SendGrid");
            var emailConfig = new EmailConfig()
            {
                ApiKey = sendgrid.GetValue<string>("ApiKey"),
                SenderEmail = sendgrid.GetValue<string>("SenderEmail"),
                SenderName = sendgrid.GetValue<string>("SenderName"),
            };
            services.AddSingleton(emailConfig);

            services.AddSingleton<IdentityInitializer>();

            services.AddSpaStaticFiles(opt =>
            {
                if (!string.IsNullOrWhiteSpace(spaPath) || Directory.Exists(spaPath))
                {
                    opt.RootPath = spaPath;
                }
                else
                {
                    opt.RootPath = "StaticFiles/DefaultSpa";
                }
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var storagePath = Configuration.GetValue<string>("StoragePath");
            if (!string.IsNullOrWhiteSpace(storagePath) && Directory.Exists(storagePath))
            {
                if (!Directory.Exists(Path.Combine(storagePath, "avatar")))
                {
                    Directory.CreateDirectory(Path.Combine(storagePath, "avatar"));
                }

                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(storagePath, "avatar")),
                    RequestPath = "/Avatar"
                });
            }

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapControllerRoute("Areas", "api/{controller=Home}/{action=Index}/{id?}");
            });
            // app.UseHttpsRedirection();


            app.UseSpaStaticFiles();

            app.UseSpa(spa => { });
        }
    }
}