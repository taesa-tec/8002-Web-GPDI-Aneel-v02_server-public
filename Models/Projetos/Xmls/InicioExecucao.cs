using System;
using System.Globalization;

namespace APIGestor.Models.Projetos.Xmls
{
    public class InicioExecucao
    {
        public PD_InicioExecProjeto PD_InicioExecProjeto { get; set; }
    }
    public class PD_InicioExecProjeto
    {
        public InicioProjeto Projeto{ get; set; }
    }
    public class InicioProjeto
    {
        public string CodProjeto { get; set; }
        private DateTime _dataIniProjeto{ get; set; }
        public string DataIniProjeto{
            get => _dataIniProjeto.ToString("ddMMyyyy", CultureInfo.InvariantCulture);
            set => _dataIniProjeto = Convert.ToDateTime(value);
        }
        public string DirPropIntProjeto { get; set; }
    }
}