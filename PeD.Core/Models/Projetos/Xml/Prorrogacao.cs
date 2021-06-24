using Newtonsoft.Json;

namespace PeD.Core.Models.Projetos.Xml
{
    public class Prorrogacao : BaseXml
    {
        public Prorrogacao(string codigo, int duracao)
        {
            PD_PrazoExecProjeto = new PrazoProjeto()
            {
                Projeto = new Projeto(codigo, duracao)
            };
        }

        [JsonIgnore] public override XmlTipo Tipo => XmlTipo.PRORROGAEXECUCAOPROJETO;

        public class Projeto
        {
            public string CodProjeto { get; set; }
            public int Duracao { get; set; }

            public Projeto(string codProjeto, int duracao)
            {
                CodProjeto = codProjeto;
                Duracao = duracao;
            }
        }

        public class PrazoProjeto
        {
            public Projeto Projeto { get; set; }
        }

        public PrazoProjeto PD_PrazoExecProjeto { get; set; }
    }
}