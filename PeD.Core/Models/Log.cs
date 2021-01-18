using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace PeD.Core.Models
{

    public class Log
    {
        [Key]
        public int Id { get; set; }
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
        public LogData Data
        {
            get
            {
                LogData data = new LogData();
                try
                {
                    data.statusAnterior = JsonConvert.DeserializeObject<List<LogItem>>(this.StatusAnterior);
                }
                catch (Exception)
                {
                    data.statusAnterior = null;
                }
                try
                {
                    data.statusNovo = JsonConvert.DeserializeObject<List<LogItem>>(this.StatusNovo);
                }
                catch (Exception)
                {
                    data.statusNovo = null;

                }
                return data;

            }
            set
            {
                try
                {
                    StatusAnterior = JsonConvert.SerializeObject(value.statusAnterior);
                    StatusNovo = JsonConvert.SerializeObject(value.statusNovo);
                }
                catch (Exception)
                {
                    StatusNovo = "";
                    StatusAnterior = "";
                }
            }
        }


    }
    public struct LogData
    {
        public List<LogItem> statusAnterior, statusNovo;
    }
    public enum Acoes
    {
        Create, Retrieve, Update, Delete
    }
}