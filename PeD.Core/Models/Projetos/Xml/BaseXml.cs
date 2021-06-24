using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace PeD.Core.Models.Projetos.Xml
{
    public abstract class BaseXml
    {
        public enum XmlTipo
        {
            NONE,
            DUTO,
            MOVIMENTACAOFINANCEIRA,
            PROGRAMA,
            PROJETOGESTAO,
            PROJETOPED,
            INTERESSEPROJETOPED,
            INICIOEXECUCAOPROJETO,
            PRORROGAEXECUCAOPROJETO,
            RELATORIOFINALPED,
            RELATORIOFINALGESTAO,
            RELATORIOAUDITORIAPED,
            RELATORIOAUDITORIAGESTAO
        }

        protected virtual string Root => "PED";
        [JsonIgnore] public virtual XmlTipo Tipo => XmlTipo.NONE;

        [JsonIgnore] public Dictionary<string, string> Attributes = new Dictionary<string, string>();

        [JsonIgnore] public bool RemoveEmptys { get; set; } = true;

        public XDocument ToXml()
        {
            var xml = JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(this), Root);
            if (xml is null)
                throw new NullReferenceException();

            var innerXml = xml.InnerXml;
            innerXml = Regex.Replace(innerXml, "\u2013", "-");
            innerXml = Regex.Replace(innerXml, "\u201C", "\"");
            innerXml = Regex.Replace(innerXml, "\u201D", "\"");

            var xDoc = XDocument.Parse(innerXml);
            xDoc.Declaration = new XDeclaration("1.0", "ISO8859-1", null);
            if (xDoc.Root is null)
                throw new NullReferenceException();

            Attributes.Add("Tipo", Tipo.ToString());
            foreach (var keyValue in Attributes)
            {
                xDoc.Root.SetAttributeValue(keyValue.Key, keyValue.Value);
            }

            if (RemoveEmptys)
            {
                xDoc.Descendants()
                    .Where(xElement => xElement.IsEmpty || String.IsNullOrWhiteSpace(xElement.Value))
                    .Remove();
            }

            return xDoc;
        }
    }
}