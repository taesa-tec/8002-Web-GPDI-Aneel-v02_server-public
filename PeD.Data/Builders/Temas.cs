using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeD.Core.Models.Catalogos;

namespace PeD.Data.Builders
{
    public static class Temas
    {
        public static EntityTypeBuilder<Tema> Config(this EntityTypeBuilder<Tema> builder)
        {
            return builder.Seed().ToTable("Temas");
        }

        public static EntityTypeBuilder<Tema> Seed(this EntityTypeBuilder<Tema> builder)
        {
            var _temas = new List<Tema>
            {
                new Tema
                {
                    Valor = "FA",
                    Nome = "Fontes alternativas de geração de energia elétrica",
                    SubTemas = new List<Tema>
                    {
                        new Tema
                        {
                            Valor = "FA01",
                            Nome =
                                "Alternativas energéticas sustentáveis de atendimento a pequenos sistemas isolados."
                        },
                        new Tema
                            {Valor = "FA02", Nome = "Geração de energia a partir de resíduos sólidos urbanos."},
                        new Tema
                        {
                            Valor = "FA03",
                            Nome = "Novos materiais e equipamentos para geração de energia por fontes alternativas."
                        },
                        new Tema
                        {
                            Valor = "FA04",
                            Nome = "Tecnologias para aproveitamento de novos combustíveis em plantas geradoras."
                        },
                        new Tema {Valor = "FA0X", Nome = "Outro.", Order = 1}
                    }
                },
                new Tema
                {
                    Valor = "GT",
                    Nome = "Geração Termelétrica",
                    SubTemas = new List<Tema>
                    {
                        new Tema
                        {
                            Valor = "GT01",
                            Nome =
                                "Avaliação de riscos e incertezas do fornecimento contínuo de gás natural para geração termelétrica."
                        },
                        new Tema
                        {
                            Valor = "GT02",
                            Nome =
                                "Novas técnicas para eficientização e diminuição da emissão de poluentes de usinas termelétricas a combustível derivado de petróleo."
                        },
                        new Tema
                        {
                            Valor = "GT03",
                            Nome =
                                "Otimização da geração de energia elétrica em plantas industriais: aumento de eficiência na cogeração."
                        },
                        new Tema {Valor = "GT04", Nome = "Micro-sistemas de cogeração residenciais."},
                        new Tema
                        {
                            Valor = "GT05", Nome = "Técnicas para captura e seqüestro de carbono de termelétricas."
                        },
                        new Tema {Valor = "GT0X", Nome = "Outro.", Order = 1}
                    }
                },
                new Tema
                {
                    Valor = "GB",
                    Nome = "Gestão de Bacias e Reservatórios",
                    SubTemas = new List<Tema>
                    {
                        new Tema
                        {
                            Valor = "GB01",
                            Nome =
                                "Emissões de gases de efeito estufa (GEE) em reservatórios de usinas hidrelétricas."
                        },
                        new Tema
                        {
                            Valor = "GB02",
                            Nome =
                                "Efeitos de mudanças climáticas globais no regime hidrológico de bacias hidrográficas."
                        },
                        new Tema
                        {
                            Valor = "GB03",
                            Nome = "Integração e otimização do uso múltiplo de reservatórios hidrelétricos."
                        },
                        new Tema
                        {
                            Valor = "GB04",
                            Nome = "Gestão sócio-patrimonial de reservatórios de usinas hidrelétricas."
                        },
                        new Tema
                            {Valor = "GB05", Nome = "Gestão da segurança de barragens de usinas hidrelétricas."},
                        new Tema
                        {
                            Valor = "GB06",
                            Nome = "Assoreamento de reservatórios formados por barragens de usinas hidrelétricas."
                        },
                        new Tema {Valor = "GB0X", Nome = "Outro.", Order = 1}
                    }
                },
                new Tema
                {
                    Valor = "MA",
                    Nome = "Meio Ambiente",
                    SubTemas = new List<Tema>
                    {
                        new Tema
                        {
                            Valor = "MA01",
                            Nome = "Impactos e restrições socioambientais de sistemas de energia elétrica."
                        },
                        new Tema
                        {
                            Valor = "MA02",
                            Nome =
                                "Metodologias para mensuração econômico-financeira de externalidades em sistemas de energia elétrica."
                        },
                        new Tema
                        {
                            Valor = "MA03",
                            Nome =
                                "Estudos de toxicidade relacionados à deterioração da qualidade da água em reservatórios. "
                        },
                        new Tema {Valor = "MA0X", Nome = "Outro.", Order = 1}
                    }
                },
                new Tema
                {
                    Valor = "SE",
                    Nome = "Segurança",
                    SubTemas = new List<Tema>
                    {
                        new Tema
                        {
                            Valor = "SE01",
                            Nome =
                                "Identificação e mitigação dos impactos de campos eletromagnéticos em organismos vivos."
                        },
                        new Tema
                            {Valor = "SE02", Nome = "Análise e mitigação de riscos de acidentes elétricos."},
                        new Tema
                            {Valor = "SE03", Nome = "Novas tecnologias para equipamentos de proteção individual."},
                        new Tema
                        {
                            Valor = "SE04",
                            Nome = "Novas tecnologias para inspeção e manutenção de sistemas elétricos."
                        },
                        new Tema {Valor = "SE0X", Nome = "Outro.", Order = 1}
                    }
                },
                new Tema
                {
                    Valor = "EE",
                    Nome = "Eficiência Energética",
                    SubTemas = new List<Tema>
                    {
                        new Tema
                            {Valor = "EE01", Nome = "Novas tecnologias para melhoria da eficiência energética."},
                        new Tema {Valor = "EE02", Nome = "Gerenciamento de carga pelo lado da demanda."},
                        new Tema
                            {Valor = "EE03", Nome = "Definição de indicadores de eficiência energética."},
                        new Tema
                        {
                            Valor = "EE04",
                            Nome = "Metodologias para avaliação de resultados de projetos de eficiência energética."
                        },
                        new Tema {Valor = "EE0X", Nome = "Outro.", Order = 1}
                    }
                },
                new Tema
                {
                    Valor = "PL",
                    Nome = "Planejamento de Sistemas de Energia Elétrica",
                    SubTemas = new List<Tema>
                    {
                        new Tema
                            {Valor = "PL01", Nome = "Planejamento integrado da expansão de sistemas elétricos."},
                        new Tema {Valor = "PL02", Nome = "Integração de centrais eólicas ao SIN."},
                        new Tema
                            {Valor = "PL03", Nome = "Integração de geração distribuída a redes elétricas."},
                        new Tema
                        {
                            Valor = "PL04",
                            Nome =
                                "Metodologia de previsão de mercado para diferentes níveis temporais e estratégias de contratação."
                        },
                        new Tema
                        {
                            Valor = "PL05",
                            Nome = "Modelos hidrodinâmicos aplicados em reservatórios de usinas hidrelétricas."
                        },
                        new Tema
                        {
                            Valor = "PL06", Nome = "Materiais supercondutores para transmissão de energia elétrica."
                        },
                        new Tema
                        {
                            Valor = "PL07",
                            Nome = "Tecnologias e sistemas de transmissão de energia em longas distâncias."
                        },
                        new Tema {Valor = "PL0X", Nome = "Outro.", Order = 1}
                    }
                },
                new Tema
                {
                    Valor = "OP",
                    Nome = "Operação de Sistemas de Energia Elétrica",
                    SubTemas = new List<Tema>
                    {
                        new Tema
                        {
                            Valor = "OP01",
                            Nome =
                                "Ferramentas de apoio à operação de sistemas elétricos de potência em tempo real."
                        },
                        new Tema
                        {
                            Valor = "OP02",
                            Nome = "Critérios de gerenciamento de carga para diferentes níveis de hierarquia."
                        },
                        new Tema
                        {
                            Valor = "OP03",
                            Nome = "Estruturas, funções e regras de operação dos mercados de serviços ancilares."
                        },
                        new Tema
                        {
                            Valor = "OP04",
                            Nome = "Otimização estrutural e paramétrica da capacidade dos sistemas de distribuição."
                        },
                        new Tema
                        {
                            Valor = "OP05",
                            Nome = "Alocação de fontes de potência reativa em sistemas de distribuição."
                        },
                        new Tema
                        {
                            Valor = "OP06",
                            Nome = "Estudo, simulação e análise do desempenho de sistemas elétricos de potência."
                        },
                        new Tema
                        {
                            Valor = "OP07",
                            Nome =
                                "Análise das grandes perturbações e impactos no planejamento, operação e controle."
                        },
                        new Tema
                        {
                            Valor = "OP08",
                            Nome = "Desenvolvimento de modelos para a otimização de despacho hidrotérmico."
                        },
                        new Tema
                        {
                            Valor = "OP09",
                            Nome =
                                "Desenvolvimento e/ou aprimoramento dos modelos de previsão de chuva versus vazão."
                        },
                        new Tema
                        {
                            Valor = "OP10",
                            Nome = "Sistemas de monitoramento da operação de usinas não-despachadas pelo ONS."
                        },
                        new Tema {Valor = "OP0X", Nome = "Outros."}
                    }
                },
                new Tema
                {
                    Valor = "SC",
                    Nome = "Supervisão, Controle e Proteção de Sistemas de Energia Elétrica",
                    SubTemas = new List<Tema>
                    {
                        new Tema
                        {
                            Valor = "SC01",
                            Nome = "Implementação de sistemas de controle (robusto, adaptativo e inteligente)."
                        },
                        new Tema {Valor = "SC02", Nome = "Análise dinâmica de sistemas em tempo real."},
                        new Tema
                        {
                            Valor = "SC03",
                            Nome = "Técnicas eficientes de restauração rápida de grandes centros de carga."
                        },
                        new Tema
                        {
                            Valor = "SC04",
                            Nome = "Desenvolvimento de técnicas para recomposição de sistemas elétricos."
                        },
                        new Tema
                        {
                            Valor = "SC05",
                            Nome =
                                "Técnicas de inteligência artificial aplicadas ao controle, operação e proteção de sistemas elétricos."
                        },
                        new Tema
                        {
                            Valor = "SC06",
                            Nome = "Novas tecnologias para supervisão do fornecimento de energia elétrica."
                        },
                        new Tema
                            {Valor = "SC07", Nome = "Desenvolvimento e aplicação de sistemas de medição fasorial."},
                        new Tema {Valor = "SC08", Nome = "Análise de falhas em sistemas elétricos."},
                        new Tema
                            {Valor = "SC09", Nome = "Compatibilidade eletromagnética em sistemas elétricos."},
                        new Tema {Valor = "SC10", Nome = "Sistemas de aterramento."},
                        new Tema {Valor = "SC0X", Nome = "Outro.", Order = 1}
                    }
                },
                new Tema
                {
                    Valor = "QC",
                    Nome = "Qualidade e Confiabilidade dos Serviços de Energia Elétrica",
                    SubTemas = new List<Tema>
                    {
                        new Tema
                        {
                            Valor = "QC01",
                            Nome =
                                "Sistemas e técnicas de monitoração e gerenciamento da qualidade da energia elétrica."
                        },
                        new Tema
                        {
                            Valor = "QC02",
                            Nome = "Modelagem e análise dos distúrbios associados à qualidade da energia elétrica."
                        },
                        new Tema
                        {
                            Valor = "QC03",
                            Nome =
                                "Requisitos para conexão de cargas potencialmente perturbadoras no sistema elétrico."
                        },
                        new Tema
                        {
                            Valor = "QC04", Nome = "Curvas de sensibilidade e de suportabilidade de equipamentos."
                        },
                        new Tema
                        {
                            Valor = "QC05",
                            Nome = "Impactos econômicos e aspectos contratuais da qualidade da energia elétrica."
                        },
                        new Tema
                        {
                            Valor = "QC06",
                            Nome = "Compensação financeira por violação de indicadores de qualidade."
                        },
                        new Tema {Valor = "QC0X", Nome = "Outro.", Order = 1}
                    }
                },
                new Tema
                {
                    Valor = "MF",
                    Nome = "Medição, faturamento e combate a perdas comerciais",
                    SubTemas = new List<Tema>
                    {
                        new Tema
                        {
                            Valor = "MF01", Nome = "Avaliação econômica para definição da perda mínima atingível."
                        },
                        new Tema
                        {
                            Valor = "MF02",
                            Nome = "Estimação, análise e redução de perdas técnicas em sistemas elétricos."
                        },
                        new Tema
                        {
                            Valor = "MF03",
                            Nome =
                                "Desenvolvimento de tecnologias para combate à fraude e ao furto de energia elétrica."
                        },
                        new Tema
                        {
                            Valor = "MF04",
                            Nome =
                                "Diagnóstico, prospecção e redução da vulnerabilidade de sistemas elétricos ao furto e à fraude."
                        },
                        new Tema
                        {
                            Valor = "MF05",
                            Nome = "Energia economizada e agregada ao mercado após regularização de fraudes."
                        },
                        new Tema
                        {
                            Valor = "MF06",
                            Nome = "Uso de indicadores socioeconômicos, dados fiscais e gastos com outros insumos."
                        },
                        new Tema
                        {
                            Valor = "MF07",
                            Nome = "Gerenciamento dos equipamentos de medição (qualidade e redução de falhas)."
                        },
                        new Tema
                        {
                            Valor = "MF08",
                            Nome = "Impacto dos projetos de eficiência energética na redução de perdas comerciais."
                        },
                        new Tema
                        {
                            Valor = "MF09",
                            Nome =
                                "Sistemas centralizados de medição, controle e gerenciamento de energia em consumidores finais."
                        },
                        new Tema
                            {Valor = "MF10", Nome = "Sistemas de tarifação e novas estruturas tarifárias."},
                        new Tema {Valor = "MF0X", Nome = "Outro.", Order = 1}
                    }
                },
                new Tema
                {
                    Valor = "OU",
                    Nome = "Outros",
                    SubTemas = new List<Tema>
                    {
                        new Tema {Valor = "OU  ", Nome = "Outros"}
                    },
                    Order = 1
                }
            };

            var id = 1;
            var temas = new List<Tema>();
            _temas.ForEach(tema =>
            {
                tema.Id = id++;
                temas.Add(tema);
                tema.SubTemas.ForEach(sub =>
                {
                    sub.ParentId = tema.Id;
                    sub.Id = id++;
                    temas.Add(sub);
                });
            });
            temas.ForEach(t => t.SubTemas = new List<Tema>());
            builder.HasData(temas);
            return builder;
        }
    }
}