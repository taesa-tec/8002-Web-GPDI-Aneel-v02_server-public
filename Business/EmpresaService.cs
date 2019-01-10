using System;
using System.Collections.Generic;
using System.Linq;
using APIGestor.Data;
using APIGestor.Models;

namespace APIGestor.Business
{
    public class EmpresaService
    {
        private GestorDbContext _context;

        public EmpresaService(GestorDbContext context)
        {
            _context = context;
        }

        public Empresa Obter(int id)
        {

            if (id>0)
            {
                return _context.Empresas.Where(
                    p => p.Id == id).FirstOrDefault();
            }
            else
                return null;
        }

        public IEnumerable<Empresa> ListarTodos()
        {
            return _context.Empresas
                .OrderBy(p => p.RazaoSocial).ToList();
        }

        public Resultado Incluir(Empresa dadosEmpresa)
        {
            Resultado resultado = DadosValidos(dadosEmpresa);
            resultado.Acao = "Inclusão de Empresa";

            if (resultado.Inconsistencias.Count == 0 &&
                _context.Empresas.Where(
                p => p.RazaoSocial == dadosEmpresa.RazaoSocial).Count() > 0)
            {
                resultado.Inconsistencias.Add(
                    "Razão Social já cadastrada");
            }

            if (resultado.Inconsistencias.Count == 0)
            {
                _context.Empresas.Add(dadosEmpresa);
                _context.SaveChanges();
            }

            return resultado;
        }

        public Resultado Atualizar(Empresa dadosEmpresa)
        {
            Resultado resultado = DadosValidos(dadosEmpresa);
            resultado.Acao = "Atualização de Empresa";

            if (resultado.Inconsistencias.Count == 0)
            {
                Empresa Empresa = _context.Empresas.Where(
                    p => p.Id == dadosEmpresa.Id).FirstOrDefault();

                if (Empresa == null)
                {
                    resultado.Inconsistencias.Add(
                        "Empresa não encontrado");
                }
                else
                {
                    Empresa.RazaoSocial = dadosEmpresa.RazaoSocial;
                    Empresa.NomeFantasia = dadosEmpresa.NomeFantasia;
                    Empresa.Uf = dadosEmpresa.Uf;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }

        public Resultado Excluir(int id)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Empresa";

            Empresa Empresa = Obter(id);
            if (Empresa == null)
            {
                resultado.Inconsistencias.Add(
                    "Empresa não encontrado");
            }
            else
            {
                _context.Empresas.Remove(Empresa);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos(Empresa Empresa)
        {
            var resultado = new Resultado();
            if (Empresa == null)
            {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados do Empresa");
            }
            else
            {
                if (String.IsNullOrWhiteSpace(Empresa.RazaoSocial))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha a Razão Social");
                }
                if (String.IsNullOrWhiteSpace(Empresa.NomeFantasia))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o Nome Fantasia da Empresa");
                }
                if (String.IsNullOrWhiteSpace(Empresa.Uf))
                {
                    resultado.Inconsistencias.Add(
                        "Preencha o UF da Empresa");
                }
            }

            return resultado;
        }
    }
}