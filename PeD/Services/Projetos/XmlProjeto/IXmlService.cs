using PeD.Models.Projetos;

namespace PeD.Services.Projetos.XmlProjeto
{

    

    interface IXmlService<T>
    {
        Resultado ValidaXml(int ProjetoId);

        // Projeto obterProjeto(int Id);

        T GerarXml(int ProjetoId, string Versao, string UserId);
    }
}
