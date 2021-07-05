using Newtonsoft.Json;
using PeD.Core.Converters;
// ReSharper disable InconsistentNaming

namespace PeD.Core.Models.Projetos.Xml.Interesse
{
    public class Interesse : BaseXml
    {
        [JsonIgnore] public override XmlTipo Tipo => XmlTipo.INTERESSEPROJETOPED;
        public PD_InteresseProjeto PD_InteresseProjeto { get; }

        public Interesse(string codProjeto, bool interesse)
        {
            PD_InteresseProjeto = new PD_InteresseProjeto()
            {
                Projeto = new Projeto()
                {
                    Interesse = interesse,
                    CodProjeto = codProjeto
                }
            };
        }
    }

    public class PD_InteresseProjeto
    {
        public Projeto Projeto { get; set; }
    }

    public class Projeto
    {
        public string CodProjeto { get; set; }

        [JsonConverter(typeof(YesOrNoConverter))]
        public bool Interesse { get; set; }
    }
}