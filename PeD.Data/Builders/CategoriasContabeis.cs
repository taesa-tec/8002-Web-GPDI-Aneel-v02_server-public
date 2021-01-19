using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeD.Core.Models.Catalogos;

namespace PeD.Data.Builders
{
    public static class CategoriasContabeis
    {
        private static List<CategoriaContabil> _categoriaContabils;

        private static List<CategoriaContabil> GetCategorias()
        {
            if (_categoriaContabils != null)
                return _categoriaContabils;
            var categorias = new List<CategoriaContabil>
            {
                new CategoriaContabil
                {
                    Valor = "RH",
                    Nome = "Recursos Humanos",
                    Atividades = new List<CategoriaContabilAtividade>
                    {
                        new CategoriaContabilAtividade
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
                    Atividades = new List<CategoriaContabilAtividade>
                    {
                        new CategoriaContabilAtividade
                        {
                            Valor = "FG",
                            Nome =
                                "Desenvolvimento de ferramenta para gestão do Programa de P&D da Empresa, excluindose aquisição de equipamentos."
                        },
                        new CategoriaContabilAtividade
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
                    Atividades = new List<CategoriaContabilAtividade>
                    {
                        new CategoriaContabilAtividade
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
                    Atividades = new List<CategoriaContabilAtividade>
                    {
                        new CategoriaContabilAtividade
                        {
                            Valor = "EC",
                            Nome =
                                "Participação dos membros da equipe de gestão em eventos sobre pesquisa, desenvolvimento e inovação relacionados ao setor elétrico e/ou em cursos de gestão tecnológica e da informação."
                        },
                        new CategoriaContabilAtividade
                        {
                            Valor = "PP",
                            Nome =
                                "Prospecção tecnológica e demais atividades necessárias ao planejamento e à elaboração do plano estratégico de investimento em P&D."
                        },
                        new CategoriaContabilAtividade
                        {
                            Valor = "RP",
                            Nome = "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução."
                        },
                        new CategoriaContabilAtividade
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
                    Atividades = new List<CategoriaContabilAtividade>
                    {
                        new CategoriaContabilAtividade
                        {
                            Valor = "EC",
                            Nome =
                                "Participação dos membros da equipe de gestão em eventos sobre pesquisa, desenvolvimento e inovação relacionados ao setor elétrico e/ou em cursos de gestão tecnológica e da informação."
                        },
                        new CategoriaContabilAtividade
                        {
                            Valor = "RP",
                            Nome = "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução."
                        },
                        new CategoriaContabilAtividade
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
                    Atividades = new List<CategoriaContabilAtividade>
                    {
                        new CategoriaContabilAtividade {Valor = "AC", Nome = "Apoio à realização do CITENEL."}
                    }
                },
                new CategoriaContabil
                {
                    Valor = "AC",
                    Nome = "Auditoria Contábil e Financeira",
                    Atividades = new List<CategoriaContabilAtividade>
                    {
                        new CategoriaContabilAtividade
                        {
                            Valor = "CA",
                            Nome = "Contratação de auditoria contábil e financeira para os projetos concluídos."
                        }
                    }
                }
            };
            var id = 1;
            var ida = 1;

            categorias.ForEach(c =>
            {
                c.Id = id++;
                c.Atividades.ForEach(a =>
                {
                    a.Id = ida++;
                    a.CategoriaContabilId = c.Id;
                });
            });
            _categoriaContabils = categorias;
            return categorias;
        }

        public static EntityTypeBuilder<CategoriaContabil> Config(this EntityTypeBuilder<CategoriaContabil> builder)
        {
            return builder.Seed();
        }

        public static EntityTypeBuilder<CategoriaContabil> Seed(this EntityTypeBuilder<CategoriaContabil> builder)
        {
            var categorias = GetCategorias().Select(c => new CategoriaContabil()
            {
                Id = c.Id,
                Nome = c.Nome,
                Valor = c.Valor
            });
            builder.HasData(categorias);
            return builder;
        }

        public static EntityTypeBuilder<CategoriaContabilAtividade> Seed(this EntityTypeBuilder<CategoriaContabilAtividade> builder)
        {
            var atividades = GetCategorias().SelectMany(c => c.Atividades);
            builder.HasData(atividades);
            return builder;
        }
    }
}