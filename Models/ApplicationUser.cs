using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace APIGestor.Models
{
    [JsonConverter(typeof(ApplicationUserConverter))]
    public class ApplicationUser : IdentityUser
    {
        public UserStatus? Status { get; set; }
        [NotMapped]
        public string StatusValor { get => (Status != null) ? Enum.GetName(typeof(UserStatus), Status) : null; }
        public string NomeCompleto { get; set; }
        public int? CatalogEmpresaId;
        [ForeignKey("CatalogEmpresaId")]
        public CatalogEmpresa CatalogEmpresa { get; set; }
        public string RazaoSocial { get; set; }
        public FotoPerfil FotoPerfil { get; set; }
        public string Role { get; set; }
        public string CPF { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
    public class FotoPerfil
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public byte[] File { get; set; }

    }
    public enum UserStatus
    {
        Inativo,
        Ativo

    }

    public class ApplicationUserConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ApplicationUser);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            serializer.Populate(reader, existingValue);
            return serializer.Deserialize(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var user = value as ApplicationUser;
            writer.WriteStartObject();
            writer.WritePropertyName("id");
            writer.WriteValue(user.Id);
            if (user.FotoPerfil != null)
            {

                writer.WritePropertyName("fotoPerfil");

                writer.WriteStartObject();

                writer.WritePropertyName("id");
                writer.WriteValue(user.FotoPerfil.Id);
                writer.WritePropertyName("file");
                writer.WriteValue(user.FotoPerfil.File);

                writer.WriteEndObject();
            }
            writer.WritePropertyName("nomeCompleto");
            writer.WriteValue(user.NomeCompleto);
            writer.WritePropertyName("userName");
            writer.WriteValue(user.UserName);
            writer.WritePropertyName("email");
            writer.WriteValue(user.Email);
            writer.WritePropertyName("status");
            writer.WriteValue(user.Status);
            writer.WritePropertyName("dataCadastro");
            writer.WriteValue(user.DataCadastro);

            writer.WritePropertyName("razaoSocial");
            writer.WriteValue(user.RazaoSocial);

            writer.WritePropertyName("role");
            writer.WriteValue(user.Role);

            writer.WritePropertyName("cpf");
            writer.WriteValue(user.CPF);

            writer.WritePropertyName("statusValor");
            writer.WriteValue(user.StatusValor);

            writer.WriteEndObject();
        }
    }
}