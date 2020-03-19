using APIGestor.Models.Demandas;
using APIGestor.Models.Demandas.Forms;

namespace APIGestor.Views.Pdf
{
    public class DemandaFormView
    {
        public Demanda Demanda { get; set; }
        public DemandaFormValues DemandaFormValues { get; set; }
        public FieldList Form { get; set; }
        public FieldRendered Rendered { get; set; }
    }
}