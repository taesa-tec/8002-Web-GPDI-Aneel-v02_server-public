using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using static APIGestor.Models.LogItem;
using System.Reflection;
using System.Collections;

namespace APIGestor.Models {
    public class LogProjeto {
        private int _id;

        [Key]
        public int Id {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get; set; }
        [ForeignKey("ProjetoId")]
        public Projeto Projeto { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string Tela { get; set; }
        public Acoes Acao { get; set; }
        [NotMapped]
        public string AcaoValor { get => Enum.GetName(typeof(Acoes), Acao); }
        public string StatusAnterior { get; set; }
        public string StatusNovo { get; set; }
        public DateTime Created { get; set; }

        [NotMapped]
        public LogProjetoData Data {
            get {
                LogProjetoData data = new LogProjetoData();
                try {
                    data.statusAnterior = JsonConvert.DeserializeObject<List<LogItem>>(this.StatusAnterior);
                }
                catch(Exception) {
                    data.statusAnterior = null;
                }
                try {
                    data.statusNovo = JsonConvert.DeserializeObject<List<LogItem>>(this.StatusNovo);
                }
                catch(Exception) {
                    data.statusNovo = null;

                }
                return data;

            }
        }


        public static List<LogItem> logItems( object Entity, object EntityOld = null ) {
            var logItems = new List<LogItem>();

            if(Entity != null) {
                var props = Entity.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                foreach(var property in props) {
                    object oldValue = null;
                    var logger = (Attributes.LoggerAttribute)property.GetCustomAttributes(typeof(Attributes.LoggerAttribute), false).FirstOrDefault();

                    if(logger != null) {

                        var propertyValue = property.GetValue(Entity);

                        if(EntityOld != null) {
                            oldValue = property.GetValue(EntityOld);

                        }

                        if(propertyValue != null && !propertyValue.Equals(oldValue)) {

                            var type = propertyValue.GetType();

                            if(type.GetInterfaces().Any(t => t == typeof(IEnumerable)) && type.IsGenericType) {

                                foreach(var item in propertyValue as IEnumerable) {
                                    var _logItems = LogProjeto.logItems(item);
                                    if(_logItems.Count > 0) {
                                        logItems.AddRange(_logItems);
                                    }
                                }
                            }
                            else {
                                var showValue = logger.hasValueFrom ? Entity.getPropValue(logger.ValueFrom) : propertyValue;
                                var showName = logger.Name != null ? logger.Name : property.Name;
                                logItems.Add(new LogItem(showName, showValue));
                            }

                        }
                    }
                }
            }

            return logItems;
        }
    }

    public class LogItem {
        public string Titulo { get; set; }
        public object Valor { get; set; }
        public string Type { get; set; }

        public LogItem( string Titulo, object Valor ) {
            this.Titulo = Titulo;
            this.Valor = Valor;
            if(Valor != null)
                this.Type = Valor.GetType().FullName;
        }
    }
    public struct LogProjetoData {
        public object statusAnterior, statusNovo;
    }
    public enum Acoes {
        Create, Retrieve, Update, Delete
    }

    static class ExtensionObject {
        public static object getPropValue( this Object obj, string propName ) {
            string[] nameParts = propName.Split('.');
            if(nameParts.Length == 1) {
                return obj.GetType().GetProperty(propName).GetValue(obj, null);
            }

            foreach(String part in nameParts) {
                if(obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if(info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }
    }
}