using System;
using Newtonsoft.Json;
using PeD.Core.Converters;

// ReSharper disable InconsistentNaming

namespace PeD.Core.Models.Projetos.Xml.InicioExecucao
{
    public class InicioExecucao : BaseXml
    {
        public InicioExecucao(string codProjeto, DateTime inicio, string compartilhamento)
        {
            PD_InicioExecProjeto = new PD_InicioExecProjeto
            {
                Projeto = new Projeto
                {
                    CodProjeto = codProjeto,
                    DataIniProjeto = inicio,
                    DirPropIntProjeto = compartilhamento
                }
            };
        }

        [JsonIgnore] public override XmlTipo Tipo => XmlTipo.INICIOEXECUCAOPROJETO;
        public PD_InicioExecProjeto PD_InicioExecProjeto { get; set; }
    }

    public class PD_InicioExecProjeto
    {
        public Projeto Projeto { get; set; }
    }

    public class Projeto
    {
        public string CodProjeto { get; set; }

        [JsonConverter(typeof(DateXmlConverter))]
        public DateTime DataIniProjeto { get; set; }

        public string DirPropIntProjeto { get; set; }
    }
}