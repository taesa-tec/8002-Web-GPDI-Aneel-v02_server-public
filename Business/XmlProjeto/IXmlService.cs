using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Data;
using APIGestor.Models;
using Microsoft.AspNetCore.Identity;
using APIGestor.Security;
using System.Xml.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace APIGestor.Business
{

    

    interface IXmlService<T>
    {
        Resultado ValidaXml(int ProjetoId);

        // Projeto obterProjeto(int Id);

        T GerarXml(int ProjetoId, string Versao, string UserId);
    }
}
