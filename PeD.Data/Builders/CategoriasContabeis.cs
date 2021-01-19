using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeD.Core.Models.Catalogos;

namespace PeD.Data.Builders
{
    public static class CategoriasContabeis
    {
        public static EntityTypeBuilder<CategoriaContabil> Config(this EntityTypeBuilder<CategoriaContabil> builder)
        {
            return builder.Seed();
        }

        public static EntityTypeBuilder<CategoriaContabil> Seed(this EntityTypeBuilder<CategoriaContabil> builder)
        {
            var categorias = new List<CategoriaContabil>
            {
                new CategoriaContabil
                {
                    Valor = "RH",
                    Nome = "Recursos Humanos",
                    Atividades = new List<Atividade>
                    {
                        new Atividade
                        {
                            Valor = "HH",
                            Nome =
                                "Dedicação horária dos membros da equipe de gestão do Programa de P&D da Empresa, quadro efetivo."
                        }
                    }
                },
                new CategoriaContabil
                {
                    Valor = "ST",
                    Nome = "Serviços de Terceiros",
                    Atividades = new List<Atividade>
                    {
                        new Atividade
                        {
                            Valor = "FG",
                            Nome =
                                "Desenvolvimento de ferramenta para gestão do Programa de P&D da Empresa, excluindose aquisição de equipamentos."
                        },
                        new Atividade
                        {
                            Valor = "PP",
                            Nome =
                                "Prospecção tecnológica e demais atividades necessárias ao planejamento e à elaboração do plano estratégico de investimento em P&D."
                        }
                    }
                },
                new CategoriaContabil
                {
                    Valor = "MC",
                    Nome = "Materiais de Consumo",
                    Atividades = new List<Atividade>
                    {
                        new Atividade
                        {
                            Valor = "RP",
                            Nome = "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução."
                        }
                    }
                },
                new CategoriaContabil
                {
                    Valor = "VD",
                    Nome = "Viagens e Diárias",
                    Atividades = new List<Atividade>
                    {
                        new Atividade
                        {
                            Valor = "EC",
                            Nome =
                                "Participação dos membros da equipe de gestão em eventos sobre pesquisa, desenvolvimento e inovação relacionados ao setor elétrico e/ou em cursos de gestão tecnológica e da informação."
                        },
                        new Atividade
                        {
                            Valor = "PP",
                            Nome =
                                "Prospecção tecnológica e demais atividades necessárias ao planejamento e à elaboração do plano estratégico de investimento em P&D."
                        },
                        new Atividade
                        {
                            Valor = "RP",
                            Nome = "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução."
                        },
                        new Atividade
                        {
                            Valor = "AP",
                            Nome =
                                "Participação dos responsáveis técnicos pelos projetos de P&D nas avaliações presenciais convocadas pela ANEEL."
                        }
                    }
                },
                new CategoriaContabil
                {
                    Valor = "OU",
                    Nome = "Outros",
                    Atividades = new List<Atividade>
                    {
                        new Atividade
                        {
                            Valor = "EC",
                            Nome =
                                "Participação dos membros da equipe de gestão em eventos sobre pesquisa, desenvolvimento e inovação relacionados ao setor elétrico e/ou em cursos de gestão tecnológica e da informação."
                        },
                        new Atividade
                        {
                            Valor = "RP",
                            Nome = "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução."
                        },
                        new Atividade
                        {
                            Valor = "BA",
                            Nome = "Buscas de anterioridade no Instituto Nacional da Propriedade Industrial (INPI)."
                        }
                    }
                },
                new CategoriaContabil
                {
                    Valor = "CT",
                    Nome = "CITENEL",
                    Atividades = new List<Atividade>
                    {
                        new Atividade {Valor = "AC", Nome = "Apoio à realização do CITENEL."}
                    }
                },
                new CategoriaContabil
                {
                    Valor = "AC",
                    Nome = "Auditoria Contábil e Financeira",
                    Atividades = new List<Atividade>
                    {
                        new Atividade
                        {
                            Valor = "CA",
                            Nome = "Contratação de auditoria contábil e financeira para os projetos concluídos."
                        }
                    }
                }
            };

            builder.HasData(categorias);
            return builder;
        }
    }
}