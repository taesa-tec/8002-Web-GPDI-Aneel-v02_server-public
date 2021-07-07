using System.Collections.Generic;
using System.Linq;
using PeD.Core.Models.Propostas;

namespace PeD.Core.Models.Relatorios.Fornecedores
{
    public class EtapaRelatorio
    {
        public static List<AlocacaoRecurso> AgruparPorCategoria(List<AlocacaoRecurso> list)
        {
            return list.GroupBy(a => a.CategoriaContabil)
                .Select(i => new AlocacaoRecurso()
                {
                    CategoriaContabil = i.Key,
                    Valor = i.Sum(a => a.Valor)
                })
                .ToList();
        }

        public static Dictionary<string, List<AlocacaoRecurso>> AgruparPorEmpresaRecebedora(List<AlocacaoRecurso> list)
        {
            return list.GroupBy(i => i.EmpresaRecebedoraCodigo)
                .ToDictionary(i => i.Key, i => i.ToList());
        }

        public static decimal CustoPorCategoria(List<AlocacaoRecurso> list, string categoria)
        {
            return list.Where(a => a.CategoriaContabil == categoria).Sum(i => i.Valor);
        }

        public static decimal CustoEntreEmpresas(List<AlocacaoRecurso> list, string financiadora, string recebedora)
        {
            return list.Where(i =>
                    i.EmpresaFinanciadoraCodigo == financiadora && i.EmpresaRecebedoraCodigo == recebedora)
                .Sum(x => x.Valor);
        }

        public static decimal CustoFinanciadora(List<AlocacaoRecurso> list, string financiadora, bool interno = true)
        {
            return list.Where(i =>
                    i.EmpresaFinanciadoraCodigo == financiadora &&
                    (interno || i.EmpresaRecebedoraCodigo != financiadora))
                .Sum(x => x.Valor);
        }

        public static decimal CustoFinanciadora(List<EtapaRelatorio> list, string financiadora, bool interno = true)
        {
            return CustoFinanciadora(list.SelectMany(e => e.Alocacoes).ToList(), financiadora, interno);
        }

        public string DescricaoAtividades { get; set; }
        public short MesInicio { get; set; }
        public short MesFim { get; set; }
        public Produto Produto { get; set; }
        public List<int> Meses { get; set; }
        public short Ordem { get; set; }

        public decimal CustoTotal(bool interno = true)
        {
            if (interno)
                return Alocacoes.Sum(a => a.Valor);
            return Alocacoes.Where(i => i.EmpresaFinanciadoraCodigo != i.EmpresaRecebedoraCodigo).Sum(a => a.Valor);
        }

        public List<AlocacaoRecurso> Alocacoes { get; set; }

        public decimal AlocacoesInternasSum => AlocacoesInternas.Sum(a => a.Valor);

        public List<AlocacaoRecurso> AlocacoesInternas
        {
            get
            {
                return Alocacoes.Where(c =>
                        c.EmpresaFinanciadoraCodigo.StartsWith("Taesa") &&
                        c.EmpresaRecebedoraCodigo.StartsWith("Taesa"))
                    .ToList();
            }
        }

        public List<AlocacaoRecurso> AlocacoesExternas
        {
            get
            {
                return Alocacoes.Where(c =>
                        c.EmpresaFinanciadoraCodigo.StartsWith("Taesa") &&
                        !c.EmpresaRecebedoraCodigo.StartsWith("Taesa"))
                    .ToList();
            }
        }


        public Dictionary<string, Dictionary<string, List<AlocacaoRecurso>>> AlocacoesEntreEmpresas
        {
            get
            {
                return Alocacoes
                    .GroupBy(i => i.EmpresaFinanciadoraCodigo)
                    .ToDictionary(i => i.Key, i => i
                        .GroupBy(x => x.EmpresaRecebedoraCodigo)
                        .ToDictionary(x => x.Key, x => x.ToList())
                    );
            }
        }

        public List<AlocacaoRecurso> AlocacaoPorCategoria => AgruparPorCategoria(Alocacoes);
        public List<AlocacaoRecurso> AlocacoesInternasPorCategoria => AgruparPorCategoria(AlocacoesInternas);
        public List<AlocacaoRecurso> AlocacoesExternasPorCategoria => AgruparPorCategoria(AlocacoesExternas);
    }
}