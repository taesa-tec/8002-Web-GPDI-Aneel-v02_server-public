using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PeD.Core.Models;

namespace PeD.Core.ApiModels
{
    public class LogDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUserDto User { get; set; }
        public string Tela { get; set; }
        public string Acao { get; set; }
        public string StatusAnterior { get; set; }
        public string StatusNovo { get; set; }
        public DateTime Created { get; set; }

        public LogData Data
        {
            get
            {
                var data = new LogData();
                try
                {
                    data.StatusAnterior = JsonConvert.DeserializeObject<List<LogItem>>(StatusAnterior);
                }
                catch (Exception)
                {
                    data.StatusAnterior = null;
                }

                try
                {
                    data.StatusNovo = JsonConvert.DeserializeObject<List<LogItem>>(StatusNovo);
                }
                catch (Exception)
                {
                    data.StatusNovo = null;
                }

                return data;
            }
            set
            {
                try
                {
                    StatusAnterior = JsonConvert.SerializeObject(value.StatusAnterior);
                    StatusNovo = JsonConvert.SerializeObject(value.StatusNovo);
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
        public List<LogItem> StatusAnterior, StatusNovo;
    }
}