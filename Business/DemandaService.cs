using System;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Models.Demandas;
using APIGestor.Data;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace APIGestor.Business
{

    public class DemandaService : BaseGestorService
    {
        public DemandaService(GestorDbContext context, IAuthorizationService authorization, LogService logService) : base(context, authorization, logService)
        {
        }

        public Demanda GetById(int id)
        {
            return _context.Demandas.FirstOrDefault(d => d.Id == id);
        }

        public bool DemandaExist(int id)
        {
            return _context.Demandas.Any(d => d.Id == id);
        }

        public List<Demanda> GetByEtapa(Etapa etapa)
        {
            return _context.Demandas.Where(d => d.EtapaAtual == etapa).ToList();
        }
        public List<Demanda> GetByEtapaStatus(EtapaStatus status)
        {
            return _context.Demandas.Where(d => d.EtapaStatus == status).ToList();
        }
        public List<Demanda> GetDemandasReprovadas()
        {
            return _context.Demandas.Where(d => d.EtapaStatus == EtapaStatus.Reprovada || d.EtapaStatus == EtapaStatus.ReprovadaPermanente).ToList();
        }
        public List<Demanda> GetDemandasAprovadas()
        {
            return _context.Demandas.Where(d => d.EtapaStatus == EtapaStatus.Aprovada && d.EtapaAtual == Etapa.AprovacaoDiretor).ToList();
        }
        public List<Demanda> GetDemandasEmElaboracao()
        {
            return _context.Demandas.Where(d => d.EtapaAtual == Etapa.Elaboracao || d.EtapaStatus == EtapaStatus.EmElaboracao).ToList();
        }
        public List<Demanda> GetDemandasCaptacao()
        {
            return _context.Demandas.Where(d => d.EtapaAtual == Etapa.Captacao).ToList();
        }
        public List<Demanda> GetDemandasPendentes()
        {
            return GetByEtapaStatus(EtapaStatus.Pendente);
        }
        public void EnviarCaptacao(int id)
        {

            var demanda = GetById(id);
            if (demanda != null)
            {
                demanda.EtapaAtual = Etapa.Captacao;
                _context.SaveChanges();
            }
        }
        public Demanda CriarDemanda(string titulo, string userId)
        {
            var demanda = new Demanda();
            demanda.Titulo = titulo;
            demanda.CriadorId = userId;
            demanda.EtapaAtual = Etapa.Elaboracao;
            demanda.EtapaStatus = EtapaStatus.EmElaboracao;
            _context.Demandas.Add(demanda);
            _context.SaveChanges();
            return demanda;
        }
        public Demanda AlterarStatusDemanda(int id, APIGestor.Models.Demandas.EtapaStatus status)
        {
            var demanda = GetById(id);
            if (demanda != null)
            {
                demanda.EtapaStatus = status;
                _context.SaveChanges();
            }
            return demanda;
        }
        public void AprovarDemanda(int id)
        {
            var demanda = GetById(id);
            if (demanda != null)
            {
                demanda.ProximaEtapa();
                demanda.EtapaStatus = EtapaStatus.EmElaboracao;
                _context.SaveChanges();
            }
            return;
        }
        public void ReprovarDemanda(int id, DemandaComentario comentario, bool permanente)
        {
            var demanda = GetById(id);
            if (demanda != null)
            {
                demanda.EtapaStatus = permanente ? EtapaStatus.ReprovadaPermanente : EtapaStatus.Reprovada;
                demanda.Comentarios = demanda.Comentarios != null ? demanda.Comentarios : new List<DemandaComentario>();
                demanda.Comentarios.Add(comentario);
                _context.SaveChanges();
            }
            return;
        }
        public object GetDemandaFormData(int id, string form)
        {
            var data = _context.DemandaFormValues.FirstOrDefault(df => df.DemandaId == id && df.FormKey == form);
            if (data != null)
            {
                return data.ToObject<object>();
            }
            return null;
        }
        public void SalvarDemandaFormData(int id, string form, object data)
        {
            var df_data = _context.DemandaFormValues.FirstOrDefault(df => df.DemandaId == id && df.FormKey == form);
            if (df_data != null)
            {
                df_data.SetValue(data);
            }
            else
            {
                df_data = new DemandaFormValues();
                df_data.DemandaId = id;
                df_data.FormKey = form;
                df_data.SetValue(data);
                _context.DemandaFormValues.Add(df_data);
            }
            _context.SaveChanges();
        }

    }
}