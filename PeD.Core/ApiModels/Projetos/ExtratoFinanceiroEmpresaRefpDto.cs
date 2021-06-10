using System.Collections.Generic;
using System.Linq;
using PeD.Core.Models.Projetos;

namespace PeD.Core.ApiModels.Projetos
{
    public abstract class ExtratoFinanceiroGroupRefpDto<TO, TE>
    {
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public abstract decimal Previsto { get; }
        public abstract decimal Realizado { get; }
        public decimal Desvio => Previsto > 0 ? ((Realizado / Previsto) - 1) * 100m : 0;
        public IEnumerable<TO> Orcamento { get; set; }
        public IEnumerable<TE> Registros { get; set; }
    }

    public class ExtratoFinanceiroGroupRefpDto : ExtratoFinanceiroGroupRefpDto<Orcamento, RegistroFinanceiroInfo>
    {
        public override decimal Previsto => Orcamento.Sum(e => e.Total);
        public override decimal Realizado => Registros.Sum(e => e.Custo);
    }

    public class ExtratoFinanceiroEmpresaRefpDto
    {
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public decimal Previsto => Categorias.Sum(ec => ec.Previsto);
        public decimal Realizado => Categorias.Sum(ec => ec.Realizado);
        public decimal Desvio => Previsto > 0 ? ((Realizado / Previsto) - 1) * 100m : 0;
        public IEnumerable<ExtratoFinanceiroGroupRefpDto> Categorias { get; set; }
    }
}