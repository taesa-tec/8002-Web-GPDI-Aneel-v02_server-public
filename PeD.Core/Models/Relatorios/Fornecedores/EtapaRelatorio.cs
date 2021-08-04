using System.Collections.Generic;
using System.Linq;
using PeD.Core.Models.Propostas;

namespace PeD.Core.Models.Relatorios.Fornecedores
{
    public class EtapaRelatorio
    {
        public static Dictionary<int, List<AlocacaoInfo>> AgruparPorEmpresaRecebedora(List<AlocacaoInfo> list)
        {
            return list.GroupBy(i => i.EmpresaRecebedoraId)
                .ToDictionary(i => i.Key, i => i.ToList());
        }

        public static decimal CustoPorCategoria(List<AlocacaoInfo> list, string categoria)
        {
            return list.Where(a => a.Categoria == categoria).Sum(i => i.Custo);
        }

        public static decimal CustoEntreEmpresas(List<AlocacaoInfo> list, int financiadoraId, int recebedoraId)
        {
            return list.Where(i =>
                    i.EmpresaFinanciadoraId == financiadoraId && i.EmpresaRecebedoraId == recebedoraId)
                .Sum(x => x.Custo);
        }

        public static decimal CustoFinanciadora(List<AlocacaoInfo> list, int financiadoraId, bool interno = true)
        {
            return list.Where(i =>
                    i.EmpresaFinanciadoraId == financiadoraId &&
                    (interno || i.EmpresaRecebedoraId != financiadoraId))
                .Sum(x => x.Custo);
        }

        public static decimal CustoFinanciadora(List<EtapaRelatorio> list, int financiadoraId, bool interno = true)
        {
            return CustoFinanciadora(list.SelectMany(e => e.Alocacoes).ToList(), financiadoraId, interno);
        }

        public int Id { get; set; }
        public string DescricaoAtividades { get; set; }
        public short MesInicio { get; set; }
        public short MesFim { get; set; }
        public Produto Produto { get; set; }
        public List<int> Meses { get; set; }
        public short Ordem { get; set; }

        public decimal CustoTotal(bool incluirInterno = true)
        {
            if (incluirInterno)
                return Alocacoes.Sum(a => a.Custo);
            return Alocacoes.Where(a=>a.EmpresaRecebedoraFuncao != Funcao.Cooperada).Sum(a => a.Custo);
        }

        public List<AlocacaoInfo> Alocacoes { get; set; }

        public decimal AlocacoesInternasSum => AlocacoesInternas.Sum(a => a.Custo);

        public List<AlocacaoInfo> AlocacoesInternas
        {
            get
            {
                return Alocacoes.Where(c => c.EmpresaFinanciadoraFuncao == Funcao.Cooperada && c.EmpresaRecebedoraFuncao == Funcao.Cooperada)
                    .ToList();
            }
        }

        public List<AlocacaoInfo> AlocacoesExternas
        {
            get
            {
                return Alocacoes
                    .Where(c => c.EmpresaFinanciadoraFuncao == Funcao.Cooperada &&
                                c.EmpresaRecebedoraFuncao == Funcao.Executora)
                    .ToList();
            }
        }


        public Dictionary<int, Dictionary<int, List<AlocacaoInfo>>> AlocacoesEntreEmpresas
        {
            get
            {
                return Alocacoes
                    .GroupBy(i => i.EmpresaFinanciadoraId)
                    .ToDictionary(i => i.Key, i => i
                        .GroupBy(x => x.EmpresaRecebedoraId)
                        .ToDictionary(x => x.Key, x => x.ToList())
                    );
            }
        }
    }
}