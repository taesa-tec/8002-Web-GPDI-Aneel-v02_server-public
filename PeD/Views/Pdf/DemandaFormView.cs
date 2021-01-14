using PeD.Models.Demandas;
using PeD.Models.Demandas.Forms;

namespace PeD.Views.Pdf
{
    public class DemandaFormView
    {
        public Demanda Demanda { get; set; }
        public DemandaFormValues DemandaFormValues { get; set; }
        public FieldList Form { get; set; }
        public FieldRendered Rendered { get; set; }
    }
}