using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using PeD.Data;
using PeD.Models;
using PeD.Models.Demandas;
using PeD.Services.Sistema;

namespace PeD.Services.Demandas
{

    public class DemandaLogService
    {
        GestorDbContext context;
        IAuthorizationService authorization;
        IWebHostEnvironment hostingEnvironment;
        SistemaService sistemaService;
        public DemandaLogService(
            GestorDbContext context,
            IAuthorizationService authorization,
            IWebHostEnvironment hostingEnvironment,
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