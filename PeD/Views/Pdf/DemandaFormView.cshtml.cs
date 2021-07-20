using PeD.Core;
using PeD.Core.Models;
using PeD.Core.Models.Demandas;
using PeD.Core.Models.Demandas.Forms;

namespace PeD.Views.Pdf
{
    public class DemandaFormView
    {
        public ApplicationUser Diretor { get; set; }
        public ApplicationUser Gerente { get; set; }
        public ApplicationUser Coordenador { get; set; }
        public EquipePeD EquipePeD { get; set; }
        public Demanda Demanda { get; set; }
        public DemandaFormValues DemandaFormValues { get; set; }
        public FieldList Form { get; set; }
        public FieldRendered Rendered { get; set; }
    }
}