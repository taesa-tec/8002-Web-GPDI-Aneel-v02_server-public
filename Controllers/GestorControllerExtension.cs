using APIGestor.Business;
using APIGestor.Models;
using APIGestor.Security;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Reflection;
using static APIGestor.Models.Log;

namespace APIGestor.Controllers {
    public static class GestorControllerExtension {

        public static string userId( this ControllerBase controller ) {
            return controller.User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
        }

        public static bool isAdmin( this ControllerBase controller ) {
            return controller.User.FindFirst(ClaimTypes.Role).Value == Roles.ROLE_ADMIN_GESTOR;
        }

        public static bool CreateLog( this ControllerBase controller, LogService service, int ProjetoId, object Entity, object oldEntity = null ) {

            if(ProjetoId == 0) {
                throw new Exception("Id de projeto inválida");
            }
            LogProjeto log = new LogProjeto();

            log.Tela = controller.Request.Headers.ContainsKey("Referer") ? controller.Request.Headers.First(header => header.Key == "Referer").Value.First() : controller.Url.ToString();
            log.ProjetoId = ProjetoId;
            log.UserId = controller.userId();

            var logInfo = LogItem.GerarItems(Entity, oldEntity);
            var oldInfo = LogItem.GerarItems(oldEntity, Entity);

            switch(controller.Request.Method) {
                case "POST":
                    log.Acao = Acoes.Create;
                    break;
                case "PUT":
                    log.Acao = Acoes.Update;
                    break;
                case "DELETE":
                    log.Acao = Acoes.Delete;
                    oldInfo = logInfo;
                    logInfo = new List<LogItem>();
                    break;
                default:
                    log.Acao = Acoes.Retrieve;
                    break;
            }


            log.StatusNovo = JsonConvert.SerializeObject(logInfo);
            log.StatusAnterior = oldInfo.Count > 0 ? JsonConvert.SerializeObject(oldInfo) : "";

            if(!(log.Acao == Acoes.Update && logInfo.Count == 0)) {
                var result = service.Incluir(log);
                return result.Sucesso;
            }
            return false;


        }
        public static bool CreateLog( this ControllerBase controller, BaseGestorService service, int ProjetoId, object Entity, object oldEntity = null ) {
            return CreateLog(controller, service.LogService, ProjetoId, Entity, oldEntity);
        }


    }
}
