using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace PeD.Core.Models
{
    public class LogItem
    {
        public string Titulo { get; set; }
        public object Valor { get; set; }
        public string Type { get; set; }

        [JsonConstructor]
        public LogItem(string titulo, object valor)
        {
            Titulo = titulo;
            Valor = valor;
            if (valor != null)
                Type = valor.GetType().FullName;
        }

        public LogItem(string titulo, object valor, string type)
        {
            Titulo = titulo;
            Valor = valor;
            Type = type;
        }
    }

    internal static class ExtensionObject
    {
        public static object GetPropValue(this object obj, string propName)
        {
            var nameParts = propName.Split('.');
            if (nameParts.Length == 1)
            {
                return obj.GetType().GetProperty(propName)?.GetValue(obj, null);
            }

            foreach (var part in nameParts)
            {
                if (obj == null)
                {
                    return null;
                }

                var type = obj.GetType();
                var info = type.GetProperty(part);
                if (info == null)
                {
                    return null;
                }

                obj = info.GetValue(obj, null);
            }

            return obj;
        }
    }
}