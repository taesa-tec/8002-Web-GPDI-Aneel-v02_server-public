using System;
using APIGestor.Data;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.IO;
using iText.Html2pdf;
using iText.Layout.Element;
using Microsoft.AspNetCore.Hosting;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Layout.Properties;
using APIGestor.Models.Demandas.Forms;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using APIGestor.Exceptions.Demandas;
using APIGestor.Business.Sistema;
using System.Linq;
using APIGestor.Models;
using System.Collections.Generic;
using APIGestor.Models.Demandas;
using Microsoft.AspNetCore.Mvc;
using APIGestor.Controllers;

namespace APIGestor.Business.Demandas
{

    public class DemandaLogService
    {
        GestorDbContext context;
        IAuthorizationService authorization;
        IHostingEnvironment hostingEnvironment;
        SistemaService sistemaService;
        public DemandaLogService(
            GestorDbContext context,
            IAuthorizationService authorization,
            IHostingEnvironment hostingEnvironment,
            SistemaService sistemaService
            )
        {
            this.context = context;
            this.authorization = authorization;
            this.hostingEnvironment = hostingEnvironment;
            this.sistemaService = sistemaService;
        }

        public IEnumerable<DemandaLog> ListarTodos(int demandaId, Acoes? acao, string user, int pag, int size)
        {
            var Logs = context.DemandaLogs
                .Include("User")
                .Where(d => d.DemandaId == demandaId);

            if (acao != null)
            {
                Logs = Logs
                    .Where(p => p.Acao == (Acoes)acao);
            }
            if (user != null)
            {
                Logs = Logs
                    .Where(p => p.UserId == user);
            }
            return Logs;
        }

        public DemandaLog Incluir(DemandaLog log)
        {
            log.Created = DateTime.Now;
            context.DemandaLogs.Add(log);
            context.SaveChanges();
            return log;
        }

        public DemandaLog Incluir(string userId, int DemandaId, string titulo, object valor)
        {
            return Incluir(userId, DemandaId, titulo, valor, null);
        }

        public DemandaLog Incluir(string userId, int DemandaId, string titulo, object valor, string type)
        {
            var logitem = type != null ? new LogItem(titulo, valor, type) : new LogItem(titulo, valor);
            var log = new DemandaLog()
            {
                DemandaId = DemandaId,
                UserId = userId,
                Data = new LogData()
                {
                    statusNovo = new List<LogItem>(){
                        logitem
                    }
                }
            };


            return Incluir(log);
        }
        public DemandaLog Incluir(string userId, int DemandaId, LogData data)
        {
            var log = new DemandaLog()
            {
                DemandaId = DemandaId,
                UserId = userId,
                Data = data
            };


            return Incluir(log);
        }

    }
}