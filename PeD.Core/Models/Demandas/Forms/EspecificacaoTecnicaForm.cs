namespace PeD.Core.Models.Demandas.Forms
{
    public class EspecificacaoTecnicaForm : FieldList
    {
        public const string TemaPath = "children.escopo-fornecimento.children.tema-aneel.value";
        public new const string Key = "especificacao-tecnica";

        public EspecificacaoTecnicaForm() : base(Key, "ESPECIFICAÇÃO TÉCNICA DEMANDA", Type.Form)
        {
            // 1
            AddRichText("apresentacao-objetivo", "APRESENTAÇÃO E OBJETIVO");

            //2
            var escopoFornecimento = AddFieldList("escopo-fornecimento", "ESCOPO DE FORNECIMENTO");
            //2.1
            escopoFornecimento.Add(new Field("tema-aneel", "Tema ANEEL", Type.Temas));
            //2.2
            escopoFornecimento.AddText("titulo-problema", "TÍTULO BREVE DO PROBLEMA");
            //2.3
            escopoFornecimento.AddRichText("descricao-problema", "DESCRIÇÃO COMPLETA DO PROBLEMA");
            //2.4
            var resultadosEsperados = escopoFornecimento.AddFieldList("resultados-esperados",
                "RESULTADOS ESPERADOS DE PESQUISA", Type.RichText);
            //2.4.x <
            resultadosEsperados.AddRichText("produto-principal", "PRODUTO PRINCIPAL");
            resultadosEsperados.AddRichTextList("produtos-complementares", "PRODUTOS COMPLEMENTARES", "").ItemTitle =
                "Produto Complementar";
            resultadosEsperados.AddRichText("produtos-academicos",
                "PRODUTOS ACADEMICOS - ARTIGOS, TESES E DISSERTAÇÕES");
            resultadosEsperados.AddRichText("produtos-outros", "OUTROS PRODUTOS");
            resultadosEsperados.AddRichText("conhecimentos-cientificos", "INTERNALIZAÇÃO DE CONHECIMENTO CIENTÍFICO");
            //2.4.x >

            //2.5 
            var fasesCadeiaInovacao =
                escopoFornecimento.AddFieldList("fases-cadeia-inovacao", "Fases da Cadeia de Inovação", Type.RichText);
            fasesCadeiaInovacao.AddRichText("de", "DESENVOLVIMENTO EXPERIMENTAL (DE)");
            fasesCadeiaInovacao.AddRichText("cs", "CABEÇA DE SÉRIE (CS)");
            fasesCadeiaInovacao.AddRichText("im", "INSERÇÃO NO MERCADO (IM)");

            //2.6
            escopoFornecimento.AddRichText("retorno-finaneceiro", "RETORNO FINANCEIRO ESPERADO (TEMPO E VALOR)");

            //2.7
            escopoFornecimento.AddRichText("projetos-similares", "PROJETO(S) E/OU PRODUTOS SIMILAR(ES)");

            //2.8
            escopoFornecimento.AddRichText("projetos-similares", "BUSCA DE ANTERIORIDADE");
            //3
            AddRichText("analise-selecao-proposta", "ANÁLISE E SELEÇÃO DAS PROPOSTAS");
            //4
            AddRichText("obrigações-proponente", "OBRIGAÇÕES DO PROPONENTE E/OU CONTRATADO");
            //5
            AddRichText("informacoes-gerais", "INFORMAÇÕES GERAIS");
        }
    }
}